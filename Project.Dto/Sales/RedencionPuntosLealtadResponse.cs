using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Milano.BackEnd.Dto.Sales;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// Clase para finalizar una transaccion de venta
    /// </summary>

    [DataContract]
    public class RedencionPuntosLealtadResponse
    {

        /// <summary>
        /// Codigo de Barras
        /// </summary>
        [DataMember(Name = "sMensaje")]
        public string ssMensaje { get; set; }

        /// <summary>
        /// Monto de la compra
        /// </summary>
        [DataMember(Name = "bError")]
        public Boolean bbError { get; set; }

        /// <summary>
        /// Monto de la compra
        /// </summary>
        [DataMember(Name = "sSesion")]
        public string ssSesion { get; set; }

    }
}
