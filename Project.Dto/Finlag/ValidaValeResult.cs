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
    public class ValidaValeResult
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "estatusVale")]
        public string EstatusVale {get; set;}

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "nombreDistribuidora")]
        public string NombreDistribuidora { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "creditoVale")]
        public string CreditoVale { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "estatusDV")]
        public string EstatusDV { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "firmaDV")]
        public string FirmaDV { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "idDistribuidora")]
        public string IdDistribuidora { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "folioVale")]
        public string Foliovale { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "quincenas")]
        public string Quincenas { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "nombre")]
        public string Nombre { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "paterno")]
        public string Paterno { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "materno")]
        public string Materno { get; set; }

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
        [DataMember(Name = "numero")]
        public string Numero { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "colonia")]
        public string Colonia { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "cP")]
        public string CP { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "municipio")]
        public string Municipio { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "estado")]
        public string Estado { get; set; }


        /// <summary>
        /// Folio del vale a consultar
        /// </summary>
        [DataMember(Name = "estatusCliente")]
        public bool EstatusCliente { get; set; }

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
