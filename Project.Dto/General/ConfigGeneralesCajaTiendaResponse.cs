using Milano.BackEnd.Dto.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.General
{
    /// <summary>
    /// Clase de respuesta del configuraciones generales de la caja y tienda
    /// </summary>
    [DataContract]
    public class ConfigGeneralesCajaTiendaResponse
    {

        /// <summary>
        /// Indica si el POS abre en Modo Consulta
        /// </summary>
        [DataMember(Name = "posModoConsulta")]
        public bool PosModoConsulta { get; set; }

        /// <summary>
        /// Color de venta Regular
        /// </summary>
        [DataMember(Name = "colorVentaRegular")]
        public string ColorVentaRegular { get; set; }

        /// <summary>
        /// Color venta Empleado
        /// </summary>
        [DataMember(Name = "colorVentaEmpleaado")]
        public string ColorVentaEmpleaado { get; set; }

        /// <summary>
        /// Color venta Mayorista
        /// </summary>
        [DataMember(Name = "colorVentaMayorista")]
        public string ColorVentaMayorista { get; set; }

        /// <summary>
        /// Color venta Devoluciones
        /// </summary>
        [DataMember(Name = "colorDevoluciones")]
        public string ColorDevoluciones { get; set; }

        /// <summary>
        /// Color formas de pago
        /// </summary>
        [DataMember(Name = "colorFormasDePago")]
        public string ColorFormasDePago { get; set; }

        /// <summary>
        /// Ruta del log de las transacciones
        /// </summary>
        [DataMember(Name = "rutaLogTransacciones")]
        public string RutaLogTransacciones { get; set; }

        /// <summary>
        /// Si pagan con vales, el monto máximo de cambio en efectivo que puede darse al cliente
        /// </summary>
        [DataMember(Name = "montoMaximoCambioVales")]
        public decimal MontoMaximoCambioVales { get; set; }

        /// <summary>
        /// Sku que se utiliza para agregar una linea al ticket con el XX% adicional por pago con vale mayorista
        /// </summary>
        [DataMember(Name = "skuPagoConValeMayorista")]
        public int SkuPagoConValeMayorista { get; set; }

        /// <summary>
        /// Sku que se utiliza para agregar una linea al ticket con el XX% adicional por comisión de pago de servicio
        /// </summary>
        [DataMember(Name = "skuComisionPagoServicios")]
        public int SkuComisionPagoServicios { get; set; }

        /// <summary>
        /// Porcentaje que se utiliza para agregar una linea al ticket con el XX% adicional por pago con vale mayorista
        /// </summary>
        [DataMember(Name = "porcentajePagoConValeMayorista")]
        public int PorcentajePagoConValeMayorista { get; set; }

        /// <summary>
        /// Sku tarjeta de regalo
        /// </summary>
        [DataMember(Name = "skuTarjetaRegalo")]
        public int SkuTarjetaRegalo { get; set; }

        /// <summary>
        /// Monto mínimo que  debe curbirse al generar un nuevo apartado
        /// </summary>
        [DataMember(Name = "montoMinimoAbonoApartado")]
        public decimal MontoMinimoAbonoApartado { get; set; }

        /// <summary>
        /// Porcentaje mínimo que  debe curbirse al generar un nuevo apartado
        /// </summary>
        [DataMember(Name = "montoMinimoPorcentajeApartado")]
        public decimal MontoMinimoPorcentajeApartado { get; set; }

        /// <summary>
        /// Porcentaje máximo descuento directo
        /// </summary>
        [DataMember(Name = "porcentajeMaximoDescuentoDirecto")]
        public decimal PorcentajeMaximoDescuentoDirecto { get; set; }

        /// <summary>
        /// Configuración de la botonera del POS
        /// </summary>
        [DataMember(Name = "configuracionBotonera")]
        public ConfiguracionBotonera ConfiguracionBotonera { get; set; }

        /// <summary>
        /// Sku para tarjeta de credito melody milano
        /// </summary>
        [DataMember(Name = "skuPagoTCMM")]
        public int SkuPagoTCMM { get; set; }

        /// <summary>
        /// Sku pago Mayorista
        /// </summary>
        [DataMember(Name = "skuPagoMayorista")]
        public int SkuPagoMayorista { get; set; }

        /// <summary>
        /// Sku pago Mayorista
        /// </summary>
        [DataMember(Name = "skuPagoComisionMayorista")]
        public int SkuPagoComisionMayorista { get; set; }

        /// <summary>
        /// Información asociada de Impuestos
        /// </summary>
        [DataMember(Name = "informacionAsociadaImpuestos")]
        public ConfigGeneralesCajaTiendaImpuesto InformacionAsociadaImpuestos { get; set; }

        /// <summary>
        /// Información catálogo de recursos
        /// </summary>
        [DataMember(Name = "informacionCatalogoRecursos")]
        public ConfigGeneralesRecurso[] InformacionCatalogoRecursos { get; set; }

        /// <summary>
        /// Version de los parches Milano
        /// </summary>
        [DataMember(Name = "versionPOS")]
        public string versionPOS { get; set; } // OCG

    }
}