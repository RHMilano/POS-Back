using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milano.BackEnd.Business;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.General;

namespace Milano.BackEnd.UniTest
{
	[TestClass]
	public class AdministracionVentaEmpleadoBusinessTest
	{
		[TestMethod]
		public void Buscar()
		{
			TokenDto token = new TokenDto(3215, 3);
			var  valor = new AdministracionVentaEmpleadoBusiness().Buscar("1717","3215","3");
			Assert.IsNotNull(valor);

		}
		
	}
}
