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
    public class RedencionPuntosLealtadRequest
    {

        /// <summary>
        /// Codigo de Barras
        /// </summary>
        [DataMember(Name = "scodigoBarras")]
        public string ssCodigoBarras { get; set; }

        /// <summary>
        /// Monto de la compra
        /// </summary>
        [DataMember(Name = "dMonto")]
        public Decimal ddMonto { get; set; }

        /// <summary>
        /// Monto de la compra
        /// </summary>
        [DataMember(Name = "iCodigoTienda")]
        public Int32 iiCodigoTienda { get; set; }

        /// <summary>
        /// Codigo Empleado
        /// </summary>
        [DataMember(Name = "iCodigoEmpleado")]
        public Int32 iiCodigoEmpleado { get; set; }

        /// <summary>
        /// Codigo Empleado
        /// </summary>
        [DataMember(Name = "iCodigoCaja")]
        public Int32 iiCodigoCaja { get; set; }


        /// <summary>
        /// Codigo Empleado
        /// </summary>
        [DataMember(Name = "iTransaccion")]
        public Int32 iiTransaccion { get; set; }


        /// <summary>
        /// Folio Venta
        /// </summary>
        [DataMember(Name = "sFolioVenta")]
        public string ssFolioVenta { get; set; }


    }
}
