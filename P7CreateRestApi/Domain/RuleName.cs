using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Controllers
{
    public class RuleName
    {
        // DONE: Map columns in data table RULENAME with corresponding fields
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Json { get; set; }
        public string Template { get; set; }
        public string SqlStr {  get; set; }
        public string SqlPart {  get; set; }

    }
}