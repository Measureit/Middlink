using System;
using System.Collections.Generic;
using System.Text;

namespace Middlink.Exceptions
{
    public class MiddlinkException : Exception
    {
        public string Code { get; }

        public MiddlinkException()
        {
        }

        public MiddlinkException(string code)
        {
            Code = code;
        }

        public MiddlinkException(string message, params object[] args)
            : this(string.Empty, message, args)
        {
        }

        public MiddlinkException(string code, string message, params object[] args)
            : this(null, code, message, args)
        {
        }

        public MiddlinkException(Exception innerException, string message, params object[] args)
            : this(innerException, string.Empty, message, args)
        {
        }

        public MiddlinkException(Exception innerException, string code, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            Code = code;
        }
    }
}
