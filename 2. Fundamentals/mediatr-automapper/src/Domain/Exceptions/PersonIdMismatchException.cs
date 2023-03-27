using System.Runtime.Serialization;

namespace Domain.Exceptions;

public class PersonIdMismatchException : Exception
{
    public PersonIdMismatchException()
    {
    }

    public PersonIdMismatchException(string message) : base(message)
    {
    }

    public PersonIdMismatchException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected PersonIdMismatchException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
