using System;
using System.Collections.Generic;

namespace TestProject.Models;

public partial class Employee
{
    public long EmpId { get; set; }

    public string FullName { get; set; } = null!;

    public DateTime Dob { get; set; }

    public virtual ICollection<ManagerEmployee2> ManagerEmployee2s { get; set; } = new List<ManagerEmployee2>();
}
