using Milano.BackEnd.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Milano.BackEnd.Repository
{
	/// <summary>
	/// Repositorio mayorista
	/// </summary>
	public class MayoristaRepository:BaseRepository 
	{

		/// <summary>
		/// Mensaje de validacion de fecha de nacimiento vs el RFC
		/// </summary>
		/// <returns></returns>
		public OperationResponse ObtenerMensajeFechasInvalidaRFC()
		{
			OperationResponse operationResponse = new OperationResponse();
			var parameters = new Dictionary<string, object>();

			List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
			parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
			parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
			var result = data.ExecuteProcedure("[dbo].[sp_vanti_MensajeMayoristaValidacionFechaRFC]", parameters, parametersOut);
			operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
			operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
			return operationResponse;
		}

		/// <summary>
		/// Validacion de qeu el cliente es mayor de 18 años
		/// </summary>
		/// <returns></returns>
		public OperationResponse ObtenerMensajeMenorEdad()
		{
			OperationResponse operationResponse = new OperationResponse();
			var parameters = new Dictionary<string, object>();

			List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
			parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
			parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
			var result = data.ExecuteProcedure("[dbo].[sp_vanti_MensajeMayoristaValidacionMayoritaMayorEdad]", parameters, parametersOut);
			operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
			operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
			return operationResponse;
		}

	}
}
