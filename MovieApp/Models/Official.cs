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

        [Display(Name = "שם פרטי")]
        public string FirstName { get; set; }

        [Display(Name = "שם משפחה")]
        public string LastName { get; set; }

        [Display(Name = "תפקיד")]
        public OfficialRole Role { get; set; }

        [Display(Name = "מגדר")]
        public OfficialGender Gender { get; set; }

        [Display(Name = "תאריך לידה")]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }

        [Display(Name = "מדינת מוצא")]
        public string OriginCountry { get; set; }

        [NotMapped]
        [Display(Name = "תמונה")]
        public IFormFile Image { get; set; }

        public string ImageUrl { get; set; }

        [Display(Name = "סרטים שבהם משתתף")]
        public ICollection<OfficialOfMovie> OfficialOfMovies { get; set; }

        public enum OfficialGender
        {
            זכר,
            נקבה,
            אחר
        }

        public enum OfficialRole
        {
            מפיק,
            במאי,
            תסריטאי,
            צלם,
            שחקן,
            עורך,
            סאונדמן,
            זמר,
            פזמונאי
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
