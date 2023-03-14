using Milano.BackEnd.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Milano.BackEnd.Repository
{

    /// <summary>
    /// Repositorio administracion tipo de cambio
    /// </summary>
    public class AdministracionTipoCambioRepository : BaseRepository
    {

        /// <summary>
        /// Obtenemos el tipo de cambio de la tabla de milano
        /// </summary>
        /// <param name="codigoDivisa"> codigo de divisa</param>
        /// <returns>valor de la divisa</returns>
        public CambioDivisaMilano ObtenerTipoCambio(string codigoDivisa)
        {
            CambioDivisaMilano cambio = null;
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoDivisa", codigoDivisa);

            foreach (var c in data.GetDataReader("[dbo].[sp_vanti_CambioDivisa]", parameters))
            {
                cambio = new CambioDivisaMilano();
                cambio.ValorCambio = Convert.ToDecimal(c.GetValue(0));
                cambio.UsarMaximoValor = Convert.ToBoolean(c.GetValue(1));
                cambio.CodigoExterno = c.GetValue(2).ToString();
            }

            return cambio;
        }

        /// <summary>
        /// Catalogo de divisas
        /// </summary>
        /// <returns></returns>
        public Divisa[] ObtenerCatalogoDivisa(int CodeStore)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", CodeStore);
            List<Divisa> lista = new List<Divisa>();

         
            foreach (var c in data.GetDataReader("[dbo].[sp_vanti_ObtenerCatalogoDivisa]", parameters))
            {
                Divisa divisa = new Divisa();
                divisa.Codigo = c.GetValue(0).ToString();
                divisa.Descripcion = c.GetValue(1).ToString();
                divisa.MontoMaximoMovimientoDivisaTransaccion = Convert.ToDecimal(c.GetValue(2));
                divisa.MontoMaximoCambioDivisaTransaccion = Convert.ToDecimal(c.GetValue(3));
                lista.Add(divisa);
            }
            return lista.ToArray();
        }

        /// <summary>
        /// Mensaje de error de servicio de cambio de divisa
        /// </summary>
        /// <returns></returns>
        public OperationResponse ObtenerMensajeCambioDivisaNoDisponible()
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();

            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("sp_vanti_MensajeTipoCambioNoDisponible", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }

    }
}
