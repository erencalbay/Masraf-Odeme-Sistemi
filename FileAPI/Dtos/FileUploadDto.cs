using System.Text.Json.Serialization;

namespace FileAPI.Dtos
{
    // Data transfer objectleri
    public class FileUploadDto : FileUploadRequest
    {
        public int UserNumber { get; set; }
    }
    public class FileUploadRequest
    {
        public IFormFileCollection Receipts { get; set; }
        public string Description { get; set; }
    }
}
