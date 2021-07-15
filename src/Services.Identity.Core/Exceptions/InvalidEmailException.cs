namespace Services.Identity.Core.Exceptions
{
    public class InvalidEmailException : DomainException
    {
        public override string Code => "invalid_email";
        public InvalidEmailException() : base("Email is not valid")
        {
        }
    }
}