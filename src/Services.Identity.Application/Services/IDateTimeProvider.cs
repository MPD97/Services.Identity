using System;

namespace Services.Identity.Application.Services
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}