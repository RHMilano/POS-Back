using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.General;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Milano.BackEnd.Repository.General
{

    /// <summary>
    /// Repositorio de egresos 
    /// </summary>
    public class EgresosRepository : BaseRepository
    {

        /// <summary>
        /// Busqueda rápida de productos
        /// </summary>
        /// <param name="codeStore">Código de la tienda</param>
        /// <param name="codeBox">Código de la caja</param>
        /// <param name="codeEmployee">Código del empleado</param>
        /// <param name="retiroParcialEfectivo">Objeto que representa el retiro parcial de efectivo</param>
        /// <returns>Respuesta de la operación</returns>
        public RetiroParcialEfectivoResponse RetiroParcialEfectivo(int codeStore, int codeBox, int codeEmployee, RetiroParcialEfectivo retiroParcialEfectivo)
        {
            RetiroParcialEfectivoResponse operationResponse = new RetiroParcialEfectivoResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@CodigoCaja", codeBox);
            parameters.Add("@Monto", retiroParcialEfectivo.monto);
            parameters.Add("@CodigoEmpleado", codeEmployee);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@FolioRetiroParcial", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 17 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_RetiroParcialEfectivo]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            operationResponse.FolioRetiro = result["@FolioRetiroParcial"].ToString();

            return operationResponse;
        }

        /// <summary>
        /// Ignorar retiro de efectivo
        /// </summary>
        /// <param name="codeBox">Numero de caja</param>
        /// <returns>Operation response </returns>
        public OperationResponse IgnorarRetiro(int codeBox)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoCaja", codeBox);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("sp_vanti_RetiroEfectivoIgnorar", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }

        /// <summary>
        /// Busqueda rápida de productos
        /// </summary>
        /// <param name="codeStore">Código de la tienda</param>
        /// <param name="codeBox">Código de la caja</param>
        /// <param name="codeEmployee">Código del empleado</param>
        /// <param name="egreso">Objeto que representa el egreso</param>
        /// <returns>Respuesta de la operación</returns>
        public OperationResponse ProcesarEgreso(int codeStore, int codeBox, int codeEmployee, Egreso egreso)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@CodigoCaja", codeBox);
            parameters.Add("@Monto", egreso.monto);
            parameters.Add("@CodigoRazon", egreso.CodigoRazon);
            parameters.Add("@NumeroAutorizacion", egreso.NumeroAutorizacion);
            parameters.Add("@CodigoEmpleado", codeEmployee);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_ProcesarEgreso]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }

    }
}
