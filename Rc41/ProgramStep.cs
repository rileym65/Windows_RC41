using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41
{
    partial class Cpu
    {
        public void ProgramStep(string line)
        {
            int i;
            int addr;
            int gaddr;
            int adr;
            int byt;
            int reg;
            int start;
            int pend;
            int lineNumber;
            int greg;
            int gbyt;
            int l;
            int d;
            byte b;
            //  if (ram[REG_R+1] == 0x00 && line == NULL) return;
            if (FlagSet(22))
            {
                EndNumber();
            }
            byteCount = 0;
            addr = (ram[REG_B + 1] << 8) | ram[REG_B + 0];
            reg = (addr & 0xfff);
            byt = (addr >> 12) & 0xf;
            adr = (reg * 7) + byt;
            ClearLinks(adr);
            lineNumber = ram[REG_E + 0] + ((ram[REG_E + 1] & 0x0f) << 8);
            if (lineNumber == 0)
            {
                adr--;
            }
            else
            {
                adr--;
                while (ram[adr] == 0) adr--;
                if (ram[adr] < 0xc0 || ram[adr] > 0xcd || ram[adr - 2] >= 0xf0)
                {
                    adr -= isize(adr);
                }
            }
            start = adr + 1;
            pend = FindEnd(adr);
            ram[pend - 2] |= 0x04;
            reg = adr / 7;
            byt = adr % 7;
            addr = (byt << 12) | reg;
            ram[REG_B + 1] = (byte)((addr >> 8) & 0xff);
            ram[REG_B + 0] = (byte)(addr & 0xff);
            if (ram[REG_R + 1] < 0x10 || ram[REG_R + 1] > 0x1c) ClearFlag(22);
            if (ram[REG_R + 1] < 0xf0)
            {
                if (ram[REG_R + 1] < 0x10 || ram[REG_R + 1] > 0x1c)
                {
                    if (ram[REG_R + 1] == 0xc1) ProgramByte(0xc0);
                    else ProgramByte(ram[REG_R + 1]);
                }
            }
            if (ram[REG_R + 1] >= 0xf0)
            {
                if (!FlagSet(F_ALPHA_IN))
                {
                    alphaPos = adr;
                    ProgramByte(0xf1);
                    ProgramByte(ram[REG_R + 0]);
                    SetFlag(F_ALPHA_IN);
                    lineNumber++;
                }
                else
                {
                    ProgramByte(ram[REG_R + 0]);
                    ram[alphaPos]++;
                }
            }
            else if (ram[REG_R + 1] == 0x1d)                          // GTO"
            {
                ProgramByte(0xf0);
                i = REG_Q;
                while (i <= REG_Q + 6 && ram[i] != 0x00)
                {
                    b = (byte)ram[i];
                    if (b != 0)
                    {
                        ProgramByte(b);
                        ram[start - 2]++;
                    }
                    i++;
                }
            }
            else if (ram[REG_R + 1] == 0x1e)                          // XEQ"
            {
                ProgramByte(0xf0);
                i = REG_Q;
                while (i <= REG_Q + 6 && ram[i] != 0x00)
                {
                    b = (byte)ram[i];
                    if (b != 0)
                    {
                        ProgramByte(b);
                        ram[start - 2]++;
                    }
                    i++;
                }
            }
            else if (ram[REG_R + 1] == 0xc0)                          // END
            {
                ProgramByte(0x00);
                ProgramByte(0x0d);
                Link(start - 1);
            }
            else if (ram[REG_R + 1] == 0xc1)                          // LBL
            {
                ProgramByte(0x00);
                ProgramByte(0xf1);
                ProgramByte(0x00);
                i = REG_Q;
                while (i <= REG_Q + 6 && ram[i] != 0x00)
                {
                    b = (byte)ram[i];
                    if (b != 0)
                    {
                        ProgramByte(b);
                        ram[start - 3]++;
                    }
                    i++;
                }
                Link(start - 1);
            }
            else if (ram[REG_R + 1] >= 0x10 && ram[REG_R + 1] <= 0x1c)
            {
                if (!FlagSet(F_SYS))
                {
                    lineNumber++;
                    ProgramByte(0x00);
                }
                SetFlag(F_SYS);
                ProgramByte(ram[REG_R + 1]);
            }
            else if (line.Length != 0)
            {
                if (line[0] == '"')
                {
                    ClearFlag(22);
                    for (i = 0; i < line.Length; i++)
                    {
                        if (line[i] != '"')
                        {
                            if (line[i] > 'e' && line[i] <= 'z') ProgramByte((byte)(line[i] - 32));
                            else if (line[i] == '|') ProgramByte(0x7f);
                            else ProgramByte((byte)line[i]);
                            ram[start - 1]++;
                        }
                    }
                }
            }
            else if (isize(REG_R + 1) > 1)
            {
                ProgramByte(ram[REG_R + 0]);
            }
            if (ram[REG_R + 1] == 0xd0 || ram[REG_R + 1] == 0xe0)
            {
                ProgramByte((byte)(((ram[REG_E + 2] & 0x0f) << 4) | ((ram[REG_E + 1] & 0xf0) >> 4)));
            }
            reg = start / 7;
            byt = start % 7;
            addr = (byt << 12) | reg;
            ram[REG_B + 1] = (byte)((addr >> 8) & 0xff);
            ram[REG_B + 0] = (byte)(addr & 0xff);
            if (!FlagSet(22) && !FlagSet(23) && !FlagSet(F_SYS)) lineNumber++;
            ram[REG_E + 0] = (byte)(lineNumber & 0xff);
            ram[REG_E + 1] &= 0xf0;
            ram[REG_E + 1] |= (byte)((lineNumber >> 8) & 0x0f);
            if (byteCount != 0)
            {
                gaddr = FindNextGlobal(start - 2);
                l = ((ram[gaddr] & 0x0f) << 8) | ram[gaddr - 1];
                if (l != 0)
                {
                    greg = l & 0x1ff;
                    gbyt = (l >> 9) & 0x7;
                    d = (greg * 7) + gbyt;
                    d += byteCount;
                    greg = d / 7;
                    gbyt = d % 7;
                    ram[gaddr] &= 0xf0;
                    ram[gaddr] |= (byte)((gbyt << 1) | (greg >> 8));
                    ram[gaddr - 1] = (byte)(greg & 0xff);
                }
            }
        }
    }
}
