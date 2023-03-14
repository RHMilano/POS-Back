using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Milano.BackEnd.Dto.Catalogs
{

    /// <summary>
    /// DTO de forma de Pago
    /// </summary>
    [DataContract]
    public class FormaPagoResponse
    {

        /// <summary>
        /// Codigo Forma de Pago
        /// </summary>
        [DataMember(Name = "codigoFormaPago")]
        public string CodigoFormaPago { get; set; }

        /// <summary>
        /// Descripcion de la Forma de Pago
        /// </summary>
        [DataMember(Name = "descripcionFormaPago")]
        public string DescripcionFormaPago { get; set; }

        /// <summary>
        /// Descripcion Corta de la Forma de Pago
        /// </summary>
        [DataMember(Name = "descripcionCorta")]
        public string DescripcionCorta { get; set; }

        /// <summary>
        /// Indica si se requiere presentar documentacion
        /// </summary>
        [DataMember(Name = "requiereDocumento")]
        public int RequiereDocumento { get; set; }

        /// <summary>
        /// Tipo de Moneda
        /// </summary>
        [DataMember(Name = "codigoMoneda")]
        public String CodigoMoneda { get; set; }

        /// <summary>
        /// Es tarjeta de Regalo
        /// </summary>
        [DataMember(Name = "esTarjetaDeRegalo")]
        public Byte EsTarjetaDeRegalo { get; set; }

        /// <summary>
        /// Es cupon
        /// </summary>
        [DataMember(Name = "esCupon")]
        public Byte EsCupon { get; set; }

    }
}
