using System;

namespace ObjectManager.Rest.Exceptions
{
    public class EventHandlerFailedException : Exception
    {
        public EventHandlerFailedException(string message) : base(message) { }
    }
}
