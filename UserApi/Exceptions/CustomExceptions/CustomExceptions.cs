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

    public class PasswordNotMatch : Exception
    {
        public PasswordNotMatch(string message) : base(message) { }
    }
    public class PasswordNotCorrect : Exception
    {
        public PasswordNotCorrect(string message) : base(message) { }
    }

    public class RoleSelectedWrong : Exception
    {
        public RoleSelectedWrong(string message) : base(message) { }
    }

}
