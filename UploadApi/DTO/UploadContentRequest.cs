namespace UploadApi.DTO
{
    public class UploadContentRequest
    {

        public string BookName {  get; set; }

        public string Author { get; set; }

        public string Year { get; set; }

        public IFormFile PDF { get; set; }

        public IFormFile  IMG { get; set; }
    }
}
