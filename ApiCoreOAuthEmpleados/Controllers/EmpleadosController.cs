using ApiCoreOAuthEmpleados.Models;
using ApiCoreOAuthEmpleados.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            return await this.repo.FindHospitalAsync(id);
        }


        /*[HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<string>>>
            GetOficios()
        {
            return await this.repo.GetOficiosAsync();
        }

        [HttpGet]
        [Route("[action]/{oficio}")]
        public async Task<ActionResult<List<Empleado>>>
            EmpleadosOficio(string oficio)
        {
            return await this.repo.GetEmpleadosOficioAsync(oficio);
        }

        //los parametros deben tener el mismo nombre
        //deben estar en el mismo orden que recibe el metodo
        [HttpGet]
        [Route("[action]/{salario}/{dept}")]
        public async Task<ActionResult<List<Empleado>>>
            EmpleadosSalario(int salario, int dept)
        {
            return await this.repo.GetEmpleadosSalarioAsync
                (salario, dept);
        }*/
    }
}
