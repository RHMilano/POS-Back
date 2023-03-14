using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milano.BackEnd.Business;
using Milano.BackEnd.Dto;
namespace Milano.BackEnd.UniTest
{
	[TestClass]
	public class TiempoAireBusinessTest
	{
		[TestMethod]
		public void  GetIdRequest()
		{
			TiempoAireRequest tiempoAireRequest = new TiempoAireRequest();
			tiempoAireRequest.SkuCode = "042000001";
			tiempoAireRequest.Telefono = "666666666666";
			tiempoAireRequest.Monto = 200;
			TokenDto token = new TokenDto(3215, 3);
			OperationResponse operation = new TiempoAireBusiness(token ).AddTiempoAire(tiempoAireRequest,1,"");
			Assert.IsTrue (operation.CodeDescription!=string.Empty);
		
		}
	}
}
