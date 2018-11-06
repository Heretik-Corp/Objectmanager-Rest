using System;

namespace ObjectManager.Rest.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }
    public class ReasonPhraseException : Exception
    {
        public ReasonPhraseException(string message) : base(message) { }
    }
}
