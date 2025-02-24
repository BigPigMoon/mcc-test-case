using System.Collections.Generic;
using System.Linq;

public class BlogService
{
    public static IDictionary<string, int> NumberOfCommentsPerUser(MyDbContext context)
    {
        return context.BlogComments
            .GroupBy(item => item.UserName)
            .ToDictionary(group => group.Key, group => group.Count());
    }

    public static List<Dictionary<string, List<string>>> PostsOrderedByLastCommentDate(MyDbContext context)
    {
        return context.BlogPosts
            .Select(post => new { Post = post, LastComment = post.Comments.OrderByDescending(item => item.CreatedDate).FirstOrDefault() })
            .Where(post => post.LastComment != null)
            .OrderByDescending(post => post.LastComment.CreatedDate)
            .Select(post =>
                new Dictionary<string, List<string>> {
                    {
                        post.Post.Title,
                        new List<string>() {
                            post.LastComment.CreatedDate.ToString("yyyy-MM-dd"),
                            post.LastComment.Text
                        }
                    }
                }
            )
            .ToList();
    }

    public static IDictionary<string, int> NumberOfLastCommentsLeftByUser(MyDbContext context)
    {
        return context.BlogPosts
            .Select(post => post.Comments.OrderByDescending(item => item.CreatedDate).FirstOrDefault())
            .Where(lastComment => lastComment != null)
            .GroupBy(post => post.UserName)
            .ToDictionary(g => g.Key, g => g.Count());
    }
}