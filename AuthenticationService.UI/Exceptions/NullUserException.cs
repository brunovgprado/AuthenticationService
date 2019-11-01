using System;

namespace AuthenticationService.UI.Exceptions
{
    public class NullUserException : Exception
    {
        public NullUserException(string message):base(message){}
    }
}