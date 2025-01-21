using System;
using System.Collections.Generic;

namespace EntityLayer;

public partial class History
{
    public int HistoryId { get; set; }

    public int Userid { get; set; }

    public int Productid { get; set; }

    public int Price { get; set; }

    public DateTime BoughtDate { get; set; }
}
