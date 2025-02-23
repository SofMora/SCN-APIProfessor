using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace APIProfessor.Models;

public partial class CommentConsult
{
    public int Id { get; set; }

    public int IdConsult { get; set; }

    public string DescriptionComment { get; set; } = null!;

    public int Author { get; set; }

    public DateTime DateComment { get; set; }

    [JsonIgnore]
    public virtual Consult? IdConsultNavigation { get; set; }
}
