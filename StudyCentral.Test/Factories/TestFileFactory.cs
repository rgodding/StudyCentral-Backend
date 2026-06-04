using Microsoft.AspNetCore.Http;
using System.Text;

namespace StudyCentral.Test.Factories
{
    public static class TestFileFactory
    {
        // Creates a fake image file for upload tests
        public static IFormFile CreateFormFile(string contentType = "image/png", string fileName = "test.png")
        {
            var content = "Fake image content for testing";

            var bytes = Encoding.UTF8.GetBytes(content);
            var stream = new MemoryStream(bytes);

            return new FormFile(stream, 0, bytes.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }

        // Creates a larger file (useful for edge testing)
        public static IFormFile CreateLargeFormFile(int sizeInKb = 100)
        {
            var content = new string('A', sizeInKb * 1024);
            var bytes = Encoding.UTF8.GetBytes(content);
            var stream = new MemoryStream(bytes);

            return new FormFile(stream, 0, bytes.Length, "file", "large_test.png")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/png"
            };
        }

        // Creates a text file (useful for negative tests)
        public static IFormFile CreateTextFile(string fileName = "test.txt")
        {
            var content = "This is a test file";
            var bytes = Encoding.UTF8.GetBytes(content);
            var stream = new MemoryStream(bytes);

            return new FormFile(stream, 0, bytes.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/plain"
            };
        }
    }
}