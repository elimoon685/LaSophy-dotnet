namespace UploadApi.Exceptions.CustomException
{
    public class BookMetaDataSaveFailedException : Exception
    {
        public BookMetaDataSaveFailedException(string message) : base(message) { }
    }
    public class FailedSaveBookException : Exception
    {
        public FailedSaveBookException(string message) : base(message) { }
    }
}
