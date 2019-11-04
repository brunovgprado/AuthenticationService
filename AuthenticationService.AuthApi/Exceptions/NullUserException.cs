using System;

namespace AuthenticationService.AuthApi.Exceptions
{
    public class NullUserException : Exception
    {
        public NullUserException(string message):base(message){}
    }
}