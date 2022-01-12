using FluentValidation.Results;
using Flunt.Notifications;

namespace IWantApp.Endpoints;

public static class ProblemDetailsExtensions
{
    public static Dictionary<string, string[]> ConvertToProblemDetails(this List<ValidationFailure> errors)
        => errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(e => e.Key, e => e.Select(x => x.ErrorMessage).ToArray());
   
}
