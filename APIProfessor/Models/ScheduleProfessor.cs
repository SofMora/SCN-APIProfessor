using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace APIProfessor.Models;

public partial class ScheduleProfessor
{
    public int Id { get; set; }

    public int IdProfessor { get; set; }

    public string Day { get; set; } = null!;

    public string Time { get; set; } = null!;

    [JsonIgnore]

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
