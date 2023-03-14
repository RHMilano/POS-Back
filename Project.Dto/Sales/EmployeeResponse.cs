using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// Clase de respuesta del usuario
    /// </summary>
    [DataContract]
    public class EmployeeResponse
    {

        /// <summary>
        /// Numero de empleado 
        /// </summary>
        [DataMember(Name = "code")]
        public int Code { get; set; }

        /// <summary>
        /// Nombre de usuario
        /// </summary>
        [DataMember(Name = "userName")]
        public string UserName { get; set; }

        /// <summary>
        /// Nombre del empleado
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Apellido paterno
        /// </summary>
        [DataMember(Name = "paternal")]
        public string Paternal { get; set; }

        /// <summary>
        /// Apellido materno
        /// </summary>
        [DataMember(Name = "maternal")]
        public string Maternal { get; set; }

        /// <summary>
        /// Puesto
        /// </summary>
        [DataMember(Name = "position")]
        public string Position { get; set; }

        /// <summary>
        /// Codigo del rol
        /// </summary>
        [DataMember(Name = "roleCode")]
        public int RoleCode { get; set; }

        /// <summary>
        /// Fecha alta
        /// </summary>
        [DataMember(Name = "initialDate")]
        public string InitialDate { get; set; }

        ///<summary>
        /// Sexo
        /// </summary>
        [DataMember(Name = "sex")]
        public string Sex { get; set; }

        ///<summary>
        /// Estatus
        /// </summary>
        [DataMember(Name = "status")]
        public string Status { get; set; }

        ///<summary>
        /// Tienda asignada
        /// </summary>
        [DataMember(Name = "store")]
        public int Store { get; set; }

    }
}
