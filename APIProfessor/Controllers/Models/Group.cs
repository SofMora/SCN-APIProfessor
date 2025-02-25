using System;
using System.Collections.Generic;

namespace APIProfessor.Models;

public partial class Group
{
    public int Id { get; set; }

    public int IdCourse { get; set; }

    public int IdProfessor { get; set; }

    public int NumberGroup { get; set; }

    public virtual Course IdCourseNavigation { get; set; } = null!;

    public virtual Professor IdProfessorNavigation { get; set; } = null!;
}
