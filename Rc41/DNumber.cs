using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41
{
    public struct DNumber
    {
        public byte sign;
        public byte[] mantissa;
        public byte esign;
        public byte[] exponent;

        public DNumber()
        {
            mantissa = new byte[20];
            exponent = new byte[3];
            sign = 0;
            esign = 0;
            for (int i = 0; i < 20; i++) mantissa[i] = 0;
            for (int i = 0; i < 3; i++) exponent[i] = 0;
        }
        public DNumber(byte s, byte m1, byte m2, byte m3, byte m4, byte m5, byte m6, byte m7, byte m8, byte m9, byte m10,
                       byte m11, byte m12, byte m13, byte m14, byte m15, byte m16, byte m17, byte m18, byte m19, byte m20,
                       byte es, byte e1, byte e2, byte e3)
        {
            sign = s;
            esign = es;
            mantissa = new byte[20];
            mantissa[0] = m1;
            mantissa[1] = m2;
            mantissa[2] = m3;
            mantissa[3] = m4;
            mantissa[4] = m5;
            mantissa[5] = m6;
            mantissa[6] = m7;
            mantissa[7] = m8;
            mantissa[8] = m9;
            mantissa[9] = m10;
            mantissa[10] = m11;
            mantissa[11] = m12;
            mantissa[12] = m13;
            mantissa[13] = m14;
            mantissa[14] = m15;
            mantissa[15] = m16;
            mantissa[16] = m17;
            mantissa[17] = m18;
            mantissa[18] = m19;
            mantissa[19] = m20;
            exponent = new byte[3];
            exponent[0] = e1;
            exponent[1] = e2;
            exponent[2] = e3;
        }


    }
}
