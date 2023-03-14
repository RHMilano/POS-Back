using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.InicioFinDia
{

    /// <summary>
    /// DTO que contiene la información del Cash Out de una Sucursal
    /// </summary>
    [DataContract]
    public class CompendioCashOut
    {

        /// <summary>
        /// Id del Compendio CashOut
        /// </summary>
        [DataMember(Name = "id")]
        public int Id { get; set; }

        /// <summary>
        /// Codigo de la tienda
        /// </summary>
        [DataMember(Name = "codigoTienda")]
        public int CodigoTienda { get; set; }

        /// <summary>
        /// Fecha del CashOut
        /// </summary>
        [DataMember(Name = "fecha")]
        public string Fecha { get; set; }

        /// <summary>
        /// Información asociada al Cash Out de de cada una de las cajas
        /// </summary>
        [DataMember(Name = "cashOutCaja")]
        public CashOutCaja[] CashOutCajas { get; set; }

    }
}
