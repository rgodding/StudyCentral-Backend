using System.Diagnostics.CodeAnalysis;

namespace StudyCentral.API.Utilities;

[ExcludeFromCodeCoverage]
public static class FileExtensionMappings
{
    public static string GetContentTypeFromExtension(
        string fileExtension)
    {
        return GetContentTypeInternal(
            fileExtension.ToLowerInvariant());
    }

    public static string GetContentTypeFromPath(
        string filePath)
    {
        return GetContentTypeInternal(
            Path.GetExtension(filePath).ToLowerInvariant());
    }

    private static string GetContentTypeInternal(
        string extension)
    {
        return extension switch
        {
            ".pdf" => "application/pdf",

            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",

            ".xls" => "application/vnd.ms-excel",
            ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",

            ".ppt" => "application/vnd.ms-powerpoint",
            ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",

            ".odt" => "application/vnd.oasis.opendocument.text",
            ".ods" => "application/vnd.oasis.opendocument.spreadsheet",
            ".odp" => "application/vnd.oasis.opendocument.presentation",

            ".txt" => "text/plain",

            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",

            _ => "application/octet-stream"
        };
    }
}