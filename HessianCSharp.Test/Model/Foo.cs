using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.caucho.model
{
    public class Foo
    {
        public int plusInt64;
        public int plusInt128;
        public int plusInt2047;
        public int minusInt64;
        public int minusInt128;
        public int minusInt2047;

        public double double1point234;
        public double zeroDouble;
        public double minusDouble64;
        public double plusDouble64;

        public sbyte[] buffer;

        public long longValue;
        public bool bValue;
        public string sValue;
    }
}
