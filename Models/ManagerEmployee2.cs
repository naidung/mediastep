using System;
using System.Collections.Generic;

namespace TestProject.Models;

public partial class ManagerEmployee2
{
    public long AutoId { get; set; }

    public int ManagerId { get; set; }

    public long EmpId { get; set; }

    public virtual Employee Emp { get; set; } = null!;

    public virtual Manager Manager { get; set; } = null!;
}
