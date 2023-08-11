using System.Runtime.Serialization;

namespace Route256.PriceCalculator.Domain.Exceptions
{
    public sealed class DomainException : Exception
    {
        public DomainException()
        {
        }

        public DomainException(string? message) : base(message)
        {
        }

        public DomainException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public DomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
