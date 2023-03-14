using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Business;
using Milano.BackEnd.Business.General;
namespace Milano.BackEnd.UniTest
{
    /// <summary>
    /// Clase de prueba búsqueda de productos de capa negocio
    /// </summary>
    [TestClass]
    public class ProductsBusinessTest
    {

        /// <summary>
        /// Método que ejecuta una búsqueda de producto
        /// </summary>
        [TestMethod]
        public void Search()
        {
            TokenDto token = new TokenDto(3, 3215);            
            ProductsRequest product = new ProductsRequest();
            product.Sku  = "";
          
            product.CodeProvider = 1;
            product.CodigoEstilo = "";
            product.CodeDepartment = 0;
            product.Description = "";
            var result = new ProductsBusiness(token).SearchAdvance (product);
            Assert.IsNotNull(result.Data);
        }
    }
}
