using Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Test.Red.Domain.ValueObjects;

public class UrlTests
{
    [Theory]
    [InlineData("invalid_url")]
    [InlineData("htp:/url.errada")]
    [InlineData("just-text")]
    [InlineData("")]
    [InlineData(null)]
    public void Should_Return_Notification_When_Url_Is_Invalid(string invalid)
    {
        // Act
        var result = new Url(invalid);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Notifications.Should().ContainSingle()
            .Which.Message.Should().Be("Url Inválida");
        result.Endereco.Should().BeNull();
    }
}
