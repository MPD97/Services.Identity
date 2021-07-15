namespace Services.Identity.Core.Exceptions
{
    public class AlreadyRevokedRefreshTokenException : DomainException
    {        
        public override string Code => "already_revoked_refresh_token";
        public AlreadyRevokedRefreshTokenException() : base("Refresh token is already revoked.")
        {
        }
    }
}