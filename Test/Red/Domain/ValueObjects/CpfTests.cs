using System;
using Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Test.Red.Domain.ValueObjects;

public class CpfTests
{
    [Theory]
    [InlineData("000.000.000-00")]
    [InlineData("123.456.789-10")]
    [InlineData("11111111111")]
    [InlineData("99999999999")]
    [InlineData("")]
    [InlineData(null)]
    public void Should_Return_Notification_When_Cpf_Is_Invalid(string invalidCpf)
    {
        // Act
        var cpf = new Cpf(invalidCpf);

        // Assert
        cpf.IsValid.Should().BeFalse();
        cpf.Notifications.Should().ContainSingle()
            .Which.Message.Should().Be("CPF inválido");
    }

    [Fact]
    public void Should_Not_Format_Invalid_Cpf()
    {
        // Arrange
        var invalidCpf = "123";

        // Act
        var cpf = new Cpf(invalidCpf);

        // Assert
        cpf.IsValid.Should().BeFalse();
        cpf.ToString().Should().NotBe("123.456.789-10"); // não deve formar padrão válido
    }
}
