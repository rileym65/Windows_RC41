using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rc41
{
    public class TapeDrive
    {
        Form1 window;
        Cpu cpu;
        Stream tapefile;
        byte[] sector = new byte[256];
        int sectorNumber;
        int sectorPtr;
        bool written;
        int file_rec;
        int file_regs;
        int file_reg;
        int file_pos;
        int file_flags;



        public TapeDrive(Cpu c, Form1 w)
        {
            window = w;
            cpu = c;
        }

        void ReadSector(int s)
        {
            if (s >= 0 && s < 512)
            {
                sectorNumber = s;
                tapefile.Seek(s * 256, SeekOrigin.Begin);
                tapefile.Read(sector);
                sectorPtr = 0;
                written = false;
            }
        }

        void WriteSector(int s)
        {
            if (s >= 0 && s < 512)
            {
                sectorNumber = s;
                tapefile.Seek(s * 256, SeekOrigin.Begin);
                tapefile.Write(sector);
                written = false;
            }
        }
        void Allocate(int l)
        {
            int sec;
            int ofs;
            sec = l / 256;
            ofs = l & 0xff;
            if (written) WriteSector(sectorNumber);
            ReadSector(sec);
            sector[ofs] = 0xff;
            WriteSector(sec);
        }

        void Deallocate(int l)
        {
            int sec;
            int ofs;
            sec = l / 256;
            ofs = l & 0xff;
            if (written) WriteSector(sectorNumber);
            ReadSector(sec);
            sector[ofs] = 0x00;
            WriteSector(sec);
        }

        int FindFile(string filename)
        {
            int s;
            int p;
            int d;
            int i;
            bool flag;
            string fname;
            if (written) WriteSector(sectorNumber);
            d = 0;
            p = 0;
            s = 2;
            flag = true;
            while (flag)
            {
                ReadSector(s);
                while (p < 256)
                {
                    if (sector[p] != 0x00)
                    {
                        fname = "";
                        i = 1;
                        while (sector[p+i] != 0)
                        {
                            fname += (char)sector[p + i];
                            i++;
                        }
                        if (filename.Equals(fname)) return p;
                    }
                    if (sector[p] == 0xff) flag = false;
                    p += 32;
                    d++;
                }
                p = 0;
                s++;
            }
            return -1;
        }

        int FindOpenDir()
        {
            int s;
            int p;
            int d;
            bool flag;
            if (written) WriteSector(sectorNumber);
            d = 0;
            p = 0;
            s = 2;
            flag = true;
            while (flag)
            {
                ReadSector(s);
                while (p < 256)
                {
                    if (sector[p] == 0x00) return d;
                    if (sector[p] == 0xff) flag = false;
                    p += 32;
                    d++;
                }
                p = 0;
                s++;
            }
            return -1;
        }

        int FindOpenSectors(int count)
        {
            int s;
            int p;
            int d;
            int c;
            bool flag;
            if (written) WriteSector(sectorNumber);
            d = 0;
            p = 0;
            s = 0;
            c = 0;
            flag = true;
            while (flag)
            {
                ReadSector(s);
                while (p < 256)
                {
                    if (sector[p] == 0x00) c++;
                    else
                    {
                        d = (s * 256) + p + 1;
                    }
                    if (c >= count) return d;
                    p++;
                }
                p = 0;
                s++;
                if (s == 2) flag = false;
            }
            return -1;
        }

        int CreateFile(string filename, int size, char typ)
        {
            int i;
            int dir;
            int dirsec;
            int dirofs;
            int rec;
            int recs;
            int regs;
            recs = (size + 255) / 256;
            regs = (size + 6) / 7;
            dir = FindOpenDir();
            if (dir < 0) return -1;
            rec = FindOpenSectors(recs);
            if (rec < 0) return -1;
            dirsec = (dir / 8) + 2;
            dirofs = (dir % 8) * 32;
            ReadSector(dirsec);
            sector[dirofs] = (byte)typ;
            for (i = 0; i < 7; i++)
                if (i < filename.Length) sector[dirofs + i + 1] = (byte)filename[i];
                else sector[dirofs + i + 1] = 0;
            sector[dirofs + 8] = 0x00;
            sector[dirofs + 9] = 0x00;
            sector[dirofs + 10] = (byte)((regs >> 8) & 0xff);
            sector[dirofs + 11] = (byte)(regs & 0xff);
            sector[dirofs + 12] = (byte)((size >> 8) & 0xff);
            sector[dirofs + 13] = (byte)(size & 0xff);
            sector[dirofs + 14] = (byte)((rec >> 8) & 0xff);
            sector[dirofs + 15] = (byte)(rec & 0xff);
            sector[dirofs + 16] = (byte)((recs >> 8) & 0xff);
            sector[dirofs + 17] = (byte)(recs & 0xff);
            WriteSector(sectorNumber);
            for (i = 0; i < recs; i++) Allocate(rec + i);
            for (i = 0; i < 256; i++) sector[i] = 0;
            for (i = 0; i < recs; i++)
                WriteSector(rec + i);
            return rec;
        }

        void td_newm(int size)
        {
            int i;
            int e;
            e = (size + 7) / 8;
            for (i = 0; i < 256; i++) sector[i] = 0;
            for (i = 0; i < e + 2; i++) sector[i] = 0xff;
            WriteSector(0);
            for (i = 0; i < 256; i++) sector[i] = 0;
            WriteSector(1);
            for (i = 0; i < e; i++)
            {
                if (i == e - 1) sector[7 * 32] = 0xff;
                WriteSector(i + 2);
            }
            for (i = 0; i < 256; i++) sector[i] = 0;
            for (i = e + 2; i < 512; i++) WriteSector(i);
        }

        void td_dir()
        {
            bool flag;
            int s;
            int r;
            int i;
            int p;
            string line;
            s = 2;
            if (written) WriteSector(sectorNumber);
            window.Print("");
            window.Print("NAME    TYPE    REGS");
            flag = true;
            while (flag)
            {
                ReadSector(s);
                p = 0;
                while (p < 256)
                {
                    if (sector[p] != 0 && sector[p] != 0xff)
                    {
                        line = "";
                        for (i = 0; i < 7; i++)
                            if (sector[p + i + 1] != 0) line += ((char)sector[p + i + 1]).ToString();
                            else line += " ";
                        line += " ";
                        if (sector[p] == 'P') line += "PR";
                        else if (sector[p] == 'D') line += "DA";
                        else if (sector[p] == 'K') line += "KE";
                        else if (sector[p] == 'S') line += "ST";
                        else if (sector[p] == 'A') line += "WA";
                        else if (sector[p] != 0x00)
                        {
                            cpu.Message("NO MEDM");
                            cpu.Error();
                            return;
                        }
                        if (sector[p + 9] != 0)
                        {
                            switch (sector[p + 9])
                            {
                                case 1: line += ",S "; break;
                                case 2: line += ",P "; break;
                                case 3: line += ",PS"; break;
                            }
                        }
                        else line += "   ";
                        line += "   ";
                        r = (sector[p + 10] << 8) | sector[p + 11];
                        line += $"{r:d4}";
                        window.ToPrinter(line);
                    }
                    else if (sector[p] == 0xff)
                    {
                        flag = false;
                    }
                    p += 32;
                }
                if (p >= 256)
                {
                    p = 0;
                    s++;
                    if (s > 511)
                    {
                        flag = false;
                        cpu.Message("NO MEDM");
                        cpu.Error();
                    }
                    else ReadSector(s);
                }
            }
        }

        void td_create()
        {
            string filename;
            int size;
            Number x;
            filename = cpu.GetAlpha();
            if (filename.Length > 7) filename = filename.Substring(0, 7);
            if (filename.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            if (FindFile(filename) >= 0)
            {
                cpu.Message("DUP FL NAME");
                cpu.Error();
                return;
            }
            x = cpu.RecallNumber(Cpu.R_X);
            size = cpu.ToInteger(x);
            CreateFile(filename, size * 7, 'D');
        }

        void td_reada()
        {
            int i;
            int fp;
            int rec;
            int p;
            string filename;
            filename = cpu.GetAlpha();
            if (filename.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            fp = FindFile(filename);
            if (fp >= 0 && sector[fp] != 'A')
            {
                cpu.Message("FL TYPE ERR");
                cpu.Error();
                return;
            }
            if (fp < 0)
            {
                cpu.Message("FL NOT FOUND");
                cpu.Error();
                return;
            }
            rec = (sector[fp + 14] << 8) | sector[fp + 15];
            p = 0;
            ReadSector(rec);
            for (i = 0; i < 16 * 7; i++) cpu.ram[i] = sector[p++];
            for (i = 0x0c0 * 7; i < 0x200 * 7; i++)
            {
                cpu.ram[i] = sector[p++];
                if (p == 256)
                {
                    rec++;
                    ReadSector(rec);
                    p = 0;
                }
            }
        }

        void td_readk()
        {
            int i;
            int address;
            int rec;
            int fp;
            int p;
            int regs;
            int size;
            int end;
            string filename;
            size = 0;
            address = 0x0c0 * 7;
            filename = cpu.GetAlpha();
            if (filename.Length > 7) filename = filename.Substring(0, 7);
            if (filename.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            fp = FindFile(filename);
            if (fp >= 0 && sector[fp] != 'K')
            {
                cpu.Message("FL TYPE ERR");
                cpu.Error();
                return;
            }
            if (fp < 0)
            {
                cpu.Message("FL NOT FOUND");
                cpu.Error();
                return;
            }
            rec = (sector[fp + 14] << 8) | sector[fp + 15];
            regs = (sector[fp + 10] << 8) | sector[fp + 11];
            end = ((cpu.ram[Cpu.REG_C + 1] & 0x0f) << 8) | cpu.ram[Cpu.REG_C + 0];
            if (0x0c0 + regs > end)
            {
                cpu.Message("NO ROOM");
                cpu.Error();
                return;
            }
            address = 0x0c0 * 7;
            while (cpu.ram[address + 6] == 0xf0)
            {
                for (i = 0; i <= 6; i++) cpu.ram[address + i] = 0x00;
                address += 7;
            }
            p = 0;
            address = 0x0c0 * 7;
            ReadSector(rec);
            size = regs * 7;
            while (size > 0)
            {
                cpu.ram[address++] = sector[p++];
                size--;
                if (p == 256)
                {
                    rec++;
                    ReadSector(rec);
                    p = 0;
                }
            }
            cpu.SetKaFlags();
        }

        void td_readp()
        {
            int i;
            int fp;
            int rec;
            int size;
            int p;
            int end;
            int adr;
            int sadr;
            int nabc;
            byte b;
            string filename;
            filename = cpu.GetAlpha();
            if (filename.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            fp = FindFile(filename);
            if (fp >= 0 && sector[fp] != 'P')
            {
                cpu.Message("FL TYPE ERR");
                cpu.Error();
                return;
            }
            if (fp < 0)
            {
                cpu.Message("FL NOT FOUND");
                cpu.Error();
                return;
            }
            rec = (sector[fp + 14] << 8) | sector[fp + 15];
            size = (sector[fp + 12] << 8) | sector[fp + 13];
            end = ((cpu.ram[Cpu.REG_C + 1] & 0x0f) << 8) | cpu.ram[Cpu.REG_C + 0];
            end = (end * 7) + 2;
            adr = cpu.FindStart(end);
            sadr = adr;
            for (i = end + 1; i <= adr; i++) cpu.ram[i] = 0x00;
            p = 0;
            ReadSector(rec);
            while (size > 0)
            {
                nabc = cpu.ToPtr(adr--);
                cpu.ram[Cpu.REG_B + 1] = (byte)((nabc >> 8) & 0xff);
                cpu.ram[Cpu.REG_B + 0] = (byte)(nabc & 0xff);
                cpu.ProgramByte(sector[p++]);
                if (p == 256)
                {
                    rec++;
                    ReadSector(rec);
                    p = 0;
                }
                size--;
            }
            cpu.ReLink();

            end = ((cpu.ram[Cpu.REG_C + 1] & 0x0f) << 8) | cpu.ram[Cpu.REG_C + 0];
            end = (end * 7) + 2;
            while (cpu.ram[sadr] == 0x00) sadr--;
            while (cpu.ram[sadr] < 0xc0 || cpu.ram[sadr] > 0xcd || cpu.ram[sadr - 2] >= 0xf0)
            {
                if (cpu.ram[sadr] >= 0xc0 && cpu.ram[sadr] <= 0xcd && cpu.ram[sadr - 2] >= 0xf0)
                {
                    if (cpu.ram[sadr - 3] != 0x00)
                    {
                        b = cpu.ram[sadr - 3];
                        cpu.UnAsn(b, 3);
                        cpu.ram[sadr - 3] = b;
                        cpu.SetKaFlag(b, true);
                    }
                }
                sadr -= cpu.isize(sadr);
            }
        }

        void td_readr()
        {
            int i;
            int rec;
            int regs;
            int fp;
            int p;
            int adr;
            int addr;
            string filename;
            filename = cpu.GetAlpha();
            if (filename.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            if (filename.Length > 7) filename = filename.Substring(7);
            fp = FindFile(filename);
            if (fp >= 0 && sector[fp] != 'D')
            {
                cpu.Message("FL TYPE ERR");
                cpu.Error();
                return;
            }
            if (fp < 0)
            {
                cpu.Message("FL NOT FOUND");
                cpu.Error();
                return;
            }
            rec = (sector[fp + 14] << 8) | sector[fp + 15];
            regs = (sector[fp + 10] << 8) | sector[fp + 11];
            ReadSector(rec);
            p = 0;
            adr = (cpu.ram[Cpu.REG_C + 2] << 4) | ((cpu.ram[Cpu.REG_C + 1] >> 4) & 0x0f);
            while (adr < 0x200)
            {
                addr = adr * 7;
                if (regs == 0)
                {
                    return;
                }
                for (i = 6; i >= 0; i--)
                {
                    cpu.ram[addr + i] = sector[p++];
                    if (p == 256)
                    {
                        rec++;
                        ReadSector(rec);
                        p = 0;
                    }
                }
                adr++;
                regs--;
            }
        }

        void td_readrx()
        {
            int i;
            int adr;
            int r00;
            int size;
            Number x;
            int b, e;
            if (file_rec < 0)
            {
                cpu.Message("FL TYPE ERR");
                cpu.Error();
                return;
            }
            x = cpu.RecallNumber(Cpu.R_X);
            b = 0;
            e = 0;
            cpu.GetBE(x, ref b, ref e);
            if (e < b) e = b;
            r00 = (cpu.ram[Cpu.REG_C + 2] << 4) | ((cpu.ram[Cpu.REG_C + 1] >> 4) & 0x0f);
            size = 0x200 - r00;
            if (b >= size || e >= size)
            {
                cpu.Message("NONEXISTENT");
                cpu.Error();
                return;
            }
            adr = (b + r00) * 7;
            ReadSector(file_rec);
            while (b <= e)
            {
                if (file_reg >= file_regs)
                {
                    cpu.Message("END OF FILE");
                    cpu.Error();
                    return;
                }
                for (i = 6; i >= 0; i--)
                {
                    cpu.ram[adr + i] = sector[file_pos++];
                    if (file_pos == 256)
                    {
                        file_rec++;
                        file_pos = 0;
                        ReadSector(file_rec);
                    }
                }
                adr += 7;
                b++;
                file_reg++;
            }
        }

        void td_reads()
        {
            int i;
            int rec;
            int fp;
            int r00_a;
            int r00_b;
            string filename;
            filename = cpu.GetAlpha();
            if (filename.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            if (filename.Length > 7) filename = filename.Substring(7);
            fp = FindFile(filename);
            if (fp >= 0 && sector[fp] != 'S')
            {
                cpu.Message("FL TYPE ERR");
                cpu.Error();
                return;
            }
            if (fp < 0)
            {
                cpu.Message("FL NOT FOUND");
                cpu.Error();
                return;
            }
            rec = (sector[fp + 14] << 8) | sector[fp + 15];
            ReadSector(rec);
            r00_a = (cpu.ram[Cpu.REG_C + 2] << 4) | ((cpu.ram[Cpu.REG_C + 1] >> 4) & 0x0f);
            r00_b = (sector[Cpu.REG_C + 2] << 4) | ((sector[Cpu.REG_C + 1] >> 4) & 0x0f);
            for (i = 0; i < Cpu.REG_P + 3; i++) cpu.ram[i] = sector[i];
            for (i = 2; i <= 6; i++) cpu.ram[Cpu.REG_D + i] = sector[Cpu.REG_D + i];
            cpu.ram[Cpu.REG_D + 1] &= 0x0f;
            cpu.ram[Cpu.REG_D + 1] |= (byte)(sector[Cpu.REG_D + 1] & 0xf0);
            cpu.ram[Cpu.REG_C + 6] = sector[Cpu.REG_C + 6];
            cpu.ram[Cpu.REG_C + 5] = (byte)((cpu.ram[Cpu.REG_C + 5] & 0x0f) | (sector[Cpu.REG_C + 5] & 0xf0));
            cpu.Resize(r00_a, r00_b);
        }

        void td_purge()
        {
            int i;
            int fp;
            int rec;
            int recs;
            string filename;
            filename = cpu.GetAlpha();
            if (filename.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            fp = FindFile(filename);
            if (fp < 0)
            {
                cpu.Message("FL NOT FOUND");
                cpu.Error();
                return;
            }
            if ((sector[fp + 9] & 1) != 0)
            {
                cpu.Message("FL SECURED");
                cpu.Error();
                return;
            }
            rec = (sector[fp + 14] << 8) | sector[fp + 15];
            recs = (sector[fp + 16] << 8) | sector[fp + 17];
            for (i = 0; i < 32; i++) sector[fp + i] = 0;
            WriteSector(sectorNumber);
            for (i = 0; i < 256; i++) sector[i] = 0;
            for (i = 0; i < recs; i++) WriteSector(rec + i);
            for (i = 0; i < recs; i++) Deallocate(rec + i);
        }

        void td_rename()
        {
            int i;
            int fp;
            int comma;
            string oldname;
            string newname;
            oldname = cpu.GetAlpha();
            if (oldname.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            comma = oldname.IndexOf(',');
            if (comma < 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            if (comma == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            newname = oldname.Substring(comma+1);
            oldname = oldname.Substring(0, comma - 1);
            if (newname.Length > 7) newname = newname.Substring(0, 7);
            if (oldname.Length > 7) oldname = oldname.Substring(0, 7);
            fp = FindFile(newname);
            if (fp >= 0)
            {
                cpu.Message("DUP FL NAME");
                cpu.Error();
                return;
            }
            fp = FindFile(oldname);
            if (fp < 0)
            {
                cpu.Message("FL NOT FOUND");
                cpu.Error();
                return;
            }
            if ((sector[fp + 9] & 1) != 0)
            {
                cpu.Message("FL SECURED");
                cpu.Error();
                return;
            }
            for (i = 1; i < 8; i++) sector[fp + i] = 0;
            for (i = 0; i < newname.Length; i++)
                sector[fp + 1 + i] = (byte)newname[i];
            WriteSector(sectorNumber);
        }

        void td_sec()
        {
            int fp;
            string filename;
            filename = cpu.GetAlpha();
            if (filename.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            fp = FindFile(filename);
            if (fp < 0)
            {
                cpu.Message("FL NOT FOUND");
                cpu.Error();
                return;
            }
            sector[fp + 9] |= 0x01;
            WriteSector(sectorNumber);
        }

        void td_seekr()
        {
            int fp;
            string filename;
            int size;
            Number x;
            filename = cpu.GetAlpha();
            if (filename.Length > 7) filename = filename.Substring(0, 7);
            if (filename.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            fp = FindFile(filename);
            if (fp < 0)
            {
                cpu.Message("FL NOT FOUND");
                cpu.Error();
                return;
            }
            if (sector[fp] != 'D')
            {
                cpu.Message("FL TYPE ERR");
                cpu.Error();
                return;
            }
            x = cpu.RecallNumber(Cpu.R_X);
            size = cpu.ToInteger(x);
            file_rec = (sector[fp + 14] << 8) | sector[fp + 15];
            file_regs = (sector[fp + 10] << 8) | sector[fp + 11];
            if (size >= file_regs)
            {
                file_rec = -1;
                file_regs = -1;
                file_flags = -1;
                file_reg = -1;
                cpu.Message("END OF FILE");
                cpu.Error();
                return;
            }
            file_flags = sector[fp + 9];
            file_reg = size;
            size *= 7;
            file_rec += (size / 256);
            file_pos = size & 0xff;
        }

        void td_unsec()
        {
            int fp;
            string filename;
            filename = cpu.GetAlpha();
            if (filename.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            fp = FindFile(filename);
            if (fp < 0)
            {
                cpu.Message("FL NOT FOUND");
                cpu.Error();
                return;
            }
            sector[fp + 9] &= 0xfe;
            WriteSector(sectorNumber);
        }

        void td_verify()
        {
            int fp;
            string filename;
            filename = cpu.GetAlpha();
            if (filename.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            fp = FindFile(filename);
            if (fp < 0)
            {
                cpu.Message("FL NOT FOUND");
                cpu.Error();
                return;
            }
        }

        void td_wrta()
        {
            int i;
            int fp;
            int rec;
            int p;
            string filename;
            filename = cpu.GetAlpha();
            if (filename.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            fp = FindFile(filename);
            if (fp >= 0 && sector[fp] != 'A')
            {
                cpu.Message("DUP FL NAME");
                cpu.Error();
                return;
            }
            if (fp >= 0 && (sector[fp + 9] & 1) != 0)
            {
                cpu.Message("FL SECURED");
                cpu.Error();
                return;
            }
            if (fp >= 0)
            {
                rec = (sector[fp + 14] << 8) | sector[fp + 15];
            }
            else
            {
                rec = CreateFile(filename, 2352, 'A');
                if (rec < 0)
                {
                    return;
                }
            }
            p = 0;
            for (i = 0; i < 16 * 7; i++) sector[p++] = cpu.ram[i];
            for (i = 0x0c0 * 7; i < 0x200 * 7; i++)
            {
                sector[p++] = cpu.ram[i];
                if (p == 256)
                {
                    WriteSector(rec);
                    rec++;
                    p = 0;
                }
            }
            if (p != 0) WriteSector(rec);
        }

        void td_wrtk()
        {
            int i;
            int address;
            int rec;
            int recs;
            int fp;
            int p;
            int dirsec;
            int dirofs;
            int size;
            string filename;
            size = 0;
            address = 0x0c0 * 7;
            while (cpu.ram[address + 6] == 0xf0)
            {
                size += 7;
                address += 7;
            }
            if (size == 0)
            {
                cpu.Message("NO KEYS");
                cpu.Error();
                return;
            }
            filename = cpu.GetAlpha();
            if (filename.Length > 7) filename = filename.Substring(0, 7);
            if (filename.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            fp = FindFile(filename);
            if (fp >= 0 && sector[fp] != 'K')
            {
                cpu.Message("DUP FL NAME");
                cpu.Error();
                return;
            }
            if (fp >= 0 && (sector[fp + 9] & 1) != 0)
            {
                cpu.Message("FL SECURED");
                cpu.Error();
                return;
            }
            if (fp > 0)
            {
                dirsec = sectorNumber;
                dirofs = fp;
                rec = (sector[fp + 14] << 8) | sector[fp + 15];
                recs = (sector[fp + 16] << 8) | sector[fp + 17];
                for (i = 0; i < recs; i++) Deallocate(rec + i);
                recs = (size + 255) / 256;
                rec = FindOpenSectors(recs);
                for (i = 0; i < recs; i++) Allocate(rec + i);
                ReadSector(dirsec);
                sector[dirofs + 10] = (byte)((((size + 6) / 7) >> 8) & 0xff);
                sector[dirofs + 11] = (byte)(((size + 6) / 7) & 0xff);
                sector[dirofs + 12] = (byte)((size >> 8) & 0xff);
                sector[dirofs + 13] = (byte)(size & 0xff);
                sector[dirofs + 14] = (byte)((rec >> 8) & 0xff);
                sector[dirofs + 15] = (byte)(rec & 0xff);
                sector[dirofs + 16] = (byte)((recs >> 8) & 0xff);
                sector[dirofs + 17] = (byte)(recs & 0xff);
                WriteSector(dirsec);
            }
            else
            {
                rec = CreateFile(filename, size, 'K');
            }
            p = 0;
            address = 0x0c0 * 7;
            while (size > 0)
            {
                sector[p++] = cpu.ram[address++];
                size--;
                if (p == 256)
                {
                    WriteSector(rec);
                    rec++;
                    p = 0;
                }
            }
            if (p > 0) WriteSector(rec);
        }

        void td_wrtp(int priv)
        {
            int i;
            int address;
            int end;
            int len;
            int rec;
            int recs;
            int fp;
            int p;
            int dirsec;
            int dirofs;
            int comma;
            string filename;
            string progname;
            progname = cpu.GetAlpha();
            if (progname.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            comma = progname.IndexOf(',');
            if (comma >= 0)
            {
                //                if (comma == progname)
                //                {
                //                    printf("Need to code for ',filename'\n");
                //                    return;
                //                }
                //                else
                //                {
                filename = progname.Substring(comma + 1);
                progname = progname.Substring(0, comma);
//                }
            }
            else
            {
                filename = progname;
            }
            if (filename.Length > 7) filename = filename.Substring(0, 7);
            if (filename.Length == 0 || progname.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            address = cpu.FindGlobal(progname);
            if (address == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            address = cpu.FindStart(address);
            end = cpu.FindEnd(address);
            end -= 2;
            len = (address - end) + 1;
            fp = FindFile(filename);
            if (fp >= 0 && sector[fp] != 'P')
            {
                cpu.Message("DUP FL NAME");
                cpu.Error();
                return;
            }
            if (fp >= 0 && (sector[fp + 9] & 1) != 0)
            {
                cpu.Message("FL SECURED");
                cpu.Error();
                return;
            }
            if (fp > 0)
            {
                dirsec = sectorNumber;
                dirofs = fp;
                rec = (sector[fp + 14] << 8) | sector[fp + 15];
                recs = (sector[fp + 16] << 8) | sector[fp + 17];
                for (i = 0; i < recs; i++) Deallocate(rec + i);
                rec = FindOpenSectors(recs);
                for (i = 0; i < recs; i++) Allocate(rec + i);
                ReadSector(dirsec);
                sector[dirofs + 10] = (byte)((((len + 6) / 7) >> 8) & 0xff);
                sector[dirofs + 11] = (byte)(((len + 6) / 7) & 0xff);
                sector[dirofs + 12] = (byte)((len >> 8) & 0xff);
                sector[dirofs + 13] = (byte)(len & 0xff);
                sector[dirofs + 14] = (byte)((rec >> 8) & 0xff);
                sector[dirofs + 15] = (byte)(rec & 0xff);
                sector[dirofs + 16] = (byte)((recs >> 8) & 0xff);
                sector[dirofs + 17] = (byte)(recs & 0xff);
                WriteSector(dirsec);
            }
            else
            {
                rec = CreateFile(filename, len, 'P');
            }
            p = 0;
            while (len > 0)
            {
                sector[p++] = cpu.ram[address--];
                len--;
                if (p == 256)
                {
                    WriteSector(rec);
                    rec++;
                    p = 0;
                }
            }
            if (p > 0) WriteSector(rec);
            fp = FindFile(filename);
            if (fp >= 0)
            {
                if (priv != 0) sector[fp + 9] |= 0x02;
                else sector[fp + 9] &= 0xfd;
                WriteSector(sectorNumber);
            }

        }

        void td_wrtr()
        {
            int i;
            int rec;
            int regs;
            int fp;
            int p;
            int adr;
            int addr;
            string filename;
            filename = cpu.GetAlpha();
            if (filename.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            if (filename.Length > 7) filename = filename.Substring(0, 7);
            fp = FindFile(filename);
            if (fp >= 0 && sector[fp] != 'D')
            {
                cpu.Message("DUP FL NAME");
                cpu.Error();
                return;
            }
            if (fp >= 0 && (sector[fp + 9] & 1) != 0)
            {
                cpu.Message("FL SECURED");
                cpu.Error();
                return;
            }
            if (fp < 0)
            {
                cpu.Message("FL NOT FOUND");
                cpu.Error();
                return;
            }
            rec = (sector[fp + 14] << 8) | sector[fp + 15];
            regs = (sector[fp + 10] << 8) | sector[fp + 11];
            ReadSector(rec);
            p = 0;
            adr = (cpu.ram[Cpu.REG_C + 2] << 4) | ((cpu.ram[Cpu.REG_C + 1] >> 4) & 0x0f);
            while (adr < 0x200)
            {
                addr = adr * 7;
                if (regs == 0)
                {
                    if (p != 0) WriteSector(rec);
                    cpu.Message("END OF FILE");
                    cpu.Error();
                    return;
                }
                for (i = 6; i >= 0; i--)
                {
                    sector[p++] = cpu.ram[addr + i];
                    if (p == 256)
                    {
                        WriteSector(rec);
                        rec++;
                        p = 0;
                    }
                }
                adr++;
                regs--;
            }
            if (p != 0) WriteSector(rec);
        }

        void td_wrtrx()
        {
            int i;
            int adr;
            int r00;
            int size;
            Number x;
            int b, e;
            if (file_rec < 0)
            {
                cpu.Message("FL TYPE ERR");
                cpu.Error();
                return;
            }
            if ((file_flags & 1) != 0)
            {
                cpu.Message("FL SECURED");
                cpu.Error();
                return;
            }
            x = cpu.RecallNumber(Cpu.R_X);
            b = 0;
            e = 0;
            cpu.GetBE(x, ref b, ref e);
            if (e < b) e = b;
            r00 = (cpu.ram[Cpu.REG_C + 2] << 4) | ((cpu.ram[Cpu.REG_C + 1] >> 4) & 0x0f);
            size = 0x200 - r00;
            if (b >= size || e >= size)
            {
                cpu.Message("NONEXISTENT");
                cpu.Error();
                return;
            }
            adr = (b + r00) * 7;
            ReadSector(file_rec);
            while (b <= e)
            {
                if (file_reg >= file_regs)
                {
                    if (file_pos != 0) WriteSector(file_rec);
                    cpu.Message("END OF FILE");
                    cpu.Error();
                    return;
                }
                for (i = 6; i >= 0; i--)
                {
                    sector[file_pos++] = cpu.ram[adr + i];
                    if (file_pos == 256)
                    {
                        WriteSector(file_rec);
                        file_rec++;
                        file_pos = 0;
                        ReadSector(file_rec);
                    }
                }
                adr += 7;
                b++;
                file_reg++;
            }
            if (file_pos != 0) WriteSector(file_rec);
        }

        void td_wrts()
        {
            int i;
            int fp;
            int rec;
            string filename;
            filename = cpu.GetAlpha();
            if (filename.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            fp = FindFile(filename);
            if (fp >= 0 && sector[fp] != 'S')
            {
                cpu.Message("DUP FL NAME");
                cpu.Error();
                return;
            }
            if (fp >= 0 && (sector[fp + 9] & 1) != 0)
            {
                cpu.Message("FL SECURED");
                cpu.Error();
                return;
            }
            if (fp >= 0)
            {
                rec = (sector[fp + 14] << 8) | sector[fp + 15];
                ReadSector(rec);
                for (i = 0; i < 112; i++) sector[i] = cpu.ram[i];
                WriteSector(rec);
            }
            else
            {
                rec = CreateFile(filename, 112, 'S');
                if (rec > 0)
                {
                    for (i = 0; i < 112; i++) sector[i] = cpu.ram[i];
                    WriteSector(rec);
                }
            }
        }

        void td_zero()
        {
            int i;
            int fp;
            int rec;
            int recs;
            string filename;
            filename = cpu.GetAlpha();
            if (filename.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            fp = FindFile(filename);
            if (fp < 0)
            {
                cpu.Message("FL NOT FOUND");
                cpu.Error();
                return;
            }
            if ((sector[fp + 9] & 1) != 0)
            {
                cpu.Message("FL SECURED");
                cpu.Error();
                return;
            }
            rec = (sector[fp + 14] << 8) | sector[fp + 15];
            recs = (sector[fp + 16] << 8) | sector[fp + 17];
            for (i = 0; i < 256; i++) sector[i] = 0;
            for (i = 0; i < recs; i++) WriteSector(rec + i);
        }

        public void Command(byte function, int addr)
        {
            if (function == 1)
            {                           /* CREATE */
                td_create();
            }

            if (function == 2)
            {                           /* DIR */
                td_dir();
            }

            if (function == 3)
            {                           /* NEWM */
                if (addr < 1 || addr > 447)
                {
                    cpu.Message("DATA ERR");
                    cpu.Error();
                    return;
                }
                td_newm(addr + 1);
            }

            if (function == 4)
            {                           /* PURGE */
                td_purge();
            }

            if (function == 5)
            {                           /* READA */
                td_reada();
            }

            if (function == 6)
            {                           /* READK */
                td_readk();
            }

            if (function == 7)
            {                           /* READP */
                td_readp();
            }

            if (function == 8)
            {                           /* READR */
                td_readr();
            }
            if (function == 9)
            {                           /* READRX */
                td_readrx();
            }

            if (function == 10)
            {                          /* READS */
                td_reads();
            }

            if (function == 11)
            {                          /* READSUB */
                cpu.GtoEnd();
                td_readp();
            }

            if (function == 12)
            {                          /* RENAME */
                td_rename();
            }

            if (function == 13)
            {                          /* SEC */
                td_sec();
            }

            if (function == 14)
            {                          /* SEEKR */
                td_seekr();
            }

            if (function == 15)
            {                          /* UNSEC */
                td_unsec();
            }

            if (function == 16)
            {                          /* VERIFY */
                td_verify();
            }

            if (function == 17)
            {                          /* WRTA */
                td_wrta();
            }

            if (function == 18)
            {                          /* WRTK */
                td_wrtk();
            }

            if (function == 19)
            {                          /* WRTP */
                td_wrtp(0);
            }

            if (function == 20)
            {                          /* WRTPV */
                td_wrtp(1);
            }

            if (function == 21)
            {                          /* WRTR */
                td_wrtr();
            }

            if (function == 22)
            {                          /* WRTRX */
                td_wrtrx();
            }

            if (function == 23)
            {                          /* WRTS */
                td_wrts();
            }

            if (function == 24)
            {                          /* ZERO */
                td_zero();
            }

        }

        public void OpenTapeDrive(string filename)
        {
            if (tapefile != null) tapefile.Close();
            tapefile = File.Open(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            if (tapefile == null)
            {
                MessageBox.Show($"Could not open tape drive: {filename}");
                return;
            }
            ReadSector(0);
            file_rec = -1;
            file_regs = -1;
            file_pos = -1;
        }

    }
}

