using Milano.BackEnd.Dto.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.FormasPago
{

    /// <summary>
    /// Clase que devuelve el resultado de una operacion de redención de cupón
    /// </summary>
    [DataContract]
    public class ProcesarMovimientoRedencionCuponResponse
    {

        /// <summary>
        /// Codigo del resultado
        /// </summary>
        [DataMember(Name = "codeNumber")]
        public string CodeNumber { get; set; }

        /// <summary>
        /// Descripcion del codigo del resultado
        /// </summary>
        [DataMember(Name = "codeDescription")]
        public string CodeDescription { get; set; }

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
        /// Saldo aplicado a la venta
        /// </summary>
        [DataMember(Name = "saldoAplicado")]
        public decimal SaldoAplicado { get; set; }

    }
}
