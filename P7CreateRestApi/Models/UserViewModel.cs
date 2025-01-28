using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Models
{
    public class UserViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
    }
}
