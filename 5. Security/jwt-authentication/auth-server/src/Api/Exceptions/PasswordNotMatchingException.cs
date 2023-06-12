using System.Runtime.Serialization;

namespace Api.Exceptions;

public class PasswordNotMatchingException : Exception
{
    public PasswordNotMatchingException()
    {
    }

    public PasswordNotMatchingException(string message) : base(message)
    {
    }

    public PasswordNotMatchingException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected PasswordNotMatchingException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
