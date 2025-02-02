using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain
{
    public class User : IdentityUser<int>
    {
        [Required]
        public string Password { get; set; }//nop
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Role {  get; set; }
    }

    public class Role : IdentityRole<int>
    {
        public Role() { }
        public Role(string RoleName) : base (RoleName) { }
    }
}