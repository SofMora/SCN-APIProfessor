using System;
using System.Collections.Generic;

namespace APIProfessor.Models;

public partial class Course
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Cycle { get; set; }

    public bool StatusCourse { get; set; }

    public string Description { get; set; } = null!;

    public int IdProfessor { get; set; }

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual Professor IdProfessorNavigation { get; set; } = null!;
}
