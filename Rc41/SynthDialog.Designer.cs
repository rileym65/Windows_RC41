namespace Rc41
{
    partial class SynthDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            cb_ByteJumper = new CheckBox();
            OkButton = new Button();
            CancelButton = new Button();
            ByteJumperKey = new ComboBox();
            ByteJumperShift = new CheckBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            RclDShift = new CheckBox();
            RclDKey = new ComboBox();
            cb_RclD = new CheckBox();
            StoDShift = new CheckBox();
            StoDKey = new ComboBox();
            cb_StoD = new CheckBox();
            StoMShift = new CheckBox();
            StoMKey = new ComboBox();
            cb_StoM = new CheckBox();
            RclMShift = new CheckBox();
            RclMKey = new ComboBox();
            cb_RclM = new CheckBox();
            StoNShift = new CheckBox();
            StoNKey = new ComboBox();
            cb_StoN = new CheckBox();
            RclNShift = new CheckBox();
            RclNKey = new ComboBox();
            cb_RclN = new CheckBox();
            XexNShift = new CheckBox();
            XexNKey = new ComboBox();
            cb_XexN = new CheckBox();
            XexMShift = new CheckBox();
            XexMKey = new ComboBox();
            cb_XexM = new CheckBox();
            XexDShift = new CheckBox();
            XexDKey = new ComboBox();
            cb_XexD = new CheckBox();
            XexCShift = new CheckBox();
            XexCKey = new ComboBox();
            cb_XexC = new CheckBox();
            RclCShift = new CheckBox();
            RclCKey = new ComboBox();
            cb_RclC = new CheckBox();
            StoBShift = new CheckBox();
            StoBKey = new ComboBox();
            cb_StoB = new CheckBox();
            RclBShift = new CheckBox();
            RclBKey = new ComboBox();
            cb_RclB = new CheckBox();
            HmsMShift = new CheckBox();
            HmsMKey = new ComboBox();
            cb_HmsM = new CheckBox();
            StoEShift = new CheckBox();
            StoEKey = new ComboBox();
            cb_StoE = new CheckBox();
            RclEShift = new CheckBox();
            RclEKey = new ComboBox();
            cb_RclE = new CheckBox();
            DelShift = new CheckBox();
            DelKey = new ComboBox();
            cb_Del = new CheckBox();
            PackShift = new CheckBox();
            PackKey = new ComboBox();
            cb_Pack = new CheckBox();
            StoCShift = new CheckBox();
            StoCKey = new ComboBox();
            cb_StoC = new CheckBox();
            XexBShift = new CheckBox();
            XexBKey = new ComboBox();
            cb_XexB = new CheckBox();
            XexEShift = new CheckBox();
            XexEKey = new ComboBox();
            cb_XexE = new CheckBox();
            XexOShift = new CheckBox();
            XexOKey = new ComboBox();
            cb_XexO = new CheckBox();
            StoOShift = new CheckBox();
            StoOKey = new ComboBox();
            cb_StoO = new CheckBox();
            RclOShift = new CheckBox();
            RclOKey = new ComboBox();
            cb_RclO = new CheckBox();
            XexPShift = new CheckBox();
            XexPKey = new ComboBox();
            cb_XexP = new CheckBox();
            StoPShift = new CheckBox();
            StoPKey = new ComboBox();
            cb_StoP = new CheckBox();
            RclPShift = new CheckBox();
            RclPKey = new ComboBox();
            cb_RclP = new CheckBox();
            XexQShift = new CheckBox();
            XexQKey = new ComboBox();
            cb_XexQ = new CheckBox();
            StoQShift = new CheckBox();
            StoQKey = new ComboBox();
            cb_StoQ = new CheckBox();
            RclQShift = new CheckBox();
            RclQKey = new ComboBox();
            cb_RclQ = new CheckBox();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            SuspendLayout();
            // 
            // cb_ByteJumper
            // 
            cb_ByteJumper.AutoSize = true;
            cb_ByteJumper.Checked = true;
            cb_ByteJumper.CheckState = CheckState.Checked;
            cb_ByteJumper.Location = new Point(16, 41);
            cb_ByteJumper.Name = "cb_ByteJumper";
            cb_ByteJumper.Size = new Size(91, 19);
            cb_ByteJumper.TabIndex = 0;
            cb_ByteJumper.Text = "Byte Jumper";
            cb_ByteJumper.UseVisualStyleBackColor = true;
            // 
            // OkButton
            // 
            OkButton.Location = new Point(16, 382);
            OkButton.Name = "OkButton";
            OkButton.Size = new Size(75, 23);
            OkButton.TabIndex = 1;
            OkButton.Text = "OK";
            OkButton.UseVisualStyleBackColor = true;
            OkButton.Click += OkButton_Click;
            // 
            // CancelButton
            // 
            CancelButton.Location = new Point(553, 382);
            CancelButton.Name = "CancelButton";
            CancelButton.Size = new Size(75, 23);
            CancelButton.TabIndex = 2;
            CancelButton.Text = "Cancel";
            CancelButton.UseVisualStyleBackColor = true;
            CancelButton.Click += CancelButton_Click;
            // 
            // ByteJumperKey
            // 
            ByteJumperKey.DropDownStyle = ComboBoxStyle.DropDownList;
            ByteJumperKey.FormattingEnabled = true;
            ByteJumperKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            ByteJumperKey.Location = new Point(139, 39);
            ByteJumperKey.Name = "ByteJumperKey";
            ByteJumperKey.Size = new Size(64, 23);
            ByteJumperKey.TabIndex = 3;
            // 
            // ByteJumperShift
            // 
            ByteJumperShift.AutoSize = true;
            ByteJumperShift.Location = new Point(113, 43);
            ByteJumperShift.Name = "ByteJumperShift";
            ByteJumperShift.Size = new Size(15, 14);
            ByteJumperShift.TabIndex = 4;
            ByteJumperShift.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(102, 21);
            label1.Name = "label1";
            label1.Size = new Size(31, 15);
            label1.TabIndex = 5;
            label1.Text = "Shift";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(139, 21);
            label2.Name = "label2";
            label2.Size = new Size(26, 15);
            label2.TabIndex = 6;
            label2.Text = "Key";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(33, 21);
            label3.Name = "label3";
            label3.Size = new Size(54, 15);
            label3.TabIndex = 7;
            label3.Text = "Function";
            // 
            // RclDShift
            // 
            RclDShift.AutoSize = true;
            RclDShift.Location = new Point(113, 130);
            RclDShift.Name = "RclDShift";
            RclDShift.Size = new Size(15, 14);
            RclDShift.TabIndex = 10;
            RclDShift.UseVisualStyleBackColor = true;
            // 
            // RclDKey
            // 
            RclDKey.DropDownStyle = ComboBoxStyle.DropDownList;
            RclDKey.FormattingEnabled = true;
            RclDKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            RclDKey.Location = new Point(139, 126);
            RclDKey.Name = "RclDKey";
            RclDKey.Size = new Size(64, 23);
            RclDKey.TabIndex = 9;
            // 
            // cb_RclD
            // 
            cb_RclD.AutoSize = true;
            cb_RclD.Checked = true;
            cb_RclD.CheckState = CheckState.Checked;
            cb_RclD.Location = new Point(16, 128);
            cb_RclD.Name = "cb_RclD";
            cb_RclD.Size = new Size(57, 19);
            cb_RclD.TabIndex = 8;
            cb_RclD.Text = "RCL d";
            cb_RclD.UseVisualStyleBackColor = true;
            // 
            // StoDShift
            // 
            StoDShift.AutoSize = true;
            StoDShift.Location = new Point(326, 130);
            StoDShift.Name = "StoDShift";
            StoDShift.Size = new Size(15, 14);
            StoDShift.TabIndex = 13;
            StoDShift.UseVisualStyleBackColor = true;
            // 
            // StoDKey
            // 
            StoDKey.DropDownStyle = ComboBoxStyle.DropDownList;
            StoDKey.FormattingEnabled = true;
            StoDKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            StoDKey.Location = new Point(352, 126);
            StoDKey.Name = "StoDKey";
            StoDKey.Size = new Size(64, 23);
            StoDKey.TabIndex = 12;
            // 
            // cb_StoD
            // 
            cb_StoD.AutoSize = true;
            cb_StoD.Checked = true;
            cb_StoD.CheckState = CheckState.Checked;
            cb_StoD.Location = new Point(229, 128);
            cb_StoD.Name = "cb_StoD";
            cb_StoD.Size = new Size(56, 19);
            cb_StoD.TabIndex = 11;
            cb_StoD.Text = "STO d";
            cb_StoD.UseVisualStyleBackColor = true;
            // 
            // StoMShift
            // 
            StoMShift.AutoSize = true;
            StoMShift.Location = new Point(326, 188);
            StoMShift.Name = "StoMShift";
            StoMShift.Size = new Size(15, 14);
            StoMShift.TabIndex = 19;
            StoMShift.UseVisualStyleBackColor = true;
            // 
            // StoMKey
            // 
            StoMKey.DropDownStyle = ComboBoxStyle.DropDownList;
            StoMKey.FormattingEnabled = true;
            StoMKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            StoMKey.Location = new Point(352, 184);
            StoMKey.Name = "StoMKey";
            StoMKey.Size = new Size(64, 23);
            StoMKey.TabIndex = 18;
            // 
            // cb_StoM
            // 
            cb_StoM.AutoSize = true;
            cb_StoM.Checked = true;
            cb_StoM.CheckState = CheckState.Checked;
            cb_StoM.Location = new Point(229, 186);
            cb_StoM.Name = "cb_StoM";
            cb_StoM.Size = new Size(60, 19);
            cb_StoM.TabIndex = 17;
            cb_StoM.Text = "STO M";
            cb_StoM.UseVisualStyleBackColor = true;
            // 
            // RclMShift
            // 
            RclMShift.AutoSize = true;
            RclMShift.Location = new Point(113, 188);
            RclMShift.Name = "RclMShift";
            RclMShift.Size = new Size(15, 14);
            RclMShift.TabIndex = 16;
            RclMShift.UseVisualStyleBackColor = true;
            // 
            // RclMKey
            // 
            RclMKey.DropDownStyle = ComboBoxStyle.DropDownList;
            RclMKey.FormattingEnabled = true;
            RclMKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            RclMKey.Location = new Point(139, 184);
            RclMKey.Name = "RclMKey";
            RclMKey.Size = new Size(64, 23);
            RclMKey.TabIndex = 15;
            // 
            // cb_RclM
            // 
            cb_RclM.AutoSize = true;
            cb_RclM.Checked = true;
            cb_RclM.CheckState = CheckState.Checked;
            cb_RclM.Location = new Point(16, 186);
            cb_RclM.Name = "cb_RclM";
            cb_RclM.Size = new Size(61, 19);
            cb_RclM.TabIndex = 14;
            cb_RclM.Text = "RCL M";
            cb_RclM.UseVisualStyleBackColor = true;
            // 
            // StoNShift
            // 
            StoNShift.AutoSize = true;
            StoNShift.Location = new Point(326, 217);
            StoNShift.Name = "StoNShift";
            StoNShift.Size = new Size(15, 14);
            StoNShift.TabIndex = 25;
            StoNShift.UseVisualStyleBackColor = true;
            // 
            // StoNKey
            // 
            StoNKey.DropDownStyle = ComboBoxStyle.DropDownList;
            StoNKey.FormattingEnabled = true;
            StoNKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            StoNKey.Location = new Point(352, 213);
            StoNKey.Name = "StoNKey";
            StoNKey.Size = new Size(64, 23);
            StoNKey.TabIndex = 24;
            // 
            // cb_StoN
            // 
            cb_StoN.AutoSize = true;
            cb_StoN.Checked = true;
            cb_StoN.CheckState = CheckState.Checked;
            cb_StoN.Location = new Point(229, 215);
            cb_StoN.Name = "cb_StoN";
            cb_StoN.Size = new Size(58, 19);
            cb_StoN.TabIndex = 23;
            cb_StoN.Text = "STO N";
            cb_StoN.UseVisualStyleBackColor = true;
            // 
            // RclNShift
            // 
            RclNShift.AutoSize = true;
            RclNShift.Location = new Point(113, 217);
            RclNShift.Name = "RclNShift";
            RclNShift.Size = new Size(15, 14);
            RclNShift.TabIndex = 22;
            RclNShift.UseVisualStyleBackColor = true;
            // 
            // RclNKey
            // 
            RclNKey.DropDownStyle = ComboBoxStyle.DropDownList;
            RclNKey.FormattingEnabled = true;
            RclNKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            RclNKey.Location = new Point(139, 213);
            RclNKey.Name = "RclNKey";
            RclNKey.Size = new Size(64, 23);
            RclNKey.TabIndex = 21;
            // 
            // cb_RclN
            // 
            cb_RclN.AutoSize = true;
            cb_RclN.Checked = true;
            cb_RclN.CheckState = CheckState.Checked;
            cb_RclN.Location = new Point(16, 215);
            cb_RclN.Name = "cb_RclN";
            cb_RclN.Size = new Size(59, 19);
            cb_RclN.TabIndex = 20;
            cb_RclN.Text = "RCL N";
            cb_RclN.UseVisualStyleBackColor = true;
            // 
            // XexNShift
            // 
            XexNShift.AutoSize = true;
            XexNShift.Location = new Point(538, 217);
            XexNShift.Name = "XexNShift";
            XexNShift.Size = new Size(15, 14);
            XexNShift.TabIndex = 31;
            XexNShift.UseVisualStyleBackColor = true;
            // 
            // XexNKey
            // 
            XexNKey.DropDownStyle = ComboBoxStyle.DropDownList;
            XexNKey.FormattingEnabled = true;
            XexNKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            XexNKey.Location = new Point(564, 213);
            XexNKey.Name = "XexNKey";
            XexNKey.Size = new Size(64, 23);
            XexNKey.TabIndex = 30;
            // 
            // cb_XexN
            // 
            cb_XexN.AutoSize = true;
            cb_XexN.Checked = true;
            cb_XexN.CheckState = CheckState.Checked;
            cb_XexN.Location = new Point(441, 215);
            cb_XexN.Name = "cb_XexN";
            cb_XexN.Size = new Size(61, 19);
            cb_XexN.TabIndex = 29;
            cb_XexN.Text = "X<> N";
            cb_XexN.UseVisualStyleBackColor = true;
            // 
            // XexMShift
            // 
            XexMShift.AutoSize = true;
            XexMShift.Location = new Point(538, 188);
            XexMShift.Name = "XexMShift";
            XexMShift.Size = new Size(15, 14);
            XexMShift.TabIndex = 28;
            XexMShift.UseVisualStyleBackColor = true;
            // 
            // XexMKey
            // 
            XexMKey.DropDownStyle = ComboBoxStyle.DropDownList;
            XexMKey.FormattingEnabled = true;
            XexMKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            XexMKey.Location = new Point(564, 184);
            XexMKey.Name = "XexMKey";
            XexMKey.Size = new Size(64, 23);
            XexMKey.TabIndex = 27;
            // 
            // cb_XexM
            // 
            cb_XexM.AutoSize = true;
            cb_XexM.Checked = true;
            cb_XexM.CheckState = CheckState.Checked;
            cb_XexM.Location = new Point(441, 186);
            cb_XexM.Name = "cb_XexM";
            cb_XexM.Size = new Size(63, 19);
            cb_XexM.TabIndex = 26;
            cb_XexM.Text = "X<> M";
            cb_XexM.UseVisualStyleBackColor = true;
            // 
            // XexDShift
            // 
            XexDShift.AutoSize = true;
            XexDShift.Location = new Point(538, 130);
            XexDShift.Name = "XexDShift";
            XexDShift.Size = new Size(15, 14);
            XexDShift.TabIndex = 34;
            XexDShift.UseVisualStyleBackColor = true;
            // 
            // XexDKey
            // 
            XexDKey.DropDownStyle = ComboBoxStyle.DropDownList;
            XexDKey.FormattingEnabled = true;
            XexDKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            XexDKey.Location = new Point(564, 126);
            XexDKey.Name = "XexDKey";
            XexDKey.Size = new Size(64, 23);
            XexDKey.TabIndex = 33;
            // 
            // cb_XexD
            // 
            cb_XexD.AutoSize = true;
            cb_XexD.Checked = true;
            cb_XexD.CheckState = CheckState.Checked;
            cb_XexD.Location = new Point(441, 128);
            cb_XexD.Name = "cb_XexD";
            cb_XexD.Size = new Size(59, 19);
            cb_XexD.TabIndex = 32;
            cb_XexD.Text = "X<> d";
            cb_XexD.UseVisualStyleBackColor = true;
            // 
            // XexCShift
            // 
            XexCShift.AutoSize = true;
            XexCShift.Location = new Point(538, 101);
            XexCShift.Name = "XexCShift";
            XexCShift.Size = new Size(15, 14);
            XexCShift.TabIndex = 40;
            XexCShift.UseVisualStyleBackColor = true;
            // 
            // XexCKey
            // 
            XexCKey.DropDownStyle = ComboBoxStyle.DropDownList;
            XexCKey.FormattingEnabled = true;
            XexCKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            XexCKey.Location = new Point(564, 97);
            XexCKey.Name = "XexCKey";
            XexCKey.Size = new Size(64, 23);
            XexCKey.TabIndex = 39;
            // 
            // cb_XexC
            // 
            cb_XexC.AutoSize = true;
            cb_XexC.Checked = true;
            cb_XexC.CheckState = CheckState.Checked;
            cb_XexC.Location = new Point(441, 99);
            cb_XexC.Name = "cb_XexC";
            cb_XexC.Size = new Size(58, 19);
            cb_XexC.TabIndex = 38;
            cb_XexC.Text = "X<> c";
            cb_XexC.UseVisualStyleBackColor = true;
            // 
            // RclCShift
            // 
            RclCShift.AutoSize = true;
            RclCShift.Location = new Point(113, 101);
            RclCShift.Name = "RclCShift";
            RclCShift.Size = new Size(15, 14);
            RclCShift.TabIndex = 37;
            RclCShift.UseVisualStyleBackColor = true;
            // 
            // RclCKey
            // 
            RclCKey.DropDownStyle = ComboBoxStyle.DropDownList;
            RclCKey.FormattingEnabled = true;
            RclCKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            RclCKey.Location = new Point(139, 97);
            RclCKey.Name = "RclCKey";
            RclCKey.Size = new Size(64, 23);
            RclCKey.TabIndex = 36;
            // 
            // cb_RclC
            // 
            cb_RclC.AutoSize = true;
            cb_RclC.Checked = true;
            cb_RclC.CheckState = CheckState.Checked;
            cb_RclC.Location = new Point(16, 99);
            cb_RclC.Name = "cb_RclC";
            cb_RclC.Size = new Size(56, 19);
            cb_RclC.TabIndex = 35;
            cb_RclC.Text = "RCL c";
            cb_RclC.UseVisualStyleBackColor = true;
            // 
            // StoBShift
            // 
            StoBShift.AutoSize = true;
            StoBShift.Location = new Point(326, 72);
            StoBShift.Name = "StoBShift";
            StoBShift.Size = new Size(15, 14);
            StoBShift.TabIndex = 46;
            StoBShift.UseVisualStyleBackColor = true;
            // 
            // StoBKey
            // 
            StoBKey.DropDownStyle = ComboBoxStyle.DropDownList;
            StoBKey.FormattingEnabled = true;
            StoBKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            StoBKey.Location = new Point(352, 68);
            StoBKey.Name = "StoBKey";
            StoBKey.Size = new Size(64, 23);
            StoBKey.TabIndex = 45;
            // 
            // cb_StoB
            // 
            cb_StoB.AutoSize = true;
            cb_StoB.Checked = true;
            cb_StoB.CheckState = CheckState.Checked;
            cb_StoB.Location = new Point(229, 70);
            cb_StoB.Name = "cb_StoB";
            cb_StoB.Size = new Size(56, 19);
            cb_StoB.TabIndex = 44;
            cb_StoB.Text = "STO b";
            cb_StoB.UseVisualStyleBackColor = true;
            // 
            // RclBShift
            // 
            RclBShift.AutoSize = true;
            RclBShift.Location = new Point(113, 72);
            RclBShift.Name = "RclBShift";
            RclBShift.Size = new Size(15, 14);
            RclBShift.TabIndex = 43;
            RclBShift.UseVisualStyleBackColor = true;
            // 
            // RclBKey
            // 
            RclBKey.DropDownStyle = ComboBoxStyle.DropDownList;
            RclBKey.FormattingEnabled = true;
            RclBKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            RclBKey.Location = new Point(139, 68);
            RclBKey.Name = "RclBKey";
            RclBKey.Size = new Size(64, 23);
            RclBKey.TabIndex = 42;
            // 
            // cb_RclB
            // 
            cb_RclB.AutoSize = true;
            cb_RclB.Checked = true;
            cb_RclB.CheckState = CheckState.Checked;
            cb_RclB.Location = new Point(16, 70);
            cb_RclB.Name = "cb_RclB";
            cb_RclB.Size = new Size(57, 19);
            cb_RclB.TabIndex = 41;
            cb_RclB.Text = "RCL b";
            cb_RclB.UseVisualStyleBackColor = true;
            // 
            // HmsMShift
            // 
            HmsMShift.AutoSize = true;
            HmsMShift.Location = new Point(113, 333);
            HmsMShift.Name = "HmsMShift";
            HmsMShift.Size = new Size(15, 14);
            HmsMShift.TabIndex = 49;
            HmsMShift.UseVisualStyleBackColor = true;
            // 
            // HmsMKey
            // 
            HmsMKey.DropDownStyle = ComboBoxStyle.DropDownList;
            HmsMKey.FormattingEnabled = true;
            HmsMKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            HmsMKey.Location = new Point(139, 329);
            HmsMKey.Name = "HmsMKey";
            HmsMKey.Size = new Size(64, 23);
            HmsMKey.TabIndex = 48;
            // 
            // cb_HmsM
            // 
            cb_HmsM.AutoSize = true;
            cb_HmsM.Checked = true;
            cb_HmsM.CheckState = CheckState.Checked;
            cb_HmsM.Location = new Point(16, 331);
            cb_HmsM.Name = "cb_HmsM";
            cb_HmsM.Size = new Size(57, 19);
            cb_HmsM.TabIndex = 47;
            cb_HmsM.Text = "HMS-";
            cb_HmsM.UseVisualStyleBackColor = true;
            // 
            // StoEShift
            // 
            StoEShift.AutoSize = true;
            StoEShift.Location = new Point(326, 159);
            StoEShift.Name = "StoEShift";
            StoEShift.Size = new Size(15, 14);
            StoEShift.TabIndex = 55;
            StoEShift.UseVisualStyleBackColor = true;
            // 
            // StoEKey
            // 
            StoEKey.DropDownStyle = ComboBoxStyle.DropDownList;
            StoEKey.FormattingEnabled = true;
            StoEKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            StoEKey.Location = new Point(352, 155);
            StoEKey.Name = "StoEKey";
            StoEKey.Size = new Size(64, 23);
            StoEKey.TabIndex = 54;
            // 
            // cb_StoE
            // 
            cb_StoE.AutoSize = true;
            cb_StoE.Checked = true;
            cb_StoE.CheckState = CheckState.Checked;
            cb_StoE.Location = new Point(229, 157);
            cb_StoE.Name = "cb_StoE";
            cb_StoE.Size = new Size(55, 19);
            cb_StoE.TabIndex = 53;
            cb_StoE.Text = "STO e";
            cb_StoE.UseVisualStyleBackColor = true;
            // 
            // RclEShift
            // 
            RclEShift.AutoSize = true;
            RclEShift.Location = new Point(113, 159);
            RclEShift.Name = "RclEShift";
            RclEShift.Size = new Size(15, 14);
            RclEShift.TabIndex = 52;
            RclEShift.UseVisualStyleBackColor = true;
            // 
            // RclEKey
            // 
            RclEKey.DropDownStyle = ComboBoxStyle.DropDownList;
            RclEKey.FormattingEnabled = true;
            RclEKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            RclEKey.Location = new Point(139, 155);
            RclEKey.Name = "RclEKey";
            RclEKey.Size = new Size(64, 23);
            RclEKey.TabIndex = 51;
            // 
            // cb_RclE
            // 
            cb_RclE.AutoSize = true;
            cb_RclE.Checked = true;
            cb_RclE.CheckState = CheckState.Checked;
            cb_RclE.Location = new Point(16, 157);
            cb_RclE.Name = "cb_RclE";
            cb_RclE.Size = new Size(56, 19);
            cb_RclE.TabIndex = 50;
            cb_RclE.Text = "RCL e";
            cb_RclE.UseVisualStyleBackColor = true;
            // 
            // DelShift
            // 
            DelShift.AutoSize = true;
            DelShift.Location = new Point(326, 333);
            DelShift.Name = "DelShift";
            DelShift.Size = new Size(15, 14);
            DelShift.TabIndex = 58;
            DelShift.UseVisualStyleBackColor = true;
            // 
            // DelKey
            // 
            DelKey.DropDownStyle = ComboBoxStyle.DropDownList;
            DelKey.FormattingEnabled = true;
            DelKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            DelKey.Location = new Point(352, 329);
            DelKey.Name = "DelKey";
            DelKey.Size = new Size(64, 23);
            DelKey.TabIndex = 57;
            // 
            // cb_Del
            // 
            cb_Del.AutoSize = true;
            cb_Del.Checked = true;
            cb_Del.CheckState = CheckState.Checked;
            cb_Del.Location = new Point(229, 331);
            cb_Del.Name = "cb_Del";
            cb_Del.Size = new Size(46, 19);
            cb_Del.TabIndex = 56;
            cb_Del.Text = "DEL";
            cb_Del.UseVisualStyleBackColor = true;
            // 
            // PackShift
            // 
            PackShift.AutoSize = true;
            PackShift.Location = new Point(538, 333);
            PackShift.Name = "PackShift";
            PackShift.Size = new Size(15, 14);
            PackShift.TabIndex = 61;
            PackShift.UseVisualStyleBackColor = true;
            // 
            // PackKey
            // 
            PackKey.DropDownStyle = ComboBoxStyle.DropDownList;
            PackKey.FormattingEnabled = true;
            PackKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            PackKey.Location = new Point(564, 329);
            PackKey.Name = "PackKey";
            PackKey.Size = new Size(64, 23);
            PackKey.TabIndex = 60;
            // 
            // cb_Pack
            // 
            cb_Pack.AutoSize = true;
            cb_Pack.Checked = true;
            cb_Pack.CheckState = CheckState.Checked;
            cb_Pack.Location = new Point(441, 331);
            cb_Pack.Name = "cb_Pack";
            cb_Pack.Size = new Size(55, 19);
            cb_Pack.TabIndex = 59;
            cb_Pack.Text = "PACK";
            cb_Pack.UseVisualStyleBackColor = true;
            // 
            // StoCShift
            // 
            StoCShift.AutoSize = true;
            StoCShift.Location = new Point(326, 101);
            StoCShift.Name = "StoCShift";
            StoCShift.Size = new Size(15, 14);
            StoCShift.TabIndex = 64;
            StoCShift.UseVisualStyleBackColor = true;
            // 
            // StoCKey
            // 
            StoCKey.DropDownStyle = ComboBoxStyle.DropDownList;
            StoCKey.FormattingEnabled = true;
            StoCKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            StoCKey.Location = new Point(352, 97);
            StoCKey.Name = "StoCKey";
            StoCKey.Size = new Size(64, 23);
            StoCKey.TabIndex = 63;
            // 
            // cb_StoC
            // 
            cb_StoC.AutoSize = true;
            cb_StoC.Location = new Point(229, 99);
            cb_StoC.Name = "cb_StoC";
            cb_StoC.Size = new Size(55, 19);
            cb_StoC.TabIndex = 62;
            cb_StoC.Text = "STO c";
            cb_StoC.UseVisualStyleBackColor = true;
            // 
            // XexBShift
            // 
            XexBShift.AutoSize = true;
            XexBShift.Location = new Point(538, 72);
            XexBShift.Name = "XexBShift";
            XexBShift.Size = new Size(15, 14);
            XexBShift.TabIndex = 67;
            XexBShift.UseVisualStyleBackColor = true;
            // 
            // XexBKey
            // 
            XexBKey.DropDownStyle = ComboBoxStyle.DropDownList;
            XexBKey.FormattingEnabled = true;
            XexBKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            XexBKey.Location = new Point(564, 68);
            XexBKey.Name = "XexBKey";
            XexBKey.Size = new Size(64, 23);
            XexBKey.TabIndex = 66;
            // 
            // cb_XexB
            // 
            cb_XexB.AutoSize = true;
            cb_XexB.Location = new Point(441, 70);
            cb_XexB.Name = "cb_XexB";
            cb_XexB.Size = new Size(59, 19);
            cb_XexB.TabIndex = 65;
            cb_XexB.Text = "X<> b";
            cb_XexB.UseVisualStyleBackColor = true;
            // 
            // XexEShift
            // 
            XexEShift.AutoSize = true;
            XexEShift.Location = new Point(538, 159);
            XexEShift.Name = "XexEShift";
            XexEShift.Size = new Size(15, 14);
            XexEShift.TabIndex = 70;
            XexEShift.UseVisualStyleBackColor = true;
            // 
            // XexEKey
            // 
            XexEKey.DropDownStyle = ComboBoxStyle.DropDownList;
            XexEKey.FormattingEnabled = true;
            XexEKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            XexEKey.Location = new Point(564, 155);
            XexEKey.Name = "XexEKey";
            XexEKey.Size = new Size(64, 23);
            XexEKey.TabIndex = 69;
            // 
            // cb_XexE
            // 
            cb_XexE.AutoSize = true;
            cb_XexE.Location = new Point(441, 157);
            cb_XexE.Name = "cb_XexE";
            cb_XexE.Size = new Size(58, 19);
            cb_XexE.TabIndex = 68;
            cb_XexE.Text = "X<> e";
            cb_XexE.UseVisualStyleBackColor = true;
            // 
            // XexOShift
            // 
            XexOShift.AutoSize = true;
            XexOShift.Location = new Point(538, 246);
            XexOShift.Name = "XexOShift";
            XexOShift.Size = new Size(15, 14);
            XexOShift.TabIndex = 79;
            XexOShift.UseVisualStyleBackColor = true;
            // 
            // XexOKey
            // 
            XexOKey.DropDownStyle = ComboBoxStyle.DropDownList;
            XexOKey.FormattingEnabled = true;
            XexOKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            XexOKey.Location = new Point(564, 242);
            XexOKey.Name = "XexOKey";
            XexOKey.Size = new Size(64, 23);
            XexOKey.TabIndex = 78;
            // 
            // cb_XexO
            // 
            cb_XexO.AutoSize = true;
            cb_XexO.Location = new Point(441, 244);
            cb_XexO.Name = "cb_XexO";
            cb_XexO.Size = new Size(61, 19);
            cb_XexO.TabIndex = 77;
            cb_XexO.Text = "X<> O";
            cb_XexO.UseVisualStyleBackColor = true;
            // 
            // StoOShift
            // 
            StoOShift.AutoSize = true;
            StoOShift.Location = new Point(326, 246);
            StoOShift.Name = "StoOShift";
            StoOShift.Size = new Size(15, 14);
            StoOShift.TabIndex = 76;
            StoOShift.UseVisualStyleBackColor = true;
            // 
            // StoOKey
            // 
            StoOKey.DropDownStyle = ComboBoxStyle.DropDownList;
            StoOKey.FormattingEnabled = true;
            StoOKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            StoOKey.Location = new Point(352, 242);
            StoOKey.Name = "StoOKey";
            StoOKey.Size = new Size(64, 23);
            StoOKey.TabIndex = 75;
            // 
            // cb_StoO
            // 
            cb_StoO.AutoSize = true;
            cb_StoO.Location = new Point(229, 244);
            cb_StoO.Name = "cb_StoO";
            cb_StoO.Size = new Size(58, 19);
            cb_StoO.TabIndex = 74;
            cb_StoO.Text = "STO O";
            cb_StoO.UseVisualStyleBackColor = true;
            // 
            // RclOShift
            // 
            RclOShift.AutoSize = true;
            RclOShift.Location = new Point(113, 246);
            RclOShift.Name = "RclOShift";
            RclOShift.Size = new Size(15, 14);
            RclOShift.TabIndex = 73;
            RclOShift.UseVisualStyleBackColor = true;
            // 
            // RclOKey
            // 
            RclOKey.DropDownStyle = ComboBoxStyle.DropDownList;
            RclOKey.FormattingEnabled = true;
            RclOKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            RclOKey.Location = new Point(139, 242);
            RclOKey.Name = "RclOKey";
            RclOKey.Size = new Size(64, 23);
            RclOKey.TabIndex = 72;
            // 
            // cb_RclO
            // 
            cb_RclO.AutoSize = true;
            cb_RclO.Location = new Point(16, 244);
            cb_RclO.Name = "cb_RclO";
            cb_RclO.Size = new Size(59, 19);
            cb_RclO.TabIndex = 71;
            cb_RclO.Text = "RCL O";
            cb_RclO.UseVisualStyleBackColor = true;
            // 
            // XexPShift
            // 
            XexPShift.AutoSize = true;
            XexPShift.Location = new Point(538, 275);
            XexPShift.Name = "XexPShift";
            XexPShift.Size = new Size(15, 14);
            XexPShift.TabIndex = 88;
            XexPShift.UseVisualStyleBackColor = true;
            // 
            // XexPKey
            // 
            XexPKey.DropDownStyle = ComboBoxStyle.DropDownList;
            XexPKey.FormattingEnabled = true;
            XexPKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            XexPKey.Location = new Point(564, 271);
            XexPKey.Name = "XexPKey";
            XexPKey.Size = new Size(64, 23);
            XexPKey.TabIndex = 87;
            // 
            // cb_XexP
            // 
            cb_XexP.AutoSize = true;
            cb_XexP.Location = new Point(441, 273);
            cb_XexP.Name = "cb_XexP";
            cb_XexP.Size = new Size(59, 19);
            cb_XexP.TabIndex = 86;
            cb_XexP.Text = "X<> P";
            cb_XexP.UseVisualStyleBackColor = true;
            // 
            // StoPShift
            // 
            StoPShift.AutoSize = true;
            StoPShift.Location = new Point(326, 275);
            StoPShift.Name = "StoPShift";
            StoPShift.Size = new Size(15, 14);
            StoPShift.TabIndex = 85;
            StoPShift.UseVisualStyleBackColor = true;
            // 
            // StoPKey
            // 
            StoPKey.DropDownStyle = ComboBoxStyle.DropDownList;
            StoPKey.FormattingEnabled = true;
            StoPKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            StoPKey.Location = new Point(352, 271);
            StoPKey.Name = "StoPKey";
            StoPKey.Size = new Size(64, 23);
            StoPKey.TabIndex = 84;
            // 
            // cb_StoP
            // 
            cb_StoP.AutoSize = true;
            cb_StoP.Location = new Point(229, 273);
            cb_StoP.Name = "cb_StoP";
            cb_StoP.Size = new Size(56, 19);
            cb_StoP.TabIndex = 83;
            cb_StoP.Text = "STO P";
            cb_StoP.UseVisualStyleBackColor = true;
            // 
            // RclPShift
            // 
            RclPShift.AutoSize = true;
            RclPShift.Location = new Point(113, 275);
            RclPShift.Name = "RclPShift";
            RclPShift.Size = new Size(15, 14);
            RclPShift.TabIndex = 82;
            RclPShift.UseVisualStyleBackColor = true;
            // 
            // RclPKey
            // 
            RclPKey.DropDownStyle = ComboBoxStyle.DropDownList;
            RclPKey.FormattingEnabled = true;
            RclPKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            RclPKey.Location = new Point(139, 271);
            RclPKey.Name = "RclPKey";
            RclPKey.Size = new Size(64, 23);
            RclPKey.TabIndex = 81;
            // 
            // cb_RclP
            // 
            cb_RclP.AutoSize = true;
            cb_RclP.Location = new Point(16, 273);
            cb_RclP.Name = "cb_RclP";
            cb_RclP.Size = new Size(57, 19);
            cb_RclP.TabIndex = 80;
            cb_RclP.Text = "RCL P";
            cb_RclP.UseVisualStyleBackColor = true;
            // 
            // XexQShift
            // 
            XexQShift.AutoSize = true;
            XexQShift.Location = new Point(538, 304);
            XexQShift.Name = "XexQShift";
            XexQShift.Size = new Size(15, 14);
            XexQShift.TabIndex = 97;
            XexQShift.UseVisualStyleBackColor = true;
            // 
            // XexQKey
            // 
            XexQKey.DropDownStyle = ComboBoxStyle.DropDownList;
            XexQKey.FormattingEnabled = true;
            XexQKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            XexQKey.Location = new Point(564, 300);
            XexQKey.Name = "XexQKey";
            XexQKey.Size = new Size(64, 23);
            XexQKey.TabIndex = 96;
            // 
            // cb_XexQ
            // 
            cb_XexQ.AutoSize = true;
            cb_XexQ.Location = new Point(441, 302);
            cb_XexQ.Name = "cb_XexQ";
            cb_XexQ.Size = new Size(61, 19);
            cb_XexQ.TabIndex = 95;
            cb_XexQ.Text = "X<> Q";
            cb_XexQ.UseVisualStyleBackColor = true;
            // 
            // StoQShift
            // 
            StoQShift.AutoSize = true;
            StoQShift.Location = new Point(326, 304);
            StoQShift.Name = "StoQShift";
            StoQShift.Size = new Size(15, 14);
            StoQShift.TabIndex = 94;
            StoQShift.UseVisualStyleBackColor = true;
            // 
            // StoQKey
            // 
            StoQKey.DropDownStyle = ComboBoxStyle.DropDownList;
            StoQKey.FormattingEnabled = true;
            StoQKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            StoQKey.Location = new Point(352, 300);
            StoQKey.Name = "StoQKey";
            StoQKey.Size = new Size(64, 23);
            StoQKey.TabIndex = 93;
            // 
            // cb_StoQ
            // 
            cb_StoQ.AutoSize = true;
            cb_StoQ.Checked = true;
            cb_StoQ.CheckState = CheckState.Checked;
            cb_StoQ.Location = new Point(229, 302);
            cb_StoQ.Name = "cb_StoQ";
            cb_StoQ.Size = new Size(58, 19);
            cb_StoQ.TabIndex = 92;
            cb_StoQ.Text = "STO Q";
            cb_StoQ.UseVisualStyleBackColor = true;
            // 
            // RclQShift
            // 
            RclQShift.AutoSize = true;
            RclQShift.Location = new Point(113, 304);
            RclQShift.Name = "RclQShift";
            RclQShift.Size = new Size(15, 14);
            RclQShift.TabIndex = 91;
            RclQShift.UseVisualStyleBackColor = true;
            // 
            // RclQKey
            // 
            RclQKey.DropDownStyle = ComboBoxStyle.DropDownList;
            RclQKey.FormattingEnabled = true;
            RclQKey.Items.AddRange(new object[] { "E+", "1/X", "SQRT", "LOG", "LN", "X<>Y", "RDN", "SIN", "COS", "TAN", "XEQ", "STO", "RCL", "SST", "ENTER", "CHS", "EEX", "<--", "-", "7", "8", "9", "+", "4", "5", "6", "*", "1", "2", "3", "/", "0", ".", "RS" });
            RclQKey.Location = new Point(139, 300);
            RclQKey.Name = "RclQKey";
            RclQKey.Size = new Size(64, 23);
            RclQKey.TabIndex = 90;
            // 
            // cb_RclQ
            // 
            cb_RclQ.AutoSize = true;
            cb_RclQ.Location = new Point(16, 302);
            cb_RclQ.Name = "cb_RclQ";
            cb_RclQ.Size = new Size(59, 19);
            cb_RclQ.TabIndex = 89;
            cb_RclQ.Text = "RCL Q";
            cb_RclQ.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(247, 21);
            label4.Name = "label4";
            label4.Size = new Size(54, 15);
            label4.TabIndex = 100;
            label4.Text = "Function";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(353, 21);
            label5.Name = "label5";
            label5.Size = new Size(26, 15);
            label5.TabIndex = 99;
            label5.Text = "Key";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(316, 21);
            label6.Name = "label6";
            label6.Size = new Size(31, 15);
            label6.TabIndex = 98;
            label6.Text = "Shift";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(457, 21);
            label7.Name = "label7";
            label7.Size = new Size(54, 15);
            label7.TabIndex = 103;
            label7.Text = "Function";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(563, 21);
            label8.Name = "label8";
            label8.Size = new Size(26, 15);
            label8.TabIndex = 102;
            label8.Text = "Key";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(526, 21);
            label9.Name = "label9";
            label9.Size = new Size(31, 15);
            label9.TabIndex = 101;
            label9.Text = "Shift";
            // 
            // SynthDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(642, 420);
            Controls.Add(label7);
            Controls.Add(label8);
            Controls.Add(label9);
            Controls.Add(label4);
            Controls.Add(label5);
            Controls.Add(label6);
            Controls.Add(XexQShift);
            Controls.Add(XexQKey);
            Controls.Add(cb_XexQ);
            Controls.Add(StoQShift);
            Controls.Add(StoQKey);
            Controls.Add(cb_StoQ);
            Controls.Add(RclQShift);
            Controls.Add(RclQKey);
            Controls.Add(cb_RclQ);
            Controls.Add(XexPShift);
            Controls.Add(XexPKey);
            Controls.Add(cb_XexP);
            Controls.Add(StoPShift);
            Controls.Add(StoPKey);
            Controls.Add(cb_StoP);
            Controls.Add(RclPShift);
            Controls.Add(RclPKey);
            Controls.Add(cb_RclP);
            Controls.Add(XexOShift);
            Controls.Add(XexOKey);
            Controls.Add(cb_XexO);
            Controls.Add(StoOShift);
            Controls.Add(StoOKey);
            Controls.Add(cb_StoO);
            Controls.Add(RclOShift);
            Controls.Add(RclOKey);
            Controls.Add(cb_RclO);
            Controls.Add(XexEShift);
            Controls.Add(XexEKey);
            Controls.Add(cb_XexE);
            Controls.Add(XexBShift);
            Controls.Add(XexBKey);
            Controls.Add(cb_XexB);
            Controls.Add(StoCShift);
            Controls.Add(StoCKey);
            Controls.Add(cb_StoC);
            Controls.Add(PackShift);
            Controls.Add(PackKey);
            Controls.Add(cb_Pack);
            Controls.Add(DelShift);
            Controls.Add(DelKey);
            Controls.Add(cb_Del);
            Controls.Add(StoEShift);
            Controls.Add(StoEKey);
            Controls.Add(cb_StoE);
            Controls.Add(RclEShift);
            Controls.Add(RclEKey);
            Controls.Add(cb_RclE);
            Controls.Add(HmsMShift);
            Controls.Add(HmsMKey);
            Controls.Add(cb_HmsM);
            Controls.Add(StoBShift);
            Controls.Add(StoBKey);
            Controls.Add(cb_StoB);
            Controls.Add(RclBShift);
            Controls.Add(RclBKey);
            Controls.Add(cb_RclB);
            Controls.Add(XexCShift);
            Controls.Add(XexCKey);
            Controls.Add(cb_XexC);
            Controls.Add(RclCShift);
            Controls.Add(RclCKey);
            Controls.Add(cb_RclC);
            Controls.Add(XexDShift);
            Controls.Add(XexDKey);
            Controls.Add(cb_XexD);
            Controls.Add(XexNShift);
            Controls.Add(XexNKey);
            Controls.Add(cb_XexN);
            Controls.Add(XexMShift);
            Controls.Add(XexMKey);
            Controls.Add(cb_XexM);
            Controls.Add(StoNShift);
            Controls.Add(StoNKey);
            Controls.Add(cb_StoN);
            Controls.Add(RclNShift);
            Controls.Add(RclNKey);
            Controls.Add(cb_RclN);
            Controls.Add(StoMShift);
            Controls.Add(StoMKey);
            Controls.Add(cb_StoM);
            Controls.Add(RclMShift);
            Controls.Add(RclMKey);
            Controls.Add(cb_RclM);
            Controls.Add(StoDShift);
            Controls.Add(StoDKey);
            Controls.Add(cb_StoD);
            Controls.Add(RclDShift);
            Controls.Add(RclDKey);
            Controls.Add(cb_RclD);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(ByteJumperShift);
            Controls.Add(ByteJumperKey);
            Controls.Add(CancelButton);
            Controls.Add(OkButton);
            Controls.Add(cb_ByteJumper);
            Name = "SynthDialog";
            Text = "Install Synthetic Keys";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox cb_ByteJumper;
        private Button OkButton;
        private Button CancelButton;
        private ComboBox ByteJumperKey;
        private CheckBox ByteJumperShift;
        private Label label1;
        private Label label2;
        private Label label3;
        private CheckBox RclDShift;
        private ComboBox RclDKey;
        private CheckBox cb_RclD;
        private CheckBox StoDShift;
        private ComboBox StoDKey;
        private CheckBox cb_StoD;
        private CheckBox StoMShift;
        private ComboBox StoMKey;
        private CheckBox cb_StoM;
        private CheckBox RclMShift;
        private ComboBox RclMKey;
        private CheckBox cb_RclM;
        private CheckBox StoNShift;
        private ComboBox StoNKey;
        private CheckBox cb_StoN;
        private CheckBox RclNShift;
        private ComboBox RclNKey;
        private CheckBox cb_RclN;
        private CheckBox XexNShift;
        private ComboBox XexNKey;
        private CheckBox cb_XexN;
        private CheckBox XexMShift;
        private ComboBox XexMKey;
        private CheckBox cb_XexM;
        private CheckBox XexDShift;
        private ComboBox XexDKey;
        private CheckBox cb_XexD;
        private CheckBox XexCShift;
        private ComboBox XexCKey;
        private CheckBox cb_XexC;
        private CheckBox RclCShift;
        private ComboBox RclCKey;
        private CheckBox cb_RclC;
        private CheckBox StoBShift;
        private ComboBox StoBKey;
        private CheckBox cb_StoB;
        private CheckBox RclBShift;
        private ComboBox RclBKey;
        private CheckBox cb_RclB;
        private CheckBox HmsMShift;
        private ComboBox HmsMKey;
        private CheckBox cb_HmsM;
        private CheckBox StoEShift;
        private ComboBox StoEKey;
        private CheckBox cb_StoE;
        private CheckBox RclEShift;
        private ComboBox RclEKey;
        private CheckBox cb_RclE;
        private CheckBox DelShift;
        private ComboBox DelKey;
        private CheckBox cb_Del;
        private CheckBox PackShift;
        private ComboBox PackKey;
        private CheckBox cb_Pack;
        private CheckBox StoCShift;
        private ComboBox StoCKey;
        private CheckBox cb_StoC;
        private CheckBox XexBShift;
        private ComboBox XexBKey;
        private CheckBox cb_XexB;
        private CheckBox XexEShift;
        private ComboBox XexEKey;
        private CheckBox cb_XexE;
        private CheckBox XexOShift;
        private ComboBox XexOKey;
        private CheckBox cb_XexO;
        private CheckBox StoOShift;
        private ComboBox StoOKey;
        private CheckBox cb_StoO;
        private CheckBox RclOShift;
        private ComboBox RclOKey;
        private CheckBox cb_RclO;
        private CheckBox XexPShift;
        private ComboBox XexPKey;
        private CheckBox cb_XexP;
        private CheckBox StoPShift;
        private ComboBox StoPKey;
        private CheckBox cb_StoP;
        private CheckBox RclPShift;
        private ComboBox RclPKey;
        private CheckBox cb_RclP;
        private CheckBox XexQShift;
        private ComboBox XexQKey;
        private CheckBox cb_XexQ;
        private CheckBox StoQShift;
        private ComboBox StoQKey;
        private CheckBox cb_StoQ;
        private CheckBox RclQShift;
        private ComboBox RclQKey;
        private CheckBox cb_RclQ;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
    }
}