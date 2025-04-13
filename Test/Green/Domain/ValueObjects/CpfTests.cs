using System;
using Bogus;
using Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Test.Green.Domain.ValueObjects;

public class CpfTests
{
    [Fact]
    public void Should_Create_Valid_Cpf()
    {
        // Arrange
        var validCpf = "529.982.247-25"; 

        // Act
        var cpf = new Cpf(validCpf);

        // Assert
        cpf.IsValid.Should().BeTrue();
        cpf.Notifications.Should().BeEmpty();
        cpf.Numero.Should().Be("52998224725");
        cpf.ToString().Should().Be("529.982.247-25");
    }

    [Fact]
    public void Should_Create_Valid_Cpf_Without_Dots_Or_Dashes()
    {
        // Arrange
        var validCpf = "52998224725";

        // Act
        var cpf = new Cpf(validCpf);

        // Assert
        cpf.IsValid.Should().BeTrue();
        cpf.Notifications.Should().BeEmpty();
        cpf.Numero.Should().Be(validCpf);
        cpf.ToString().Should().Be("529.982.247-25");
    }
}
