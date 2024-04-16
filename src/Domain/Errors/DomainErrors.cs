using Domain.Shared;

namespace Domain.Errors;

public static class DomainErrors
{
    public static class Common
    {
        public static Error TooLongLength() => new("Length is too long");
    }
    
    public static class FullName
    {
        public static Error FirstNameLengthOutOfRange() => new("Firstname length is out of range");

        public static Error LastnameLengthOutOfRange() => new("Lastname length is out of range");
    }
    
    public static class PasswordHash
    {
        public static Error LengthOutOfRange() => new("Password length is out of range");

        public static Error InvalidFormat() =>
            new("Password must meet the following criteria: " +
                "at least one lowercase letter, at least one uppercase letter, " +
                "at least one digit and at least one special character");
    }
    
    public static class UserRegistration
    {
        public static Error InvalidOrExpiredConfirmationCode() => new("Confirmation code is invalid or expired");

        public static Error NotFound() => new("User with provided id was not found");

        public static Error AlreadyConfirmed() => new("User email already confirmed");
    }
    
    public static class User
    {
        public static Error InvalidCredentials() => new("Email or password is incorrect");

        public static Error InvalidRefreshToken() => new("Refresh token is invalid");

        public static Error RefreshSessionExpired() => new("Refresh token expired");
        public static Error Unauthorized() => new("User is not authorized");
    }

    public static class Email
    {
        public static Error InvalidEmail() => new("Email is invalid");
    }

    public static class Product
    {
        public static Error InvalidPriceValue() => new("Price should be greater than 0");

        public static Error InvalidDimentions() => new("Dimensions values should be greater than 0");

        public static Error NotFound() => new("Specified product was not found");

        public static Error RangeNotFound() => new("Some of products were not found");
    }

    public static class Category
    {
        public static Error RangeNotFound() => new("Some of categories were not found");

        public static Error NotFound() => new("Such category were not found");
    }

    public static class Order
    {
        public static Error InvalidQuantity() => new("Quantity should be greater than 0");

        public static Error InvalidTotalAmount() => new("Total amount should be greater than 0");

        public static Error NotFound() => new("Specified order was not found");
    }

    public static class Role
    {
        public static Error NotFound() => new("Specified role was not found");
    }
}