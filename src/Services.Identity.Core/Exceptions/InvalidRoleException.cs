namespace Services.Identity.Core.Exceptions
{
    public class InvalidRoleException : DomainException
    {
        public override string Code => "invalid_role";

        public InvalidRoleException() : base("Role is not valid.")
        {
        }
    }
}