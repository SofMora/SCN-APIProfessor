using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace APIProfessor.Models;

public partial class Comment
{
    public int Id { get; set; }

    public int IdNews { get; set; }

    public string Description { get; set; } = null!;

    public int Author { get; set; }

    public DateTime CommentDate { get; set; }

    [JsonIgnore]
    public virtual News? IdNewsNavigation { get; set; }
}
