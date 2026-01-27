using System;
using System.Collections.Generic;

namespace DatabasFirst_Collegedb.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public int FacultyId { get; set; }

    public string CourseName { get; set; } = null!;

    public int Duration { get; set; }

    public int? AvailableSeats { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual Faculty Faculty { get; set; } = null!;
}
