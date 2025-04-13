using System;
using Domain.ValueObjects;
using FluentAssertions;

namespace Test.Red.Domain.ValueObjects;

public class BigStringTests
{
     [Fact]
    public void Should_Return_Notification_When_Text_Exceeds_5000_Characters()
    {
        // Arrange
        var text = new string('A', 5001);

        // Act
        var bigString = new BigString(text);

        // Assert
        bigString.IsValid.Should().BeFalse();
        bigString.Notifications.Should().ContainSingle()
            .Which.Message.Should().Be("Body cannot be longer than 5000 characters");

        bigString.Body.Should().Be(text); 
    }
}
