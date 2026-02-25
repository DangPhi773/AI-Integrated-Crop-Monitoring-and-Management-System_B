using System;
using System.Collections.Generic;

namespace CMMS.DAL.Entities;

public partial class Report
{
    public Guid ReportId { get; set; }

    public Guid? WorkerId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? SubmitDate { get; set; }

    public virtual User? Worker { get; set; }
}
