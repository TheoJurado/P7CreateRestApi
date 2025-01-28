using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Controllers.Domain
{
    public class Rating
    {
        // DONE: Map columns in data table RATING with corresponding fields
        [Key]
        public int Id { get; set; }
        [Required]
        public string MoodyRating { get; set; }
        [Required]
        public string SandPRating {  get; set; }
        [Required]
        public string FitchRating {  get; set; }
        [Required]
        public byte? OrderNumber { get; set; }

    }
}