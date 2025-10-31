using Microsoft.AspNetCore.Identity;
using ReviewHall.Core.Entities;

public class Review
{
    public Guid ReviewId { get; set; }
    public Guid BookId { get; set; }
    public string UserId { get; set; } // ✅ foreign key to IdentityUser
    public string Content { get; set; }
    public int Rating { get; set; }
    public DateTime ReviewDate { get; set; }

    public Book Book { get; set; }
    public IdentityUser User { get; set; }

    public string Comment
    {
        get => Content;
        set => Content = value;
    }
}
