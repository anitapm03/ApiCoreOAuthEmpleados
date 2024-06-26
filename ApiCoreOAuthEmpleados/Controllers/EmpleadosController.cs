﻿using ApiCoreOAuthEmpleados.Models;
using ApiCoreOAuthEmpleados.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ApiCoreOAuthEmpleados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {

        private RepositoryEmpleados repo;

        public EmpleadosController(RepositoryEmpleados repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Empleado>>> GetEmpleados()
        {
            return await this.repo.GetEmpleadosAsync();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Empleado>> FindEmpleado(int id)
        {
            return await this.repo.FindEmpleadoAsync(id);
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<Empleado>>
            PerfilEmpleado()
        {
            //internamente cuando recibimos el token
            //el usuario es validado y almacena datos como
            //httpcontext.user.identity.isauthenticated
            //como hemos incluido la key de los claims,
            //automaticamente tbn tenemos dichos claims
            //como en las apps MVC
            Claim claim = HttpContext.User
                .FindFirst(x => x.Type == "UserData");
            //recuperamos el json del empleado
            string jsonEmpleado = claim.Value;
            Empleado emp =
                JsonConvert.DeserializeObject<Empleado>(jsonEmpleado);
            return emp;

        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<Empleado>>>
            GetCompisEmpleado()
        {
            string jsonEmp = HttpContext.User
                .FindFirst(x => x.Type == "UserData").Value;
            Empleado emp =  
                JsonConvert.DeserializeObject<Empleado>(jsonEmp);
            List<Empleado> compis = await
                this.repo.GetCompisDeptAsync(emp.Departamento);
            return compis;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<string>>>
            Oficios()
        {
            return await this.repo.GetOficiosAsync();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<Empleado>>>
            EmpleadosOficio([FromQuery] List<string> oficio)
        {
            return await this.repo.GetEmpleadosOficiosAsync(oficio);
        }

        [HttpPut]
        [Route("[action]/{incremento}")]
        public async Task<ActionResult>
            SubirSalarioOficio(
            int incremento, 
            [FromQuery] List<string> oficio)
        {
            await this.repo.
                IncrementarSalarioOficioAsync(incremento, oficio);
            return Ok();
        }
    }
}
