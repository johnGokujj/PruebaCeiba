using Microsoft.AspNetCore.Mvc;
using PruebaIngresoBibliotecario.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaIngresoBibliotecario.Business
{
    public class PrestamoB
    {
        /// <summary>
        /// revisar si el usuario es de tipo USUARIO_INVITADO, de esta verifica si ya tiene prestamos
        /// </summary>
        /// <param name="lst"> datos en memoria de los registros</param>
        /// <param name="nuevo">nuevo registro de prestamo</param>
        /// <returns>lista de respuesta</returns>
        public List<string> validarUsuario (List<PrestamosE> lst, PrestamosE nuevo)
        {
            try
            {
                if (nuevo.TipoUsuario == TipoUsuarioPrestamo.INVITADO)
                {
                    PrestamosE presta = lst.Where(x => x.identificacionUsuario == nuevo.identificacionUsuario).FirstOrDefault();
                    if (presta != null)
                        return new List<string> { String.Format("El usuario con identificacion {0} ya tiene un libro prestado por lo cual no se le puede realizar otro prestamo", nuevo.identificacionUsuario) };
                }

                return new List<string> { "OK", obtenerFechaEntrega(nuevo.TipoUsuario).ToString() };

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ActionResult> CrearPrestamo(PruebaIngresoBibliotecario.DBContext.PersistenceContext ctx, string Id)
        {
            using (ctx)
            {
                nuevo.Id = Guid.NewGuid().ToString();
                nuevo.fechaMaximaDevolucion = DateTime.Parse(datos[1]);
                ctx.Prestamos.Add(nuevo);
                ctx.SaveChanges();
                ctx.Dispose();
                return new OkObjectResult(new { nuevo.Id, fechaMaximaDevolucion = nuevo.fechaMaximaDevolucion.ToString("dd/MM/yyyy") });
            }
        }



        public async Task<ActionResult> Tieneprestado(List<PrestamosE> lst , string Id)
        {
            try
            {
                PrestamosE datos =  lst.Where(x => x.Id == Id).FirstOrDefault();
                if (datos != null)
                    return new OkObjectResult(new { datos.Id, datos.isbn, datos.identificacionUsuario });
                else
                    return new NotFoundObjectResult(new { Message = string.Format("El prestamo con id {0} no existe", Id) });

            }
            catch (Exception ex)
            {
                throw new Exception( ex.Message);
            }
        }

        /// <summary>
        /// Obtiene la fecha a devolver el prestamo, segun el tipo de Usuario
        /// </summary>
        /// <param name="tipoUsu">Tipo de Usuario Prestamo</param>
        /// <returns>Fecha</returns>
        public DateTime obtenerFechaEntrega(TipoUsuarioPrestamo tipoUsu)
        {
            DateTime fecha = DateTime.Today;

            try
            {
                switch (tipoUsu)
                {
                    case TipoUsuarioPrestamo.AFILIADO:
                        fecha = calcularFecha(fecha, fecha.AddDays(10),10);
                        break;

                    case TipoUsuarioPrestamo.EMPLEADO:
                        fecha = calcularFecha(fecha, fecha.AddDays(8),8);
                        break;

                    case TipoUsuarioPrestamo.INVITADO:
                        fecha = calcularFecha(fecha, fecha.AddDays(7),7);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return fecha;
        }

        /// <summary>
        /// Funcion para regresar una fecha a partir de una fecha inicial y una final
        /// </summary>
        /// <param name="fechaIni">Fecha Inicio</param>
        /// <param name="fechaFin">Fecha Fin</param>
        /// <param name="dias">Numero de dias a contar</param>
        /// <returns>Fecha procesada</returns>
        public DateTime calcularFecha(DateTime fechaIni, DateTime fechaFin,int dias)
        {
            var fechas = Enumerable.Range(0, 5 + fechaFin.Subtract(fechaIni).Days)
                        .Select(offset => fechaIni.AddDays(offset)).ToArray();

            fechas =  fechas.Where(x => x.DayOfWeek != DayOfWeek.Saturday && x.DayOfWeek != DayOfWeek.Sunday)
                            .ToArray();

            return fechas[dias];
        }

    }
}
