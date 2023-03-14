using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milano.BackEnd.Business.General;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Dto;

namespace Milano.BackEnd.UniTest
{
    /// <summary>
    /// Prueba para las configuraciones generales de la caja y la tienda
    /// </summary>
    [TestClass]
    public class ConfigGeneralesCajaTiendaBusinessTest
    {

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void getConfig()
        {
            TokenDto token = new TokenDto(3, 3215);            
            ConfigGeneralesCajaTiendaBusiness configGeneralesCajaTiendaBusiness = new ConfigGeneralesCajaTiendaBusiness(token);
            Assert.IsNotNull(configGeneralesCajaTiendaBusiness.getConfigs());
        }

    }
}
