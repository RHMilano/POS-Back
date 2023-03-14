using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milano.BackEnd.Dto.Configuracion;
using Milano.BackEnd.Repository.General;

namespace Milano.BackEnd.UniTest
{
    /// <summary>
    /// /
    /// </summary>
    [TestClass]
    public class GetConfigGeneralesCajaTest
    {
        [TestMethod]
        public void GetBotonCnfgTest()
        {
            var cnfbtn = new ConfigGeneralesCajaTiendaRepository();
            var result = cnfbtn.GetBotonCnfg(3, 3021);
            Assert.IsTrue(result.ConfiguracionBotones != null);

        }

        [TestMethod]
        public void GetCnfgGralTest()
		{
			var cnfbtn = new ConfigGeneralesCajaTiendaRepository();
			var result = cnfbtn.GetConfig(3, 3021, 1717);
			Assert.IsTrue(result != null);

        }
    }
}
