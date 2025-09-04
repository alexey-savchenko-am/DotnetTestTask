using System.Text.Json.Serialization;

namespace SharedKernel.Core.Output;

public class Error
    : IEquatable<Error>
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Code { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Message { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Error? InnerError { get; }


    protected Error()
    { }

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public Error(string code, string message, Error innerError)
        : this(code, message)
    {
        InnerError = innerError;
    }

    public static readonly Error None = new();
    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.");
    public static implicit operator string?(Error error) => error.Code;
    public static implicit operator Result(Error error) => Result.Failure(error);

    public bool Equals(Error? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (Code == other.Code && Message == other.Message)
        {
            return true;
        }

        return false;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || obj is not Error other)
        {
            return false;
        }

        return Equals(other);
    }

    public override int GetHashCode()
    {
        return Code?.Length ?? 0;
    }
}
