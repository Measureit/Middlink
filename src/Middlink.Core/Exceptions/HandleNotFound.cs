using System;

namespace Middlink.Core.Exceptions
{
    public class HandleNotFound : Exception
    {
        public Type EventType { get; }

        public HandleNotFound(Type eventType)
        {
            EventType = eventType;
        }
    }
}
