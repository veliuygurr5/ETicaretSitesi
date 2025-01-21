using System;
using System.Collections.Generic;

namespace EntityLayer;

public partial class Seller
{
    public int SellerId { get; set; }

    public string Title { get; set; } = null!;

    public string TaxNo { get; set; } = null!;

    public string Phone { get; set; } = null!;
    public string Password { get; set; } = null!;

    public string Description { get; set; } = null!;
}
