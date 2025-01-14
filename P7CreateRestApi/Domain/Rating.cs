using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Controllers.Domain
{
    public class Rating
    {
        // DONE: Map columns in data table RATING with corresponding fields
        [Key]
        public int Id { get; set; }
        public string MoodyRating { get; set; }
        public string SandPRating {  get; set; }
        public string FitchRating {  get; set; }
        public byte? OrderNumber { get; set; }

    }
}