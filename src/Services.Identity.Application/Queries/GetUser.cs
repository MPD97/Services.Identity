using System;
using Convey.CQRS.Queries;
using Services.Identity.Application.DTO;

namespace Services.Identity.Application.Queries
{
    public class GetUser :IQuery<UserDto>
    {
        public Guid UserId { get; set; }
    }
}