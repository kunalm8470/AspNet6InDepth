using System.Runtime.Serialization;

namespace Api.Exceptions;

public class RefreshTokenNotFoundException : Exception
{
    public RefreshTokenNotFoundException()
    {
    }

    public RefreshTokenNotFoundException(string message) : base(message)
    {
    }

    public RefreshTokenNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected RefreshTokenNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
