using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// DTO Cliente encontrado en la busqueda
    /// </summary>
    [DataContract]
    public class ClienteResponse
    {
        /// <summary>
        /// Nombre del cliente
        /// </summary>
        [DataMember(Name = "nombre")]
        public string Nombre { get; set; }
        /// <summary>
        /// Código del cliente
        /// </summary>
        [DataMember(Name = "codigoCliente")]
        public Int64 CodigoCliente { get; set; }
        /// <summary>
        /// Telefono del cliente
        /// </summary>
        [DataMember(Name = "telefono")]
        public string Telefono { get; set; }
        /// <summary>
        /// Código de tienda
        /// </summary>
        [DataMember(Name = "codigoTienda")]
        public int CodigoTienda { get; set; }
        /// <summary>
        /// Código de caja
        /// </summary>
        [DataMember(Name = "codigoCaja")]
        public int CodigoCaja { get; set; }
        /// <summary>
        /// Apellido paterno
        /// </summary>
        [DataMember(Name = "apellidoPaterno")]
        public string ApellidoPaterno { get; set; }
        /// <summary>
        /// Apellido materno
        /// </summary>
        [DataMember(Name = "apellidoMaterno")]
        public string ApellidoMaterno { get; set; }
        /// <summary>
        /// Calle 
        /// </summary>
        [DataMember(Name = "calle")]
        public string Calle { get; set; }
        /// <summary>
        /// Numero exterior
        /// </summary>
        [DataMember(Name = "noExterior")]
        public string NoExterior { get; set; }
        /// <summary>
        /// Numero interior
        /// </summary>
        [DataMember(Name = "noInterior")]
        public string NoInterior { get; set; }
        /// <summary>
        /// Ciudad
        /// </summary>
        [DataMember(Name = "ciudad")]
        public string Ciudad { get; set; }
        /// <summary>
        /// Estado
        /// </summary>
        [DataMember(Name = "estado")]
        public string Estado { get; set; }
        /// <summary>
        /// Codigo Postal
        /// </summary>
        [DataMember(Name = "codigoPostal")]
        public string CodigoPostal { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [DataMember(Name = "email")]
        public string Email { get; set; }

    }
}
