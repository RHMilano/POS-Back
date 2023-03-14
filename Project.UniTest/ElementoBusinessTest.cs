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
	public class ElementoBusinessTest
	{
		[TestMethod]
		public void Proveedores()
		{
			var result = new ElementosBusiness().Proveedores();
			Assert.IsTrue(result.Data.Length > 0);
		}

		[TestMethod]
		public void Departamentos()
		{
			var result = new ElementosBusiness().Departamentos ();
			Assert.IsTrue(result.Data.Length > 0);
		}

		[TestMethod]
		public void SuDepartamentos()
		{
			var result = new ElementosBusiness().Subdepartamentos (1);
			Assert.IsTrue(result.Data.Length > 0);
		}

		[TestMethod]
		public void Clases()
		{
			var result = new ElementosBusiness().Clases (1,1);
			Assert.IsTrue(result.Data.Length > 0);
		}

		[TestMethod]
		public void SubClases()
		{
			var result = new ElementosBusiness().SubClases (1,1,1);
			Assert.IsTrue(result.Data.Length > 0);
		}
	}
}
