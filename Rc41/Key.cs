using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41
{
    public class Key
    {
        public string key;
        public byte cmd;
        public byte scmd;
        public byte keycode;
        public byte skeycode;
        public byte bit;
        public byte alpha;
        public byte salpha;
        public Key(string k, byte c, byte sc, byte kc, byte skc, byte bt, byte a, byte sa)
        {
            key = k;
            cmd = c;
            scmd = sc;
            keycode = kc;
            skeycode = skc;
            bit = bt;
            alpha = a;
            salpha = sa;
        }
    }
}
