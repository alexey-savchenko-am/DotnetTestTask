using System.Text.Json.Serialization;

namespace SharedKernel.Core.Output;

public class Result<TValue>
    : Result
{
    private readonly TValue? _value;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public TValue Value => IsSuccess
        ? _value!
        : default(TValue);//throw new InvalidOperationException("The value of the failure result can not be accessed");

    protected Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public static Result<TValue> Success(TValue value) => new(value!, true, Error.None);
    public static new Result<TValue> Failure(Error error) => new(default, false, error);

    public static implicit operator Result<TValue>(TValue value) => Success(value);
    public static implicit operator Result<TValue>(Error error) => Failure(error);
    public static implicit operator TValue(Result<TValue> result) => result.Value;
}
