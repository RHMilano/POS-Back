using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.General
{
    /// <summary>
    /// Resultado de búsqueda de productos
    /// </summary>
    [DataContract]
    public class ProductsFindResponse
    {
        /// <summary>
        /// Numero total de registros encontrados
        /// </summary>
        [DataMember(Name = "numeroRegistros")]
        public int NumeroRegistros { get; set; }

        /// <summary>
        /// Productos encontrados
        /// </summary>
        [DataMember(Name = "productos")]
        public ProductsResponse[] Products { get; set; }

    }
}
