namespace UserApi.Exceptions.CustomExceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException(string message) : base(message) { }
    }

    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException(string message) : base(message) { }
    }
    public class EmailAlreadyInUseException : Exception
    {
        public EmailAlreadyInUseException(string message) : base(message) { }
    }
}
