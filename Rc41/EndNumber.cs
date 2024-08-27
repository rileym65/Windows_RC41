using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41
{
    partial class Cpu
    {
        public void EndNumber()
        {
            int i;
            int p;
            int e;
            int d;
            byte b1;
            byte b2;
            byte dp;
            string buffer;
            Number nm;
            if (!FlagSet(22)) return;
            ClearFlag(22);

            if (FlagSet(F_PRGM))
            {
                dp = (byte)(ram[REG_E + 2] & 0x0f);
                b1 = ram[REG_R + 1];
                b2 = ram[REG_R + 0];
                ClearFlag(F_SYS);
                p = REG_Q + 6;
                i = 0;
                while (p > REG_P+2 && ram[p] < 10)
                {
                    if (dp != 0x0f && i >= dp)
                    {
                        ram[REG_R + 1] = 0x1a;
                        ProgramStep("");
                        dp = 0x0f;
                    }
                    i++;
                    ram[REG_R + 1] = (byte)(0x10 + ram[p]);
                    ProgramStep("");
                    p--;
                }
                if ((ram[REG_E+1] & 0x10) != 0)
                {
                    ram[REG_R + 1] = 0x1c;
                    ProgramStep("");
                }
                if (ram[REG_P + 5] == 11)
                {
                    ram[REG_R + 1] = 0x1b;
                    ProgramStep("");
                    if (ram[REG_P+4] != 0xff)
                    {
                        ram[REG_R + 1] = (byte)(0x10+ram[REG_P+4]);
                        ProgramStep("");
                    }
                    if (ram[REG_P + 3] != 0xff)
                    {
                        ram[REG_R + 1] = (byte)(0x10 + ram[REG_P + 3]);
                        ProgramStep("");
                    }
                    if ((ram[REG_E + 1] & 0x20) != 0)
                    {
                        ram[REG_R + 1] = 0x1c;
                        ProgramStep("");
                    }
                }
                ClearFlag(F_SYS);
                ram[REG_R + 1] = b1;
                ram[REG_R + 0] = b2;
                return;
            }

            nm = new Number();
            nm.sign = (byte)(((ram[REG_E + 1] & 0x10) != 0) ? 9 : 0);
            i = REG_Q + 6;
            p = 0;
            e = -1;
            d = ram[REG_E + 2] & 0x0f;
            while (p < 10 && ram[i] < 10)
            {
                nm.mantissa[p++] = ram[i--];
                if (p <= d) e++;
            }
            while (p < 10) nm.mantissa[p++] = 0x00;
            if (!IsZero(nm))
            {
                while (nm.mantissa[0] == 0x00)
                {
                    for (i = 0; i < 9; i++) nm.mantissa[i] = nm.mantissa[i + 1];
                    nm.mantissa[9] = 0;
                    e--;
                }
            }
            if (ram[REG_P + 5] == 0x0b)
            {
                if ((ram[REG_E + 1] & 0x20) != 0)
                {
                    if (ram[REG_P + 3] == 0xff) e -= ram[REG_P + 4];
                    else e -= ((ram[REG_P + 4] * 10) + ram[REG_P + 3]);
                }
                else
                {
                    if (ram[REG_P + 3] == 0xff) e += ram[REG_P + 4];
                    else e += ((ram[REG_P + 4] * 10) + ram[REG_P + 3]);
                }
            }
            nm.esign = 0;
            if (e < 0)
            {
                nm.esign = 9;
                e = -e;
            }
            nm.exponent[0] = (byte)(e / 10);
            nm.exponent[1] = (byte)(e % 10);
            StoreNumber(nm, R_X);
            ram[LIFT] = (byte)'E';
            if (window.PrinterMode() != 'M' && window.PrinterOn())
            {
                buffer = Format(nm);
                while (buffer.Length < 24) buffer = " " + buffer;
                printer.Print(buffer);
            }
        }
    }
}
