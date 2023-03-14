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
    /// Repositorio de apartado
    /// </summary>
    public class ApartadoRepository : BaseRepository
    {

        /// <summary>
        /// Anular Venta
        /// </summary>
        /// <param name="anularTotalizarApartadoRequest">Parametro con el folio de la venta</param>
        /// <param name="token">Token del usuario activo</param>
        /// <returns></returns>
        public TransApartadoResponse AnularApartado(AnularTotalizarApartadoRequest anularTotalizarApartadoRequest, TokenDto token)
        {
            TransApartadoResponse operationResponse = new TransApartadoResponse();
            using (TransactionScope scope = new TransactionScope())
            {
                operationResponse = this.AnularTransaccionApartado(anularTotalizarApartadoRequest, token);
                // TODO: Implementar lógica TransactionLogRepository
                //new TransactionLogRepository<AnularTotalizarVentaRequest>(token).Add(anularTotalizarVentaRequest, anularTotalizarVentaRequest.FolioVenta);                
                scope.Complete();
            }
            return operationResponse;
        }

        private TransApartadoResponse AnularTransaccionApartado(AnularTotalizarApartadoRequest anularTotalizarVentaRequest, TokenDto token)
        {
            TransApartadoResponse operationResponse = new TransApartadoResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioApartado", anularTotalizarVentaRequest.FolioApartado);
            parameters.Add("@CodigoTienda", token.CodeStore);
            parameters.Add("@CodigoCaja", token.CodeBox);
            parameters.Add("@CodigoRazon", anularTotalizarVentaRequest.CodigoRazon);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@FolioVenta", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });

            var result = this.data.ExecuteProcedure("[dbo].[sp_vanti_AnularTransaccionApartado]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            operationResponse.FolioVenta = result["@FolioVenta"].ToString();

            return operationResponse;
        }

        /// <summary>
        /// Cambio de Piezas de una Linea Ticket
        /// </summary>        
        /// <param name="codeStore">Código de tienda</param>      
        /// <param name="codeBox">Código de caja</param>      
        /// <param name="codeEmployee">Código de empleado cajero</param>  
        /// <param name="lineaTicket">Objeto de peticion linea ticket de la venta</param>        
        /// <returns>Resultado de la operación</returns>
        public OperationResponse CambiarPiezasLineaTicketVenta(int codeStore, int codeBox, int codeEmployee, LineaTicket lineaTicket)
        {
            OperationResponse operationResponse = new OperationResponse();
            using (TransactionScope scope = new TransactionScope())
            {
                var parameters = new Dictionary<string, object>();
                parameters.Add("@FolioOperacion", lineaTicket.cabeceraVentaRequest.FolioOperacion);
                parameters.Add("@CodigoTienda", codeStore);
                parameters.Add("@CodigoCaja", codeBox);
                parameters.Add("@Secuencia", lineaTicket.Secuencia);
                parameters.Add("@CantidadVendida", lineaTicket.CantidadVendida);
                parameters.Add("@CantidadDevuelta", lineaTicket.CantidadDevuelta);
                parameters.Add("@TasaImpuesto1", lineaTicket.Articulo.TasaImpuesto1);
                parameters.Add("@TasaImpuesto2", lineaTicket.Articulo.TasaImpuesto2);
                parameters.Add("@ImporteImpuesto1", lineaTicket.ImporteVentaLineaImpuestos1);
                parameters.Add("@ImporteImpuesto2", lineaTicket.ImporteVentaLineaImpuestos2);
                parameters.Add("@CostoUnitario", lineaTicket.Articulo.PrecioConImpuestos);
                parameters.Add("@CodigoImpuesto1", lineaTicket.Articulo.CodigoImpuesto1);
                parameters.Add("@CodigoImpuesto2", lineaTicket.Articulo.CodigoImpuesto2);
                parameters.Add("@ImporteTotal", lineaTicket.ImporteVentaLineaNeto);
                List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
                parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
                parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
                var result = this.data.ExecuteProcedure("[dbo].[sp_vanti_CambiarPiezasLineaTicketApartado]", parameters, parametersOut);
                operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
                operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
                // Actualizar la cabecera de Venta
                this.ActualizarCabeceraApartado(codeStore, codeBox, codeEmployee, lineaTicket.cabeceraVentaRequest);
                scope.Complete();
            }
            return operationResponse;
        }

        /// <summary>
        /// Cambio de Precio de una Linea Ticket
        /// </summary>        
        /// <param name="codeStore">Código de tienda</param>      
        /// <param name="codeBox">Código de caja</param>      
        /// <param name="codeEmployee">Código de empleado cajero</param>  
        /// <param name="cambiarPrecioRequest">Objeto de peticion linea ticket de la venta</param>        
        /// <returns>Resultado de la operación</returns>
        public OperationResponse CambiarPrecioLineaTicketVenta(int codeStore, int codeBox, int codeEmployee, CambiarPrecioRequest cambiarPrecioRequest)
        {
            OperationResponse operationResponse = new OperationResponse();
            using (TransactionScope scope = new TransactionScope())
            {
                var parameters = new Dictionary<string, object>();
                parameters.Add("@FolioOperacion", cambiarPrecioRequest.LineaTicket.cabeceraVentaRequest.FolioOperacion);
                parameters.Add("@CodigoTienda", codeStore);
                parameters.Add("@CodigoCaja", codeBox);
                parameters.Add("@Secuencia", cambiarPrecioRequest.LineaTicket.Secuencia);
                parameters.Add("@CantidadVendida", cambiarPrecioRequest.LineaTicket.CantidadVendida);
                parameters.Add("@CantidadDevuelta", cambiarPrecioRequest.LineaTicket.CantidadDevuelta);
                parameters.Add("@ImporteImpuesto1", cambiarPrecioRequest.LineaTicket.ImporteVentaLineaImpuestos1);
                parameters.Add("@ImporteImpuesto2", cambiarPrecioRequest.LineaTicket.ImporteVentaLineaImpuestos2);
                parameters.Add("@CostoUnitario", cambiarPrecioRequest.LineaTicket.Articulo.PrecioConImpuestos);
                parameters.Add("@ImporteTotal", cambiarPrecioRequest.LineaTicket.ImporteVentaLineaNeto);
                parameters.Add("@PrecioCambiadoConImpuestos", cambiarPrecioRequest.LineaTicket.Articulo.PrecioCambiadoConImpuestos);
                parameters.Add("@PrecioCambiadoImpuesto1", cambiarPrecioRequest.LineaTicket.Articulo.precioCambiadoImpuesto1);
                parameters.Add("@PrecioCambiadoImpuesto2", cambiarPrecioRequest.LineaTicket.Articulo.precioCambiadoImpuesto2);
                parameters.Add("@CodigoRazon", cambiarPrecioRequest.CodigoRazon);
                List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
                parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
                parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
                var result = this.data.ExecuteProcedure("[dbo].[sp_vanti_CambiarPrecioLineaTicketApartado]", parameters, parametersOut);
                operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
                operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
                // Actualizar la cabecera de Venta
                this.ActualizarCabeceraApartado(codeStore, codeBox, codeEmployee, cambiarPrecioRequest.LineaTicket.cabeceraVentaRequest);
                scope.Complete();
            }
            return operationResponse;
        }


        /// <summary>
        /// Eliminación de una Linea Ticket
        /// </summary>        
        /// <param name="codeStore">Código de tienda</param>      
        /// <param name="codeBox">Código de caja</param>      
        /// <param name="codeEmployee">Código de empleado cajero</param>  
        /// <param name="secuenciaOriginalLineaTicket">Secuencia original de la línea eliminada</param>  
        /// <param name="lineaTicket">Objeto de peticion linea ticket de la venta</param>        
        /// <returns>Resultado de la operación</returns>
        public OperationResponse EliminarLineaTicketApartado(int codeStore, int codeBox, int codeEmployee, int secuenciaOriginalLineaTicket, LineaTicket lineaTicket)
        {
            OperationResponse operationResponse = new OperationResponse();
            using (TransactionScope scope = new TransactionScope())
            {
                // Actualizar el estatus de la linea que fue eliminada (eliminación lógica)
                var parameters = new Dictionary<string, object>();
                parameters.Add("@FolioOperacion", lineaTicket.cabeceraVentaRequest.FolioOperacion);
                parameters.Add("@CodigoTienda", codeStore);
                parameters.Add("@CodigoCaja", codeBox);
                parameters.Add("@Secuencia", secuenciaOriginalLineaTicket);
                List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
                var result = this.data.ExecuteProcedure("[dbo].[sp_vanti_ActualizarEstatusLineaTicketApartado]", parameters, parametersOut);
                // Agregar la nueva linea con transacción de tipo 87
                parameters = new Dictionary<string, object>();
                parameters.Add("@FolioApartado", lineaTicket.cabeceraVentaRequest.FolioOperacion);
                parameters.Add("@CodigoTienda", codeStore);
                parameters.Add("@CodigoCaja", codeBox);
                parameters.Add("@Secuencia", lineaTicket.Secuencia);
                parameters.Add("@TipoTransaccion", lineaTicket.TipoDetalleVenta);
                parameters.Add("@Sku", lineaTicket.Articulo.Sku);
                parameters.Add("@CantidadVendida", (lineaTicket.CantidadVendida * -1));
                parameters.Add("@CantidadDevuelta", 0);
                parameters.Add("@Upc", lineaTicket.Articulo.Upc);
                parameters.Add("@TasaImpuesto1", lineaTicket.Articulo.TasaImpuesto1);
                parameters.Add("@TasaImpuesto2", lineaTicket.Articulo.TasaImpuesto2);
                parameters.Add("@ImporteImpuesto1", (lineaTicket.ImporteVentaLineaImpuestos1 * -1));
                parameters.Add("@ImporteImpuesto2", (lineaTicket.ImporteVentaLineaImpuestos2 * -1));
                parameters.Add("@CostoUnitario", (lineaTicket.Articulo.PrecioConImpuestos * -1));
                parameters.Add("@CodigoImpuesto1", lineaTicket.Articulo.CodigoImpuesto1);
                parameters.Add("@CodigoImpuesto2", lineaTicket.Articulo.CodigoImpuesto2);
                parameters.Add("@ImporteSubtotal", (lineaTicket.ImporteVentaLineaBruto * -1));
                parameters.Add("@ImporteTotal", (lineaTicket.ImporteVentaLineaNeto * -1));
                //Si se trata de una venta de Tiempo Aire se agrega la referencia
                if (lineaTicket.cabeceraVentaRequest.TipoCabeceraVenta == "5")
                {
                    parameters.Add("@Referencia", lineaTicket.Articulo.InformacionProveedorExternoTA.SkuCompania);
                }
                parametersOut = new List<System.Data.SqlClient.SqlParameter>();
                parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
                parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
                result = this.data.ExecuteProcedure("[dbo].[sp_vanti_AgregarLineaTicketApartado]", parameters, parametersOut);
                operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
                operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
                // Actualizar la cabecera de Venta
                this.ActualizarCabeceraApartado(codeStore, codeBox, codeEmployee, lineaTicket.cabeceraVentaRequest);
                scope.Complete();
            }
            return operationResponse;
        }


        /// <summary>
        /// Almacenamiento de una Linea Ticket
        /// </summary>        
        /// <param name="codeStore">Código de tienda</param>      
        /// <param name="codeBox">Código de caja</param>      
        /// <param name="codeEmployee">Código de empleado cajero</param>  
        /// <param name="lineaTicket">Objeto de peticion linea ticket de la venta</param>        
        /// <returns>Resultado de la operación</returns>
        public OperacionLineaTicketVentaResponse AgregarLineaTicketVenta(int codeStore, int codeBox, int codeEmployee, LineaTicket lineaTicket)
        {
            OperacionLineaTicketVentaResponse operacionLineaTicketVentaResponse = new OperacionLineaTicketVentaResponse();
            using (TransactionScope scope = new TransactionScope())
            {
                operacionLineaTicketVentaResponse.FolioOperacion = lineaTicket.cabeceraVentaRequest.FolioOperacion;
                if (operacionLineaTicketVentaResponse.FolioOperacion == "")
                {
                    operacionLineaTicketVentaResponse.FolioOperacion = this.AgregarCabeceraApartado(codeStore, codeBox,
                        codeEmployee, lineaTicket.cabeceraVentaRequest);
                }
                this.AgregarDetalleApartado(operacionLineaTicketVentaResponse.FolioOperacion, codeStore, codeBox,
                    lineaTicket.cabeceraVentaRequest.TipoCabeceraVenta, lineaTicket);
                this.ActualizarCabeceraApartado(codeStore, codeBox, codeEmployee, lineaTicket.cabeceraVentaRequest);
                scope.Complete();
            }
            return operacionLineaTicketVentaResponse;
        }


        private string AgregarCabeceraApartado(int codigoTienda, int codigoCaja, int codigoEmpleadoCajero, CabeceraVentaRequest cabeceraVentaRequest)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", codigoTienda);
            parameters.Add("@CodigoCaja", codigoCaja);
            parameters.Add("@CodigoEmpleado", codigoEmpleadoCajero);
            parameters.Add("@SubTotal", cabeceraVentaRequest.ImporteVentaBruto);
            parameters.Add("@Iva", cabeceraVentaRequest.ImporteVentaImpuestos);
            parameters.Add("@Total", cabeceraVentaRequest.ImporteVentaNeto);
            parameters.Add("@CodigoCliente", cabeceraVentaRequest.CodigoCliente);
            parameters.Add("@TipoTransaccion", cabeceraVentaRequest.TipoCabeceraVenta);
            if (cabeceraVentaRequest.CodigoEmpleadoVendedor > 0)
            {
                parameters.Add("@CodigoEmpleadoVendedor", cabeceraVentaRequest.CodigoEmpleadoVendedor);
            }
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@FolioApartado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_AgregarCabeceraApartado]", parameters, parametersOut);
            return result["@FolioApartado"].ToString();
        }

        private void AgregarDetalleApartado(string folioApartado, int codigoTienda, int codigoCaja, string tipoCabeceraVenta, LineaTicket lineaTicket)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioApartado", folioApartado);
            parameters.Add("@CodigoTienda", codigoTienda);
            parameters.Add("@CodigoCaja", codigoCaja);
            parameters.Add("@Secuencia", lineaTicket.Secuencia);
            parameters.Add("@TipoTransaccion", lineaTicket.TipoDetalleVenta);
            parameters.Add("@Sku", lineaTicket.Articulo.Sku);
            parameters.Add("@CantidadVendida", lineaTicket.CantidadVendida);
            parameters.Add("@CantidadDevuelta", lineaTicket.CantidadDevuelta);
            parameters.Add("@Upc", lineaTicket.Articulo.Upc);
            parameters.Add("@TasaImpuesto1", lineaTicket.Articulo.TasaImpuesto1);
            parameters.Add("@TasaImpuesto2", lineaTicket.Articulo.TasaImpuesto2);
            parameters.Add("@ImporteImpuesto1", lineaTicket.ImporteVentaLineaImpuestos1);
            parameters.Add("@ImporteImpuesto2", lineaTicket.ImporteVentaLineaImpuestos2);
            parameters.Add("@CostoUnitario", lineaTicket.Articulo.PrecioConImpuestos);
            parameters.Add("@CodigoImpuesto1", lineaTicket.Articulo.CodigoImpuesto1);
            parameters.Add("@CodigoImpuesto2", lineaTicket.Articulo.CodigoImpuesto2);
            parameters.Add("@ImporteTotal", lineaTicket.ImporteVentaLineaNeto);
            parameters.Add("@ImporteSubTotal", lineaTicket.ImporteVentaLineaBruto);
            parameters.Add("@DigitoVerificadorCorrecto", lineaTicket.Articulo.DigitoVerificadorArticulo.DigitoVerificadorCorrecto);
            parameters.Add("@DigitoVerificadorLeido", lineaTicket.Articulo.DigitoVerificadorArticulo.DigitoVerificadorActual);
            //Si se trata de una venta de Tiempo Aire se agrega la referencia
            if (tipoCabeceraVenta == "5")
            {
                parameters.Add("@Referencia", lineaTicket.Articulo.InformacionProveedorExternoTA.SkuCompania);
            }
            if (lineaTicket.Articulo.InformacionTarjetaRegalo != null)
            {
                parameters.Add("@FolioTarjetaRegalo", lineaTicket.Articulo.InformacionTarjetaRegalo.FolioTarjeta);
                parameters.Add("@DescripcionTarjetaRegalo", lineaTicket.Articulo.InformacionTarjetaRegalo.Descripcion);
            }
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            data.ExecuteProcedure("[dbo].[sp_vanti_AgregarLineaTicketApartado]", parameters, parametersOut);
        }

        private void ActualizarCabeceraApartado(int codigoTienda, int codigoCaja, int codigoEmpleadoCajero, CabeceraVentaRequest cabeceraVentaRequest)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioOperacion", cabeceraVentaRequest.FolioOperacion);
            parameters.Add("@CodigoTienda", codigoTienda);
            parameters.Add("@CodigoCaja", codigoCaja);
            parameters.Add("@CodigoEmpleado", codigoEmpleadoCajero);
            parameters.Add("@SubTotal", cabeceraVentaRequest.ImporteVentaBruto);
            parameters.Add("@Iva", cabeceraVentaRequest.ImporteVentaImpuestos);
            parameters.Add("@Total", cabeceraVentaRequest.ImporteVentaNeto);
            if (cabeceraVentaRequest.CodigoEmpleadoVendedor > 0)
            {
                parameters.Add("@CodigoEmpleadoVendedor", cabeceraVentaRequest.CodigoEmpleadoVendedor);
            }
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            this.data.ExecuteProcedure("[dbo].[sp_vanti_ActualizarCabeceraApartado]", parameters, parametersOut);
        }

        /// <summary>
        /// Totalizar Venta
        /// </summary>
        /// <param name="totalizarVentaRequest">Petición de totalización de venta</param>        
        /// <param name="codeStore">Código de tienda</param>      
        /// <param name="codeBox">Código de caja</param>      
        /// <param name="codeEmployee">Código de empleado cajero</param>  
        /// <returns>Folio de Venta</returns>
        public TotalizarApartadoResponse TotalizarApartado(TotalizarApartadoRequest totalizarVentaRequest, int codeStore, int codeBox, int codeEmployee)
        {
            FormasPagoRepository formasPagoRepository = new FormasPagoRepository();

            TotalizarApartadoResponse totalizarVentaResponse = new TotalizarApartadoResponse();
            using (TransactionScope scope = new TransactionScope())
            {
                totalizarVentaResponse.FolioOperacion = totalizarVentaRequest.cabeceraVentaRequest.FolioOperacion;
                // Actualizar el estatus de la venta
                var parameters = new Dictionary<string, object>();
                parameters.Add("@FolioOperacion", totalizarVentaResponse.FolioOperacion);
                parameters.Add("@CodigoTienda", codeStore);
                parameters.Add("@CodigoCaja", codeBox);
                List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
                this.data.ExecuteProcedure("[dbo].[sp_vanti_TotalizarApartado]", parameters, parametersOut);
                // Información acerca de las formas de pago que deben mostrarse en el Front
                totalizarVentaResponse.InformacionAsociadaFormasPago = formasPagoRepository.GetConfigFormasPago(codeBox, codeStore, totalizarVentaRequest.cabeceraVentaRequest.FolioOperacion, totalizarVentaRequest.cabeceraVentaRequest.FolioDevolucion, totalizarVentaRequest.cabeceraVentaRequest.TipoCabeceraVenta);
                totalizarVentaResponse.InformacionAsociadaFormasPagoMonedaExtranjera = formasPagoRepository.GetConfigFormasPagoExt(codeBox, codeStore, totalizarVentaRequest.cabeceraVentaRequest.FolioOperacion, totalizarVentaRequest.cabeceraVentaRequest.FolioDevolucion, totalizarVentaRequest.cabeceraVentaRequest.TipoCabeceraVenta);
                // Actualizar la cabecera                
                this.ActualizarCabeceraApartado(codeStore, codeBox, codeEmployee, totalizarVentaRequest.cabeceraVentaRequest);
                scope.Complete();
            }
            return totalizarVentaResponse;
        }

        /// <summary>
        /// Finalizar apartado
        /// </summary>
        /// <param name="codeStore"></param>
        /// <param name="codeBox"></param>
        /// <param name="codeEmployee"></param>
        /// <param name="request"></param>
        /// 
        /// <returns></returns>
        public TransApartadoResponse FinalizarApartado(int codeStore, int codeBox, int codeEmployee, FinalizarApartadoRequest request)
        {
            TransApartadoResponse operationResponse = new TransApartadoResponse();
            new FormasPagoRepository().AsociarFormasPago(codeStore, codeBox, codeEmployee, request.FolioApartado, request.FormasPagoUtilizadas, "APARTADO");
            operationResponse = this.FinalizarTransaccionApartado(codeStore, codeBox, request);
            // Si la venta fue procesada exitosamente se almacena el log de transacción
            if (operationResponse.CodeNumber.Equals("342"))
            {
                // TODO: Falta llenar el objeto transacción
                //TransactionLogRepository<Transaccion> transactionLogRepository = new TransactionLogRepository<Transaccion>(new TokenDto(codeStore, codeBox));
                //Transaccion ventaFinalizada = new Transaccion();
                if (request.LineasConDigitoVerificadorIncorrecto != null)
                {
                    this.RegistrarArticulosConIncidenciaDigitoVerificador(request.FolioApartado, codeStore, codeBox, request.LineasConDigitoVerificadorIncorrecto);

                }
                //transactionLogRepository.Add(ventaFinalizada, codeStore + "." + codeBox + "." + finalizarVentaRequest.FolioVenta);
            }
            return operationResponse;
        }

        private TransApartadoResponse FinalizarTransaccionApartado(int codeStore, int codeBox, FinalizarApartadoRequest finalizarApartadoRequest)
        {
            TransApartadoResponse operationResponse = new TransApartadoResponse();
            InformacionAsociadaRetiroEfectivo informacionAsociadaRetiroEfectivo = new InformacionAsociadaRetiroEfectivo();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioApartado", finalizarApartadoRequest.FolioApartado);
            parameters.Add("@CodigoCaja", codeBox);
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@TotalPagosRealizados", finalizarApartadoRequest.FormasPagoUtilizadas.Length);
            parameters.Add("@DiasVencimiento", finalizarApartadoRequest.DiasVencimiento);
            parameters.Add("@ImportePago", finalizarApartadoRequest.ImportePagado);
            parameters.Add("@Saldo", finalizarApartadoRequest.Saldo);
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

            var result = data.ExecuteProcedure("[sp_vanti_FinalizarTransaccionApartado]", parameters, parametersOut);
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

        /// <summary>
        /// Obtener detalle de apartado de pago por folio
        /// </summary>
        /// <param name="request">Folio del apartado</param>
        /// <param name="codeStore">Código de la tienda</param>
        /// <param name="codeBox">Código de la caja</param>
        /// <returns></returns>
        public ApartadoResponse BuscarPorFolio(ApartadoBusquedaRequest request, int codeStore, int codeBox)
        {
            ApartadoResponse apartadoResponse = new ApartadoResponse();
            FormasPagoRepository formasPagoRepository = new FormasPagoRepository();
            List<ApartadosEncontradosResponse> lista = new List<ApartadosEncontradosResponse>();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioApartado", request.FolioApartado);
            parameters.Add("@Telefono", request.Telefono);
            parameters.Add("@Nombre", request.Nombre);
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@CodigoCaja", codeBox);
            foreach (var item in data.GetDataReader("[dbo].[sp_vanti_server_BuscaApartadoCabPorFolio]", parameters))
            {
                ApartadosEncontradosResponse apartado = new ApartadosEncontradosResponse();
                apartado.CodigoTienda = Convert.ToInt32(item.GetValue(0));
                apartado.CodigoCaja = Convert.ToInt32(item.GetValue(1));
                apartado.CodigoCliente = Convert.ToInt64(item.GetValue(2));
                apartado.Estatus = item.GetValue(3).ToString();
                apartado.ImportePagado = Convert.ToDecimal(item.GetValue(4));
                apartado.Saldo = Convert.ToDecimal(item.GetValue(5).ToString());
                apartado.DiasVencimiento = Convert.ToInt32(item.GetValue(6));
                apartado.TipoCabeceraApartado = item.GetValue(7).ToString();
                apartado.CodigoEmpleado = Convert.ToInt32(item.GetValue(8).ToString());
                apartado.CodigoEmpleadoVendedor = Convert.ToInt32(item.GetValue(9).ToString());
                apartado.ImporteApartadoBruto = Convert.ToDecimal(item.GetValue(10));
                apartado.ImporteApartadoImpuestos = Convert.ToDecimal(item.GetValue(11));
                apartado.ImporteApartadoNeto = Convert.ToDecimal(item.GetValue(12));
                apartado.ConsecutivoSecuenciaFormasPago = Convert.ToInt32(item.GetValue(13));
                apartado.FolioApartado = item.GetValue(14).ToString();
                apartado.NombreCliente = item.GetValue(15).ToString();
                apartado.TelefonoCliente = item.GetValue(16).ToString();
                apartado.FechaVencimiento = item.GetValue(17).ToString();
                apartado.FechaCancelacion = item.GetValue(18).ToString();
                lista.Add(apartado);
            }
            apartadoResponse.Apartados = lista.ToArray();
            if (apartadoResponse.Apartados.Length > 0)
            {
                // Información acerca de las formas de pago que deben mostrarse en el Front
                apartadoResponse.InformacionAsociadaFormasPago = formasPagoRepository.GetConfigFormasPago(codeBox, codeStore, "", "", apartadoResponse.Apartados[0].TipoCabeceraApartado);
                apartadoResponse.InformacionAsociadaFormasPagoMonedaExtranjera = formasPagoRepository.GetConfigFormasPagoExt(codeBox, codeStore, "", "", apartadoResponse.Apartados[0].TipoCabeceraApartado);
            }
            return apartadoResponse;
        }


        /// <summary>
        /// Cancelar apartado
        /// </summary>
        /// <param name="folioApartado">folio de apartado</param>
        /// <param name="codigoCaja">código de la caja</param>
        /// <param name="codigoTienda">código de la tienda</param>
        /// <returns>Respuesta de la operación</returns>
        public TransApartadoResponse CancelarApartado(string folioApartado, int codigoCaja, int codigoTienda, int codigoEmpleado)
        {
            TransApartadoResponse transApartadoResponse = new TransApartadoResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioApartado", folioApartado);
            parameters.Add("@CodigoCaja", codigoCaja);
            parameters.Add("@CodigoTienda", codigoTienda);
            parameters.Add("@CodigoEmpleado", codigoEmpleado);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@FolioVenta", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@FolioNotaCredito", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_server_CancelarApartado]", parameters, parametersOut);
            transApartadoResponse.CodeNumber = result["@CodigoResultado"].ToString();
            transApartadoResponse.CodeDescription = result["@MensajeResultado"].ToString();
            transApartadoResponse.FolioVenta = result["@FolioVenta"].ToString();
            transApartadoResponse.FolioNotaCreditoGenerada = result["@FolioNotaCredito"].ToString();
            return transApartadoResponse;
        }

        /// <summary>
        /// Obtener los plazos de apartado
        /// </summary>
        /// <returns></returns>
        public ApartadoPlazosResponse[] ObtenerPlazos()
        {
            var parameters = new Dictionary<string, object>();
            List<ApartadoPlazosResponse> list = new List<ApartadoPlazosResponse>();
            foreach (var item in data.GetDataReader("[dbo].[sp_vanti_PlazosObtener]", parameters))
            {
                ApartadoPlazosResponse apartado = new ApartadoPlazosResponse();
                apartado.CodigoPlazo = Convert.ToInt32(item.GetValue(0));
                apartado.Dias = Convert.ToInt32(item.GetValue(1));
                apartado.Descripcion = item.GetValue(2).ToString();
                list.Add(apartado);
            }
            return list.ToArray();
        }

        /// <summary>
        /// Proceso para ceder apartados
        /// </summary>
        public int CederApartados()
        {
            var parameters = new Dictionary<string, object>();
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_ApartadosCeder]", parameters, null);
            return 1;
        }

        private void RegistrarArticulosConIncidenciaDigitoVerificador(string folioOperacion, int codeStore, int codeBox, LineaTicket[] lineas)
        {
            foreach (var linea in lineas)
            {
                var parameters = new Dictionary<string, object>();
                parameters.Add("@FolioOperacion", folioOperacion);
                parameters.Add("@CodigoTienda", codeStore);
                parameters.Add("@CodigoCaja", codeBox);
                parameters.Add("@Sku", linea.Articulo.Sku);
                parameters.Add("@Cantidad", linea.CantidadVendida);
                parameters.Add("@DigitoVerificadorMal", linea.Articulo.DigitoVerificadorArticulo.DigitoVerificadorActual);
                parameters.Add("@DigitoVerificadorBien", linea.Articulo.DigitoVerificadorArticulo.DigitoVerificadorCorrecto);
                List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
                data.ExecuteProcedure("[dbo].[sp_vanti_RegistrarIncidenciasDigitoVerificador]", parameters, parametersOut);
            }
        }

    }
}
