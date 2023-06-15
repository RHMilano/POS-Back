using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;

namespace Milano.BackEnd.Dto.Lealtad
{
    /// <summary>
    /// Clase DTO para envio de parametros AcumularPuntosDescuentosRequest, del programa de lealtad
    /// </summary>
    [DataContract]
    public class AcumularPuntosDescuentosRequest
    {

        /// <summary>
        /// Fecha
        /// </summary>
        [DataMember(Name = "sFecha")]
        public string ssFecha { get; set; }

        /// <summary>
        /// Codigo cliente
        /// </summary>
        [DataMember(Name = "iCodigoCliente")]
        public int iiCodigoCliente { get; set; }

        /// <summary>
        /// Numero de tienda 
        /// </summary>
        [DataMember(Name = "iCodigoTienda")]
        public int iiCodigoTienda { get; set; }

        /// <summary>
        /// Numero de caja
        /// </summary>
        [DataMember(Name = "iCodigoCaja")]
        public int iiCodigoCaja { get; set; }

        /// <summary>
        /// Numero de empleado de la tienda
        /// </summary>
        [DataMember(Name = "iCodigoEmpleado")]
        public int iiCodigoEmpleado { get; set; }

        /// <summary>
        /// Folio de venta
        /// </summary>
        [DataMember(Name = "sFolioVenta")]
        public string ssFolioVenta { get; set; }

        /// <summary>
        /// Codigo de promocion
        /// </summary>
        [DataMember(Name = "iCodigoPromocion")]
        public int iiCodigoPromocion { get; set; }

        /// <summary>
        /// Monto de venta sin IVA
        /// </summary>
        [DataMember(Name = "dVentaSinIVA")]
        public double ddVentaSinIVA { get; set; }

        /// <summary>
        /// Monto IVA
        /// </summary>
        [DataMember(Name = "dIVA")]
        public double ddIVA { get; set; }

        /// <summary>
        /// Numero de transaccion
        /// </summary>
        [DataMember(Name = "iTransaccion")]
        public int iiTransaccion { get; set; }

        /// <summary>
        /// Puntos acumulados
        /// </summary>
        [DataMember(Name = "dPuntosAcumulados")]
        public double ddPuntosAcumulados { get; set; }

        /// <summary>
        /// Importe de descuento
        /// </summary>
        [DataMember(Name = "dImporteDescuento")]
        public double ddImporteDescuento { get; set; }

        /// <summary>
        ///  Tipo de puntos acumulados en base a las reglas de lealtad
        /// </summary>
        [DataMember(Name = "iCodigoTipoPuntos")]
        public int iiCodigoTipoPuntos { get; set; }



    }
}
