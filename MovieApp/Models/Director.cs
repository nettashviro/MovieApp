using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Models
{
    public class Director
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }
        public Gender Gender { get; set; }

        [Display(Name = "Birth date")]
        public DateTime Birthdate { get; set; }

        public string OriginCountry { get; set; }

        public string ImageUrl { get; set; }
    }
}

public enum Gender
{
    Male,
    Female,
    Other
}
