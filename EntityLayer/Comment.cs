using System;
using System.Collections.Generic;

namespace EntityLayer;

public partial class Comment
{
    public int CommentId { get; set; }

    public string Text { get; set; } = null!;

    public int Userid { get; set; }

    public int Productid { get; set; }

    public int Sellerid { get; set; }

    public DateTime Date { get; set; }
}
