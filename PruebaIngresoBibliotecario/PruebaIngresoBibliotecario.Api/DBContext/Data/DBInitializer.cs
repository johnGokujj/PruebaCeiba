using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PruebaIngresoBibliotecario.Entities;
using System;
using System.Linq;

namespace PruebaIngresoBibliotecario.DBContext.Data
{
    public static class DBInitializer
    {
        /// <summary>
        /// Inicia llenando la Bases de datos y llenandola con algunos datos
        /// Fue creado con el fin de realizar las pruebas iniciales por medio de Postman
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void Init(IServiceProvider serviceProvider)
        {
            try
            {
                using (var _context = new PersistenceContext(serviceProvider.GetRequiredService<DbContextOptions<PersistenceContext>>()))
                {
                    if (_context.Prestamos.Any())
                        return;

                    _context.Prestamos.AddRange(
                        new Entities.PrestamosE { Isbn = Guid.NewGuid().ToString(), IdentificacionUsuario = "goku123", TipoUsuario = TipoUsuarioPrestamo.AFILIADO , Id = "bec109ba-48bd-4393-bb4a-b3ca8c7aebad", FechaMaximaDevolucion = new DateTime()},
                        new Entities.PrestamosE { Isbn = Guid.NewGuid().ToString(), IdentificacionUsuario = "vegeta123", TipoUsuario = TipoUsuarioPrestamo.EMPLEADO, Id = Guid.NewGuid().ToString(), FechaMaximaDevolucion = new DateTime() },
                        new Entities.PrestamosE { Isbn = Guid.NewGuid().ToString(), IdentificacionUsuario = "sayayin123", TipoUsuario = TipoUsuarioPrestamo.INVITADO, Id = Guid.NewGuid().ToString(), FechaMaximaDevolucion = new DateTime() }
                        );

                    _context.SaveChanges();

                }

            }
            catch(Exception)
            {
                return;
            }
        }

    }
}
