using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.Apartados;
using Milano.BackEnd.Dto.Sales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Milano.BackEnd.Repository
{
    /// <summary>
    /// Repositorio de apartados
    /// </summary>
    public class ApartadoAbonoRepository : BaseRepository
    {

        /// <summary>
        /// Abonar un apartado
        /// </summary>
        /// <param name="codeStore">Código de la tienda</param>
        /// <param name="codeBox">Código de la caja</param>
        /// <param name="codeEmployee">Código del empleado</param>
        /// <param name="abonoApartadoRequest">Petición del abono</param>
        /// <param name="clasificacionVenta">Clasificación de la venta realizada</param>
        /// <returns></returns>
        public TransApartadoResponse Abonar(int codeStore, int codeBox, int codeEmployee, AbonoApartadoRequest abonoApartadoRequest, string clasificacionVenta)
        {
            TransApartadoResponse operationResponse = new TransApartadoResponse();
            new FormasPagoRepository().AsociarFormasPago(codeStore, codeBox, codeEmployee, abonoApartadoRequest.FolioApartado, abonoApartadoRequest.FormasPagoUtilizadas, clasificacionVenta);
            operationResponse = this.AbonarApartadoInternal(codeStore, codeBox, codeEmployee, abonoApartadoRequest, 0);
            return operationResponse;
        }

        private TransApartadoResponse AbonarApartadoInternal(int codeStore, int codeBox, int codeEmployee, AbonoApartadoRequest abonoApartadoRequest, decimal abonoPagado)
        {
            TransApartadoResponse operationResponse = new TransApartadoResponse();
            InformacionAsociadaRetiroEfectivo informacionAsociadaRetiroEfectivo = new InformacionAsociadaRetiroEfectivo();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioApartado", abonoApartadoRequest.FolioApartado);
            parameters.Add("@CodigoCaja", codeBox);
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@CodigoEmpleado", codeEmployee);
            parameters.Add("@ImportePagado", abonoApartadoRequest.ImportePagado);
            parameters.Add("@ImporteCambio", abonoApartadoRequest.ImporteCambio);
            parameters.Add("@AbonoPagado", abonoPagado);
            parameters.Add("@Saldo", abonoApartadoRequest.Saldo);
            if (abonoApartadoRequest.ApartadoLiquidado)
            {
                parameters.Add("@EsLiquidacion", 1);
            }
            else
            {
                parameters.Add("@EsLiquidacion", 0);
            }
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeEfectivoMaximo", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@EfectivoMaximoCaja", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Decimal, Scale = 2 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@DotacionInicial", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Decimal, Scale = 2 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MontoActualCaja", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Decimal, Scale = 2 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MostrarAlertaRetiroEfectivo", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Bit });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@PermitirIgnorar", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Bit });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@FolioVenta", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });

            var result = data.ExecuteProcedure("[dbo].[sp_vanti_server_AbonarApartado]", parameters, parametersOut);
            // Información referente a retiro de Efectivo
            informacionAsociadaRetiroEfectivo.MensajeEfectivoMaximo = result["@MensajeEfectivoMaximo"].ToString();
            informacionAsociadaRetiroEfectivo.EfectivoMaximoPermitidoCaja = Convert.ToDecimal(result["@EfectivoMaximoCaja"]);
            informacionAsociadaRetiroEfectivo.DotacionInicialCaja = Convert.ToDecimal(result["@DotacionInicial"]);
            informacionAsociadaRetiroEfectivo.EfectivoActualCaja = Convert.ToDecimal(result["@MontoActualCaja"]);
            informacionAsociadaRetiroEfectivo.MostrarAlertaRetiroEfectivo = Convert.ToBoolean(result["@MostrarAlertaRetiroEfectivo"]);
            informacionAsociadaRetiroEfectivo.PermitirIgnorarAlertaRetiroEfectivo = Convert.ToBoolean(result["@PermitirIgnorar"]);
            operationResponse.informacionAsociadaRetiroEfectivo = informacionAsociadaRetiroEfectivo;
            // Información referente a estatus de la operación
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            operationResponse.FolioVenta = result["@FolioVenta"].ToString();
            return operationResponse;
        }

    }
}
