using Milano.BackEnd.Dto.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.General
{

    /// <summary>
    /// Clase que representa el total contabilizado en caja de una forma de pago en particular
    /// </summary>
    [DataContract]
    public class LecturaTotalDetalleFormaPago
    {

        /// <summary>
        /// Total Importe Fisico
        /// </summary>
        [DataMember(Name = "importeFisico")]
        public decimal ImporteFisico { get; set; }

        /// <summary>
        /// Total Importe Teorico
        /// </summary>
        [DataMember(Name = "importeTeorico")]
        public decimal ImporteTeorico { get; set; }

        /// <summary>
        /// Total Importe Retiro
        /// </summary>
        [DataMember(Name = "importeRetiro")]
        public decimal ImporteRetiro { get; set; }

        /// <summary>
        /// Total Ingresos con Retiros Parciales incluidos
        /// </summary>
        [DataMember(Name = "totalIngresosConRetirosParciales")]
        public decimal TotalIngresosConRetirosParciales { get; set; }

        /// <summary>
        /// Total Ingresos con Retiros Parciales y Fondo Fijo Incluidos
        /// </summary>
        [DataMember(Name = "totalIngresosConRetirosParcialesConFondoFijo")]
        public decimal TotalIngresosConRetirosParcialesConFondoFijo { get; set; }

        /// <summary>
        /// Total Retiros Parciales en caso de aplicar
        /// </summary>
        [DataMember(Name = "totalRetirosParciales")]
        public decimal TotalRetirosParciales { get; set; }

        /// <summary>
        /// Total Fondo Fijo
        /// </summary>
        [DataMember(Name = "totalFondoFijo")]
        public decimal TotalFondoFijo { get; set; }

        /// <summary>
        /// Información asociada de la Forma de Pago
        /// </summary>
        [DataMember(Name = "informacionAsociadaFormaPago")]
        public ConfigGeneralesCajaTiendaFormaPago InformacionAsociadaFormasPago { get; set; }

        /// <summary>
        /// Información asociada de las denominaciones en caso de aplicar
        /// </summary>
        [DataMember(Name = "informacionAsociadaDenominaciones")]
        public Denominacion[] InformacionAsociadaDenominaciones { get; set; }

    }
}