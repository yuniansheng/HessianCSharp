using hessiancsharp.io;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HessianCSharp.Test
{
    public class Util
    {
        public static CHessian2Input GetHessian2Input(string content)
        {
            var buffer = new byte[content.Length / 2];
            for (int i = 0; i < content.Length; i += 2)
            {
                buffer[i / 2] = Convert.ToByte(content.Substring(i, 2), 16);
            }

            var stream = new MemoryStream(buffer);
            CHessian2Input input = new CHessian2Input(stream);
            return input;
        }
    }
}
