using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Rc41
{
    public struct Number
    {
        public byte sign;
        public byte[] mantissa;
        public byte esign;
        public byte[] exponent;

        public Number()
        {
            mantissa = new byte[10];
            exponent = new byte[2];
            sign = 0;
            esign = 0;
            for (int i=0; i<10; i++) mantissa[i] = 0;
            for (int i=0; i<2; i++) exponent[i] = 0;
        }
        public Number(byte s, byte m1, byte m2, byte m3, byte m4, byte m5, byte m6, byte m7, byte m8, byte m9, byte m10, byte es, byte e1, byte e2)
        {
            sign = s;
            esign = es;
            mantissa = new byte[10];
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
            exponent = new byte[2];
            exponent[0] = e1;
            exponent[1] = e2;
        }
    }
}
