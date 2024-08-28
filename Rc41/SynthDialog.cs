using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rc41
{
    public partial class SynthDialog : Form
    {
        byte[] keys = new byte[]
        {
            0x01,               //  0 - E+
            0x11,               //  1 - 1/X
            0x21,               //  2 - SQRT
            0x31,               //  3 - LOG
            0x41,               //  4 - LN
            0x02,               //  5 - X<>Y
            0x12,               //  6 - RDN
            0x22,               //  7 - SIN
            0x32,               //  8 - COS
            0x42,               //  9 - TAN
            0x13,               // 10 - XEQ
            0x23,               // 11 - STO
            0x33,               // 12 - RCL
            0x43,               // 13 - SST
            0x04,               // 14 - ENTER
            0x24,               // 15 - CHS
            0x34,               // 16 - EEX
            0x44,               // 17 - <--
            0x05,               // 18 - -
            0x15,               // 19 - 7
            0x25,               // 20 - 8
            0x35,               // 21 - 9
            0x06,               // 22 - +
            0x16,               // 23 - 4
            0x26,               // 24 - 5
            0x36,               // 25 - 6
            0x07,               // 22 - *
            0x17,               // 23 - 1
            0x27,               // 24 - 2
            0x37,               // 25 - 3
            0x08,               // 26 - /
            0x18,               // 27 - 0
            0x28,               // 28 - .
            0x38,               // 29 - RS
        };

        byte[] skeys = new byte[]
        {
            0x09,               //  0 - E-
            0x19,               //  1 - Y^X
            0x29,               //  2 - X^2
            0x39,               //  3 - 10^X
            0x49,               //  4 - E^X
            0x0a,               //  5 - CLE
            0x1a,               //  6 - %
            0x2a,               //  7 - ASIN
            0x3a,               //  8 - ACOS
            0x4a,               //  9 - ATAN
            0x1b,               // 10 - ASN
            0x2b,               // 11 - LBL
            0x3b,               // 12 - GTO
            0x4b,               // 13 - BST
            0x0c,               // 14 - CAT
            0x2c,               // 15 - ISG
            0x3c,               // 16 - RTN
            0x4c,               // 17 - CLX
            0x0d,               // 18 - X=Y?
            0x1d,               // 19 - SF
            0x2d,               // 20 - CF
            0x3d,               // 21 - FS?
            0x0e,               // 22 - X<=Y?
            0x1e,               // 23 - BEEP
            0x2e,               // 24 - P-R
            0x3e,               // 25 - R-P
            0x0f,               // 22 - X>Y?
            0x1f,               // 23 - FIX
            0x2f,               // 24 - SCI
            0x3f,               // 25 - ENG
            0x10,               // 26 - X=0?
            0x20,               // 27 - PI
            0x30,               // 28 - LASTX
            0x40,               // 29 - VIEW
        };

        public SynthDialog()
        {
            InitializeComponent();
            ByteJumperKey.SelectedIndex = 0;
            QLoaderKey.SelectedIndex = 0;
            QLoaderShift.Checked = true;
            RclDKey.SelectedIndex = 4;
            StoDKey.SelectedIndex = 4;
            StoDShift.Checked = true;
            RclMKey.SelectedIndex = 9;
            StoMKey.SelectedIndex = 9;
            StoMShift.Checked = true;
            RclNKey.SelectedIndex = 8;
            StoNKey.SelectedIndex = 8;
            StoNShift.Checked = true;
            XexMKey.SelectedIndex = 7;
            XexNKey.SelectedIndex = 7;
            XexNShift.Checked = true;
            XexDKey.SelectedIndex = 3;
            XexDShift.Checked = true;
            RclCKey.SelectedIndex = 2;
            XexCKey.SelectedIndex = 2;
            XexCShift.Checked = true;
            RclBKey.SelectedIndex = 1;
            StoBKey.SelectedIndex = 1;
            StoBShift.Checked = true;
            HmsMKey.SelectedIndex = 18;
            HmsMShift.Checked = true;
            RclEKey.SelectedIndex = 23;
            RclEShift.Checked = true;
            StoEKey.SelectedIndex = 24;
            StoEShift.Checked = true;
            DelKey.SelectedIndex = 25;
            DelShift.Checked = true;
            PackKey.SelectedIndex = 29;
            PackShift.Checked = true;
            StoCKey.SelectedIndex = 0;
            XexBKey.SelectedIndex = 0;
            XexEKey.SelectedIndex = 0;
            RclOKey.SelectedIndex = 0;
            StoOKey.SelectedIndex = 0;
            XexOKey.SelectedIndex = 0;
            RclPKey.SelectedIndex = 0;
            StoPKey.SelectedIndex = 0;
            XexPKey.SelectedIndex = 0;
            RclQKey.SelectedIndex = 0;
            StoQKey.SelectedIndex = 22;
            XexQKey.SelectedIndex = 0;
            StoQShift.Checked = true;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult=DialogResult.Cancel;
            Close();
        }

        public byte ByteJumper()
        {
            if (!cb_ByteJumper.Checked) return 0;
            if (ByteJumperShift.Checked) return skeys[ByteJumperKey.SelectedIndex];
            return keys[ByteJumperKey.SelectedIndex];
        }

        public byte QLoader()
        {
            if (!cb_QLoader.Checked) return 0;
            if (QLoaderShift.Checked) return skeys[QLoaderKey.SelectedIndex];
            return keys[QLoaderKey.SelectedIndex];
        }

        public byte RclD()
        {
            if (!cb_RclD.Checked) return 0;
            if (RclDShift.Checked) return skeys[RclDKey.SelectedIndex];
            return keys[RclDKey.SelectedIndex];
        }

        public byte StoD()
        {
            if (!cb_StoD.Checked) return 0;
            if (StoDShift.Checked) return skeys[StoDKey.SelectedIndex];
            return keys[StoDKey.SelectedIndex];
        }

        public byte RclM()
        {
            if (!cb_RclM.Checked) return 0;
            if (RclMShift.Checked) return skeys[RclMKey.SelectedIndex];
            return keys[RclMKey.SelectedIndex];
        }

        public byte StoM()
        {
            if (!cb_StoM.Checked) return 0;
            if (StoMShift.Checked) return skeys[StoMKey.SelectedIndex];
            return keys[StoMKey.SelectedIndex];
        }

        public byte RclN()
        {
            if (!cb_RclN.Checked) return 0;
            if (RclNShift.Checked) return skeys[RclNKey.SelectedIndex];
            return keys[RclNKey.SelectedIndex];
        }

        public byte StoN()
        {
            if (!cb_StoN.Checked) return 0;
            if (StoNShift.Checked) return skeys[StoNKey.SelectedIndex];
            return keys[StoNKey.SelectedIndex];
        }

        public byte XexM()
        {
            if (!cb_XexM.Checked) return 0;
            if (XexMShift.Checked) return skeys[XexMKey.SelectedIndex];
            return keys[XexMKey.SelectedIndex];
        }

        public byte XexN()
        {
            if (!cb_XexN.Checked) return 0;
            if (XexNShift.Checked) return skeys[XexNKey.SelectedIndex];
            return keys[XexNKey.SelectedIndex];
        }

        public byte XexD()
        {
            if (!cb_XexD.Checked) return 0;
            if (XexDShift.Checked) return skeys[XexDKey.SelectedIndex];
            return keys[XexDKey.SelectedIndex];
        }
        public byte RclC()
        {
            if (!cb_RclC.Checked) return 0;
            if (RclCShift.Checked) return skeys[RclCKey.SelectedIndex];
            return keys[RclCKey.SelectedIndex];
        }
        public byte XexC()
        {
            if (!cb_XexC.Checked) return 0;
            if (XexCShift.Checked) return skeys[XexCKey.SelectedIndex];
            return keys[XexCKey.SelectedIndex];
        }

        public byte RclB()
        {
            if (!cb_RclB.Checked) return 0;
            if (RclBShift.Checked) return skeys[RclBKey.SelectedIndex];
            return keys[RclBKey.SelectedIndex];
        }

        public byte StoB()
        {
            if (!cb_StoB.Checked) return 0;
            if (StoBShift.Checked) return skeys[StoBKey.SelectedIndex];
            return keys[StoBKey.SelectedIndex];
        }

        public byte HmsMinus()
        {
            if (!cb_HmsM.Checked) return 0;
            if (HmsMShift.Checked) return skeys[HmsMKey.SelectedIndex];
            return keys[HmsMKey.SelectedIndex];
        }

        public byte RclE()
        {
            if (!cb_RclE.Checked) return 0;
            if (RclEShift.Checked) return skeys[RclEKey.SelectedIndex];
            return keys[RclEKey.SelectedIndex];
        }

        public byte StoE()
        {
            if (!cb_StoE.Checked) return 0;
            if (StoEShift.Checked) return skeys[StoEKey.SelectedIndex];
            return keys[StoEKey.SelectedIndex];
        }

        public byte Del()
        {
            if (!cb_Del.Checked) return 0;
            if (DelShift.Checked) return skeys[DelKey.SelectedIndex];
            return keys[DelKey.SelectedIndex];
        }

        public byte Pack()
        {
            if (!cb_Pack.Checked) return 0;
            if (PackShift.Checked) return skeys[PackKey.SelectedIndex];
            return keys[PackKey.SelectedIndex];
        }

        public byte StoC()
        {
            if (!cb_StoC.Checked) return 0;
            if (StoCShift.Checked) return skeys[StoCKey.SelectedIndex];
            return keys[StoCKey.SelectedIndex];
        }

        public byte XexB()
        {
            if (!cb_XexB.Checked) return 0;
            if (XexBShift.Checked) return skeys[XexBKey.SelectedIndex];
            return keys[XexBKey.SelectedIndex];
        }

        public byte XexE()
        {
            if (!cb_XexE.Checked) return 0;
            if (XexEShift.Checked) return skeys[XexEKey.SelectedIndex];
            return keys[XexEKey.SelectedIndex];
        }

        public byte RclO()
        {
            if (!cb_RclO.Checked) return 0;
            if (RclOShift.Checked) return skeys[RclOKey.SelectedIndex];
            return keys[RclOKey.SelectedIndex];
        }

        public byte StoO()
        {
            if (!cb_StoO.Checked) return 0;
            if (StoOShift.Checked) return skeys[StoOKey.SelectedIndex];
            return keys[StoOKey.SelectedIndex];
        }

        public byte XexO()
        {
            if (!cb_XexO.Checked) return 0;
            if (XexOShift.Checked) return skeys[XexOKey.SelectedIndex];
            return keys[XexOKey.SelectedIndex];
        }

        public byte RclP()
        {
            if (!cb_RclP.Checked) return 0;
            if (RclPShift.Checked) return skeys[RclPKey.SelectedIndex];
            return keys[RclPKey.SelectedIndex];
        }

        public byte StoP()
        {
            if (!cb_StoP.Checked) return 0;
            if (StoPShift.Checked) return skeys[StoPKey.SelectedIndex];
            return keys[StoPKey.SelectedIndex];
        }

        public byte XexP()
        {
            if (!cb_XexP.Checked) return 0;
            if (XexPShift.Checked) return skeys[XexPKey.SelectedIndex];
            return keys[XexPKey.SelectedIndex];
        }

        public byte RclQ()
        {
            if (!cb_RclQ.Checked) return 0;
            if (RclQShift.Checked) return skeys[RclQKey.SelectedIndex];
            return keys[RclQKey.SelectedIndex];
        }

        public byte StoQ()
        {
            if (!cb_StoQ.Checked) return 0;
            if (StoQShift.Checked) return skeys[StoQKey.SelectedIndex];
            return keys[StoQKey.SelectedIndex];
        }

        public byte XexQ()
        {
            if (!cb_XexQ.Checked) return 0;
            if (XexQShift.Checked) return skeys[XexQKey.SelectedIndex];
            return keys[XexQKey.SelectedIndex];
        }

    }
}
