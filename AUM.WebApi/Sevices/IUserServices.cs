using AUM.WebApi.Entities;
using AUM.WebApi.Models.Dto;
using Azure.Identity;

namespace AUM.WebApi.Sevices
{
    public interface IUserServices
    {
        public Task<bool> Login (string username, string password);

        public Task<bool> Register(RegisterDto register);

        public Task<string> GenereateEmailConfirmationCode(string username);

        public Task<bool> ConfirmUserEmail(string username,string token);

        public Task<string> ForgotPassword(string username);

        public Task<bool> resetPassword(ResetPasswordDto reset);

        public Task<string> GenerateJwtToken(string username);

        

    }
}
