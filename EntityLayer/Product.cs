using System;
using System.Collections.Generic;

namespace EntityLayer;

public partial class Product
{
    public int ProductId { get; set; }

    public int Sellerid { get; set; }

    public int Categoryid { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Price { get; set; }

    public string? Image1 { get; set; }

    public string? Image2 { get; set; }

    public string? Image3 { get; set; }

    public string? Image4 { get; set; }

    public string? Image5 { get; set; }

    public DateTime AddDate { get; set; }
}
