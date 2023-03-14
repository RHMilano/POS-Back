using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;

namespace Milano.BackEnd.Dto.Lealtad
{
    /// <summary>
    /// Respuesta  para la consulta de los clientes
    /// </summary>
    [DataContract]
    public class ConsultaClienteLealtadResponse
    {
        /// <summary>
        /// Mensajes en caso de error
        /// </summary>
        [DataMember(Name = "sMensajeError")]
        public string ssMensajeError { get; set; }

        /// <summary>
        /// Numero de clientes encontrados
        /// </summary>
        [DataMember(Name = "iCantidadClientes")]
        public int iiCantidadClientes { get; set; }

        /// <summary>
        /// Indica si la cantidad de resultados esta restringida
        /// </summary>
        [DataMember(Name = "bCantidadLimitada")]
        public bool bbCantidadLimitada { get; set; }

        /// <summary>
        /// Indica si la cantidad de resultados esta restringida
        /// </summary>
        [DataMember(Name = "InfoClientesCRM")]
        public rInfoClientesCRM[] IInfoClientesCRM { get; set; }

    }

    /// <summary>
    /// Clase que ayuda a construir una peticion de para aplicar vale
    /// </summary>
    //[DataContract]
    public class rInfoClientesCRM
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
        [DataMember(Name = "sNivel")]
        public string ssNivel { get; set; }

        /// <summary>
        /// Telefono del cliente
        /// </summary>
        [DataMember(Name = "bPrimeraCompra")]
        public bool bbPrimeraCompra { get; set; }

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
        /// Nombre del cliente
        /// </summary>
        [DataMember(Name = "sGenero")]
        public string ssGenero { get; set; }

        /// <summary>
        /// Fecha de nacimiento del cliente
        /// </summary>
        [DataMember(Name = "sFechaNacimiento")]
        public string ssFechaNacimiento { get; set; }

        /// <summary>
        /// Fecha de nacimiento del cliente
        /// </summary>
        [DataMember(Name = "dSaldo")]
        public double ddSaldo { get; set; }

        /// <summary>
        /// Fecha de nacimiento del cliente
        /// </summary>
        [DataMember(Name = "sFechaRegistro")]
        public string ssFechaRegistro { get; set; }

        /// <summary>
        /// Correo electronico del cliente
        /// </summary>
        [DataMember(Name = "sEmail")]
        public string ssEmail { get; set; }

    }
}
