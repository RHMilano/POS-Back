using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// Clase de peticion de usuario
    /// </summary>
    [DataContract]
    public class EmployeeRequest
    {
        /// <summary>
        /// Número de empleado
        /// </summary>
        [DataMember(Name = "code")]
        public int Code { get; set; }

        /// <summary>
        /// Nombre del empleado
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
