using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.InicioFinDia
{

    /// <summary>
    /// DTO que contiene la información de la relación de Caja
    /// </summary>
    [DataContract]
    public class RelacionCaja
    {

        /// <summary>
        /// Total de registros
        /// </summary>
        [DataMember(Name = "totalRegistros")]
        public int TotalRegistros { get; set; }

        /// <summary>
        /// Id de la relación de caja
        /// </summary>
        [DataMember(Name = "idRelacionCaja")]
        public int IdRelacionCaja { get; set; }

        /// <summary>
        /// Codigo de la tienda
        /// </summary>
        [DataMember(Name = "codigoTienda")]
        public int CodigoTienda { get; set; }

        /// <summary>
        /// Total con IVA de la relación de Caja
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
        /// Fecha asociada a la Relación de Caja
        /// </summary>
        [DataMember(Name = "fecha")]
        public string Fecha { get; set; }

        /// <summary>
        /// Información sobre los depósitos asociados a la relación de caja
        /// </summary>
        [DataMember(Name = "depositosAsociados")]
        public DepositoAsociado[] DepositosAsociados { get; set; }

        /// <summary>
        /// Información asociada los grupos del reporte: INGRESOS, EGRESOS, ANEXOS
        /// </summary>
        [DataMember(Name = "gruposRelacionCaja")]
        public GrupoRelacionCaja[] GruposRelacionCaja { get; set; }

    }
}
