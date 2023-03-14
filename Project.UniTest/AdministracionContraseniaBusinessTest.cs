using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Business;

namespace Milano.BackEnd.UniTest
{
	[TestClass]
	public class AdministracionContraseniaBusinessTest
	{
		[TestMethod]
		public void CambiarPassword()
		{
			TokenDto token = new TokenDto(3215, 3);
		
			OperationResponse operationResponse = new AdministracionContraseniaBusiness(token).CambiarPassword(1717,"milano123");
			Assert.IsNotNull(operationResponse);
		}
	}
}
