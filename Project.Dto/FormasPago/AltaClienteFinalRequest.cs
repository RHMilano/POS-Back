using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// Dto para alta de clientes
    /// </summary>
    [DataContract]
    public class AltaClienteFinalRequest
    {
        /// <summary>
        /// INE del cliente final
        /// </summary>
        [DataMember(Name = "ine")]
        public string Ine { get; set; }
        /// <summary>
        /// RFC del cliente final
        /// </summary>
        [DataMember(Name = "rfc")]
        public string Rfc { get; set; }
        /// <summary>
        /// Nombre del cliente final
        /// </summary>
        [DataMember(Name = "nombre")]
        public string Nombre { get; set; }
        /// <summary>
        /// Apellidos del cliente
        /// </summary>
        [DataMember(Name = "apellidos")]
        public string Apellidos { get; set; }
        /// <summary>
        /// Fecha de nacimiento del cliente
        /// </summary>
        [DataMember(Name = "fechaNacimiento")]
        public string FechaNacimiento { get; set; }
        /// <summary>
        /// Sexo del cliente
        /// </summary>
        [DataMember(Name = "sexo")]
        public string Sexo { get; set; }
        /// <summary>
        /// Calle del cliente
        /// </summary>
        [DataMember(Name = "calle")]
        public string Calle { get; set; }
        /// <summary>
        /// Numero exterior del cliente
        /// </summary>
        [DataMember(Name = "numeroExterior")]
        public string NumeroExterior { get; set; }
        /// <summary>
        /// Numero interior del cliente
        /// </summary>
        [DataMember(Name = "numeroInterior")]
        public string NumeroInterior { get; set; }
        /// <summary>
        /// Colonia del cliente
        /// </summary>
        [DataMember(Name = "colonia")]
        public string Colonia { get; set; }
        /// <summary>
        /// Código Postal de cliente
        /// </summary>
        [DataMember(Name = "cp")]
        public int Cp { get; set; }

		/// <summary>
		/// Municipio
		/// </summary>
		[DataMember(Name = "municipio")]
		public string Municipio { get; set; }



        /// <summary>
        /// Ciudad del cliente
        /// </summary>
        [DataMember(Name = "ciudad")]
        public string Ciudad { get; set; }
        /// <summary>
        /// Estado del cliente
        /// </summary>
        [DataMember(Name = "estado")]
        public string Estado { get; set; }
        /// <summary>
        /// Código de mayorista del cliente
        /// </summary>
        [DataMember(Name = "codigoMayorista")]
        public string CodigoMayorista { get; set; }
        /// <summary>
        /// Telefono del cliente
        /// </summary>
        [DataMember(Name = "telefono")]
        public string Telefono { get; set; }

    }
}
