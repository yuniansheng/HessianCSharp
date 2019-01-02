using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace hessiancsharp.util
{
    public static class ByteExtensions
    {
        public static sbyte[] ToSignedByteArray(this byte[] buffer)
        {
            var copy = new sbyte[buffer.Length];
            new MemoryStream((byte[])(Array)copy).Write(buffer, 0, buffer.Length);
            return copy;
        }
    }
}
