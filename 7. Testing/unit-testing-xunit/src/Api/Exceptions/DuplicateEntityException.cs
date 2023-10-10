using System.Runtime.Serialization;

namespace Api.Exceptions;

public class DuplicateEntityException : Exception
{
    public DuplicateEntityException()
    {
    }

    public DuplicateEntityException(string message) : base(message)
    {
    }

    public DuplicateEntityException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected DuplicateEntityException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
