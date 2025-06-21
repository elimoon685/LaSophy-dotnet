using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using SharedContract.HttpClient;
using UploadApi.DTO;
using UploadApi.DTO.ApiResponse;
using UploadApi.Exceptions.CustomException;

namespace UploadApi.Service.AzureBlobStorage
{
    public class BookUploadService : IBookUploadService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        //like creating the instance of 
        public BookUploadService(BlobServiceClient blobServiceClient, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _blobServiceClient = blobServiceClient;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }



        // upload successfully and return the url
        public async Task<UploadResponse> UploadPDFAsync(UploadContentRequest formData, string bookPdf, string bookImg)
        {


            //which container i get
            var containerClient = _blobServiceClient.GetBlobContainerClient(_configuration["AzureBlobStorage:Container"]);

            await containerClient.CreateIfNotExistsAsync();

            var file = formData.PDF;
            var img = formData.IMG;
            var blobName = $"{bookPdf}/{file.FileName}";
            var blobNameImg = $"{bookImg}/{img.FileName}";
            //create the blob instance
            var blobClient = containerClient.GetBlobClient(blobName);

            var blobClientImg = containerClient.GetBlobClient(blobNameImg);


            await using var pdfstream = file.OpenReadStream();
            
            await blobClient.UploadAsync(pdfstream, new BlobHttpHeaders { ContentType = file.ContentType });




            await using var imgstream = img.OpenReadStream();
            
            await blobClientImg.UploadAsync(imgstream, new BlobHttpHeaders { ContentType = img.ContentType });
            



            BookMetaData bookMeta = new BookMetaData
            {
                Title = formData.BookName,
                Author = formData.Author,
                Year = formData.Year,
                PdfPath = formData.PDF.FileName,
                ImgPath = formData.IMG.FileName
            };
            //Addbook metaData


            var client = _httpClientFactory.CreateClient("BookApiClient");
            var httpResponse = await client.PostAsJsonAsync("api/Comments/book", bookMeta);
            //Read and deserialize the actual content
            if (httpResponse.IsSuccessStatusCode)
            {
                UploadResponse uploadResponse = new UploadResponse
                {
                    PdfUrl = blobClient.Uri.ToString(),
                    ImgUrl = blobClientImg.Uri.ToString(),

                };

                return uploadResponse;
            }
            else
            {
                await blobClient.DeleteIfExistsAsync();

                await blobClientImg.DeleteIfExistsAsync();

                var result = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<string>>();

                if (result?.ErrorCode =="400")
                {
                    throw new BookMetaDataSaveFailedException("Book already exist!");
                }

                throw new FailedSaveBookException("Failed to save book!");
            }




        }

       
    }
}
