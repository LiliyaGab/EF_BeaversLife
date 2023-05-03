﻿using System;
using System.Collections.Generic;

namespace MSSQL_Core_App.Models;

public partial class Person
{
    public int PersonId { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public DateTime? HireDate { get; set; }

    public DateTime? EnrollmentDate { get; set; }

    public string Discriminator { get; set; } = null!;

    public virtual OfficeAssignment? OfficeAssignment { get; set; }

    public virtual ICollection<StudentGrade> StudentGrades { get; set; } = new List<StudentGrade>();

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<Pet> PetPets { get; set; } = new List<Pet>();
}
