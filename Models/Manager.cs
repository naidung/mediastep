using System;
using System.Collections.Generic;

namespace TestProject.Models;

public partial class Manager
{
    public int ManagerId { get; set; }

    public string FullName { get; set; } = null!;

    public virtual ICollection<ManagerEmployee2> ManagerEmployee2s { get; set; } = new List<ManagerEmployee2>();
}
