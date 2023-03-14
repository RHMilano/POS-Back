using Milano.BackEnd.Dto.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.General
{

    /// <summary>
    /// Clase que engloba la configuracion de impuestos
    /// </summary>
    [DataContract]
    public class ConfigGeneralesCajaTiendaImpuesto
    {

        /// <summary>
        /// Código del Impuesto
        /// </summary>
        [DataMember(Name = "codigoImpuesto")]
        public string CodigoImpuesto { get; set; }


        /// <summary>
        /// Porcentaje del Impuesto
        /// </summary>
        [DataMember(Name = "porcentajeImpuesto")]
        public decimal PorcentajeImpuesto { get; set; }

    }
}