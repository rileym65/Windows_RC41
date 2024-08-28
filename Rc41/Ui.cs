using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Rc41
{
    public class Ui
    {
        protected const int M_BASE = 0;
        protected const int M_NARG = 1;
        protected const int M_AARG = 2;
        protected const int M_ASN = 3;

        protected const int N_IND = 0x10;
        protected const int N_ST = 0x20;
        protected const int N_ALPHA = 0x40;
        protected const int N_DOT = 0x80;

        protected const int K_APPEND = 0x81;
        protected const int K_ASTO = 0x82;
        protected const int K_ARCL = 0x83;
        protected const int K_SST = 0x84;
        protected const int K_BS = 0x85;
        protected const int K_CLA = 0x86;
        protected const int K_RS = 0x87;
        protected const int K_AVIEW = 0x88;
        protected const int K_BST = 0x89;

        public Cpu cpu;
        public Form1 window;
        protected int mode;
        protected int pendingKey;

//        public string key;
//        public byte cmd;
//        public byte scmd;
//        public byte keycode;
//        public byte skeycode;
//        public byte bit;
//        public byte alpha;
//        public byte salpha;

        public Key[] keys = new Key[]
        {
//                   name    cmd  scmd     kc   skc  bit      alpha     salpha
            new Key("01",     96,   97,  0x01, 0x09,  36, (byte)'A', (byte)'a' ),      // 00  E+     E-
            new Key("11",     16,  127,  0x11, 0x19,  28, (byte)'B', (byte)'b' ),      // 01  1/X    Y^X
            new Key("21",    102,  126,  0x21, 0x29,  20, (byte)'C', (byte)'c' ),      // 02  SQRT   X^2
            new Key("31",     70,   17,  0x31, 0x39,  12, (byte)'D', (byte)'d' ),      // 03  LOG    10^X
            new Key("41",     71,   48,  0x41, 0x49,   4, (byte)'E', (byte)'e' ),      // 04  LN     E^X
            new Key("02",    124,   37,  0x02, 0x0a,  35, (byte)'F',       126 ),      // 05  X<>Y   CLE
            new Key("12",     90,   80,  0x12, 0x1a,  27, (byte)'G', (byte)'%' ),      // 06  RDN    %
            new Key("22",     99,   25,  0x22, 0x2a,  19, (byte)'H',        29 ),      // 07  SIN    ASIN
            new Key("32",     42,   19,  0x32, 0x3a,  11, (byte)'I', (byte)'<' ),      // 08  COS    ACOS
            new Key("42",    110,   28,  0x42, 0x4a,   3, (byte)'J', (byte)'>' ),      // 09  TAN    ATAN
            new Key("13",    125,   26,  0x13, 0x1b,  26, (byte)'K',  K_APPEND ),      // 10  XEQ    ASN             append
            new Key("23",    108,   69,  0x23, 0x2b,  18, (byte)'L',    K_ASTO ),      // 11  STO    LBL             asto
            new Key("33",     89,   61,  0x33, 0x3b,  10, (byte)'M',    K_ARCL ),      // 12  RCL    GTO             arcl
            new Key("43",    103,   31,  0x43, 0x4b,   2,     K_SST,     K_BST ),      // 13  SST    BST     sst     bst
            new Key("04",     52,   32,  0x04, 0x0c,  33, (byte)'N', (byte)'^' ),      // 14  ENTER^ CAT
            new Key("24",     34,   67,  0x24, 0x2c,  17, (byte)'O',        13 ),      // 15  CHS    ISG
            new Key("34",     11,   92,  0x34, 0x3c,   9, (byte)'P', (byte)'$' ),      // 16  EEX    RTN
            new Key("44",    254,   41,  0x44, 0x4c,   1,      K_BS,     K_CLA ),      // 17  BS     CLX     bs      cla
            new Key("05",     13,  118,  0x05, 0x0d,  32, (byte)'Q', (byte)'-' ),      // 18  -      X=Y?
            new Key("15",      7,   95,  0x15, 0x1d,  24, (byte)'R', (byte)'7' ),      // 19  7      SF
            new Key("25",      8,   33,  0x25, 0x2d,  16, (byte)'S', (byte)'8' ),      // 20  8      CF
            new Key("35",      9,   58,  0x35, 0x3d,   8, (byte)'T', (byte)'9' ),      // 21  9      FS?
            new Key("06",     12,  121,  0x06, 0x0e,  31, (byte)'U', (byte)'+' ),      // 22  +      X<=Y?
            new Key("16",      4,   30,  0x16, 0x1e,  23, (byte)'V', (byte)'4' ),      // 23  4      BEEP
            new Key("26",      5,   78,  0x26, 0x2e,  15, (byte)'W', (byte)'5' ),      // 24  5      P-R
            new Key("36",      6,   87,  0x36, 0x3e,   7, (byte)'X', (byte)'6' ),      // 25  6      R-P
            new Key("07",     14,  122,  0x07, 0x0f,  30, (byte)'Y', (byte)'*' ),      // 26  *      X>Y?
            new Key("17",      1,   56,  0x17, 0x1f,  22, (byte)'Z', (byte)'1' ),      // 27  1      FIX
            new Key("27",      2,   94,  0x27, 0x2f,  14, (byte)'=', (byte)'2' ),      // 28  2      SCI
            new Key("37",      3,   51,  0x37, 0x3f,   6, (byte)'?', (byte)'3' ),      // 29  3      ENG
            new Key("08",     15,  113,  0x08, 0x10,  29, (byte)':', (byte)'/' ),      // 30  /      X=0?
            new Key("18",      0,   82,  0x18, 0x20,  21, (byte)' ', (byte)'0' ),      // 31  0      PI
            new Key("28",     10,   68,  0x28, 0x30,  13, (byte)',', (byte)'.' ),      // 32  .      LASTX
            new Key("38",    109,  112,  0x38, 0x40,   5,      K_RS,   K_AVIEW ),      // 33  R/S    VIEW    rs      aview
            new Key("",      255,  255,  0xff, 0xff,   0,         0,         0 )
        };

        public Ui(Cpu c, Form1 w)
        {
            cpu = c;   
            window = w;
            mode = M_BASE;
            pendingKey = -1;
        }



        public bool SearchKaPrograms(byte keycode)
        {
            int adr;
            int dst;
            int i;
            int flag;
            adr = ((cpu.ram[Cpu.REG_C + 1] & 0x0f) << 8) | cpu.ram[Cpu.REG_C + 0];
            adr = cpu.FromPtr(adr) + 2;
            flag = 0;
            while (flag == 0)
            {
                if (cpu.ram[adr] >= 0xc0 && cpu.ram[adr] <= 0xcd &&
                    cpu.ram[adr - 2] >= 0xf0 && cpu.ram[adr - 3] == keycode)
                {
                    flag = 1;
                }
                else
                {
                    dst = ((cpu.ram[adr] & 0x0f) << 8) | cpu.ram[adr - 1];
                    dst = ((dst & 0x1ff) * 7) + ((dst >> 9) & 0x7);
                    if (dst == 0) flag = 2;
                    else adr += dst - 1;
                }
            }
            if (flag == 1)
            {
                adr = cpu.ToPtr(adr + 1);
                for (i = 0; i < 7; i++) cpu.ram[Cpu.REG_B + i] = 0x00;
                for (i = 0; i < 7; i++) cpu.ram[Cpu.REG_A + i] = 0x00;
                cpu.ram[Cpu.REG_B + 1] = (byte)((adr >> 8) & 0xff);
                cpu.ram[Cpu.REG_B + 0] = (byte)(adr & 0xff);
                cpu.ram[Cpu.REG_E + 1] |= 0x0f;
                cpu.ram[Cpu.REG_E + 0] = 0xff;
                if (cpu.FlagSet(22)) cpu.EndNumber();
                return true;
            }
            return false;
        }

        public void AddNumber(char n)
        {
            int i;
            int p;
            p = -1;
            Number x;
            cpu.ram[Cpu.PENDING] = (byte)'E';
            if (cpu.FlagSet(22) == false)
            {
                for (i = Cpu.REG_P + 3; i < Cpu.REG_R; i++) cpu.ram[i] = 0xff;
                cpu.ram[Cpu.REG_E + 1] &= 0x0f;
                cpu.ram[Cpu.REG_E + 2] |= 0x0f;
                cpu.SetFlag(22);
            }
            if (n < 10)
            {                                       /* digit */
                if (cpu.ram[Cpu.REG_P + 5] == 11)
                {
                    if (cpu.ram[Cpu.REG_P + 4] == 0xff) cpu.ram[Cpu.REG_P + 4] = (byte)n;
                    else if (cpu.ram[Cpu.REG_P + 3] == 0xff) cpu.ram[Cpu.REG_P + 3] = (byte)n;
                }
                else
                {
                    p = 0;
                    while (p < 10 && cpu.ram[Cpu.REG_Q + 6 - p] != 0xff) p++;
                    if (p < 10) cpu.ram[Cpu.REG_Q + 6 - p] = (byte)n;
                }
            }
            if (n == 11)
            {                                      /* EEX */
                if (cpu.ram[Cpu.REG_Q + 6] == 0xff) cpu.ram[Cpu.REG_Q + 6] = 0x01;
                if (cpu.ram[Cpu.REG_P + 5] == 0xff) cpu.ram[Cpu.REG_P + 5] = 11;
            }
            if (n == 12)
            {                                      /* CHS */
                if (cpu.ram[Cpu.REG_P + 5] == 11) cpu.ram[Cpu.REG_E + 1] ^= 0x20;
                else cpu.ram[Cpu.REG_E + 1] ^= 0x10;
            }
            if (n == 10)
            {                                      /* . */
                if ((cpu.ram[Cpu.REG_E + 2] & 0x0f) == 0x0f && cpu.ram[Cpu.REG_P + 5] != 11)
                {
                    p = 0;
                    while (p < 10 && cpu.ram[Cpu.REG_Q + 6 - p] != 0xff) p++;
                    cpu.ram[Cpu.REG_E + 2] &= 0xf0;
                    cpu.ram[Cpu.REG_E + 2] |= (byte)p;
                }
            }

        }

        public void BackNumber()
        {
            int p;
            int dp;
            if (cpu.ram[Cpu.REG_P + 5] == 11)
            {
                if (cpu.ram[Cpu.REG_P + 3] != 0xff) cpu.ram[Cpu.REG_P + 3] = 0xff;
                else if (cpu.ram[Cpu.REG_P + 4] != 0xff) cpu.ram[Cpu.REG_P + 4] = 0xff;
                else if ((cpu.ram[Cpu.REG_E + 1] & 0x20) != 0) cpu.ram[Cpu.REG_E + 1] ^= 0x20;
                else cpu.ram[Cpu.REG_P + 5] = 0xff;
            }
            else
            {
                p = 0;
                while (cpu.ram[Cpu.REG_Q + 6 - p] != 0xff && p < 10) p++;
                if (p <= 10)
                {
                    p--;
                    dp = cpu.ram[Cpu.REG_E + 2] & 0x0f;
                    if (dp != 15 && p < dp)
                    {
                        cpu.ram[Cpu.REG_E + 2] |= 0x0f;
                    }
                    else
                        cpu.ram[Cpu.REG_Q + 6 - p] = 0xff;
                }
                if (cpu.ram[Cpu.REG_Q + 6] == 0xff)
                {
                    cpu.ClearFlag(22);
                    mode = M_BASE;
                }
            }
            window.Display(cpu.Display());
        }

        string Display()
        {
            int i;
            byte cmd;
            string line;
            cmd = cpu.ram[Cpu.REG_R + 0];

            if (mode == M_ASN)
            {
                line = "ASN ";
                i = Cpu.REG_Q;
                while (i < Cpu.REG_Q + 7 && cpu.ram[i] != 0x00) line += (char)cpu.ram[i++];
                line += " _";
                return line;
            }

            if (mode == M_BASE && cpu.FlagSet(Cpu.F_ALPHA))
            {
                line = "";
                i = Cpu.REG_P + 2;
                while (i >= Cpu.REG_M && cpu.ram[i] == 0x00) i--;
                while (i >= Cpu.REG_M) line += (char)cpu.ram[i--];
                if (cpu.FlagSet(Cpu.F_ALPHA_IN)) line += "_";
                return line;
            }
            if (cpu.FlagSet(Cpu.F_PRGM) && (cpu.ram[Cpu.REG_E+1] & N_DOT) == 0x00 &&
                (cpu.catalog[cmd].flags & 0x80) == 0)
                line = $"{(((cpu.ram[Cpu.REG_E + 1] & 0x0f) << 8) | cpu.ram[Cpu.REG_E + 0]) + 1:d2} ";
            else
                line = "";
            line += cpu.catalog[cmd].name + " ";
            if ((cpu.ram[Cpu.REG_E + 1] & N_DOT) != 0) line += ".";
            if (cpu.FlagSet(Cpu.F_ALPHA_IN) && mode != M_NARG)
            {
                i = 0;
                while (i < 7 && cpu.ram[Cpu.REG_Q + i] != 0x00) {
                    line += (char)cpu.ram[Cpu.REG_Q + i];
                    i++;
                }
                if (i < 7) line += "_";
            }
            else
            {
                if ((cpu.ram[Cpu.REG_E + 1] & N_IND) != 0) line += "IND ";
                if ((cpu.ram[Cpu.REG_E + 1] & N_ST) != 0) line += "ST ";
                if ((cpu.ram[Cpu.REG_E + 1] & N_ST) == 0)
                {
                    if ((cpu.catalog[cmd].flags & 0x03) == 0x3)
                    {
                        if ((cpu.ram[Cpu.REG_R + 2] & 0x0f) != 0x0f) line += (char)('0' + (cpu.ram[Cpu.REG_R + 2] & 0x0f));
                        else line += '_';
                    }
                    if ((cpu.catalog[cmd].flags & 0x03) >= 0x2 || (cpu.ram[Cpu.REG_E+1] & N_IND) != 0)
                    {
                        if ((cpu.ram[Cpu.REG_R + 1] & 0xf0) != 0xf0) line += (char)('0' + ((cpu.ram[Cpu.REG_R + 1] & 0xf0) >> 4));
                        else line += '_';
                    }
                }
                line += '_';
            }

            return line;
        }
 
        public void ProgramAlpha(int key)
        {
            if (key == 0x81) key = 0x7f;
            cpu.ram[Cpu.REG_R + 0] = (byte)key;
            if (cpu.FlagSet(Cpu.F_ALPHA_IN))
            {
                if (cpu.ram[Cpu.REG_P + 6] >= 15) return;
                cpu.ram[Cpu.REG_P + 6]++;
                cpu.ram[Cpu.REG_R + 1] = (byte)(0xf0 + cpu.ram[Cpu.REG_P + 6]);
            }
            else
            {
                cpu.ram[Cpu.REG_P + 6] = 1;
                cpu.ram[Cpu.REG_R + 1] = 0xf1;
            }
            cpu.ProgramStep("");
            window.Display(Display());
        }

        public void ProgramAlphaBack()
        {
            int addr;
            int ln;
            if (!cpu.FlagSet(Cpu.F_ALPHA_IN)) return;
            if (cpu.ram[Cpu.REG_P + 6] == 0) return;
            addr = cpu.FromPtr((cpu.ram[Cpu.REG_B + 1] << 8) | cpu.ram[Cpu.REG_B + 0]);
            cpu.ram[addr - 1] = 0x00;
            addr = cpu.ToPtr(addr + 1);
            cpu.ram[Cpu.REG_B + 1] = (byte)((addr >> 8) & 0xff);
            cpu.ram[Cpu.REG_B + 0] = (byte)(addr & 0xff);
            cpu.ram[Cpu.REG_P + 6]--;
            cpu.ram[cpu.alphaPos]--;
            if (cpu.ram[Cpu.REG_P + 6] == 0x00)
            {
                cpu.ram[cpu.alphaPos] = 0x00;
                cpu.ClearFlag(Cpu.F_ALPHA_IN);
                ln = (cpu.ram[Cpu.REG_E + 1] << 8) | cpu.ram[Cpu.REG_E + 0];
                cpu.GotoLine(ln - 1);
            }
            if (cpu.FlagSet(Cpu.F_ALPHA_IN))
                window.Display(cpu.Display() + "_");
            else
                window.Display(cpu.Display());
        }

        public void AlphaDown(int key)
        {
            int p;
            byte c;
            c = (cpu.FlagSet(Cpu.F_SHIFT)) ? keys[key].salpha : keys[key].alpha;
            cpu.ClearFlag(Cpu.F_SHIFT);
            window.Shift(false);

            if (mode == M_AARG)
            {
                if (c >= 0x80)
                {
                    pendingKey = c;
                    return;
                }
                p = 0;
                while (cpu.ram[Cpu.REG_Q + p] != 0x00) p++;
                if (p < 7)
                {
                    cpu.ram[Cpu.REG_Q + p] = c;
                    window.Display(Display());
                }
                return;
            }

            else if (mode == M_BASE)
            {
                if (cpu.FlagSet(Cpu.F_PRGM) && (c < 0x80 || c == 0x81))
                {
                    ProgramAlpha(c);
                    window.Display(cpu.Display()+"_");
                    return;
                }
                if (!cpu.FlagSet(Cpu.F_ALPHA_IN) && c < 0x80)
                {
                    for (p = Cpu.REG_M; p < Cpu.REG_P + 3; p++) cpu.ram[p] = 0x00;
                }
                if (c < 0x80)
                {
                    cpu.SetFlag(Cpu.F_ALPHA_IN);
                    for (p = Cpu.REG_P + 2; p > Cpu.REG_M; p--) cpu.ram[p] = cpu.ram[p - 1];
                    cpu.ram[Cpu.REG_M] = c;
                }
                else if (c == K_APPEND)
                {
                    cpu.SetFlag(Cpu.F_ALPHA_IN);
                }
                else if (c > 0x81) pendingKey = c;
                window.Display(Display());
                
            }
        }

        public void AlphaUp(int key)
        {
            int i;
            byte c;
            bool flag;
            if (mode == M_BASE)
            {
                //            c = (cpu.FlagSet(Cpu.F_SHIFT)) ? keys[key].salpha : keys[key].alpha;
                if (key == K_ASTO)                    // ASTO
                {
                    cpu.ram[Cpu.REG_R + 0] = 27;
                    mode = M_NARG;
                    cpu.ram[Cpu.REG_R + 2] &= 0xf0;
                    cpu.ram[Cpu.REG_R + 1] = 0xff;
                    cpu.ram[Cpu.REG_E + 1] &= 0x0f;
                    cpu.ram[Cpu.REG_E + 2] &= 0xf0;
                    window.Display(Display());
                }
                if (key == K_ARCL)                   // ARCL
                {
                    cpu.ram[Cpu.REG_R + 0] = 23;
                    mode = M_NARG;
                    cpu.ram[Cpu.REG_R + 2] &= 0xf0;
                    cpu.ram[Cpu.REG_R + 1] = 0xff;
                    cpu.ram[Cpu.REG_E + 1] &= 0x0f;
                    cpu.ram[Cpu.REG_E + 2] &= 0xf0;
                    window.Display(Display());
                }
                if (key == K_BS)                     // BS
                {
                    if (cpu.FlagSet(Cpu.F_PRGM) && cpu.FlagSet(Cpu.F_ALPHA_IN))
                    {
                        ProgramAlphaBack();
                        return;
                    }
                    if (cpu.FlagSet(Cpu.F_ALPHA_IN))
                    {
                        flag = false;
                        for (i = Cpu.REG_M; i <= Cpu.REG_P + 2; i++)
                            if (cpu.ram[i] != 0x00) flag = true;
                        if (!flag) cpu.ClearFlag(Cpu.F_ALPHA_IN);
                        for (i = Cpu.REG_M; i < Cpu.REG_P + 2; i++) cpu.ram[i] = cpu.ram[i + 1];
                        cpu.ram[Cpu.REG_P + 2] = 0x00;
                    }
                    else
                    {
                        for (i = Cpu.REG_M; i <= Cpu.REG_P + 2; i++) cpu.ram[i] = 0x00;
                    }
                    window.Display(Display());
                }
                if (key == K_CLA)                  // CLA
                {
                    if (cpu.FlagSet(Cpu.F_PRGM))
                    {
                        cpu.ram[Cpu.REG_R + 1] = 0x87;
                        cpu.ram[Cpu.REG_R + 0] = 0x00;
                        cpu.Execute();
                        window.Display(cpu.Display());
                        return;
                    }
                    cpu.ClearFlag(Cpu.F_ALPHA_IN);
                    for (i = Cpu.REG_M; i < Cpu.REG_P + 3; i++) cpu.ram[i] = 0x00;
                    window.Display(Display());
                }

                if (key == K_RS)
                {
                    if (!cpu.FlagSet(Cpu.F_PRGM))
                    {
                        if (cpu.running)
                        {
                            cpu.running = false;
                            window.RunTimerEnabled(false);
                        }
                        else
                        {
                            cpu.goose = "\x81           ";
                            window.Display(cpu.goose);
                            cpu.ram[Cpu.REG_E + 0] = 0xff;
                            cpu.ram[Cpu.REG_E + 1] |= 0x0f;
                            cpu.running = true;
                            window.RunTimerEnabled(true);
                        }
                        return;
                    }
                    cpu.ram[Cpu.REG_R + 1] = 0x84;
                    cpu.ram[Cpu.REG_R + 0] = 0x00;
                    cpu.Execute();
                    window.Display(cpu.Display());
                    return;
                }
                if (key == K_AVIEW) {
                    cpu.ram[Cpu.REG_R + 1] = 0x7e;
                    cpu.ram[Cpu.REG_R + 0] = 0x00;
                    cpu.Execute();
                    window.Display(cpu.Display());
                    return;
                }
            }

            if (mode == M_AARG)
            {
                if (key == K_BS)                     // BS
                {
                    i = Cpu.REG_Q + 6;
                    while (i >= Cpu.REG_Q && cpu.ram[i] == 0x00) i--;
                    if (i >= Cpu.REG_Q)
                    {
                        cpu.ram[i] = 0x00;
                        window.Display(Display());
                    }
                    else
                    {
                        mode = M_BASE;
                        cpu.ClearFlag(Cpu.F_ALPHA_IN);
                        cpu.ClearFlag(Cpu.F_ALPHA);
                        window.Alpha(false);
                        window.Display(cpu.Display());
                    }
                    return;
                }

            }
        }

        public void SetupAlphaArg()
        {
            cpu.ram[Cpu.REG_E + 1] |= N_ALPHA;
        }

        public void KeyUp(int key)
        {
            int addr;
            byte cmd;
            byte post;
            byte b;
            int i;
            byte keycode;
            bool user;
            string name;
            int link;

            if (cpu.FlagSet(Cpu.F_CAT))
            {
                if (cpu.FlagSet(Cpu.F_SHIFT)) cmd = keys[key].scmd;
                else cmd = keys[key].cmd;
                cpu.ClearFlag(Cpu.F_SHIFT);
                window.Shift(false);
                if (cmd == Cpu.CMD_RS)
                {
                    cpu.catPause = !cpu.catPause;
                }
                if (!cpu.catPause) return;
                if (cmd == Cpu.CMD_BS)
                {
                    cpu.catPause = false;
                    cpu.ClearFlag(Cpu.F_CAT);
                    window.Display(cpu.Display());
                }
                if (cmd == Cpu.CMD_SST)
                {
                    if (cpu.ram[Cpu.REG_R + 0] == 1)
                    {
                        cpu.ShowNextGlobal(false);
                    }
                    else
                    {
                        b = (byte)(cpu.ram[Cpu.REG_R + 1] + 1);
                        if (cpu.catalog[b].catalog == cpu.ram[Cpu.REG_R + 0])
                        {
                            window.Display(cpu.catalog[b].name);
                            if (window.PrinterMode() == 'T') window.Print(cpu.catalog[b].name);
                            cpu.ram[Cpu.REG_R + 1] = b;
                        }
                        else
                        {
                            cpu.ClearFlag(Cpu.F_CAT);
                            cpu.catPause = false;
                            window.Display(cpu.Display());
                        }
                    }

                }
                if (cmd == Cpu.CMD_BST)
                {
                    if (cpu.ram[Cpu.REG_R + 0] == 1)
                    {
                        cpu.ShowPriorGlobal(false);
                    }
                    else
                    {
                        b = (byte)(cpu.ram[Cpu.REG_R + 1] - 1);
                        if (cpu.catalog[b].catalog == cpu.ram[Cpu.REG_R + 0])
                        {
                            window.Display(cpu.catalog[b].name);
                            if (window.PrinterMode() == 'T') window.Print(cpu.catalog[b].name);
                            cpu.ram[Cpu.REG_R + 1] = b;
                        }
                    }
                    return;
                }
                return;
            }

            if (mode == M_ASN)
            {
                if (cpu.FlagSet(Cpu.F_SHIFT)) keycode = keys[key].skeycode;
                else keycode = keys[key].keycode;
                cpu.ClearFlag(Cpu.F_SHIFT);
                window.Shift(false);
                name = "";
                i = Cpu.REG_Q;
                while (i <= Cpu.REG_Q + 6 && cpu.ram[i] != 0x00) name += (char)cpu.ram[i++];
                cpu.Asn(name, keycode);
                mode = M_BASE;
                window.Display(cpu.Display());
                return;
            }

            if (cpu.FlagSet(Cpu.F_USER) && !cpu.FlagSet(Cpu.F_ALPHA) && mode == M_BASE)
            {
                if (cpu.FlagSet(Cpu.F_SHIFT)) keycode = keys[key].skeycode;
                else keycode = keys[key].keycode;
                user = cpu.GetKaFlag(keycode);
                if (user)
                {
                    addr = cpu.SearchKaRegisters(keycode);
                    if (addr >= 0)
                    {
                        if (cpu.ram[addr+1] == 0x04 && cpu.ram[addr+0] == 0x02)         // DEL
                        {
                            cpu.ClearFlag(Cpu.F_SHIFT);
                            window.Shift(false);
                            cpu.ram[Cpu.REG_R + 0] = Cpu.CMD_DEL;
                            cpu.ram[Cpu.REG_R + 1] = 0xff;
                            cpu.ram[Cpu.REG_R + 2] |= 0x0f;
                            cpu.ram[Cpu.REG_E + 1] &= 0x0f;
                            mode = M_NARG;
                            window.Display(Display());
                            return;
                        }
                        if (cpu.ram[addr + 1] == 0x04 && cpu.ram[addr + 0] == 0x04)         // CLP
                        {
                            cpu.ClearFlag(Cpu.F_SHIFT);
                            window.Shift(false);
                            cpu.ram[Cpu.REG_R + 0] = Cpu.CMD_CLP;
                            cpu.ram[Cpu.REG_R + 1] = 0xff;
                            cpu.ram[Cpu.REG_R + 2] |= 0x0f;
                            cpu.ram[Cpu.REG_E + 1] &= 0x0f;
                            mode = M_NARG;
                            window.Display(Display());
                            return;
                        }
                        if (cpu.ram[addr + 1] == 0x04 && cpu.ram[addr + 0] == 0x06)         // SIZE
                        {
                            cpu.ClearFlag(Cpu.F_SHIFT);
                            window.Shift(false);
                            cpu.ram[Cpu.REG_R + 0] = Cpu.CMD_SIZE;
                            cpu.ram[Cpu.REG_R + 1] = 0xff;
                            cpu.ram[Cpu.REG_R + 2] |= 0x0f;
                            cpu.ram[Cpu.REG_E + 1] &= 0x0f;
                            mode = M_NARG;
                            window.Display(Display());
                            return;
                        }
                        if (cpu.ram[addr + 1] == 0x04 && cpu.ram[addr + 0] == 0x0a)         // PACK
                        {
                            cpu.ClearFlag(Cpu.F_SHIFT);
                            window.Shift(false);
                            cpu.Pack();
                            mode = M_BASE;
                            window.Display(cpu.Display());
                            return;
                        }
                        if (cpu.ram[addr + 1] <= 0x0f)
                        {
                            link = cpu.reverse[cpu.ram[addr + 0]].link;
                            if ((cpu.reverse[cpu.ram[addr + 0]].size & 0x3) > 1)
                            {
                                cpu.ram[Cpu.REG_R + 0] = (byte)link;
                                cpu.ram[Cpu.REG_R + 2] &= 0xf0;
                                cpu.ram[Cpu.REG_R + 1] = 0xff;
                                switch (cpu.catalog[link].flags & 0x03)
                                {
                                    case 0x01: cpu.ram[Cpu.REG_R + 1] = 0x0f; break;
                                    case 0x03: cpu.ram[Cpu.REG_R + 2] |= 0x0f; break;
                                }
                                cpu.ram[Cpu.REG_E + 1] &= 0x0f;
                                cpu.ram[Cpu.REG_E + 2] &= 0xf0;
                                mode = M_NARG;
                                cpu.ClearFlag(Cpu.F_SHIFT);
                                window.Shift(false);
                                window.Display(Display());
                                return;

                            }
                            cpu.ram[Cpu.REG_R + 1] = cpu.ram[addr + 0];
                            cpu.ram[Cpu.REG_R + 0] = 0x00;
                        }
                        else
                        {
                            cpu.ram[Cpu.REG_R + 1] = cpu.ram[addr + 1];
                            cpu.ram[Cpu.REG_R + 0] = cpu.ram[addr + 0];
                        }
                        cpu.ClearFlag(Cpu.F_SHIFT);
                        window.Shift(false);
                        cpu.Execute();
                        return;
                    }
                    if (SearchKaPrograms(keycode))
                    {
                        cpu.ClearFlag(Cpu.F_SHIFT);
                        window.Shift(false);
                        cpu.running = true;
                        window.RunTimerEnabled(true);
                        return;
                    }
                }
            }



            cmd = (cpu.FlagSet(Cpu.F_SHIFT)) ? keys[key].scmd : keys[key].cmd;
            if (cmd == Cpu.CMD_BS && cpu.FlagSet(Cpu.F_MSG))
            {
                cpu.ClearFlag(Cpu.F_MSG);
                window.Display(cpu.Display());
                return;
            }
            cpu.ClearFlag(Cpu.F_MSG);

            if (mode == M_BASE)
            {
                cpu.ClearFlag(Cpu.F_SHIFT);
                window.Shift(false);
                if (cmd == 34 && cpu.FlagSet(Cpu.F_NUM_IN)) cmd = 202;
                if (cmd < 12 || cmd == 202)
                {
                    if (cmd == 202) cmd = 12;
                    AddNumber((char)cmd);
                    window.Display(cpu.Display());
                    return;
                }

                if (cmd == Cpu.CMD_BS)
                {
                    if (cpu.FlagSet(Cpu.F_PRGM) && !cpu.FlagSet(Cpu.F_NUM_IN))
                    {
                        cpu.Del(1);
                        window.Display(cpu.Display());
                        return;
                    }
                    else if (cpu.FlagSet(Cpu.F_NUM_IN))
                    {
                        BackNumber();
                    }
                    else
                    {
                        cpu.ram[Cpu.REG_R + 1] = 0x77;
                        cpu.ram[Cpu.REG_R + 0] = 0x00;
                        cpu.Execute();
                    }
                    return;
                }

                if (cpu.FlagSet(Cpu.F_NUM_IN)) cpu.EndNumber();

                if (cmd == Cpu.CMD_BST)
                {
                    if (cpu.FlagSet(Cpu.F_PRGM))
                    {
                        cpu.Bst();
                        window.Display(cpu.Display());
                        return;
                    }
                    else
                    {
                        return;
                    }
                }

                if (cmd == Cpu.CMD_SST)
                {
                    if (cpu.FlagSet(Cpu.F_PRGM))
                    {
                        cpu.Sst();
                        window.Display(cpu.Display());
                        return;
                    }
                    else
                    {
                        cpu.Sst();
                        window.Display(cpu.Display());
                        return;
                    }
                }

                if (cmd == Cpu.CMD_RS)
                {
                    if (!cpu.FlagSet(Cpu.F_PRGM))
                    {
                        if (cpu.running)
                        {
                            cpu.running = false;
                            window.RunTimerEnabled(false);
                        }
                        else
                        {
                            cpu.goose = "\x81           ";
                            window.Display(cpu.goose);
                            cpu.ram[Cpu.REG_E + 0] = 0xff;
                            cpu.ram[Cpu.REG_E + 1] |= 0x0f;
                            cpu.running = true;
                            window.RunTimerEnabled(true);
                        }
                        return;
                    }
                }
                if ((cpu.catalog[cmd].flags & 0x1f) == 0x00)
                {
                    cpu.ram[Cpu.REG_R + 1] = cpu.catalog[cmd].cmd;
                    cpu.ram[Cpu.REG_R + 0] = 0x00;
                    cpu.Execute();
                    return;
                }
                if ((cpu.catalog[cmd].flags & 0x13) != 0x00)
                {
                    cpu.ram[Cpu.REG_R + 0] = cmd;
                    cpu.ram[Cpu.REG_R + 2] &= 0xf0;
                    cpu.ram[Cpu.REG_R + 1] = 0xff;
                    if ((cpu.catalog[cmd].flags & 0x1f) == 0x10)
                    {
                        cpu.ram[Cpu.REG_R + 0] = (byte)cmd;
                        cpu.ram[Cpu.REG_R + 2] &= 0xf0;
                        cpu.ram[Cpu.REG_R + 1] = 0xff;
                        switch (cpu.catalog[cmd].flags & 0x03)
                        {
                            case 0x01: cpu.ram[Cpu.REG_R + 1] = 0x0f; break;
                            case 0x03: cpu.ram[Cpu.REG_R + 2] |= 0x0f; break;
                        }
                        cpu.ram[Cpu.REG_E + 1] &= 0x0f;
                        cpu.ram[Cpu.REG_E + 2] &= 0xf0;
                        mode = M_NARG;
                        window.Display(Display());
                        return;
                    }
                    else
                    {
                        switch (cpu.catalog[cmd].flags & 0x03)
                        {
                            case 0x01: cpu.ram[Cpu.REG_R + 1] = 0x0f; break;
                            case 0x03: cpu.ram[Cpu.REG_R + 2] |= 0x0f; break;
                        }
                        cpu.ram[Cpu.REG_E + 1] &= 0x0f;
                        cpu.ram[Cpu.REG_E + 2] &= 0xf0;
                        mode = M_NARG;
                        window.Display(Display());
                    }
                    return;
                }
            }

            if (mode == M_NARG)
            {
                if (cmd == Cpu.CMD_BS)
                {
                    if ((cpu.catalog[cpu.ram[Cpu.REG_R + 0]].flags & 0x003) == 2 &&
                        cpu.ram[Cpu.REG_R+1] == 0xff)
                    {
                        mode = M_BASE;
                        window.Display(cpu.Display());
                        return;
                    }
                    if ((cpu.catalog[cpu.ram[Cpu.REG_R + 0]].flags & 0x003) == 3 &&
                        cpu.ram[Cpu.REG_R + 1] == 0xff && (cpu.ram[Cpu.REG_R+2] & 0x0f) == 0x0f)
                    {
                        mode = M_BASE;
                        window.Display(cpu.Display());
                        return;
                    }
                    if ((cpu.ram[Cpu.REG_R + 1] & 0xf0) != 0xf0)
                    {
                        cpu.ram[Cpu.REG_R + 1] |= 0xf0;
                    }
                    else if (cpu.ram[Cpu.REG_R + 1] == 0xff &&
                        (cpu.catalog[cpu.ram[Cpu.REG_R + 0]].flags & 0x003) == 3 &&
                        (cpu.ram[Cpu.REG_R + 2] & 0x0f) != 0f)
                    {
                        cpu.ram[Cpu.REG_R + 2] |= 0x0f;
                    }
                    window.Display(Display());
                    return;
                }
                if ((cpu.ram[Cpu.REG_E + 1] & N_ST) != 0)
                {
                    if (cmd == 0x09) cpu.ram[Cpu.REG_R + 1] = 0xb2;
                    else if (cmd == 0x01) cpu.ram[Cpu.REG_R + 1] = 0xb3;
                    else if (cmd == 0x0e) cpu.ram[Cpu.REG_R + 1] = 0xb4;
                    else if (cmd == 0x06) cpu.ram[Cpu.REG_R + 1] = 0xb5;
                    else if (cmd == 0x6c) cpu.ram[Cpu.REG_R + 1] = 0xb6;
                    else return;
                }
                else if (cmd == 10 && (cpu.catalog[cpu.ram[Cpu.REG_R + 0]].flags & 0x20) != 0)
                {
                    if (cpu.ram[Cpu.REG_R + 1] != 0xff) return;

                    if ((cpu.ram[Cpu.REG_E + 1] & N_DOT) != 0)
                    {
                        if ((cpu.ram[Cpu.REG_R+2] & 0x0f) != 0x0f) return;
                        mode = M_BASE;
                        cpu.GtoEnd();
                        window.Display(cpu.Display());
                        return;
                    }
                    cpu.ram[Cpu.REG_R + 0] = Cpu.CMD_GTO_DOT;
                    cpu.ram[Cpu.REG_E + 1] |= N_DOT;
                    cpu.ram[Cpu.REG_R + 2] |= 0x0f;
                    window.Display(Display());
                    return;
                }
                else if (cmd < 10)
                {
                    if ((cpu.ram[Cpu.REG_R + 2] & 0x0f) == 0x0f)
                        cpu.ram[Cpu.REG_R + 2] = (byte)((cpu.ram[Cpu.REG_R + 2] & 0xf0) | cmd);
                    else if ((cpu.ram[Cpu.REG_R + 1] & 0xf0) == 0xf0)
                        cpu.ram[Cpu.REG_R + 1] = (byte)((cpu.ram[Cpu.REG_R + 1] & 0x0f) | (cmd << 4));
                    else
                        cpu.ram[Cpu.REG_R + 1] = (byte)((cpu.ram[Cpu.REG_R + 1] & 0xf0) | cmd);
                }
                else if (cmd == 10)
                {
                    if (cpu.ram[Cpu.REG_R + 1] != 0xff) return;
                    if ((cpu.ram[Cpu.REG_E + 1] & N_IND) != 0 || (cpu.catalog[cpu.ram[Cpu.REG_R + 0]].flags & 0x04) != 0)
                    {
                        cpu.ram[Cpu.REG_E + 1] |= N_ST;
                        cpu.ram[Cpu.REG_R + 1] = 0x0f;
                    }
                }
                else if (cmd == 12 && cpu.ram[Cpu.REG_R + 0] == 108 && cpu.ram[Cpu.REG_R + 1] == 0xff) cpu.ram[Cpu.REG_R + 0] = 104;  // ST+
                else if (cmd == 13 && cpu.ram[Cpu.REG_R + 0] == 108 && cpu.ram[Cpu.REG_R + 1] == 0xff) cpu.ram[Cpu.REG_R + 0] = 105;  // ST-
                else if (cmd == 14 && cpu.ram[Cpu.REG_R + 0] == 108 && cpu.ram[Cpu.REG_R + 1] == 0xff) cpu.ram[Cpu.REG_R + 0] = 106;  // ST*
                else if (cmd == 15 && cpu.ram[Cpu.REG_R + 0] == 108 && cpu.ram[Cpu.REG_R + 1] == 0xff) cpu.ram[Cpu.REG_R + 0] = 107;  // ST/
                if ((cpu.ram[Cpu.REG_R + 1] & 0x0f) != 0x0f)
                {
                    if (cpu.ram[Cpu.REG_R+0] == Cpu.CMD_DEL)
                    {
                        i = (byte)(((cpu.ram[Cpu.REG_R + 1] >> 4) * 10) + (cpu.ram[Cpu.REG_R + 1] & 0x0f));
                        i += (cpu.ram[Cpu.REG_R + 2] & 0x0f) * 100;
                        mode = M_BASE;
                        if (cpu.FlagSet(Cpu.F_PRGM)) cpu.Del(i);
                        window.Display(cpu.Display());
                        return;
                    }
                    if (cpu.ram[Cpu.REG_R + 0] == Cpu.CMD_SIZE)
                    {
                        i = (byte)(((cpu.ram[Cpu.REG_R + 1] >> 4) * 10) + (cpu.ram[Cpu.REG_R + 1] & 0x0f));
                        i += (cpu.ram[Cpu.REG_R + 2] & 0x0f) * 100;
                        mode = M_BASE;
                        cpu.Size(i);
                        window.Display(cpu.Display());
                        return;
                    }
                    if (cpu.ram[Cpu.REG_R + 0] == Cpu.CMD_GTO_DOT)
                    {
                        i = (byte)(((cpu.ram[Cpu.REG_R + 1] >> 4) * 10) + (cpu.ram[Cpu.REG_R + 1] & 0x0f));
                        i += (cpu.ram[Cpu.REG_R + 2] & 0x0f) * 100;
                        mode = M_BASE;
                        cpu.GotoLine(i);
                        window.Display(cpu.Display());
                        return;
                    }
                    if (cpu.ram[Cpu.REG_R + 0] == Cpu.CMD_NEWM)
                    {
                        i = (byte)(((cpu.ram[Cpu.REG_R + 1] >> 4) * 10) + (cpu.ram[Cpu.REG_R + 1] & 0x0f));
                        i += (cpu.ram[Cpu.REG_R + 2] & 0x0f) * 100;
                        mode = M_BASE;
                        cpu.tapeDrive.Command(3, i);
                        window.Display(cpu.Display());
                        return;
                    }
                    if (cpu.ram[Cpu.REG_R + 0] == Cpu.CMD_GTO_ALPHA)
                    {
                        cpu.ram[Cpu.REG_R + 1] = 0x1d;
                        cpu.ram[Cpu.REG_R + 0] = 0x00;
                        mode = M_BASE;
                        cpu.Execute();
                        window.Display(cpu.Display());
                        return;
                    }
                    if (cpu.ram[Cpu.REG_R+0] == Cpu.CMD_CAT)
                    {
                        cpu.SetFlag(Cpu.F_CAT);
                        cpu.catPause = false;
                        mode = M_BASE;
                        b = (byte)(cpu.ram[Cpu.REG_R + 1] & 0x0f);
                        cpu.ram[Cpu.REG_R + 0] = b;
                        cpu.ram[Cpu.REG_R + 1] = 0;
                        if (b == 1)
                        {
                            window.DisplayTimerEnabled(false);
                            i = (cpu.ram[Cpu.REG_C + 2] << 4) | ((cpu.ram[Cpu.REG_C + 1] & 0xf0) >> 4);
                            cpu.ram[Cpu.REG_B + 1] = (byte)((i >> 8) & 0x0f);
                            cpu.ram[Cpu.REG_B + 0] = (byte)(i & 0xff);
                            cpu.ShowNextGlobal(true);
                            window.DisplayTimerEnabled(true);
                        }
                        if (b == 2)
                        {
                            window.DisplayTimerEnabled(false);
                            cpu.ram[Cpu.REG_R + 1] = 128;
                            window.Display(cpu.catalog[128].name);
                            if (window.PrinterMode() == 'T') window.Print(cpu.catalog[128].name);
                            window.DisplayTimerEnabled(true);
                        }
                        if (b == 3)
                        {
                            window.DisplayTimerEnabled(false);
                            cpu.ram[Cpu.REG_R + 1] = 12;
                            window.Display(cpu.catalog[12].name);
                            if (window.PrinterMode() == 'T') window.Print(cpu.catalog[12].name);
                            window.DisplayTimerEnabled(true);
                        }
                        return;
                    }
                    if (cpu.ram[Cpu.REG_R + 0] == Cpu.CMD_GTO && (cpu.ram[Cpu.REG_E+1] & N_IND) != 0)
                    {
                        post = (byte)(((cpu.ram[Cpu.REG_R + 1] >> 4) * 10) + (cpu.ram[Cpu.REG_R + 1] & 0x0f));
                        cpu.ram[Cpu.REG_R + 1] = 0xae;
                        cpu.ram[Cpu.REG_R + 0] = post;
                        mode = M_BASE;
                        cpu.Execute();
                        window.Display(cpu.Display());
                        return;
                    }
                    cmd = cpu.ram[Cpu.REG_R + 0];
                    if (cpu.ram[Cpu.REG_R + 0] == Cpu.CMD_XEQ && (cpu.ram[Cpu.REG_E + 1] & N_IND) != 0)
                    {
                        post = (byte)(((cpu.ram[Cpu.REG_R + 1] >> 4) * 10) + (cpu.ram[Cpu.REG_R + 1] & 0x0f));
                        cpu.ram[Cpu.REG_R + 1] = 0xae;
                        cpu.ram[Cpu.REG_R + 0] = (byte)(post | 0x80);
                        mode = M_BASE;
                        cpu.Execute();
                        window.Display(cpu.Display());
                        return;
                    }
                    post = (byte)(((cpu.ram[Cpu.REG_R + 1] >> 4) * 10) + (cpu.ram[Cpu.REG_R + 1] & 0x0f));
                    if ((cpu.ram[Cpu.REG_E + 1] & N_IND) != 0) post |= 0x80;
                    cpu.ram[Cpu.REG_R + 1] = cpu.catalog[cpu.ram[Cpu.REG_R + 0]].cmd;
                    cpu.ram[Cpu.REG_R + 0] = post;
                    mode = M_BASE;
                    cpu.Execute();
                    window.Display(cpu.Display());
                    return;
                }
                window.Display(Display());
                return;
            }
        }

        public void XeqAlphaArg()
        {
            int addr;
            int p;
            string buffer;
            byte cmd;
            buffer = "";
            p = Cpu.REG_Q;
            while (p <= Cpu.REG_Q + 6 && cpu.ram[p] != 0x00) buffer += (char)cpu.ram[p++];
            cmd = cpu.ram[Cpu.REG_R + 0];
            if (cmd == Cpu.CMD_XEQ_A)
            {
                addr = cpu.FindGlobal(buffer);
                if (addr != 0)
                {
                    cpu.ram[Cpu.REG_R + 1] = cpu.catalog[cpu.ram[Cpu.REG_R + 0]].cmd;
                    cpu.ram[Cpu.REG_R + 0] = 0x00;
                    cpu.Execute();
                    return;
                }

                cpu.errFlag = false;
                cpu.ClearFlag(Cpu.F_MSG);

                p = 0;
                while (cpu.catalog[p].flags != 0xff && !buffer.Equals(cpu.catalog[p].name)) p++;
                if (cpu.catalog[p].flags != 0xff)
                {
                    if ((cpu.catalog[p].flags & 0x7f) == 0x00)
                    {
                        if (p == Cpu.CMD_PACK)
                        {
                            cpu.ram[Cpu.REG_R + 0] = Cpu.CMD_PACK;
                            cpu.ram[Cpu.REG_R + 1] = 0x00;
                            cpu.Execute();
                        }
                        else
                        {
                            cpu.ram[Cpu.REG_R + 1] = cpu.catalog[p].cmd;
                            cpu.ram[Cpu.REG_R + 0] = cpu.catalog[p].post;
                            cpu.Execute();
                        }
                    }
                    else
                    {
                        cpu.ram[Cpu.REG_R + 0] = (byte)p;
                        cpu.ram[Cpu.REG_R + 2] &= 0xf0;
                        cpu.ram[Cpu.REG_R + 1] = 0xff;
                        switch (cpu.catalog[p].flags & 0x03)
                        {
                            case 0x01: cpu.ram[Cpu.REG_R + 1] = 0x0f; break;
                            case 0x03: cpu.ram[Cpu.REG_R + 2] |= 0x0f; break;
                        }
                        cpu.ram[Cpu.REG_E + 1] &= 0x0f;
                        cpu.ram[Cpu.REG_E + 2] &= 0xf0;
                        mode = M_NARG;
                        window.Display(Display());
                        return;
                    }
                    return;
                }
                else if (cpu.FlagSet(52))
                {
                    cpu.ram[Cpu.REG_R + 1] = cpu.catalog[cpu.ram[Cpu.REG_R + 0]].cmd;
                    cpu.ram[Cpu.REG_R + 0] = cpu.catalog[cpu.ram[Cpu.REG_R + 0]].post;
                    cpu.Execute();
                    return;
                }
                else
                {
                    cpu.Message("NONEXISTENT");
                    cpu.Error();
                    window.Display(cpu.Display());
                }
            }

            if (cmd == Cpu.CMD_GTO_ALPHA)
            {
                if (cpu.FlagSet(Cpu.F_PRGM))
                {
                    cpu.ram[Cpu.REG_R + 1] = 0x1d;
                    cpu.ram[Cpu.REG_R + 0] = 0x00;
                    mode = M_BASE;
                    cpu.Execute();
                    window.Display(cpu.Display());
                    return;
                }
                addr = cpu.FindGlobal(buffer);
                if (addr != 0)
                {
                    addr = cpu.ToPtr(addr);
                    cpu.ram[Cpu.REG_B + 1] = (byte)((addr >> 8) & 0xff);
                    cpu.ram[Cpu.REG_B + 0] = (byte)(addr & 0xff);
                    cpu.ram[Cpu.REG_E + 1] |= 0x0f;
                    cpu.ram[Cpu.REG_E + 0] = 0xff;
                    cpu.ram[Cpu.REG_E + 0] = 0xff;
                    cpu.ram[Cpu.REG_E + 1] |= 0x0f;

                }
                else
                {
                    cpu.Message("NONEXISTENT");
                    cpu.Error();
                }
                mode = M_BASE;
                window.Display(cpu.Display());
            }

            if (cmd == Cpu.CMD_GTO_DOT)
            {
                addr = cpu.FindGlobal(buffer);
                if (addr != 0)
                {
                    addr = cpu.ToPtr(addr);
                    cpu.ram[Cpu.REG_B + 1] = (byte)((addr >> 8) & 0xff);
                    cpu.ram[Cpu.REG_B + 0] = (byte)(addr & 0xff);
                    cpu.ram[Cpu.REG_E + 1] |= 0x0f;
                    cpu.ram[Cpu.REG_E + 0] = 0xff;
                    cpu.ram[Cpu.REG_E + 0] = 0xff;
                    cpu.ram[Cpu.REG_E + 1] |= 0x0f;

                }
                else
                {
                    cpu.Message("NONEXISTENT");
                    cpu.Error();
                }
                mode = M_BASE;
                window.Display(cpu.Display());
            }

            if (cmd == Cpu.CMD_LBL_A)
            {
                cpu.ram[Cpu.REG_R + 1] = cpu.catalog[cpu.ram[Cpu.REG_R + 0]].cmd;
                cpu.ram[Cpu.REG_R + 0] = 0x00;
                cpu.Execute();
                return;
            }

            if (cmd == Cpu.CMD_PRP)
            {
                cpu.printer.Prp(buffer);
                mode = M_BASE;
                window.Display(cpu.Display());
                return;
            }

            if (cmd == Cpu.CMD_CLP)
            {
                cpu.Clp(buffer);
                mode = M_BASE;
                window.Display(cpu.Display());
                return;
            }

            if (cmd == Cpu.CMD_ASN)
            {
                mode = M_ASN;
                window.Display(Display());
                return;
            }
        }

        public void Key_Alpha()
        {
            if (cpu.FlagSet(Cpu.F_CAT)) return;
            if (mode == M_BASE)
            {
                if (cpu.FlagSet(Cpu.F_ALPHA)) cpu.ClearFlag(Cpu.F_ALPHA);
                else cpu.SetFlag(Cpu.F_ALPHA);
                window.Alpha(cpu.FlagSet(Cpu.F_ALPHA));
                if (cpu.FlagSet(Cpu.F_PRGM) && cpu.FlagSet(Cpu.F_ALPHA_IN))
                {
                    cpu.alphaPos++;
                    int reg = cpu.alphaPos / 7;
                    int byt = cpu.alphaPos % 7;
                    int addr = (byt << 12) | reg;
                    cpu.ram[Cpu.REG_B + 1] = (byte)((addr >> 8) & 0xff);
                    cpu.ram[Cpu.REG_B + 0] = (byte)(addr & 0xff);
                }
                cpu.ClearFlag(Cpu.F_ALPHA_IN);
                window.Display(cpu.Display());
            }
            else if (mode == M_NARG && (cpu.catalog[cpu.ram[Cpu.REG_R+0]].flags & 0x10) != 0x00)
            {
                cpu.SetFlag(Cpu.F_ALPHA_IN);
                window.Alpha(true);
                if (cpu.ram[Cpu.REG_R + 0] == Cpu.CMD_XEQ) cpu.ram[Cpu.REG_R + 0] = Cpu.CMD_XEQ_A;
                if (cpu.ram[Cpu.REG_R + 0] == Cpu.CMD_LBL) cpu.ram[Cpu.REG_R + 0] = Cpu.CMD_LBL_A;
                if (cpu.ram[Cpu.REG_R + 0] == Cpu.CMD_GTO) cpu.ram[Cpu.REG_R + 0] = Cpu.CMD_GTO_ALPHA;
                for (var i = Cpu.REG_Q; i <= Cpu.REG_Q + 6; i++)
                    cpu.ram[i] = 0x00;
                mode = M_AARG;
                window.Display(Display());
            }
            else if (mode == M_AARG)
            {
                cpu.ClearFlag(Cpu.F_ALPHA_IN);
                window.Alpha(cpu.FlagSet(Cpu.F_ALPHA));
                mode = M_BASE;
                XeqAlphaArg();
            }
        }

        public void Key_Prgm()
        {
            if (cpu.FlagSet(Cpu.F_CAT)) return;
            if (mode != M_BASE) return;
            if (cpu.FlagSet(Cpu.F_PRGM)) cpu.ClearFlag(Cpu.F_PRGM);
            else cpu.SetFlag(Cpu.F_PRGM);
            window.Prog(cpu.FlagSet(Cpu.F_PRGM));
            window.Display(cpu.Display());
        }

        public void Key_Shift()
        {
            if (cpu.FlagSet(Cpu.F_CAT) && !cpu.catPause) return;
            if (cpu.FlagSet(Cpu.F_SHIFT)) cpu.ClearFlag(Cpu.F_SHIFT);
            else cpu.SetFlag(Cpu.F_SHIFT);
            if (cpu.FlagSet(Cpu.F_SHIFT) && mode == M_NARG)
            {
                cpu.ClearFlag(Cpu.F_SHIFT);
                if ((cpu.catalog[cpu.ram[Cpu.REG_R+0]].flags & 0x08) != 0) {
                    cpu.ram[Cpu.REG_E + 1] |= N_IND;
                    cpu.ram[Cpu.REG_R + 1] = 0xff;
                    window.Display(Display());
                }
            }
            window.Shift(cpu.FlagSet(Cpu.F_SHIFT));
        }

        public void Key_User()
        {
            if (cpu.FlagSet(Cpu.F_CAT)) return;
            if (mode != M_BASE) return;
            if (cpu.FlagSet(Cpu.F_USER)) cpu.ClearFlag(Cpu.F_USER);
            else cpu.SetFlag(Cpu.F_USER);
            window.User(cpu.FlagSet(Cpu.F_USER));
        }

        public void ButtonDown(string tag)
        {
            int key;
            key = 0;
            pendingKey = -1;
            while (keys[key].cmd != 0xff && !keys[key].key.Equals(tag)) key++;
            if (keys[key].cmd == 0xff) return;
            if ((cpu.FlagSet(Cpu.F_ALPHA) || cpu.FlagSet(Cpu.F_ALPHA_IN)) && mode != M_NARG) AlphaDown(key);
            else pendingKey = key;
        }

        public void ButtonUp(string tag)
        {
            if (pendingKey < 0) return;
            if ((cpu.FlagSet(Cpu.F_ALPHA) || cpu.FlagSet(Cpu.F_ALPHA_IN)) && mode != M_NARG) AlphaUp(pendingKey);
            else KeyUp(pendingKey);
            pendingKey = -1;
        }

    }
}
