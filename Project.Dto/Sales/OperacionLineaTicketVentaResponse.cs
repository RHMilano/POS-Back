using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Sales
{
    /// <summary>
    /// Clase que devuelve el resultado de una operacion sobre una linea de ticket de venta
    /// </summary>
    [DataContract]
    public class OperacionLineaTicketVentaResponse
    {

        public OperacionLineaTicketVentaResponse()
        {
            DescuentosPromocionalesAplicadosLinea = new DescuentoPromocionalLinea[0];
            DescuentosPromocionalesPosiblesLinea = new DescuentoPromocionalLinea[0];
        }

        /// <summary>
        /// Folio de operación asignado
        /// </summary>
        [DataMember(Name = "folioOperacion")]
        public string FolioOperacion { get; set; }

        /// <summary>
        /// Descuentos promocionales aplicadaas por linea de venta
        /// </summary>
        [DataMember(Name = "descuentosPromocionalesAplicadosLinea")]
        public DescuentoPromocionalLinea[] DescuentosPromocionalesAplicadosLinea { get; set; }

        /// <summary>
        /// Descuentos promocionales posibles de aplicar por linea de venta
        /// </summary>
        [DataMember(Name = "descuentosPromocionalesPosiblesLinea")]
        public DescuentoPromocionalLinea[] DescuentosPromocionalesPosiblesLinea { get; set; }

    }
}
