using Milano.BackEnd.Dto.Configuracion;
using Milano.BackEnd.Dto.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.InicioFinDia
{

    /// <summary>
    /// Clase que representa el detalle de una forma de pago para una lecturaZ en Cash Out
    /// </summary>
    [DataContract]
    public class DetalleLecturaFormaPago
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
        /// Total Retiros Parciales en caso de aplicar
        /// </summary>
        [DataMember(Name = "totalRetirosParciales")]
        public decimal TotalRetirosParciales { get; set; }

        /// <summary>
        /// Fondo Fijo configurado actual
        /// </summary>
        [DataMember(Name = "fondoFijoActual")]
        public decimal FondoFijoActual { get; set; }

        /// <summary>
        /// Información asociada de la Forma de Pago
        /// </summary>
        [DataMember(Name = "informacionAsociadaFormaPago")]
        public ConfigGeneralesCajaTiendaFormaPago InformacionAsociadaFormasPago { get; set; }

    }
}