using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace APIProfessor.Models;

public partial class Appointment
{
    public int Id { get; set; }

    public int IdProfessor { get; set; }

    public int IdStudent { get; set; }

    public int IdSchedule { get; set; }

    public bool StatusAppointment { get; set; }

    public bool TypeAppointment { get; set; }

    public DateTime DateAppointment { get; set; }

    public string DescriptionAppointment { get; set; } = null!;

    public string SubjectAppointment { get; set; } = null!;

    public string CommentStatus { get; set; } = null!;

    [JsonIgnore]
    public virtual ScheduleProfessor? IdScheduleNavigation { get; set; }
}
