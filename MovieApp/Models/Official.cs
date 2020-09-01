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

        [Display(Name = "שם מלא")]
        public virtual string FullName { get { return FirstName + " " + LastName; } }

        [Display(Name = "גיל")]
        public virtual int Age { get
            {
                DateTime now = DateTime.Today;
                int age = now.Year - Birthdate.Year;
                if (Birthdate > now.AddYears(-age)) age--;
                return age;
            } }

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
            [Display(Name = "תפקיד")]
            Role,
            [Display(Name = "מגדר")]
            Gender,
            [Display(Name = "מדינת מוצא")]
            OriginCountry
        }

        public enum OfficialAverageOfOptions
        {
            [Display(Name = "גיל")]
            Age
        }

        public enum OfficialAverageByOptions
        {
            [Display(Name = "תפקיד")]
            Role,
            [Display(Name = "מדינת מוצא")]
            OriginCountry
        }
    }
}

