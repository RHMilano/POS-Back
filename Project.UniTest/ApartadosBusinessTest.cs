using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milano.BackEnd.Business;
using Milano.BackEnd.Dto;
namespace Milano.BackEnd.UniTest
{
	[TestClass]
	public class ApartadosBusinessTest
	{
		[TestMethod]
		public void CederApartados()
		{
			var result = new ApartadosBusiness(new TokenDto(1, 1)).CederApartados();
			Assert.IsTrue(result.Data == 1);
		}
	}
}
