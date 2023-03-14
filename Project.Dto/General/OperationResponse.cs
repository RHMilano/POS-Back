using Milano.BackEnd.Dto.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{

    /// <summary>
    /// Clase que devuelve el resultado de una operacion en base de datos
    /// </summary>
    [DataContract]
    public class OperationResponse
    {

        /// <summary>
        /// Constructor por default
        /// </summary>
        public OperationResponse()
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
        /// Identificador de la transaccion realizada
        /// </summary>
        [DataMember(Name = "transaccion")]
        public int Transaccion { get; set; }

        /// <summary>
        /// Identificador de la transaccion realizada
        /// </summary>
        [DataMember(Name = "codigoTipoTrxCab")]
        public string CodigoTipoTrxCab { get; set; }

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
