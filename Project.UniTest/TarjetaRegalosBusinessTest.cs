using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milano.BackEnd.Business;
using Milano.BackEnd.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milano.BackEnd.UniTest
{
	[TestClass]
	public class TarjetaRegalosBusinessTest
	{
		[TestMethod]
		public void ActivarTarjeta()
		{
			TokenDto token = new TokenDto(3215, 3);
			var a =new TarjetaRegalosBusiness(token).ActivarTarjeta(1717, "167254","12345678912345678");
			Assert.IsNotNull(a);
		}
		[TestMethod]
		public void Cobro()
		{
			TokenDto token = new TokenDto(3215, 3);
			var a = new TarjetaRegalosBusiness(token).Cobro (1717, "167254", 1, "12345678912345678", 1);
			Assert.IsNotNull(a);
		}
		[TestMethod]
		public void Busqueda()
		{
			TokenDto token = new TokenDto(3215, 3);
			var a = new TarjetaRegalosBusiness(token).Busqueda ("174665");
			Assert.IsNotNull(a);
		}

	}
}
