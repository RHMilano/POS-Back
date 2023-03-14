using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milano.BackEnd.Business.General;
using Milano.BackEnd.Dto.General;

namespace Milano.BackEnd.UniTest
{
    /// <summary>
    /// Descripción resumida de ConfigurationServiceBusinessTest
    /// </summary>
    [TestClass]
    public class InstallationServiceBusinessTest
    {
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void InsertConfigurationServiceTest()
        {
            InstallationServiceBusiness installationServiceBusiness = new InstallationServiceBusiness();
            ConfiguracionServiceRequest configurationService = new ConfiguracionServiceRequest();
            configurationService.CodigoCaja = 4;
            configurationService.IpEstaticaCaja = "192.192";
            configurationService.CodigoEmpleado = 4;
            var result = installationServiceBusiness.InsertConfigurationBox(configurationService);
            Assert.IsTrue(result.Data.CodeNumber == "200");
        }
    }
}
