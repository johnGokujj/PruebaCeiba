using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace PruebaIngresoBibliotecario.Data
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
                        new Entities.PrestamosE { isbn = new Guid(), identificacionUsuario = "goku123", tipoUsu = Entities.TipoUsuarioE.USUARIO_AFILIADO},
                        new Entities.PrestamosE { isbn = new Guid(), identificacionUsuario = "vegeta123", tipoUsu = Entities.TipoUsuarioE.USUARIO_EMPLEADO_DE_LA_BIBLIOTECA },
                        new Entities.PrestamosE { isbn = new Guid(), identificacionUsuario = "sayayin123", tipoUsu = Entities.TipoUsuarioE.USUARIO_INVITADO }
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
