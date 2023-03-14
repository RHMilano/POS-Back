using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Business;
namespace Milano.BackEnd.UniTest
{
	[TestClass]
	public class AdministracionTiempoAireBusinessTest
	{
		[TestMethod]
		public void ObtenerListaCompanias()
		{
			TokenDto token = new TokenDto(3215, 3);
			var lista = new AdministracionTiempoAireBusiness(token).ObtenerListaEmpresas();
			Assert.IsNotNull(lista.Data );
		}
		[TestMethod]
		public void ObtenerMontoPorCompania()
		{
			TokenDto token = new TokenDto(3215, 3);
			var lista = new AdministracionTiempoAireBusiness(token).ObtenerProductosTA ("1");
			Assert.IsNotNull(lista.Data );
		}


	}
}
