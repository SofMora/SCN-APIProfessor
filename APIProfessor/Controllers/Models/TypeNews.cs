using System;
using System.Collections.Generic;

namespace APIProfessor.Models;

public partial class TypeNews
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<News> News { get; set; } = new List<News>();
}
