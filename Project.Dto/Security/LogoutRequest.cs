using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// Clase de usuario para solicitar Logout de la aplicación
    /// </summary>
    [DataContract]
    public class LogoutRequest
    {
        /// <summary>
        /// Número de empleado
        /// </summary>
        [DataMember(Name = "numberEmployee")]
        public int NumberEmployee { get; set; }

    }
}
