using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    [DataContract]
    public class ApartadosEncontradosResponse
    {
        /// <summary>
        /// Código Cabecera Tipo de Transacción de apartado
        /// </summary>
        [DataMember(Name = "codigoTipoCabeceraApartado")]
        public string TipoCabeceraApartado { get; set; }
        /// <summary>
        /// Código Detalle Tipo de Transacción apartado
        /// </summary>
        [DataMember(Name = "codigoTipoDetalleApartado")]
        public string TipoDetalleApartado { get; set; }

        /// <summary>
        /// Código de cliente
        /// </summary>
        [DataMember(Name = "codigoCliente")]
        public Int64 CodigoCliente { get; set; }

        /// Total venta Subtotal (Sin impuestos)
        /// <summary>
        [DataMember(Name = "importeApartadoBruto")]
        public decimal ImporteApartadoBruto { get; set; }
        /// <summary>
        /// Total impuestos de la venta
        /// </summary>
        [DataMember(Name = "importeApartadoImpuestos")]
        public decimal ImporteApartadoImpuestos { get; set; }
        /// <summary>
        /// Total venta con impuestos
        /// </summary>
        [DataMember(Name = "importeApartadoNeto")]
        public decimal ImporteApartadoNeto { get; set; }
        /// <summary>
        /// Código de empleado 
        /// </summary>
        [DataMember(Name = "codigoEmpleado")]
        public int CodigoEmpleado { get; set; }

        /// <summary>
        /// Código de empleado vendedor
        /// </summary>
        [DataMember(Name = "codigoEmpleadoVendedor")]
        public int CodigoEmpleadoVendedor { get; set; }

        /// <summary>
        /// Artículos incluidos
        /// </summary>
        [DataMember(Name = "articulos")]
        public LineaTicket[] Lineas { get; set; }
        /// <summary>
        /// Codigo de tienda
        /// </summary>
        [DataMember(Name = "codigoTienda")]
        public int CodigoTienda { get; set; }
        /// <summary>
        /// Folio apartado
        /// </summary>
        [DataMember(Name = "folioApartado")]
        public string FolioApartado { get; set; }
        /// <summary>
        /// Codigo de caja
        /// </summary>
        [DataMember(Name = "codigoCaja")]
        public int CodigoCaja { get; set; }
        /// <summary>
        /// Estatus
        /// </summary>
        [DataMember(Name = "estatus")]
        public string Estatus { get; set; }
        /// <summary>
        /// Importe pagado
        /// </summary>
        [DataMember(Name = "importePagado")]
        public decimal ImportePagado { get; set; }
        /// <summary>
        /// Saldo
        /// </summary>
        [DataMember(Name = "saldo")]
        public decimal Saldo { get; set; }
        /// <summary>
        /// Dias de vencimiento
        /// </summary>
        [DataMember(Name = "diasVencimiento")]
        public int DiasVencimiento { get; set; }
        /// <summary>
        /// Consecutivo secuencia formas de pago
        /// </summary>
        [DataMember(Name = "consecutivoSecuenciaFormasPago")]
        public int ConsecutivoSecuenciaFormasPago { get; set; }

        /// <summary>
        /// Nombre de cliente
        /// </summary>
        [DataMember(Name = "nombreCliente")]
        public string NombreCliente { get; set; }

        /// <summary>
        ///Telefono cliente
        /// </summary>
        [DataMember(Name = "telefonoCliente")]
        public string TelefonoCliente { get; set; }

        /// <summary>
        /// Fecha de vencimiento
        /// </summary>
        [DataMember(Name = "fechaVencimiento")]
        public string FechaVencimiento { get; set; }

        /// <summary>
        /// Fecha de cancelación
        /// </summary>
        [DataMember(Name = "fechaCancelacion")]
        public string FechaCancelacion { get; set; }
    }
}
