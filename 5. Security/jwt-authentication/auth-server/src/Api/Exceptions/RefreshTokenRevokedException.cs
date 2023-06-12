using System.Runtime.Serialization;

namespace Api.Exceptions;

public class RefreshTokenRevokedException : Exception
{
    public RefreshTokenRevokedException()
    {
    }

    public RefreshTokenRevokedException(string message) : base(message)
    {
    }

    public RefreshTokenRevokedException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected RefreshTokenRevokedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
