using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Milano.BackEnd.Repository;
using System.Data;
using Milano.BackEnd.Dto;
using System.Configuration;
using Milano.BackEnd.Utils;
using Milano.BackEnd.Dto.Sales;


namespace Milano.BackEnd.Repository.Sales
{
    /// <summary>
    /// Repositorio de Validación de Fraude Tiempo Aire
    /// </summary>
    public class FraudValidationRepository : BaseRepository
    {

        /// <summary>
        ///Validar fraude en Tiempo Aire
        /// </summary>
        /// <param name="numeroTelefonico">Número telefónico/param>
        /// <param name="codigoTienda">Código de la tienda</param>
        /// <returns></returns>
        public OperationResponse FraudValidationTA(decimal numeroTelefonico, int codigoTienda)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@NumeroTelefonico", numeroTelefonico);
            parameters.Add("@CodigoTienda", codigoTienda);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_ValidacionFraudeTA]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }
    }
}
