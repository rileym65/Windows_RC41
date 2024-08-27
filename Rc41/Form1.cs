using Microsoft.VisualBasic.ApplicationServices;
using System.Windows.Forms.VisualStyles;
using System.Xml.Serialization;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace Rc41
{
    public partial class Form1 : Form
    {
        public Cpu cpu;
        public Debugger debugger;
        public Ui ui;
        PictureBox[] cells;
        PictureBox[] semis;
        public Form1()
        {
            int x;
            InitializeComponent();
            //            B_SumPlus.Text = "\u2211+";
            //            SumMinus.Text = "\u2211-";
            //            ClearSum.Text = "CL\u2211";
            this.Width = 370;
            cells = new PictureBox[12];
            cells[0] = Cell1;
            cells[1] = Cell2;
            cells[2] = Cell3;
            cells[3] = Cell4;
            cells[4] = Cell5;
            cells[5] = Cell6;
            cells[6] = Cell7;
            cells[7] = Cell8;
            cells[8] = Cell9;
            cells[9] = Cell10;
            cells[10] = Cell11;
            cells[11] = Cell12;
            semis = new PictureBox[12];
            semis[0] = Semi1;
            semis[1] = Semi2;
            semis[2] = Semi3;
            semis[3] = Semi4;
            semis[4] = Semi5;
            semis[5] = Semi6;
            semis[6] = Semi7;
            semis[7] = Semi8;
            semis[8] = Semi9;
            semis[9] = Semi10;
            semis[10] = Semi11;
            semis[11] = Semi12;
            x = 15;
            for (var i = 0; i < 12; i++)
            {
                cells[i].Left = x;
                cells[i].Image = imageList1.Images[2];
                x += 16;
                semis[i].Left = x;
                semis[i].Image = imageList2.Images[3];
                x += 8;
            }
            cpu = new Cpu(this);
            cpu.printer = new Printer(cpu, this);
            cpu.tapeDrive = new TapeDrive(cpu, this);
            cpu.cardReader = new CardReader(cpu, this);
            ui = new Ui(cpu, this);
            cpu.OpenTapeDrive("tape1.dat");
            debugger = new Debugger(this, cpu);
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            if (((Button)sender).Tag.Equals("<prgm>")) ui.Key_Prgm();
            if (((Button)sender).Tag.Equals("<user>")) ui.Key_User();
            if (((Button)sender).Tag.Equals("<on>"))
            {
                cpu.Save();
                Application.Exit();
            }
        }

        public void Print(string line)
        {
            PrinterOutput.AppendText(line + "\r\n");
        }

        public void Alpha(bool b)
        {
            A_Alpha.Visible = b;
        }

        public void Flag_0(bool b)
        {
            A_0.Visible = b;
        }

        public void Flag_1(bool b)
        {
            A_1.Visible = b;
        }

        public void Flag_2(bool b)
        {
            A_2.Visible = b;
        }

        public void Flag_3(bool b)
        {
            A_3.Visible = b;
        }

        public void Flag_4(bool b)
        {
            A_4.Visible = b;
        }

        public void G(bool b)
        {
            A_G.Visible = b;
        }

        public void Prog(bool b)
        {
            A_Prog.Visible = b;
        }

        public void Rad(bool b)
        {
            A_Rad.Visible = b;
        }

        public void Shift(bool b)
        {
            A_Shift.Visible = b;
        }

        public void User(bool b)
        {
            A_User.Visible = b;
            if (b) EnterUser();
            else ExitUser();
        }

        public void Display(string msg)
        {
            int i;
            int p;
            char c;
            i = 0;
            p = 0;
            for (i = 0; i < msg.Length; i++)
            {
                if (p < 24)
                {
                    c = msg[i];
                    if (c > 129) c = (char)2;
                    if (c == '.' || c == ',' || c == ':' || c == ';')
                    {
                        if ((p & 1) == 0)
                        {
                            cells[p / 2].Image = imageList1.Images[32];
                            p++;
                        }
                        if (p < 24)
                        {
                            if (c == '.') semis[p / 2].Image = imageList2.Images[2];
                            if (c == ',') semis[p / 2].Image = imageList2.Images[1];
                            if (c == ':') semis[p / 2].Image = imageList2.Images[3];
                            if (c == ';') semis[p / 2].Image = imageList2.Images[4];
                        }
                    }
                    else
                    {
                        if ((p & 1) == 1)
                        {
                            semis[p / 2].Image = imageList2.Images[0];
                            p++;
                        }
                        if (p < 24)
                        {
                            cells[p / 2].Image = imageList1.Images[c];
                        }
                    }
                    p++;
                }
            }
            while (p < 24)
            {
                if ((p & 1) == 0) cells[p / 2].Image = imageList1.Images[32];
                if ((p & 1) == 1) semis[p / 2].Image = imageList2.Images[0];
                p++;
            }
        }

        private void b_Alpha_Click(object sender, EventArgs e)
        {
            //           cpu.KeyAlpha();
            ui.Key_Alpha();
        }

        private void ButtonDown(object sender, MouseEventArgs e)
        {
            ui.ButtonDown((string)((Button)sender).Tag);
        }

        private void ButtonUp(object sender, MouseEventArgs e)
        {
           ui.ButtonUp((string)((Button)sender).Tag);
        }

        private void b_ShiftClick(object sender, EventArgs e)
        {
            ui.Key_Shift();
        }

        private void PrintButton_Click(object sender, EventArgs e)
        {
            cpu.Print();
        }

        private void AdvButton_Click(object sender, EventArgs e)
        {
            cpu.Adv();
        }

        private void b_Peripherals_Click(object sender, EventArgs e)
        {
            if (b_Peripherals.Text.Equals("-->"))
            {
                this.Width = 680;
                b_Peripherals.Text = "<--";
            }
            else
            {
                this.Width = 370;
                b_Peripherals.Text = "-->";
            }
        }

        private void DisplayTimer_Tick(object sender, EventArgs e)
        {
            cpu.Tick();
        }

        public bool PrintToFile()
        {
            return cb_PrintToFile.Checked;
        }

        public bool DisplayTimerEnabled()
        {
            return DisplayTimer.Enabled;
        }

        public void DisplayTimerEnabled(bool b)
        {
            DisplayTimer.Enabled = b;
        }

        public char PrinterMode()
        {
            if (pm_Man.Checked) return 'M';
            if (pm_Trace.Checked) return 'T';
            if (pm_Norm.Checked) return 'N';
            return ' ';
        }

        public bool PrinterOn()
        {
            return PrinterPowerOn.Checked;
        }

        public void ToPrinter(string line)
        {
            cpu.printer.Print(line);
        }

        private void DebugButton_Click(object sender, EventArgs e)
        {
            this.Width = 1194;
        }

        private void StackButton_Click(object sender, EventArgs e)
        {
            debugger.ShowStatRegs(true);
        }

        public void DebugPrint(string line)
        {
            DebugOutput.AppendText(line + "\r\n");
        }

        private void StatButton_Click(object sender, EventArgs e)
        {
            debugger.ShowStatRegs(false);
        }

        private void FlagsButton_Click(object sender, EventArgs e)
        {
            debugger.ShowFlags();
        }

        private void InfoButton_Click(object sender, EventArgs e)
        {
            debugger.ShowInfo();
        }

        private void DregsButton_Click(object sender, EventArgs e)
        {
            debugger.ShowDregs(Convert.ToInt32(DregsFrom.Text), Convert.ToInt32(DregsTo.Text));
        }

        public int FromHex(string hex)
        {
            int ret;
            ret = 0;
            foreach (char c in hex)
            {
                if (c >= '0' && c <= '9') ret = (ret * 16) + (c - '0');
                if (c >= 'A' && c <= 'F') ret = (ret * 16) + (c - 55);
                if (c >= 'a' && c <= 'f') ret = (ret * 16) + (c - 87);
            }
            return ret;
        }
        private void RegsButton_Click(object sender, EventArgs e)
        {
            debugger.ShowRegs(FromHex(RegsFrom.Text), FromHex(RegsTo.Text));
        }

        private void RunTimer_Tick(object sender, EventArgs e)
        {
            cpu.RunTick();
        }

        public bool Fast()
        {
            return cb_Fast.Checked;
        }

        public void RunTimerEnabled(bool b)
        {
            RunTimer.Enabled = b;
        }

        private void RamClearButton_Click(object sender, EventArgs e)
        {
            if (AllowClear.Checked) cpu.RamClear();
            AllowClear.Checked = false;
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            cpu.Reset();
        }

        private void ByteJumper_Click(object sender, EventArgs e)
        {
            SynthDialog dialog = new SynthDialog();
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (dialog.ByteJumper() != 0) cpu.AssignKey(0xf1, 0x41, dialog.ByteJumper());    // Byte Jumper
                if (dialog.RclB() != 0) cpu.AssignKey(0x90, 0x7c, dialog.RclB());                // RCL b
                if (dialog.RclC() != 0) cpu.AssignKey(0x90, 0x7d, dialog.RclC());                // RCL c
                if (dialog.RclD() != 0) cpu.AssignKey(0x90, 0x7e, dialog.RclD());                // RCL d
                if (dialog.RclE() != 0) cpu.AssignKey(0x90, 0x7f, dialog.RclE());                // RCL e
                if (dialog.RclM() != 0) cpu.AssignKey(0x90, 0x75, dialog.RclM());                // RCL M
                if (dialog.RclN() != 0) cpu.AssignKey(0x90, 0x76, dialog.RclN());                // RCL N
                if (dialog.RclO() != 0) cpu.AssignKey(0x90, 0x77, dialog.RclO());                // RCL O
                if (dialog.RclP() != 0) cpu.AssignKey(0x90, 0x78, dialog.RclP());                // RCL P
                if (dialog.RclQ() != 0) cpu.AssignKey(0x90, 0x79, dialog.RclQ());                // RCL Q
                if (dialog.StoB() != 0) cpu.AssignKey(0x91, 0x7c, dialog.StoB());                // Sto b
                if (dialog.StoC() != 0) cpu.AssignKey(0x91, 0x7d, dialog.StoC());                // Sto c
                if (dialog.StoD() != 0) cpu.AssignKey(0x91, 0x7e, dialog.StoD());                // Sto d
                if (dialog.StoE() != 0) cpu.AssignKey(0x91, 0x7f, dialog.StoE());                // Sto e
                if (dialog.StoM() != 0) cpu.AssignKey(0x91, 0x75, dialog.StoM());                // Sto M
                if (dialog.StoN() != 0) cpu.AssignKey(0x91, 0x76, dialog.StoN());                // Sto N
                if (dialog.StoO() != 0) cpu.AssignKey(0x91, 0x77, dialog.StoO());                // Sto O
                if (dialog.StoP() != 0) cpu.AssignKey(0x91, 0x78, dialog.StoP());                // Sto P
                if (dialog.StoQ() != 0) cpu.AssignKey(0x91, 0x79, dialog.StoQ());                // Sto Q
                if (dialog.XexB() != 0) cpu.AssignKey(0xce, 0x7c, dialog.XexB());                // X<> b
                if (dialog.XexC() != 0) cpu.AssignKey(0xce, 0x7d, dialog.XexC());                // X<> c
                if (dialog.XexD() != 0) cpu.AssignKey(0xce, 0x7e, dialog.XexD());                // X<> d
                if (dialog.XexE() != 0) cpu.AssignKey(0xce, 0x7f, dialog.XexE());                // X<> e
                if (dialog.XexM() != 0) cpu.AssignKey(0xce, 0x75, dialog.XexM());                // X<> M
                if (dialog.XexN() != 0) cpu.AssignKey(0xce, 0x76, dialog.XexN());                // X<> N
                if (dialog.XexO() != 0) cpu.AssignKey(0xce, 0x77, dialog.XexO());                // X<> O
                if (dialog.XexP() != 0) cpu.AssignKey(0xce, 0x78, dialog.XexP());                // X<> P
                if (dialog.XexQ() != 0) cpu.AssignKey(0xce, 0x79, dialog.XexQ());                // X<> Q
                if (dialog.HmsMinus() != 0) cpu.AssignKey(0x04, 0x4a, dialog.HmsMinus());        // HMS-
                if (dialog.Del() != 0) cpu.AssignKey(0x04, 0x02, dialog.Del());                  // DEL
                if (dialog.Pack() != 0) cpu.AssignKey(0x04, 0x0a, dialog.Pack());                // PACK
                DebugOutput.AppendText("Synthetic Key Assignments made\r\n");
            }
        }

        private void CardButton_Click(object sender, EventArgs e)
        {
            DialogResult result;
            if (cpu.FlagSet(52))
            {
                result = SaveCardDialog.ShowDialog(this);
                if (result == DialogResult.OK) cpu.Card(SaveCardDialog.FileName);
            }
            else
            {
                result = LoadCardDialog.ShowDialog(this);
                if (result == DialogResult.OK) cpu.Card(LoadCardDialog.FileName);
            }
        }

        public string LoadCard()
        {
            DialogResult result;
            result = LoadCardDialog.ShowDialog();
            if (result == DialogResult.OK)
                return LoadCardDialog.FileName;
            return null;
        }

        public string SaveCard()
        {
            DialogResult result;
            result = SaveCardDialog.ShowDialog();
            if (result == DialogResult.OK)
                return SaveCardDialog.FileName;
            return null;
        }

        private void AllowClear_CheckedChanged(object sender, EventArgs e)
        {
            RamClearButton.Visible = AllowClear.Checked;
        }

        private void TapeButton_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result = SaveCardDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                cpu.tapeDrive.OpenTapeDrive(SaveCardDialog.FileName);
            }
        }

        public void ExitUser()
        {
            B_SumPlus.Text = "E+";
            b_Recip.Text = "1/X";
            b_Sqrt.Text = "SQRT";
            b_Log.Text = "LOG";
            b_Ln.Text = "LN";
            b_XexY.Text = "X<>Y";
            b_Rdn.Text = "RDN";
            b_Sin.Text = "SIN";
            b_Cos.Text = "COS";
            b_Tan.Text = "TAN";
            b_Xeq.Text = "XEQ";
            b_Sto.Text = "STO";
            b_Rcl.Text = "RCL";
            b_Sst.Text = "SST";
            b_Enter.Text = "ENTER";
            b_Chs.Text = "CHS";
            b_Eex.Text = "EEX";
            b_Clx.Text = "<--";
            b_Minus.Text = "-";
            b_7.Text = "7";
            b_8.Text = "8";
            b_9.Text = "9";
            b_Plus.Text = "+";
            b_4.Text = "4";
            b_5.Text = "5";
            b_6.Text = "6";
            b_Mult.Text = "*";
            b_1.Text = "1";
            b_2.Text = "2";
            b_3.Text = "3";
            b_Div.Text = "/";
            b_0.Text = "0";
            b_Dot.Text = ".";
            b_Rs.Text = "R/S";

            l_SumMinus.Text = "E-";
            l_Recip.Text = "Y^X";
            l_Sqrt.Text = "X^2";
            l_Log.Text = "10^X";
            l_Ln.Text = "E^X";
            l_XexY.Text = "CLE";
            l_Rdn.Text = "%";
            l_Sin.Text = "ASIN";
            l_Cos.Text = "ACOS";
            l_Tan.Text = "ATAN";
            l_Xeq.Text = "ASN";
            l_Sto.Text = "LBL";
            l_Rcl.Text = "GTO";
            l_Sst.Text = "BST";
            l_Enter.Text = "CATALOG";
            l_Chs.Text = "ISG";
            l_Eex.Text = "RTN";
            l_Clx.Text = "CLX/A";
            l_Minus.Text = "X=Y?";
            l_7.Text = "SF";
            l_8.Text = "CF";
            l_9.Text = "FS?";
            l_Plus.Text = "X<=Y?";
            l_4.Text = "BEEP";
            l_5.Text = "P-R";
            l_6.Text = "R-P";
            l_Mult.Text = "X>Y?";
            l_1.Text = "FIX";
            l_2.Text = "SCI";
            l_3.Text = "ENG";
            l_Div.Text = "X>0?";
            l_0.Text = "PI";
            l_Dot.Text = "LASTX";
            l_Rs.Text = "VIEW";
        }


        public string SearchKaPrograms(byte keycode)
        {
            int adr;
            int dst;
            int i;
            int flag;
            int l;
            string ret;
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
                adr -= 2;
                l = cpu.ram[adr] & 0x0f;
                l--;
                adr -= 2;
                ret = "";
                for (i = 0; i < l; i++) ret += ((char)cpu.ram[adr--]).ToString();
                return ret;
            }
            return "";
        }

        public string GetUserLabel(byte keycode)
        {
            int i;
            byte b1, b2;
            string ret;

            i = cpu.SearchKaRegisters(keycode);
            if (i >= 0)
            {
                b2 = cpu.ram[i];
                b1 = cpu.ram[i + 1];
                if (b1 >= 0xf0)
                {
                    ret = "BJ";
                }
                else if (b1 == 0x04)
                {
                    if (b2 < 0x10)
                    {
                        switch (b2)
                        {
                            case 0x00: ret = "CAT"; break;
                            case 0x01: ret = "@c"; break;
                            case 0x02: ret = "DEL"; break;
                            case 0x03: ret = "COPY"; break;
                            case 0x04: ret = "CLP"; break;
                            case 0x05: ret = "R/S"; break;
                            case 0x06: ret = "SIZE"; break;
                            case 0x07: ret = "BST"; break;
                            case 0x08: ret = "SST"; break;
                            case 0x09: ret = "ON"; break;
                            case 0x0a: ret = "PACK"; break;
                            case 0x0b: ret = "<--"; break;
                            case 0x0c: ret = "ALPHA"; break;
                            case 0x0d: ret = "2__"; break;
                            case 0x0e: ret = "SHIFT"; break;
                            case 0x0f: ret = "ASN"; break;
                            default: ret = ""; break;
                        }
                        return ret;
                    }
                    ret = cpu.reverse[b2].name;
                }
                else
                {
                    ret = cpu.reverse[b1].name + " ";
                    switch (b2)
                    {
                        case 0x70: ret += "T"; break;
                        case 0x71: ret += "Z"; break;
                        case 0x72: ret += "Y"; break;
                        case 0x73: ret += "X"; break;
                        case 0x74: ret += "L"; break;
                        case 0x75: ret += "M"; break;
                        case 0x76: ret += "N"; break;
                        case 0x77: ret += "O"; break;
                        case 0x78: ret += "P"; break;
                        case 0x79: ret += "Q"; break;
                        case 0x7a: ret += "|-"; break;
                        case 0x7b: ret += "a"; break;
                        case 0x7c: ret += "b"; break;
                        case 0x7d: ret += "c"; break;
                        case 0x7e: ret += "d"; break;
                        case 0x7f: ret += "e"; break;
                        default: ret += $"{b2:d2}"; break;
                    }
                }
                return ret;
            }

            ret = SearchKaPrograms(keycode);
            if (ret.Length > 0)
            {
                if (ret.Length > 5) { ret = ret.Substring(0, 5); }
                return ret;
            }

                i = 0;
            while (ui.keys[i].keycode != keycode && ui.keys[i].skeycode != keycode && ui.keys[i].keycode != 0xff) i++;
            if (ui.keys[i].keycode != 0xff)
            {
                if (ui.keys[i].keycode == keycode) i = ui.keys[i].cmd; else i = ui.keys[i].scmd;
                ret = cpu.catalog[i].name;
                if (ret.Length > 5) ret = ret.Substring(0, 5);
                return ret;
            }
            return "";
        }

        public void EnterUser()
        {
            if (cpu.GetKaFlag(0x01)) B_SumPlus.Text = GetUserLabel(0x01);
            if (cpu.GetKaFlag(0x11)) b_Recip.Text = GetUserLabel(0x11);
            if (cpu.GetKaFlag(0x21)) b_Sqrt.Text = GetUserLabel(0x21);
            if (cpu.GetKaFlag(0x31)) b_Log.Text = GetUserLabel(0x31);
            if (cpu.GetKaFlag(0x41)) b_Ln.Text = GetUserLabel(0x41);

            if (cpu.GetKaFlag(0x02)) b_XexY.Text = GetUserLabel(0x02);
            if (cpu.GetKaFlag(0x12)) b_Rdn.Text = GetUserLabel(0x12);
            if (cpu.GetKaFlag(0x22)) b_Sin.Text = GetUserLabel(0x22);
            if (cpu.GetKaFlag(0x32)) b_Cos.Text = GetUserLabel(0x32);
            if (cpu.GetKaFlag(0x42)) b_Tan.Text = GetUserLabel(0x42);

            if (cpu.GetKaFlag(0x13)) b_Xeq.Text = GetUserLabel(0x13);
            if (cpu.GetKaFlag(0x23)) b_Sto.Text = GetUserLabel(0x23);
            if (cpu.GetKaFlag(0x33)) b_Rcl.Text = GetUserLabel(0x33);
            if (cpu.GetKaFlag(0x43)) b_Sst.Text = GetUserLabel(0x43);

            if (cpu.GetKaFlag(0x04)) b_Enter.Text = GetUserLabel(0x04);
            if (cpu.GetKaFlag(0x24)) b_Chs.Text = GetUserLabel(0x24);
            if (cpu.GetKaFlag(0x34)) b_Eex.Text = GetUserLabel(0x34);
            if (cpu.GetKaFlag(0x44)) b_Clx.Text = GetUserLabel(0x44);

            if (cpu.GetKaFlag(0x05)) b_Minus.Text = GetUserLabel(0x05);
            if (cpu.GetKaFlag(0x15)) b_7.Text = GetUserLabel(0x15);
            if (cpu.GetKaFlag(0x25)) b_8.Text = GetUserLabel(0x25);
            if (cpu.GetKaFlag(0x35)) b_9.Text = GetUserLabel(0x35);

            if (cpu.GetKaFlag(0x06)) b_Plus.Text = GetUserLabel(0x06);
            if (cpu.GetKaFlag(0x16)) b_4.Text = GetUserLabel(0x16);
            if (cpu.GetKaFlag(0x26)) b_5.Text = GetUserLabel(0x26);
            if (cpu.GetKaFlag(0x36)) b_6.Text = GetUserLabel(0x36);

            if (cpu.GetKaFlag(0x07)) b_Mult.Text = GetUserLabel(0x07);
            if (cpu.GetKaFlag(0x17)) b_1.Text = GetUserLabel(0x17);
            if (cpu.GetKaFlag(0x27)) b_2.Text = GetUserLabel(0x27);
            if (cpu.GetKaFlag(0x37)) b_3.Text = GetUserLabel(0x37);

            if (cpu.GetKaFlag(0x08)) b_Div.Text = GetUserLabel(0x08);
            if (cpu.GetKaFlag(0x18)) b_0.Text = GetUserLabel(0x18);
            if (cpu.GetKaFlag(0x28)) b_Dot.Text = GetUserLabel(0x28);
            if (cpu.GetKaFlag(0x38)) b_Rs.Text = GetUserLabel(0x38);

            if (cpu.GetKaFlag(0x09)) l_SumMinus.Text = GetUserLabel(0x09);
            if (cpu.GetKaFlag(0x19)) l_Recip.Text = GetUserLabel(0x19);
            if (cpu.GetKaFlag(0x29)) l_Sqrt.Text = GetUserLabel(0x29);
            if (cpu.GetKaFlag(0x39)) l_Log.Text = GetUserLabel(0x39);
            if (cpu.GetKaFlag(0x49)) l_Ln.Text = GetUserLabel(0x49);

            if (cpu.GetKaFlag(0x0a)) l_XexY.Text = GetUserLabel(0x0a);
            if (cpu.GetKaFlag(0x1a)) l_Rdn.Text = GetUserLabel(0x1a);
            if (cpu.GetKaFlag(0x2a)) l_Sin.Text = GetUserLabel(0x2a);
            if (cpu.GetKaFlag(0x3a)) l_Cos.Text = GetUserLabel(0x3a);
            if (cpu.GetKaFlag(0x4a)) l_Tan.Text = GetUserLabel(0x4a);

            if (cpu.GetKaFlag(0x1b)) l_Xeq.Text = GetUserLabel(0x1b);
            if (cpu.GetKaFlag(0x2b)) l_Sto.Text = GetUserLabel(0x2b);
            if (cpu.GetKaFlag(0x3b)) l_Rcl.Text = GetUserLabel(0x3b);
            if (cpu.GetKaFlag(0x4b)) l_Sst.Text = GetUserLabel(0x4b);

            if (cpu.GetKaFlag(0x0c)) l_Enter.Text = GetUserLabel(0x0c);
            if (cpu.GetKaFlag(0x2c)) l_Chs.Text = GetUserLabel(0x2c);
            if (cpu.GetKaFlag(0x3c)) l_Eex.Text = GetUserLabel(0x3c);
            if (cpu.GetKaFlag(0x4c)) l_Clx.Text = GetUserLabel(0x4c);

            if (cpu.GetKaFlag(0x0d)) l_Minus.Text = GetUserLabel(0x0d);
            if (cpu.GetKaFlag(0x1d)) l_7.Text = GetUserLabel(0x1d);
            if (cpu.GetKaFlag(0x2d)) l_8.Text = GetUserLabel(0x2d);
            if (cpu.GetKaFlag(0x3d)) l_9.Text = GetUserLabel(0x3d);

            if (cpu.GetKaFlag(0x0e)) l_Plus.Text = GetUserLabel(0x0e);
            if (cpu.GetKaFlag(0x1e)) l_4.Text = GetUserLabel(0x1e);
            if (cpu.GetKaFlag(0x2e)) l_5.Text = GetUserLabel(0x2e);
            if (cpu.GetKaFlag(0x3e)) l_6.Text = GetUserLabel(0x3e);

            if (cpu.GetKaFlag(0x0f)) l_Mult.Text = GetUserLabel(0x0f);
            if (cpu.GetKaFlag(0x1f)) l_1.Text = GetUserLabel(0x1f);
            if (cpu.GetKaFlag(0x2f)) l_2.Text = GetUserLabel(0x2f);
            if (cpu.GetKaFlag(0x3f)) l_3.Text = GetUserLabel(0x3f);

            if (cpu.GetKaFlag(0x10)) l_Div.Text = GetUserLabel(0x10);
            if (cpu.GetKaFlag(0x20)) l_0.Text = GetUserLabel(0x20);
            if (cpu.GetKaFlag(0x30)) l_Dot.Text = GetUserLabel(0x30);
            if (cpu.GetKaFlag(0x40)) l_Rs.Text = GetUserLabel(0x40);
        }


    }
}
