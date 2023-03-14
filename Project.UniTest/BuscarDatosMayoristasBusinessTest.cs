using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Business.General;

namespace Milano.BackEnd.UniTest
{
    /// <summary>
    /// Prueba para la búsqueda de Mayoristas
    /// </summary>
    [TestClass]
    public class BuscarDatosMayoristasBusinessTest
    {

        [TestMethod]
        public void searchByCode()
        {
            BuscarDatosMayoristasBusiness mb = new BuscarDatosMayoristasBusiness();
            BuscarDatosMayoristasRequest mr = new BuscarDatosMayoristasRequest();
            mr.MayoristaCode = 123;
            Assert.IsNotNull(mb.searchMayorista(mr));
        }

        [TestMethod]
        public void searchByName()
        {
            BuscarDatosMayoristasBusiness mb = new BuscarDatosMayoristasBusiness();
            BuscarDatosMayoristasRequest mr = new BuscarDatosMayoristasRequest();
            mr.MayoristaName = "Mayorista";
            Assert.IsNotNull(mb.searchMayorista(mr));
        }

    }
}
