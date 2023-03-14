using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.Sales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Milano.BackEnd.Repository.Sales
{
    /// <summary>
    /// Repositorio de validacion del cupon
    /// </summary>
    public class CuponesRedimirRepository : BaseRepository
    {

        /// <summary>
        /// Valida el saldo del cupón
        /// </summary>
        /// <param name="cuponRedimirRequest"></param>
        /// <returns></returns>
        public CuponRedimirResponse ValidarSaldo(CuponRedimirRequest cuponRedimirRequest)
        {
            CuponRedimirResponse cuponRedimirResponse = new CuponRedimirResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioVentaRedencion", cuponRedimirRequest.FolioVenta);
            parameters.Add("@FolioCupon", cuponRedimirRequest.FolioCupon);
            parameters.Add("@CodigoTienda", cuponRedimirRequest.CodigoTienda);
            parameters.Add("@CodigoCaja", cuponRedimirRequest.CodigoCaja);
            parameters.Add("@SaldoCupon", cuponRedimirRequest.SaldoCupon);
            parameters.Add("@CodigoPromocion", cuponRedimirRequest.CodigoPromocion);

            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MaximoRedencionOut", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Decimal, Precision = 11, Scale = 2 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeRedencionOut", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoTipoTrxCab", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@Transaccion", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@EsRedimibleHoy", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });

            var result = data.ExecuteProcedure("[dbo].[sp_vanti_PrmRedimirCupon]", parameters, parametersOut);

            cuponRedimirResponse.Saldo = Convert.ToDecimal(result["@MaximoRedencionOut"]);
            cuponRedimirResponse.MensajeRedencion = result["@MensajeRedencionOut"].ToString();
            cuponRedimirResponse.CodigoTipoTrxCab = result["@CodigoTipoTrxCab"].ToString();
            cuponRedimirResponse.Transaccion = Convert.ToInt32(result["@Transaccion"]);
            cuponRedimirResponse.EsRedimibleHoy = Convert.ToInt32(result["@EsRedimibleHoy"]);

            return cuponRedimirResponse;
        }

        /// <summary>
        /// Método para persistir cupón promocional
        /// </summary>
        /// <param name="cuponPersistirRequest">Información a persistir</param>
        /// <returns></returns>
        public OperationResponse PersistirCupon(CuponPersistirRequest cuponPersistirRequest)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioVenta", cuponPersistirRequest.FolioVenta);
            parameters.Add("@FolioCupon", cuponPersistirRequest.FolioCupon);
            parameters.Add("@CodigoCaja", cuponPersistirRequest.CodigoCaja);
            parameters.Add("@CodigoTienda", cuponPersistirRequest.CodigoTienda);
            parameters.Add("@CodigoEmpleado", cuponPersistirRequest.CodigoEmpleado);
            parameters.Add("@MaximoRedencion", cuponPersistirRequest.MaximoRedencion);
            parameters.Add("@Transaccion", cuponPersistirRequest.Transaccion);
            parameters.Add("@Sesion", cuponPersistirRequest.Sesion);

            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });

            var result = data.ExecuteProcedure("[dbo].[sp_vanti_PersistirCupon]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>        
        public CuponRedimirResponse ValidarCuponLocal(string FolioCupon, int Tienda, int Caja)
        {
            CuponRedimirResponse cuponRedimirResponse = new CuponRedimirResponse();
            var parameters = new Dictionary<string, object>();

            parameters.Add("@FolioCupon", FolioCupon);
            parameters.Add("@CodigoTienda", Tienda);
            parameters.Add("@CodigoCaja", Caja);

            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();

            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@Saldo", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Decimal });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeRedencion", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.VarChar, Size = 200 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@Transaccion", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoTipoTrxCab", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.VarChar, Size = 10 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@EsRedimibleHoy", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoPromocion", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });

            var result = data.ExecuteProcedure("[dbo].[spValidarCuponLocal]", parameters, parametersOut);

            cuponRedimirResponse.Saldo = Convert.ToInt32(result["@Saldo"]);
            cuponRedimirResponse.MensajeRedencion = result["@MensajeRedencion"].ToString();
            cuponRedimirResponse.Transaccion = Convert.ToInt32(result["@Transaccion"]);
            cuponRedimirResponse.CodigoTipoTrxCab = result["@CodigoTipoTrxCab"].ToString();
            cuponRedimirResponse.EsRedimibleHoy = Convert.ToInt32(result["@EsRedimibleHoy"]);
            cuponRedimirResponse.CodigoPromocion = Convert.ToInt32(result["@CodigoPromocion"]);

            return cuponRedimirResponse;
        }
    }
}
