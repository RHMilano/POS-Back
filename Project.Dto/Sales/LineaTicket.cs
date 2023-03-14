using Milano.BackEnd.Dto.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// Clase que representa una línea de artículo
    /// </summary>
    [DataContract]
    public class LineaTicket
    {

        /// <summary>
		/// Constructor por default
		/// </summary>
		public LineaTicket()
        {
            DescuentosPromocionalesAplicadosLinea = new DescuentoPromocionalLinea[0];
            DescuentosPromocionalesPosiblesLinea = new DescuentoPromocionalLinea[0];
        }

        /// <summary>
        /// Bandera que indica si la linea pertenece a la venta original
        /// </summary>
        [DataMember(Name = "perteneceVentaOriginal")]
        public Boolean PerteneceVentaOriginal { get; set; }

        /// <summary>
        /// Número secuencia en el Ticket
        /// </summary>
        [DataMember(Name = "secuencia")]
        public int Secuencia { get; set; }

        /// <summary>
        /// Artículo de ésta línea
        /// </summary>
        [DataMember(Name = "articulo")]
        public Articulo Articulo { get; set; }

        /// <summary>
        /// Cantidad de artículos vendidos
        /// </summary>
        [DataMember(Name = "cantidadVendida")]
        public int CantidadVendida { get; set; }

        /// <summary>
        /// Cantidad de artículos devueltos
        /// </summary>
        [DataMember(Name = "cantidadDevuelta")]
        public int CantidadDevuelta { get; set; }

        /// <summary>
        /// Descuento directo línea
        /// </summary>
        [DataMember(Name = "descuentoDirectoLinea")]
        public DescuentoDirectoLinea DescuentoDirectoLinea { get; set; }

        /// <summary>
        /// Descuentos promocionales aplicados por linea de venta
        /// </summary>
        [DataMember(Name = "descuentosPromocionalesAplicadosLinea")]
        public DescuentoPromocionalLinea[] DescuentosPromocionalesAplicadosLinea { get; set; }

        /// <summary>
        /// Descuentos promocionales posibles de aplicar por linea de venta
        /// </summary>
        [DataMember(Name = "descuentosPromocionalesPosiblesLinea")]
        public DescuentoPromocionalLinea[] DescuentosPromocionalesPosiblesLinea { get; set; }

        /// <summary>
        /// Total venta linea descuentos
        /// </summary>
        [DataMember(Name = "importeVentaLineaDescuentos")]
        public decimal ImporteVentaLineaDescuentos { get; set; }

        /// <summary>
        /// Total venta linea Subtotal (Sin impuestos)
        /// </summary>
        [DataMember(Name = "importeVentaLineaBruto")]
        public decimal ImporteVentaLineaBruto { get; set; }

        /// <summary>
        /// Total devolución linea Subtotal (Sin impuestos)
        /// </summary>
        [DataMember(Name = "importeDevolucionLineaBruto")]
        public decimal ImporteDevolucionLineaBruto { get; set; }

        /// <summary>
        /// Total devolución linea impuestos de la venta
        /// </summary>
        [DataMember(Name = "importeDevolucionLineaImpuestos1")]
        public decimal ImporteDevolucionLineaImpuestos1 { get; set; }

        /// <summary>
        /// Total devolución linea impuestos de la venta
        /// </summary>
        [DataMember(Name = "importeDevolucionLineaImpuestos2")]
        public decimal ImporteDevolucionLineaImpuestos2 { get; set; }

        /// <summary>
        /// Total linea impuestos de la venta
        /// </summary>
        [DataMember(Name = "importeVentaLineaImpuestos1")]
        public decimal ImporteVentaLineaImpuestos1 { get; set; }

        /// <summary>
        /// Total linea impuestos de la venta
        /// </summary>
        [DataMember(Name = "importeVentaLineaImpuestos2")]
        public decimal ImporteVentaLineaImpuestos2 { get; set; }

        /// <summary>
        /// Total venta linea con Impuestos
        /// </summary>
        [DataMember(Name = "importeVentaLineaNeto")]
        public decimal ImporteVentaLineaNeto { get; set; }

        /// <summary>
        /// Total devolución linea con Impuestos
        /// </summary>
        [DataMember(Name = "importeDevolucionLineaNeto")]
        public decimal ImporteDevolucionLineaNeto { get; set; }

        /// <summary>
        /// Total impuestos devolución linea
        /// </summary>
        [DataMember(Name = "importeDevolucionLineaImpuestos")]
        public decimal ImporteDevolucionLineaImpuestos { get; set; }

        /// <summary>
        /// Código Detalle Tipo de Transacción Venta
        /// </summary>
        [DataMember(Name = "codigoTipoDetalleVenta")]
        public string TipoDetalleVenta { get; set; }

        /// <summary>
        /// Información asociada de la cabecera de Venta
        /// </summary>
        [DataMember(Name = "cabeceraVentaAsociada")]
        public CabeceraVentaRequest cabeceraVentaRequest { get; set; }

    }
}
