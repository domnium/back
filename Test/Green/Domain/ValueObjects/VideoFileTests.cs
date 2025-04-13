using Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Test.Green.Domain.ValueObjects;

public class VideoFileTests
{
    [Fact]
    public void Should_Create_Valid_VideoFile()
    {
        // Arrange
        var content = new MemoryStream(new byte[1_000_000]); // 1MB
        var fileName = "video.mp4";

        // Act
        var videoFile = new VideoFile(content, fileName);

        // Assert
        videoFile.IsValid.Should().BeTrue();
        videoFile.Notifications.Should().BeEmpty();
        videoFile.File.Should().BeSameAs(content);
        videoFile.FileName.Should().Be(fileName);
        videoFile.FileSize.Should().Be(content.Length);
    }
}
