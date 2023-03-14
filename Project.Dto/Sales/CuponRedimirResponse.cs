using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Sales
{
    /// <summary>
    /// Respuesta de redimir cupon y validaciones
    /// </summary>
    [DataContract]
    public class CuponRedimirResponse
    {

        /// <summary>
        /// Saldo a aplicar del cupon
        /// </summary>
        [DataMember(Name = "saldo")]
        public decimal Saldo { get; set; }

        /// <summary>
        /// Mensaje de respuesta de redencion del cupon
        /// </summary>
        [DataMember(Name = "mensajeRedencion")]
        public string MensajeRedencion { get; set; }

        /// <summary>
        /// Transacción asignada a la operación
        /// </summary>
        [DataMember(Name = "transaccion")]
        public int Transaccion { get; set; }

        /// <summary>
        /// Código del tipo de transacción cabecero
        /// </summary>
        [DataMember(Name = "codigoTipoTrxCab")]
        public string CodigoTipoTrxCab { get; set; }

        /// <summary>
        /// El cupon cumple con las reglas de redencion del mismo dia
        /// </summary>
        [DataMember(Name = "esRedimibleHoy")]
        public int EsRedimibleHoy { get; set; }

        /// <summary>
        /// Codigo de la promocion generada con el cupon
        /// </summary>
        [DataMember(Name = "codigoPromocion")]
        public int? CodigoPromocion { get; set; }

    }
}
