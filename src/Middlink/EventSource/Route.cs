using Middlink.Exceptions;
using System;
using System.Collections.Generic;

namespace Middlink.EventSource
{
    public class Route<T> : Dictionary<Type, Action<T>>
    {
        private readonly bool _aggregateHandleIsMandatory;

        public Route(bool aggregateHandleIsMandatory = true)
        {
            _aggregateHandleIsMandatory = aggregateHandleIsMandatory;
        }

        public void Handle(T @event)
        {
            var eventType = @event.GetType();

            if (this.TryGetValue(eventType, out var handler))
            {
                handler(@event);
            }

            else if (_aggregateHandleIsMandatory)
            {
                throw new HandleNotFound(eventType);
            }
        }
    }
}
