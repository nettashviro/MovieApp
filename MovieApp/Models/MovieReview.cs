using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Models
{
    public class MovieReview
    {
        [Key]
        public int Id { get; set; }

        [Range(0, 10)]
        public double Rank { get; set; }

        [MaxLength(30)]
        public string Title { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }

        [DefaultValue(false)]
        public bool IsViolent { get; set; }

        [DefaultValue(false)]
        public bool IsBlackAndWhite { get; set; }

        [Range(1, 99)]
        public int RecommendedAge { get; set; }

        [DefaultValue(false)]
        public bool IsHavingSequel { get; set; }

        [ForeignKey("movie_id")]
        public Movie Movie { get; set; }
    }
}
