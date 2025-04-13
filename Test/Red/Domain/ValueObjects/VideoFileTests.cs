using Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Test.Red.Domain.ValueObjects;

public class VideoFileTests
{
    [Fact]
    public void Should_Return_Notification_When_File_Is_Null()
    {
        // Act
        var videoFile = new VideoFile(null, "video.mp4");

        // Assert
        videoFile.IsValid.Should().BeFalse();
        videoFile.Notifications.Should().ContainSingle()
            .Which.Message.Should().Be("File cannot be null");
    }

    [Fact]
    public void Should_Return_Notification_When_File_Is_Too_Large()
    {
        // Arrange
        var file = new MemoryStream(new byte[10_000_000_001]); // 10GB + 1 byte

        // Act
        var videoFile = new VideoFile(file, "video.mp4");

        // Assert
        videoFile.IsValid.Should().BeFalse();
        videoFile.Notifications.Should().ContainSingle()
            .Which.Message.Should().Be("File size must be less than 10MB");
    }

    [Fact]
    public void Should_Return_Notification_When_FileName_Is_Null_Or_Empty()
    {
        var file = new MemoryStream(new byte[1_000]);

        // Act
        var videoFile1 = new VideoFile(file, null);
        var videoFile2 = new VideoFile(file, "");

        // Assert
        videoFile1.IsValid.Should().BeFalse();
        videoFile1.Notifications.Should().ContainSingle()
            .Which.Message.Should().Be("File name cannot be null or empty");

        videoFile2.IsValid.Should().BeFalse();
        videoFile2.Notifications.Should().ContainSingle()
            .Which.Message.Should().Be("File name cannot be null or empty");
    }
}
