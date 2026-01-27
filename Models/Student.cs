using System;
using System.Collections.Generic;

namespace DatabasFirst_Collegedb.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Image { get; set; }

    public int? Age { get; set; }

    public string Batch { get; set; } = null!;

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual User User { get; set; } = null!;
}
