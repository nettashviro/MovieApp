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
        [Display(Name = "מייל")]
        public string Email { get; set; }

        [Required]
        [DisplayName("שם משתמש")]
        public string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "סיסמה")]
        public string Password { get; set; }

        public UserType Type { get; set; }
        public ICollection<Movie> MovieWatched { get; set; }

        public string ProfileImageUrl { get; set; }
    }
}
