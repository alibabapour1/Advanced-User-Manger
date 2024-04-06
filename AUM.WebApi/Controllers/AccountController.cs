using AUM.WebApi.Models.Dto;
using AUM.WebApi.Sevices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Win32;
using NuGet.Common;
using System.Security.Cryptography.Pkcs;
using System.Security.Policy;

namespace AUM.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly EmailService _emailService;

        public AccountController(IUserServices userServices, EmailService emailService)
        {
            _userServices = userServices;
            _emailService = emailService;
        }
        [HttpPost("register")]
        public  async Task<IActionResult> Register(RegisterDto register)
        {
             var res = await _userServices.Register(register);
             if (res) 
            {
                var token = await _userServices.GenereateEmailConfirmationCode(register.Email);
                string url =  Url.Action("ConfirmEmail", "Account", new { Username = register.Email, Token = token }, protocol: Request.Scheme) ;
                string body = $"To Confirm Your EmailAddress Please Click The Link ! <a href={url} > Confirm </a> ";
                _emailService.SendEmailAsync(register.Email, "Email Confirmation",body);
                return Ok("A Verification Email Has Been Sent To Your EmailAddress , Please Make Sure To Verify Your EmailAddress !"); 
            }
             else {  return BadRequest(405); }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string Username , string Password) 
        {
            var res =await _userServices.Login(Username, Password);
            if (res) 
            {
                var JwtToken =await _userServices.GenerateJwtToken(Username);

                HttpContext.Response.Headers.Add("Authorization", "Bearer " + JwtToken);
                

                return Ok(res);
                
            }
            else { return BadRequest(404); }
        }


        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string username,string token) 
        {
            var res = await _userServices.ConfirmUserEmail(username, token);
            if(res) { return Ok("Confirmed !");}
            else { return BadRequest("Unsuccessful Email Confirmation !"); }
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> Forgotpassword(string username)
        {
            var res =await _userServices.ForgotPassword(username);
            if (res == null) { return BadRequest("Invalid Username !"); }
            string url = Url.Action("ResetPassword", "Account", new { Username = username, Token = res }, protocol: Request.Scheme);
            string body = $"To Reset Your Password Please click the Link Below ! <br  /> <a href={url} > Reset-Password </a> ";
            _emailService.SendEmailAsync(username,"Reset Your Password",body);
            return Ok("Reset Password Link Hast Been Sent To Your EmailAddress .");
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto reset)
        {
            if(reset==null) { return BadRequest(); }
            var  res = await _userServices.resetPassword(reset);
            if (res) { return Ok("Your Password Has been Reseted Sucessfuly !"); }
            else { return BadRequest("Error"); }
        }

        [Authorize]
        [HttpGet("TestAuth")]
        public IActionResult TestAuth() 
        {
            return Ok("Authorized");
        }
    }
}
