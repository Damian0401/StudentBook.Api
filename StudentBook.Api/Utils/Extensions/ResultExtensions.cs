using Microsoft.AspNetCore.Http.HttpResults;
using StudentBook.Api.Common.Shared;

namespace StudentBook.Api.Utils.Extensions;

public static class ResultExtensions
{
    public static Results<NoContent, NotFound, ValidationProblem> ResolveStatusCode(
        this Result result)
    {
        if (result.IsSuccess)
        {
            return TypedResults.NoContent();
        }

        return result.ErrorType switch
        {
            ResultErrorType.Invalid => TypedResults.ValidationProblem(result.Errors),
            ResultErrorType.NotFound => TypedResults.NotFound(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}