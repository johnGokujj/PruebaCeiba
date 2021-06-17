using PruebaIngresoBibliotecario.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PruebaIngresoBibliotecario.Business
{
    public class PrestamoB
    {
        public void crearPrestamo(PrestamoB todoItem)
        {
            //if()
        }

        public List<string> tieneprestado(List<PrestamosE> lst , Guid Id)
        {
            try
            {
                PrestamosE presta = lst.Where(x => x.Id == Id).FirstOrDefault();
                if (presta != null)
                    return new List<string> { "200", string.Format("id : {0} , isbn : {1}, identificaciónUsuario : {2}, tipoUsuario : {3} fechaMaximaDevolucion : {4}", presta.Id, presta.isbn, presta.identificacionUsuario, presta.tipoUsu, presta.fechaMaximaDevolucion) };
            }
            catch (Exception ex)
            {
                throw new Exception( ex.Message);
            }
            return null;
        }

        public DateTime obtenerFechaEntrega(int tipoUsu)
        {
            DateTime fecha = DateTime.Today;

            try
            {
                switch (tipoUsu)
                {
                    case 1:// TipoUsuarioE.USUARIO_AFILIADO:
                        fecha = calcularFecha(fecha, fecha.AddDays(10),10);
                        break;

                    case 2:// TipoUsuarioE.USUARIO_EMPLEADO_DE_LA_BIBLIOTECA:
                        fecha = calcularFecha(fecha, fecha.AddDays(8),8);
                        break;

                    case 3:// TipoUsuarioE.USUARIO_INVITADO:
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


        public DateTime calcularFecha(DateTime fechaIni, DateTime fechaFin,int dias)
        {
            var fechas = Enumerable.Range(0, 5 + fechaFin.Subtract(fechaIni).Days)
                        .Select(offset => fechaIni.AddDays(offset))
                        .ToArray();
            fechas =  fechas.Where(x => x.DayOfWeek != DayOfWeek.Saturday)
                            .Where(x => x.DayOfWeek != DayOfWeek.Sunday).ToArray();
            return fechas[dias];
        }

    }
}
