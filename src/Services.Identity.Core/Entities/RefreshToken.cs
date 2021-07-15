using System;
using Services.Identity.Core.Exceptions;

namespace Services.Identity.Core.Entities
{
    public class RefreshToken : AggregateRoot
    {
        public AggregateId UserId { get; }
        public string Token { get; }
        public DateTime CreatedAt { get; }
        public DateTime? RevokedAt { get; private set; }
        public bool Revoked => RevokedAt.HasValue;

        protected RefreshToken()
        {
        }

        public RefreshToken(AggregateId id, AggregateId userId, string token, DateTime createdAt,
            DateTime? revokedAt = null)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new EmptyRefreshTokenException();
            }

            Id = id;
            UserId = userId;
            Token = token;
            CreatedAt = createdAt;
            RevokedAt = revokedAt;
        }

        public void Revoke(DateTime revokedAt)
        {
            if (Revoked)
            {
                throw new AlreadyRevokedRefreshTokenException();
            }

            RevokedAt = revokedAt;
        }
    }
}