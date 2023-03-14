using Milano.BackEnd.Dto.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{

    /// <summary>
    /// Respuesta a la búsqueda de un producto
    /// </summary>
    [DataContract]
    public class ProductsResponse
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductsResponse()
        {
            this.Articulo = new Articulo();
        }

        /// <summary>
        /// Artículo de ésta línea
        /// </summary>
        [DataMember(Name = "articulo")]
        public Articulo Articulo { get; set; }

    }

}
