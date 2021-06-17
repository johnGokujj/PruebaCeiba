using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PruebaIngresoBibliotecario.Entities;
using PruebaIngresoBibliotecario.Business;
using System;
using PruebaIngresoBibliotecario.DBContext;
using System.Linq;
using System.Collections.Generic;

namespace PruebaIngresoBibliotecario.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamoController : ControllerBase
    {
        private readonly PersistenceContext ctx;


        public PrestamoController(PersistenceContext context)
        {
            ctx = context;
        }


        [HttpPost]
        [Route("prestamo")]
        public ActionResult prestamo([FromBody] PrestamosE nuevo)
        {
            PrestamoB miPrestamoB = new PrestamoB();

            if (nuevo.identificacionUsuario.Length>10)
                return new  OkObjectResult(new { StatusCode=400, Message = "La identificacionUsuario debe ser maximo de 10." });

            nuevo.fechaMaximaDevolucion = miPrestamoB.obtenerFechaEntrega(nuevo.tipoUsu);

            using (ctx)
            {
                PrestamosE presta = ctx.Prestamos.Where(x => x.Id == nuevo.Id).FirstOrDefault();
                if (presta != null)
                    return new OkObjectResult(new { StatusCode = 400, Message = String.Join("El usuario con identificacion {0} ya tiene un libro prestado por lo cual no se le puede realizar otro prestamo", nuevo.identificacionUsuario) });

                ctx.Prestamos.Add(nuevo);
                ctx.SaveChanges();
                return new OkObjectResult(new { StatusCode = 200, Message = "id : {0} , fechaMaximaDevolucion {1}",nuevo.Id, nuevo.fechaMaximaDevolucion });

            }
        }

        [HttpGet]
        [Route("prestamo/{Id-prestamo}")]
        public ActionResult prestamo(Guid Id)
        {
            PrestamoB miPrestamoB = new PrestamoB();
            List<string> datos = miPrestamoB.tieneprestado(ctx.Prestamos.ToList(), Id);
            if(datos != null)
                return new OkObjectResult(new { StatusCode = datos[0], Message = datos[1] });
            else
                return new OkObjectResult(new { StatusCode = 404, Message = string.Format("El prestamo con id {0} no existe", Id ) });
        }

    }
}
