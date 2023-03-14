using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;

namespace Milano.BackEnd.Dto.Lealtad
{
    /// <summary>
    /// Clase que ayuda a construir una peticion de para aplicar vale
    /// </summary>
    [DataContract]
    public class ConsultaClienteLealtadRequest
    {
        /// <summary>
        /// Codigo del cliente de sistema de credito
        /// </summary>
        [DataMember(Name = "iCodigoCliente")]
        public int iiCodigoCliente { get; set; }

        /// <summary>
        /// Numero de cliente de credito
        /// </summary>
        [DataMember(Name = "iCodigoClienteSistemaCredito")]
        public int iiCodigoClienteSistemaCredito { get; set; }

        /// <summary>
        /// Numero de empleado
        /// </summary>
        [DataMember(Name = "iCodigoEmpleado")]
        public int iiCodigoEmpleado { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "iCodigoClienteWeb")]
        public int iiCodigoClienteWeb { get; set; }

        /// <summary>
        /// Telefono del cliente
        /// </summary>
        [DataMember(Name = "sTelefono")]
        public string ssTelefono { get; set; }

        /// <summary>
        /// Apellido paterno del cliente
        /// </summary>
        [DataMember(Name = "sPaterno")]
        public string ssPaterno { get; set; }

        /// <summary>
        /// Apellido materno del cliente
        /// </summary>
        [DataMember(Name = "sMaterno")]
        public string ssMaterno { get; set; }

        /// <summary>
        /// Nombre del cliente
        /// </summary>
        [DataMember(Name = "sNombre")]
        public string ssNombre { get; set; }

        /// <summary>
        /// Fecha de nacimiento del cliente
        /// </summary>
        [DataMember(Name = "sFechaNacimiento")]
        public string ssFechaNacimiento { get; set; }

        /// <summary>
        /// Numero de tienda donde se registra al cliente
        /// </summary>
        [DataMember(Name = "iCodigoTiendaRegistro")]
        public int iiCodigoTiendaRegistro { get; set; }

        /// <summary>
        /// Correo electronico del cliente
        /// </summary>
        [DataMember(Name = "sEmail")]
        public string ssEmail { get; set; }

        /// <summary>
        /// Numero de caja donde se registra al cliente
        /// </summary>
        [DataMember(Name = "iCodigoTienda")]
        public int iiCodigoTienda { get; set; }

        /// <summary>
        /// Numero de empelado que registra al cliente
        /// </summary>
        [DataMember(Name = "iCodigoCaja")]
        public int iiCodigoCaja { get; set; }

    }
}
