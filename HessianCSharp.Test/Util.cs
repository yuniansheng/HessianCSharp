using hessiancsharp.io;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HessianCSharp.Test
{
    public static class Util
    {
        public static byte[] Serialize2(object obj)
        {
            using (var stream = new MemoryStream())
            {
                CHessian2Output output = new CHessian2Output(stream);
                output.WriteObject(obj);
                output.Close();
                return stream.ToArray();
            }
        }

        public static object Deserialize2(byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer))
            {
                CHessian2Input input = new CHessian2Input(stream);
                return input.ReadObject();
            }
        }

        public static byte[] ToBuffer(this string content)
        {
            var buffer = new byte[content.Length / 2];
            for (int i = 0; i < content.Length; i += 2)
            {
                buffer[i / 2] = Convert.ToByte(content.Substring(i, 2), 16);
            }
            return buffer;
        }

        public static string ToHexString(this byte[] buffer)
        {
            var builder = new StringBuilder(buffer.Length * 2);
            for (int i = 0; i < buffer.Length; i++)
            {
                builder.Append(buffer[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
