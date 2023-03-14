using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// DTO de elemento tipo Input
    /// </summary>
    [DataContract]
    public class InputElement
    {

        /// <summary>
        /// Tipo del elemento input: Texto (1), Numero (2), Dinero (3), Password (4), Submit (5), Fecha (6)              
        /// </summary>
        [DataMember(Name = "tipoInput")]
        public string  TipoInput { get; set; }

        /// <summary>
        /// Valor mínimo del input
        /// </summary>
        [DataMember(Name = "valorMinimo")]
        public int ValorMinimo { get; set; }

        /// <summary>
        /// Valor máximo del input
        /// </summary>
        [DataMember(Name = "valorMaximo")]
        public int ValorMaximo { get; set; }

    }
}
