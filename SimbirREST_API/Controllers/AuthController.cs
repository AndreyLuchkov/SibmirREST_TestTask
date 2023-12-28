using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SimbirREST_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace SimbirREST_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(JwtSecurityTokenHandler authenticationHandler, IConfiguration configuration) : ControllerBase
    {
        public JwtSecurityTokenHandler AuthenticationHandler { get; } = authenticationHandler;
        public IConfiguration Configuration { get; } = configuration;
        public IConfigurationSection JWTOptions { get => Configuration.GetSection("JWTOptions"); }

        /// <summary>
        /// Аутентификация пользователя
        /// </summary>
        /// <response code="200">Возвращает JWT токен</response>
        /// <response code="401">Если пользователь не администратор</response>
        [HttpPost]
        [Route("[action]")]
        public IResult Login(LoginModel model)
        {
            LoginModel adminCredentials = new();
            Configuration.GetSection("AdminCredentials").Bind(adminCredentials);

            if (model.Login == adminCredentials.Login && model.Password == adminCredentials.Password)
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTOptions["key"]!));
                var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

                var identity = new ClaimsIdentity(new GenericIdentity(model.Login), [new Claim("role", "admin")]);
                var token = AuthenticationHandler.CreateJwtSecurityToken(
                    subject: identity,
                    signingCredentials: signingCredentials,
                    audience: JWTOptions["Audience"],
                    issuer: JWTOptions["Issuer"],
                    expires: DateTime.UtcNow.AddHours(1));

                return Results.Ok(AuthenticationHandler.WriteToken(token));
            }
            else
            {
                return Results.Unauthorized();
            }
        }
    }
}
