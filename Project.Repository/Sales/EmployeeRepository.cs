using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Repository;
using System.Data;
using System.Configuration;
using Milano.BackEnd.Utils;

namespace Milano.BackEnd.Repository
{
    /// <summary>
    /// Repositorio de Empleados
    /// </summary>
    public class EmployeeRepository : BaseRepository
    {
		/// <summary>
		/// Búsqueda de empleado
		/// </summary>
		/// <param name="numberEmployee">Número de empleado</param>
		/// <param name="name">Nombre del empleado</param>
		/// <param name="codigoTienda">Codigo de tienda</param>
		/// <returns>Lista de empleados</returns>
		public EmployeeResponse[] SearchEmployee(int numberEmployee, string name,int codigoTienda)
        {

            List<EmployeeResponse> list = new List<EmployeeResponse>();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoEmpleado", numberEmployee);
            parameters.Add("@Nombre", name);
			parameters.Add("@CodigoTienda", codigoTienda);
			foreach (var r in data.GetDataReader("sp_vanti_EmpleadosBusqueda", parameters))
            {
                EmployeeResponse employee = new EmployeeResponse();

                employee.Code = Convert.ToInt32 ( r.GetValue (0));
                employee.UserName = r.GetValue (1).ToString ();
                employee.Paternal = r.GetValue(2).ToString();
				employee.Maternal = r.GetValue(3).ToString();
				employee.Name = r.GetValue(4).ToString();
				employee.Position = r.GetValue(5).ToString();
				employee.RoleCode = Convert.ToInt32 ( r.GetValue (6));
                employee.InitialDate = Convert.ToDateTime ( r.GetValue (7)).ToShortDateString();
                employee.Sex = r.GetValue(8).ToString ();
                employee.Status = r.GetValue(9).ToString ();
                employee.Store = Convert.ToInt32 ( r.GetValue (10));
                list.Add(employee);
            }
            return list.ToArray();
        }
    }
}
