using System;

using Bogus;
using Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Test.Green.Domain.ValueObjects;
public class BigStringTests
{
    [Fact]
    public void Should_Create_Valid_BigString()
    {
        // Arrange
        var faker = new Faker();
        var text = faker.Lorem.Paragraphs(3); // < 5000 chars

        // Act
        var bigString = new BigString(text);

        // Assert
        bigString.Body.Should().Be(text);
        bigString.IsValid.Should().BeTrue();
        bigString.Notifications.Should().BeEmpty();
    }

    [Fact]
    public void Should_Allow_Empty_String()
    {
        var bigString = new BigString(string.Empty);

        bigString.Body.Should().BeEmpty();
        bigString.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Allow_Null_String()
    {
        var bigString = new BigString(null);

        bigString.Body.Should().BeNull();
        bigString.IsValid.Should().BeTrue();
    }
}
