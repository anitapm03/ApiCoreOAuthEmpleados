using ApiCoreOAuthEmpleados.Data;
using ApiCoreOAuthEmpleados.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCoreOAuthEmpleados.Repositories
{
    public class RepositoryEmpleados
    {
        private HospitalContext context;

        public RepositoryEmpleados(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<Empleado>> GetEmpleadosAsync()
        {
            return await this.context.Empleados.ToListAsync();
        }

        public async Task<Empleado> FindHospitalAsync(int idhospital)
        {
            return await
                this.context.Empleados
                .FirstOrDefaultAsync(x => x.IdEmpleado == idhospital);
        }

        public async Task<Empleado> 
            LoginEmpleadoAsync(string apellido, int idEmpleado)
        {
            return await this.context.Empleados
                .Where(x => x.Apellido == apellido
                && x.IdEmpleado == idEmpleado)
                .FirstOrDefaultAsync();
        }


        /*public async Task<List<Empleado>>
            GetEmpleadosOficioAsync(string oficio)
        {
            return await this.context.Empleados
                .Where(z => z.Oficio == oficio).ToListAsync();
        }

        public async Task<List<string>> GetOficiosAsync()
        {
            var consulta = (from datos in this.context.Empleados
                            select datos.Oficio).Distinct();
            return await consulta.ToListAsync();
        }

        public async Task<List<Empleado>>
            GetEmpleadosSalarioAsync(int salario, int dept)
        {
            return await this.context.Empleados
                .Where(x => x.Departamento == dept
                && x.Salario >= salario).ToListAsync();
        }
        */
    }
}
