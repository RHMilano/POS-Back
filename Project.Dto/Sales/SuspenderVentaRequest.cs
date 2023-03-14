using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Sales
{

    /// <summary>
    /// Clase de peticion de suspensión de venta
    /// </summary>
    [DataContract]
    public class SuspenderVentaRequest
    {

        /// <summary>
        /// Información asociada de la cabecera de Venta
        /// </summary>
        [DataMember(Name = "cabeceraVentaAsociada")]
        public CabeceraVentaRequest cabeceraVentaRequest { get; set; }

    }
}
