using System;
using System.Collections.Generic;
using System.Text;

namespace hessiancsharp.io
{
    /// <summary>
    /// Exception for faults when the fault doesn't return a java exception.
    /// This exception is required for MicroHessianInput.
    /// </summary>
    public class HessianServiceException : Exception
    {
        public string Code { get; }

        public object Detail { get; }

        public HessianServiceException()
        {
        }

        public HessianServiceException(string message, string code, object detail) : base(message)
        {
            this.Code = code;
            this.Detail = detail;
        }
    }
}
