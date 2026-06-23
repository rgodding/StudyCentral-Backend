using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Moq;
using StudyCentral.API.Services;
using StudyCentral.Test.Factories;
using Xunit;

namespace StudyCentral.Test.ServiceTests;

public class BlobServiceTests
{
    [Fact]
    public async Task UploadFile_WhenFileIsValid_ReturnsBlobUploadResult()
    {
        // Arrange
        var formFile = TestFileFactory.CreateTextFile("test notes.txt");
        var blobClientMock = new Mock<BlobClient>();
        var containerClientMock = new Mock<BlobContainerClient>();
        var response = Response.FromValue(
            BlobsModelFactory.BlobContentInfo(
                eTag: new ETag("\"etag\""),
                lastModified: DateTimeOffset.UtcNow,
                contentHash: null,
                versionId: null,
                encryptionKeySha256: null,
                encryptionScope: null,
                blobSequenceNumber: 0),
            Mock.Of<Response>());

        blobClientMock
            .Setup(client => client.UploadAsync(
                It.IsAny<Stream>(),
                It.IsAny<BlobUploadOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        containerClientMock
            .Setup(client => client.GetBlobClient(It.IsAny<string>()))
            .Returns(blobClientMock.Object);

        var service = new BlobService(containerClientMock.Object);

        // Act
        var result = await service.UploadFile("test notes.txt", formFile);

        // Assert
        Assert.Equal("test notes.txt", result.FileName);
        Assert.Equal("text/plain", result.ContentType);
        Assert.Contains("test_notes.txt", result.BlobName);
    }

    [Fact]
    public async Task UploadFile_WhenFileIsEmpty_ThrowsArgumentException()
    {
        // Arrange
        var service = new BlobService(Mock.Of<BlobContainerClient>());

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.UploadFile("empty.txt", null!));
    }
}
