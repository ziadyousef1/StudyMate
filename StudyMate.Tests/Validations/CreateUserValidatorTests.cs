using Xunit;
using StudyMate.DTOs.Authentication;
using StudyMate.Validations.Authentication;

public class CreateUserValidatorTests
{
    private readonly CreateUserValidator _validator = new();

    [Fact]
    public void Should_Fail_When_FirstName_Is_Missing()
    {
        var user = new RegisterUser
        {
            LastName = "Ahmed", Email = "Ahmed@.com", Password = "Abc123!", ConfirmPassword = "Abc123!"
        };
        var result = _validator.Validate(user);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "First name is required");
    }

    [Fact]
    public void Should_Fail_When_Email_Is_Invalid()
    {
        var user = new RegisterUser { FirstName = "Ahmed", LastName = "Ahsraf", Email = "notanemail", Password = "Abc123!", ConfirmPassword = "Abc123!" };
        var result = _validator.Validate(user);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Email is not valid");
    }

    [Fact]
    public void Should_Fail_When_Password_Too_Short()
    {
        var user = new RegisterUser
        {
            FirstName = "Ahmed", LastName = "Ashraf", Email = "Ahmed@.com", Password = "A1!", ConfirmPassword = "A1!"
        };
        var result = _validator.Validate(user);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Password must be at least 6 characters");
    }

    [Fact]
    public void Should_Fail_When_Passwords_Do_Not_Match()
    {
        var user = new RegisterUser
        {
            FirstName = "Ahmed", LastName = "Ashraf", Email = "Ahmed@.com", Password = "Abc123!", ConfirmPassword = ":)"
        };
        var result = _validator.Validate(user);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Passwords do not match");
    }

    [Fact]
    public void Should_Pass_With_Valid_Data()
    {
        var user = new RegisterUser
        {
            FirstName = "Ahmed", LastName = "Ashraf", Email = "Ahmed@.com", Password = "Abc123!", ConfirmPassword = "Abc123!"
        };
        var result = _validator.Validate(user);
        Assert.True(result.IsValid);
    }
}
