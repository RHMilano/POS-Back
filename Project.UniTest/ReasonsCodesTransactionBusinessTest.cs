using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Business;

namespace Milano.BackEnd.UniTest
{
    /// <summary>
    /// Clase de prueba oobtener el catálogo de razones de capa negocio
    /// </summary>
    [TestClass]
    public class ReasonsCodesTransactionBusinessTest
    {

        /// <summary>
        /// Método que regresa catálogo de razones en base a parámetro del tipo
        /// </summary>
        [TestMethod]
        public void SearchReasonsCodesTransactionBusiness()
        {
            ReasonsCodesTransactionBusiness reasonsCodesTransactionBusiness = new ReasonsCodesTransactionBusiness();
            ReasonsCodesTransactionRequest reasonsCodesTransactionRequest = new ReasonsCodesTransactionRequest();
            reasonsCodesTransactionRequest.CodigoTipoRazonMMS = "CSH";
            var result = reasonsCodesTransactionBusiness.CatalogoReasonsCodesTransaction(reasonsCodesTransactionRequest);
            Assert.IsTrue(result.Data.Length > 0);
        }
    }
}
