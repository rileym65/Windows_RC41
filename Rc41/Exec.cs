using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41
{
    partial class Cpu
    {

        Number Acos(Number a)
        {
            a = Asin(a);
            return Sub(S_PI2, a);
        }

        public Number Add(Number a, Number b)
        {
            DNumber da;
            DNumber db;
            DNumber dc;
            da = NumberToDNumber(a);
            db = NumberToDNumber(b);
            dc = D_Add(da, db);
            return DNumberToNumber(dc);
        }


        void Arcl(int rreg)
        {
            int i;
            int p;
            int reg;
            int b;
            string buffer;
            Number n;
            b = ram[REG_C + 2] << 4;
            b |= ((ram[REG_C + 1] >> 4) & 0xf);
            if (rreg < 0x70)
            {
                reg = b + rreg;
            }
            else if (rreg >= 0x70 && rreg <= 0x7f) reg = rreg - 0x70;
            else if (rreg >= 0x80 && rreg <= 0xef)
            {
                reg = b + (rreg - 0x80);
                if (reg > 0xfff)
                {
                    Message("NONEXISTENT");
                    Error();
                    return;
                }
                reg = b + ToInteger(RecallNumber(reg));
            }
            else if (rreg >= 0xf0 && rreg <= 0xff)
            {
                reg = b + ToInteger(RecallNumber(rreg - 0xf0));
            }
            else reg = 0x200;
            if (reg > 0x1ff)
            {
                Message("NONEXISTENT");
                Error();
                return;
            }
            buffer = "";
            if (ram[reg * 7 + 6] == 0x10)
            {
                p = 0;
                for (i = 5; i >= 0; i--)
                    if (ram[reg * 7 + i] != 0x00)
                        buffer += ((char)ram[reg * 7 + i]).ToString();
            }
            else
            {
                n = RecallNumber(reg);
                buffer = Format(n);
                while (buffer[0] == ' ')
                {
                    buffer = buffer.Substring(1);
                }
            }
            p = 0;
            while (p < buffer.Length && buffer[p] != 0)
            {
                for (i = (REG_P + 2); i > (REG_M); i--) ram[i] = ram[i - 1];
                ram[REG_M] = (byte)buffer[p++];
            }
        }

        Number Asin(Number a)
        {
            Number t;
            Number x;
            t = Mul(a, a);
            t = Sub(S_ONE, t);
            t = Sqrt(t);
            x = Div(a, t);
            t = Atan(x);
            return t;
        }

        public void Asn(string command, byte keycode)
        {
            int i;
            bool flag;
            int entry;
            int cat;
            int ky;
            int addr;
            int b1;
            int b2;
            addr = 0;
            b1 = 0;
            b2 = 0;
            if (command.Equals(""))
            {
                cat = 4;
            }
            else
            {
                entry = 0;
                flag = true;
                cat = 0;
                while (flag)
                {
                    if (command.Equals(catalog[entry].name, StringComparison.OrdinalIgnoreCase)) flag = false;
                    else if (catalog[++entry].flags == 0xff) flag = false;
                }
                if (catalog[entry].flags != 0xff)
                {
                    cat = 1;
                    if ((catalog[entry].post) != 0)
                    {
                        b1 = catalog[entry].cmd;
                        b2 = catalog[entry].post;
                    }
                    else
                    {
                        b1 = 0x04;
                        b2 = catalog[entry].cmd;
                        if (b2 == 0x30) b2 = 0x91;
                        if (b2 == 0x20) b2 = 0x90;
                    }
                }
                if (cat == 0)
                {
                    addr = FindGlobal(command);
                    if (addr != 0)
                    {
                        addr--;
                        cat = 3;
                    }
                }
                if (cat == 0)
                {
                    Message("NONEXISTENT");
                    Error();
                    return;
                }
            }
            UnAsn(keycode, 3);
            if (cat == 4) return;
            if (cat == 3)
            {
                ram[addr - 3] = keycode;
                SetKaFlag(keycode, true);
                return;
            }
            addr = 0x0c0 * 7;
            flag = true;
            while (flag)
            {
                if (ram[addr + 6] != 0xf0) flag = false;
                if (ram[addr + 5] == 0x00) flag = false;
                if (ram[addr + 2] == 0x00) flag = false;
                if (flag) addr += 7;
            }
            if (ram[addr + 6] != 0xf0)
            {
                for (i = 0; i < 6; i++) ram[addr + i] = 0x00;
                ram[addr + 6] = 0xf0;
            }
            if (ram[addr + 2] == 0x00)
            {
                ram[addr + 2] = (byte)b1;
                ram[addr + 1] = (byte)b2;
                ram[addr + 0] = keycode;
            }
            else
            {
                ram[addr + 5] = (byte)b1;
                ram[addr + 4] = (byte)b2;
                ram[addr + 3] = keycode;
            }
            SetKaFlag(keycode, true);
        }

        void Asto(int rreg)
        {
            int i;
            int p;
            int l;
            int reg;
            int b;
            b = ram[REG_C + 2] << 4;
            b |= ((ram[REG_C + 1] >> 4) & 0xf);
            if (rreg < 0x70)
            {
                reg = b + rreg;
            }
            else if (rreg >= 0x70 && rreg <= 0x7f) reg = rreg - 0x70;
            else if (rreg >= 0x80 && rreg <= 0xef)
            {
                reg = b + (rreg - 0x80);
                if (reg > 0x1ff)
                {
                    Message("NONEXISTENT");
                    Error();
                    return;
                }
                reg = b + ToInteger(RecallNumber(reg));
            }
            else if (rreg >= 0xf0 && rreg <= 0xff)
            {
                reg = b + ToInteger(RecallNumber(rreg - 0xf0));
            }
            else reg = 0x200;
            if (reg > 0xfff)
            {
                Message("NONEXISTENT");
                Error();
                return;
            }
            reg *= 7;
            p = (REG_P) + 2;
            while (p > (REG_M) && ram[p] == 0) p--;
            if (p == (REG_M) && ram[p] == 0)
            {
                ram[reg + 6] = 0x10;
                for (i = 0; i < 6; i++) ram[reg + i] = 0x00;
            }
            else
            {
                l = p - (REG_M) + 1;
                if (l > 6) l = 6;
                for (i = 0; i < 6; i++) ram[reg + i] = 0x00;
                i = 5;
                ram[reg + 6] = 0x10;
                while (l > 0)
                {
                    for (i = 5; i > 0; i--) ram[reg + i] = ram[reg + i - 1];
                    ram[reg] = ram[p--];
                    l--;
                }
            }
        }

        Number Atan(Number a)
        {
            int i;
            bool flag;
            bool addsub;
            DNumber x;
            DNumber xsqr;
            DNumber sum;
            DNumber den;
            DNumber last;
            DNumber term;
            x = NumberToDNumber(a);
            xsqr = D_Mul(x, x);
            addsub = true;
            if (x.esign != 0)
            {
                den = D_TWO;
                den = D_Add(den, D_ONE);
                sum = x;
                flag = true;
                while (flag)
                {
                    last = sum;
                    x = D_Mul(x, xsqr);
                    term = D_Div(x, den);
                    if (addsub) sum = D_Sub(sum, term);
                    else sum = D_Add(sum, term);
                    addsub = (addsub) ? false : true;
                    den = D_Add(den, D_TWO);
                    flag = false;
                    for (i = 0; i < 12; i++)
                        if (last.mantissa[i] != sum.mantissa[i]) flag = true;
                }
            }
            else
            {
                x.sign = 0;
                sum = D_PI2;
                den = D_ONE;
                flag = true;
                while (flag)
                {
                    last = sum;
                    term = D_Mul(den, x);
                    term = D_Div(D_ONE, term);
                    if (addsub) sum = D_Sub(sum, term);
                    else sum = D_Add(sum, term);
                    addsub = (addsub) ? false : true;
                    den = D_Add(den, D_TWO);
                    x = D_Mul(x, xsqr);
                    flag = false;
                    for (i = 0; i < 12; i++)
                        if (last.mantissa[i] != sum.mantissa[i]) flag = true;
                }
                sum.sign = a.sign;
            }
            return DNumberToNumber(sum);
        }

        Number AtoN(string number)
        {
            int i;
            int p;
            int e;
            int dp;
            int x;
            int sx;
            int ps;
            Number ret = new Number();
            ret.sign = 0;
            ret.esign = 0;
            ps = 0;
            for (i = 0; i < 10; i++) ret.mantissa[i] = 0;
            for (i = 0; i < 2; i++) ret.exponent[i] = 0;
            while (number[ps] == ' ') ps++;
            if (number[ps] == '+')
            {
                ret.sign = 0;
                ps++;
            }
            else if (number[ps] == '-')
            {
                ret.sign = 9;
                ps++;
            }
            while (ps < number.Length && number[ps] == '0') ps++;
            if (ps < number.Length && number[ps] == '.')
            {
                p = 0;
                e = -1;
                ps++;
                while (ps < number.Length && number[ps] == '0')
                {
                    e--;
                    ps++;
                }
                while (ps < number.Length && number[ps] >= '0' && number[ps] <= '9')
                {
                    if (p < 10)
                    {
                        ret.mantissa[p++] = (byte)(number[ps] - '0');
                    }
                    ps++;
                }
            }
            else
            {
                dp = 0;
                e = -1;
                p = 0;
                while (ps <number.Length &&((number[ps] >= '0' && number[ps] <= '9') || number[ps] == '.'))
                {
                    if (number[ps] != '.') if (p < 10) ret.mantissa[p++] = (byte)(number[ps] - '0');
                    if (number[ps] == '.') dp = 1;
                    if (dp == 0) e++;
                    ps++;
                }
            }
            if (ps < number.Length && (number[ps] == 'E' || number[ps] == 'e'))
            {
                ps++;
                sx = 1;
                if (number[ps] == '+')
                {
                    sx = 1;
                    ps++;
                }
                if (number[ps] == '-')
                {
                    sx = -1;
                    ps++;
                }
                x = 0;
                while (ps < number.Length && number[ps] >= '0' && number[ps] <= '9')
                {
                    x = (x * 10) + (number[ps] - '0');
                    ps++;
                }
                x *= sx;
                e += x;
            }
            if (e >= 0) ret.esign = 0; else ret.esign = 9;
            if (e < 0) e = -e;
            ret.exponent[0] = (byte)(e / 10);
            ret.exponent[1] = (byte)(e % 10);
            return ret;
        }

        void Aview()
        {
            int n;
            int m;
            int p;
            string buffer;
            n = 0;
            p = 0;
            m = REG_P + 2;
            buffer = "";
            while (m >= REG_M)
            {
                if (ram[m] == 0 && n != 0) buffer += ((char)0x00).ToString();
                else if (ram[m] == 0) ;
                else buffer += ((char)ram[m]).ToString();
                if (ram[m] != 0) n = -1;
                m--;
            }
            Message(buffer);
        }

        public void Bst()
        {
            int line;
            line = ram[REG_E + 0] | ((ram[REG_E + 1] & 0x0f) << 8);
            line--;
            if (line < 1) line = 65535;
            GotoLine(line);
        }

        void Cf(byte n)
        {
            Number a;
            if (n >= 0x80 && n <= 0xef)
            {
                a = Rcl(n & 0x7f);
                if (a.sign != 9 && a.sign != 0)
                {
                    Message("ALPHA DATA");
                    Error();
                    return;
                }
                n = (byte)ToInteger(a);
            }
            else if (n >= 0xf0)
            {
                a = RecallNumber(n & 0x0f);
                n = (byte)ToInteger(a);
            }
            if (n >= 30)
            {
                Message("NONEXISTENT");
                Error();
                return;
            }
            ClearFlag(n);
            Annunciators();
        }

        void Cle()
        {
            int i;
            int b;
            int ofs;
            b = ram[REG_C + 2] << 4;
            b |= ((ram[REG_C + 1] >> 4) & 0xf);
            ofs = ram[REG_C + 6] << 4;
            ofs |= ((ram[REG_C + 5] >> 4) & 0xf);
            b += ofs;
            for (i = 0; i < 6; i++)
                StoreNumber(ZERO, b + i);
        }

        public void Clp(string name)
        {
            int i;
            int address;
            int size;
            address = FindGlobal(name);
            if (address == 0) return;
            address = FindStart(address);
            while (ram[address] < 0xc0 || ram[address] > 0xcd || ram[address - 2] >= 0xf0)
            {
                if (ram[address] >= 0xc0 && ram[address] <= 0xcd && ram[address - 2] >= 0xf0 && ram[address - 3] != 0x00)
                {
                    SetKaFlag(ram[address - 3], false);
                }
                size = isize(address);
                for (i = 0; i < size; i++)
                    ram[address--] = 0x00;
            }
            if ((ram[address - 2] & 0xf0) != 0x20)
            {
                for (i = 0; i < 3; i++)
                    ram[address--] = 0x00;
            }

            ReLink();
            Pack();
        }

        void Clrg()
        {
            int b;
            b = ram[REG_C + 2] << 4;
            b |= ((ram[REG_C + 1] >> 4) & 0xf);
            while (b <= 0x1ff)
            {
                StoreNumber(ZERO, b);
                b++;
            }
        }

        public void ClearFlag(byte f)
        {
            int offset;
            offset = 6 - (f / 8);
            f = (byte)(f & 7);
            f = (byte)(0x80 >> f);
            ram[REG_D + offset] |= f;
            ram[REG_D + offset] ^= f;
        }

        void ClearLinks(int address)
        {
            int pstart;
            int pend;
            if (linksCleared) return;
            linksCleared = true;
            pend = FindEnd(address);
            pstart = FindStart(address);
            while (ram[pstart] == 0x00) pstart--;
            while (pstart > pend)
            {
                if ((ram[pstart] & 0xf0) == 0xb0)
                {
                    ram[pstart - 1] = 0;
                }
                if ((ram[pstart] & 0xf0) == 0xd0)
                {
                    ram[pstart] &= 0xf0;
                    ram[pstart - 1] = 0;
                }
                if ((ram[pstart] & 0xf0) == 0xe0)
                {
                    ram[pstart] &= 0xf0;
                    ram[pstart - 1] = 0;
                }
                pstart -= isize(pstart);
            }
        }

        Number Cos(Number a)
        {
            int i;
            bool flag;
            DNumber x;
            DNumber fact;
            DNumber one;
            DNumber sum;
            DNumber sqr;
            DNumber num;
            DNumber den;
            DNumber term;
            DNumber last;
            bool addsub;
            x = NumberToDNumber(a);
            one = new DNumber();
            one.sign = 0;
            one.esign = 0;
            one.mantissa[0] = 1;
            for (i = 1; i < 20; i++) one.mantissa[i] = 0;
            for (i = 0; i < 3; i++) one.exponent[i] = 0;
            sum = one;
            den = one;
            fact = one;
            num = one;
            sqr = D_Mul(x, x);
            addsub = true;
            flag = true;
            while (flag)
            {
                last = sum;
                num = D_Mul(num, sqr);
                den = D_Mul(den, fact);
                fact = D_Add(fact, one);
                den = D_Mul(den, fact);
                fact = D_Add(fact, one);
                term = D_Div(num, den);
                term.sign = (byte)((addsub) ? 9 : 0);
                addsub = (addsub) ? false : true;
                sum = D_Add(sum, term);
                flag = false;
                for (i = 0; i < 12; i++)
                    if (last.mantissa[i] != sum.mantissa[i]) flag = true;
            }
            return DNumberToNumber(sum);
        }

        DNumber D_Add(DNumber a, DNumber b)
        {
            int i;
            int ea;
            int eb;
            int c;
            bool addsub;
            int[] temp1 = new int[20];
            int[] temp2 = new int[20];
            DNumber result;
            result = new DNumber();
            addsub = (a.sign == b.sign) ? false : true;
            ea = (a.exponent[0] * 100) + (a.exponent[1] * 10) + a.exponent[2];
            eb = (b.exponent[0] * 100) + (b.exponent[1] * 10) + b.exponent[2];
            if (a.esign == 9) ea = -ea;
            if (b.esign == 9) eb = -eb;
            for (i = 0; i < 20; i++)
            {
                temp1[i] = a.mantissa[i];
                temp2[i] = b.mantissa[i];
            }
            while (ea > eb)
            {
                for (i = 19; i > 0; i--) temp2[i] = temp2[i - 1];
                temp2[0] = 0;
                eb++;
            }
            while (eb > ea)
            {
                for (i = 19; i > 0; i--) temp1[i] = temp1[i - 1];
                temp1[0] = 0;
                ea++;
            }
            c = 0;
            for (i = 0; i < 20; i++) c += temp1[i];
            if (c == 0) return b;
            c = 0;
            for (i = 0; i < 20; i++) c += temp2[i];
            if (c == 0) return a;
            if (addsub)
            {
                if (a.sign != 0) d_tens(temp1);
                if (b.sign != 0) d_tens(temp2);
            }
            c = 0;
            for (i = 19; i >= 0; i--)
            {
                temp1[i] += (temp2[i] + c);
                if (temp1[i] >= 10)
                {
                    temp1[i] -= 10;
                    c = 1;
                }
                else c = 0;
            }
            result.sign = a.sign;
            if (addsub)
            {
                if (c == 0)
                {
                    d_tens(temp1);
                    if (result.sign == 0) result.sign = 9;
                }
                if (c == 1 && result.sign != 0) result.sign = 0;
            }
            if ((addsub == false) && (c != 0))
            {
                for (i = 19; i > 0; i--) temp1[i] = temp1[i - 1];
                temp1[0] = 1;
                ea++;
            }
            while (temp1[0] == 0 &&
                   (temp1[1] != 0 || temp1[2] != 0 ||
                    temp1[3] != 0 || temp1[4] != 0 ||
                    temp1[5] != 0 || temp1[6] != 0 ||
                    temp1[7] != 0 || temp1[8] != 0 ||
                    temp1[9] != 0 || temp1[10] != 0 ||
                    temp1[11] != 0 || temp1[12] != 0 ||
                    temp1[13] != 0 || temp1[14] != 0 ||
                    temp1[15] != 0 || temp1[16] != 0 ||
                    temp1[17] != 0 || temp1[18] != 0 ||
                    temp1[19] != 0))
            {
                for (i = 0; i < 19; i++) temp1[i] = temp1[i + 1];
                temp1[19] = 0;
                ea--;
            }
            if (ea >= 0) result.esign = 0; else result.esign = 9;
            if (ea < 0) ea = -ea;
            result.exponent[0] = (byte)((ea / 100));
            ea = ea % 100;
            result.exponent[1] = (byte)((ea / 10));
            result.exponent[2] = (byte)((ea % 10));
            for (i = 0; i < 20; i++) result.mantissa[i] = (byte)temp1[i];
            return result;
        }

        DNumber D_Div(DNumber a, DNumber b)
        {
            int i;
            int c;
            int ea, eb;
            int f;
            int[] temp1 = new int[21];
            int[] temp2 = new int[21];
            int p;
            bool flag;
            DNumber result;
            result = new DNumber();
            p = 9;
            ea = (a.exponent[0] * 100) + (a.exponent[1] * 10) + a.exponent[2];
            eb = (b.exponent[0] * 100) + (b.exponent[1] * 10) + b.exponent[2];
            if (a.esign != 0) ea = -ea;
            if (b.esign != 0) eb = -eb;
            result.sign = (byte)((a.sign == b.sign) ? 0 : 9);

            for (i = 0; i < 20; i++)
            {
                temp1[i] = a.mantissa[i];
                temp2[i] = b.mantissa[i];
                result.mantissa[i] = 0;
            }

            p = 0;
            while (p < 20)
            {
                c = 0;
                for (i = 0; i < 20; i++) c += temp1[i];
                if (c != 0)
                {
                    f = 0;
                    for (i = 0; i < 20; i++)
                    {
                        if (temp1[i] > temp2[i]) { f = 1; i = 22; }
                        else if (temp1[i] < temp2[i]) { f = -1; i = 22; }
                    }
                    if (f >= 0)
                    {
                        result.mantissa[p]++;
                        c = 0;
                        for (i = 19; i >= 0; i--)
                        {
                            temp1[i] -= (temp2[i] + c);
                            if (temp1[i] < 0)
                            {
                                temp1[i] += 10;
                                c = 1;
                            }
                            else c = 0;
                        }
                    }
                    else
                    {
                        for (i = 19; i > 0; i--) temp2[i] = temp2[i - 1];
                        temp2[0] = 0;
                        p++;
                    }
                }
                else p = 20;
            }
            flag = true;
            while (result.mantissa[0] == 0 && flag)
            {
                flag = false;
                for (i = 0; i < 18; i++)
                {
                    result.mantissa[i] = result.mantissa[i + 1];
                    if (result.mantissa[i] != 0) flag = true;
                }
                result.mantissa[19] = 0;
                ea--;
            }
            ea -= eb;
            result.esign = (byte)((ea < 0) ? 9 : 0);
            if (ea < 0) ea = -ea;
            result.exponent[0] = (byte)(ea / 100);
            ea = ea % 100;
            result.exponent[1] = (byte)(ea / 10);
            result.exponent[2] = (byte)(ea % 10);
            return result;
        }

        Number Dec(Number a)
        {
            int i;
            int n;
            string tmp;
            if (a.esign != 0)
            {
                Message("DATA ERROR");
                Error();
                return a;
            }
            if (a.exponent[0] != 0)
            {
                Message("DATA ERROR");
                Error();
                return a;
            }
            for (i = a.exponent[1] + 1; i < 10; i++)
                if (a.mantissa[i] != 0)
                {
                    Message("DATA ERROR");
                    Error();
                    return a;
                }
            for (i = 0; i < 10; i++)
                if (a.mantissa[i] > 7)
                {
                    Message("DATA ERROR");
                    Error();
                    return a;
                }
            n = a.mantissa[0];
            for (i = 1; i <= a.exponent[1]; i++)
            {
                n *= 8;
                n += a.mantissa[i];
            }
            tmp = $"{n:d}";
            a = AtoN(tmp);
            return a;
        }

        public void Del(int n)
        {
            int addr;
            int s;
            char relink;
            int lineNumber;
            relink = 'N';
            if (FlagSet(52) == false) return;
            lineNumber = ((ram[REG_E + 1] & 0x0f) << 8) | ram[REG_E + 0];
            if (lineNumber == 0xfff)
            {
                FixLineNumber();
                lineNumber = ((ram[REG_E + 1] & 0x0f) << 8) | ram[REG_E + 0];
            }
            if (lineNumber > 0) lineNumber--;
            addr = FromPtr((ram[REG_B + 1] << 8) | ram[REG_B + 0]);
            addr--;
            while (n > 0)
            {
                while (ram[addr] == 0x00) addr--;
                if (ram[addr] >= 0xc0 && ram[addr] <= 0xcd && ram[addr - 2] >= 0xf0) relink = 'Y';
                if (ram[addr] >= 0xc0 && ram[addr] <= 0xcd && ram[addr - 2] < 0xf0) n = 0;
                else
                {
                    s = isize(addr);
                    while (s > 0)
                    {
                        ram[addr--] = 0;
                        s--;
                    }
                    n--;
                }
            }
            if (relink == 'Y') ReLink();
            ram[REG_E+1] = (byte)((lineNumber >> 8) & 0xff);
            ram[REG_E + 0] = (byte)(lineNumber & 0xff);
            if (lineNumber > 0) GotoLine(lineNumber);
        }

        Number Div(Number a, Number b)
        {
            DNumber da;
            DNumber db;
            DNumber dc;
            if (IsZero(b))
            {
                Message("DATA ERROR");
                Error();
                return b;
            }
            da = NumberToDNumber(a);
            db = NumberToDNumber(b);
            dc = D_Div(da, db);
            return DNumberToNumber(dc);
        }

        int Dse(byte post)
        {
            int i;
            int p;
            int e;
            int count;
            int final;
            int inc;
            Number x;
            Number y;
            string tmp;
            x = Rcl(post);
            e = x.exponent[0] * 10 + x.exponent[1];
            if (x.esign == 9) e = -e;
            count = 0;
            final = 0;
            inc = 0;
            p = 0;
            while (e >= 0)
            {
                count *= 10;
                if (p < 10) count += x.mantissa[p++];
                e--;
            }
            for (i = 0; i < 3; i++)
            {
                final = (final * 10);
                if (p < 10) final += x.mantissa[p++];
            }
            for (i = 0; i < 2; i++)
            {
                inc = (inc * 10);
                if (p < 10) inc += x.mantissa[p++];
            }
            if (inc == 0) inc = 1;
            if (x.sign == 9) count = -count;
            count -= inc;
            tmp = $"{count:d}.{final:d3}{inc:d2}";
            y = AtoN(tmp);
            if (count < 0) y.sign = 9;
            Sto(y, post);
            if (count <= final) return -1;
            return 0;
        }

        Number DNumberToNumber(DNumber a)
        {
            int i;
            Number r;
            r = new Number();
            r.sign = a.sign;
            r.esign = a.esign;
            for (i = 0; i < 10; i++) r.mantissa[i] = a.mantissa[i];
            r.exponent[0] = a.exponent[1];
            r.exponent[1] = a.exponent[2];
            return r;
        }

        DNumber D_Mul(DNumber a, DNumber b)
        {
            int i;
            int t;
            int j;
            int c;
            int ea, eb;
            byte[] temp = new byte[41];
            int p;
            DNumber result;
            result = new DNumber();
            p = 9;
            ea = (a.exponent[0] * 100) + (a.exponent[1] * 10) + a.exponent[2];
            eb = (b.exponent[0] * 100) + (b.exponent[1] * 10) + b.exponent[2];
            if (a.esign != 0) ea = -ea;
            if (b.esign != 0) eb = -eb;
            result.sign = (byte)((a.sign == b.sign) ? 0 : 9);
            ea += eb;
            for (i = 0; i < 40; i++) temp[i] = 0;
            for (p = 19; p >= 0; p--)
            {
                for (i = 0; i < b.mantissa[p]; i++)
                {
                    c = 0;
                    for (j = 19; j >= 0; j--)
                    {
                        temp[p + j + 2] += (byte)((a.mantissa[j] + c));
                        if (temp[p + j + 2] >= 10)
                        {
                            temp[p + j + 2] -= 10;
                            c = 1;
                        }
                        else c = 0;
                    }
                    if (c != 0)
                    {
                        temp[p + 1]++;
                    }
                }
            }
            p = 1;
            ea++;
            t = 0;
            for (i = 0; i < 19; i++) t += temp[i];
            if (t == 0) return D_ZERO;
            while (temp[p] == 0)
            {
                p++;
                ea--;
            }
            for (i = 0; i < 19; i++)
                if (p + i < 19) result.mantissa[i] = temp[p + i];
                else result.mantissa[i] = 0;
            result.esign = (byte)((ea < 0) ? 9 : 0);
            if (ea < 0) ea = -ea;
            result.exponent[0] = (byte)((ea / 100));
            ea = ea % 100;
            result.exponent[1] = (byte)(ea / 10);
            result.exponent[2] = (byte)(ea % 10);
            return result;
        }

        DNumber D_Sub(DNumber a, DNumber b)
        {
            b.sign = (byte)((b.sign == 0) ? 9 : 0);
            return D_Add(a, b);
        }

        void d_tens(int[] n)
        {
            int i;
            int c;
            for (i = 0; i < 20; i++) n[i] = 9 - n[i];
            c = 1;
            for (i = 19; i >= 0; i--)
            {
                n[i] += c;
                if (n[i] > 9)
                {
                    n[i] -= 10;
                    c = 1;
                }
                else c = 0;
            }
        }

        void EMinus()
        {
            int b;
            int ofs;
            Number x;
            Number y;
            b = ram[REG_C + 2] << 4;
            b |= ((ram[REG_C + 1] >> 4) & 0xf);
            ofs = ram[REG_C + 6] << 4;
            ofs |= ((ram[REG_C + 5] >> 4) & 0xf);
            b += ofs;
            a = RecallNumber(b);
            x = RecallNumber(R_X);
            c = Sub(a, x);
            StoreNumber(c, b);
            a = RecallNumber(b + 1);
            c = Mul(x, x);
            a = Sub(a, c);
            StoreNumber(a, b + 1);
            a = RecallNumber(b + 2);
            y = RecallNumber(R_Y);
            c = Sub(a, y);
            StoreNumber(c, b + 2);
            a = RecallNumber(b + 3);
            c = Mul(y, y);
            a = Sub(a, c);
            StoreNumber(c, b + 3);
            a = RecallNumber(b + 4);
            c = Mul(x, y);
            a = Sub(a, c);
            StoreNumber(a, b + 4);
            a = RecallNumber(b + 5);
            a = Sub(a, S_ONE);
            StoreNumber(a, b + 5);
            StoreNumber(a, R_X);
        }


        void Eng(byte n)
        {
            Number a;
            if (n >= 0x80 && n <= 0xef)
            {
                a = Rcl(n & 0x7f);
                if (a.sign != 9 && a.sign != 0)
                {
                    Message("ALPHA DATA");
                    Error();
                    return;
                }
                n = (byte)ToInteger(a);
            }
            else if (n >= 0xf0)
            {
                a = RecallNumber(n & 0x0f);
                n = (byte)ToInteger(a);
            }
            if (n >= 0x0a)
            {
                Message("DATA ERRROR");
                Error();
                return;
            }
            ClearFlag(40);
            SetFlag(41);
            if ((n & 8) != 0) SetFlag(36); else ClearFlag(36);
            if ((n & 4) != 0) SetFlag(37); else ClearFlag(37);
            if ((n & 2) != 0) SetFlag(38); else ClearFlag(38);
            if ((n & 1) != 0) SetFlag(39); else ClearFlag(39);
        }

        void EPlus()
        {
            int b;
            int ofs;
            Number x;
            Number y;
            b = ram[REG_C + 2] << 4;
            b |= ((ram[REG_C + 1] >> 4) & 0xf);
            ofs = ram[REG_C + 6] << 4;
            ofs |= ((ram[REG_C + 5] >> 4) & 0xf);
            b += ofs;
            a = RecallNumber(b);
            x = RecallNumber(R_X);
            c = Add(a, x);
            StoreNumber(c, b);
            a = RecallNumber(b + 1);
            c = Mul(x, x);
            a = Add(a, c);
            StoreNumber(a, b + 1);
            a = RecallNumber(b + 2);
            y = RecallNumber(R_Y);
            c = Add(a, y);
            StoreNumber(c, b + 2);
            a = RecallNumber(b + 3);
            c = Mul(y, y);
            a = Add(a, c);
            StoreNumber(a, b + 3);
            a = RecallNumber(b + 4);
            c = Mul(x, y);
            a = Add(a, c);
            StoreNumber(a, b + 4);
            a = RecallNumber(b + 5);
            a = Add(a, S_ONE);
            StoreNumber(a, b + 5);
            StoreNumber(a, R_X);
        }

        void EReg(byte post)
        {
            int b;
            int e;
            b = ram[REG_C + 2] << 4;
            b |= ((ram[REG_C + 1] >> 4) & 0xf);
            e = 0xffff;
            if (post < 0x70) e = post;
            if (post >= 0x80 && post <= 0xef)
            {
                e = ToInteger(RecallNumber(b + (post & 0x7f)));
            }
            if (post >= 0xf0 && post <= 0xff)
            {
                e = ToInteger(RecallNumber(post & 0x0f));
            }
            if (b + e > 0xfff - 6)
            {
                Message("NONEXISTENT");
                Error();
                return;
            }
            ram[REG_C + 6] = (byte)((e >> 4) & 0xff);
            ram[REG_C + 5] &= 0x0f;
            ram[REG_C + 5] |= (byte)((e & 0xf) << 4);
        }

        public void Error()
        {
            if (FlagSet(25))
            {
                ClearFlag(25);
                return;
            }
            errFlag = true;
            running = false;
        }

        Number Ex(Number a)
        {
            int i;
            bool flag;
            DNumber one;
            DNumber x;
            DNumber sum;
            DNumber num;
            DNumber den;
            DNumber term;
            DNumber last;
            DNumber fact;
            x = NumberToDNumber(a);
            one = D_ONE;
            sum = D_Add(x, one);
            num = x;
            den = one;
            fact = one;
            flag = true;
            while (flag)
            {
                last = sum;
                num = D_Mul(num, x);
                fact = D_Add(fact, one);
                den = D_Mul(den, fact);
                term = D_Div(num, den);
                sum = D_Add(sum, term);
                flag = false;
                for (i = 0; i < 15; i++)
                    if (last.mantissa[i] != sum.mantissa[i]) flag = true;
            }
            return DNumberToNumber(sum);
        }

        int Exec(int addr)
        {
            byte cmd;
            int byt;
            bool flag;
            int i;
            int j;
            int oaddr;
            string buffer;
            byte b2;
            double d;
            double x;
            double y;
            string n;
            oaddr = addr;
            cmd = 0;
            errFlag = false;
            while (cmd == 0) cmd = ram[addr--];
            if (running == false && cmd == 0x54 && FlagSet(22)) cmd = 0x1c;
            //            if (debug)
            //            {
            //                ProgramList(1, addr + 1, buffer);
            //                printf("-->%s\n", buffer);
            //            }
            if (running == false)
            {
                if ((cmd < 0x10 || cmd > 0x1c) && FlagSet(22))
                {
                    EndNumber();
                    ram[PENDING] = (byte)'E';
                }
                ram[PENDING] = (byte)((FlagSet(22) == false) ? 'E' : 'D');
            }
            else ram[PENDING] = (byte)'E';
            switch (cmd)
            {
                case 0x00:                                             // NULL
                    break;
                case 0x01:                                             // LBL 00
                case 0x02:                                             // LBL 01
                case 0x03:                                             // LBL 02
                case 0x04:                                             // LBL 03
                case 0x05:                                             // LBL 04
                case 0x06:                                             // LBL 05
                case 0x07:                                             // LBL 06
                case 0x08:                                             // LBL 07
                case 0x09:                                             // LBL 08
                case 0x0a:                                             // LBL 09
                case 0x0b:                                             // LBL 10
                case 0x0c:                                             // LBL 11
                case 0x0d:                                             // LBL 12
                case 0x0e:                                             // LBL 13
                case 0x0f:                                             // LBL 14
                    if (running)
                    {
                        goose = goose.Substring(11) + goose.Substring(0, 11);
                        window.Display(goose);
                    }
                    break;

                case 0x10:                                             // 0
                case 0x11:                                             // 1
                case 0x12:                                             // 2
                case 0x13:                                             // 3
                case 0x14:                                             // 4
                case 0x15:                                             // 5
                case 0x16:                                             // 6
                case 0x17:                                             // 7
                case 0x18:                                             // 8
                case 0x19:                                             // 9
                case 0x1a:                                             // .
                case 0x1b:                                             // EEX
                case 0x1c:                                             // CHS
                    if (running)
                    {
                        addr = ExecNumber(addr + 1);
                    }
                    else
                    {
                        AddNumber((char)(cmd - 0x10));
                    }
                    break;
                case 0x1d:                                             // GTO"
                    addr = GtoAlpha(addr);
                    addr--;
                    break;
                case 0x1e:                                             // XEQ"
                    addr = GtoAlpha(addr);
                    if (addr != 0)
                    {
                        Push((oaddr) - isize(oaddr - 1));
                    }
                    break;


                case 0x20:                                             // RCL 00
                    a = Rcl(0);
                    StoreNumber(a, R_X);
                    break;
                case 0x21:                                             // RCL 01
                    a = Rcl(1);
                    StoreNumber(a, R_X);
                    break;
                case 0x22:                                             // RCL 02
                    a = Rcl(2);
                    StoreNumber(a, R_X);
                    break;
                case 0x23:                                             // RCL 03
                    a = Rcl(3);
                    StoreNumber(a, R_X);
                    break;
                case 0x24:                                             // RCL 04
                    a = Rcl(4);
                    StoreNumber(a, R_X);
                    break;
                case 0x25:                                             // RCL 05
                    a = Rcl(5);
                    StoreNumber(a, R_X);
                    break;
                case 0x26:                                             // RCL 06
                    a = Rcl(6);
                    StoreNumber(a, R_X);
                    break;
                case 0x27:                                             // RCL 07
                    a = Rcl(7);
                    StoreNumber(a, R_X);
                    break;
                case 0x28:                                             // RCL 08
                    a = Rcl(8);
                    StoreNumber(a, R_X);
                    break;
                case 0x29:                                             // RCL 09
                    a = Rcl(9);
                    StoreNumber(a, R_X);
                    break;
                case 0x2a:                                             // RCL 10
                    a = Rcl(10);
                    StoreNumber(a, R_X);
                    break;
                case 0x2b:                                             // RCL 11
                    a = Rcl(11);
                    StoreNumber(a, R_X);
                    break;
                case 0x2c:                                             // RCL 12
                    a = Rcl(12);
                    StoreNumber(a, R_X);
                    break;
                case 0x2d:                                             // RCL 13
                    a = Rcl(13);
                    StoreNumber(a, R_X);
                    break;
                case 0x2e:                                             // RCL 14
                    a = Rcl(14);
                    StoreNumber(a, R_X);
                    break;
                case 0x2f:                                             // RCL 15
                    a = Rcl(15);
                    StoreNumber(a, R_X);
                    break;

                case 0x30:                                             // STO 00
                    a = RecallNumber(R_X);
                    Sto(a, 0);
                    break;
                case 0x31:                                             // STO 01
                    a = RecallNumber(R_X);
                    Sto(a, 1);
                    break;
                case 0x32:                                             // STO 02
                    a = RecallNumber(R_X);
                    Sto(a, 2);
                    break;
                case 0x33:                                             // STO 03
                    a = RecallNumber(R_X);
                    Sto(a, 3);
                    break;
                case 0x34:                                             // STO 04
                    a = RecallNumber(R_X);
                    Sto(a, 4);
                    break;
                case 0x35:                                             // STO 05
                    a = RecallNumber(R_X);
                    Sto(a, 5);
                    break;
                case 0x36:                                             // STO 06
                    a = RecallNumber(R_X);
                    Sto(a, 6);
                    break;
                case 0x37:                                             // STO 07
                    a = RecallNumber(R_X);
                    Sto(a, 7);
                    break;
                case 0x38:                                             // STO 08
                    a = RecallNumber(R_X);
                    Sto(a, 8);
                    break;
                case 0x39:                                             // STO 09
                    a = RecallNumber(R_X);
                    Sto(a, 9);
                    break;
                case 0x3a:                                             // STO 10
                    a = RecallNumber(R_X);
                    Sto(a, 10);
                    break;
                case 0x3b:                                             // STO 11
                    a = RecallNumber(R_X);
                    Sto(a, 11);
                    break;
                case 0x3c:                                             // STO 12
                    a = RecallNumber(R_X);
                    Sto(a, 12);
                    break;
                case 0x3d:                                             // STO 13
                    a = RecallNumber(R_X);
                    Sto(a, 13);
                    break;
                case 0x3e:                                             // STO 14
                    a = RecallNumber(R_X);
                    Sto(a, 14);
                    break;
                case 0x3f:                                             // STO 15
                    a = RecallNumber(R_X);
                    Sto(a, 15);
                    break;

                case 0x40:                                             // +
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_X);
                    b = RecallNumber(R_Y);
                    if ((a.sign == 0 || a.sign == 9) && (b.sign == 0 || b.sign == 9))
                    {
                        c = Add(a, b);
                        StoreNumber(c, R_X);
                        StoreNumber(a, R_L);
                        a = RecallNumber(R_Z);
                        StoreNumber(a, R_Y);
                        a = RecallNumber(R_T);
                        StoreNumber(a, R_Z);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x41:                                             // -
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_X);
                    b = RecallNumber(R_Y);
                    if ((a.sign == 0 || a.sign == 9) && (b.sign == 0 || b.sign == 9))
                    {
                        c = Sub(b, a);
                        StoreNumber(c, R_X);
                        StoreNumber(a, R_L);
                        a = RecallNumber(R_Z);
                        StoreNumber(a, R_Y);
                        a = RecallNumber(R_T);
                        StoreNumber(a, R_Z);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x42:                                             // *
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_X);
                    b = RecallNumber(R_Y);
                    if ((a.sign == 0 || a.sign == 9) && (b.sign == 0 || b.sign == 9))
                    {
                        c = Mul(a, b);
                        StoreNumber(c, R_X);
                        StoreNumber(a, R_L);
                        a = RecallNumber(R_Z);
                        StoreNumber(a, R_Y);
                        a = RecallNumber(R_T);
                        StoreNumber(a, R_Z);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x43:                                             // /
                    ram[LIFT] = (byte)'D';
                    b = RecallNumber(R_X);
                    a = RecallNumber(R_Y);
                    if ((a.sign == 0 || a.sign == 9) && (b.sign == 0 || b.sign == 9))
                    {
                        c = Div(a, b);
                        StoreNumber(c, R_X);
                        StoreNumber(b, R_L);
                        a = RecallNumber(R_Z);
                        StoreNumber(a, R_Y);
                        a = RecallNumber(R_T);
                        StoreNumber(a, R_Z);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x44:                                             // X<Y?
                    a = RecallNumber(R_X);
                    b = RecallNumber(R_Y);
                    if ((a.sign == 0 || a.sign == 9) && (b.sign == 0 || b.sign == 9))
                    {
                        c = Sub(a, b);
                        if (addr < 0x0c0)
                        {
                            if (c.sign != 0) Message("YES");
                            else Message("NO");
                        }
                        else
                        {
                            if (c.sign == 0) addr = Skip(addr);
                        }
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x45:                                             // X>Y?
                    b = RecallNumber(R_X);
                    a = RecallNumber(R_Y);
                    if ((a.sign == 0 || a.sign == 9) && (b.sign == 0 || b.sign == 9))
                    {
                        c = Sub(a, b);
                        if (addr < 0x0c0)
                        {
                            if (c.sign != 0) Message("YES");
                            else Message("NO");
                        }
                        else
                        {
                            if (c.sign == 0) addr = Skip(addr);
                        }
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x46:                                             // X<=Y?
                    a = RecallNumber(R_X);
                    b = RecallNumber(R_Y);
                    if ((a.sign == 0 || a.sign == 9) && (b.sign == 0 || b.sign == 9))
                    {
                        c = Sub(a, b);
                        flag = false;
                        if (c.sign != 0) flag = true;
                        if (IsZero(c)) flag = true;
                        if (addr < 0x0c0)
                        {
                            if (flag) Message("YES");
                            else Message("NO");
                        }
                        else
                        {
                            if (flag == false) addr = Skip(addr);
                        }
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x47:                                             // E+
                    a = RecallNumber(R_X);
                    StoreNumber(a, R_L);
                    EPlus();
                    ram[PENDING] = (byte)'D';
                    break;
                case 0x48:                                             // E-
                    a = RecallNumber(R_X);
                    StoreNumber(a, R_L);
                    EMinus();
                    ram[PENDING] = (byte)'D';
                    break;
                case 0x49:                                             // HMS+
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_X);
                    b = RecallNumber(R_Y);
                    if ((a.sign == 0 || a.sign == 9) && (b.sign == 0 || b.sign == 9))
                    {
                        StoreNumber(a, R_L);
                        a = HmsPlus(a, b);
                        StoreNumber(a, R_X);
                        a = RecallNumber(R_Z);
                        StoreNumber(a, R_Y);
                        a = RecallNumber(R_T);
                        StoreNumber(a, R_Z);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x4a:                                             // HMS-
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_X);
                    b = RecallNumber(R_Y);
                    if ((a.sign == 0 || a.sign == 9) && (b.sign == 0 || b.sign == 9))
                    {
                        StoreNumber(a, R_L);
                        a = HmsMinus(a, b);
                        StoreNumber(a, R_X);
                        a = RecallNumber(R_Z);
                        StoreNumber(a, R_Y);
                        a = RecallNumber(R_T);
                        StoreNumber(a, R_Z);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x4b:                                             // MOD
                    b = RecallNumber(R_X);
                    a = RecallNumber(R_Y);
                    if ((a.sign == 0 || a.sign == 9) && (b.sign == 0 || b.sign == 9))
                    {
                        c = Mod(a, b);
                        StoreNumber(c, R_X);
                        StoreNumber(b, R_L);
                        a = RecallNumber(R_Z);
                        StoreNumber(a, R_Y);
                        a = RecallNumber(R_T);
                        StoreNumber(a, R_Z);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x4c:                                             // %
                    a = RecallNumber(R_Y);
                    b = RecallNumber(R_X);
                    if ((a.sign == 0 || a.sign == 9) && (b.sign == 0 || b.sign == 9))
                    {
                        StoreNumber(b, R_L);
                        i = b.exponent[0] * 10 + b.exponent[1];
                        if (b.esign != 0) i = -i;
                        i -= 2;
                        b.esign = (byte)((i < 0) ? 9 : 0);
                        if (i < 0) i = -i;
                        b.exponent[0] = (byte)(i / 10);
                        b.exponent[1] = (byte)(i % 10);
                        c = Mul(a, b);
                        StoreNumber(c, R_X);
                        a = RecallNumber(R_Z);
                        StoreNumber(a, R_Y);
                        a = RecallNumber(R_T);
                        StoreNumber(a, R_Z);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x4d:                                             // %ch
                    a = RecallNumber(R_Y);
                    b = RecallNumber(R_X);
                    if ((a.sign == 0 || a.sign == 9) && (b.sign == 0 || b.sign == 9))
                    {
                        StoreNumber(b, R_L);
                        c = Sub(b, a);
                        i = c.exponent[0] * 10 + c.exponent[1];
                        if (c.esign != 0) i = -i;
                        i += 2;
                        c.esign = (byte)((i < 0) ? 9 : 0);
                        if (i < 0) i = -i;
                        c.exponent[0] = (byte)(i / 10);
                        c.exponent[1] = (byte)(i % 10);
                        c = Div(c, a);
                        StoreNumber(c, R_X);
                        a = RecallNumber(R_Z);
                        StoreNumber(a, R_Y);
                        a = RecallNumber(R_T);
                        StoreNumber(a, R_Z);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x4e:                                             // P-R
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        n = NtoA(a);
                        x = Convert.ToDouble(n);
                        b = RecallNumber(R_Y);
                        if (FlagSet(43) == false && FlagSet(42) == false) b = Mul(b, DTOR);
                        else if (FlagSet(42)) b = Mul(b, GTOR);
                        n = NtoA(b);
                        y = Convert.ToDouble(n);
                        d = Math.Cos(y) * x;
                        n = $"{d:e12}";
                        a = AtoN(n);
                        SetX(a, 1, 0);
                        d = Math.Sin(y) * x;
                        n = $"{d:e12}";
                        a = AtoN(n);
                        StoreNumber(a, R_Y);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x4f:                                             // R-P
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        n = NtoA(a);
                        x = Convert.ToDouble(n);
                        b = RecallNumber(R_Y);
                        n = NtoA(b);
                        y = Convert.ToDouble(n);
                        d = Math.Sqrt(x * x + y * y);
                        n = $"{d:e12}";
                        a = AtoN(n);
                        SetX(a, 1, 0);
                        d = Math.Asin(y / d);
                        if (x < 0 && y >= 0) d = Math.PI - d;
                        if (x < 0 && y < 0) d = -(Math.PI + d);
                        n = $"{d:e12}";
                        a = AtoN(n);
                        if (FlagSet(43) == false && FlagSet(42) == false) a = Mul(a, RTOD);
                        else if (FlagSet(42)) a = Mul(a, RTOG);
                        StoreNumber(a, R_Y);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;

                case 0x50:                                             // LN
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        StoreNumber(a, R_L);
                        a = Ln(a);
                        StoreNumber(a, R_X);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x51:                                             // X^2
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        StoreNumber(a, R_L);
                        a = Mul(a, a);
                        StoreNumber(a, R_X);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x52:                                             // SQRT
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        StoreNumber(a, R_L);
                        a = Sqrt(a);
                        StoreNumber(a, R_X);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x53:                                             // Y^X
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        StoreNumber(a, R_L);
                        b = RecallNumber(R_Y);
                        a = YtoX(a, b);
                        StoreNumber(a, R_X);
                        a = RecallNumber(R_Z);
                        StoreNumber(a, R_Y);
                        a = RecallNumber(R_T);
                        StoreNumber(a, R_Z);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x54:                                             // CHS
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        a.sign = (byte)((a.sign == 0) ? 9 : 0);
                        SetX(a, 0, 0);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x55:                                             // E^X
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        StoreNumber(a, R_L);
                        a = Ex(a);
                        StoreNumber(a, R_X);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x56:                                             // LOG
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        StoreNumber(a, R_L);
                        a = Ln(a);
                        a = Mul(a, LOGE);
                        StoreNumber(a, R_X);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x57:                                             // 10^X
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        StoreNumber(a, R_L);
                        a = Mul(a, ILOGE);
                        a = Ex(a);
                        StoreNumber(a, R_X);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x58:                                             // E^X-1
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        StoreNumber(a, R_L);
                        a = Ex(a);
                        a = Sub(a, S_ONE);
                        StoreNumber(a, R_X);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x59:                                             // SIN
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        StoreNumber(a, R_L);
                        if (FlagSet(43) == false && FlagSet(42) == false) a = Mul(a, DTOR);
                        else if (FlagSet(42)) a = Mul(a, GTOR);
                        a = Sin(a);
                        StoreNumber(a, R_X);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x5a:                                             // COS
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        StoreNumber(a, R_L);
                        if (FlagSet(43) == false && FlagSet(42) == false) a = Mul(a, DTOR);
                        else if (FlagSet(42)) a = Mul(a, GTOR);
                        a = Cos(a);
                        StoreNumber(a, R_X);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x5b:                                             // TAN
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        StoreNumber(a, R_L);
                        if (FlagSet(43) == false && FlagSet(42) == false) a = Mul(a, DTOR);
                        else if (FlagSet(42)) a = Mul(a, GTOR);
                        a = Tan(a);
                        StoreNumber(a, R_X);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x5c:                                             // ASIN
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        StoreNumber(a, R_L);
                        a = Asin(a);
                        if (FlagSet(43) == false && FlagSet(42) == false) a = Mul(a, RTOD);
                        else if (FlagSet(42)) a = Mul(a, RTOG);
                        StoreNumber(a, R_X);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x5d:                                             // ACOS
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        StoreNumber(a, R_L);
                        a = Acos(a);
                        if (FlagSet(43) == false && FlagSet(42) == false) a = Mul(a, RTOD);
                        else if (FlagSet(42)) a = Mul(a, RTOG);
                        StoreNumber(a, R_X);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x5e:                                             // ATAN
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        StoreNumber(a, R_L);
                        a = Atan(a);
                        if (FlagSet(43) == false && FlagSet(42) == false) a = Mul(a, RTOD);
                        else if (FlagSet(42)) a = Mul(a, RTOG);
                        StoreNumber(a, R_X);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x5f:                                             // DEC
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        StoreNumber(a, R_L);
                        a = Dec(a);
                        StoreNumber(a, R_X);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;

                case 0x60:                                             // 1/X
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        StoreNumber(a, R_L);
                        a = Div(S_ONE, a);
                        StoreNumber(a, R_X);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x61:                                             // ABS
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        StoreNumber(a, R_L);
                        a.sign = 0;
                        StoreNumber(a, R_X);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x62:                                             // FACT
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        StoreNumber(a, R_L);
                        a = Fact(a);
                        StoreNumber(a, R_X);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x63:                                             // X<>0?
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        flag = false;
                        for (i = 0; i < 10; i++)
                            if (a.mantissa[i] != 0) flag = true;
                        if (addr < 0x0c0)
                        {
                            if (flag) Message("YES");
                            else Message("NO");
                        }
                        else
                        {
                            if (flag == false) addr = Skip(addr);
                        }
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x64:                                             // X>0?
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        flag = false;
                        for (i = 0; i < 10; i++)
                            if (a.mantissa[i] != 0) flag = true;
                        if (a.sign != 0) flag = false;
                        if (addr < 0x0c0)
                        {
                            if (flag) Message("YES");
                            else Message("NO");
                        }
                        else
                        {
                            if (flag == false) addr = Skip(addr);
                        }
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x65:                                             // LN1+X
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        StoreNumber(a, R_L);
                        a = Ln1PlusX(a);
                        StoreNumber(a, R_X);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x66:                                             // X<0?
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        flag = false;
                        for (i = 0; i < 10; i++)
                            if (a.mantissa[i] != 0) flag = true;
                        if (a.sign == 0) flag = false;
                        if (addr < 0x0c0)
                        {
                            if (flag) Message("YES");
                            else Message("NO");
                        }
                        else
                        {
                            if (flag == false) addr = Skip(addr);
                        }
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x67:                                             // X=0?
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        flag = true;
                        for (i = 0; i < 10; i++)
                            if (a.mantissa[i] != 0) flag = false;
                        if (addr < 0x0c0)
                        {
                            if (flag) Message("YES");
                            else Message("NO");
                        }
                        else
                        {
                            if (flag == false) addr = Skip(addr);
                        }
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x68:                                             // INT
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        StoreNumber(a, R_L);
                        a = Integer(a);
                        StoreNumber(a, R_X);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x69:                                             // FRC
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        StoreNumber(a, R_L);
                        a = Fractional(a);
                        StoreNumber(a, R_X);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x6a:                                             // D-R
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        StoreNumber(a, R_L);
                        a = Mul(a, DTOR);
                        StoreNumber(a, R_X);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x6b:                                             // R-D
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        StoreNumber(a, R_L);
                        a = Mul(a, RTOD);
                        StoreNumber(a, R_X);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x6c:                                             // HMS
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        StoreNumber(a, R_L);
                        a = Hms(a);
                        StoreNumber(a, R_X);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x6d:                                             // HR
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        StoreNumber(a, R_L);
                        a = Hr(a);
                        StoreNumber(a, R_X);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x6e:                                             // RND
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        StoreNumber(a, R_L);
                        Rnd();
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x6f:                                             // OCT
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_X);
                    StoreNumber(a, R_L);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        a = Oct(a);
                        StoreNumber(a, R_X);
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;

                case 0x70:                                             // CLE
                    Cle();
                    break;
                case 0x71:                                             // X<>Y
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_X);
                    b = RecallNumber(R_Y);
                    StoreNumber(b, R_X);
                    StoreNumber(a, R_Y);
                    break;
                case 0x72:                                             // PI
                    StoreNumber(S_PI, R_X);
                    break;
                case 0x73:                                             // CLST
                    StoreNumber(ZERO, R_X);
                    StoreNumber(ZERO, R_Y);
                    StoreNumber(ZERO, R_Z);
                    StoreNumber(ZERO, R_T);
                    break;
                case 0x74:                                             // R^
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_T);
                    b = RecallNumber(R_Z);
                    StoreNumber(b, R_T);
                    b = RecallNumber(R_Y);
                    StoreNumber(b, R_Z);
                    b = RecallNumber(R_X);
                    StoreNumber(b, R_Y);
                    StoreNumber(a, R_X);
                    break;
                case 0x75:                                             // RDN
                    ram[LIFT] = (byte)'D';
                    a = RecallNumber(R_X);
                    b = RecallNumber(R_Y);
                    StoreNumber(b, R_X);
                    b = RecallNumber(R_Z);
                    StoreNumber(b, R_Y);
                    b = RecallNumber(R_T);
                    StoreNumber(b, R_Z);
                    StoreNumber(a, R_T);
                    break;
                case 0x76:                                             // LASTX
                    a = RecallNumber(R_L);
                    StoreNumber(a, R_X);
                    break;
                case 0x77:                                             // CLX
                    ram[LIFT] = (byte)'D';
                    StoreNumber(ZERO, R_X);
                    ram[PENDING] = (byte)'D';
                    break;
                case 0x78:                                             // X=Y?
                    a = RecallNumber(R_X);
                    b = RecallNumber(R_Y);
                    flag = true;
                    if (a.sign != b.sign) flag = false;
                    if (a.esign != b.esign) flag = false;
                    for (i = 0; i < 10; i++)
                        if (a.mantissa[i] != b.mantissa[i]) flag = false;
                    for (i = 0; i < 2; i++)
                        if (a.exponent[i] != b.exponent[i]) flag = false;
                    if (addr < 0x0c0)
                    {
                        if (flag) Message("YES");
                        else Message("NO");
                    }
                    else
                    {
                        if (flag == false) addr = Skip(addr);
                    }
                    break;
                case 0x79:                                             // X<>Y?
                    a = RecallNumber(R_X);
                    b = RecallNumber(R_Y);
                    flag = false;
                    if (a.sign != b.sign) flag = true;
                    if (a.esign != b.esign) flag = true;
                    for (i = 0; i < 10; i++)
                        if (a.mantissa[i] != b.mantissa[i]) flag = true;
                    for (i = 0; i < 2; i++)
                        if (a.exponent[i] != b.exponent[i]) flag = true;
                    if (addr < 0x0c0)
                    {
                        if (flag) Message("YES");
                        else Message("NO");
                    }
                    else
                    {
                        if (flag == false) addr = Skip(addr);
                    }
                    break;
                case 0x7a:                                             // SIGN
                    a = RecallNumber(R_X);
                    StoreNumber(a, R_L);
                    if (a.mantissa[0] == 0 && a.mantissa[1] == 0 &&
                        a.mantissa[2] == 0 && a.mantissa[3] == 0 &&
                        a.mantissa[4] == 0 && a.mantissa[5] == 0 &&
                        a.mantissa[6] == 0 && a.mantissa[7] == 0 &&
                        a.mantissa[8] == 0 && a.mantissa[9] == 0)
                        StoreNumber(S_ONE, R_X);
                    else if (a.sign == 0) StoreNumber(S_ONE, R_X);
                    else if (a.sign != 0) StoreNumber(S_NEGONE, R_X);
                    break;
                case 0x7b:                                             // X<=0?
                    a = RecallNumber(R_X);
                    if (a.sign == 0 || a.sign == 9)
                    {
                        if (a.sign != 0) flag = true;
                        else
                        {
                            flag = true;
                            for (i = 0; i < 10; i++)
                                if (a.mantissa[i] != 0) flag = false;
                        }
                        if (addr < 0x0c0)
                        {
                            if (flag) Message("YES");
                            else Message("NO");
                        }
                        else
                        {
                            if (flag == false) addr = Skip(addr);
                        }
                    }
                    else
                    {
                        Message("ALPHA DATA");
                        Error();
                    }
                    break;
                case 0x7c:                                             // MEAN
                    Mean();
                    break;
                case 0x7d:                                             // SDEV
                    Sdev();
                    break;
                case 0x7e:                                             // AVIEW
                    Aview();
                    SetFlag(50);
                    break;
                case 0x7f:                                             // CLD
                    ClearFlag(50);
                    break;

                case 0x80:                                             // DEG
                    ClearFlag(42);
                    ClearFlag(43);
                    break;
                case 0x81:                                             // RAD
                    ClearFlag(42);
                    SetFlag(43);
                    break;
                case 0x82:                                             // GRAD
                    ClearFlag(43);
                    SetFlag(42);
                    break;
                case 0x83:                                             // ENTER^
                    for (i = 0; i < 21; i++) ram[i] = ram[i + 7];
                    ram[PENDING] = (byte)'D';
                    break;
                case 0x84:                                             // STOP
                    running = false;
                    ram[REG_E + 1] |= 0x0f;
                    ram[REG_E + 0] = 0xff;
                    Annunciators();
                    window.Display(Display());
                    break;
                case 0x85:                                             // RTN
                    addr = Rtn(addr);
                    break;
                case 0x86:                                             // BEEP
                    if (FlagSet(F_AUDIO)) sound.PlayBeep();
                    break;
                case 0x87:                                             // CLA
                    for (i = REG_M; i < REG_P + 3; i++)
                        ram[i] = 0;
                    break;
                case 0x88:                                             // ASHF
                    i = REG_P + 2;
                    while (i > REG_M && ram[i] == 0) i--;
                    for (j = 0; j < 6; j++)
                        if (i >= REG_M) ram[i--] = 0;
                    break;
                case 0x89:                                             // PSE
                    running = false;
                    ram[REG_E + 1] |= 0x0f;
                    ram[REG_E + 0] = 0xff;
                    break;
                case 0x8a:                                             // CLRG
                    Clrg();
                    break;
                case 0x8b:                                             // AOFF
                    ClearFlag(48);
                    Annunciators();
                    break;
                case 0x8c:                                             // AON
                    SetFlag(48);
                    ram[PENDING] = (byte)'N';
                    Annunciators();
                    break;
                case 0x8d:                                             // OFF
                    on = false;
                    break;
                case 0x8e:                                             // PROMPT
                    Aview();
                    running = false;
                    break;
                case 0x8f:                                             // ADV
                    if (FlagSet(55) && FlagSet(21))
                    {
                        printer.PrintBuffer();
                        printer.Print("");
                    }
                    break;

                case 0x90:                                             // RCL
                    b2 = ram[addr--];
                    a = Rcl(b2);
                    StoreNumber(a, R_X);
                    break;
                case 0x91:                                             // STO
                    a = RecallNumber(R_X);
                    b2 = ram[addr--];
                    Sto(a, b2);
                    break;
                case 0x92:                                             // ST+
                    ram[LIFT] = (byte)'D';
                    b2 = ram[addr--];
                    a = RecallNumber(R_X);
                    b = Rcl(b2);
                    a = Add(a, b);
                    Sto(a, b2);
                    break;
                case 0x93:                                             // ST-
                    ram[LIFT] = (byte)'D';
                    b2 = ram[addr--];
                    b = RecallNumber(R_X);
                    a = Rcl(b2);
                    a = Sub(a, b);
                    Sto(a, b2);
                    break;
                case 0x94:                                             // ST*
                    ram[LIFT] = (byte)'D';
                    b2 = ram[addr--];
                    a = RecallNumber(R_X);
                    b = Rcl(b2);
                    a = Mul(a, b);
                    Sto(a, b2);
                    break;
                case 0x95:                                             // ST/
                    ram[LIFT] = (byte)'D';
                    b2 = ram[addr--];
                    b = RecallNumber(R_X);
                    a = Rcl(b2);
                    a = Div(a, b);
                    Sto(a, b2);
                    break;
                case 0x96:                                             // ISG
                    i = Isg(ram[addr--]);
                    if (addr >= 0x0c0)
                    {
                        if (i != 0) addr = Skip(addr);
                    }
                    break;
                case 0x97:                                             // DSE
                    i = Dse(ram[addr--]);
                    if (addr >= 0x0c0)
                    {
                        if (i != 0) addr = Skip(addr);
                    }
                    break;
                case 0x98:                                             // VIEW
                    View(ram[addr--]);
                    break;
                case 0x99:                                             // EREG
                    EReg(ram[addr--]);
                    break;
                case 0x9a:                                             // ASTO
                    Asto(ram[addr--]);
                    break;
                case 0x9b:                                             // ARCL
                    Arcl(ram[addr--]);
                    break;
                case 0x9c:                                             // FIX
                    Fix(ram[addr--]);
                    break;
                case 0x9d:                                             // SCI
                    Sci(ram[addr--]);
                    break;
                case 0x9e:                                             // ENG
                    Eng(ram[addr--]);
                    break;
                case 0x9f:                                             // TONE
                    if (FlagSet(F_AUDIO)) Tone(ram[addr--]);
                    break;

                case 0xa0:                                             // XROM
                case 0xa1:
                case 0xa2:
                case 0xa3:
                case 0xa4:
                case 0xa5:
                case 0xa6:
                case 0xa7:
                    b2 = ram[addr--];
                    byt = ((cmd & 0x0f) << 2) | ((b2 & 0xc0) >> 6);
                    b2 &= 0x3f;
                    if (byt == 29) printer.Command(b2);
                    else if (byt == 28) tapeDrive.Command(b2, addr);
                    else if (byt == 30) addr = cardReader.Command(b2, addr);
                    else
                    {
                        Message("NONEXISTENT");
                        Error();
                    }
                    break;
                case 0xa8:                                             // SF
                    Sf(ram[addr--]);
                    break;
                case 0xa9:                                             // CF
                    Cf(ram[addr--]);
                    break;
                case 0xaa:                                             // FS?C
                    flag = FsQc(ram[addr--]);
                    if (errFlag == false)
                    {
                        if (addr < 0x0c0)
                        {
                            if (flag) Message("YES");
                            else Message("NO");
                        }
                        else
                        {
                            if (flag == false) addr = Skip(addr);
                        }
                    }
                    break;
                case 0xab:                                             // FC?C
                    flag = FsQc(ram[addr--]);
                    if (errFlag == false)
                    {
                        if (addr < 0x0c0)
                        {
                            if (flag) Message("NO");
                            else Message("YES");
                        }
                        else
                        {
                            if (flag != false) addr = Skip(addr);
                        }
                    }
                    break;
                case 0xac:                                             // FS?
                    flag = Fs(ram[addr--]);
                    if (errFlag == false)
                    {
                        if (addr < 0x0c0)
                        {
                            if (flag) Message("YES");
                            else Message("NO");
                        }
                        else
                        {
                            if (flag == false) addr = Skip(addr);
                        }
                    }
                    break;
                case 0xad:                                             // FC?
                    flag = Fs(ram[addr--]);
                    if (errFlag == false)
                    {
                        if (addr < 0x0c0)
                        {
                            if (flag) Message("NO");
                            else Message("YES");
                        }
                        else
                        {
                            if (flag != false) addr = Skip(addr);
                        }
                    }
                    break;
                case 0xae:                                             // GTO/XEQ IND
                    b2 = ram[addr];
                    addr = GtoXeqInd(addr + 1);
                    if (addr == 0)
                    {
                        running = false;
                        addr = oaddr;
                    }
                    if (addr != 0 && b2 >= 0x80)
                    {
                        Push((oaddr) - isize(oaddr - 1));
                    }
                    break;


                case 0xb1:                                             // GTO 00
                case 0xb2:                                             // GTO 01
                case 0xb3:                                             // GTO 02
                case 0xb4:                                             // GTO 03
                case 0xb5:                                             // GTO 04
                case 0xb6:                                             // GTO 05
                case 0xb7:                                             // GTO 06
                case 0xb8:                                             // GTO 07
                case 0xb9:                                             // GTO 08
                case 0xba:                                             // GTO 09
                case 0xbb:                                             // GTO 10
                case 0xbc:                                             // GTO 11
                case 0xbd:                                             // GTO 12
                case 0xbe:                                             // GTO 13
                case 0xbf:                                             // GTO 14
                    addr = Gto2(addr + 1);
                    break;

                case 0xc0:                                             // GLOBAL
                case 0xc1:
                case 0xc2:
                case 0xc3:
                case 0xc4:
                case 0xc5:
                case 0xc6:
                case 0xc7:
                case 0xc8:
                case 0xc9:
                case 0xca:
                case 0xcb:
                case 0xcc:
                case 0xcd:
                    addr--;
                    b2 = ram[addr];
                    if (b2 >= 0xf0)
                    {                                 // Label
                        addr -= (b2 & 0xf);
                        addr--;
                    }
                    else
                    {                                            // End
                        addr = Rtn(addr);
                        //           running = 0;
                        //           addr = oaddr;
                        if (addr != 0)
                        {
                            ram[REG_E + 1] |= 0x0f;
                            ram[REG_E + 0] = 0xff;
                        }
                        else
                        {
                            ram[REG_E + 1] &= 0xf0;
                            ram[REG_E + 0] = 0x00;
                        }
                    }
                    break;
                case 0xce:                                             // X<>
                    ram[LIFT] = (byte)'D';
                    b2 = ram[addr--];
                    a = RecallNumber(R_X);
                    b = Rcl(b2);
                    Sto(a, b2);
                    StoreNumber(b, R_X);
                    break;
                case 0xcf:                                             // LBL
                    if (running)
                    {
                        goose = goose.Substring(11) + goose.Substring(0, 11);
                        window.Display(goose);
                    }
                    addr--;
                    break;

                case 0xd0:                                             // GTO
                case 0xd1:                                             // GTO
                case 0xd2:                                             // GTO
                case 0xd3:                                             // GTO
                case 0xd4:                                             // GTO
                case 0xd5:                                             // GTO
                case 0xd6:                                             // GTO
                case 0xd7:                                             // GTO
                case 0xd8:                                             // GTO
                case 0xd9:                                             // GTO
                case 0xda:                                             // GTO
                case 0xdb:                                             // GTO
                case 0xdc:                                             // GTO
                case 0xdd:                                             // GTO
                case 0xde:                                             // GTO
                case 0xdf:                                             // GTO
                    addr = GtoXeq(addr + 1);
                    if (addr == 0)
                    {
                        running = false;
                        addr = oaddr;
                    }
                    break;

                case 0xe0:                                             // XEQ
                case 0xe1:                                             // XEQ
                case 0xe2:                                             // XEQ
                case 0xe3:                                             // XEQ
                case 0xe4:                                             // XEQ
                case 0xe5:                                             // XEQ
                case 0xe6:                                             // XEQ
                case 0xe7:                                             // XEQ
                case 0xe8:                                             // XEQ
                case 0xe9:                                             // XEQ
                case 0xea:                                             // XEQ
                case 0xeb:                                             // XEQ
                case 0xec:                                             // XEQ
                case 0xed:                                             // XEQ
                case 0xee:                                             // XEQ
                case 0xef:                                             // XEQ
                    addr = GtoXeq(addr + 1);
                    if (addr == 0)
                    {
                        running = false;
                        addr = oaddr;
                    }
                    else
                    {
                        if (running)
                        {
                            Push((oaddr - 1) - isize(oaddr - 1));
                        }
                        else
                        {
                            running = true;
                            for (i = 0; i < 7; i++) ram[REG_A + i] = 0;
                            for (i = 2; i < 7; i++) ram[REG_B + i] = 0;
                            window.RunTimerEnabled(true);
                            goose = "\x81           ";
                            window.Display(goose);
                        }
                    }
                    break;

                case 0xf0:                                             // TEXT 0
                    for (i = REG_M; i <= REG_P + 2; i++)
                        ram[i] = 0;
                    break;
                case 0xf1:                                             // TEXT 1
                case 0xf2:                                             // TEXT 2
                case 0xf3:                                             // TEXT 3
                case 0xf4:                                             // TEXT 4
                case 0xf5:                                             // TEXT 5
                case 0xf6:                                             // TEXT 6
                case 0xf7:                                             // TEXT 7
                case 0xf8:                                             // TEXT 8
                case 0xf9:                                             // TEXT 9
                case 0xfa:                                             // TEXT 10
                case 0xfb:                                             // TEXT 11
                case 0xfc:                                             // TEXT 12
                case 0xfd:                                             // TEXT 13
                case 0xfe:                                             // TEXT 14
                case 0xff:                                             // TEXT 15
                    addr = (ram[REG_B + 1] << 8) | ram[REG_B + 0];
                    addr = FromPtr(addr);
                    if (running)
                    {
                        addr--;
                        while (ram[addr] == 0x00) addr--;
                    }
                    cmd = ram[addr--];
                    cmd &= 0x0f;


                    if (ram[addr] != 0x7f)
                    {
                        for (i = REG_M; i <= REG_P + 2; i++)
                            ram[i] = 0;
                    }
                    for (i = 0; i < cmd; i++)
                    {
                        if (i > 0 || ram[addr] != 0x7f)
                        {
                            for (j = REG_P + 2; j > REG_M; j--)
                                ram[j] = ram[j - 1];
                            ram[REG_M] = ram[addr--];
                        }
                        else addr--;
                    }
                    addr = ToPtr(addr+1);
                    ram[REG_B + 1] = (byte)((addr >> 8) & 0xff);
                    ram[REG_B + 0] = (byte)(addr & 0xff);
                    addr = FromPtr(addr)-1;
                    break;

            }
            if (ram[PENDING] != 'N') ram[LIFT] = ram[PENDING];
            //            if (debug) ShowStatRegs(0);
            //            if (singleStep)
            //            {
            //                printf(":");
            //                fgets(buffer, 2, stdin);
            //            }
            errFlag = false;
            return addr;
        }

        int ExecNumber(int address)
        {
            int e;
            int ex;
            int i;
            int p;
            byte dp;
            char mode;
            Number n;
            dp = 0;
            p = 0;
            e = -1;
            ex = 0;
            n = new Number();
            n.sign = 0;
            n.esign = 0;
            mode = 'Z';
            for (i = 0; i < 10; i++) n.mantissa[i] = 0;
            n.exponent[0] = 0;
            n.exponent[1] = 0;
            while (ram[address] >= 0x10 && ram[address] <= 0x1c)
            {
                if (ram[address] >= 0x10 && ram[address] <= 0x19)
                {
                    if (mode == 'Z' && ram[address] > 0x10) mode = 'M';
                    if (mode == 'M')
                    {
                        if (p < 10) n.mantissa[p++] = (byte)(ram[address] - 0x10);
                        if (dp == 0) e++;
                    }
                    if (mode == 'E')
                    {
                        ex = (ex * 10) + (ram[address] - 0x10);
                    }
                    if (mode == 'Z' && ram[address] == 0x10 && dp != 0) e--;
                }
                if (ram[address] == 0x1a && mode != 'E')
                {
                    if (dp == 0) dp = 0xff;
                }
                if (ram[address] == 0x1b)
                {
                    if (mode != 'E')
                    {
                        mode = 'E';
                        ex = 0;
                        if (n.mantissa[0] == 0) n.mantissa[0] = 1;
                    }
                }
                if (ram[address] == 0x1c)
                {
                    if (mode == 'M') n.sign = (byte)((n.sign == 0) ? 9 : 0);
                    else n.esign = (byte)((n.esign == 0) ? 9 : 0);
                }
                address--;
            }
            if (n.esign != 0) e -= ex; else e += ex;
            if (e > 99 || e < -99)
            {
                Message("RANGE ERR");
                Error();
                return address;
            }
            n.esign = (byte)((e < 0) ? 9 : 0);
            if (e < 0) e = -e;
            n.exponent[0] = (byte)(e / 10);
            n.exponent[1] = (byte)(e % 10);
            StoreNumber(n, R_X);
            ram[PENDING] = (byte)'E';
            return address;
        }

        Number Fact(Number a)
        {
            int i;
            int n;
            double r;
            string tmp;
            if (a.esign != 0 || a.sign != 0)
            {
                Message("DATA ERROR");
                Error();
                return a;
            }
            if (a.exponent[0] != 0)
            {
                Message("DATA ERROR");
                Error();
                return a;
            }
            for (i = a.exponent[1] + 1; i < 10; i++)
                if (a.mantissa[i] != 0)
                {
                    Message("DATA ERROR");
                    Error();
                    return a;
                }
            n = a.mantissa[0];
            for (i = 1; i <= a.exponent[1]; i++)
            {
                n *= 10;
                n += a.mantissa[i];
            }
            if (n > 69)
            {
                Message("OUT OF RANGE");
                Error();
                return a;
            }
            r = 1;
            for (i = 1; i <= n; i++)
            {
                r *= i;
            }
            tmp = $"{r:e10}";
            a = AtoN(tmp);
            return a;
        }

        bool FcQc(byte n)
        {
            Number a;
            bool flag;
            if (n >= 0x80 && n <= 0xef)
            {
                a = Rcl(n & 0x7f);
                if (a.sign != 9 && a.sign != 0)
                {
                    Message("ALPHA DATA");
                    Error();
                    return false;
                }
                n = (byte)ToInteger(a);
            }
            else if (n >= 0xf0)
            {
                a = RecallNumber(n & 0x0f);
                n = (byte)ToInteger(a);
            }
            if (n >= 30)
            {
                Message("NONEXISTENT");
                Error();
                return false;
            }
            flag = FlagSet(n);
            ClearFlag(n);
            return flag;
        }

        public int FindEnd(int address)
        {
            while (ram[address] < 0xc0 || ram[address] >= 0xce || ram[address - 2] >= 0xf0)
                address -= isize(address);
            return address;
        }

        public int FindGlobal(string label)
        {
            int addr;
            string lbl1;
            string lbl2;
            int i;
            int p;
            bool flag;
            int dst;
            lbl1 = "";
            for (i = 0; i < label.Length; i++)
            {
                if (label[i] != '"')
                {
                    lbl1 += label.ElementAt(i).ToString();
                }
            }
            addr = ((ram[REG_C + 1] & 0x0f) << 8) | ram[REG_C + 0];
            addr = FromPtr(addr) + 2;
            flag = true;
            while (flag)
            {
                if (ram[addr] >= 0xc0 && ram[addr] <= 0xcd && ram[addr - 2] >= 0xf0)
                {
                    lbl2 = "";
                    for (i = 1; i < (ram[addr - 2] & 0xf); i++)
                        lbl2 += ((char)(ram[addr - 3 - i])).ToString();
                    if (lbl1.Equals(lbl2))
                    {
                        return addr + 1;
                    }
                }
                dst = ((ram[addr] & 0x0f) << 8) | ram[addr - 1];
                dst = ((dst & 0x1ff) * 7) + ((dst >> 9) & 0x7);
                if (dst == 0) flag = false;
                else addr += dst - 1;
            }
            Message("NONEXISTENT");
            Error();
            return 0;
        }

        int FindNextGlobal(int address)
        {
            while (ram[address] < 0xc0 || ram[address] >= 0xce)
                address -= isize(address);
            return address;
        }

        public int FindStart(int address)
        {
            int addr;
            int byt;
            int reg;
            bool flag;
            int l;
            while (ram[address] < 0xc0 || ram[address] >= 0xce)
                address -= isize(address);

            flag = true;
            while (flag)
            {
                l = ((ram[address] & 0x0f) << 8) | ram[address - 1];
                if (l == 0x00)
                {
                    address = (ram[REG_C + 2] << 4) | ((ram[REG_C + 1] & 0xf0) >> 4);
                    address *= 7;
                    address--;
                    flag = false;
                }
                else
                {
                    reg = l & 0x1ff;
                    byt = (l >> 9) & 0x7;
                    addr = reg * 7 + byt;
                    address += addr;
                    address--;
                    if (ram[address - 2] < 0xf0)
                    {
                        address -= 3;
                        flag = false;
                    }
                }
            }
            return address;
        }

        void Fix(byte n)
        {
            Number a;
            if (n >= 0x80 && n <= 0xef)
            {
                a = Rcl(n & 0x7f);
                if (a.sign != 9 && a.sign != 0)
                {
                    Message("ALPHA DATA");
                    Error();
                    return;
                }
                n = (byte)ToInteger(a);
                if (n >= 0x0a)
                {
                    Message("DATA ERRROR");
                    Error();
                    return;
                }
            }
            else if (n >= 0xf0)
            {
                a = RecallNumber(n & 0x0f);
                n = (byte)ToInteger(a);
                if (n >= 0x0a)
                {
                    Message("DATA ERRROR");
                    Error();
                    return;
                }
            }
            SetFlag(40);
            ClearFlag(41);
            if ((n & 8) != 0) SetFlag(36); else ClearFlag(36);
            if ((n & 4) != 0) SetFlag(37); else ClearFlag(37);
            if ((n & 2) != 0) SetFlag(38); else ClearFlag(38);
            if ((n & 1) != 0) SetFlag(39); else ClearFlag(39);
        }

        void FixLineNumber()
        {
            int addr;
            int ladr;
            int lineNumber = 0;
            addr = (ram[REG_B + 1] << 8) | ram[REG_B + 0];
            addr = FromPtr(addr);
            addr--;
            ladr = FindStart(addr);
            while (ladr > addr)
            {
                ladr -= isize(ladr);
                if (ram[ladr] != 0) lineNumber++;
            }
            if (lineNumber == 0) lineNumber = 1;
            ram[REG_E + 1] &= 0xf0;
            ram[REG_E + 1] |= (byte)(((lineNumber >> 8) & 0x0f));
            ram[REG_E + 0] = (byte)(lineNumber & 0xff);
        }

        public bool FlagSet(byte f)
        {
            int offset;
            offset = 6 - (f / 8);
            f = (byte)(f & 7);
            f = (byte)(0x80 >> f);
            if ((ram[REG_D + offset] & f) == 0) return false;
            return true;
        }

        public string Format(Number a)
        {
            int i;
            int d;
            int e;
            int mode;
            int decimals;
            string dest;
            int comma;

            dest = "";

            if (a.sign == 0x01)
            {
                if (a.mantissa[1] != 0x00 || a.mantissa[2] != 0x00)
                    dest += ((char)((a.mantissa[1] << 4) | (a.mantissa[2]))).ToString();
                if (a.mantissa[3] != 0x00 || a.mantissa[4] != 0x00)
                    dest += ((char)((a.mantissa[3] << 4) | (a.mantissa[4]))).ToString();
                if (a.mantissa[5] != 0x00 || a.mantissa[6] != 0x00)
                    dest += ((char)((a.mantissa[5] << 4) | (a.mantissa[6]))).ToString();
                if (a.mantissa[7] != 0x00 || a.mantissa[8] != 0x00)
                    dest += ((char)((a.mantissa[7] << 4) | (a.mantissa[8]))).ToString();
                if (a.mantissa[9] != 0x00 || a.esign != 0x00)
                    dest += ((char)((a.mantissa[9] << 4) | a.esign)).ToString();
                if (a.exponent[0] != 0x00 || a.exponent[1] != 0x00)
                    dest += ((char)((a.exponent[0] << 4) | (a.exponent[1]))).ToString();
                return dest;
            }

            if (a.sign != 0) dest += "-"; else dest += " ";
            decimals = 0;
            if (FlagSet(36)) decimals |= 8;
            if (FlagSet(37)) decimals |= 4;
            if (FlagSet(38)) decimals |= 2;
            if (FlagSet(39)) decimals |= 1;

            mode = 0;
            if ((FlagSet(40) == false) && (FlagSet(41) == false)) mode = 0;
            if ((FlagSet(40) == false) && (FlagSet(41) == true)) mode = 1;
            if ((FlagSet(40) == true) && (FlagSet(41) == false)) mode = 2;

            e = a.exponent[0] * 10 + a.exponent[1];

            if (mode == 2 && a.exponent[0] > 0 && decimals != 10 && a.esign == 0x00) mode = 0;

            if (mode == 2)
            {                                               /* FIX */
                if (a.esign != 0)
                {
                    dest += "0";
                    if (decimals != 0 || FlagSet(29))
                    {
                        if (FlagSet(28)) dest += '.'; else dest += ',';
                    }
                    d = 0;
                    for (i = 1; i < a.exponent[1]; i++)
                    {
                        if (d < decimals)
                        {
                            dest += '0';
                            d++;
                        }
                    }
                    for (i = 0; i < 10; i++)
                    {
                        if (d < decimals)
                        {
                            dest += (char)(a.mantissa[i] + '0');
                            d++;
                        }
                    }
                }
                else
                {
                    d = 0;
                    dest += (char)(a.mantissa[0] + '0');
                    d++;
                    comma = e - 3;
                    if (comma > 0) comma = comma % 3;
                    for (i = 0; i < a.exponent[1]; i++)
                    {
                        if (comma == 0 && FlagSet(29))
                        {
                            if (FlagSet(28)) dest += ',';
                            else dest += '.';
                            comma = 3;
                        }
                        if (comma > 0) comma--;
                        dest += (char)(a.mantissa[i + 1] + '0');
                        d++;
                    }
                    if (decimals != 0 || FlagSet(29))
                    {
                        if (FlagSet(28)) dest += '.'; else dest += ',';
                    }
                    for (i = 0; i < decimals; i++)
                    {
                        if (d < 10)
                        {
                            if (d + a.exponent[1] >= 10) dest += '0';
                            else dest += (char)(a.mantissa[d] + '0');
                            d++;
                        }
                    }
                }
            }


            if (mode == 0)
            {                                               /* SCI */
                dest += ((char)(a.mantissa[0] + '0')).ToString();
                if (decimals != 0)
                {
                    if (FlagSet(28)) dest += "."; else dest += ",";
                }
                for (i = 0; i < 7; i++)
                    if (i < decimals) dest += ((char)(a.mantissa[i + 1] + '0')).ToString();
                for (i = 0; i < 7 - decimals; i++)
                    dest += " ";
                if (a.esign != 0) dest += "-"; else dest += " ";
                dest += ((char)(a.exponent[0] + '0')).ToString();
                dest += ((char)(a.exponent[1] + '0')).ToString();
            }

            if (mode == 1)
            {                                               /* ENG */
                e = a.exponent[0] * 10 + a.exponent[1];
                dest += ((char)(a.mantissa[0] + '0')).ToString();
                d = 1;
                while ((e % 3) != 0)
                {
                    dest += ((char)(a.mantissa[d] + '0')).ToString();
                    if (a.esign != 0) e++; else e--;
                    d++;
                }
                if (decimals != 0)
                {
                    if (FlagSet(28)) dest += "."; else dest += ",";
                }
                for (i = 0; i < decimals; i++)
                {
                    if (d < 9)
                    {
                        dest += ((char)(a.mantissa[d] + '0')).ToString();
                        d++;
                    }
                }
                while (d < 9) { dest += " "; d++; }
                if (a.esign != 0) dest = "-"; else dest += " ";
                dest += ((char)((e / 10) + '0')).ToString();
                dest += ((char)((e % 10) + '0')).ToString();
            }

            return dest;
        }


        Number Fractional(Number a)
        {
            int i;
            int e;
            if (a.esign != 0) return a;
            e = (a.exponent[0] * 10) + a.exponent[1];
            while (e >= 0)
            {
                for (i = 0; i < 9; i++) a.mantissa[i] = a.mantissa[i + 1];
                e--;
            }
            while (a.mantissa[0] == 0 && IsZero(a) == false)
            {
                for (i = 0; i < 9; i++) a.mantissa[i] = a.mantissa[i + 1];
                e--;
            }
            if (IsZero(a)) return ZERO;
            a.esign = 9;
            e = -e;
            a.exponent[0] = (byte)(e / 10);
            a.exponent[1] = (byte)(e % 10);
            return a;
        }

        public int FromPtr(int ptr)
        {
            return ((ptr & 0xfff) * 7) + ((ptr >> 12) & 0xf);
        }

        bool Fs(byte n)
        {
            Number a;
            bool flag;
            if (n >= 0x80 && n <= 0xef)
            {
                a = Rcl(n & 0x7f);
                if (a.sign != 9 && a.sign != 0)
                {
                    Message("ALPHA DATA");
                    Error();
                    return false;
                }
                n = (byte)ToInteger(a);
            }
            else if (n >= 0xf0)
            {
                a = RecallNumber(n & 0x0f);
                n = (byte)ToInteger(a);
            }
            if (n >= 56)
            {
                Message("NONEXISTENT");
                Error();
                return false;
            }
            flag = FlagSet(n);
            return flag;
        }

        bool FsQc(byte n)
        {
            Number a;
            bool flag;
            if (n >= 0x80 && n <= 0xef)
            {
                a = Rcl(n & 0x7f);
                if (a.sign != 9 && a.sign != 0)
                {
                    Message("ALPHA DATA");
                    Error();
                    return false;
                }
                n = (byte)ToInteger(a);
            }
            else if (n >= 0xf0)
            {
                a = RecallNumber(n & 0x0f);
                n = (byte)ToInteger(a);
            }
            if (n >= 30)
            {
                Message("NONEXISTENT");
                Error();
                return false;
            }
            flag = FlagSet(n);
            ClearFlag(n);
            return flag;
        }

        public string GetAlpha()
        {
            int m;
            int p;
            string buffer;
            m = REG_P + 2;
            while (m >= REG_M && ram[m] == 0x00) m--;
            p = 0;
            buffer = "";
            while (m >= REG_M)
            {
                if (ram[m] != 0x00) buffer += ((char)ram[m]).ToString();
                m--;
            }
            return buffer;
        }

        public void GetBE(Number x, ref int b, ref int e)
        {
            int i;
            int p;
            int n;
            b = 0;
            e = 0;
            p = 0;
            if (x.esign == 0)
            {
                b = x.mantissa[p++];
                n = x.exponent[0] * 10 + x.exponent[1];
                while (n > 0)
                {
                    b *= 10;
                    if (p < 10) b += x.mantissa[p++];
                    n--;
                }
            }
            else
            {
                n = x.exponent[0] * 10 + x.exponent[1];
                while (n > 0)
                {
                    n--;
                    p--;
                }
                p++;
            }
            for (i = 0; i < 3; i++)
            {
                e *= 10;
                if (p >= 0 && p < 10) e += x.mantissa[p];
                p++;
            }
        }

        HMS GetHms(Number a)
        {
            int e;
            int p;
            HMS ret = new HMS();
            e = a.exponent[0] * 10 + a.exponent[1];
            if (a.esign != 0) e = -e;
            p = 0;
            ret.hours = 0;
            ret.minutes = 0;
            ret.seconds = 0;
            ret.fractional = 0;
            while (e >= 0)
            {
                if (p < 10) ret.hours = (ret.hours * 10) + a.mantissa[p++];
                else ret.hours *= 10;
                e--;
            }
            e++;
            while (e < 0) { p++; e++; }
            if (p < 10) ret.minutes = a.mantissa[p++] * 10;
            if (p < 10) ret.minutes += a.mantissa[p++];
            if (p < 10) ret.seconds = a.mantissa[p++] * 10;
            if (p < 10) ret.seconds += a.mantissa[p++];
            if (p < 10) ret.fractional = a.mantissa[p++] * 10;
            if (p < 10) ret.fractional += a.mantissa[p++];
            if (a.sign != 0)
            {
                ret.hours = -ret.hours;
                ret.minutes = -ret.minutes;
                ret.seconds = -ret.seconds;
                ret.fractional = -ret.fractional;
            }
            return ret;
        }

        public bool GetKaFlag(byte flag)
        {
            int i;
            int f;
            int b;
            int o;
            i = 0;
            while (keys[i].keycode != 0xff && keys[i].keycode != flag && keys[i].skeycode != flag) i++;
            if (keys[i].keycode == 0xff) return false;
            f = keys[i].bit - 1;
            b = 6 - (f >> 3);
            o = 7 - (f & 0x07);
            i = flag & 0x0f;
            if (i >= 1 && i <= 8)
            {                 /* unshifted */
                if ((ram[REG_R + b] & (1 << o)) != 0) return true;
            }
            else
            {                                  /* shifted */
                if ((ram[REG_E + b] & (1 << o)) != 0) return true;
            }
            return false;
        }

        int GtoAlpha(int address)
        {
            int i;
            int n;
            int addr;
            string token;

            ram[REG_E + 1] |= 0x0f;
            ram[REG_E + 0] = 0xff;
            token = "";
            for (i = 0; i < (ram[address] & 0xf); i++)
                token += ((char)ram[address - 1 - i]).ToString();
            n = FindGlobal(token);
            if (n != 0)
            {
                addr = n;
                n = ToPtr(n);
                ram[REG_B + 1] = (byte)((n >> 8) & 0xff);
                ram[REG_B + 0] = (byte)(n & 0xff);
                return addr;
            }
            Message("NONEXISTENT");
            Error();
            return 0;
        }

        public void GotoLine(int line)
        {
            int addr;
            int adr;
            int l;
            addr = (ram[REG_B + 1] << 8) | ram[REG_B + 0];
            adr = FromPtr(addr);
            adr--;

            adr = FindStart(adr);
            adr++;

            if (line > 1)
            {
                l = line;
                line = 1;
                while (l > 1)
                {
                    while (ram[adr - 1] == 0) adr--;
                    if (ram[adr - 1] >= 0xc0 && ram[adr - 1] < 0xce && ram[adr - 3] < 0xf0) l = 0;
                    else
                    {
                        adr -= isize(adr - 1);
                        line++;
                        l--;
                    }
                }
            }
            addr = ToPtr(adr);
            ram[REG_B + 1] = (byte)((addr >> 8) & 0xff);
            ram[REG_B + 0] = (byte)(addr & 0xff);
            ram[REG_E + 0] = (byte)(line & 0xff);
            ram[REG_E + 1] &= 0xf0;
            ram[REG_E + 1] |= (byte)(((line >> 8) & 0x0f));
        }

        int Gto2(int address)
        {
            int addr;
            int last;
            int jump;
            int ret;
            int ofs;
            int dif;
            byte lbl;
            bool flag;
            lbl = (byte)(ram[address] & 0x0f);
            address--;
            addr = (ram[REG_B + 1] << 8) | ram[REG_B + 0];
            addr = FromPtr(addr) - 1;
            if (address >= 0x0c0 && ram[addr - 1] != 0)
            {
                addr--;
                jump = (ram[addr] & 0x0f) * 7;
                jump += ((ram[addr] >> 4) & 0x7);
                if (ram[addr] < 0x80) jump = -jump;
                addr += jump;
                ret = addr - 1;
                addr = ToPtr(addr);
                ram[REG_B + 1] = (byte)((addr >> 8) & 0xff);
                ram[REG_B + 0] = (byte)(addr & 0xff);
                ram[REG_E + 0] = 0xff;
                ram[REG_E + 1] |= 0x0f;
                return ret;
            }
            last = addr;
            flag = true;
            while (flag)
            {
                if (ram[addr] == lbl) flag = false;
                else if (ram[addr] >= 0xc0 && ram[addr] <= 0xcd &&
                         ram[addr - 2] < 0xf0)
                {
                    addr = FindStart(addr) + 1;
                }
                else
                {
                    addr -= isize(addr);
                }
                if (flag != false && addr == last)
                {
                    Message("NONEXISTENT");
                    Error();
                    return 0;
                }
            }
            addr++;
            if (address >= 0x0c0)
            {
            }
            dif = (last - 1) - addr;
            ofs = 0;
            if (dif < 0)
            {
                dif = -dif;
                ofs = 0x80;
            }
            if (dif > 112)
            {
                dif = 0;
                ofs = 0;
            }
            ofs |= (dif / 7);
            ofs |= ((dif % 7) << 4);
            if (address >= 0x0c0)
                ram[last - 1] = (byte)ofs;
            ret = addr - 1;
            addr = ToPtr(addr);
            ram[REG_B + 1] = (byte)(addr >> 8);
            ram[REG_B + 0] = (byte)(addr & 0xff);
            ram[REG_E + 0] = 0xff;
            ram[REG_E + 1] |= 0x0f;
            return ret;
        }

        public void GtoEnd()
        {
            int n;
            int end;
            int adr;
            int ptr;
            int r00;
            r00 = (ram[REG_C + 2] << 4) | ((ram[REG_C + 1] >> 4) & 0x0f);
            r00 *= 7;
            end = ((ram[REG_C + 1] & 0x0f) << 8) | (ram[REG_C + 0] & 0xff);
            end *= 7;
            adr = end + 3;
            while (ram[adr] == 0x00 && adr < r00) adr++;
            if (adr < r00)
            {
                if (ram[adr + 2] < 0xc0 || ram[adr + 2] > 0xcd || ram[adr] >= 0xf0)
                {
                    ptr = ((ram[end + 2] & 0x0f) << 8) | ram[end + 1];
                    ram[end - 7] = 0x2d;
                    ram[end - 6] = 0x01;
                    ram[end - 5] = 0xc2;
                    ram[end + 2] = (byte)(0xc0 | ((ptr & 0xf00) >> 8));
                    ram[end + 1] = (byte)(ptr & 0xff);
                    ram[end] = 0x0d;
                    end -= 7;
                    end /= 7;
                    ram[REG_C + 1] &= 0xf0;
                    ram[REG_C + 1] |= (byte)(((end >> 8) & 0x0f));
                    ram[REG_C + 0] = (byte)(end & 0xff);
                }
            }
            n = ((ram[REG_C + 1] & 0x0f) << 8) | ram[REG_C + 0];
            ram[REG_B + 0] = (byte)(n & 0xff);
            ram[REG_B + 1] = (byte)(0x30 | ((n >> 8) & 0x0f));
            ram[REG_E + 0] = 0x00;
            ram[REG_E + 1] &= 0xf0;
        }

        int GtoXeq(int address)
        {
            int addr;
            int last;
            int jump;
            int ret;
            int ofs;
            int dif;
            byte lbl;
            bool flag;
            if (address >= 0x0c0)
                lbl = (byte)(ram[address - 2] & 0x7f);
            else
                lbl = ram[address - 1];
            //               lbl = (byte)(((ram[REG_E + 2] & 0x0f) << 4) | ((ram[REG_E + 1] >> 4) & 0x0f));
            address--;
            addr = (ram[REG_B + 1] << 8) | ram[REG_B + 0];
            addr = FromPtr(addr) - 1;
            if (address >= 0x0c0 && ram[addr - 1] != 0)
            {
                jump = ((ram[addr] & 0x0f) << 8) | ram[addr - 1];
                jump = ((jump & 0x1ff) * 7) + ((jump >> 9) & 0x07);
                if (ram[addr - 2] < 0x80) jump = -jump;
                addr += jump;
                ret = addr - 1;
                addr = ToPtr(addr);
                ram[REG_B + 1] = (byte)((addr >> 8) & 0xff);
                ram[REG_B + 0] = (byte)(addr & 0xff);
                ram[REG_E + 0] = 0xff;
                ram[REG_E + 1] |= 0x0f;
                return ret;
            }
            last = addr;
            flag = true;
            while (flag)
            {
                if (lbl < 15 && ((ram[addr] & 0x7f) == lbl + 1)) flag = false;
                else if (lbl > 14 && (ram[addr] == 0xcf) && ((ram[addr - 1] & 0x7f) == lbl)) flag = false;
                else if (ram[addr] >= 0xc0 && ram[addr] <= 0xcd &&
                         ram[addr - 2] < 0xf0)
                {
                    addr = FindStart(addr) + 1;
                }
                else
                {
                    addr -= isize(addr);
                }
                if (flag != false && addr == last)
                {
                    Message("NONEXISTENT");
                    Error();
                    return 0;
                }
            }
            addr++;
            if (address >= 0x0c0)
            {
                dif = last - addr;
                ofs = 0;
                if (dif < 0)
                {
                    dif = -dif;
                    ram[last - 2] |= 0x80;
                }
                if (dif > 511)
                {
                    dif = 0;
                    ofs = 0;
                }
                if (dif != 0)
                {
                    ofs |= (dif / 7);
                    ofs |= ((dif % 7) << 9);
                    ram[last] &= 0xf0;
                    ram[last] |= (byte)((ofs >> 8) & 0x0f);
                    ram[last - 1] = (byte)(ofs & 0xff);
                }
            }
            if (lbl < 15) ret = addr - 2;
            else ret = addr - 1;
            addr = ToPtr(addr);
            ram[REG_B + 1] = (byte)(addr >> 8);
            ram[REG_B + 0] = (byte)(addr & 0xff);
            ram[REG_E + 0] = 0xff;
            ram[REG_E + 1] |= 0x0f;
            return ret;
        }

        int GtoXeqInd(int address)
        {
            int addr;
            int last;
            int ret;
            byte lbl;
            bool flag;
            Number n;
            lbl = (byte)(ram[address - 1] & 0x7f);
            n = Rcl(lbl);
            lbl = (byte)ToInteger(n);
            address--;
            addr = (ram[REG_B + 1] << 8) | ram[REG_B + 0];
            addr = FromPtr(addr) - 1;
            last = addr;
            flag = true;
            while (flag)
            {
                if (lbl < 15 && ((ram[addr - 1] & 0x7f) == lbl + 1)) flag = false;
                else if (lbl > 14 && (ram[addr] == 0xcf) && ((ram[addr - 1] & 0x7f) == lbl)) flag = false;
                else if (ram[addr] >= 0xc0 && ram[addr] <= 0xcd &&
                         ram[addr - 2] < 0xf0)
                {
                    addr = FindStart(addr) + 1;
                }
                else
                {
                    addr -= isize(addr);
                }
                if (flag != false && addr == last)
                {
                    Message("NONEXISTENT");
                    Error();
                    return 0;
                }
            }
            addr++;
            if (lbl < 15) ret = addr - 2;
            else ret = addr - 1;
            addr = ToPtr(addr);
            ram[REG_B + 1] = (byte)(addr >> 8);
            ram[REG_B + 0] = (byte)(addr & 0xff);
            ram[REG_E + 0] = 0xff;
            ram[REG_E + 1] |= 0x0f;
            return ret;
        }

        Number Hms(Number a)
        {
            Number b;
            string tmp;
            int m;
            int s;
            int h;
            int f;
            double y;
            string n;
            n = NtoA(a);
            y = Convert.ToDouble(n);
            h = (int)y;
            y -= h;
            y *= 3600;
            m = (int)(y / 60.0);
            y -= m * 60;
            s = (int)y;
            y -= (int)y;
            y *= 100;
            f = (int)y;
            tmp = $"{h:d2}.{m:d2}{s:d2}{f:d}";
            b = AtoN(tmp);
            return b;
        }

        Number HmsMinus(Number a, Number b)
        {
            a.sign = (byte)((a.sign != 0) ? 0 : 9);
            return HmsPlus(a, b);
        }

        Number HmsPlus(Number a, Number b)
        {
            string buffer;
            HMS ha;
            HMS hb;
            ha = GetHms(a);
            hb = GetHms(b);
            ha.fractional += hb.fractional;
            if (ha.fractional >= 100) { ha.fractional -= 100; ha.seconds++; }
            if (ha.fractional < 0) { ha.fractional += 100; ha.seconds--; }
            ha.seconds += hb.seconds;
            if (ha.seconds >= 60) { ha.seconds -= 60; ha.minutes++; }
            if (ha.seconds < 0) { ha.seconds += 60; ha.minutes--; }
            ha.minutes += hb.minutes;
            if (ha.minutes >= 60) { ha.minutes -= 60; ha.hours++; }
            if (ha.minutes < 0) { ha.minutes += 60; ha.hours--; }
            ha.hours += hb.hours;
            buffer = $"{ha.hours:d}.{ha.minutes:d2}{ha.seconds:d2}{ha.fractional:d2}";
            a = AtoN(buffer);
            return a;
        }

        Number Hr(Number a)
        {
            HMS h;
            string tmp;
            double d;
            h = GetHms(a);
            d = h.minutes * 60 + h.seconds + ((double)h.fractional / 100.0);
            d /= 3600.0;
            d += h.hours;
            tmp = $"{d:e12}";
            a = AtoN(tmp);
            return a;
        }

        Number Integer(Number a)
        {
            int i;
            int e;
            if (a.esign != 0) return ZERO;
            e = (a.exponent[0] * 10) + a.exponent[1];
            if (e < 10)
            {
                for (i = 1 + e; i < 10; i++) a.mantissa[i] = 0;
            }
            return a;
        }

        int Isg(byte post)
        {
            int i;
            int p;
            int e;
            int count;
            int final;
            int inc;
            Number x;
            Number y;
            string tmp;
            x = Rcl(post);
            e = x.exponent[0] * 10 + x.exponent[1];
            if (x.esign == 9) e = -e;
            count = 0;
            final = 0;
            inc = 0;
            p = 0;
            while (e >= 0)
            {
                count *= 10;
                if (p < 10) count += x.mantissa[p++];
                e--;
            }
            for (i = 0; i < 3; i++)
            {
                if (e >= -1)
                {
                    final = (final * 10);
                    if (p < 10) final += x.mantissa[p++];
                }
                e++;
            }
            for (i = 0; i < 2; i++)
            {
                inc = (inc * 10);
                if (p < 10) inc += x.mantissa[p++];
            }
            if (inc == 0) inc = 1;
            if (x.sign == 9) count = -count;
            count += inc;
            tmp = $"{count:d}.{final:d3}{inc:d2}";
            y = AtoN(tmp);
            if (count < 0) y.sign = 9;
            Sto(y, post);
            if (count > final) return -1;
            return 0;
        }

        bool IsInteger(Number a)
        {
            int e;
            e = (a.exponent[0] * 10) + a.exponent[1];
            if (a.esign != 0) return false;
            e++;
            while (e < 10)
            {
                if (a.mantissa[e] != 0) return false;
                e++;
            }
            return true;
        }

        public int isize(int address)
        {
            int size;
            byte cmd;
            byte b;
            cmd = ram[address];
            if (cmd >= 0x10 && cmd <= 0x1c)
            {
                size = 1;
                address--;
                while (ram[address] >= 0x10 && ram[address] <= 0x1c)
                {
                    address--;
                    size++;
                }
                return size;
            }
            if (cmd == 0x1d || cmd == 0x1e)
            {
                b = ram[address - 1];
                return 2 + (b & 0xf);
            }
            if (cmd < 0x90) return 1;
            if (cmd >= 0x90 && cmd <= 0x9f) return 2;
            if (cmd >= 0xa0 && cmd <= 0xa7) return 2;
            if (cmd >= 0xa8 && cmd <= 0xae) return 2;
            if (cmd >= 0xb0 && cmd <= 0xbf) return 2;
            if (cmd >= 0xc0 && cmd <= 0xcd)
            {
                b = ram[address - 2];
                if (b < 0xf0) return 3;
                return 3 + (b & 0xf);
            }
            if (cmd >= 0xce && cmd <= 0xcf) return 2;
            if (cmd >= 0xd0 && cmd <= 0xdf) return 3;
            if (cmd >= 0xe0 && cmd <= 0xef) return 3;
            if (cmd >= 0xf0 && cmd <= 0xff) return 1 + (cmd & 0xf);
            return 1;
        }

        bool IsZero(Number a)
        {
            int i;
            for (i = 0; i < 10; i++)
                if (a.mantissa[i] != 0) return false;
            return true;
        }

        void Link(int address)
        {
            int adr;
            int reg;
            int byt;
            int next;
            int diff;
            int lnext;
            int prev;
            next = FindNextGlobal(address - 1);
            lnext = ((ram[next] & 0x0f) << 8) | ram[next - 1];
            prev = 0;
            if (lnext != 0)
            {
                prev = ((lnext & 0x1ff) * 7) + ((lnext >> 9) & 0x7) + next + byteCount - 1;
            }
            diff = 1 + address - next;
            reg = diff / 7;
            byt = diff % 7;
            adr = (byt << 9) | reg;
            ram[next] &= 0xf0;
            ram[next] |= (byte)((adr >> 8) & 0x0f);
            ram[next - 1] = (byte)(adr & 0xff);
            if (lnext != 0)
            {
                diff = 1 + prev - address;
                reg = diff / 7;
                byt = diff % 7;
                adr = (byt << 9) | reg;
                ram[address] &= 0xf0;
                ram[address] |= (byte)((adr >> 8) & 0x0f);
                ram[address - 1] = (byte)(adr & 0xff);
            }
            byteCount = 0;
        }

        Number Ln(Number a)
        {
            int i;
            bool flag;
            DNumber one;
            DNumber two;
            DNumber n;
            DNumber x;
            DNumber sum;
            DNumber num;
            DNumber term;
            DNumber b;
            DNumber last;
            if (IsZero(a))
            {
                Message("DATA ERROR");
                Error();
                return a;
            }
            if (a.sign != 0)
            {
                Message("DATA ERROR");
                Error();
                return a;
            }
            x = NumberToDNumber(a);
            one = D_ONE;
            two = D_TWO;
            sum = D_Div(D_Sub(x, one), D_Add(x, one));
            n = D_Mul(sum, sum);
            b = sum;
            num = one;
            flag = true;
            while (flag)
            {
                last = sum;
                b = D_Mul(b, n);
                num = D_Add(num, two);
                term = D_Div(one, num);
                term = D_Mul(term, b);
                sum = D_Add(sum, term);
                flag = false;
                for (i = 0; i < 15; i++)
                {
                    if (last.mantissa[i] != sum.mantissa[i]) flag = true;
                }
            }
            sum = D_Add(sum, sum);
            return DNumberToNumber(sum);
        }

        Number Ln1PlusX(Number a)
        {
            int i;
            bool flag;
            DNumber one;
            DNumber x;
            DNumber sum;
            DNumber num;
            DNumber den;
            DNumber term;
            DNumber last;
            bool addsub;
            x = NumberToDNumber(a);
            one = new DNumber();
            one.sign = 0;
            one.esign = 0;
            one.mantissa[0] = 1;
            for (i = 1; i < 20; i++) one.mantissa[i] = 0;
            for (i = 0; i < 3; i++) one.exponent[i] = 0;
            sum = x;
            num = x;
            den = one;
            addsub = true;
            flag = true;
            while (flag)
            {
                last = sum;
                num = D_Mul(num, x);
                den = D_Add(den, one);
                term = D_Div(num, den);
                if (addsub)
                {
                    sum = D_Sub(sum, term);
                    addsub = false;
                }
                else
                {
                    sum = D_Add(sum, term);
                    addsub = true;
                }
                flag = false;
                for (i = 0; i < 15; i++)
                    if (last.mantissa[i] != sum.mantissa[i]) flag = true;
            }
            return DNumberToNumber(sum);
        }

        void Mean()
        {
            int b;
            int ofs;
            Number x;
            Number y;
            b = ram[REG_C + 2] << 4;
            b |= ((ram[REG_C + 1] >> 4) & 0xf);
            ofs = ram[REG_C + 6] << 4;
            ofs |= ((ram[REG_C + 5] >> 4) & 0xf);
            b += ofs;
            x = RecallNumber(R_X);
            StoreNumber(x, R_L);
            y = RecallNumber(b + 0);
            x = RecallNumber(b + 5);
            if (IsZero(x))
            {
                Message("DATA ERROR");
                Error();
                return;
            }
            x = Div(y, x);
            StoreNumber(x, R_X);
            y = RecallNumber(b + 2);
            x = RecallNumber(b + 5);
            x = Div(y, x);
            StoreNumber(x, R_Y);
        }

        public void Message(string msg)
        {
            display = msg;
            SetFlag(50);
        }

        Number Mod(Number x, Number y)
        {
            Number a;
            a = Div(x, y);
            a = Integer(a);
            a = Mul(y, a);
            a = Sub(x, a);
            a.sign = (byte)((x.sign != y.sign) ? 9 : 0);
            return a;
        }

        Number Mul(Number a, Number b)
        {
            DNumber da;
            DNumber db;
            DNumber dc;
            da = NumberToDNumber(a);
            db = NumberToDNumber(b);
            dc = D_Mul(da, db);
            return DNumberToNumber(dc);
        }

        Number Normalize(Number n)
        {
            int i;
            int e;
            if (IsZero(n)) return n;
            e = (n.exponent[0] * 10) + n.exponent[1];
            if (n.esign != 0) e = -e;
            while (n.mantissa[0] == 0)
            {
                for (i = 0; i < 9; i++) n.mantissa[i] = n.mantissa[i + 1];
                n.mantissa[9] = 0;
                e--;
            }
            if (e < 0)
            {
                n.esign = 9;
                e = -e;
            }
            else n.esign = 0;
            n.exponent[0] = (byte)(e / 10);
            n.exponent[1] = (byte)(e % 10);
            return n;
        }

        string NtoA(Number a)
        {
            int p;
            int i;
            string buffer;
            buffer = "";
            p = 0;
            if (a.sign != 0) buffer += "-";
            buffer += ((char)(a.mantissa[0] + '0')).ToString();
            buffer += ".";
            for (i = 1; i < 10; i++)
            {
                buffer += ((char)(a.mantissa[i] + '0')).ToString();
            }
            buffer += "E";
            buffer += (a.esign != 0) ? "-" : "+";
            buffer += ((char)(a.exponent[0] + '0')).ToString();
            buffer += ((char)(a.exponent[1] + '0')).ToString();
            return buffer;
        }

        DNumber NumberToDNumber(Number a)
        {
            int i;
            DNumber r;
            r = new DNumber();
            r.sign = a.sign;
            r.esign = a.esign;
            for (i = 0; i < 10; i++) r.mantissa[i] = a.mantissa[i];
            for (i = 10; i < 20; i++) r.mantissa[i] = 0;
            r.exponent[0] = 0;
            r.exponent[1] = a.exponent[0];
            r.exponent[2] = a.exponent[1];
            return r;
        }

        Number Oct(Number a)
        {
            int i;
            int n;
            string tmp = "";
            if (a.esign != 0)
            {
                Message("DATA ERROR");
                Error();
                return a;
            }
            if (a.exponent[0] != 0)
            {
                Message("DATA ERROR");
                Error();
                return a;
            }
            for (i = a.exponent[1] + 1; i < 10; i++)
                if (a.mantissa[i] != 0)
                {
                    Message("DATA ERROR");
                    Error();
                    return a;
                }
            n = a.mantissa[0];
            for (i = 1; i <= a.exponent[1]; i++)
            {
                n *= 10;
                n += a.mantissa[i];
            }
            if (n > 1073741823)
            {
                Message("DATA ERROR");
                Error();
                return a;
            }
            i = 9;
            while (i >= 0)
            {
                tmp = ((char)((n & 0x7) + '0')).ToString() + tmp;
                n >>= 3;
                i--;
            }
            a = AtoN(tmp);
            return a;
        }

        public void Pack()
        {
            int i;
            int end;
            int cur;
            int addr;
            int next;
            int r00;
            int total;
            int count;
            bool pack;
            int start;
            int bytes;
            int prg;
            window.Display("PACKING");
            prg = FromPtr((ram[REG_B + 1] << 8) | ram[REG_B + 0]);
            end = (((ram[REG_C + 1] & 0x0f) << 8) | ram[REG_C + 0]) * 7 + 2;
            r00 = ((ram[REG_C + 2] << 4) | ((ram[REG_C + 1] >> 4) & 0x0f));
            r00 = r00 * 7;
            cur = end;
            pack = false;
            while (cur < r00)
            {
                next = ((ram[cur] << 8) | ram[cur - 1]) & 0xfff;
                if (next == 0x000)
                {
                    next = r00;
                    pack = true;
                }
                else
                    next = cur + (((next & 0x1ff) * 7) + ((next >> 9) & 0x7)) - 1;
                if (ram[cur - 2] < 0xf0)
                {
                    if ((ram[cur - 2] & 0x0f) == 0x0d) pack = true; else pack = false;
                    if ((ram[cur - 2] & 0xf0) == 0x20) pack = true;
                    if (pack)
                    {
                        ram[cur - 2] &= 0xf0;
                        ram[cur - 2] |= 0x09;
                    }
                }
                if (pack)
                {
                    addr = next;
                    total = 0;
                    count = 0;
                    while (addr > cur)
                    {
                        addr -= isize(addr);
                        start = addr;
                        if (ram[addr] >= 0xb0 && ram[addr] <= 0xbf)
                        {
                            ram[addr - 1] = 0;
                        }
                        if (ram[addr] >= 0xd0 && ram[addr] <= 0xef)
                        {
                            ram[addr] &= 0xf0;
                            ram[addr - 1] = 0x00;
                            ram[addr - 2] &= 0x7f;
                        }
                        while (ram[addr] == 0)
                        {
                            addr--;
                            count++;
                        }
                        if (count > 0)
                        {
                            total += count;
                            bytes = (addr - end) + 3;
                            for (i = 0; i < bytes; i++)
                                ram[start - i] = ram[addr--];
                            if (start >= prg) prg += count;
                            addr = start;
                            end += count;
                            cur += count;
                            for (i = 0; i < count + 3; i++) ram[end - 3 - i] = 0;
                        }
                        count = 0;
                    }
                    if (total > 0)
                    {
                        i = ((ram[cur] & 0x0f) << 8) | ram[cur - 1];
                        i = ((i & 0x1ff) * 7) + ((i >> 9) & 0x07);
                        if (i != 0) i -= total;
                        i = ((i / 7) & 0x1ff) | ((i % 7) << 9);
                        ram[cur] = (byte)(((i >> 8) & 0xff) | 0xc0);
                        ram[cur - 1] = (byte)(i & 0xff);
                    }
                }
                cur = next;
                if (cur == r00 - 1) cur = r00;
            }
            count = 0;
            while ((end % 7) != 2)
            {
                count++;
                ram[end - 3] = ram[end - 2];
                ram[end - 2] = ram[end - 1];
                ram[end - 1] = ram[end - 0];
                ram[end - 0] = 0;
                end--;
            }
            i = ((ram[end] & 0x0f) << 8) | ram[end - 1];
            i = ((i & 0x1ff) * 7) + ((i >> 9) & 0x07);
            if (i != 0) i += count;
            i = ((i / 7) & 0x1ff) | ((i % 7) << 9);
            ram[end] = (byte)(((i >> 8) & 0xff) | 0xc0);
            ram[end - 1] = (byte)(i & 0xff);
            end /= 7;
            ram[REG_C + 1] &= 0xf0;
            ram[REG_C + 1] |= (byte)(((end >> 8) & 0x0f));
            ram[REG_C + 0] = (byte)(end & 0xff);
            prg = ToPtr(prg);
            ram[REG_B + 1] = (byte)((prg >> 8) & 0xff);
            ram[REG_B + 0] = (byte)((prg & 0xff));
            ReLink();
        }

        /*
        string Postfix(byte i, int adr)
        {
            byte b;
            byte c;
            string buffer = $"{reverse[i].name} ";
            if ((reverse[i].size & 0xf0) == 0x40) return buffer;
            adr--;
            b = ram[adr];
            if (b >= 0x80) buffer += "IND ";
            b &= 0x7f;
            if ((reverse[i].size & 0xf0) == 0x90)
            {
                if (b < 100)
                {
                    buffer += $"{b:d2}";
                }
                if (b >= 102 && b <= 111)
                {
                    c = (byte)(b - 102 + 'A');
                    buffer += ((char)c).ToString();
                }
                if (b >= 123 && b <= 127)
                {
                    c = (byte)(b - 123 + 'a');
                    buffer += ((char)c).ToString();
                }
            }
            else if (b == 112) buffer += "T";
            else if (b == 113) buffer += "Z";
            else if (b == 114) buffer += "Y";
            else if (b == 115) buffer += "X";
            else if (b == 116) buffer += "L";
            else if (b == 117) buffer += "M";
            else if (b == 118) buffer += "N";
            else if (b == 119) buffer += "O";
            else if (b == 120) buffer += "P";
            else if (b == 121) buffer += "Q";
            else if (b == 122) buffer += ((char)0x7f).ToString();
            else if (b == 123) buffer += "a";
            else if (b == 124) buffer += "b";
            else if (b == 125) buffer += "c";
            else if (b == 126) buffer += "d";
            else if (b == 127) buffer += "e";
            else
            {
                if ((reverse[i].size & 0xf0) == 0x80)
                {
                    if (b < 65)
                    {
                        buffer += $"{b % 10:d}";
                    }
                    else
                    {
                        buffer += $"{(char)b}";
                    }
                }
                else
                {
                    buffer += $"{b:d2}";
                }
            }
            return buffer;
        }
        */

        public int ProgramBack()
        {
            int i;
            int addr;
            int adr;
            int byt;
            int reg;
            int pqr;
            addr = (ram[REG_B + 1] << 8) | ram[REG_B + 0];
            reg = (addr & 0xfff);
            byt = (addr >> 12) & 0xf;
            adr = (reg * 7) + byt;
            adr--;
            while (ram[adr] == 0) adr--;
            while (ram[adr] >= 0x10 && ram[adr] <= 0x1c) adr--;
            adr++;
            ram[adr] = 0x00;
            adr+=2;
            reg = adr / 7;
            byt = adr % 7;
            addr = (byt << 12) | reg;
            ram[REG_B + 1] = (byte)((addr >> 8) & 0xff);
            ram[REG_B + 0] = (byte)(addr & 0xff);
            return adr;
        }

        public void ProgramByte(byte b)
        {
            int i;
            int addr;
            int adr;
            int byt;
            int reg;
            int pqr;
            addr = (ram[REG_B + 1] << 8) | ram[REG_B + 0];
            reg = (addr & 0xfff);
            byt = (addr >> 12) & 0xf;
            adr = (reg * 7) + byt;
            if (ram[adr] != 0)
            {
                pqr = ((ram[REG_C + 1] & 0x0f) << 8) | ram[REG_C + 0];
                pqr *= 7;
                pqr -= 7;
                while (pqr < adr)
                {
                    ram[pqr] = ram[pqr + 7];
                    pqr++;
                }
                for (i = 0; i < 7; i++) ram[adr - i] = 0;
                pqr = ((ram[REG_C + 1] & 0x0f) << 8) | ram[REG_C + 0];
                pqr--;
                ram[REG_C + 0] = (byte)(pqr & 0xff);
                ram[REG_C + 1] &= 0xf0;
                ram[REG_C + 1] |= (byte)((pqr >> 8) & 0x0f);
                byteCount += 7;
            }
            ram[adr--] = b;
            reg = adr / 7;
            byt = adr % 7;
            addr = (byt << 12) | reg;
            ram[REG_B + 1] = (byte)((addr >> 8) & 0xff);
            ram[REG_B + 0] = (byte)(addr & 0xff);
        }

        string ProgramLine()
        {
            int addr;
            int reg;
            int byt;
            int adr;
            int lineNumber;
            int end;
            string buffer;
            addr = (ram[REG_B + 1] << 8) | ram[REG_B + 0];
            reg = (addr & 0xfff);
            byt = (addr >> 12) & 0xf;
            adr = (reg * 7) + byt;
            if (FlagSet(F_PRGM) && FlagSet(F_ALPHA_IN)) adr = alphaPos+1;
            lineNumber = ram[REG_E + 0] + ((ram[REG_E + 1] & 0x0f) << 8);
            if (lineNumber == 0xfff)
            {
                FixLineNumber();
                lineNumber = ram[REG_E + 0] + ((ram[REG_E + 1] & 0x0f) << 8);
            }
            end = ((ram[REG_C + 1] & 0x0f) << 8) | ram[REG_C + 0];
            if (lineNumber == 0)
            {
                buffer = $"00 REG {end - FindBottom():d2}";
            }
            else
            {
                adr--;
                buffer = ProgramList(lineNumber, adr);
            }

            return buffer;
        }

        public string ProgramList(int lineNumber, int adr)
        {
            int i;
            int b;
            int b2;
            int end;
            string buffer = "";
            end = ((ram[REG_C + 1] & 0x0f) << 8) | ram[REG_C + 0];
            while (ram[adr] == 0) adr--;
            if (lineNumber < 100) buffer = $"{lineNumber:d02} ";
            else buffer = $"{lineNumber} ";

            b = ram[adr];
            if (b >= 0xc0 && b < 0xce)
            {
                if (ram[adr - 2] < 0xf0)
                {
                    if ((ram[adr - 2] & 0xf0) == 0x20)
                    {
                        buffer = $".END. REG {end - FindBottom():d}";
                    }
                    else
                    {
                        if (lineNumber < 100) buffer = $"{lineNumber:d02} END";
                        else buffer = $"{lineNumber:D} END";
                    }
                }
                else
                {
                    if (lineNumber < 100) buffer = $"{lineNumber:d02} ";
                    else buffer = $"{lineNumber:d} ";
                    buffer += "LBL\x7a";
                    adr -= 2;
                    b = ram[adr] - 1;
                    b &= 0xf;
                    adr -= 2;
                    for (i = 0; i < b; i++)
                    {
                        buffer += ((char)ram[adr--]).ToString();

                    }
                }
            }
            else
            {
                if (b >= 0x10 && b <= 0x1c)
                {
                    buffer = $"{lineNumber:d2} " + ProgramNumber(ref adr);
                }
                else if (b == 0xae)
                {
                    b2 = ram[adr - 1];
                    if (b2 >= 0x80)
                    {
                        buffer = $"{lineNumber:d2} XEQ IND {b2 & 0x7f:d02}";
                    }
                    else
                    {
                        buffer = $"{lineNumber:d2} GTO IND {b2 & 0x7f:d02}";
                    }
                }
                else if (b >= 0xa0 && b <= 0xa7)
                {
                    b2 = ram[adr - 1];
                    i = 0;
                    while (reverse2[i].cmd != 0xff && (reverse2[i].cmd != b || reverse2[i].post != b2)) i++;
                    if (reverse2[i].cmd != 0xff)
                    {
                        buffer = $"{lineNumber:d2} {reverse2[i].name}";
                    }
                    else
                    {
                        b = ((b & 0x0f) << 2) | ((b2 & 0xc0) >> 6);
                        b2 &= 0x3f;
                        buffer = $"{lineNumber:d2} XROM {b:d2},{b2:d2}";
                    }
                }
                else if (b < 0xf0 && (reverse[b].size & 0x0f) == 1)
                {
                    buffer += $"{reverse[b].name}";
                }
                else if (b < 0xf0 && (reverse[b].size & 0x0f) == 2)
                {
                    buffer += Postfix((byte)b, ram[adr-1]);
                }
                else if ((reverse[b].size & 0xf0) == 0x60)
                {
                    if (ram[adr - 2] >= 102 && ram[adr - 2] <= 111)
                        buffer += $"{reverse[b].name} {ram[adr - 2] - 102 + 'A':c}";
                    else if (ram[adr - 2] >= 123 && ram[adr - 2] <= 127)
                        buffer += $"{reverse[b].name} {ram[adr - 2] - 123 + 'a':c}";
                    else
                        buffer += $"{reverse[b].name} {ram[adr - 2] & 0x7f:d02}";
                }
                else if ((reverse[b].size & 0xf0) == 0x10)
                {
                    buffer += $"{reverse[b].name}\x7a";
                    b = ram[--adr] & 0x0f;
                    for (i = 0; i < b; i++)
                    {
                        buffer += ((char)ram[--adr]).ToString();
                    }
                }
                else if (b >= 0xf0)
                {
                    adr--;
                    buffer = buffer.Substring(0, buffer.Length - 1) + "\x7a";
                    for (i = 0; i < (b & 0x0f); i++)
                    {
                        if (ram[adr] == 0x7f) { buffer += (char)0x7f; adr--; }
                        else if (ram[adr] == 0x00) { buffer += (char)0; adr--; }
                        else if (ram[adr] > 0x7f) { buffer += (char)2; adr--; }
                        else buffer += (char)ram[adr--];
                    }
                }
            }
            return buffer;
        }

        string ProgramNumber(ref int adr)
        {
            int b;
            int p;
            char mode;
            string mant = "";
            string expn = "";
            char sign;
            char esign;
            string buffer;
            mode = 'M';
            p = 0;
            sign = ' ';
            esign = ' ';
            while (ram[adr] >= 0x10 && ram[adr] <= 0x1c) adr++;
            adr--;
            b = ram[adr];
            while (b >= 0x10 && b <= 0x1c)
            {
                if (b <= 0x19)
                {
                    if (mode == 'M') mant += (char)('0' + b - 0x10);
                    else expn += (char)('0' + b - 0x10);
                }
                if (b == 0x1a) mant += '.';
                if (b == 0x1b)
                {
                    if (mode == 'M')
                    {
                        p = 0;
                        mode = 'E';
                    }
                }
                if (b == 0x1c)
                {
                    if (mode == 'M') sign = (sign == ' ') ? '-' : ' ';
                    else esign = (esign == ' ') ? '-' : ' ';
                }
                adr--;
                b = ram[adr];
            }
            buffer = "";
            if (sign == '-') buffer += "-";
            buffer += mant;
            if (mode == 'E')
            {
                buffer += " E";
                if (esign == '-') buffer += "-";
                buffer += expn;
            }
            return buffer;
        }

 
        void Push(int address)
        {
            int i;
            address = ToPtr(address);
            for (i = 6; i > 1; i--) ram[REG_A + i] = ram[REG_A + i - 2];
            ram[REG_A + 1] = ram[REG_B + 6];
            ram[REG_A + 0] = ram[REG_B + 5];
            for (i = 6; i > 1; i--) ram[REG_B + i] = ram[REG_B + i - 2];
            ram[REG_B + 3] = (byte)((address >> 8) & 0xff);
            ram[REG_B + 2] = (byte)(address & 0xff);
        }

        public Number Rcl(int rreg)
        {
            int reg;
            int b;
            b = ram[REG_C + 2] << 4;
            b |= ((ram[REG_C + 1] >> 4) & 0xf);
            if (rreg < 0x70)
            {
                reg = b + rreg;
            }
            else if (rreg >= 0x70 && rreg <= 0x7f) reg = rreg - 0x70;
            else if (rreg >= 0x80 && rreg <= 0xef)
            {
                reg = b + (rreg - 0x80);
                if (reg > 0x1ff)
                {
                    Message("NONEXISTENT");
                    Error();
                    return ZERO;
                }
                reg = b + ToInteger(RecallNumber(reg));
            }
            else if (rreg >= 0xf0 && rreg <= 0xff)
            {
                reg = b + ToInteger(RecallNumber(rreg - 0xf0));
            }
            else reg = 0x200;
            if (reg > 0x1ff)
            {
                Message("NONEXISTENT");
                Error();
                return ZERO;
            }
            return RecallNumber(reg);
        }

        public Number RecallNumber(int reg)
        {
            Number a = new Number();
            reg *= 7;
            a.sign = (byte)((ram[reg + 6] >> 4) & 0xf);
            a.mantissa[0] = (byte)(ram[reg + 6] & 0xf);
            a.mantissa[1] = (byte)((ram[reg + 5] >> 4) & 0xf);
            a.mantissa[2] = (byte)(ram[reg + 5] & 0xf);
            a.mantissa[3] = (byte)((ram[reg + 4] >> 4) & 0xf);
            a.mantissa[4] = (byte)(ram[reg + 4] & 0xf);
            a.mantissa[5] = (byte)((ram[reg + 3] >> 4) & 0xf);
            a.mantissa[6] = (byte)(ram[reg + 3] & 0xf);
            a.mantissa[7] = (byte)((ram[reg + 2] >> 4) & 0xf);
            a.mantissa[8] = (byte)(ram[reg + 2] & 0xf);
            a.mantissa[9] = (byte)((ram[reg + 1] >> 4) & 0xf);
            a.esign = (byte)(ram[reg + 1] & 0xf);
            a.exponent[0] = (byte)((ram[reg + 0] >> 4) & 0xf);
            a.exponent[1] = (byte)(ram[reg + 0] & 0xf);
            return a;
        }

        public void ReLink()
        {
            int end;
            int count;
            int last;
            int len;
            int regs;
            int bytes;
            int address;
            bool first;
            end = 0;
            count = 0;
            address = (ram[REG_C + 2] << 4) | ((ram[REG_C + 1] >> 4) & 0x0f);
            address *= 7;
            last = address;
            first = true;
            address++;
            while (end == 0)
            {
                if (ram[address] >= 0xc0 && ram[address] <= 0xcd && count > 0)
                {
                    len = last - address;
                    regs = len / 7;
                    bytes = len % 7;
                    len = (bytes << 9) | (regs & 0x1ff);
                    if (first) len = 0;
                    first = false;
                    ram[address] &= 0xf0;
                    ram[address] |= (byte)((len >> 8) & 0x0f);
                    ram[address - 1] = (byte)(len & 0xff);
                    if ((ram[address - 2] & 0xf0) == 0x20) end = -1;
                    else
                    {
                        last = address + 1;
                        count = isize(address);
                        address -= isize(address);
                    }
                }
                else
                {
                    count += isize(address);
                    address -= isize(address);
                }
            }
        }

        public void Resize(int old_r00, int new_r00)
        {
            int btm;
            int ofs;
            int end;
            int src;
            int dst;
            if (old_r00 == new_r00) return;
            if (old_r00 > new_r00)
            {
                btm = 0x0c0 * 7;
                while (ram[btm + 6] == 0xf0) btm += 7;
                btm /= 7;
                ofs = old_r00 - new_r00;
                end = ((ram[REG_C + 1] & 0x0f) << 8) | ram[REG_C + 0];
                if ((end - ofs) < btm)
                {
                    Message("NO ROOM");
                    Error();
                    return;
                }
                dst = (end - ofs) * 7;
                src = end * 7;
                while (dst < 0xe00)
                {
                    ram[dst] = (byte)((src < 0xe00) ? ram[src] : 0x00);
                    dst++;
                    src++;
                }
                ofs = -ofs;
            }
            else
            {
                ofs = new_r00 - old_r00;
                src = ((0x1ff - ofs) * 7) + 6;
                dst = (0x1ff * 7) + 6;
                end = ((ram[REG_C + 1] & 0x0f) << 8) | ram[REG_C + 0];
                end *= 7;
                while (src >= end)
                {
                    ram[dst--] = ram[src--];
                }
                end = 0x0c0 * 7;
                while (ram[end + 6] == 0xf0) end += 7;
                while (src >= end) ram[src--] = 0x00;
            }
            end = ((ram[REG_C + 1] & 0x0f) << 8) | ram[REG_C + 0];
            end += ofs;
            ram[REG_C + 1] &= 0xf0;
            ram[REG_C + 1] |= (byte)(((end >> 8) & 0x0f));
            ram[REG_C + 0] = (byte)(end & 0xff);
            ram[REG_C + 2] = (byte)(new_r00 >> 4);
            ram[REG_C + 1] &= 0x0f;
            ram[REG_C + 1] |= (byte)(((new_r00 & 0x0f) << 4));
            end = ((ram[REG_B + 1] & 0x0f) << 8) | ram[REG_B + 0];
            end += ofs;
            ram[REG_B + 1] &= 0xf0;
            ram[REG_B + 1] |= (byte)(((end >> 8) & 0x0f));
            ram[REG_B + 0] = (byte)(end & 0xff);
        }

        void Rnd()
        {
            int i;
            int s;
            int decimals;
            int mode;
            int e;
            Number a;
            decimals = 0;
            if (FlagSet(36)) decimals |= 8;
            if (FlagSet(37)) decimals |= 4;
            if (FlagSet(38)) decimals |= 2;
            if (FlagSet(39)) decimals |= 1;
            mode = 0;
            if (FlagSet(40)) mode |= 2;
            if (FlagSet(41)) mode |= 1;
            a = RecallNumber(R_X);
            if (mode == 0)
            {
                for (i = decimals; i < 10; i++)
                    if (i + 1 < 10) a.mantissa[i + 1] = 0;
            }
            if (mode == 1)
            {
                e = (a.exponent[0] * 10) + a.exponent[1];
                s = e % 3;
                for (i = decimals + s; i < 10; i++)
                    if (i + 1 < 10) a.mantissa[i + 1] = 0;
            }
            if (mode == 2)
            {
                e = (a.exponent[0] * 10) + a.exponent[1];
                for (i = decimals + e; i < 10; i++)
                    if (i + 1 < 10) a.mantissa[i + 1] = 0;
            }
            StoreNumber(a, R_X);
        }

        int Rtn(int addr)
        {
            int i;
            if (running)
            {
                if (ram[REG_B + 2] == 0x00 && ram[REG_B + 3] == 0x00)
                {
                    running = false;
                    GotoLine(0);
                    for (i = REG_B + 2; i < REG_B + 7; i++) ram[i] = 0x00;
                    for (i = REG_A + 0; i < REG_A + 7; i++) ram[i] = 0x00;
                    ram[REG_E] = 0x00;
                    ram[REG_E + 1] &= 0xf0;
                    return 0;
                }
                for (i = REG_B + 0; i < REG_B + 5; i++) ram[i] = ram[i + 2];
                ram[REG_B + 5] = ram[REG_A + 0];
                ram[REG_B + 6] = ram[REG_A + 1];
                for (i = REG_A + 0; i < REG_A + 5; i++) ram[i] = ram[i + 2];
                ram[REG_A + 5] = 0x00;
                ram[REG_A + 6] = 0x00;
                addr = (ram[REG_B + 1] << 8) | ram[REG_B + 0];
                addr = FromPtr(addr);
                return addr - 1;
            }
            else
            {
                GotoLine(0);
                for (i = REG_B + 2; i < REG_B + 7; i++) ram[i] = 0x00;
                for (i = REG_A + 0; i < REG_A + 7; i++) ram[i] = 0x00;
            }
            return addr - 1;
        }

        void Sci(byte n)
        {
            Number a;
            if (n >= 0x80 && n <= 0xef)
            {
                a = Rcl(n & 0x7f);
                if (a.sign != 9 && a.sign != 0)
                {
                    Message("ALPHA DATA");
                    Error();
                    return;
                }
                n = (byte)ToInteger(a);
            }
            else if (n >= 0xf0)
            {
                a = RecallNumber(n & 0x0f);
                n = (byte)ToInteger(a);
            }
            if (n >= 0x0a)
            {
                Message("DATA ERRROR");
                Error();
                return;
            }
            ClearFlag(40);
            ClearFlag(41);
            if ((n & 8) != 0) SetFlag(36); else ClearFlag(36);
            if ((n & 4) != 0) SetFlag(37); else ClearFlag(37);
            if ((n & 2) != 0) SetFlag(38); else ClearFlag(38);
            if ((n & 1) != 0) SetFlag(39); else ClearFlag(39);
        }

        void Sdev()
        {
            int b;
            int ofs;
            Number a;
            string tmp;
            double x, x2, meanx, n;
            b = ram[REG_C + 2] << 4;
            b |= ((ram[REG_C + 1] >> 4) & 0xf);
            ofs = ram[REG_C + 6] << 4;
            ofs |= ((ram[REG_C + 5] >> 4) & 0xf);
            b += ofs;
            a = RecallNumber(R_X);
            StoreNumber(a, R_L);

            a = RecallNumber(b + 0);
            tmp = NtoA(a);
            x = Convert.ToDouble(tmp);
            a = RecallNumber(b + 1);
            tmp = NtoA(a);
            x2 = Convert.ToDouble(tmp);
            a = RecallNumber(b + 5);
            tmp = NtoA(a);
            n = Convert.ToDouble(tmp);
            if (n == 1)
            {
                Message("DATA ERROR");
                Error();
                return;
            }
            meanx = x / n;
            x = Math.Sqrt((x2 - (x * meanx)) / (n - 1));
            tmp = $"{x:e12}";
            a = AtoN(tmp);
            StoreNumber(a, R_X);
            a = RecallNumber(b + 2);
            tmp = NtoA(a);
            x = Convert.ToDouble(tmp);
            a = RecallNumber(b + 3);
            tmp = NtoA(a);
            x2 = Convert.ToDouble(tmp);
            if (n == 1)
            {
                Message("DATA ERROR");
                Error();
                return;
            }
            meanx = x / n;
            x = Math.Sqrt((x2 - (x * meanx)) / (n - 1));
            tmp = $"{x:e12}";
            a = AtoN(tmp);
            StoreNumber(a, R_Y);
        }

        public void SetFlag(byte f)
        {
            int offset;
            offset = 6 - (f / 8);
            f = (byte)(f & 7);
            f = (byte)(0x80 >> f);
            ram[REG_D + offset] |= f;
        }

        public void SetKaFlag(byte flag, bool set)
        {
            int i;
            int f;
            int b;
            int o;
            i = 0;
            while (keys[i].keycode != 0xff && keys[i].keycode != flag && keys[i].skeycode != flag) i++;
            if (keys[i].keycode == 0xff) return;
            f = keys[i].bit - 1;
            b = 6 - (f >> 3);
            o = 7 - (f & 0x07);
            i = flag & 0x0f;
            if (i >= 1 && i <= 8)
            {                 /* unshifted */
                if (set) ram[REG_R + b] |= (byte)((1 << o));
                else ram[REG_R + b] &= (byte)((0xff ^ (1 << o)));
            }
            else
            {                                  /* shifted */
                if (set) ram[REG_E + b] |= (byte)((1 << o));
                else ram[REG_E + b] &= (byte)((0xff ^ (1 << o)));
            }
        }

        public void SetKaFlags()
        {
            int i;
            int l;
            int addr;
            int reg;
            int byt;
            for (i = REG_E + 3; i <= REG_E + 6; i++) ram[i] = 0x00;
            for (i = REG_R + 3; i <= REG_R + 6; i++) ram[i] = 0x00;
            ram[REG_E + 2] &= 0x0f;
            ram[REG_R + 2] &= 0x0f;
            addr = 0x0c0 * 7;
            while (ram[addr + 6] == 0xf0)
            {
                if (ram[addr + 4] != 0x00) SetKaFlag(ram[addr + 3], true);
                if (ram[addr + 1] != 0x00) SetKaFlag(ram[addr + 0], true);
                addr += 7;
            }
            addr = ((ram[REG_C + 1] & 0x0f) << 8) | ram[REG_C + 0];
            addr *= 7;
            addr += 2;
            l = ((ram[addr] & 0x0f) << 8) | ram[addr - 1];
            while (l != 0x000)
            {
                reg = l & 0x1ff;
                byt = (l >> 9) & 0x7;
                addr += (reg * 7 + byt);
                addr--;
                l = ((ram[addr] & 0x0f) << 8) | ram[addr - 1];
                if (ram[addr - 2] >= 0xf0)
                {
                    if (ram[addr - 3] != 0x00) SetKaFlag(ram[addr - 3], true);
                }
            }


        }

        void SetX(Number n, int l, int s)
        {
            Number a;
            if (lift && s != 0)
            {
                a = RecallNumber(R_Z);
                StoreNumber(a, R_T);
                a = RecallNumber(R_Y);
                StoreNumber(a, R_Z);
                a = RecallNumber(R_X);
                StoreNumber(a, R_Y);
            }
            if (l != 0)
            {
                a = RecallNumber(R_X);
                StoreNumber(a, R_L);
            }
            StoreNumber(n, R_X);
            lift = false;
        }

        void Sf(byte n)
        {
            Number a;
            if (n >= 0x80 && n <= 0xef)
            {
                a = Rcl(n & 0x7f);
                if (a.sign != 9 && a.sign != 0)
                {
                    Message("ALPHA DATA");
                    Error();
                    return;
                }
                n = (byte)ToInteger(a);
            }
            else if (n >= 0xf0)
            {
                a = RecallNumber(n & 0x0f);
                n = (byte)ToInteger(a);
            }
            if (n >= 30)
            {
                Message("NONEXISTENT");
                Error();
                return;
            }
            SetFlag(n);
            Annunciators();
        }

        Number Sin(Number a)
        {
            int i;
            bool flag;
            DNumber x;
            DNumber fact;
            DNumber one;
            DNumber sum;
            DNumber sqr;
            DNumber num;
            DNumber den;
            DNumber term;
            DNumber last;
            bool addsub;
            x = NumberToDNumber(a);
            one = D_ONE;
            sum = x;
            den = one;
            fact = D_TWO;
            num = x;
            sqr = D_Mul(x, x);
            addsub = true;
            flag = true;
            while (flag)
            {
                last = sum;
                num = D_Mul(num, sqr);
                den = D_Mul(den, fact);
                fact = D_Add(fact, one);
                den = D_Mul(den, fact);
                fact = D_Add(fact, one);
                term = D_Div(num, den);
                term.sign = (byte)((addsub) ? 9 : 0);
                addsub = (addsub) ? false : true;
                sum = D_Add(sum, term);
                flag = false;
                for (i = 0; i < 12; i++)
                    if (last.mantissa[i] != sum.mantissa[i]) flag = true;
            }
            return DNumberToNumber(sum);
        }


        public void Size(int n)
        {
            int end;
            int r00;
            int nw;
            int dst;
            int src;
            int dif;
            end = ((ram[REG_C + 1] & 0x0f) << 8) | ram[REG_C + 0];
            r00 = (ram[REG_C + 2] << 4) | ((ram[REG_C + 1] & 0xf0) >> 4);
            nw = 0x200 - n;
            dif = nw - r00;
            if (dif > 0)
            {
                dst = 0x1ff * 7 + 6;
                src = (0x1ff - dif) * 7 + 6;
                while (src >= (0x0c0 * 7)) ram[dst--] = ram[src--];
                while (dst >= (0x0c0 * 7)) ram[dst--] = 0x00;
                r00 += dif;
                end += dif;
            }
            else
            {
                dif = -dif;
                dst = 0x0c0 * 7;
                src = (0x0c0 + dif) * 7;
                while (src <= (0x1ff * 7 + 6)) ram[dst++] = ram[src++];
                while (dst <= (0x1ff * 7 + 6)) ram[dst++] = 0x00;
                r00 -= dif;
                end -= dif;
            }
            ram[REG_C + 2] = (byte)((r00 >> 4) & 0xff);
            ram[REG_C + 1] = (byte)(((r00 & 0x0f) << 4) | ((end >> 8) & 0x0f));
            ram[REG_C + 0] = (byte)(end & 0xff);
        }

        public int Skip(int addr)
        {
            while (ram[addr] == 0x00) addr--;
            addr -= isize(addr);
            return addr;
        }

        Number Sqrt(Number a)
        {
            int i;
            bool flag;
            DNumber x;
            DNumber e;
            DNumber last;
            DNumber t;
            if (a.sign != 0)
            {
                Message("DATA ERROR");
                Error();
                return a;
            }
            x = NumberToDNumber(a);
            e = D_Div(x, D_TWO);

            flag = true;
            while (flag)
            {
                last = e;
                t = D_Div(x, e);
                t = D_Add(t, e);
                e = D_Mul(t, D_HALF);

                flag = false;
                for (i = 0; i < 15; i++)
                    if (last.mantissa[i] != e.mantissa[i]) flag = true;
            }
            return DNumberToNumber(e);
        }

        public void Sst()
        {
            int addr;
            int adr;
            int byt;
            int reg;
            int line;
            if (FlagSet(52))
            {
                line = ram[REG_E + 0] | ((ram[REG_E + 1] & 0x0f) << 8);
                line++;
                ram[REG_E + 0] = (byte)(line & 0xff);
                ram[REG_E + 1] &= 0xf0;
                ram[REG_E + 1] |= (byte)((line >> 8) & 0x0f);
                if (line == 1) return;
                addr = (ram[REG_B + 1] << 8) | ram[REG_B + 0];
                reg = (addr & 0xfff);
                byt = (addr >> 12) & 0xf;
                adr = (reg * 7) + byt;
                adr--;
                while (ram[adr] == 0) adr--;
                if (ram[adr] < 0xc0 || ram[adr] >= 0xce || ram[adr - 2] >= 0xf0)
                {
                    adr -= isize(adr);
                    adr++;
                    reg = adr / 7;
                    byt = adr % 7;
                    addr = reg | (byt << 12);
                    ram[REG_B + 0] = (byte)(addr & 0xff);
                    ram[REG_B + 1] = (byte)((addr >> 8) & 0xff);
                }
                else
                {
                    GotoLine(1);
                }
            }
            else
            {
                if (FlagSet(22)) EndNumber();
                SetFlag(F_SST);
                running = true;
                ram[REG_E + 1] |= 0x0f;
                ram[REG_E + 0] = 0xff;
                addr = (ram[REG_B + 1] << 8) | ram[REG_B + 0];
                addr = FromPtr(addr) - 1;
                addr = Exec(addr);
                addr = ToPtr(addr + 1);
                ram[REG_B + 1] = (byte)((addr >> 8) & 0xff);
                ram[REG_B + 0] = (byte)(addr & 0xff);
                running = false;
                ClearFlag(F_SST);
            }
        }

        public void Sto(Number a, int rreg)
        {
            int reg;
            int b;
            b = ram[REG_C + 2] << 4;
            b |= ((ram[REG_C + 1] >> 4) & 0xf);
            if (rreg < 0x70)
            {
                reg = b + rreg;
            }
            else if (rreg >= 0x70 && rreg <= 0x7f) reg = rreg - 0x70;
            else if (rreg >= 0x80 && rreg <= 0xef)
            {
                reg = b + (rreg - 0x80);
                if (reg > 0x1ff)
                {
                    Message("NONEXISTENT");
                    Error();
                    return;
                }
                reg = b + ToInteger(RecallNumber(reg));
            }
            else if (rreg >= 0xf0 && rreg <= 0xff)
            {
                reg = b + ToInteger(RecallNumber(rreg - 0xf0));
            }
            else reg = 0x200;
            if (reg > 0x1ff)
            {
                Message("NONEXISTENT");
                Error();
                return;
            }
            StoreNumber(a, reg);
        }

        public void StoreNumber(Number n, int reg)
        {
            int i;
            if (reg == R_X && ram[LIFT] == 'E')
            {
                for (i = 0; i < 21; i++) ram[i] = ram[i + 7];
                ram[LIFT] = (byte)'D';
            }
            reg *= 7;
            ram[reg + 6] = (byte)((n.sign << 4) | (n.mantissa[0] & 0xf));
            ram[reg + 5] = (byte)((n.mantissa[1] << 4) | (n.mantissa[2] & 0xf));
            ram[reg + 4] = (byte)((n.mantissa[3] << 4) | (n.mantissa[4] & 0xf));
            ram[reg + 3] = (byte)((n.mantissa[5] << 4) | (n.mantissa[6] & 0xf));
            ram[reg + 2] = (byte)((n.mantissa[7] << 4) | (n.mantissa[8] & 0xf));
            ram[reg + 1] = (byte)((n.mantissa[9] << 4) | (n.esign & 0xf));
            ram[reg + 0] = (byte)((n.exponent[0] << 4) | (n.exponent[1] & 0xf));
        }

        public Number Sub(Number a, Number b)
        {
            b.sign = (byte)((b.sign == 0) ? 9 : 0);
            return Add(a, b);
        }


        Number Tan(Number a)
        {
            Number x;
            Number s;
            Number c;
            s = Sin(a);
            c = Cos(a);
            x = Div(s, c);
            return x;
        }

        void tens(int[] n)
        {
            int i;
            int c;
            for (i = 0; i < 21; i++) n[i] = 9 - n[i];
            c = 1;
            for (i = 20; i >= 0; i--)
            {
                n[i] += c;
                if (n[i] > 9)
                {
                    n[i] -= 10;
                    c = 1;
                }
                else c = 0;
            }
        }

        Number TensCompliment(Number a)
        {
            int i;
            int c;
            c = 1;
            for (i = 9; i >= 0; i--)
            {
                a.mantissa[i] = (byte)((9 - a.mantissa[i]) + c);
                if (a.mantissa[i] > 9)
                {
                    a.mantissa[i] -= 10;
                    c = 1;
                }
                else c = 0;
            }
            return a;
        }

        public int ToInteger(Number a)
        {
            int e;
            int r;
            int p;
            if (a.esign != 0) return 0;
            e = (a.exponent[0] * 10) + a.exponent[1];
            r = a.mantissa[0];
            p = 1;
            while (e > 0)
            {
                r *= 10;
                if (p < 10) r += a.mantissa[p++];
                e--;
            }
            return r;
        }

        void Tone(byte n)
        {
            Number a;
            if (n >= 0x80 && n <= 0xef)
            {
                a = Rcl(n & 0x7f);
                if (a.sign != 9 && a.sign != 0)
                {
                    Message("ALPHA DATA");
                    Error();
                    return;
                }
                n = (byte)ToInteger(a);
            }
            else if (n >= 0xf0)
            {
                a = RecallNumber(n & 0x0f);
                n = (byte)ToInteger(a);
                sound.PlayTone(n);
            }
            sound.PlayTone(n);
        }

        public int ToPtr(int addr)
        {
            int reg;
            int byt;
            reg = addr / 7;
            byt = addr % 7;
            return (reg & 0xfff) | (byt << 12);
        }

        public void UnAsn(byte keycode, int flag)
        {
            int addr;
            int dst;
            SetKaFlag(keycode, false);
            if ((flag & 1) != 0)
            {
                addr = 0x0c0 * 7;
                while (ram[addr + 6] == 0xf0)
                {
                    if (ram[addr + 0] == keycode)
                    {
                        ram[addr + 0] = 0x00;
                        ram[addr + 1] = 0x00;
                        ram[addr + 2] = 0x00;
                    }
                    if (ram[addr + 3] == keycode)
                    {
                        ram[addr + 3] = 0x00;
                        ram[addr + 4] = 0x00;
                        ram[addr + 5] = 0x00;
                    }
                    addr += 7;
                }
            }
            if ((flag & 2) != 0)
            {
                addr = ((ram[REG_C + 1] & 0x0f) << 8) | ram[REG_C + 0];
                addr = FromPtr(addr) + 2;
                dst = -1;
                while (dst != 0)
                {
                    if (ram[addr] >= 0xc0 && ram[addr] <= 0xcd &&
                        ram[addr - 2] >= 0xf0 && ram[addr - 3] == keycode)
                    {
                        ram[addr - 3] = 0x00;
                    }
                    dst = ((ram[addr] & 0x0f) << 8) | ram[addr - 1];
                    dst = ((dst & 0x1ff) * 7) + ((dst >> 9) & 0x7);
                    if (dst != 0) addr += dst - 1;
                }
            }
        }

        void View(byte post)
        {
            string tmp;
            Number a;
            a = Rcl(post);
            if (errFlag == false)
            {
                tmp = Format(a);
                Message(tmp);
                SetFlag(50);
            }
        }

        Number YtoX(Number y, Number x)
        {
            x = Ln(x);
            x = Mul(x, y);
            x = Ex(x);
            return x;
        }


    }
}
