﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace FrameworkLib_Common.Model
{
    //[Table("Animals")]
    public class Animal
    {
        private Food _food = null!;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(128)] public string? Name { get; set; }
        public int Age { get; set; }

        public virtual List<Club>? Clubs { get; set; }
        public virtual ICollection<Grade>? Grades { get; set; }
        public virtual Job Job { get; set; } = null!;
        public int? JobId { get; set; }

        public virtual Food Food { get; set; } = null!;
        //public virtual Food Food
        //{
        //    get => _food;
        //    set => _food = value;
        //}

        // Translates to string in db so Include is not needed.
        public string IpAddress { get; set; } = null!;

        //public JsonDocument? Passport { get; set; }

        public override string ToString()
        {
            return $"Animal : Id = {Id} Name = {Name} IpAddress = {IpAddress}";
        }
    }
}