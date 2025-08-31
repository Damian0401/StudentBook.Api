namespace StudentBook.Api.Common.Shared;

public enum ResultErrorType
{
    None,
    NotFound,
    Invalid,
}

public class Result
{
    public required bool IsSuccess { get; init; }
    public bool IsFailure => !this.IsSuccess;
    public required ResultErrorType ErrorType { get; init; }
    public required IDictionary<string, string[]> Errors { get; init; }

    public static Result Success()
    {
        return new()
        {
            IsSuccess = true,
            ErrorType = ResultErrorType.None,
            Errors = new Dictionary<string, string[]>(),
        };
    }

    public static Result Failure(
        ResultErrorType errorType,
        IDictionary<string, string[]>? errors = null)
    {
        return new()
        {
            IsSuccess = false,
            ErrorType = errorType,
            Errors = errors ?? new Dictionary<string, string[]>(),
        };
    }
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;
    public TValue Value
    {
        get
        {
            if (this._value is null || this.IsFailure)
            {
                throw new InvalidOperationException();
            }
            return this._value;
        }
        init => this._value = value;
    }

    public new static Result<TValue> Success(TValue value)
    {
        return new()
        {
            IsSuccess = true,
            Value = value,
            ErrorType = ResultErrorType.None,
            Errors = new Dictionary<string, string[]>(),
        };
    }

    public new static Result<TValue> Failure(
        ResultErrorType errorType,
        IDictionary<string, string[]> errors)
    {
        return new()
        {
            IsSuccess = false,
            ErrorType = errorType,
            Errors = errors,
        };
    }
}