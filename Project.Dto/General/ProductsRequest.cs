using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;

namespace Milano.BackEnd.Dto
{

    /// <summary>
    /// Clase de Productos para petición
    /// </summary>
    [DataContract]
    public class ProductsRequest
    {
        /// <summary>
        /// SKU del producto
        /// </summary>
        [DataMember(Name = "sku")]
        public string Sku { get; set; }

      

        /// <summary>
        ///  Codigo de la tienda
        /// </summary>
        [DataMember(Name = "codeStore")]
        public decimal CodeStore { get; set; }

        /// <summary>
        ///  Estilo del producto
        /// </summary>
        [DataMember(Name = "codigoEstilo")]
        public string CodigoEstilo { get; set; }

        /// <summary>
        /// Proveedor del producto
        /// </summary>
        [DataMember(Name = "codeProvider")]
        public int CodeProvider { get; set; }

        /// <summary>
        /// Departamento del producto
        /// </summary>
        [DataMember(Name = "codeDepartment")]
        public int CodeDepartment { get; set; }

        /// <summary>
        /// SubDepartamento del producto
        /// </summary>
        [DataMember(Name = "codeSubDepartment")]
        public int CodeSubDepartment { get; set; }

        /// <summary>
        /// Clase del producto
        /// </summary>
        [DataMember(Name = "codeClass")]
        public int CodeClass { get; set; }

        /// <summary>
        /// SubClase del producto
        /// </summary>
        [DataMember(Name = "codeSubClass")]
        public int CodeSubClass { get; set; }

        /// <summary>
        /// Descripción del producto
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }

           /// <summary>
        /// Numero de pagina 
        /// </summary>
        [DataMember(Name = "numeroPagina")]
        public int NumeroPagina { get; set; }

        /// <summary>
        /// Registros por pagina
        /// </summary>
        [DataMember(Name = "registrosPorPagina")]
        public int RegistrosPorPagina { get; set; }

    }
}
