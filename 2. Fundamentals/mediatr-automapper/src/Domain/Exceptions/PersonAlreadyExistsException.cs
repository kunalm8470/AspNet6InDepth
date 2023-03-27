using System.Runtime.Serialization;

namespace Domain.Exceptions;

public class PersonAlreadyExistsException : Exception
{
    public PersonAlreadyExistsException()
    {
    }

    public PersonAlreadyExistsException(string? message) : base(message)
    {
    }

    public PersonAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected PersonAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
