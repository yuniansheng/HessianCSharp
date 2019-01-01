using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace hessiancsharp.io
{
    public class HessianProtocolException : IOException
    {
        public HessianProtocolException()
        {
        }

        public HessianProtocolException(string message) : base(message)
        {
        }

        public HessianProtocolException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public HessianProtocolException(string message, int hresult) : base(message, hresult)
        {
        }

        protected HessianProtocolException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
