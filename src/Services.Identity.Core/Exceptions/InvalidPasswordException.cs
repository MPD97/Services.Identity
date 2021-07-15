namespace Services.Identity.Core.Exceptions
{
    public class InvalidPasswordException : DomainException
    {
        public override string Code => "invalid_password";
        public InvalidPasswordException() : base("Password is not valid.")
        {
        }
    }
}