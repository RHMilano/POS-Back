using Milano.BackEnd.Dto.Sales;
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
    public class RegistroLealtadRequest
    {

        /// <summary>
        /// Token para la peticion de lealtad
        /// </summary>
        [DataMember(Name = "sToken")]
        public string ssToken { get; set; }

        /// <summary>
        /// Codigo del cliente de sistema de credito
        /// </summary>
        [DataMember(Name = "iCodigoClienteSistemaCredito")]
        public int iiCodigoClienteSistemaCredito { get; set; }

        /// <summary>
        /// Numero de empleado de la tienda
        /// </summary>
        [DataMember(Name = "iCodigoEmpleado")]
        public int iiCodigoEmpleado { get; set; }

        /// <summary>
        /// Codigo de lealtad del cliente
        /// </summary>
        [DataMember(Name = "iCodigoClienteWeb")]
        public int iiCodigoClienteWeb { get; set; }

        /// <summary>
        /// Numero telefonico del cliente
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
        /// Genero del cliente
        /// </summary>
        [DataMember(Name = "sGenero")]
        public string ssGenero { get; set; }

        /// <summary>
        /// Fecha de nacimiento del cliente
        /// </summary>
        [DataMember(Name = "sFechaNacimiento")]
        public string ssFechaNacimiento { get; set; }

        /// <summary>
        /// Folio de venta de la primera compra del cliente al momento del registro
        /// </summary>
        [DataMember(Name = "sFolioVenta")]
        public string ssFolioVenta { get; set; }

        /// <summary>
        /// Numero de tienda donde se registra al cliente
        /// </summary>
        [DataMember(Name = "iCodigoTiendaRegistra")]
        public int iiCodigoTiendaRegistra { get; set; }

        /// <summary>
        /// Numero de caja donde se registra al cliente
        /// </summary>
        [DataMember(Name = "iCodigoCajaRegistra")]
        public int iiCodigoCajaRegistra { get; set; }

        /// <summary>
        /// Numero de empelado que registra al cliente
        /// </summary>
        [DataMember(Name = "iCodigoEmpleadoRegistra")]
        public int iiCodigoEmpleadoRegistra { get; set; }

        /// <summary>
        /// Correo electronico del cliente
        /// </summary>
        [DataMember(Name = "sEmail")]
        public string ssEmail { get; set; }

    }
}
