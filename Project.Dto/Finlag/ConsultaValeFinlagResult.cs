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
    public class ConsultaValeFinlagResult
    {
        [DataMember(Name = "tiendaAplica")]
        public int TiendaAplica { get; set; }

        [DataMember(Name = "idCajaAplica")]
        public int IdCajaAplica { get; set; }

        [DataMember(Name = "fechaAplicacion")]
        public string FechaAplicacion { get; set; }

        [DataMember(Name = "folioVenta")]
        public string FolioVenta { get; set; }

        [DataMember(Name = "folioVale")]
        public string FolioVale { get; set; }

        [DataMember(Name = "DV")]
        public string DV { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "nombre")]
        public string Nombre { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "aPaterno")]
        public string Apaterno { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "aMaterno")]
        public string Amaterno { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "iNE")]
        public string INE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "fechaNacimiento")]
        public string FechaNacimiento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "calle")]
        public string Calle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "numExt")]
        public string NumExt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "colonia")]
        public string Colonia { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "estado")]
        public string Estado { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "municipio")]
        public string Municipio { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "cP")]
        public string CP { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "sexo")]
        public string Sexo { get; set; }

        [DataMember(Name = "montoAplicado")]
        public decimal MontoAplicado { get; set; }

        [DataMember(Name = "concepto")]
        public string Concepto { get; set; }

        [DataMember(Name = "quincenas")]
        public int Quincenas { get; set; }

        [DataMember(Name = "total")]
        public decimal Total { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "estatusVale")]
        public string EstatusVale { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "NumeroCodigo")]
        public int NumeroCodigo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "DescripcionCodigo")]
        public string DescripcionCodigo { get; set; }
    }
}
