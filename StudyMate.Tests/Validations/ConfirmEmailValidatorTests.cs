using StudyMate.DTOs.Authentication;
using StudyMate.Validations.Authentication;

namespace StudyMate.Tests.Validations;

public class ConfirmEmailValidatorTests
{
    [Fact]
    public void Should_Fail_When_Email_Is_Empty()
    {
        var validator = new ConfirmEmailValidator();
        var result = validator.Validate(new ConfirmEmail { Email = "", Code = 26656 });
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Should_Fail_When_Code_Is_Empty()
    {
        var validator = new ConfirmEmailValidator();
        var result = validator.Validate(new ConfirmEmail { Email = "test@example.com" });
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Should_Pass_With_Valid_Data()
    {
        var validator = new ConfirmEmailValidator();
        var result = validator.Validate(new ConfirmEmail { Email = "test@example.com", Code = 55265 });
        Assert.True(result.IsValid);
    }
}