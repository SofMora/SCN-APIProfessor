using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace APIProfessor.Models;

public partial class Consult
{
    public int Id { get; set; }

    public int IdCourse { get; set; }

    public string DescriptionConsult { get; set; } = null!;

    public bool TypeConsult { get; set; }

    public int Author { get; set; }

    public DateTime DateConsult { get; set; }

    public bool StatusConsult { get; set; }

    public virtual ICollection<CommentConsult> CommentConsults { get; set; } = new List<CommentConsult>();
}
