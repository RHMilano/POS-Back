using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.Finlag;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Milano.BackEnd.Repository.Finlag
{
    /// <summary>
    /// 
    /// </summary>
    public class FinlagRepository : BaseRepository
    {

        /// <summary>
        /// Procesar movimiento con un vale de Finlag
        /// </summary>
        /// <param name="codigoTienda">Código de tienda</param>
        /// <param name="codigoCaja">Código de caja</param>
        /// <param name="codigoEmpleado">Código de empleado</param>
        /// <param name="folioOperacionAsociada">Folio de la operación asociada</param>
        /// <param name="consultaTramaImpresionResult">Objeto petición de procesamiento de trama impresión Finlag</param>
        /// <returns>Resultado de la operacion</returns>
        public OperationResponse ProcesarTramaImpresion(int codigoTienda, int codigoCaja, int codigoEmpleado, String folioOperacionAsociada, ConsultaTramaImpresionResult consultaTramaImpresionResult)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", codigoTienda);
            parameters.Add("@CodigoCaja", codigoCaja);
            parameters.Add("@CodigoEmpleado", codigoEmpleado);
            parameters.Add("@FolioOperacion", folioOperacionAsociada);
            // Parametros Finlag
            parameters.Add("@Calle", consultaTramaImpresionResult.Calle);
            parameters.Add("@Colonia", consultaTramaImpresionResult.Colonia);
            parameters.Add("@CP", consultaTramaImpresionResult.CP);
            parameters.Add("@EfectivoPuntos", consultaTramaImpresionResult.EfectivoPuntos);
            parameters.Add("@Estado", consultaTramaImpresionResult.Estado);
            parameters.Add("@FechaPrimerPago", consultaTramaImpresionResult.FechaPrimerPago);
            parameters.Add("@FolioVale", consultaTramaImpresionResult.FolioVale);
            parameters.Add("@IdCajaAplica", consultaTramaImpresionResult.IdCajaAplica);
            parameters.Add("@IdDistribuidora", consultaTramaImpresionResult.IdDistribuidora);
            parameters.Add("@MontoAplicado", consultaTramaImpresionResult.MontoAplicado);
            parameters.Add("@Municipio", consultaTramaImpresionResult.Municipio);
            parameters.Add("@NombreCompleto", consultaTramaImpresionResult.NombreCompleto);
            parameters.Add("@NumeroExt", consultaTramaImpresionResult.NumeroExt);
            parameters.Add("@Pagare", consultaTramaImpresionResult.Pagare);
            parameters.Add("@PagoQuincenal", consultaTramaImpresionResult.PagoQuincenal);
            parameters.Add("@PuntosUtilizados", consultaTramaImpresionResult.PuntosUtilizados);
            parameters.Add("@PuntoVenta", consultaTramaImpresionResult.PuntoVenta);
            parameters.Add("@Quincenas", consultaTramaImpresionResult.Quincenas);
            parameters.Add("@TiendaAplica", consultaTramaImpresionResult.TiendaAplica);
            parameters.Add("@TipoVenta", consultaTramaImpresionResult.TipoVenta);
            parameters.Add("@TotalPagar", consultaTramaImpresionResult.TotalPagar);
            // Parametros Finlag
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_ProcesarTramaImpresionFinlag]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }

        /// <summary>
        /// Procesar movimiento con un vale de Finlag
        /// </summary>
        /// <param name="codigoTienda">Código de tienda</param>
        /// <param name="codigoCaja">Código de caja</param>
        /// <param name="codigoEmpleado">Código de empleado</param>
        /// <param name="aplicaValeRequest">Objeto petición de procesamiento de movimiento vale Finlag</param>
        /// <returns>Resultado de la operacion</returns>
        public OperationResponse ProcesarMovimientoAplicarVale(int codigoTienda, int codigoCaja, int codigoEmpleado, AplicaValeRequest aplicaValeRequest)
        {
            OperationResponse operationResponse = new OperationResponse();
            String nombreCompleto = aplicaValeRequest.Nombre + " " + aplicaValeRequest.Apaterno + " " + aplicaValeRequest.Amaterno;
            operationResponse = this.PersistirMovimientoAplicarVale(codigoTienda, codigoCaja, codigoEmpleado,
              aplicaValeRequest.FolioOperacionAsociada, aplicaValeRequest.CodigoFormaPagoImporte, aplicaValeRequest.Estatus, aplicaValeRequest.SecuenciaFormaPagoImporte,
              aplicaValeRequest.ImporteVentaTotal, "0", 0, 0, aplicaValeRequest.FolioVale, nombreCompleto);
            //TODO: Almacenar la transacción hacia File System si aplica
            return operationResponse;
        }


        private OperationResponse PersistirMovimientoAplicarVale(int codigoTienda, int codigoCaja, int codigoEmpleado, string folioOperacionAsociada, string codigoFormaPagoImporte, string estatus,
            int secuenciaFormaPagoImporte, decimal importeVentaTotal, string codigoMoneda, int importeMonedaExtranjera, int tasaConversion, string folioVale, string nombreCompleto)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", codigoTienda);
            parameters.Add("@CodigoCaja", codigoCaja);
            parameters.Add("@CodigoEmpleado", codigoEmpleado);
            parameters.Add("@FolioOperacion", folioOperacionAsociada);
            parameters.Add("@CodigoFormaPago", codigoFormaPagoImporte);
            parameters.Add("@Estatus", estatus);
            parameters.Add("@Secuencia", secuenciaFormaPagoImporte);
            parameters.Add("@ImportePago", importeVentaTotal);
            parameters.Add("@CodigoMoneda", codigoMoneda);
            parameters.Add("@ImporteMonedaExtranjera", importeMonedaExtranjera);
            parameters.Add("@TasaConversion", tasaConversion);
            parameters.Add("@FolioValeFinlag", folioVale);
            parameters.Add("@NombreCompletoClienteFinlag", nombreCompleto);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_ProcesarMovimientoTransaccionFinlag]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }

    }
}
