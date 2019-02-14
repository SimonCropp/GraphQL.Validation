using FluentValidation;
using GraphQL.FluentValidation;
using ObjectApproval;
using Xunit;

public class NotWhiteSpaceTests
{
    [Fact]
    public void Null_valid()
    {
        var instance = new TheClass();

        Validate(instance);
    }

    [Fact]
    public void Value_valid()
    {
        var instance = new TheClass
        {
            Member = "Content"
        };

        Validate(instance);
    }
    [Fact]
    public void Space_invalid()
    {
        var instance = new TheClass
        {
            Member = " "
        };

        Validate(instance);
    }

    [Fact]
    public void Tab_invalid()
    {
        var instance = new TheClass
        {
            Member = "	"
        };

        Validate(instance);
    }

    [Fact]
    public void Empty_invalid()
    {
        var instance = new TheClass
        {
            Member = string.Empty
        };

        Validate(instance);
    }

    [Fact]
    public void Newline_invalid()
    {
        var instance = new TheClass
        {
            Member = "\r"
        };

        Validate(instance);
    }

    static void Validate(TheClass instance)
    {
        var validator = new TheValidator();
        var result = validator.Validate(instance);
        ObjectApprover.VerifyWithJson(result);
    }

    class TheValidator : AbstractValidator<TheClass>
    {
        public TheValidator()
        {
            RuleFor(_ => _.Member)
                .NotWhiteSpace();
        }
    }

    class TheClass
    {
        public string Member { get; set; }
    }
}