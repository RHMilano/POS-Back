using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.FormasPago;
using Milano.BackEnd.Dto.MM;
using Milano.BackEnd.Dto.MilanoEntities;
using Milano.BackEnd.Dto.Recovery;
using Milano.BackEnd.Dto.Sales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;
using Milano.BackEnd.Dto.General;

namespace Milano.BackEnd.Repository
{

    /// <summary>
    /// Repositorio para el procesamiento de cobros
    /// </summary>
    public class PaymentProcessingRepository : BaseRepository
    {

        /// <summary>
        /// Procesar movimiento en Efectivo
        /// </summary>
        /// <param name="codeStore">Código de tienda</param>      
        /// <param name="codeBox">Código de caja</param>
        /// <param name="codeEmployee">Código del empleado cajero</param>        
        /// <param name="folioOperacion">Folio de la operación correspondiente</param>
        /// <param name="formaPagoUtilizado">Formas de pago utilizadas</param>
        /// <param name="Autorizacion">Numero de autorizacion</param>
        /// <returns>Secuencial actual de la operación</returns>
        public void ProcesarMovimientoEfectivo(int codeStore, int codeBox, int codeEmployee, String folioOperacion, FormaPagoUtilizado formaPagoUtilizado)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                this.PersistirMovimientoEfectivo(codeStore, codeBox, codeEmployee, folioOperacion, formaPagoUtilizado);
                //TODO: Almacenar la transacción hacia File System si aplica
                scope.Complete();
            }
        }

        private void PersistirMovimientoEfectivo(int codeStore, int codeBox, int codeEmployee, String folioOperacion, FormaPagoUtilizado formaPagoUtilizado)
        {
            this.PersistirMovimientoEfectivo(codeStore, codeBox, codeEmployee,
                folioOperacion, formaPagoUtilizado.CodigoFormaPagoImporte, formaPagoUtilizado.Estatus, formaPagoUtilizado.SecuenciaFormaPagoImporte,
                formaPagoUtilizado.ImporteMonedaNacional, formaPagoUtilizado.InformacionTipoCambio.CodigoTipoDivisa,
                formaPagoUtilizado.InformacionTipoCambio.ImporteMonedaExtranjera,
                formaPagoUtilizado.InformacionTipoCambio.TasaConversionVigente, formaPagoUtilizado.Autorizacion);
            // Si existe cambio en la operación completa también debe registrarse
            if (formaPagoUtilizado.ImporteCambioMonedaNacional > 0)
            {
                this.PersistirMovimientoEfectivo(codeStore, codeBox, codeEmployee,
                folioOperacion, formaPagoUtilizado.CodigoFormaPagoImporteCambio, formaPagoUtilizado.Estatus, formaPagoUtilizado.SecuenciaFormaPagoImporteCambio,
                (formaPagoUtilizado.ImporteCambioMonedaNacional * -1), "0", 0, 0, formaPagoUtilizado.Autorizacion);
            }
            // TODO: Agrega validación y lógica para Sale para moneda extanjera
        }

        private OperationResponse PersistirMovimientoEfectivo(int codeStore, int codeBox, int codeEmployee, String folioVenta,
            string codigoFormaPago, string estatus, int secuencia, decimal importeMonedaNacional, string codigoTipoDivisa, decimal importeMonedaExtranjera,
            decimal tasaConversionUtilizada, string Autorizacion = "")
        {
            Autorizacion = string.IsNullOrEmpty(Autorizacion) ? " " : Autorizacion;

            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@CodigoCaja", codeBox);
            parameters.Add("@FolioOperacion", folioVenta);
            parameters.Add("@CodigoFormaPago", codigoFormaPago);
            parameters.Add("@Estatus", estatus);
            parameters.Add("@Secuencia", secuencia);
            parameters.Add("@CodigoEmpleado", codeEmployee);
            parameters.Add("@ImportePago", importeMonedaNacional);
            parameters.Add("@CodigoMoneda", codigoTipoDivisa);
            parameters.Add("@TasaConversion", tasaConversionUtilizada);
            parameters.Add("@ImporteMonedaExtranjera", importeMonedaExtranjera);
            parameters.Add("@Autorizacion", Autorizacion);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_ProcesarMovimientoTransaccionEfectivo]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }

        /// <summary>
        /// Procesar movimiento con Vales
        /// </summary>
        /// <param name="codeStore">Código de tienda</param>      
        /// <param name="codeBox">Código de caja</param>
        /// <param name="codeEmployee">Código del empleado cajero</param>        
        /// <param name="folioOperacion">Folio de la operación correspondiente</param>
        /// <param name="formaPagoUtilizado">Formas de pago utilizadas</param>
        /// <returns>Secuencial actual de la operación</returns>
        public void ProcesarMovimientoVales(int codeStore, int codeBox, int codeEmployee, String folioOperacion, FormaPagoUtilizado formaPagoUtilizado, string clasificacionVenta)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                this.PersistirMovimientoVales(codeStore, codeBox, codeEmployee, folioOperacion, formaPagoUtilizado, clasificacionVenta);
                //TODO: Almacenar la transacción hacia File System si aplica
                scope.Complete();
            }
        }

        private void PersistirMovimientoVales(int codeStore, int codeBox, int codeEmployee, String folioOperacion, FormaPagoUtilizado formaPagoUtilizado, string clasificacionVenta)
        {
            OperationResponse operationResponse = new OperationResponse();
            // Si existe cambio excedente en la operación deben registrarse como detalle de transacción
            if (formaPagoUtilizado.ImporteCambioExcedenteMonedaNacional > 0)
            {
                operationResponse = ProcesarMovimientoExcedenteCambioVales(folioOperacion, codeStore,
                    codeBox, formaPagoUtilizado.CodigoTipoTransaccionImporteCambioExcedente, formaPagoUtilizado.SecuenciaFormaPagoImporteCambioExcedente, formaPagoUtilizado.ImporteCambioExcedenteMonedaNacional, clasificacionVenta);
            }
            operationResponse = this.PersistirMovimientoVales(codeStore, codeBox, codeEmployee,
                folioOperacion, formaPagoUtilizado.CodigoFormaPagoImporte, formaPagoUtilizado.Estatus, formaPagoUtilizado.SecuenciaFormaPagoImporte,
                formaPagoUtilizado.ImporteMonedaNacional, "0", 0, 0, clasificacionVenta);
            // Si existe cambio en la operación también debe registrarse
            if (formaPagoUtilizado.ImporteCambioMonedaNacional > 0)
            {
                operationResponse = this.PersistirMovimientoVales(codeStore, codeBox, codeEmployee,
                folioOperacion, formaPagoUtilizado.CodigoFormaPagoImporteCambio, formaPagoUtilizado.Estatus, formaPagoUtilizado.SecuenciaFormaPagoImporteCambio,
                (formaPagoUtilizado.ImporteCambioMonedaNacional * -1), "0", 0, 0, clasificacionVenta);
            }
        }

        private OperationResponse PersistirMovimientoVales(int codeStore, int codeBox, int codigoEmpleado, String folioVenta,
            string codigoFormaPago, string estatus, int secuencia, decimal importeMonedaNacional, string codigoTipoDivisa, decimal importeMonedaExtranjera,
            decimal tasaConversionUtilizada, string clasificacionVenta)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@CodigoCaja", codeBox);
            parameters.Add("@FolioOperacion", folioVenta);
            parameters.Add("@CodigoFormaPago", codigoFormaPago);
            parameters.Add("@Estatus", estatus);
            parameters.Add("@Secuencia", secuencia);
            parameters.Add("@CodigoEmpleado", codigoEmpleado);
            parameters.Add("@ImportePago", importeMonedaNacional);
            parameters.Add("@CodigoMoneda", codigoTipoDivisa);
            parameters.Add("@TasaConversion", tasaConversionUtilizada);
            parameters.Add("@ImporteMonedaExtranjera", importeMonedaExtranjera);
            parameters.Add("@ClasificacionVenta", clasificacionVenta);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_ProcesarMovimientoTransaccionVales]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }

        private OperationResponse ProcesarMovimientoExcedenteCambioVales(string folioVenta, int codigoTienda, int codigoCaja, string codigoTipoTrxDet, int secuencia, decimal importeTotal, string clasificacionVenta)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioVenta", folioVenta);
            parameters.Add("@CodigoTienda", codigoTienda);
            parameters.Add("@CodigoCaja", codigoCaja);
            parameters.Add("@CodigoTipoTrxDet", codigoTipoTrxDet);
            parameters.Add("@Secuencia", secuencia);
            parameters.Add("@ImporteTotal", importeTotal);
            parameters.Add("@ClasificacionVenta", clasificacionVenta);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_ProcesarMovimientoTransaccionExcedenteCambioVales]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }

        /// <summary>
        /// Asociar movimientos de pago con la transacción de forma secuencial
        /// </summary>
        /// <param name="codeStore">Código de tienda</param>      
        /// <param name="codeBox">Código de caja</param>                
        /// <param name="folioOperacion">Folio de la operación correspondiente</param>
        /// <param name="formaPagoUtilizado">Formas de pago utilizadas</param>        
        public void AsociarMovimientoRegistrado(int codeStore, int codeBox, String folioOperacion, FormaPagoUtilizado formaPagoUtilizado)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@CodigoCaja", codeBox);
            parameters.Add("@FolioOperacion", folioOperacion);
            parameters.Add("@CodigoFormaPago", formaPagoUtilizado.CodigoFormaPagoImporte);
            parameters.Add("@Secuencia", formaPagoUtilizado.SecuenciaFormaPagoImporte);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            data.ExecuteProcedure("[dbo].[sp_vanti_AsociarMovimientoRegistrado]", parameters, parametersOut);
        }


        /// <summary>
        /// Obtener la configuración de los meses sin interes
        /// </summary>
        /// <param name="monto"></param>
        /// <returns></returns>
        public ConfiguracionMSI ObtenerConfiguracionMSI()
        {
            ConfiguracionMSI configuracionMSI = new ConfiguracionMSI();
            var parameters = new Dictionary<string, object>();
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();

            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@versionBBVA", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@mesesSinInteresesVisa", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@mesesSinInteresesAmex", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@montoMinimoVisa", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@montoMaximoVisa", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@montoMinimoAmex", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@montoMaximoAmex", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });

            var promociones = data.ExecuteProcedure("[dbo].[sp_vanti_obtenerConfiguracionMSIv2]", parameters, parametersOut);

            configuracionMSI.VersionBBVA = Convert.ToInt32(promociones["@versionBBVA"]);
            configuracionMSI.MesesSinInteresesVisa = Convert.ToInt32(promociones["@mesesSinInteresesVisa"]);
            configuracionMSI.MesesSinInteresesAmex = Convert.ToInt32(promociones["@mesesSinInteresesAmex"]);
            configuracionMSI.MontoMinimoVisa = Convert.ToInt32(promociones["@montoMinimoVisa"]);
            configuracionMSI.MontoMaximoVisa = Convert.ToInt32(promociones["@montoMaximoVisa"]);
            configuracionMSI.MontoMinimoAmex = Convert.ToInt32(promociones["@montoMinimoAmex"]);
            configuracionMSI.MontoMaximoAmex = Convert.ToInt32(promociones["@montoMaximoAmex"]);

            return configuracionMSI;

        }






        /// <summary>
        /// Procesar movimiento con tarjeta de regalo
        /// </summary>
        /// <param name="codigoTienda">Código de tienda</param>
        /// <param name="codigoCaja">Código de caja</param>
        /// <param name="codigoEmpleado">Código de empleado</param>
        /// <param name="request">Objeto petición de procesamiento de movimiento en tarjeta de regalo</param>
        /// <returns>Resultado de la operacion</returns>
        public OperationResponse ProcesarMovimientoTarjetaRegalo(int codigoTienda, int codigoCaja, int codigoEmpleado, ProcesarMovimientoTarjetaRegaloRequest request)
        {
            OperationResponse operationResponse = new OperationResponse();
            operationResponse = this.PersistirMovimientoTarjetRegalo(codigoTienda, codigoCaja, codigoEmpleado,
              request.FolioOperacionAsociada, request.CodigoFormaPagoImporte, request.Estatus, request.SecuenciaFormaPagoImporte,
              request.ImporteVentaTotal, "0", 0, 0, request.ClasificacionVenta, request.FolioTarjeta);
            //TODO: Almacenar la transacción hacia File System si aplica
            return operationResponse;
        }

        /// <summary>
        /// Procesar movimiento con cupón promocional
        /// </summary>
        /// <param name="codigoTienda">Código de tienda</param>
        /// <param name="codigoCaja">Código de caja</param>
        /// <param name="codigoEmpleado">Código de empleado</param>
        /// <param name="request">Objeto petición de procesamiento de cupón promocional</param>
        /// <returns>Resultado de la operacion</returns>
        public ProcesarMovimientoRedencionCuponResponse ProcesarMovimientoRedencionCuponPromocional(int codigoTienda, int codigoCaja, int codigoEmpleado, ProcesarMovimientoRedencionCuponRequest request)
        {
            ProcesarMovimientoRedencionCuponResponse procesarMovimientoRedencionCuponResponse = new ProcesarMovimientoRedencionCuponResponse();
            OperationResponse operationResponse = new OperationResponse();
            
            operationResponse = this.PersistirMovimientoRedencionCuponPromocional(codigoTienda, codigoCaja, codigoEmpleado,
            
            request.FolioOperacionAsociada, request.CodigoFormaPagoImporte, request.Estatus, request.SecuenciaFormaPagoImporte,
            request.ImporteVentaTotal, "0", 0, 0, request.FolioCuponPromocional);
            procesarMovimientoRedencionCuponResponse.CodeDescription = operationResponse.CodeDescription;
            procesarMovimientoRedencionCuponResponse.CodeNumber = operationResponse.CodeNumber;
            procesarMovimientoRedencionCuponResponse.SaldoAplicado = request.ImporteVentaTotal;
            procesarMovimientoRedencionCuponResponse.CodigoTipoTrxCab = operationResponse.CodigoTipoTrxCab;
            procesarMovimientoRedencionCuponResponse.Transaccion = operationResponse.Transaccion;
            return procesarMovimientoRedencionCuponResponse;
        }

        /// <summary>
        /// Procesar movimiento con tarjeta bancaria de la venta
        /// </summary>
        /// <param name="codigoTienda">Código de tienda</param>
        /// <param name="codigoCaja">Código de caja</param>
        /// <param name="codigoEmpleado">Código de empleado</param>
        /// <param name="autorizacion">Número de autorización bancaria</param>
        /// <param name="numeroTarjeta">Ultimos 4 digitos del número de tarjeta</param>
        /// <param name="request">Objeto petición de procesamiento de movimiento de la tarjeta bancaria</param>
        /// <returns>Resultado de la operacion</returns>
        public OperationResponse ProcesarMovimientoTarjetaBancariaVenta(int codigoTienda, int codigoCaja, int codigoEmpleado, string autorizacion, string numeroTarjeta, ProcesarMovimientoTarjetaBancariaRequest request)
        {
            //OCG: Modificación para integración de plazos y pago
            OperationResponse operationResponse = new OperationResponse();
            // Se persiste información de negocio a base de datos
            operationResponse = this.PersistirMovimientoTarjetaBancaria(codigoTienda, codigoCaja, codigoEmpleado,
              request.Venta.FolioOperacionAsociada, request.Venta.CodigoFormaPagoImporte, request.Venta.Estatus, request.Venta.SecuenciaFormaPagoImporte,
              request.Venta.ImporteVentaTotal, "", 0, 0, request.Venta.ClasificacionVenta, autorizacion, numeroTarjeta,
              "N", "N");
            return operationResponse;
        }
        /// <summary>
        /// PersistirPromocionesVenta
        /// </summary>
        /// <param name="folioOperacionAsociada"></param>
        /// <param name="codeStore"></param>
        /// <param name="codeBox"></param>
        /// <param name="importeDescuento"></param>
        /// <param name="codigoPromocionAplicado"></param>
        /// <param name="descripcionCodigoPromocionAplicado"></param>
        /// <param name="formaPagoCodigoPromocionAplicado"></param>
        /// <returns></returns>
        public OperationResponse PersistirPromocionesVenta(string folioOperacionAsociada, int codeStore, int codeBox, decimal importeDescuento
            , int codigoPromocionAplicado, string descripcionCodigoPromocionAplicado, decimal porcentajeDescuento, int codigoRazonDescuento, string formaPagoCodigoPromocionAplicado)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioOperacion", folioOperacionAsociada);
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@CodigoCaja", codeBox);
            parameters.Add("@ImporteDescuento", importeDescuento);
            parameters.Add("@CodigoPromocion", codigoPromocionAplicado);
            parameters.Add("@PorcentajeDescuento", porcentajeDescuento);
            parameters.Add("@CodigoRazonDescuento", codigoRazonDescuento);
            parameters.Add("@DescripcionPromocion", descripcionCodigoPromocionAplicado);
            parameters.Add("@FormaPagoPromocion", formaPagoCodigoPromocionAplicado);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_PersisitirPromocionesCab]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }




        public OperationResponse PersistirPromocionesVenta()
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();

            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_VerificaPromocionMSIv2]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }





        /// <summary>
        /// PersistirPromocionesLineaVenta
        /// </summary>
        /// <param name="folioOperacionAsociada"> Folio operacion</param>
        /// <param name="codeStore">Código Tienda</param>
        /// <param name="codeBox">Código Caja</param>
        /// <param name="secuencia">Secuencia </param>
        /// <param name="importeDescuento">Importe descuent</param>
        /// <param name="codigoPromocionAplicado">Código Promoción aplicado</param>
        /// <param name="descripcionCodigoPromocionAplicado">Descripción Promoción</param>
        /// <param name="formaPagoCodigoPromocionAplicado">Forma de Pago</param>
        /// <returns></returns>
        public OperationResponse PersistirPromocionesLineaVenta(string folioOperacionAsociada, int codeStore, int codeBox, int secuencia
                , decimal importeDescuento, int codigoPromocionAplicado, string descripcionCodigoPromocionAplicado, decimal porcentajeDescuento, int codigoRazonDescuento, string formaPagoCodigoPromocionAplicado)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioOperacion", folioOperacionAsociada);
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@CodigoCaja", codeBox);
            parameters.Add("@SecuenciaLineaTicket", secuencia);
            parameters.Add("@ImporteDescuento", importeDescuento);
            parameters.Add("@CodigoPromocion", codigoPromocionAplicado);
            parameters.Add("@PorcentajeDescuento", porcentajeDescuento);
            parameters.Add("@CodigoRazonDescuento", codigoRazonDescuento);
            parameters.Add("@DescripcionPromocion", descripcionCodigoPromocionAplicado);
            parameters.Add("@FormaPagoPromocion", formaPagoCodigoPromocionAplicado);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_PersistirPromocionesDet]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }

        /// <summary>
        /// Procesar movimiento con tarjeta bancaria del retiro
        /// </summary>
        /// <param name="codigoTienda">Código de tienda</param>
        /// <param name="codigoCaja">Código de caja</param>
        /// <param name="codigoEmpleado">Código de empleado</param>
        /// <param name="autorizacion">Número de autorización bancaria</param>
        /// <param name="numeroTarjeta">Ultimos 4 digitos del número de tarjeta</param>
        /// <param name="request">Objeto petición de procesamiento de movimiento de la tarjeta bancaria</param>
        /// <returns>Resultado de la operacion</returns>
        public OperationResponse ProcesarMovimientoTarjetaBancariaRetiro(int codigoTienda, int codigoCaja, int codigoEmpleado, string autorizacion, string numeroTarjeta, ProcesarMovimientoTarjetaBancariaRequest request)
        {
            // OCG: Los valores de "-1" son por que no se deben de usar en PersistirMovimientoTarjetaBancaria 
            // por que no es movimiento de tarjeta de crédito TCMM
            OperationResponse operationResponse = new OperationResponse();
            operationResponse = this.PersistirMovimientoTarjetaBancaria(codigoTienda, codigoCaja, codigoEmpleado,
              request.Retiro.FolioOperacionAsociada, request.Retiro.CodigoFormaPagoImporte, request.Retiro.Estatus, request.Retiro.SecuenciaFormaPagoImporte,
              request.Retiro.ImporteCashBack * -1, "", 0, 0, request.Retiro.ClasificacionVenta, autorizacion, numeroTarjeta,
              "N", "N");
            return operationResponse;
        }

        /// <summary>
        /// Procesar movimiento con tarjeta bancaria del retiro
        /// </summary>
        /// <param name="codigoTienda">Código de tienda</param>
        /// <param name="codigoCaja">Código de caja</param>
        /// <param name="codigoEmpleado">Código de empleado</param>
        /// <param name="autorizacion">Número de autorización bancaria</param>
        /// <param name="numeroTarjeta">Ultimos 4 digitos del número de tarjeta</param>
        /// <param name="request">Objeto petición de procesamiento de movimiento de la tarjeta bancaria</param>
        /// <returns>Resultado de la operacion</returns>
        public OperationResponse ProcesarMovimientoTarjetaBancariaPuntos(int codigoTienda, int codigoCaja, int codigoEmpleado, string autorizacion, string numeroTarjeta, ProcesarMovimientoTarjetaBancariaRequest request)
        {
            // Las "N" indican que esos parámetros no se deben de tomar en cuenta
            OperationResponse operationResponse = new OperationResponse();
            operationResponse = this.PersistirMovimientoTarjetaBancaria(codigoTienda, codigoCaja, codigoEmpleado,
              request.Puntos.FolioOperacionAsociada, request.Puntos.CodigoFormaPagoImporte, request.Puntos.Estatus, request.Puntos.SecuenciaFormaPagoImporte,
              request.Puntos.ImporteVentaTotal, "", 0, 0, request.Puntos.ClasificacionVenta, autorizacion, numeroTarjeta,
              "N", "N");
            return operationResponse;
        }

        /// <summary>
        /// Actualizar el código de autorización de un movimiento con tarjeta Melody-Milano
        /// </summary>
        /// <param name="codeStore">Código de tienda</param>
        /// <param name="codeBox">Código de caja</param>        
        /// <param name="transaccion">Número de transaccion de la operación</param>     
        /// <param name="autorizacion">Número de autorización bancaria</param>              
        /// <param name="request">Objeto petición de procesamiento de movimiento de la tarjeta bancaria</param>
        /// <returns>Resultado de la operacion</returns>
        public OperationResponse ActualizarMovimientoTarjetaMelodyMilano(int codeStore, int codeBox, int transaccion, string autorizacion, FinalizarCompraRequest request)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@CodigoCaja", codeBox);
            parameters.Add("@Secuencia", request.SecuenciaFormaPagoImporte);
            parameters.Add("@Transaccion", transaccion);
            parameters.Add("@CodigoFormaPago", request.CodigoFormaPagoImporte);
            parameters.Add("@FolioOperacion", request.FolioOperacionAsociada);
            parameters.Add("@Autorizacion", autorizacion);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_ActualizarMovimientoTarjetaMelodyMilano]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }

        /// <summary>
        /// Procesar movimiento con tarjeta Melody-Milano
        /// </summary>
        /// <param name="codigoTienda">Código de tienda</param>
        /// <param name="codigoCaja">Código de caja</param>
        /// <param name="codigoEmpleado">Código de empleado</param>
        /// <param name="autorizacion">Número de autorización bancaria</param>
        /// <param name="numeroTarjeta">Ultimos 4 digitos del número de tarjeta</param>
        /// <param name="request">Objeto petición de procesamiento de movimiento de la tarjeta bancaria</param>
        /// <returns>Resultado de la operacion</returns>
        public OperationResponse ProcesarMovimientoTarjetaMelodyMilano(int codigoTienda, int codigoCaja, int codigoEmpleado, string autorizacion, string numeroTarjeta, FinalizarCompraRequest request)
        {
            // OCG: Aqui si aplica que se pasen los valores de plazo y monto a pagar
            OperationResponse operationResponse = new OperationResponse();
            operationResponse = this.PersistirMovimientoTarjetaBancaria(codigoTienda, codigoCaja, codigoEmpleado,
              request.FolioOperacionAsociada, request.CodigoFormaPagoImporte, request.Estatus, request.SecuenciaFormaPagoImporte,
              request.ImporteVentaTotal, "", 0, 0, request.ClasificacionVenta, autorizacion, numeroTarjeta,
              request.MesesFinanciados.ToString(), request.ImporteVentaTotal.ToString());
            return operationResponse;
        }

        //Referencia principal al procedimiento almacenado
        private OperationResponse PersistirMovimientoTarjetaBancaria(int codeStore, int codeBox, int codigoEmpleado, String folioVenta,
        string codigoFormaPago, string estatus, int secuencia, decimal importeMonedaNacional, string codigoTipoDivisa, decimal importeMonedaExtranjera,
        decimal tasaConversionUtilizada, string clasificacionVenta, string autorizacion, string numeroTarjeta,
        string plazo, string montoPlazo)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@CodigoCaja", codeBox);
            parameters.Add("@FolioOperacion", folioVenta);
            parameters.Add("@CodigoFormaPago", codigoFormaPago);
            parameters.Add("@Estatus", estatus);
            parameters.Add("@Secuencia", secuencia);
            parameters.Add("@CodigoEmpleado", codigoEmpleado);
            parameters.Add("@ImportePago", importeMonedaNacional);
            parameters.Add("@CodigoMoneda", codigoTipoDivisa);
            parameters.Add("@TasaConversion", tasaConversionUtilizada);
            parameters.Add("@Autorizacion", autorizacion);
            parameters.Add("@NumeroTarjeta", numeroTarjeta);
            parameters.Add("@ImporteMonedaExtranjera", importeMonedaExtranjera);
            parameters.Add("@ClasificacionVenta", clasificacionVenta);

            // OCG: Integración de plazo y monto del plazo
            //parameters.Add("@Plazo", plazo);
            //parameters.Add("@MontoPlazo", montoPlazo);

            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@Consecutivo", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_ProcesarMovimientoTransaccionTarjetaBancaria]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            operationResponse.Transaccion = Convert.ToInt32(result["@Consecutivo"]);
            return operationResponse;
        }

        public string spMilano_ActualizaPlazoFinanciamiento(UpdatePlanFinanciamientoTCMM param)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@folioOperacion", param.folioOperacion);
            parameters.Add("@codigoTienda", param.codigoTienda);
            parameters.Add("@codigoCaja", param.codigoCaja);
            parameters.Add("@fechaactualizacion", param.fechaActualizacion);
            parameters.Add("@codigoFormaPago", param.codigoFormaPago);
            parameters.Add("@financiamientoId", param.financiamientoId);
            parameters.Add("@montoMensualidad", param.montoMensualidad);

            data.ExecuteProcedure("spMilano_ActualizaPlazoFinanciamiento", parameters);

            return "";
        }

        private OperationResponse PersistirMovimientoTarjetRegalo(int codeStore, int codeBox, int codigoEmpleado, String folioVenta,
        string codigoFormaPago, string estatus, int secuencia, decimal importeMonedaNacional, string codigoTipoDivisa, decimal importeMonedaExtranjera,
        decimal tasaConversionUtilizada, string clasificacionVenta, string tarjetaRegalo)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@CodigoCaja", codeBox);
            parameters.Add("@FolioOperacion", folioVenta);
            parameters.Add("@CodigoFormaPago", codigoFormaPago);
            parameters.Add("@Estatus", estatus);
            parameters.Add("@Secuencia", secuencia);
            parameters.Add("@CodigoEmpleado", codigoEmpleado);
            parameters.Add("@ImportePago", importeMonedaNacional);
            parameters.Add("@CodigoMoneda", codigoTipoDivisa);
            parameters.Add("@TasaConversion", tasaConversionUtilizada);
            parameters.Add("@ImporteMonedaExtranjera", importeMonedaExtranjera);
            parameters.Add("@ClasificacionVenta", clasificacionVenta);
            parameters.Add("@NumeroTarjetaRegalo", tarjetaRegalo);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_ProcesarMovimientoTransaccionTarjetaRegalo]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }

        private OperationResponse PersistirMovimientoRedencionCuponPromocional(int codeStore, int codeBox, int codigoEmpleado, String folioVenta,
        string codigoFormaPago, string estatus, int secuencia, decimal importeMonedaNacional, string codigoTipoDivisa, decimal importeMonedaExtranjera,
        decimal tasaConversionUtilizada, string folioCuponPromocional)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@CodigoCaja", codeBox);
            parameters.Add("@CodigoEmpleado", codigoEmpleado);
            parameters.Add("@FolioOperacion", folioVenta);
            parameters.Add("@CodigoFormaPago", codigoFormaPago);
            parameters.Add("@Estatus", estatus);
            parameters.Add("@Secuencia", secuencia);
            parameters.Add("@ImportePago", importeMonedaNacional);
            parameters.Add("@CodigoMoneda", codigoTipoDivisa);
            parameters.Add("@ImporteMonedaExtranjera", importeMonedaExtranjera);
            parameters.Add("@TasaConversion", tasaConversionUtilizada);
            parameters.Add("@FolioCuponPromocional", folioCuponPromocional);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@Transaccion", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoTipoTrxCab", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.VarChar, Size = 10 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_ProcesarMovimientoTransaccionRedencionCuponPromocional]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            operationResponse.Transaccion = Convert.ToInt32(result["@Transaccion"]);
            operationResponse.CodigoTipoTrxCab = result["@CodigoTipoTrxCab"].ToString();
            
            return operationResponse;
        }

        /// <summary>
        /// Procesar movimiento con venta a empleado
        /// </summary>
        /// <param name="codigoTienda">Código de tienda</param>
        /// <param name="codigoCaja">Código de caja</param>
        /// <param name="codigoEmpleado">Código de empleado</param>
        /// <param name="request">Objeto petición de procesamiento de movimiento en venta a empleado</param>
        /// <returns>Resultado de la operacion</returns>
        public OperationResponse ProcesarMovimientoVentaEmpleado(int codigoTienda, int codigoCaja, int codigoEmpleado, ProcesarMovimientoVentaEmpleadoRequest request)
        {
            OperationResponse operationResponse = new OperationResponse();
            operationResponse = this.PersistirMovimientoVentaEmpleado(codigoTienda, codigoCaja, codigoEmpleado,
              request.FolioOperacionAsociada, request.CodigoFormaPagoImporte, request.Estatus, request.SecuenciaFormaPagoImporte,
              decimal.Parse(request.MontoFinanciado.ToString()), "0", 0, 0);
            //TODO: Almacenar la transacción hacia File System si aplica
            return operationResponse;
        }

        private OperationResponse PersistirMovimientoVentaEmpleado(int codeStore, int codeBox, int codigoEmpleado, String folioVenta,
        string codigoFormaPago, string estatus, int secuencia, decimal importeMonedaNacional, string codigoTipoDivisa, decimal importeMonedaExtranjera,
        decimal tasaConversionUtilizada)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@CodigoCaja", codeBox);
            parameters.Add("@FolioOperacion", folioVenta);
            parameters.Add("@CodigoFormaPago", codigoFormaPago);
            parameters.Add("@Estatus", estatus);
            parameters.Add("@Secuencia", secuencia);
            parameters.Add("@CodigoEmpleado", codigoEmpleado);
            parameters.Add("@ImportePago", importeMonedaNacional);
            parameters.Add("@CodigoMoneda", codigoTipoDivisa);
            parameters.Add("@TasaConversion", tasaConversionUtilizada);
            parameters.Add("@ImporteMonedaExtranjera", importeMonedaExtranjera);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_ProcesarMovimientoTransaccionVentaEmpleado]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }


        /// <summary>
        /// Procesar forma de pago mayorista
        /// </summary>
        /// <param name="codigoTienda">Código de tienda</param>
        /// <param name="codigoCaja">Código de caja</param>
        /// <param name="codigoEmpleado">Código empleado</param>
        /// <param name="request">Dto con datos del pago a mayorista</param>
        /// <returns>Resultado de la operación</returns>
        public OperationResponse ProcesarMovimientoMayorista(int codigoTienda, int codigoCaja, int codigoEmpleado, ProcesarMovimientoMayorista request)
        {
            OperationResponse operationResponse = new OperationResponse();
            operationResponse = this.PersistirMovimientoMayorista(codigoTienda, codigoCaja, codigoEmpleado,
              request.FolioOperacionAsociada, request.CodigoFormaPagoImporte, request.Estatus, request.SecuenciaFormaPagoImporte,
              decimal.Parse(request.MontoFinanciado.ToString()), "0", 0, 0, request.NumeroVale.ToString(), request.CodigoClienteFinal, request.NombreClienteFinal);
            //TODO: Almacenar la transacción hacia File System si aplica
            return operationResponse;
        }

        private OperationResponse PersistirMovimientoMayorista(int codeStore, int codeBox, int codigoEmpleado, String folioVenta,
        string codigoFormaPago, string estatus, int secuencia, decimal importeMonedaNacional, string codigoTipoDivisa, decimal importeMonedaExtranjera,
        decimal tasaConversionUtilizada, string numeroVale, int codigoClienteFinal, string nombreClienteFinal)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@CodigoCaja", codeBox);
            parameters.Add("@FolioOperacion", folioVenta);
            parameters.Add("@CodigoFormaPago", codigoFormaPago);
            parameters.Add("@Estatus", estatus);
            parameters.Add("@Secuencia", secuencia);
            parameters.Add("@CodigoEmpleado", codigoEmpleado);
            parameters.Add("@ImportePago", importeMonedaNacional);
            parameters.Add("@CodigoMoneda", codigoTipoDivisa);
            parameters.Add("@TasaConversion", tasaConversionUtilizada);
            parameters.Add("@ImporteMonedaExtranjera", importeMonedaExtranjera);
            parameters.Add("@NumeroVale", numeroVale);
            parameters.Add("@CodigoClienteFinal", codigoClienteFinal);
            parameters.Add("@NombreClienteFinal", nombreClienteFinal);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_ProcesarMovimientoTransaccionMayorista]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }

        /// <summary>
        /// Validar cuando una nota de credito se quiere redemir el mismo dia
        /// </summary>
        /// <param name="codeStore"></param>
        /// <param name="codeBox"></param>
        /// <param name="folioNotaCredito"></param>
        /// <returns></returns>
        public OperationResponse ValidacionRedimirNotaCredito(int codeStore, int codeBox, string folioNotaCredito)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@CodigoCaja", codeBox);
            parameters.Add("@FolioNotaCredito", folioNotaCredito);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_ValidacionNotaCreditoOffline]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }

        /// <summary>
        /// Procesar forma de pago Nota de Crédito
        /// </summary>
        /// <param name="codeStore">Código de tienda</param>
        /// <param name="codeBox">Código de caja</param>
        /// <param name="codeEmployee">Código empleado</param>
        /// <param name="procesarMovimientoNotaCreditoRequest">DTO con la información del pago</param>
        /// <param name="redimirNotaOffline">Inidica si debe redimirse de forma offline</param>
        /// <returns>Resultado de la operación</returns>
        public OperationResponse PersistirMovimientoNotaCredito(int codeStore, int codeBox, int codeEmployee, ProcesarMovimientoNotaCreditoRequest procesarMovimientoNotaCreditoRequest, int redimirNotaOffline)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@CodigoCaja", codeBox);
            parameters.Add("@CodigoEmpleado", codeEmployee);
            parameters.Add("@FolioOperacion", procesarMovimientoNotaCreditoRequest.FolioOperacionAsociada);
            parameters.Add("@CodigoFormaPago", procesarMovimientoNotaCreditoRequest.CodigoFormaPagoImporte);
            parameters.Add("@Estatus", procesarMovimientoNotaCreditoRequest.Estatus);
            parameters.Add("@Secuencia", procesarMovimientoNotaCreditoRequest.SecuenciaFormaPagoImporte);
            parameters.Add("@ImportePago", procesarMovimientoNotaCreditoRequest.ImporteVentaTotal);
            parameters.Add("@FolioNotaCredito", procesarMovimientoNotaCreditoRequest.FolioNotaCredito);
            parameters.Add("@RedimirNotaOffline", redimirNotaOffline);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@Transaccion", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoTipoTrxCab", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.VarChar, Size = 10 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_ProcesarMovimientoTransaccionNotaCredito]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            operationResponse.Transaccion = Convert.ToInt32(result["@Transaccion"]);
            operationResponse.CodigoTipoTrxCab = result["@CodigoTipoTrxCab"].ToString();
            return operationResponse;
        }

        /// <summary>
        /// Mensaje de error de vale no disponible
        /// </summary>
        /// <returns></returns>
        public OperationResponse ObtenerMensajeValeNoDisponible()
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("sp_vanti_MensajeValeMayoristaNoDisponible", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }










        /// <summary>
        /// Procesar movimiento PinPad Móvil
        /// </summary>
        /// <param name="codigoTienda">Código de tienda</param>
        /// <param name="codigoCaja">Código de caja</param>
        /// <param name="codigoEmpleado">Código de empleado</param>
        /// <param name="request">Objeto petición de procesamiento de movimiento en tarjeta de regalo</param>
        /// <returns>Resultado de la operacion</returns>
        public OperationResponse ProcesarPagoPinPadMovil(int codigoTienda, int codigoCaja, int codigoEmpleado, ProcesarMovimientoPagoPinPadMovilRequest request)
        {
            OperationResponse operationResponse = new OperationResponse();
            operationResponse = this.PersistirPagoPinPadMovil(codigoTienda, codigoCaja, codigoEmpleado, request.FolioOperacionAsociada, request.CodigoFormaPagoImporte,
                                   request.Estatus, request.SecuenciaFormaPagoImporte, request.ImporteVentaTotal, "0", 0, 0, request.FolioAutorizacionPinPadMovil);
            return operationResponse;
        }

        private OperationResponse PersistirPagoPinPadMovil(int codeStore, int codeBox, int codigoEmpleado, String folioVenta,
        string codigoFormaPago, string estatus, int secuencia, decimal importeMonedaNacional, string codigoTipoDivisa, decimal importeMonedaExtranjera,
        decimal tasaConversionUtilizada, string folioAutorizacionPinPadMovil)
        {
            //TODO: Revisar
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@CodigoCaja", codeBox);
            parameters.Add("@FolioOperacion", folioVenta);
            parameters.Add("@CodigoFormaPago", codigoFormaPago);
            parameters.Add("@Estatus", estatus);
            parameters.Add("@Secuencia", secuencia);
            parameters.Add("@CodigoEmpleado", codigoEmpleado);
            parameters.Add("@ImportePago", importeMonedaNacional);
            parameters.Add("@CodigoMoneda", codigoTipoDivisa);
            parameters.Add("@TasaConversion", tasaConversionUtilizada);
            parameters.Add("@ImporteMonedaExtranjera", importeMonedaExtranjera);
            parameters.Add("@FolioAutorizacionPinPadMovil", folioAutorizacionPinPadMovil);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_ProcesarMovimientoPagoPinPadMovil]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }


        /// <summary>
        /// OCG: Registrar pago realizado por una transferencia
        /// </summary>
        /// <param name="codigoTienda"></param>
        /// <param name="codigoCaja"></param>
        /// <param name="codigoEmpleado"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public OperationResponse ProcesarPagoTransferencia(int codigoTienda, int codigoCaja, int codigoEmpleado, ProcesarMovimientoPagoTransferenciaRequest request)
        {
            OperationResponse operationResponse = new OperationResponse();
            operationResponse = this.PersistirPagoTransferencia(codigoTienda, codigoCaja, codigoEmpleado,
            request.FolioOperacionAsociada, request.CodigoFormaPagoImporte, request.Estatus, request.SecuenciaFormaPagoImporte,
            request.ImporteVentaTotal, "0", 0, 0, request.FolioAutorizacionPinPadMovil);
            return operationResponse;
        }

        /// <summary>
        ///  OCG: Registrar pago realizado por una transferencia
        /// </summary>
        /// <param name="codeStore"></param>
        /// <param name="codeBox"></param>
        /// <param name="codigoEmpleado"></param>
        /// <param name="folioVenta"></param>
        /// <param name="codigoFormaPago"></param>
        /// <param name="estatus"></param>
        /// <param name="secuencia"></param>
        /// <param name="importeMonedaNacional"></param>
        /// <param name="codigoTipoDivisa"></param>
        /// <param name="importeMonedaExtranjera"></param>
        /// <param name="tasaConversionUtilizada"></param>
        /// <param name="folioAutorizacionPinPadMovil"></param>
        /// <returns></returns>
        private OperationResponse PersistirPagoTransferencia(int codeStore, int codeBox, int codigoEmpleado, String folioVenta,
      string codigoFormaPago, string estatus, int secuencia, decimal importeMonedaNacional, string codigoTipoDivisa, decimal importeMonedaExtranjera,
      decimal tasaConversionUtilizada, string folioAutorizacionPinPadMovil)
        {
            //TODO: Revisar
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@CodigoCaja", codeBox);
            parameters.Add("@FolioOperacion", folioVenta);
            parameters.Add("@CodigoFormaPago", codigoFormaPago);
            parameters.Add("@Estatus", estatus);
            parameters.Add("@Secuencia", secuencia);
            parameters.Add("@CodigoEmpleado", codigoEmpleado);
            parameters.Add("@ImportePago", importeMonedaNacional);
            parameters.Add("@CodigoMoneda", codigoTipoDivisa);
            parameters.Add("@TasaConversion", tasaConversionUtilizada);
            parameters.Add("@ImporteMonedaExtranjera", importeMonedaExtranjera);
            parameters.Add("@FolioAutorizacionPinPadMovil", folioAutorizacionPinPadMovil);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_ProcesarMovimientoPagoTransferencia]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }




    }

}
