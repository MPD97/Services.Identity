using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;

namespace Services.Identity.Application
{
    public static class Extension
    {
        public static IConveyBuilder AddApplication(this IConveyBuilder builder) 
        => builder
            .AddCommandHandlers()
            .AddEventHandlers()
            .AddInMemoryCommandDispatcher()
            .AddInMemoryEventDispatcher();
    }
}