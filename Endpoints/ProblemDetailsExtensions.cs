using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace IWantApp.Endpoints;

public static class ProblemDetailsExtensions
{
    public static Dictionary<string, string[]> ConvertToProblemDetails(this List<ValidationFailure> errors)
        => errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(e => e.Key, e => e.Select(x => x.ErrorMessage).ToArray());

    public static Dictionary<string, string[]> ConvertToProblemDetails(this IEnumerable<IdentityError> errors )
    {
        var dictionary = new Dictionary<string, string[]>();
        dictionary.Add("Error", errors.Select(e => e.Description).ToArray());
        return dictionary;
    }


}
