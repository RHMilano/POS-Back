using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.InicioFinDia
{

    /// <summary>
    /// DTO que contiene la información de un Grupo en la relación de Caja
    /// </summary>
    [DataContract]
    public class GrupoRelacionCaja
    {

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "idGrupo")]
        public int IdGrupo { get; set; }

        /// <summary>
        /// Cabecera de la sección
        /// </summary>
        [DataMember(Name = "encabezado")]
        public string Encabezado { get; set; }

        /// <summary>
        /// Total con IVA del Grupo
        /// </summary>
        [DataMember(Name = "totalConIVA")]
        public decimal TotalConIVA { get; set; }

        /// <summary>
        /// Total sin IVA del Grupo
        /// </summary>
        [DataMember(Name = "totalSinIVA")]
        public decimal TotalSinIVA { get; set; }

        /// <summary>
        /// Total IVA del Grupo
        /// </summary>
        [DataMember(Name = "iVA")]
        public decimal IVA { get; set; }

        /// <summary>
        /// Información asociada  las secciones del reporte: VENTAS , TIEMPO AIRE, SERVICIOS, ETC.
        /// </summary>
        [DataMember(Name = "seccionesRelacionCaja")]
        public SeccionRelacionCaja[] SeccionesRelacionCaja { get; set; }

    }
}
