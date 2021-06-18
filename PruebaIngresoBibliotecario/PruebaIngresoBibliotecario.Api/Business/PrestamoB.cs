using Microsoft.AspNetCore.Mvc;
using PruebaIngresoBibliotecario.DBContext;
using PruebaIngresoBibliotecario.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaIngresoBibliotecario.Business
{
    /// <summary>
    /// Clase Encargada de manejar el Negocio de los prestamos de la Biblioteca
    /// </summary>
    public class PrestamoB
    {
        /// <summary>
        /// Funcion para crear un prestamo en la biblioteca
        /// </summary>
        /// <param name="ctx">Datos en memoria existentes que se pueden manipular</param>
        /// <param name="nuevo">Nuevo prestamo a realizar</param>
        /// <returns></returns>
        public async Task<ActionResult> CrearPrestamo(PersistenceContext ctx,PrestamosE nuevo)
        {
            //Regex r = new Regex("^[a-zA-Z0-9]*$"); // por si tiene que validar que sea Alfanumerico
            if (nuevo.IdentificacionUsuario.Length > 10 || !Enum.IsDefined(typeof(TipoUsuarioPrestamo), nuevo.TipoUsuario))
                return new BadRequestObjectResult(new { Message = "La identificacionUsuario debe ser maximo de 10." });

            List<string> datos = ValidarUsuario( ctx.Prestamos.ToList(), nuevo);

            if (datos[0] == "OK")
            {
                try
                {
                    using (ctx)
                    {
                        nuevo.Id = Guid.NewGuid().ToString();
                        nuevo.FechaMaximaDevolucion = DateTime.Parse(datos[1]);
                        ctx.Prestamos.Add(nuevo);
                        await ctx.SaveChangesAsync();
                        return new OkObjectResult(new { nuevo.Id, fechaMaximaDevolucion = nuevo.FechaMaximaDevolucion.ToString("dd/MM/yyyy") });
                    }
                }
                catch(Exception ex)
                { 
                    return new NotFoundObjectResult(new { ex.Message });
                }
            }
            else
                return new BadRequestObjectResult(new { mensaje = datos[0] });
        }

        /// <summary>
        /// Funcion para revisar si el usuario es de tipo USUARIO_INVITADO, de esta verifica si ya tiene prestamos
        /// </summary>
        /// <param name="lst"> datos en memoria de los registros</param>
        /// <param name="nuevo">nuevo registro de prestamo</param>
        /// <returns>lista de respuesta</returns>
        public List<string> ValidarUsuario(List<PrestamosE> lst, PrestamosE nuevo)
        {
            try
            {
                if (nuevo.TipoUsuario == TipoUsuarioPrestamo.INVITADO)
                {
                    PrestamosE presta = lst.Where(x => x.IdentificacionUsuario == nuevo.IdentificacionUsuario).FirstOrDefault();
                    if (presta != null)
                        return new List<string> { String.Format("El usuario con identificacion {0} ya tiene un libro prestado por lo cual no se le puede realizar otro prestamo", nuevo.IdentificacionUsuario) };
                }

                return new List<string> { "OK", nuevo.ObtenerFechaEntrega(nuevo.TipoUsuario).ToString() };

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Funcion para buscar si existe un prestamo teniendo en cuenta su Id
        /// </summary>
        /// <param name="lst">Lista de los prestamos existentes</param>
        /// <param name="Id">Identificador del prestamo a buscar</param>
        /// <returns></returns>
        public async Task<ActionResult> TienePrestado(PersistenceContext ctx, string Id)
        {
            try
            {
                PrestamosE datos = await ctx.Prestamos.FindAsync(Id);

                if (datos != null)
                    return new OkObjectResult(new { datos.Id, datos.Isbn, datos.IdentificacionUsuario });
                else
                    return new NotFoundObjectResult(new { Message = string.Format("El prestamo con id {0} no existe", Id) });

            }
            catch (Exception ex)
            {
                throw new Exception( ex.Message);
            }
        }

    }
}
