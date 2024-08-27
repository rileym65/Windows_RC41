using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41
{
    public class Rev2
    {
        public int cmd;
        public int post;
        public string name;

        public Rev2(int c, int p, string n)
        {
            cmd = c;
            post = p;
            name = n;
        }
    }
}
