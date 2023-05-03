﻿using System;
using System.Collections.Generic;

namespace MSSQL_Core_App.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public int? StudentGradeEnrollmentId { get; set; }

    public virtual ICollection<CommentTag> CommentTags { get; set; } = new List<CommentTag>();

    public virtual StudentGrade? StudentGradeEnrollment { get; set; }
}
