using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Models
{
    public class Official
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }
        
        public OfficialRole Role { get; set; }

        public OfficialGender Gender { get; set; }

        [Display(Name = "Birth date")]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }

        public string OriginCountry { get; set; }

        [NotMapped]
        [Display(Name = "Picture")]
        public IFormFile Image { get; set; }

        public string ImageUrl { get; set; }

        public enum OfficialGender
        {
            Male,
            Female,
            Other
        }

        public enum OfficialRole
        {
            Producer,
            Director,
            Writer,
            Photographer,
            Actor,
            Editor,
            Soundperson
        }

        public enum OfficialSpliceOptions
        {
            Role,
            Gender,
            OriginCountry
        }

        public enum OfficialAverageOfOptions
        {
            Age
        }

        public enum OfficialAverageByOptions
        {
            Role,
            OriginCountry
        }
    }
}
