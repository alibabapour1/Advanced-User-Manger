using AUM.WebApi.Entities;
using AUM.WebApi.Models.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AUM.WebApi.Sevices
{
    public class UserServices : IUserServices

    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;
        public UserServices(SignInManager<User> signInManager, UserManager<User> userManager,  IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }
        public async Task<bool> Login(string username, string password)
        {
            await _signInManager.SignOutAsync();
            var User = await  _userManager.FindByNameAsync(username);
            if (User == null) {return false;}
            var res  = await _signInManager.CheckPasswordSignInAsync(User, password, false);
            return res.Succeeded;
        }

        public async Task<bool> Register(RegisterDto register)
        {
            User user = new User
            { 
                FirstName = register.FirstName,
                LastName = register.LastName,
                Email = register.Email,
                UserName = register.Email
             };

            var res = _userManager.CreateAsync(user).Result;
            await _userManager.AddPasswordAsync(user,register.Password);
           
            return res.Succeeded;
        }

        public async Task<string> GenereateEmailConfirmationCode(string username) 
        {
            var user = await _userManager.FindByNameAsync(username);
            string code = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;
            return code;

        }

        public async Task<bool> ConfirmUserEmail(string username, string token)
        {
            var user = await _userManager.FindByNameAsync(username); 
            var res =await _userManager.ConfirmEmailAsync(user, token);
            if (res.Succeeded)
            {
                user.EmailConfirmed = true;
            }

            return res.Succeeded;

        }


        public async Task<string> ForgotPassword(string username) 
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user==null) { return null; }
            var res = await _userManager.GeneratePasswordResetTokenAsync(user);
            return res;

        }

        public async Task<bool> resetPassword(ResetPasswordDto reset)
        {
            
            var user = await _userManager.FindByNameAsync(reset.UserName);
            if (user==null) { return false;}
            var res = await _userManager.ResetPasswordAsync(user, reset.Token, reset.ConfirmPassword);
            return res.Succeeded;
        }

        public async  Task<string> GenerateJwtToken(string username)
        {
            var claims = new[]
            {
                new Claim(ClaimValueTypes.String, username),
                
            }; 


            var JwtToken = new JwtSecurityToken
                (
                    issuer:_config.GetSection("JwtConf:Issuer").Value,
                    audience: _config.GetSection("JwtConf:Audience").Value,
                    claims:claims,
                    expires:DateTime.Now.AddHours(2),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JwtConf:Key").Value)),
                     SecurityAlgorithms.HmacSha256)
                ) ;

            var tokenString = new JwtSecurityTokenHandler().WriteToken(JwtToken);

            return tokenString;

        }
    }
}
