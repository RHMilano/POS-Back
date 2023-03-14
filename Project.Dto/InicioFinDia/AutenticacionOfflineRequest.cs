using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.InicioFinDia
{
    /// <summary>
    /// DTO para la autentitación Offline
    /// </summary>
    [DataContract]
    public class AutenticacionOfflineRequest
    {

        /// <summary>
        /// Clave de autenticación
        /// </summary>
        [DataMember(Name = "clave")]
        public String Clave { get; set; }

    }
}
