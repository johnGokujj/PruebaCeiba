using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PruebaIngresoBibliotecario.Entities;
using PruebaIngresoBibliotecario.Business;
using System;
using PruebaIngresoBibliotecario.DBContext;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;

namespace PruebaIngresoBibliotecario.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamoController : ControllerBase
    {
        private readonly PersistenceContext ctx;

        /// <summary>
        /// Asignacion del contexto para poder manejar los datos en memoria
        /// </summary>
        /// <param name="context"></param>
        public PrestamoController(PersistenceContext context)
        {
            ctx = context;
        }

        /// <summary>
        /// Funcion que permite ingresar un nuevo prestamo a la biblioteca
        /// </summary>
        /// <param name="nuevo">Contiene la nueva entidad a ingresar</param>
        /// <returns>Mensaje de informacion</returns>
        [HttpPost]
        public async Task<IActionResult> prestamo([FromBody] PrestamosE nuevo)
        {
            PrestamoB miPrestamoB = new PrestamoB();
            return await miPrestamoB.CrearPrestamo(ctx, nuevo);
        }

        /// <summary>
        /// Funcion para buscar un prestamo en la biblioteca 
        /// </summary>
        /// <param name="Id"> Identificador del prestamo</param>
        /// <returns>Mensaje de informacion</returns>
        [HttpGet]
        [Route("{idPrestamo}")]
        public async Task<ActionResult> prestamo(string idPrestamo)
        {
            PrestamoB miPrestamoB = new PrestamoB();
            return await miPrestamoB.TienePrestado(ctx, idPrestamo);
        }

    }
}
