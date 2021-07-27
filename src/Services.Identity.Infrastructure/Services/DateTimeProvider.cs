using System;
using Services.Identity.Application.Services;

namespace Services.Identity.Infrastructure.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now  => DateTime.UtcNow;
    }
}