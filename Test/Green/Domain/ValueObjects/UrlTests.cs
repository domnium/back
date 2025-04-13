using Bogus;
using Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Test.Green.Domain.ValueObjects;

public class UrlTests
{
    [Fact]
    public void Should_Create_Valid_Url()
    {
        // Arrange
        var url = "https://github.com/kaue";

        // Act
        var result = new Url(url);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Notifications.Should().BeEmpty();
        result.Endereco.Should().Be(url);
    }

    [Fact]
    public void Should_Trim_Url_And_Still_Be_Valid()
    {
        // Arrange
        var rawUrl = "   https://example.com/teste  ";
        var expected = "https://example.com/teste";

        // Act
        var result = new Url(rawUrl);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Endereco.Should().Be(expected);
    }
}
