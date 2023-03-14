using Milano.BackEnd.Dto.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.InicioFinDia
{

    /// <summary>
    /// DTO que contiene la información de un depósito asociado
    /// </summary>
    [DataContract]
    public class DepositoAsociado
    {

        /// <summary>
        /// Total depósito
        /// </summary>
        [DataMember(Name = "totalConIVA")]
        public decimal TotalConIVA { get; set; }

        /// <summary>
        /// Información asociada de la Forma de Pago
        /// </summary>
        [DataMember(Name = "informacionAsociadaFormaPago")]
        public ConfigGeneralesCajaTiendaFormaPago InformacionAsociadaFormasPago { get; set; }

    }
}
