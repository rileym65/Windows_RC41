using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Rc41
{
    public class CardReader
    {
        Form1 window;
        Cpu cpu;

        public CardReader(Cpu c, Form1 w)
        {
            this.window = w;
            cpu = c;
        }


        public void Rall(string filename)
        {
            Stream file;
            int len;
            byte[] card = new byte[5];
            file = File.Open(filename, FileMode.Open, FileAccess.ReadWrite);
            if (file == null)
            {
                cpu.Message("CARD ERR");
                MessageBox.Show($"Could not open card file: {filename}");
                return;
            }
            len = Cpu.RAMTOP;
            len *= 7;
            file.Read(card, 0, 5);
            file.Read(cpu.ram, 5, len);
            file.Close();
        }

        public void Rprg(string filename)
        {
            Stream file;
            int address;
            int sadr;
            int nabc;
            int len;
            //  int regs;
            byte b;
            byte[] buffer = new byte[1];
            byte[] card = new byte[7];
            file = File.Open(filename, FileMode.Open, FileAccess.ReadWrite);
            if (file == null)
            {
                cpu.Message("CARD ERR");
                cpu.Error();
                MessageBox.Show($"Could not open card file: {filename}");
                return;
            }
            file.Read(card, 0, 7);
            len = (card[5] << 8) | card[6];
            //    regs = (len + 6) / 7;
            cpu.GtoEnd();
            address = ((cpu.ram[Cpu.REG_C + 1] & 0x0f) << 8) | cpu.ram[Cpu.REG_C + 0];
            address *= 7;
            address += 2;
            sadr = address;
            while (len > 0)
            {
                nabc = cpu.ToPtr(address);
                cpu.ram[Cpu.REG_B + 1] = (byte)((nabc >> 8) & 0xff);
                cpu.ram[Cpu.REG_B + 0] = (byte)(nabc & 0xff);
                file.Read(buffer);
                cpu.ProgramByte(buffer[0]);
                address--;
                len--;
            }
            file.Close();
            cpu.ReLink();

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

            cpu.Pack();

        }

        public void Rsts(string filename) {
            int i;
            int j;
            Stream file;
            int adr;
            int regs;
            int old_r00;
            int new_r00;
            byte[] card = new byte[5];
            byte[] buffer = new byte[7];
            file = File.Open(filename, FileMode.Open, FileAccess.ReadWrite);
            if (file == null)
            {
                MessageBox.Show($"Could not open card file: {filename}");
                return;
            }
            file.Read(card, 0, 5);
            regs = card[3] * 256 + card[4];
            adr = 0;
            for (i = 0; i < 9; i++) {
                file.Read(buffer, 0, 7);
                for (j = 0; j < 7; j++) cpu.ram[adr + 6 - j] = buffer[j];
                adr += 7;
            }
            file.Read(buffer, 0, 7);

            cpu.ram[Cpu.REG_C + 6] = buffer[0];
            cpu.ram[Cpu.REG_C + 5] &= 0x0f;
            cpu.ram[Cpu.REG_C + 5] |= (byte)(buffer[1] & 0xf0);
            new_r00 = (buffer[4] << 4) | ((buffer[5] & 0xf0) >> 4);
            file.Read(buffer, 0, 7);
            for (i = 0; i < 6; i++) cpu.ram[Cpu.REG_D + 6 - i] = buffer[i];
            cpu.ram[Cpu.REG_D + 1] &= 0x0f;
            cpu.ram[Cpu.REG_D + 1] |= (byte)(buffer[5] & 0xf0);
            adr = 0x0c0 * 7;
            while (cpu.ram[adr + 6] == 0xf0) {
                for (i = 0; i <= 6; i++) cpu.ram[adr + i] = 0;
                adr += 6;
            }
            adr = 0x0c0 * 7;
            regs -= 11;
            while (regs > 0) {
                file.Read(buffer, 0, 7);
                for (i = 0; i <= 6; i++) cpu.ram[adr + 6 - i] = buffer[i];
                adr += 7;
                regs--;
            }
            file.Close();
            old_r00 = (cpu.ram[Cpu.REG_C + 2] << 4) | ((cpu.ram[Cpu.REG_C + 1] >> 4) & 0x0f);
            cpu.Resize(old_r00, new_r00);
            cpu.SetKaFlags();
        }

 
        public int Command(byte function, int addr)
        {
            
            int s;
            int e;
            int n;
            int i;
            int flag;
            int r00;
            int len;
            int p;
            int address;
            byte[] card = new byte[5];
            byte[] cardBuffer = new byte[4096];
            Number a;
            Number b;
            Number x;
            string filename;
            Stream file;

            if (function == 2)
            {                           // RDTA
                filename = window.LoadCard();
                if (filename == null) return addr;
                r00 = (cpu.ram[Cpu.REG_C + 2] << 4) | ((cpu.ram[Cpu.REG_C + 1] >> 4) & 0x0f);

                file = File.Open(filename, FileMode.Open, FileAccess.ReadWrite);
                if (file == null)
                {
                    MessageBox.Show($"Could not open card file: {filename}");
                    return addr;
                }

                len = Cpu.RAMTOP - r00;
                r00 *= 7;
                file.Read(card, 0, 5);

                if (card[0] != 'D')
                {
                    cpu.Message("CARD ERR");
                }
                else
                {
                    file.Read(cardBuffer, 5, len);
                    for (i = 0; i < len; i++)
                        cpu.ram[r00 + i] = cardBuffer[i];
                }
                file.Close();
            }

            if (function == 3)
            {                           // RDTAX
                x = cpu.RecallNumber(Cpu.R_X);
                s = 0;
                e = 0;
                p = 0;
                if (x.esign == 0)
                {
                    s = x.mantissa[p++];
                    n = x.exponent[0] * 10 + x.exponent[1];
                    while (n > 0)
                    {
                        s *= 10;
                        if (p < 10) s += x.mantissa[p++];
                        n--;
                    }
                }
                else
                {
                    n = x.exponent[0] * 10 + x.exponent[1];
                    while (n > 0)
                    {
                        n--;
                        p--;
                    }
                    p++;
                }
                for (i = 0; i < 3; i++)
                {
                    e *= 10;
                    if (p >= 0 && p < 10) e += x.mantissa[p];
                    p++;
                }
                r00 = (cpu.ram[Cpu.REG_C + 2] << 4) | ((cpu.ram[Cpu.REG_C + 1] >> 4) & 0x0f);
                len = 1 + e - s;
                s += r00;
                e += r00;
                s *= 7;
                e *= 7;
                len *= 7;
                filename = window.LoadCard();
                if (filename == null) return addr;
                file = File.Open(filename, FileMode.Open, FileAccess.ReadWrite);
                if (file == null)
                {
                    MessageBox.Show($"Could not open card file: {filename}");
                    return addr;
                }
                file.Read(card, 0, 5);
                if (card[0] != 'D')
                {
                    cpu.Message("CARD ERR");
                }
                else
                {
                    file.Read(cardBuffer, 5, len);
                    for (i = 0; i < len; i++)
                        cpu.ram[s + i] = cardBuffer[i];
                }
                file.Close();
            }

            if (function == 5)
            {                           // VER
                filename = window.LoadCard();
                if (filename == null) return addr;
                file = File.Open(filename, FileMode.Open, FileAccess.ReadWrite);
                if (file == null)
                {
                    MessageBox.Show($"Could not open card file: {filename}");
                    return addr;
                }
                file.Read(card, 0, 5);
                cpu.Message($"TYPE {(char)card[0]} TR 01");
                file.Close();
            }


            if (function == 6)
            {                           // WALL
                filename = window.SaveCard();
                if (filename == null) return addr;
                file = File.Open(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                if (file == null)
                {
                    MessageBox.Show($"Could not open card file: {filename}");
                    return addr;
                }
                r00 = (cpu.ram[Cpu.REG_C + 2] << 4) | ((cpu.ram[Cpu.REG_C + 1] >> 4) & 0x0f);
                len = Cpu.RAMTOP;
                r00 *= 7;
                card[0] = (byte)'A';
                card[1] = 0x00;
                card[2] = 0x01;
                i = ((len + 15) / 16);
                card[3] = (byte)((i >> 8) & 0xff);
                card[4] = (byte)(i & 0xff);
                len *= 7;
                file.Write(card, 0, 5);
                file.Write(cpu.ram, 5, len);
                file.Close();
            }

            if (function == 7)
            {                           // WDTA
                filename = window.SaveCard();
                if (filename == null) return addr;
                file = File.Open(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                if (file == null)
                {
                    MessageBox.Show($"Could not open card file: {filename}");
                    return addr;
                }
                r00 = (cpu.ram[Cpu.REG_C + 2] << 4) | ((cpu.ram[Cpu.REG_C + 1] >> 4) & 0x0f);
                len = Cpu.RAMTOP - r00;
                r00 *= 7;
                card[0] = (byte)'D';
                card[1] = 0x00;
                card[2] = 0x01;
                i = ((len + 15) / 16);
                card[3] = (byte)((i >> 8) & 0xff);
                card[4] = (byte)(i & 0xff);
                len *= 7;
                file.Write(card, 0, 5);
                for (i = 0; i < len; i++)
                    cardBuffer[i] = cpu.ram[r00 + i];
                file.Write(cardBuffer, 0, len);
                file.Close();
            }

            if (function == 8)
            {                           // WDTAX
                x = cpu.RecallNumber(Cpu.R_X);
                s = 0;
                e = 0;
                p = 0;
                if (x.esign == 0)
                {
                    s = x.mantissa[p++];
                    n = x.exponent[0] * 10 + x.exponent[1];
                    while (n > 0)
                    {
                        s *= 10;
                        if (p < 10) s += x.mantissa[p++];
                        n--;
                    }
                }
                else
                {
                    n = x.exponent[0] * 10 + x.exponent[1];
                    while (n > 0)
                    {
                        n--;
                        p--;
                    }
                    p++;
                }
                for (i = 0; i < 3; i++)
                {
                    e *= 10;
                    if (p >= 0 && p < 10) e += x.mantissa[p];
                    p++;
                }
                r00 = (cpu.ram[Cpu.REG_C + 2] << 4) | ((cpu.ram[Cpu.REG_C + 1] >> 4) & 0x0f);
                len = 1 + e - s;
                s += r00;
                e += r00;
                s *= 7;
                e *= 7;
                filename = window.SaveCard();
                if (filename == null) return addr;
                file = File.Open(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                if (file == null)
                {
                    MessageBox.Show($"Could not open card file: {filename}");
                    return addr;
                }
                card[0] = (byte)'D';
                card[1] = 0x00;
                card[2] = 0x01;
                i = ((len + 15) / 16);
                card[3] = (byte)((i >> 8) & 0xff);
                card[4] = (byte)(i & 0xff);
                len *= 7;
                file.Write(card, 0, 5);
                for (i = 0; i < len; i++)
                    cardBuffer[i] = cpu.ram[s + i];
                file.Write(cardBuffer, 5, len);
                file.Close();
            }

            if (function == 10)
            {                          // WSTS
                filename = window.SaveCard();
                if (filename == null) return addr;
                file = File.Open(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                if (file == null)
                {
                    MessageBox.Show($"Could not open card file: {filename}");
                    return addr;
                }
                len = 11;
                address = 0x0c0 * 7;
                while (cpu.ram[address + 6] == 0xf0)
                {
                    address += 7;
                    len++;
                }
                card[0] = (byte)'S';
                card[1] = 0x00;
                card[2] = 0x01;
                card[3] = (byte)((len >> 8) & 0xff);
                card[4] = (byte)(len & 0xff);
                file.Write(card, 0, 5);
                address = 0;
                for (i = 0; i < 9; i++)
                {
                    for (s = 0; s <= 6; s++)
                        cardBuffer[6 - s] = cpu.ram[address + s];
                    file.Write(cardBuffer, 0, 7);
                    address += 7;
                }
                address = Cpu.REG_C;
                for (i = 0; i < 2; i++)
                {
                    for (s = 0; s <= 6; s++)
                        cardBuffer[6 - s] = cpu.ram[address + s];
                    file.Write(cardBuffer, 0, 7);
                    address += 7;
                }
                len -= 11;
                address = 0x0c0 * 7;
                address = 0x0c0 * 7;
                while (len > 0)
                {
                    for (s = 0; s <= 6; s++)
                        cardBuffer[6 - s] = cpu.ram[address + s];
                    file.Write(cardBuffer, 0, 7);
                    address += 7;
                    len--;
                }
                file.Close();
            }

            if (function == 11)
            {                          // 7CLREG
                for (i = 0; i < 10; i++) cpu.Sto(cpu.ZERO, i);
                for (i = 20; i <= 25; i++) cpu.Sto(cpu.ZERO, i);
            }

            if (function == 12)
            {                          // 7DSP0
                cpu.ClearFlag(36);
                cpu.ClearFlag(37);
                cpu.ClearFlag(38);
                cpu.ClearFlag(39);
            }

            if (function == 13)
            {                          // 7DSP1
                cpu.ClearFlag(36);
                cpu.ClearFlag(37);
                cpu.ClearFlag(38);
                cpu.SetFlag(39);
            }


            if (function == 14)
            {                          // 7DSP2
                cpu.ClearFlag(36);
                cpu.ClearFlag(37);
                cpu.SetFlag(38);
                cpu.ClearFlag(39);
            }

            if (function == 15)
            {                          // 7DSP3
                cpu.ClearFlag(36);
                cpu.ClearFlag(37);
                cpu.SetFlag(38);
                cpu.SetFlag(39);
            }

            if (function == 16)
            {                          // 7DSP4
                cpu.ClearFlag(36);
                cpu.SetFlag(37);
                cpu.ClearFlag(38);
                cpu.ClearFlag(39);
            }

            if (function == 17)
            {                          // 7DSP5
                cpu.ClearFlag(36);
                cpu.SetFlag(37);
                cpu.ClearFlag(38);
                cpu.SetFlag(39);
            }

            if (function == 18)
            {                          // 7DSP6
                cpu.ClearFlag(36);
                cpu.SetFlag(37);
                cpu.SetFlag(38);
                cpu.ClearFlag(39);
            }

            if (function == 19)
            {                          // 7DSP7
                cpu.ClearFlag(36);
                cpu.SetFlag(37);
                cpu.SetFlag(38);
                cpu.SetFlag(39);
            }

            if (function == 20)
            {                          // 7DSP8
                cpu.SetFlag(36);
                cpu.ClearFlag(37);
                cpu.ClearFlag(38);
                cpu.ClearFlag(39);
            }

            if (function == 21)
            {                          // 7DSP9
                cpu.SetFlag(36);
                cpu.ClearFlag(37);
                cpu.ClearFlag(38);
                cpu.SetFlag(39);
            }

            if (function == 22)
            {                          // 7DSPI
                a = cpu.Rcl(25);
                i = cpu.ToInteger(a);
                if (i < 0 || i > 9 || a.sign != 0)
                {
                    cpu.Message("DATA ERROR");
                    cpu.Error();
                }
                else
                {
                    if ((i & 8) != 0) cpu.SetFlag(36); else cpu.ClearFlag(36);
                    if ((i & 4) != 0) cpu.SetFlag(37); else cpu.ClearFlag(37);
                    if ((i & 2) != 0) cpu.SetFlag(38); else cpu.ClearFlag(38);
                    if ((i & 1) != 0) cpu.SetFlag(39); else cpu.ClearFlag(39);
                }
            }

            if (function == 23)
            {                          // 7DSZ
                a = cpu.Rcl(25);
                a = cpu.Sub(a, cpu.S_ONE);
                cpu.Sto(a, 25);
                flag = 0;
                for (i = 0; i < 10; i++)
                    if (a.mantissa[i] != 0) flag = -1;
                if (flag == 0 && addr >= 0x0c0) addr = cpu.Skip(addr);
            }

            if (function == 24)
            {                          // 7DSZI
                a = cpu.Rcl(25);
                i = cpu.ToInteger(a);
                if (i < 0 || i > 25 || a.sign != 0)
                {
                    cpu.Message("DATA ERROR");
                    cpu.Error();
                }
                else
                {
                    a = cpu.Rcl(i);
                    a = cpu.Sub(a, cpu.S_ONE);
                    cpu.Sto(a, i);
                    flag = 0;
                    for (i = 0; i < 10; i++)
                        if (a.mantissa[i] != 0) flag = -1;
                    if (flag == 0 && addr >= 0x0c0) addr = cpu.Skip(addr);
                }
            }

            if (function == 25)
            {                          // 7ENG
                cpu.ClearFlag(40);
                cpu.SetFlag(41);
            }

            if (function == 26)
            {                          // 7FIX
                cpu.SetFlag(40);
                cpu.ClearFlag(41);
            }

            if (function == 29)
            {                          // 7ISZ
                a = cpu.Rcl(25);
                a = cpu.Add(a, cpu.S_ONE);
                cpu.Sto(a, 25);
                flag = 0;
                for (i = 0; i < 10; i++)
                    if (a.mantissa[i] != 0) flag = -1;
                if (flag == 0) addr = cpu.Skip(addr);
            }

            if (function == 30)
            {                          // 7ISZI
                a = cpu.Rcl(25);
                i = cpu.ToInteger(a);
                if (i < 0 || i > 25 || a.sign != 0)
                {
                    cpu.Message("DATA ERROR");
                    cpu.Error();
                }
                else
                {
                    a = cpu.Rcl(i);
                    a = cpu.Add(a, cpu.S_ONE);
                    cpu.Sto(a, i);
                    flag = 0;
                    for (i = 0; i < 10; i++)
                        if (a.mantissa[i] != 0) flag = -1;
                    if (flag == 0 && addr >= 0x0c0) addr = cpu.Skip(addr);
                }
            }

            if (function == 31)
            {                          // 7P<>S
                for (i = 0; i < 10; i++)
                {
                    a = cpu.Rcl(i);
                    b = cpu.Rcl(i + 10);
                    cpu.Sto(a, i + 10);
                    cpu.Sto(b, i);
                }
            }

            if (function == 36)
            {                          // 7SCI
                cpu.ClearFlag(40);
                cpu.ClearFlag(41);
            }
            
            return addr;
        }

        public void Wprg(string filename)
        {
            int i;
            Stream file;
            int address;
            int end;
            int len;
            byte[] card = new byte[7];
            byte[] buffer = new byte[2];
            address = (cpu.ram[Cpu.REG_B + 1] << 8) | cpu.ram[Cpu.REG_B + 0];
            address = cpu.FromPtr(address);
            address = cpu.FindStart(address);
            end = cpu.FindEnd(address);
            file = File.Open(filename, FileMode.OpenOrCreate, FileAccess.Write);

            if (file != null)
            {
                end -= 2;
                len = (address - end) / 7;
                card[0] = (byte)'P';
                card[1] = 0x00;
                card[2] = 0x01;
                i = ((len + 15) / 16);
                card[3] = (byte)((i >> 8) & 0xff);
                card[4] = (byte)(i & 0xff);
                len = (address - end) + 1;
                card[5] = (byte)((len >> 8) & 0xff);
                card[6] = (byte)(len & 0xff);
                file.Write(card, 0, 7);
                while (len > 0)
                {
                    buffer[0] = cpu.ram[address];
                    file.Write(buffer, 0, 1);
                    address--;
                    len--;
                }
                file.Close();
            }
            else
            {
                cpu.Message("CARD ERR");
                cpu.Error();
            }
        }


    }
}
