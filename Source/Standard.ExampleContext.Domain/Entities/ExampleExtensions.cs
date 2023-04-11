using Standard.ExampleContext.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace Standard.ExampleContext.Domain.Entities;

public static class ExampleExtensions
{
    public static string GetFullName(this Example example)
    {
        return $"{example.FirstName} {example.Surname}";
    }

    public static void SetCreatedDate(this Example example)
    {
        example.Created = DateTime.Now;
    }

    public static void SetUpdatedDate(this Example example)
    {
        example.Updated = DateTime.Now;
    }

    public static void ValidatePassword(this Example example)
    {
        if (string.IsNullOrEmpty(example.Password))
            throw new ValidationException("The Password is required.");

        if (example.Password.Length < 8 || example.Password.Length > 20)
            throw new ValidationException(
                "The Surname must be a string with a minimum length of 8 and a maximum length of 20.");
    }

    public static void ValidateEmail(this Example example)
    {
        if (string.IsNullOrEmpty(example.Email))
            throw new ValidationException("The Email is required.");

        if (!Regex.IsMatch(example.Email,
                @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                RegexOptions.IgnoreCase))
            throw new ValidationException("The Email is not a valid e-mail address.");
    }

    public static void ValidateSurname(this Example example)
    {
        if (string.IsNullOrEmpty(example.Surname))
            throw new ValidationException("The Surname is required.");

        if (example.Surname.Length < 2 || example.Surname.Length > 100)
            throw new ValidationException(
                "The Surname must be a string with a minimum length of 2 and a maximum length of 100.");
    }

    public static void ValidateFistName(this Example example)
    {
        if (string.IsNullOrEmpty(example.FirstName))
            throw new ValidationException("The First Name is required.");

        if (example.FirstName.Length < 2 || example.FirstName.Length > 100)
            throw new ValidationException(
                "The First Name must be a string with a minimum length of 2 and a maximum length of 100.");
    }
}