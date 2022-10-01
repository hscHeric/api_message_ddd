using Entities.Entities;
using Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using WebAPIs.Models;
using WebAPIs.Token;

namespace WebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/CreateToken")]
        public async Task<IActionResult> CreateToken([FromBody]Login login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
            {
                return Unauthorized();
            }
            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var userCurrent = await _userManager.FindByEmailAsync(login.Email);
                var idUser = userCurrent.Id;

                var token = new TokenJWTBuilder().AddSecurityKey(JwtSecurityKey.Create("Secret_Key-12345678"))
                                                 .AddSubject("Heric - Dev")
                                                 .AddIssuer("API.DDD.TESTE")
                                                 .AddAudience("API.DDD.TESTE")
                                                 .AddClaim("idUser", idUser)
                                                 .AddExpiry(60)
                                                 .Builder();
                return Ok(token.value);
            }
            return Unauthorized();
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/AddUser")]
        public async Task<IActionResult> AddUser([FromBody]Login login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
            {
                return Unauthorized();
            }

            var user = new ApplicationUser
            {
                UserName = login.Email,
                Email = login.Email,
                CPF = login.CPF,
                Type = UserType.Common
            };

            var result = await _userManager.CreateAsync(user, login.Password);
            if (result.Errors.Any())
            {
                return Ok(result.Errors);
            }

            //Geração e confirmação de um codigo para email.
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result2 = await _userManager.ConfirmEmailAsync(user, code);

            //if (resultado2.Succeeded)
            //{
            //    return Ok("Usuário Adicionado com Sucesso");
            //}
            //return Ok("Erro ao confirmar usuários");

            return result2.Succeeded ? Ok("Usuário Adicionado com Sucesso") : Ok("Erro ao Adicionar Usuário");
        }
            
    }
}
