using System.Collections.Generic;
using Services.Identity.Core.Exceptions;

namespace Services.Identity.Core.Entities
{
    public abstract class AggregateRoot
    {
        private readonly IList<IDomainEvent> _events = new List<IDomainEvent>();
        public IEnumerable<IDomainEvent> Events => _events;
        
        public AggregateId Id { get; protected set; }
        public int Version { get; protected set; }

        protected void AddEvent(IDomainEvent @event)
        {
            if (_events is null)
            {
                throw new NullDomainEventException();
            }
            _events.Add(@event);
        }

        public void ClearEvents() => _events.Clear();
    }
}