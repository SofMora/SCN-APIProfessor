using System;
using System.Collections.Generic;

namespace APIProfessor.Models;

public partial class Professor
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Description { get; set; }

    public byte[]? Photo { get; set; }

    public string? SocialLink { get; set; }

    public bool? StatusProfessor { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
}
