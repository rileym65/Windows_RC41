using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rc41
{
    public class Printer
    {
        int printPosition;
        string printBuffer;
        Cpu cpu;
        Form1 window;

        public Printer(Cpu c, Form1 window)
        {
            cpu = c;
            this.window = window;
            printPosition = 0;
            printBuffer = "";
        }

        public void Print(string line)
        {
            char c;
            string printLine;
            printLine = "";
            for (int i=0; i<line.Length; i++)
            {
                c = line[i];
                if (c == 0x7a) c = '"';
                if (c == 0x7f) c = '|';
                if (c == 0x00) c = '_';
                if (c < 32 || c > 126) c = '#';
                printLine += c;
            }
            window.Print(printLine);
            if (window.PrintToFile())
            {
                using (StreamWriter file = new StreamWriter("printer.txt", true))
                {
                    file.WriteLine(printLine);
                }
            }
        }

        void PrintToBuffer(byte b)
        {
            printBuffer += ((char)b).ToString();
            printPosition++;
            if (printBuffer.Length >= 24)
            {
                Print(printBuffer);
                printBuffer = "";
                printPosition = 0;
            }
        }

        public void PrintBuffer()
        {
            string buffer;
            if (printBuffer.Length != 0)
            {
                buffer = "";
                while (buffer.Length + printBuffer.Length < 24) buffer += " ";
                buffer += printBuffer;
                Print(buffer);
                printBuffer = "";
                printPosition = 0;
            }
        }

        string DecodeInstruction(byte b1, byte b2)
        {
            int i;
            string line = "";
            if (b1 == 0x04 && b2 < 0x10)
            {
                switch (b2)
                {
                    case 0x00: return "CAT";
                    case 0x01: return "@c";
                    case 0x02: return "DEL";
                    case 0x03: return "COPY";
                    case 0x04: return "CLP";
                    case 0x05: return "R/S";
                    case 0x06: return "SIZE";
                    case 0x07: return "BST";
                    case 0x08: return "SST";
                    case 0x09: return "ON";
                    case 0x0a: return "PACK";
                    case 0x0b: return "<--";
                    case 0x0c: return "ALPHA";
                    case 0x0d: return "2__";
                    case 0x0e: return "SHIFT";
                    case 0x0f: return "ASN";
                    default: return "";
                }
            }
            if (b1 >= 0xa0 && b1 <= 0xa7)
            {
                i = 0;
                while (cpu.reverse2[i].cmd != 0xff &&
                       (cpu.reverse2[i].cmd != b1 ||
                        cpu.reverse2[i].post != b2)) i++;
                if (cpu.reverse2[i].cmd == 0xff) line += $"XROM {(b1&0x0f) << 2 | ((b2&0xc0) >> 6):d2},{b2 & 0x3f:d2}";
                else line += cpu.reverse2[i].name;
                return line;
            }
            if (b1 < 0x10)
            {
                i = 0;
                while (cpu.reverse[i].cmd != 0xff &&
                       cpu.reverse[i].cmd != b2) i++;
                if (cpu.reverse[i].cmd == 0xff)  line += $"XROM {(b1 & 0x0f) << 2 | ((b2 & 0xc0) >> 6):d2},{b2 & 0x3f:d2}";
                else line += cpu.reverse[i].name;
                return line;
            }
            i = 0;
            while (cpu.reverse[i].cmd != 0xff &&
                   cpu.reverse[i].cmd != b1) i++;
            if (cpu.reverse[i].cmd == 0xff)
            {
                line += $"XROM {(b1 & 0x0f) << 2 | ((b2 & 0xc0) >> 6):d2},{b2 & 0x3f:d2}";
                return line;
            }
            line += cpu.reverse[i].name + " ";
            if (b2 >= 0x80) line += "IND ";
            b2 &= 0x7f;
            if (b2 < 0x70) line += b2.ToString("d02");
            else
                switch (b2)
                {
                    case 0x70: line += "T"; break;
                    case 0x71: line += "Z"; break;
                    case 0x72: line += "Y"; break;
                    case 0x73: line += "X"; break;
                    case 0x74: line += "L"; break;
                    case 0x75: line += "M"; break;
                    case 0x76: line += "N"; break;
                    case 0x77: line += "O"; break;
                    case 0x78: line += "P"; break;
                    case 0x79: line += "Q"; break;
                    case 0x7a: line += "|-"; break;
                    case 0x7b: line += "a"; break;
                    case 0x7c: line += "b"; break;
                    case 0x7d: line += "c"; break;
                    case 0x7e: line += "d"; break;
                    case 0x7f: line += "e"; break;
                }
            return line;
        }

        public void Prp(string name)
        {
            int lineNumber;
            int address;
            int end;
            string line;
            address = cpu.FindGlobal(name);
            if (address == 0) return;
            address = cpu.FindStart(address);
            end = cpu.FindEnd(address);
            lineNumber = 1;
            Print("");
            while (cpu.ram[address] == 0x00) address--;
            while (address >= end)
            {
                line = cpu.ProgramList(lineNumber, address);
                Print(line);
                lineNumber++;
                address -= cpu.isize(address);
                while (cpu.ram[address] == 0x00) address--;
            }
        }

        public void Command(byte function)
        {
            int addr;
            //  int    end;
            int i;
            int m;
            int n;
            int p;
            int f;
            int s;
            int e;
            int c;
            int reg;
            int r00;
            int[] r = new int[6];
            Number x;
            string buffer;
            if (cpu.FlagSet(55) == false)
            {
                cpu.Message("NONEXISTENT");
                cpu.Error();
                return;
            }
            if (cpu.FlagSet(21) == false) return;

            if (function == 1)
            {                           // ACA
                n = 0;
                m = Cpu.REG_P + 2;
                buffer = "";
                while (m >= Cpu.REG_M)
                {
                    if (cpu.ram[m] == 0 && n != 0) buffer += "_";
                    else if (cpu.ram[m] == 0) ;
                    else if (cpu.ram[m] < ' ' || cpu.ram[m] > 0x7e) PrintToBuffer((byte)'#');
                    else if (cpu.ram[m] >= 'f' && cpu.ram[m] <= 'z' && cpu.FlagSet(13) == false)
                        PrintToBuffer((byte)(cpu.ram[m] - 32));
                    else if (cpu.ram[m] >= 'F' && cpu.ram[m] <= 'Z' && cpu.FlagSet(13) != false)
                        PrintToBuffer((byte)(cpu.ram[m] + 32));
                    else PrintToBuffer(cpu.ram[m]);
                    if (cpu.ram[m] != 0) n = -1;
                    m--;
                }
            }

            else if (function == 2)
            {                      // ACCHR
                x = cpu.RecallNumber(Cpu.R_X);
                if (x.esign >= 0)
                {
                    m = x.mantissa[0];
                    for (n = 0; n < ((x.exponent[0] * 10) + x.exponent[1]); n++)
                    {
                        if (n + 1 < 10) m = (m * 10) + x.mantissa[n + 1];
                        else m *= 10;
                    }
                    if (m > 127)
                    {
                        cpu.Message("NONEXISTENT");
                        cpu.Error();
                        return;
                    }
                }
                else m = 0;
                PrintToBuffer((byte)m);
            }

            else if (function == 3)
            {                      // ACCOL
                x = cpu.RecallNumber(Cpu.R_X);
                if (x.esign >= 0)
                {
                    m = x.mantissa[0];
                    for (n = 0; n < ((x.exponent[0] * 10) + x.exponent[1]); n++)
                    {
                        if (n + 1 < 10) m = (m * 10) + x.mantissa[n + 1];
                        else m *= 10;
                    }
                    if (m > 127)
                    {
                        cpu.Message("DATA ERROR");
                        cpu.Error();
                        return;
                    }
                }
                else m = 0;
                PrintToBuffer((byte)(m | 0x80));
            }

            else if (function == 4)
            {                      // ACSPEC
                r[6] = ((cpu.ram[Cpu.REG_X + 6] & 0x01) << 6) | ((cpu.ram[Cpu.REG_X + 5] & 0xfc) >> 2);
                r[5] = ((cpu.ram[Cpu.REG_X + 5] & 0x03) << 5) | ((cpu.ram[Cpu.REG_X + 4] & 0xf8) >> 3);
                r[4] = ((cpu.ram[Cpu.REG_X + 4] & 0x07) << 4) | ((cpu.ram[Cpu.REG_X + 3] & 0xf0) >> 4);
                r[3] = ((cpu.ram[Cpu.REG_X + 3] & 0x0f) << 3) | ((cpu.ram[Cpu.REG_X + 2] & 0xe0) >> 5);
                r[2] = ((cpu.ram[Cpu.REG_X + 2] & 0x1f) << 2) | ((cpu.ram[Cpu.REG_X + 1] & 0xc0) >> 6);
                r[1] = ((cpu.ram[Cpu.REG_X + 1] & 0x3f) << 1) | ((cpu.ram[Cpu.REG_X + 0] & 0x80) >> 7);
                r[0] = cpu.ram[Cpu.REG_X + 0] & 0x7f;
                for (i = 6; i >= 0; i--)
                    PrintToBuffer((byte)(r[i] | 0x80));
            }

            else if (function == 5)
            {                      // ACX
                x = cpu.RecallNumber(Cpu.R_X);
                buffer = cpu.Format(x);
                for (n = 0; n < buffer.Length; n++)
                {
                    PrintToBuffer((byte)(buffer[n]));
                }
            }

            else if (function == 6)
            {                      // BLDSPEC
                cpu.ram[Cpu.LIFT] = (byte)'D';
                cpu.ram[Cpu.PENDING] = (byte)'E';
                x = cpu.RecallNumber(Cpu.R_X);
                if (x.esign >= 0)
                {
                    p = x.mantissa[0];
                    for (n = 0; n < ((x.exponent[0] * 10) + x.exponent[1]); n++)
                    {
                        if (n + 1 < 10) p = (p * 10) + x.mantissa[n + 1];
                        else p *= 10;
                    }
                    if (p > 127)
                    {
                        cpu.Message("DATA ERROR");
                        cpu.Error();
                        return;
                    }
                }
                else
                {
                    cpu.Message("DATA ERROR");
                    cpu.Error();
                    return;
                }
                if ((cpu.ram[Cpu.REG_Y + 6] & 0xf0) != 0x10)
                {
                    for (i = Cpu.REG_Y; i < Cpu.REG_Y + 6; i++) cpu.ram[i] = 0x00;
                    cpu.ram[Cpu.REG_Y + 6] = 0x10;
                }
                r[5] = ((cpu.ram[Cpu.REG_Y + 5] & 0x03) << 5) | ((cpu.ram[Cpu.REG_Y + 4] & 0xf8) >> 3);
                r[4] = ((cpu.ram[Cpu.REG_Y + 4] & 0x07) << 4) | ((cpu.ram[Cpu.REG_Y + 3] & 0xf0) >> 4);
                r[3] = ((cpu.ram[Cpu.REG_Y + 3] & 0x0f) << 3) | ((cpu.ram[Cpu.REG_Y + 2] & 0xe0) >> 5);
                r[2] = ((cpu.ram[Cpu.REG_Y + 2] & 0x1f) << 2) | ((cpu.ram[Cpu.REG_Y + 1] & 0xc0) >> 6);
                r[1] = ((cpu.ram[Cpu.REG_Y + 1] & 0x3f) << 1) | ((cpu.ram[Cpu.REG_Y + 0] & 0x80) >> 7);
                r[0] = cpu.ram[Cpu.REG_Y + 0] & 0x7f;
                cpu.ram[Cpu.REG_Y + 6] = (byte)(0x10 | ((r[5] & 0x40) >> 6));
                cpu.ram[Cpu.REG_Y + 5] = (byte)(((r[5] & 0x3f) << 2) | ((r[4] >> 5) & 0x03));
                cpu.ram[Cpu.REG_Y + 4] = (byte)(((r[4] & 0x1f) << 3) | ((r[3] >> 4) & 0x07));
                cpu.ram[Cpu.REG_Y + 3] = (byte)(((r[3] & 0x0f) << 4) | ((r[2] >> 3) & 0x0f));
                cpu.ram[Cpu.REG_Y + 2] = (byte)(((r[2] & 0x07) << 5) | ((r[1] >> 2) & 0x1f));
                cpu.ram[Cpu.REG_Y + 1] = (byte)(((r[1] & 0x03) << 6) | ((r[0] >> 1) & 0x3f));
                cpu.ram[Cpu.REG_Y + 0] = (byte)(((r[0] & 0x01) << 7) | (p & 0x7f));
                x = cpu.RecallNumber(Cpu.R_Y); cpu.StoreNumber(x, Cpu.R_X);
                x = cpu.RecallNumber(Cpu.R_Z); cpu.StoreNumber(x, Cpu.R_Y);
                x = cpu.RecallNumber(Cpu.R_T); cpu.StoreNumber(x, Cpu.R_Z);
            }

            else if (function == 8)
            {                      // PRA
                if (printBuffer.Length != 0)
                {
                    PrintBuffer();
                }
                n = 0;
                p = 0;
                m = Cpu.REG_P + 2;
                buffer = "";
                while (m >= Cpu.REG_M)
                {
                    if (cpu.ram[m] == 0 && n != 0) buffer += "_";
                    else if (cpu.ram[m] == 0) ;
                    else if (cpu.ram[m] < ' ' || cpu.ram[m] > 0x7e) buffer += "#";
                    else buffer += ((char)cpu.ram[m]).ToString();
                    if (cpu.ram[m] != 0) n = -1;
                    m--;
                }
                Print(buffer);
            }

            else if (function == 10)
            {                     // PRBUF
                PrintBuffer();
            }

            else if (function == 11)
            {                     // PRFLAGS
                Print("");
                if (printBuffer.Length != 0) PrintBuffer();
                Print("STATUS:");
                reg = (cpu.ram[Cpu.REG_C + 2] << 4) + ((cpu.ram[Cpu.REG_C + 1] >> 4) & 0x0f);
                Print($"SIZE= {Cpu.RAMTOP - reg:d03}");
                reg = (cpu.ram[Cpu.REG_C + 6] << 4) | ((cpu.ram[Cpu.REG_C + 5] >> 4) & 0x0f);
                Print($"E= {reg:d03}");
                if (cpu.FlagSet(42) == false && cpu.FlagSet(43) == false) Print("DEG");
                if (cpu.FlagSet(42) != false && cpu.FlagSet(43) == false) Print("GRAD");
                if (cpu.FlagSet(42) == false && cpu.FlagSet(43) != false) Print("RAD");
                n = 0;
                if (cpu.FlagSet(36)) n |= 8;
                if (cpu.FlagSet(37)) n |= 4;
                if (cpu.FlagSet(38)) n |= 2;
                if (cpu.FlagSet(39)) n |= 1;
                if (cpu.FlagSet(40) == false && cpu.FlagSet(41) == false) Print($"SCI {n}");
                if (cpu.FlagSet(40) != false && cpu.FlagSet(41) == false) Print($"FIX {n}");
                if (cpu.FlagSet(40) == false && cpu.FlagSet(41) != false) Print($"ENG {n}");
                Print("");
                Print("FLAGS:");
                m = Cpu.REG_D + 6;
                f = 0;
                while (m >= Cpu.REG_D)
                {
                    p = cpu.ram[m--];
                    for (n = 0; n < 8; n++)
                    {
                        if ((p & 0x80) != 0) Print($"F {f:d02}  SET");
                        else Print($"F {f:d02}  CLEAR");
                        p <<= 1;
                        f++;
                    }
                }
            }

            else if (function == 12)
            {                          // PRKEYS
                Print("");
                addr = 0x0c0 * 7;
                c = 0;
                while (cpu.ram[addr + 6] == 0xf0)
                {
                    if (cpu.ram[addr + 0] != 0)
                    {
                        if (c == 0) Print("USER KEYS:");
                        c++;
                        buffer = "";
                        if ((cpu.ram[addr + 0] & 0x0f) >= 0x01 && (cpu.ram[addr + 0] & 0x0f) <= 0x08)
                            buffer += $" {cpu.ram[addr + 0]:x02}: ";
                        else
                            buffer += $"-{cpu.ram[addr + 0] & 0xf7:x02}: ";
                        buffer += DecodeInstruction(cpu.ram[addr + 2], cpu.ram[addr + 1]);
                        Print(buffer);
                    }
                    if (cpu.ram[addr + 3] != 0)
                    {
                        if (c == 0) Print("USER KEYS:");
                        c++;
                        buffer = "";
                        if ((cpu.ram[addr + 3] & 0x0f) >= 0x01 && (cpu.ram[addr + 3] & 0x0f) <= 0x08)
                            buffer += $" {cpu.ram[addr + 3]:x02}: ";
                        else
                            buffer += $"-{cpu.ram[addr + 3] & 0xf7:x02}: ";
                        buffer += DecodeInstruction(cpu.ram[addr + 5], cpu.ram[addr + 4]);
                        Print(buffer);
                    }
                    addr += 7;
                }
                addr = (cpu.ram[Cpu.REG_C + 2] << 4) | ((cpu.ram[Cpu.REG_C + 1] & 0xf0) >> 4);
                addr = (addr * 7) - 1;
                while (cpu.ram[addr] < 0xc0 || cpu.ram[addr] >= 0xce || (cpu.ram[addr - 2] & 0xf0) != 0x20)
                {
                    if (cpu.ram[addr] >= 0xc0 && cpu.ram[addr] < 0xce)
                    {
                        if (cpu.ram[addr - 2] >= 0xf0)
                        {
                            if (cpu.ram[addr - 3] != 0x00)
                            {
                                if (c == 0) Print("USER KEYS:");
                                c++;
                                buffer = "";
                                if ((cpu.ram[addr - 3] & 0x0f) <= 0x08) buffer += $" {cpu.ram[addr - 3]:x02}: \"";
                                else buffer += $"-{cpu.ram[addr - 3] & 0xf7:x02}: \"";
                                for (i = 1; i < (cpu.ram[addr - 2] & 0x0f); i++)
                                    if (cpu.ram[addr - 3 - i] == 0) buffer += "_";
                                    else buffer += ((char)cpu.ram[addr - 3 - i]).ToString();
                                Print(buffer);
                            }
                        }
                    }
                    addr -= cpu.isize(addr);
                }
                if (c == 0) Print("USER KEYS:NONE");
            }

            else if (function == 16)
            {                          // PRREG
                if (printBuffer.Length != 0) PrintBuffer();
                n = 0;
                reg = (cpu.ram[Cpu.REG_C + 2] << 4) | ((cpu.ram[Cpu.REG_C + 1] >> 4) & 0x0f);
                while (reg < Cpu.RAMTOP)
                {
                    x = cpu.RecallNumber(reg);
                    buffer = cpu.Format(x);
                    Print($"R{n:d02}= {buffer}");
                    n++;
                    reg++;
                }
            }

            else if (function == 17)
            {                          // PRREGX
                if (printBuffer.Length != 0) PrintBuffer();
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
                reg = (cpu.ram[Cpu.REG_C + 2] << 4) | ((cpu.ram[Cpu.REG_C + 1] >> 4) & 0x0f);
                n = s;
                e += reg;
                reg += s;
                if (reg > 0x1ff)
                {
                    cpu.Message("NONEXISTENT");
                    cpu.Error();
                }
                else
                {
                    while (reg <= e)
                    {
                        if (reg > 0xfff)
                        {
                            cpu.Message("NONEXISTENT");
                            cpu.Error();
                            reg = e + 1;
                        }
                        else
                        {
                            x = cpu.RecallNumber(reg);
                            buffer = cpu.Format(x);
                            Print($"R{n:d02}= {buffer}");
                            n++;
                            reg++;
                        }
                    }
                }
            }

            else if (function == 18)
            {                          // PRE
                if (printBuffer.Length != 0) PrintBuffer();
                reg = (cpu.ram[Cpu.REG_C + 6] << 4) | ((cpu.ram[Cpu.REG_C + 5] >> 4) & 0x0f);
                r00 = ((cpu.ram[Cpu.REG_C + 2] << 4) | ((cpu.ram[Cpu.REG_C + 1] >> 4) & 0x0f));
                x = cpu.RecallNumber(reg + r00);
                buffer = cpu.Format(x);
                Print($"EX  = {buffer}");
                x = cpu.RecallNumber(reg + r00 + 1);
                buffer = cpu.Format(x);
                Print($"EX^2= {buffer}");
                x = cpu.RecallNumber(reg + r00 + 2);
                buffer = cpu.Format(x);
                Print($"EY  = {buffer}");
                x = cpu.RecallNumber(reg + r00 + 3);
                buffer = cpu.Format(x);
                Print($"EY^2= {buffer}");
                x = cpu.RecallNumber(reg + r00 + 4);
                buffer = cpu.Format(x);
                Print($"EXY = {buffer}");
                x = cpu.RecallNumber(reg + r00 + 5);
                buffer = cpu.Format(x);
                Print($"N   = {buffer}");
            }

            else if (function == 19)
            {                          // PRSTK
                Print("");
                if (printBuffer.Length != 0) PrintBuffer();
                x = cpu.RecallNumber(Cpu.R_T);
                buffer = cpu.Format(x);
                Print($"T= {buffer}");
                x = cpu.RecallNumber(Cpu.R_Z);
                buffer = cpu.Format(x);
                Print($"Z= {buffer}");
                x = cpu.RecallNumber(Cpu.R_Y);
                buffer = cpu.Format(x);
                Print($"Y= {buffer}");
                x = cpu.RecallNumber(Cpu.R_X);
                buffer = cpu.Format(x);
                Print($"X= {buffer}");
            }

            else if (function == 20)
            {                          // PRX
                if (printBuffer.Length != 0) PrintBuffer();
                x = cpu.RecallNumber(Cpu.R_X);
                buffer = cpu.Format(x);
                while (buffer.Length < 24) buffer = " " + buffer;
                Print(buffer);
            }

            else if (function == 21)      // REGPLOT
            {

            }

            else if (function == 22)
            {                          // SKPCHR
                x = cpu.RecallNumber(Cpu.R_X);
                if (x.esign >= 0)
                {
                    m = x.mantissa[0];
                    for (n = 0; n < ((x.exponent[0] * 10) + x.exponent[1]); n++)
                    {
                        if (n + 1 < 10) m = (m * 10) + x.mantissa[n + 1];
                        else m *= 10;
                    }
                    if (m > 127)
                    {
                        cpu.Message("NONEXISTENT");
                        cpu.Error();
                        return;
                    }
                }
                else m = 0;
                for (i = 0; i < m; i++)
                    PrintToBuffer((byte)' ');
            }

            else if (function == 23)
            {                          // SKPCOL
                x = cpu.RecallNumber(Cpu.R_X);
                if (x.esign >= 0)
                {
                    m = x.mantissa[0];
                    for (n = 0; n < ((x.exponent[0] * 10) + x.exponent[1]); n++)
                    {
                        if (n + 1 < 10) m = (m * 10) + x.mantissa[n + 1];
                        else m *= 10;
                    }
                    if (m > 127)
                    {
                        cpu.Message("NONEXISTENT");
                        cpu.Error();
                        return;
                    }
                }
                else m = 0;
                for (i = 0; i < m; i++)
                    PrintToBuffer(0x80);
            }
            else
            {
                cpu.Message("NONEXISTENT");
                cpu.Error();
            }

        }
    }
}
