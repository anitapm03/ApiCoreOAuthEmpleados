using ApiCoreOAuthEmpleados.Helpers;
using ApiCoreOAuthEmpleados.Models;
using ApiCoreOAuthEmpleados.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ApiCoreOAuthEmpleados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private RepositoryEmpleados repo;
        //cuando generemos el token, debemos integrar
        //dentro de dicho token, issuer, audience...
        //para que lo valide cuando nos lo envien
        private HelperActionServicesOAuth helper;

        public AuthController(RepositoryEmpleados repo, 
            HelperActionServicesOAuth helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        //necesitamos un metodo post para validar el
        //user y que recibira LoginModel
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Login(LoginModel model)
        {
            //buscamos al empleado en nuestro repo
            Empleado empleado = 
                await this.repo.LoginEmpleadoAsync
                (model.UserName, int.Parse(model.Password));
            if(empleado == null)
            {
                return Unauthorized();
            }
            else
            {
                //debemos crear unas credenciales para incluuirlas
                //dentro del token y que estaran compuestas por el 
                //secret key cifrado y el tipo de cifrado que 
                //deseemos incluir en eñ token
                SigningCredentials credentials =
                    new SigningCredentials(
                        this.helper.GetKeyToken(),
                        SecurityAlgorithms.HmacSha256);
                //el token se genera con una clase y debemos indicar
                //los elementos que almacenará dentro de dicho token
                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: this.helper.Issuer,
                    audience: this.helper.Audience,
                    signingCredentials: credentials,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    notBefore: DateTime.UtcNow
                    );
                //por ultimo devolvemos una respuesta afirmativa 
                //con un objeto anonimo en formato JSON
                return Ok(
                    new
                    {
                        response = 
                        new JwtSecurityTokenHandler()
                        .WriteToken( token )
                    });
            }
        }
    }
}
