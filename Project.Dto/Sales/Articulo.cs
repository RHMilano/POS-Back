using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// Clase que representa un artículo
    /// </summary>
    [DataContract]
    public class Articulo
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public Articulo()
        {
            this.Clase = "";
            this.CodigoImpuesto1 = "";
            this.CodigoImpuesto2 = "";
            this.Depto = "";
            this.DigitoVerificadorArticulo = new DigitoVerificadorArticulo();
            this.Estilo = "";
            this.InformacionProveedorExternoAsociadaPS = new InformacionProveedorExternoAsociadaPS();
            this.InformacionProveedorExternoTA = new InformacionProveedorExternoTA();
            this.RutaImagenLocal = "";
            this.RutaImagenRemota = "";
            this.SubClase = "";
            this.SubDepartamento = "";
            this.Upc = "";
            this.InformacionTarjetaRegalo = new InformacionTarjetaRegalo();
            this.EsTarjetaRegalo = false;
            this.DescripcionProveedor = "";
        }

        /// <summary>
        /// Bandera que indica que si es tarjeta regalo
        /// </summary>
        [DataMember(Name = "esTarjetaRegalo")]
        public bool EsTarjetaRegalo { get; set; }

        /// <summary>
        /// SKU del producto
        /// </summary>
        [DataMember(Name = "sku")]
        public int Sku { get; set; }

        /// <summary>
        /// UPC del producto
        /// </summary>
        [DataMember(Name = "upc")]
        public string Upc { get; set; }

        /// <summary>
        /// Código marca del producto
        /// </summary>
        [DataMember(Name = "codigoMarca")]
        public int CodigoMarca { get; set; }

        /// <summary>
        /// Código departamento del producto
        /// </summary>
        [DataMember(Name = "codigoDepto")]
        public int CodigoDepto { get; set; }

        /// <summary>
        /// Descripción del departamento
        /// </summary>
        [DataMember(Name = "depto")]
        public string Depto { get; set; }

        /// <summary>
        /// Código subdepartamento del producto
        /// </summary>
        [DataMember(Name = "codigoSubDepto")]
        public int CodigoSubDepartamento { get; set; }

        /// <summary>
        /// Descripción del subdepartamento
        /// </summary>
        [DataMember(Name = "subDepartamento")]
        public string SubDepartamento { get; set; }

        /// <summary>
        /// Código clase del producto
        /// </summary>
        [DataMember(Name = "codigoClase")]
        public int CodigoClase { get; set; }

        /// <summary>
        /// Descripción de clase del producto
        /// </summary>
        [DataMember(Name = "clase")]
        public string Clase { get; set; }

        /// <summary>
        /// Código subclase del producto
        /// </summary>
        [DataMember(Name = "codigoSubClase")]
        public int CodigoSubClase { get; set; }

        /// <summary>
        /// Descripción de subclase del producto
        /// </summary>
        [DataMember(Name = "subClase")]
        public string SubClase { get; set; }

        /// <summary>
        /// Descripcion de estilo del producto
        /// </summary>
        [DataMember(Name = "estilo")]
        public string Estilo { get; set; }

        /// <summary>
        /// Precio del producto con impuestos
        /// </summary>
        [DataMember(Name = "precioConImpuestos")]
        public decimal PrecioConImpuestos { get; set; }

        /// <summary>
        /// Precio del producto con impuestos cambiado desde UI
        /// </summary>
        [DataMember(Name = "precioCambiadoConImpuestos")]
        public decimal PrecioCambiadoConImpuestos { get; set; }

        /// <summary>
        /// Imagen local del producto
        /// </summary>
        [DataMember(Name = "rutaImagenLocal")]
        public string RutaImagenLocal { get; set; }

        /// <summary>
        /// Imagen remota del producto
        /// </summary>
        [DataMember(Name = "rutaImagenRemota")]
        public string RutaImagenRemota { get; set; }

        /// <summary>
        /// Impuesto 1 del producto
        /// </summary>
        [DataMember(Name = "impuesto1")]
        public decimal Impuesto1 { get; set; }

        /// <summary>
        /// Impuesto 2 del producto
        /// </summary>
        [DataMember(Name = "impuesto2")]
        public decimal Impuesto2 { get; set; }

        /// <summary>
        /// Impuesto 1 del producto con precio cambiado desde UI
        /// </summary>
        [DataMember(Name = "precioCambiadoImpuesto1")]
        public decimal precioCambiadoImpuesto1 { get; set; }

        /// <summary>
        /// Impuesto 2 del producto con precio cambiado desde UI
        /// </summary>
        [DataMember(Name = "precioCambiadoImpuesto2")]
        public decimal precioCambiadoImpuesto2 { get; set; }

        /// <summary>
        /// Tasas Impuesto 1 del producto
        /// </summary>
        [DataMember(Name = "tasaImpuesto1")]
        public decimal TasaImpuesto1 { get; set; }

        /// <summary>
        /// Tasas Impuesto 2 del producto
        /// </summary>
        [DataMember(Name = "tasaImpuesto2")]
        public decimal TasaImpuesto2 { get; set; }

        /// <summary>
        /// Codigo del impuesto 1
        /// </summary>
        [DataMember(Name = "codigoImpuesto1")]
        public string CodigoImpuesto1 { get; set; }

        /// <summary>
        /// Codigo del impuesto 2
        /// </summary>
        [DataMember(Name = "codigoImpuesto2")]
        public string CodigoImpuesto2 { get; set; }

        /// <summary>
        /// Digito verificador asociado
        /// </summary>
        [DataMember(Name = "digitoVerificadorArticulo")]
        public DigitoVerificadorArticulo DigitoVerificadorArticulo { get; set; }

        /// <summary>
        /// Información asociada a un proveedor externo de tiempo aire
        /// </summary>
        [DataMember(Name = "informacionProveedorExternoTA")]
        public InformacionProveedorExternoTA InformacionProveedorExternoTA { get; set; }

        /// <summary>
        /// Informacion asociada a un proveedor externo de pago se servicios
        /// </summary>
        [DataMember(Name = "informacionProveedorExternoAsociadaPS")]
        public InformacionProveedorExternoAsociadaPS InformacionProveedorExternoAsociadaPS { get; set; }

        /// <summary>
        /// Informacion asociada a la tarjeta de regalo
        /// </summary>
        [DataMember(Name = "informacionTarjetaRegalo")]
        public InformacionTarjetaRegalo InformacionTarjetaRegalo { get; set; }

        /// <summary>
        /// Informacion asociada a la transacción pago de una TCMM
        /// </summary>
        [DataMember(Name = "informacionPagoTCMM")]
        public InformacionPagoTCMM InformacionPagoTCMM { get; set; }

        /// <summary>
        /// Descripción Proveedor
        /// </summary>
        [DataMember(Name = "descripcionProveedor")]
        public String DescripcionProveedor { get; set; }

    }
}
