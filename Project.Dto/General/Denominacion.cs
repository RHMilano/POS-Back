using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.General
{

    /// <summary>
    /// Clase que representa una denominación
    /// </summary>
    [DataContract]
    public class Denominacion
    {
        /// <summary>
        /// Codigo de la forma de pago de la denominación
        /// </summary>
        [DataMember(Name = "codigoFormaPago")]
        public string CodigoFormaPago { get; set; }

        /// <summary>
        /// Cantidad de la denominación
        /// </summary>
        [DataMember(Name = "cantidad")]
        public int Cantidad { get; set; }

        /// <summary>
        /// Valor de la denominación
        /// </summary>
        [DataMember(Name = "valorDenominacion")]
        public decimal ValorDenominacion { get; set; }

        /// <summary>
        /// Texto presentable de la denominación
        /// </summary>
        [DataMember(Name = "textoDenominacion")]
        public string TextoDenominacion { get; set; }

    }
}