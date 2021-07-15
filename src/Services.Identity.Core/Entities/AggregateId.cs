using System;
using Services.Identity.Core.Exceptions;

namespace Services.Identity.Core.Entities
{
    public class AggregateId : IEquatable<AggregateId>
    {
        public Guid Value { get; }

        public AggregateId()
        {
            Value = Guid.NewGuid();
        }

        public AggregateId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new InvalidAggregateIdException();
            }
            Value = value;
        }

        public bool Equals(AggregateId other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is AggregateId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(AggregateId left, AggregateId right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AggregateId left, AggregateId right)
        {
            return !Equals(left, right);
        }
        
        public static implicit operator Guid(AggregateId id)
            => id.Value;

        public static implicit operator AggregateId(Guid id)
            => new AggregateId(id);

        public override string ToString() => Value.ToString();
    }
}