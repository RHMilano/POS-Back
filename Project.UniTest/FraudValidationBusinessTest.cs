using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milano.BackEnd.Business.Sales;
using Milano.BackEnd.Dto.Sales;
using Milano.BackEnd.Dto;

namespace Milano.BackEnd.UniTest
{
    /// <summary>
    /// Clase de prueba validación de fraude en tiempo aire de capa negocio
    /// </summary>
    [TestClass]
    public class FraudValidationBusinessTest
    {

        /// <summary>
        /// Método que ejecuta una validación de fraude TA
        /// </summary>
        [TestMethod]
        public void FraudValidationServiceTest()
        {
            TokenDto token = new TokenDto(3, 3215);            
            FraudValidationBusiness fraudValidationBusiness = new FraudValidationBusiness(token);
            FraudValidationRequest fraudValidation = new FraudValidationRequest();
            fraudValidation.NumeroTelefonico = 5584739202;
            var result = fraudValidationBusiness.FraudValidationTA(fraudValidation);
            Assert.IsTrue(result.Data.CodeNumber == "300");
        }
    }
}
