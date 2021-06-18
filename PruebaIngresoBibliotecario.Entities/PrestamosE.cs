using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace PruebaIngresoBibliotecario.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class PrestamosE
    {
        /// <summary>
        /// Id generado al realizarce el prestamo
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// identificador unico de un libro
        /// </summary>
        public string Isbn { get; set; }

        /// <summary>
        /// numero de la identificacion del usuario
        /// </summary>
        public string IdentificacionUsuario { get; set; }

        /// <summary>
        /// determina la relacion que tiene el usuario con la biblioteca
        /// </summary>
        public TipoUsuarioPrestamo TipoUsuario { get; set; }

        /// <summary>
        /// fecha de cuando debe devolver el libro
        /// </summary>
        public DateTime FechaMaximaDevolucion { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public PrestamosE()
        {

        }

        /// <summary>
        /// Obtiene la fecha a devolver el prestamo, segun el tipo de Usuario
        /// </summary>
        /// <param name="tipoUsu">Tipo de Usuario Prestamo</param>
        /// <returns>Fecha</returns>
        public DateTime ObtenerFechaEntrega(TipoUsuarioPrestamo tipoUsu)
        {
            DateTime fecha = DateTime.Today;

            try
            {
                switch (tipoUsu)
                {
                    case TipoUsuarioPrestamo.AFILIADO:
                        fecha = CalcularFecha(fecha, fecha.AddDays(10), 10);
                        break;

                    case TipoUsuarioPrestamo.EMPLEADO:
                        fecha = CalcularFecha(fecha, fecha.AddDays(8), 8);
                        break;

                    case TipoUsuarioPrestamo.INVITADO:
                        fecha = CalcularFecha(fecha, fecha.AddDays(7), 7);
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
        public DateTime CalcularFecha(DateTime fechaIni, DateTime fechaFin, int dias)
        {
            var fechas = Enumerable.Range(0, 5 + fechaFin.Subtract(fechaIni).Days)
                        .Select(offset => fechaIni.AddDays(offset)).ToArray();

            fechas = fechas.Where(x => x.DayOfWeek != DayOfWeek.Saturday && x.DayOfWeek != DayOfWeek.Sunday)
                            .ToArray();

            return fechas[dias];
        }
    }

    /// <summary>
    /// Id tipo Usuario biblioteca
    /// </summary>
    public enum TipoUsuarioPrestamo
    {
        AFILIADO = 1,
        EMPLEADO = 2,
        INVITADO = 3
    }
}
