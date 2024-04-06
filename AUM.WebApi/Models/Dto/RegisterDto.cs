using System.ComponentModel.DataAnnotations;

namespace AUM.WebApi.Models.Dto
{
    public class RegisterDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(ConfirmPassword))]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set;}
        
    }
}
