using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Sales
{

    /// <summary>
    /// Clase de peticion de cabecera de venta
    /// </summary>
    [DataContract]
    public class CabeceraVentaRequest
    {

        /// <summary>
        /// Folio de la devolución
        /// </summary>
        [DataMember(Name = "folioDevolucion")]
        public string FolioDevolucion { get; set; }

        /// <summary>
        /// Folio de la venta original
        /// </summary>
        [DataMember(Name = "folioVentaOriginal")]
        public string FolioVentaOriginal { get; set; }

        /// <summary>
        /// Total venta con impuestos original
        /// </summary>
        [DataMember(Name = "importeVentaNetoOriginal")]
        public decimal ImporteVentaNetoOriginal { get; set; }

        /// <summary>
        /// Saldo a favor de la devolución
        /// </summary>
        [DataMember(Name = "devolucionSaldoAFavor")]
        public decimal DevolucionSaldoAFavor { get; set; }

        /// <summary>
        /// Bandera que indica si el cliente tiene un saldo pendiente por pagar
        /// </summary>
        [DataMember(Name = "clienteTieneSaldoPendientePagar")]
        public bool ClienteTieneSaldoPendientePagar { get; set; }

        /// <summary>
        /// Impote total de la devolución
        /// </summary>
        [DataMember(Name = "importeDevolucionTotal")]
        public decimal ImporteDevolucionTotal { get; set; }

        /// <summary>
        /// Folio de operación asignado
        /// </summary>
        [DataMember(Name = "folioOperacion")]
        public string FolioOperacion { get; set; }

        /// <summary>
        /// Codigo cliente
        /// </summary>
        [DataMember(Name = "codigoCliente")]
        public Int64 CodigoCliente { get; set; }

        /// <summary>
        /// Código de mayorista para venta
        /// </summary>
        [DataMember(Name = "codigoMayorista")]
        public int CodigoMayorista { get; set; }

        /// <summary>
        /// Código de mayorista para pago de credito 
        /// </summary>
        [DataMember(Name = "codigoMayoristaCredito")]
        public int CodigoMayoristaCredito { get; set; }

        /// <summary>
        /// Código de empleado vendedor
        /// </summary>
        [DataMember(Name = "codigoEmpleadoVendedor")]
        public int CodigoEmpleadoVendedor { get; set; }

        /// <summary>
        /// Número de nómina del empleado en caso de una Venta Empleado
        /// </summary>
        [DataMember(Name = "numeroNominaVentaEmpleado")]
        public int NumeroNominaVentaEmpleado { get; set; }

        /// <summary>
        /// Código Cabecera Tipo de Transacción Venta
        /// </summary>
        [DataMember(Name = "codigoTipoCabeceraVenta")]
        public string TipoCabeceraVenta { get; set; }

        /// <summary>
        /// Total descuentos de la venta
        /// </summary>
        [DataMember(Name = "importeVentaDescuentos")]
        public decimal ImporteVentaDescuentos { get; set; }

        /// <summary>
        /// Total devolución Subtotal (Sin impuestos)
        /// </summary>
        [DataMember(Name = "importeDevolucionBruto")]
        public decimal ImporteDevolucionBruto { get; set; }

        /// <summary>
        /// Total impuestos de la devolución
        /// </summary>
        [DataMember(Name = "importeDevolucionImpuestos")]
        public decimal ImporteDevolucionImpuestos { get; set; }

        /// <summary>
        /// Total devolucion con impuestos
        /// </summary>
        [DataMember(Name = "importeDevolucionNeto")]
        public decimal ImporteDevolucionNeto { get; set; }

        /// <summary>
        /// Total venta Subtotal (Sin impuestos)
        /// </summary>
        [DataMember(Name = "importeVentaBruto")]
        public decimal ImporteVentaBruto { get; set; }

        /// <summary>
        /// Total impuestos de la venta
        /// </summary>
        [DataMember(Name = "importeVentaImpuestos")]
        public decimal ImporteVentaImpuestos { get; set; }

        /// <summary>
        /// Total venta con impuestos
        /// </summary>
        [DataMember(Name = "importeVentaNeto")]
        public decimal ImporteVentaNeto { get; set; }

        /// <summary>
        /// Nombre de membresia : nombre de empleado, mayorista
        /// </summary>
        [DataMember(Name = "nombreMembresia")]
        public string NombreMembresia { get; set; }

    }
}
