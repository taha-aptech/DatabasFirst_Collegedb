using System;
using System.Collections.Generic;

namespace DatabasFirst_Collegedb.Models;

public partial class Faculty
{
    public int FacultyId { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Image { get; set; }

    public int? Experience { get; set; }

    public string Department { get; set; } = null!;

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual User User { get; set; } = null!;
}
