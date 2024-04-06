using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AUM.WebApi.Models.Dto
{
    public class ResetPasswordDto
    {
        
        public string UserName { get; set; }
        
        public string Token { get; set; }
        public string Password { get; set; }
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }


    }
}
