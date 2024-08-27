using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41
{
    public class Catalog
    {
        public int catalog;
        public string name;
        public byte cmd;
        public byte post;
        public int key;
        public int flags;

        public Catalog(int ct, string n, byte c, byte p, int k, int f)
        {
            catalog = ct;
            name = n;
            cmd = c;
            post = p;
            key = k;
            flags = f;
        }
    }
}
