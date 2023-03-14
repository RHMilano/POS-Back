using Milano.BackEnd.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Milano.BackEnd.Repository.MM
{

    /// <summary>
    /// 
    /// </summary>
    public class MelodyMilanoRepository : BaseRepository
    {
        /// <summary>
        /// Persistir Consulta de Saldo TMM
        /// </summary>
        /// <returns></returns>        
        public OperationResponse PersistirConsultarInformacionTCMM(int codigoCaja, int codigoTienda, int codigoEmpleado, decimal saldoLinea, decimal saldoCorte, string fechaLimitePago, decimal pagoMinimo, int saldoEnPuntos, decimal equivalenteEnPuntos, int puntosAcumulados)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            if (fechaLimitePago == "" || fechaLimitePago == null)
            {
                parameters.Add("@FechaLimitePago", "");
            }
            else
            {
                parameters.Add("@FechaLimitePago", Convert.ToDateTime(fechaLimitePago));
            }
            parameters.Add("@CodigoCaja", codigoCaja);
            parameters.Add("@CodigoTienda", codigoTienda);
            parameters.Add("@CodigoEmpleado", codigoEmpleado);
            parameters.Add("@SaldoLinea", saldoLinea);
            parameters.Add("@SaldoCorte", saldoCorte);
            parameters.Add("@PagoMinimo", pagoMinimo);
            parameters.Add("@SaldoEnPuntos", saldoEnPuntos);
            parameters.Add("@EquivalenteEnPuntos", equivalenteEnPuntos);
            parameters.Add("@PuntosAcumulados", puntosAcumulados);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var resultado = data.ExecuteProcedure("[dbo].[sp_vanti_PersistenciaConsultaSaldoTCMM]", parameters, parametersOut);
            operationResponse.CodeNumber = resultado["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = resultado["@MensajeResultado"].ToString();
            return operationResponse;
        }
    }
}
