using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.InicioFinDia
{

    /// <summary>
    /// DTO que contiene la información de una sección en la relación de Caja
    /// </summary>
    [DataContract]
    public class SeccionRelacionCaja
    {

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "idSeccion")]
        public int IdSeccion { get; set; }

        /// <summary>
        /// Cabecera de la sección
        /// </summary>
        [DataMember(Name = "encabezado")]
        public string Encabezado { get; set; }

        /// <summary>
        /// Total con IVA de la Sección
        /// </summary>
        [DataMember(Name = "totalConIVA")]
        public decimal TotalConIVA { get; set; }

        /// <summary>
        /// Total sin IVA de la relación de Caja
        /// </summary>
        [DataMember(Name = "totalSinIVA")]
        public decimal TotalSinIVA { get; set; }

        /// <summary>
        /// Total IVA de la relación de Caja
        /// </summary>
        [DataMember(Name = "iVA")]
        public decimal IVA { get; set; }

        /// <summary>
        /// Información asociada al Desglose de la Sección
        /// </summary>
        [DataMember(Name = "desgloseRelacionCaja")]
        public Desglose[] DesgloseRelacionCaja { get; set; }

    }
}
