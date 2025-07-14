namespace CommentsApi.Exceptions.CustomExceptions
{
    public class BookMetaDataSavedFailedException:Exception
    {
        public BookMetaDataSavedFailedException(string message) : base(message) { }
    }

    public class SaveBookMetaDataException:Exception
    {
        public SaveBookMetaDataException(string message) : base(message) { }
    }
    public class BookNotExistException : Exception
    {
        public BookNotExistException(string message) : base(message) { }
    }
}
