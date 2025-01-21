using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer;

public partial class User
{
    public int UserId { get; set; }

    [Required]
    public string UserName { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Surname { get; set; } = null!;

    [Required]
    public string Mail { get; set; } = null!;

    public string? Adress { get; set; }

    public DateTime UserRegisterDate { get; set; }

    public DateTime UserOnlineDate { get; set; }
}
