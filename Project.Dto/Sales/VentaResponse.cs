using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{

    /// <summary>
    /// DTO Venta por Folio
    /// </summary>
    [DataContract]
    public class VentaResponse
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
        /// Folio de venta
        /// </summary>
        [DataMember(Name = "folioVenta")]
        public string FolioVenta { get; set; }

        /// <summary>
        /// Estado de la venta
        /// </summary>
        [DataMember(Name = "estatus")]
        public string Estatus { get; set; }

        /// <summary>
        /// Consecutivo secuencia
        /// </summary>
        [DataMember(Name = "consecutivoSecuencia")]
        public int ConsecutivoSecuencia { get; set; }

        /// <summary>
        /// Código de mayorista
        /// </summary>
        [DataMember(Name = "codigoMayorista")]
        public int CodigoMayorista { get; set; }

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
        /// Artículos incluidos
        /// </summary>
        [DataMember(Name = "lineasTicket")]
        public LineaTicket[] Lineas { get; set; }

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
        /// Información de mayorista
        /// </summary>
        [DataMember(Name = "informacionMayorista")]
        public BusquedaMayoristaResponse InformacionMayorista { get; set; }

        /// <summary>
        /// Información de empleado de milano
        /// </summary>
        [DataMember(Name = "informacionEmpleadoMilano")]
        public EmpleadoMilanoResponse InformacionEmpleadoMilano { get; set; }

        /// <summary>
        /// Informacion Empleado vendedor
        /// </summary>
        [DataMember(Name = "informacionEmpleadoVendedor")]
        public EmployeeResponse InformacionEmpleadoVendedor { get; set; }

        /// <summary>
        /// Numero de transaccion de la venta
        /// </summary>
        [DataMember(Name = "numeroTransaccion")]
        public int NumeroTransaccion { get; set; }

    }
}
