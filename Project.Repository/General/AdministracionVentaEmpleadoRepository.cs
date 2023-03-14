using Milano.BackEnd.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Milano.BackEnd.Repository
{
	/// <summary>
	/// Repositorio de ventas empleado
	/// </summary>
	public class AdministracionVentaEmpleadoRepository : BaseRepository
	{
		/// <summary>
		/// Obtenemos el id de la compania
		/// </summary>
		/// <returns></returns>
		public int ObtenerCompania()
		{
			int idCompania = 0;
			var parameters = new Dictionary<string, object>();
			foreach (var c in data.GetDataReader("[dbo].[sp_vanti_EmpresaVentasEmpleadoObtener]", parameters))
						idCompania = Convert.ToInt32 (  c.GetValue(0).ToString());
			return idCompania;
		}

	
		/// <summary>
		/// Validar si se permite la venta a empleado
		/// </summary>
		/// <returns></returns>
		public OperationResponse ValidarVentaEmpleado()
		{
			OperationResponse operationResponse = new OperationResponse();
			var parameters = new Dictionary<string, object>();

			foreach (var item in data.GetDataReader("[dbo].[sp_vanti_ValidarVentaEmpleado]", parameters))
			{
				operationResponse.CodeNumber = item.GetValue(0).ToString();
				operationResponse.CodeDescription = item.GetValue(1).ToString();
			}
			return operationResponse;
		}
	}
}
