using System;
using System.Collections.Generic;
using System.Text;

namespace Middlink.Exceptions
{
    public class InfrastructureException : Exception
    {
        public string Code { get; }

        public InfrastructureException()
        {
        }

        public InfrastructureException(string code)
        {
            Code = code;
        }

        public InfrastructureException(string message, params object[] args)
            : this(string.Empty, message, args)
        {
        }

        public InfrastructureException(string code, string message, params object[] args)
            : this(null, code, message, args)
        {
        }

        public InfrastructureException(Exception innerException, string message, params object[] args)
            : this(innerException, string.Empty, message, args)
        {
        }

        public InfrastructureException(Exception innerException, string code, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            Code = code;
        }
    }
}
