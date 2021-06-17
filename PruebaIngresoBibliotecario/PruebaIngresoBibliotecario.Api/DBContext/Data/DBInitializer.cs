using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PruebaIngresoBibliotecario.Entities;
using System;
using System.Linq;

namespace PruebaIngresoBibliotecario.DBContext.Data
{
    public static class DBInitializer
    {
        public static void Init(IServiceProvider serviceProvider)
        {
            try
            {
                using (var _context = new PersistenceContext(serviceProvider.GetRequiredService<DbContextOptions<PersistenceContext>>()))
                {
                    if (_context.Prestamos.Any())
                        return;

                    _context.Prestamos.AddRange(
                        new Entities.PrestamosE { isbn = new Guid(), identificacionUsuario = "goku123", tipoUsu = 1 , Id = new Guid("bec109ba-48bd-4393-bb4a-b3ca8c7aebad"), fechaMaximaDevolucion = new DateTime()},
                        new Entities.PrestamosE { isbn = new Guid(), identificacionUsuario = "vegeta123", tipoUsu = 2, Id = new Guid(), fechaMaximaDevolucion = new DateTime() },
                        new Entities.PrestamosE { isbn = new Guid(), identificacionUsuario = "sayayin123", tipoUsu = 3, Id = new Guid(), fechaMaximaDevolucion = new DateTime() }
                        );

                    _context.SaveChanges();

                }

            }
            catch(Exception ex)
            {
                return;
            }
        }

    }
}
