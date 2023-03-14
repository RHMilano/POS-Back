using Milano.BackEnd.Dto.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Sales
{

    /// <summary>
    /// Clase de peticion de totalización de venta
    /// </summary>
    [DataContract]
    public class TotalizarVentaRequest
    {

        /// <summary>
        /// Número total de lineas en el ticket 
        /// </summary>
        [DataMember(Name = "secuenciaActual")]
        public int SecuenciaActual { get; set; }

        /// <summary>
        /// Información asociada de la cabecera de Venta
        /// </summary>
        [DataMember(Name = "cabeceraVentaAsociada")]
        public CabeceraVentaRequest cabeceraVentaRequest { get; set; }

    }
}
