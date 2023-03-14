using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Finlag
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class TablaAmortizacionRequest
    {
        /// <summary>
        /// Id de la empresa distribuidora del vale
        /// </summary>
        [DataMember(Name = "idDistribuidora")]
        public int IdDistribuidora { get; set; }

        /// <summary>
        /// El folio del vale de entrada
        /// </summary>
        [DataMember(Name = "folioVale")]
        public string FolioVale { get; set; }

        /// <summary>
        /// Monto del vale
        /// </summary>
        [DataMember(Name = "montoVenta")]
        public double MontoVenta { get; set; }

        /// <summary>
        /// Nombre del solicitante del vale
        /// </summary>
        [DataMember(Name = "nombre")]
        public string Nombre { get; set; }

        /// <summary>
        /// Apellido Paterno del solicitante del vale
        /// </summary>
        [DataMember(Name = "aPaterno")]
        public string Apaterno { get; set; }

        /// <summary>
        /// Apellido Materno del solicitante del vale
        /// </summary>
        [DataMember(Name = "aMaterno")]
        public string Amaterno { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "fechaNacimiento")]
        public string FechaNacimiento { get; set; }

        /// <summary>
        /// Calle de la dirección del solicitante del vale
        /// </summary>
        [DataMember(Name = "calle")]
        public string Calle { get; set; }

        /// <summary>
        /// Numero exterior de la dirección del solicitante del vale
        /// </summary>
        [DataMember(Name = "numExt")]
        public string NumExt { get; set; }

        /// <summary>
        /// Colonia de la dirección del solicitante del vale
        /// </summary>
        [DataMember(Name = "colonia")]
        public string Colonia { get; set; }

        /// <summary>
        /// Estado de la dirección del solicitante del vale
        /// </summary>
        [DataMember(Name = "estado")]
        public string Estado { get; set; }

        /// <summary>
        /// Municipio de la dirección del solicitante del vale
        /// </summary>
        [DataMember(Name = "municipio")]
        public string Municipio { get; set; }

        /// <summary>
        /// Código Postal del solicitante del vale
        /// </summary>
        [DataMember(Name = "cP")]
        public string CP { get; set; }
    }
}
