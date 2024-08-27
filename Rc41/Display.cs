using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41
{
    partial class Cpu
    {
        public string Display()
        {
            int i;
            int p;
            int e;
            int l;
            Number a;
            string buffer;
            if (FlagSet(52) && !FlagSet(22))
            {
                buffer = ProgramLine();
            }
            else if (FlagSet(50))
            {
                buffer = display;
            }
            else if (FlagSet(48))
            {
                i = REG_P + 2;
                while (ram[i] == 0x00 && i >= REG_M) i--;
                p = 0;
                buffer = "";
                while (i >= REG_M)
                {
                    if (ram[i] == 0x00) buffer += (char)0x00;
                    else buffer += (char)ram[i];
                    i--;
                }
                if (FlagSet(23)) buffer += "_";
            }
            else if (FlagSet(22))
            {
                if (FlagSet(F_PRGM))
                {
                    buffer = $"{((ram[REG_E + 1] & 0x0f) << 8) + ram[REG_E + 0]+1:d2} ";
                }
                else buffer = "";
                if ((ram[REG_E + 1] & 0x10) != 0) buffer += "-";
                l = 9;
                p = 0;
                while (p < 10)
                {
                    if (p == (ram[REG_E + 2] & 0x0f)) { buffer += "."; l = 10; }
                    if (ram[REG_Q + 6 - p] != 0xff &&
                        ram[REG_Q + 6 - p] != 11)
                    {
                        buffer += ((char)(ram[REG_Q + 6 - p] + '0')).ToString();
                        p++;
                    }
                    else
                    {
                        p = 10;
                    }
                }
                if (ram[REG_P + 5] == 11)
                {
                    if (FlagSet(F_PRGM))
                    {
                        buffer += " E";
                    }
                    else
                    {
                        while (buffer.Length < l) buffer += " ";
                    }
                    if ((ram[REG_E + 1] & 0x20) != 0) buffer += "-";
                    if (ram[REG_P + 4] != 0xff) buffer += ((char)('0' + ram[REG_P + 4])).ToString();
                    if (ram[REG_P + 3] != 0xff) buffer += ((char)('0' + ram[REG_P + 3])).ToString();
                }
                if (ram[REG_P+3] == 0xff) buffer += "_";
            }
            else
            {
                a = RecallNumber(R_X);
                if (FlagSet(22))
                {
                    e = (a.exponent[0] * 10) + a.exponent[1];
                    if (a.esign != 0) e = -e;
                    e += ex;
                    a.esign = (byte)((e >= 0) ? 0 : 9);
                    if (e < 0) e = -e;
                    a.exponent[0] = (byte)(e / 10);
                    a.exponent[1] = (byte)(e % 10);
                }
                buffer = Format(a);
            }
            return buffer;
        }
    }
}
