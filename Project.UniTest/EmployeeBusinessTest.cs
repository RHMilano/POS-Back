using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Business;

namespace Milano.BackEnd.UniTest
{
    /// <summary>
    /// Clase de prueba búsqueda empleados de capa negocio
    /// </summary>
    [TestClass]
    public class EmployeeBusinessTest
    {

        /// <summary>
        /// Método que ejecuta una búsqueda de empleados
        /// </summary>
        [TestMethod]
        public void SearchEmployeeTest()
		{

			TokenDto token = new TokenDto(3215, 3);
			EmployeeBusiness employeeBusiness = new EmployeeBusiness(token);
            EmployeeRequest employeeRequest = new EmployeeRequest();
            employeeRequest.Code = 0;
            employeeRequest.Name = "Mariana";
            var result = employeeBusiness.SearchEmployee(employeeRequest);
            Assert.IsTrue(result.Data.Length > 0);
        }

    }
}
