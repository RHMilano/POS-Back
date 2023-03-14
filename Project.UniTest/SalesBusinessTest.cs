using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Business;
using Milano.BackEnd.Dto.Sales;

namespace Milano.BackEnd.UniTest
{
    /// <summary>
    /// Descripción resumida de SalesBusinessTest
    /// </summary>
    [TestClass]
    public class SalesBusinessTest
    {

        [TestMethod]
        public void BuscarVentaPorFolio()
        {
            TokenDto token = new TokenDto(3215, 3);
            VentaResponse reponse = new SalesBusiness(token).BuscarVentaPorFolio("5", 0);
            Assert.IsNotNull(reponse);
        }

    }
}
