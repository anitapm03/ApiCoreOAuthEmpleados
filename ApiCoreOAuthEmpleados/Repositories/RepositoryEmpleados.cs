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

        public async Task<Empleado> FindEmpleadoAsync(int idhospital)
        {
            return await
                this.context.Empleados
                .FirstOrDefaultAsync(x => x.IdEmpleado == idhospital);
        }

        public async Task<List<Empleado>>
            GetCompisDeptAsync(int idDept)
        {
            return await this.context.Empleados
                .Where(z => z.Departamento == idDept)
                .ToListAsync();
        }

        public async Task<Empleado> 
            LoginEmpleadoAsync(string apellido, int idEmpleado)
        {
            return await this.context.Empleados
                .Where(x => x.Apellido == apellido
                && x.IdEmpleado == idEmpleado)
                .FirstOrDefaultAsync();
        }

        public async Task<List<string>> GetOficiosAsync()
        {
            var consulta = (from datos in this.context.Empleados 
                            select datos.Oficio).Distinct();
            return await consulta.ToListAsync();
        }

        public async Task<List<Empleado>>
            GetEmpleadosOficiosAsync(List<string> oficios)
        {
            var consulta = from datos in this.context.Empleados
                           where oficios.Contains(datos.Oficio)
                           select datos;
            return await consulta.ToListAsync();
        }

        public async Task IncrementarSalarioOficioAsync
            (int incremento, List<string> oficios)
        {
            List<Empleado> empleados = await
                this.GetEmpleadosOficiosAsync(oficios);
            foreach (Empleado emp in empleados)
            {
                emp.Salario += incremento;
            }

            await this.context.SaveChangesAsync();
        }
    }
}
