using System;
using System.Collections.Generic;

namespace APIProfessor.Models;

public partial class ErrorLog
{
    public int Id { get; set; }

    public string? ProcedureName { get; set; }

    public string? ErrorMessage { get; set; }

    public DateTime? ErrorDate { get; set; }
}
