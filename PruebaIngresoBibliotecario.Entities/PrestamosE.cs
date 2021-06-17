using System;
using System.Text.Json.Serialization;

namespace PruebaIngresoBibliotecario.Entities
{
    public class PrestamosE
    {
        /// <summary>
        /// identificador unico de un libro
        /// </summary>
        [JsonIgnore]
        public Guid isbn { get; set; }

        /// <summary>
        /// numero de la identificacion del usuario
        /// </summary>
        public string identificacionUsuario { get; set; }

        /// <summary>
        /// determina la relacion que tiene el usuario con la biblioteca
        /// </summary>
        public int tipoUsu;

        /// <summary>
        /// fecha de cuando debe devolver el libro
        /// </summary>
        public DateTime fechaMaximaDevolucion { get; set; }

        /// <summary>
        /// Id generado al realizarce el prestamo
        /// </summary>
        [JsonIgnore]
        public Guid Id { get; set; }
    }

    /// <summary>
    /// Id tipo Usuario biblioteca
    /// </summary>
    public enum TipoUsuarioE
    {
        USUARIO_AFILIADO = 1,
        USUARIO_EMPLEADO_DE_LA_BIBLIOTECA = 2,
        USUARIO_INVITADO = 3
    }
}
