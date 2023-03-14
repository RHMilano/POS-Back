using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Milano.BackEnd.Repository;
using System.Data;
using System.Configuration;
using Milano.BackEnd.Utils;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Dto;

namespace Milano.BackEnd.Repository.General
{
    /// <summary>
    /// Repositorio de Configuración de caja
    /// </summary>
    public class InstallationServiceRepository : BaseRepository
    {
        /// <summary>
        ///Insertar configuración de caja
        /// </summary>
        /// <param name="codigoCaja">Número de la caja</param>
        /// <param name="ipEstaticaCaja">Ip estática de la caja</param>
        /// <param name="codigoEmpleado">Código del Empleado</param>
        /// <returns></returns>
        public OperationResponse InsertConfigurationBox(int codigoCaja, string ipEstaticaCaja, int codigoEmpleado)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoCaja", codigoCaja);
            parameters.Add("@IpEstaticaCaja", ipEstaticaCaja);
            parameters.Add("@CodigoEmpleado", codigoEmpleado);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_ConfiguracionNuevoPOS]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }
    }
}
