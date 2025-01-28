using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain
{
    public class User : IdentityUser<int>
    {
        [Key]
        public override int Id {  get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Role {  get; set; }
    }
}