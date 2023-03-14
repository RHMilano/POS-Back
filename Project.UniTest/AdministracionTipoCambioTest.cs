using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milano.BackEnd.Business;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.General;

namespace Milano.BackEnd.UniTest
{
	[TestClass]
	public class AdministracionTipoCambioTest
	{
		[TestMethod]
		public void ObtenerTipoCambio()
		{
			TipoCambioRequest tipo = new TipoCambioRequest();
			tipo.CodigoTipoDivisa = "USD";
			tipo.ImporteMonedaNacional = 1000;
			ResponseBussiness < TipoCambioResponse > res = new AdministracionTipoCambio().ObtenerTipoCambio(tipo);
			Assert.IsNotNull(res); 
		}
	}
}
