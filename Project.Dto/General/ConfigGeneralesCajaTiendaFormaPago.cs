using Milano.BackEnd.Dto.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.General
{

    /// <summary>
    /// Clase que engloba la configuracion de la forma de pago
    /// </summary>
    [DataContract]
    public class ConfigGeneralesCajaTiendaFormaPago
    {

        /// <summary>
        /// Identificador de la forma de pago
        /// </summary>
        [DataMember(Name = "identificadorFormaPago")]
        public string IdentificadorFormaPago { get; set; }

        /// <summary>
        /// Código de la forma de pago
        /// </summary>
        [DataMember(Name = "codigoFormaPago")]
        public string CodigoFormaPago { get; set; }

        /// <summary>
        /// Descripción de la forma de pago
        /// </summary>
        [DataMember(Name = "descripcionFormaPago")]
        public string DescripcionFormaPago { get; set; }

    }
}