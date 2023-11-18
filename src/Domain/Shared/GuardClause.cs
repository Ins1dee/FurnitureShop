using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Ardalis.GuardClauses;

namespace Domain.Shared;

public static class GuardClause
{
    public static void InvalidEmail(
        this IGuardClause guardClause,
        string input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null)
    {
        EmailAddressAttribute emailValidator = new();

        if (!emailValidator.IsValid(input))
        {
            throw new ApplicationException(message);
        }
    }
}