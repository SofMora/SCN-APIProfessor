using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace APIProfessor.Models;

public partial class News
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int Author { get; set; }

    public string TextNews { get; set; } = null!;

    public DateTime DateNews { get; set; }

    public byte[]? Images { get; set; }

    public int TypeNews { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [JsonIgnore]
    public virtual TypeNews? TypeNewsNavigation { get; set; }
}
