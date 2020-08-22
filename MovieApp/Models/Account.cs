using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Models
{
    public class Account
    {
        public enum UserType
        {
            Admin,
            Customer
        }
   
        [Key]
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [DisplayName("Username")]
        public string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public UserType Type { get; set; }
        public ICollection<Movie> MovieWatched { get; set; }

/*
        [NotMapped]
        [Display(Name = "Picture")]
        public IFormFile ProfileImage { get; set; }

        [DefaultValue("hey")]
        public string ProfileImageUrl { get; set; }*/
    }
}
