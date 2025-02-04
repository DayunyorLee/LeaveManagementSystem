﻿using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagementSystem.Web.Data
{
    public class LeaveType
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(150)")]
        public string Name { get; set; }
        public int NUmberOfDays { get; set; } 

    }
}
