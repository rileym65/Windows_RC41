using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41
{
    public class Rev
    {
        public int cmd;
        public int post;
        public string name;
        public int size;
        public int flags;
        public int link;

        public Rev(int c, int p, string nm, int sz, int fl, int l)
        {
            cmd = c;
            post = p;
            name = nm;
            size = sz;
            flags = fl;
            link = l;
        }
    }
}
