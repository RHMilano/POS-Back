using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Business;
namespace Milano.BackEnd.UniTest
{
	[TestClass]
	public class AdministracionPagoServiciosBusinessTest
	{
		[TestMethod]
		public void ObtenerListaCompanias()
		{
			TokenDto token = new TokenDto(3215, 3);
			var result = new AdministracionPagoServiciosBusiness(token).ObtenerListaEmpresas ();
			Assert.IsNotNull(result); 
		}

		[TestMethod]
		public void PagoServicio()
		{
			TokenDto token = new TokenDto(3215, 3);
			PagoServiciosRequest pago = new PagoServiciosRequest();
			pago.SkuCode = 1;
			pago.SkuCodePagoServicio = "";
			pago.Cuenta = "501097118548123";
			
			
			var result = new AdministracionPagoServiciosBusiness(token).PagoServicio (pago,100,"");
			Assert.IsNotNull(result);
		}
	}
}
