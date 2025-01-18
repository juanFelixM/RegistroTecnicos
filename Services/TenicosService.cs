using Microsoft.EntityFrameworkCore;
using RegistroTecnicos.DAL;
using RegistroTecnicos.Models;
using System.Linq.Expressions;

namespace RegistroTecnicos.Services
{
    public class TecnicosService (IDbContextFactory<Contexto> DbFactory)
    {
        public async Task<bool> ExisteNombreDuplicado(string nombre, int tecnicoId = 0)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Tecnicos.AnyAsync(t => t.Nombre == nombre && t.TecnicoId != tecnicoId);
        }

        public async Task<List<Tecnico>> ObtenerTodosLosTecnicos()
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Tecnicos.AsNoTracking().ToListAsync();
        }

        public async Task<Tecnico?> Buscar(int tecnicoId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Tecnicos.FirstOrDefaultAsync(t => t.TecnicoId == tecnicoId);
        }

        public async Task<bool> Guardar(Tecnico tecnico)
        {
            tecnico.Nombre = tecnico.Nombre.Trim();
            if (!await Existe(tecnico.TecnicoId))
            {
                return await Insertar(tecnico);
            }
            else
            {
                return await Modificar(tecnico);
            }
        }

        private async Task<bool> Existe(int tecnicoId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Tecnicos.AnyAsync(t => t.TecnicoId == tecnicoId);
        }

        private async Task<bool> Insertar(Tecnico tecnico)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.Tecnicos.Add(tecnico);
            return await contexto.SaveChangesAsync() > 0;
        }

        private async Task<bool> Modificar(Tecnico tecnico)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.Update(tecnico);
            return await contexto.SaveChangesAsync() > 0;
        }

        public async Task<bool> Eliminar(int tecnicoId)
        {
            try
            {
                await using var contexto = await DbFactory.CreateDbContextAsync();
                var tecnico = await contexto.Tecnicos.FindAsync(tecnicoId);

                if (tecnico == null)
                {
                    Console.WriteLine($"El técnico con ID {tecnicoId} no existe.");
                    return false;
                }

                contexto.Tecnicos.Remove(tecnico);

                var cambios = await contexto.SaveChangesAsync();
                Console.WriteLine($"Cambios aplicados: {cambios}");
                return cambios > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el técnico: {ex.Message}");
                return false;
            }
        }



        public async Task<List<Tecnico>> Listar(Expression<Func<Tecnico, bool>> criterio)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Tecnicos.Where(criterio).AsNoTracking().ToListAsync();
        }
    }
}
