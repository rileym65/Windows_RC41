using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41
{
    partial class Cpu
    {
        public void AddNumber(char n)
        {
            int i;
            int p;
            p = -1;
            Number x;
            ram[PENDING] = (byte)'E';
            if (FlagSet(22) == false)
            {
                for (i = REG_P + 3; i < REG_R; i++) ram[i] = 0xff;
                ram[REG_E + 1] &= 0x0f;
                ram[REG_E + 2] |= 0x0f;
                SetFlag(22);
            }
            if (n < 10)
            {                                       /* digit */
                if (ram[REG_P + 5] == 11)
                {
                    if (ram[REG_P + 4] == 0xff) ram[REG_P + 4] = (byte)n;
                    else if (ram[REG_P + 3] == 0xff) ram[REG_P + 3] = (byte)n;
                }
                else
                {
                    p = 0;
                    while (p < 10 && ram[REG_Q + 6 - p] != 0xff) p++;
                    if (p < 10) ram[REG_Q + 6 - p] = (byte)n;
                }
            }
            if (n == 11)
            {                                      /* EEX */
                if (ram[REG_Q + 6] == 0xff) ram[REG_Q + 6] = 0x01;
                if (ram[REG_P + 5] == 0xff) ram[REG_P + 5] = 11;
            }
            if (n == 12)
            {                                      /* CHS */
                if (ram[REG_P + 5] == 11) ram[REG_E + 1] ^= 0x20;
                else ram[REG_E + 1] ^= 0x10;
            }
            if (n == 10)
            {                                      /* . */
                if ((ram[REG_E + 2] & 0x0f) == 0x0f && ram[REG_P + 5] != 11)
                {
                    p = 0;
                    while (p < 10 && ram[REG_Q + 6 - p] != 0xff) p++;
                    ram[REG_E + 2] &= 0xf0;
                    ram[REG_E + 2] |= (byte)p;
                }
            }

        }

        /*
        public void BackNumber()
        {
            int p;
            int dp;
            if (ram[REG_P + 5] == 11)
            {
                if (ram[REG_P + 3] != 0xff) ram[REG_P + 3] = 0xff;
                else if (ram[REG_P + 4] != 0xff) ram[REG_P + 4] = 0xff;
                else if ((ram[REG_E + 1] & 0x20) != 0) ram[REG_E + 1] ^= 0x20;
                else ram[REG_P + 5] = 0xff;
            }
            else
            {
                p = 0;
                while (ram[REG_Q + 6 - p] != 0xff && p < 10) p++;
                if (p<=10)
                {
                    p--;
                    dp = ram[REG_E + 2] & 0x0f;
                    if (dp != 15 && p < dp)
                    {
                        ram[REG_E + 2] |= 0x0f;
                    }
                    else
                    ram[REG_Q + 6 - p] = 0xff;
                }
                if (ram[REG_Q+6] == 0xff)
                {
                    ClearFlag(22);
                    uiMode = 0;
                }
            }
            window.Display(Display());
        }
        */
    }
}
