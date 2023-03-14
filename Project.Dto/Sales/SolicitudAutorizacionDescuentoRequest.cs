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
    public class SolicitudAutorizacionDescuentoRequest
    {

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "codigoTienda")]
        public int CodigoTienda { get; set; }

        /// <summary>
        /// Folio de la venta
        /// </summary>
        [DataMember(Name = "folioVenta")]
        public string FolioVenta { get; set; }

        /// <summary>
        /// Precio 
        /// </summary>
        [DataMember(Name = "montoVenta")]
        public decimal MontoVenta { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "codigoCaja")]
        public int CodigoCaja { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "codigoRazonDescuento")]
        public int CodigoRazonDescuento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "opcionDescuento")]
        public int OpcionDescuento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "tipoDescuento")]
        public string TipoDescuento { get; set; }


        /// <summary>
        /// Porcentaje de Descuento
        /// </summary>
        [DataMember(Name = "montoDescuento")]
        public decimal MontoDescuento { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "linea")]
        public int Linea { get; set; }


        /// <summary>
        /// Codigo de Sku
        /// </summary>
        [DataMember(Name = "sku")]
        public int Sku { get; set; }

      

      



       

 


        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "consecutivoVenta")]
        public int ConsecutivoVenta { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "fecha")]
        public DateTime? Fecha { get; set; }


      

    }
}
