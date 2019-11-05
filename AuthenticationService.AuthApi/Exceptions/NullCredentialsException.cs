using System;

namespace AuthenticationService.AuthApi.Exceptions
{
    public class NullCredentialsException : Exception
    {
        public NullCredentialsException(string message):base(message){}
    }
}