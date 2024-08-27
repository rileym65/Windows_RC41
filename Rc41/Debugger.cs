using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rc41
{
    public class Debugger
    {
        Form1 window;
        Cpu cpu;

        public Debugger(Form1 w, Cpu c)
        {
            window = w;
            cpu = c;
        }

        public void ShowDregs(int from, int to)
        {
            int i;
            int j;
            int b;
            string line;
            window.DebugPrint("");
            b = (cpu.ram[Cpu.REG_C + 2] << 4) + ((cpu.ram[Cpu.REG_C + 1] >> 4) & 0x0f);
            from += b;
            to += b;
            if (from > 0x1ff) from = 0x1ff;
            if (to > 0x1ff) to = 0x1ff;
            if (to > from)
            {
                i = from; from = to; to = i;
            }
            for (i = from; i >= to; i--)
            {
                line = $"{i - b:d3}:";
                for (j = 6; j >= 0; j--)
                    line += $" {cpu.ram[i * 7 + j]:X2}";
                window.DebugPrint(line);
            }
        }

        public void ShowFlags()
        {
            string line;
            window.DebugPrint("");
            line = "";
            if ((cpu.ram[Cpu.REG_D + 6] & 0x80) != 0) line += "00:* "; else line += "00:- ";
            if ((cpu.ram[Cpu.REG_D + 6] & 0x40) != 0) line += "01:* "; else line += "01:- ";
            if ((cpu.ram[Cpu.REG_D + 6] & 0x20) != 0) line += "02:* "; else line += "02:- ";
            if ((cpu.ram[Cpu.REG_D + 6] & 0x10) != 0) line += "03:* "; else line += "03:- ";
            if ((cpu.ram[Cpu.REG_D + 6] & 0x08) != 0) line += "04:* "; else line += "04:- ";
            if ((cpu.ram[Cpu.REG_D + 6] & 0x04) != 0) line += "05:* "; else line += "05:- ";
            if ((cpu.ram[Cpu.REG_D + 6] & 0x02) != 0) line += "06:* "; else line += "06:- ";
            if ((cpu.ram[Cpu.REG_D + 6] & 0x01) != 0) line += "07:* "; else line += "07:- ";
            if ((cpu.ram[Cpu.REG_D + 5] & 0x80) != 0) line += "08:* "; else line += "08:- ";
            if ((cpu.ram[Cpu.REG_D + 5] & 0x40) != 0) line += "09:* "; else line += "09:- ";
            if ((cpu.ram[Cpu.REG_D + 5] & 0x20) != 0) line += "10:* "; else line += "10:- ";
            if ((cpu.ram[Cpu.REG_D + 5] & 0x10) != 0) line += "11:* "; else line += "11:- ";
            window.DebugPrint(line);
            line = "";
            if ((cpu.ram[Cpu.REG_D + 5] & 0x08) != 0) line += "12:* "; else line += "12:- ";
            if ((cpu.ram[Cpu.REG_D + 5] & 0x04) != 0) line += "13:* "; else line += "13:- ";
            if ((cpu.ram[Cpu.REG_D + 5] & 0x02) != 0) line += "14:* "; else line += "14:- ";
            if ((cpu.ram[Cpu.REG_D + 5] & 0x01) != 0) line += "15:* "; else line += "15:- ";
            if ((cpu.ram[Cpu.REG_D + 4] & 0x80) != 0) line += "16:* "; else line += "16:- ";
            if ((cpu.ram[Cpu.REG_D + 4] & 0x40) != 0) line += "17:* "; else line += "17:- ";
            if ((cpu.ram[Cpu.REG_D + 4] & 0x20) != 0) line += "18:* "; else line += "18:- ";
            if ((cpu.ram[Cpu.REG_D + 4] & 0x10) != 0) line += "19:* "; else line += "19:- ";
            if ((cpu.ram[Cpu.REG_D + 4] & 0x08) != 0) line += "20:* "; else line += "20:- ";
            if ((cpu.ram[Cpu.REG_D + 4] & 0x04) != 0) line += "21:* "; else line += "21:- ";
            if ((cpu.ram[Cpu.REG_D + 4] & 0x02) != 0) line += "22:* "; else line += "22:- ";
            if ((cpu.ram[Cpu.REG_D + 4] & 0x01) != 0) line += "23:* "; else line += "23:- ";
            window.DebugPrint(line);
            line = "";
            if ((cpu.ram[Cpu.REG_D + 3] & 0x80) != 0) line += "24:* "; else line += "24:- ";
            if ((cpu.ram[Cpu.REG_D + 3] & 0x40) != 0) line += "25:* "; else line += "25:- ";
            if ((cpu.ram[Cpu.REG_D + 3] & 0x20) != 0) line += "26:* "; else line += "26:- ";
            if ((cpu.ram[Cpu.REG_D + 3] & 0x10) != 0) line += "27:* "; else line += "27:- ";
            if ((cpu.ram[Cpu.REG_D + 3] & 0x08) != 0) line += "28:* "; else line += "28:- ";
            if ((cpu.ram[Cpu.REG_D + 3] & 0x04) != 0) line += "29:* "; else line += "29:- ";
            if ((cpu.ram[Cpu.REG_D + 3] & 0x02) != 0) line += "30:* "; else line += "30:- ";
            if ((cpu.ram[Cpu.REG_D + 3] & 0x01) != 0) line += "31:* "; else line += "31:- ";
            if ((cpu.ram[Cpu.REG_D + 2] & 0x80) != 0) line += "32:* "; else line += "32:- ";
            if ((cpu.ram[Cpu.REG_D + 2] & 0x40) != 0) line += "33:* "; else line += "33:- ";
            if ((cpu.ram[Cpu.REG_D + 2] & 0x20) != 0) line += "34:* "; else line += "34:- ";
            if ((cpu.ram[Cpu.REG_D + 2] & 0x10) != 0) line += "35:* "; else line += "35:- ";
            window.DebugPrint(line);
            line = "";
            if ((cpu.ram[Cpu.REG_D + 2] & 0x08) != 0) line += "36:* "; else line += "36:- ";
            if ((cpu.ram[Cpu.REG_D + 2] & 0x04) != 0) line += "37:* "; else line += "37:- ";
            if ((cpu.ram[Cpu.REG_D + 2] & 0x02) != 0) line += "38:* "; else line += "38:- ";
            if ((cpu.ram[Cpu.REG_D + 2] & 0x01) != 0) line += "39:* "; else line += "39:- ";
            if ((cpu.ram[Cpu.REG_D + 1] & 0x80) != 0) line += "40:* "; else line += "40:- ";
            if ((cpu.ram[Cpu.REG_D + 1] & 0x40) != 0) line += "41:* "; else line += "41:- ";
            if ((cpu.ram[Cpu.REG_D + 1] & 0x20) != 0) line += "42:* "; else line += "42:- ";
            if ((cpu.ram[Cpu.REG_D + 1] & 0x10) != 0) line += "43:* "; else line += "43:- ";
            if ((cpu.ram[Cpu.REG_D + 1] & 0x08) != 0) line += "44:* "; else line += "44:- ";
            if ((cpu.ram[Cpu.REG_D + 1] & 0x04) != 0) line += "45:* "; else line += "45:- ";
            if ((cpu.ram[Cpu.REG_D + 1] & 0x02) != 0) line += "46:* "; else line += "46:- ";
            if ((cpu.ram[Cpu.REG_D + 1] & 0x01) != 0) line += "47:* "; else line += "47:- ";
            window.DebugPrint(line);
            line = "";
            if ((cpu.ram[Cpu.REG_D + 0] & 0x80) != 0) line += "48:* "; else line += "48:- ";
            if ((cpu.ram[Cpu.REG_D + 0] & 0x40) != 0) line += "49:* "; else line += "49:- ";
            if ((cpu.ram[Cpu.REG_D + 0] & 0x20) != 0) line += "50:* "; else line += "50:- ";
            if ((cpu.ram[Cpu.REG_D + 0] & 0x10) != 0) line += "51:* "; else line += "51:- ";
            if ((cpu.ram[Cpu.REG_D + 0] & 0x08) != 0) line += "52:* "; else line += "52:- ";
            if ((cpu.ram[Cpu.REG_D + 0] & 0x04) != 0) line += "53:* "; else line += "53:- ";
            if ((cpu.ram[Cpu.REG_D + 0] & 0x02) != 0) line += "54:* "; else line += "54:- ";
            if ((cpu.ram[Cpu.REG_D + 0] & 0x01) != 0) line += "55:* "; else line += "55:- ";
            window.DebugPrint(line);
        }

        public void ShowInfo()
        {
            int value;
            string line;
            window.DebugPrint("");
            value = (cpu.ram[Cpu.REG_C + 6] << 4) | ((cpu.ram[Cpu.REG_C + 5] >> 4) & 0x0f);
            line = $"Stat     : {value:d4}  (${value:X4})";
            window.DebugPrint(line);
            value = (cpu.ram[Cpu.REG_C + 2] << 4) | ((cpu.ram[Cpu.REG_C + 1] >> 4) & 0x0f);
            line = $"R00      : {value:d4}  (${value:X4})";
            window.DebugPrint(line);
            value = ((cpu.ram[Cpu.REG_C + 1] & 0x0f) << 8) | cpu.ram[Cpu.REG_C + 0];
            line = $".END.    : {value:d4}  (${value:X4})";
            window.DebugPrint(line);
            value = ((cpu.ram[Cpu.REG_E + 1] & 0x0f) << 8) | cpu.ram[Cpu.REG_E + 0];
            line = $"Line #   : {value:d4}  (${value:X4})";
            window.DebugPrint(line);
            value = ((cpu.ram[Cpu.REG_B + 1] & 0x0f) << 8) | cpu.ram[Cpu.REG_B + 0];
            value *= 7;
            value += ((cpu.ram[Cpu.REG_B + 1] & 0xf0) >> 4);
            line = $"Addr     : {value:d4}  (${cpu.ram[Cpu.REG_B + 1]:X2}{cpu.ram[Cpu.REG_B + 0]:X2})";
            window.DebugPrint(line);
        }

        public void ShowRegs(int from, int to)
        {
            int i;
            int j;
            string line;
            window.DebugPrint("");
            if (from > 0x1ff) from = 0x1ff;
            if (to > 0x1ff) to = 0x1ff;
            if (to > from)
            {
                i = from; from = to; to = i;
            }
            for (i = from; i >= to; i--)
            {
                line = $"{i:X3}:";
                for (j = 6; j >= 0; j--)
                    line += $" {cpu.ram[i * 7 + j]:X2}";
                line += "    ";
                for (j=6; j>=0; j--)
                {
                    if (cpu.ram[i * 7 + j] > 32 && cpu.ram[i * 7 + j] < 128) line += $"{(char)cpu.ram[i * 7 + j]}";
                      else line += ".";
                }
                window.DebugPrint(line);
            }
        }

        public void ShowStatRegs(bool stackOnly)
        {
            int i;
            string line;

            window.DebugPrint("");

            if (!stackOnly)
            {
                line = " e: ";
                for (i = 6; i >= 0; i--)
                    line += $"{cpu.ram[Cpu.REG_E + i]:X2} ";
                window.DebugPrint(line);

                line = " d: ";
                for (i = 6; i >= 0; i--)
                    line += $"{cpu.ram[Cpu.REG_D + i]:X2} ";
                window.DebugPrint(line);

                line = " c: ";
                for (i = 6; i >= 0; i--)
                    line += $"{cpu.ram[Cpu.REG_C + i]:X2} ";
                window.DebugPrint(line);

                line = " b: ";
                for (i = 6; i >= 0; i--)
                    line += $"{cpu.ram[Cpu.REG_B + i]:X2} ";
                window.DebugPrint(line);

                line = " a: ";
                for (i = 6; i >= 0; i--)
                    line += $"{cpu.ram[Cpu.REG_A + i]:X2} ";
                window.DebugPrint(line);

                line = "|-: ";
                for (i = 6; i >= 0; i--)
                    line += $"{cpu.ram[Cpu.REG_R + i]:X2} ";
                window.DebugPrint(line);

                line = " Q: ";
                for (i = 6; i >= 0; i--)
                    line += $"{cpu.ram[Cpu.REG_Q + i]:X2} ";
                window.DebugPrint(line);

                line = " P: ";
                for (i = 6; i >= 0; i--)
                    line += $"{cpu.ram[Cpu.REG_P + i]:X2} ";
                window.DebugPrint(line);

                line = " O: ";
                for (i = 6; i >= 0; i--)
                    line += $"{cpu.ram[Cpu.REG_O + i]:X2} ";
                window.DebugPrint(line);

                line = " N: ";
                for (i = 6; i >= 0; i--)
                    line += $"{cpu.ram[Cpu.REG_N + i]:X2} ";
                window.DebugPrint(line);

                line = " M: ";
                for (i = 6; i >= 0; i--)
                    line += $"{cpu.ram[Cpu.REG_M + i]:X2} ";
                window.DebugPrint(line);

            }

            line = " L: ";
            for (i = 6; i >= 0; i--)
                line += $"{cpu.ram[Cpu.REG_L + i]:X2} ";
            window.DebugPrint(line);

            line = " X: ";
            for (i = 6; i >= 0; i--)
                line += $"{cpu.ram[Cpu.REG_X + i]:X2} ";
            window.DebugPrint(line);

            line = " Y: ";
            for (i = 6; i >= 0; i--)
                line += $"{cpu.ram[Cpu.REG_Y + i]:X2} ";
            window.DebugPrint(line);

            line = " Z: ";
            for (i = 6; i >= 0; i--)
                line += $"{cpu.ram[Cpu.REG_Z + i]:X2} ";
            window.DebugPrint(line);

            line = " T: ";
            for (i = 6; i >= 0; i--)
                line += $"{cpu.ram[Cpu.REG_T + i]:X2} ";
            window.DebugPrint(line);

        }

    }
}

