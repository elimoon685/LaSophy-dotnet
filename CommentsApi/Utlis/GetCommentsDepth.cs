using CommentsApi.Models;

namespace CommentsApi.Utlis
{
    public class GetCommentsDepth
    {


        public static int GetDepth(BookComments comment, List<BookComments> allComments)
        {
            int depth = 0;
            var current = comment;
            while (current.ParentCommentId != null)
            {
                current = allComments.FirstOrDefault(c => c.CommentsId == current.ParentCommentId);

                    if (current == null)
                    break;
                depth++;
            }
            return depth;
        }
    }
}
