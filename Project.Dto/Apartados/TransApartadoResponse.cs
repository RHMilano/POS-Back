using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Milano.BackEnd.Dto.Sales;

namespace Milano.BackEnd.Dto.Apartados
{
    /// <summary>
    /// Clase que devuelve el resultado de una operacion en base de datos
    /// </summary>
    [DataContract]
    public class TransApartadoResponse
    {

        /// <summary>
        /// Constructor por default
        /// </summary>
        public TransApartadoResponse()
        {
            DescuentosPromocionalesAplicadosLinea = new DescuentoPromocionalLinea[0];
            DescuentosPromocionalesPosiblesLinea = new DescuentoPromocionalLinea[0];
        }

        /// <summary>
        /// Codigo del resultado
        /// </summary>
        [DataMember(Name = "codeNumber")]
        public string CodeNumber { get; set; }

        /// <summary>
        /// Descripcion del codigo del resultado
        /// </summary>
        [DataMember(Name = "codeDescription")]
        public string CodeDescription { get; set; }

        /// <summary>
        /// Folio de la venta que genera el apartado
        /// </summary>
        [DataMember(Name = "folioVenta")]
        public string FolioVenta { get; set; }

        /// <summary>
        /// Folio de la Nota de Crédito que genera la cancelación del apartado
        /// </summary>
        [DataMember(Name = "folioNotaCreditoGenerada")]
        public string FolioNotaCreditoGenerada { get; set; }

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

        /// <summary>
        /// Información Asociada Retiro de Efectivo
        /// </summary>
        [DataMember(Name = "informacionAsociadaRetiroEfectivo")]
        public InformacionAsociadaRetiroEfectivo informacionAsociadaRetiroEfectivo { get; set; }
    }
}
