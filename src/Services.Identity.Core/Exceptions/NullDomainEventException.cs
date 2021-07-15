namespace Services.Identity.Core.Exceptions
{
    public class NullDomainEventException :DomainException
    {
        public override string Code => "null_domain_event";

        public NullDomainEventException() : base("Domain event is null.")
        {
        }
    }
}