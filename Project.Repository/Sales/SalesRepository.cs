using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Dto.Recovery;
using Milano.BackEnd.Dto.Sales;
using Milano.BackEnd.Repository.General;
using Newtonsoft.Json;
using Project.Services.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Transactions;
using System.Xml.Serialization;

namespace Milano.BackEnd.Repository
{

    /// <summary>
    /// Repositorio de Ventas
    /// </summary>
    public class SalesRepository : BaseRepository
    {

        /* FUNCIONES PARA MANIPULAR LINEAS DEL TICKET */

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
                    // Se pasa el número de folio web en el depto
                    if (lineaTicket.TipoDetalleVenta == "48")
                    {
                        lineaTicket.cabeceraVentaRequest.NombreMembresia = $"{lineaTicket.Articulo.Depto}|{lineaTicket.Articulo.SubDepartamento}";
                    }

                    operacionLineaTicketVentaResponse.FolioOperacion = this.AgregarCabeceraVenta(codeStore, codeBox,
                    codeEmployee, lineaTicket.cabeceraVentaRequest, lineaTicket.TipoDetalleVenta);
                }
                this.AgregarDetalleVenta(operacionLineaTicketVentaResponse.FolioOperacion, codeStore, codeBox,
                    lineaTicket.cabeceraVentaRequest.TipoCabeceraVenta, lineaTicket);
                this.ActualizarCabeceraVenta(codeStore, codeBox, codeEmployee, lineaTicket.cabeceraVentaRequest);
                scope.Complete();
            }
            return operacionLineaTicketVentaResponse;
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
                parameters.Add("@ImporteSubtotal", lineaTicket.ImporteVentaLineaBruto);
                parameters.Add("@ImporteTotal", lineaTicket.ImporteVentaLineaNeto);
                List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
                parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
                parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
                var result = this.data.ExecuteProcedure("[dbo].[sp_vanti_CambiarPiezasLineaTicketVenta]", parameters, parametersOut);
                operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
                operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
                // Actualizar la cabecera de Venta
                this.ActualizarCabeceraVenta(codeStore, codeBox, codeEmployee, lineaTicket.cabeceraVentaRequest);
                scope.Complete();
            }
            return operationResponse;
        }

        /// <summary>
        /// Persistir cupones promocionales generados
        /// </summary>
        /// <param name="cuponPromocionalVenta">Información referente al cupón promocional generado</param>        
        /// <returns></returns>
        public CuponPersistirResponse PersistirCuponPromocionalGenerado(CuponPromocionalVenta cuponPromocionalVenta)
        {
            CuponPersistirResponse operationResponse = new CuponPersistirResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FechaCreacion", cuponPromocionalVenta.FechaCreacion);
            parameters.Add("@CodigoTienda", cuponPromocionalVenta.CodigoTienda);
            parameters.Add("@CodigoCaja", cuponPromocionalVenta.CodigoCaja);
            parameters.Add("@Transaccion", cuponPromocionalVenta.Transaccion);
            parameters.Add("@CodigoPromocionAplicado", cuponPromocionalVenta.CodigoPromocionAplicado);
            parameters.Add("@FolioOperacion", cuponPromocionalVenta.FolioOperacion);
            parameters.Add("@Status", cuponPromocionalVenta.Status);
            parameters.Add("@FechaCancelacion", cuponPromocionalVenta.FechaCancelacion);
            parameters.Add("@Saldo", cuponPromocionalVenta.Saldo);
            parameters.Add("@ImporteDescuento", cuponPromocionalVenta.ImporteDescuento);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            //  parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CuponFolio", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });

            var result = data.ExecuteProcedure("[dbo].[sp_vanti_PersisitirCuponPromocionalGenerado]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            //  operationResponse.FolioCupon = result["@CuponFolio"].ToString();
            return operationResponse;
        }





        /// <summary>
        /// Persistir promociones venta
        /// </summary>
        /// <param name="folioOperacion"></param>
        /// <param name="codeStore"></param>
        /// <param name="codeBox"></param>
        /// <param name="importeDescuento"></param>
        /// <param name="codigoPromocionAplicado"></param>
        /// <param name="descripcionCodigoPromocionAplicado"></param>
        /// <param name="formaPago"></param>
        /// <returns></returns>
        public OperationResponse PersistirPromocionesVenta(string folioOperacion, int codeStore, int codeBox
            , decimal importeDescuento, int codigoPromocionAplicado, string descripcionCodigoPromocionAplicado, decimal porcentajeDescuento, int codigoRazonDescuento, string formaPago)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioOperacion", folioOperacion);
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@CodigoCaja", codeBox);
            parameters.Add("@ImporteDescuento", importeDescuento);
            parameters.Add("@CodigoPromocion", codigoPromocionAplicado);
            parameters.Add("@DescripcionPromocion", descripcionCodigoPromocionAplicado);
            parameters.Add("@PorcentajeDescuento", porcentajeDescuento);
            parameters.Add("@CodigoRazonDescuento", codigoRazonDescuento);
            parameters.Add("@FormaPagoPromocion", formaPago);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_PersisitirPromocionesCab]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }

        /// <summary>
        /// Elimina las promociones a nivel cabecera
        /// </summary>
        /// <param name="folioOperacion">Folio de la venta</param>
        /// <param name="codigoTienda">Código de tienda</param>      
        /// <param name="codigoCaja">Código de caja</param>           
        /// <returns>Resultado de la operación</returns>
        public OperationResponse EliminarPromocionesCab(string folioOperacion, int codigoTienda, int codigoCaja)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioOperacion", folioOperacion);
            parameters.Add("@CodigoTienda", codigoTienda);
            parameters.Add("@CodigoCaja", codigoCaja);
            List<DescuentoPromocionalVenta> list = new List<DescuentoPromocionalVenta>();
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_EliminarPromocionesCab]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }

        /// <summary>
        /// Eliminar lineas por sku 
        /// </summary>
        /// <param name="folioOperacion"></param>
        /// <param name="codigoTienda"></param>
        /// <param name="codigoCaja"></param>        
        /// <returns></returns>
        public OperationResponse EliminarLineaMayorista(string folioOperacion, int codigoTienda, int codigoCaja)
        {
            ConfigGeneralesCajaTiendaResponse configGeneralesCajaTiendaResponse = new ConfigGeneralesCajaTiendaRepository().GetConfigSinBotonera(codigoCaja, codigoTienda, 0);
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioVenta", folioOperacion);
            parameters.Add("@CodigoTienda", codigoTienda);
            parameters.Add("@CodigoCaja", codigoCaja);
            parameters.Add("@Sku", configGeneralesCajaTiendaResponse.SkuPagoConValeMayorista.ToString());
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_EliminarLineaTicketVentaMayorista]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }

        /// <summary>
        /// Obtener cantidades linea mayorista
        /// </summary>
        /// <param name="folioOperacion"></param>
        /// <param name="codigoTienda"></param>
        /// <param name="codigoCaja"></param>        
        /// <returns></returns>
        public Decimal[] ObtenerLineaMayorista(string folioOperacion, int codigoTienda, int codigoCaja)
        {
            ConfigGeneralesCajaTiendaResponse configGeneralesCajaTiendaResponse = new ConfigGeneralesCajaTiendaRepository().GetConfigSinBotonera(codigoCaja, codigoTienda, 0);
            OperationResponse operationResponse = new OperationResponse();
            Decimal[] cantidades = new Decimal[2];
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioVenta", folioOperacion);
            parameters.Add("@CodigoTienda", codigoTienda);
            parameters.Add("@CodigoCaja", codigoCaja);
            parameters.Add("@Sku", configGeneralesCajaTiendaResponse.SkuPagoConValeMayorista.ToString());
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@ImporteTotal", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Decimal, Scale = 2 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@ImporteImpuesto1", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Decimal, Scale = 2 });
            var resultado = data.ExecuteProcedure("[dbo].[sp_vanti_ObtenerLineaTicketVentaMayorista]", parameters, parametersOut);
            cantidades[0] = Convert.ToDecimal(resultado["@ImporteTotal"]);
            cantidades[1] = Convert.ToDecimal(resultado["@ImporteImpuesto1"]);
            return cantidades;
        }

        /// <summary>
        /// Persistir promociones línea venta
        /// </summary>
        /// <param name="folioOperacion"> Folio Operación</param>
        /// <param name="codeStore">Codigo tienda</param>
        /// <param name="codeBox">Codigo Caja</param>
        /// <param name="secuencia">Secuencia </param>
        /// <param name="importeDescuento"> Importe descuento</param>
        /// <param name="codigoPromocionAplicado">Codio promocion aplicado</param>
        /// <param name="descripcionCodigoPromocionAplicado">Descrición de codigo</param>
        /// <param name="formaPago">forma de pagp </param>
        /// <returns></returns>
        public OperationResponse PersistirPromocionesLineaVenta(string folioOperacion, int codeStore, int codeBox, int secuencia, decimal importeDescuento, int codigoPromocionAplicado, string descripcionCodigoPromocionAplicado, decimal porcentajeDescuento, int codigoRazonDescuento, string formaPago)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioOperacion", folioOperacion);
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@CodigoCaja", codeBox);
            parameters.Add("@SecuenciaLineaTicket", secuencia);
            parameters.Add("@ImporteDescuento", importeDescuento);
            parameters.Add("@CodigoPromocion", codigoPromocionAplicado);
            parameters.Add("@DescripcionPromocion", descripcionCodigoPromocionAplicado);
            parameters.Add("@PorcentajeDescuento", porcentajeDescuento);
            parameters.Add("@CodigoRazonDescuento", codigoRazonDescuento);
            parameters.Add("@FormaPagoPromocion", formaPago);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_PersistirPromocionesDet]", parameters, parametersOut);
            return operationResponse;
        }

        /// <summary>
        /// Elimina las promociones a nivel detalle
        /// </summary>
        /// <param name="folioOperacion">Folio de la venta</param>
        /// <param name="codigoTienda">Código de tienda</param>      
        /// <param name="codigoCaja">Código de caja</param>                   
        /// <returns>Resultado de la operación</returns>
        public OperationResponse EliminarPromocionesDet(string folioOperacion, int codigoTienda, int codigoCaja)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioOperacion", folioOperacion);
            parameters.Add("@CodigoTienda", codigoTienda);
            parameters.Add("@CodigoCaja", codigoCaja);
            List<DescuentoPromocionalVenta> list = new List<DescuentoPromocionalVenta>();
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_EliminarPromocionesDet]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
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
                parameters.Add("@ImporteSubtotal", cambiarPrecioRequest.LineaTicket.ImporteVentaLineaBruto);
                parameters.Add("@ImporteTotal", cambiarPrecioRequest.LineaTicket.ImporteVentaLineaNeto);
                parameters.Add("@PrecioCambiadoConImpuestos", cambiarPrecioRequest.LineaTicket.Articulo.PrecioCambiadoConImpuestos);
                parameters.Add("@PrecioCambiadoImpuesto1", cambiarPrecioRequest.LineaTicket.Articulo.precioCambiadoImpuesto1);
                parameters.Add("@PrecioCambiadoImpuesto2", cambiarPrecioRequest.LineaTicket.Articulo.precioCambiadoImpuesto2);
                parameters.Add("@CodigoRazon", cambiarPrecioRequest.CodigoRazon);
                List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
                parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
                parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
                var result = this.data.ExecuteProcedure("[dbo].[sp_vanti_CambiarPrecioLineaTicketVenta]", parameters, parametersOut);
                operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
                operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
                // Actualizar la cabecera de Venta
                this.ActualizarCabeceraVenta(codeStore, codeBox, codeEmployee, cambiarPrecioRequest.LineaTicket.cabeceraVentaRequest);
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
        public OperationResponse EliminarLineaTicketVenta(int codeStore, int codeBox, int codeEmployee, int secuenciaOriginalLineaTicket, LineaTicket lineaTicket, int codigoRazon)
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
                var result = this.data.ExecuteProcedure("[dbo].[sp_vanti_ActualizarEstatusLineaTicketVenta]", parameters, parametersOut);
                if ((!String.IsNullOrEmpty(lineaTicket.cabeceraVentaRequest.FolioDevolucion)) && (lineaTicket.PerteneceVentaOriginal))
                {
                    // Se trata de una devolución
                    parameters = new Dictionary<string, object>();
                    parameters.Add("@FolioVenta", lineaTicket.cabeceraVentaRequest.FolioOperacion);
                    parameters.Add("@FolioDevolucion", lineaTicket.cabeceraVentaRequest.FolioDevolucion);
                    parameters.Add("@CodigoTienda", codeStore);
                    parameters.Add("@CodigoCaja", codeBox);
                    parameters.Add("@CodigoEmpleado", codeEmployee);
                    parameters.Add("@Secuencia", lineaTicket.Secuencia);
                    parameters.Add("@secuenciaOriginalLineaTicket", secuenciaOriginalLineaTicket);
                    parameters.Add("@TipoTransaccion", lineaTicket.cabeceraVentaRequest.TipoCabeceraVenta);
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
                    parameters.Add("@ImporteSubtotal", lineaTicket.ImporteVentaLineaBruto);
                    parameters.Add("@ImporteTotal", lineaTicket.ImporteVentaLineaNeto);
                    parameters.Add("@ImporteDevolucionSubtotal", lineaTicket.ImporteDevolucionLineaBruto);
                    parameters.Add("@ImporteDevolucionIva", lineaTicket.ImporteDevolucionLineaImpuestos);
                    parameters.Add("@ImporteDevolucionTotal", lineaTicket.ImporteDevolucionLineaNeto);
                    parameters.Add("@ImporteVentaDevolucionImpuestos", lineaTicket.cabeceraVentaRequest.ImporteDevolucionImpuestos);
                    parameters.Add("@ImporteVentaDevolucionTotal", lineaTicket.cabeceraVentaRequest.ImporteDevolucionNeto);
                    parameters.Add("@FolioVentaOriginal", lineaTicket.cabeceraVentaRequest.FolioVentaOriginal);
                    parameters.Add("@PerteneceVentaOriginal", 1);
                    parameters.Add("@CodigoRazon", codigoRazon);
                }
                else
                {
                    // Se trata de una eliminación normal
                    // Agregar la nueva linea con transacción de tipo 87                    
                    parameters = new Dictionary<string, object>();
                    parameters.Add("@FolioVenta", lineaTicket.cabeceraVentaRequest.FolioOperacion);
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
                    parameters.Add("@CostoUnitario", (lineaTicket.Articulo.PrecioConImpuestos * -1));
                    parameters.Add("@CodigoImpuesto1", lineaTicket.Articulo.CodigoImpuesto1);
                    parameters.Add("@CodigoImpuesto2", lineaTicket.Articulo.CodigoImpuesto2);
                    // Ajuste necesario para integrar caso de motor de promociones cuando se elimina después de aplicar un descuento por Motor de Promociones
                    if ((lineaTicket.DescuentosPromocionalesAplicadosLinea != null) && (lineaTicket.DescuentosPromocionalesAplicadosLinea.Length > 0))
                    {
                        var parametersInner = new Dictionary<string, object>();
                        List<System.Data.SqlClient.SqlParameter> parametersOutInner = new List<System.Data.SqlClient.SqlParameter>();
                        parametersInner.Add("@FolioVenta", lineaTicket.cabeceraVentaRequest.FolioOperacion);
                        parametersInner.Add("@CodigoTienda", codeStore);
                        parametersInner.Add("@CodigoCaja", codeBox);
                        parametersInner.Add("@Sku", lineaTicket.Articulo.Sku);
                        parametersInner.Add("@SecuenciaOriginal", secuenciaOriginalLineaTicket);
                        parametersOutInner.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@ImporteImpuesto1Original", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Decimal, Scale = 2 });
                        parametersOutInner.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@ImporteImpuesto2Original", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Decimal, Scale = 2 });
                        parametersOutInner.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@ImporteSubtotalOriginal", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Decimal, Scale = 2 });
                        parametersOutInner.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@ImporteTotalOriginal", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Decimal, Scale = 2 });
                        var resultInner = this.data.ExecuteProcedure("[dbo].[sp_vanti_ObtenerDatosLineaOriginalTicketVenta]", parametersInner, parametersOutInner);
                        parameters.Add("@ImporteImpuesto1", ((Convert.ToDecimal(resultInner["@ImporteImpuesto1Original"])) * -1));
                        parameters.Add("@ImporteImpuesto2", ((Convert.ToDecimal(resultInner["@ImporteImpuesto2Original"])) * -1));
                        parameters.Add("@ImporteSubtotal", ((Convert.ToDecimal(resultInner["@ImporteSubtotalOriginal"])) * -1));
                        parameters.Add("@ImporteTotal", ((Convert.ToDecimal(resultInner["@ImporteTotalOriginal"])) * -1));
                    }
                    else
                    {
                        parameters.Add("@ImporteImpuesto1", (lineaTicket.ImporteVentaLineaImpuestos1 * -1));
                        parameters.Add("@ImporteImpuesto2", (lineaTicket.ImporteVentaLineaImpuestos2 * -1));
                        parameters.Add("@ImporteSubtotal", (lineaTicket.ImporteVentaLineaBruto * -1));
                        parameters.Add("@ImporteTotal", (lineaTicket.ImporteVentaLineaNeto * -1));
                    }
                    //Si se trata de una venta de Tiempo Aire se agrega la referencia
                    if (lineaTicket.cabeceraVentaRequest.TipoCabeceraVenta == "5")
                    {
                        parameters.Add("@Referencia", lineaTicket.Articulo.InformacionProveedorExternoTA.SkuCompania);
                    }
                }
                parametersOut = new List<System.Data.SqlClient.SqlParameter>();
                parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
                parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
                result = this.data.ExecuteProcedure("[dbo].[sp_vanti_AgregarLineaTicketVenta]", parameters, parametersOut);
                operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
                operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
                // Actualizar la cabecera de Venta
                this.ActualizarCabeceraVenta(codeStore, codeBox, codeEmployee, lineaTicket.cabeceraVentaRequest);
                scope.Complete();
            }
            return operationResponse;
        }

        /* FUNCIONES PARA LA TOTALIZACIÓN DE VENTAS */

        /// <summary>
        /// Totalizar Venta
        /// </summary>
        /// <param name="totalizarVentaRequest">Petición de totalización de venta</param>        
        /// <param name="codeStore">Código de tienda</param>      
        /// <param name="codeBox">Código de caja</param>      
        /// <param name="codeEmployee">Código de empleado cajero</param>          
        /// <returns>Folio de Venta</returns>
        public TotalizarVentaResponse TotalizarVenta(TotalizarVentaRequest totalizarVentaRequest, int codeStore, int codeBox, int codeEmployee)
        {
            TotalizarVentaResponse totalizarVentaResponse = new TotalizarVentaResponse();
            FormasPagoRepository formasPagoRepository = new FormasPagoRepository();
            using (TransactionScope scope = new TransactionScope())
            {
                totalizarVentaResponse.FolioOperacion = totalizarVentaRequest.cabeceraVentaRequest.FolioOperacion;
                // Agregar linea ticket de mayorista si es el caso
                if (totalizarVentaRequest.cabeceraVentaRequest.CodigoMayorista > 0 && totalizarVentaRequest.cabeceraVentaRequest.FolioVentaOriginal == "")
                {
                    ConfigGeneralesCajaTiendaResponse configGeneralesCajaTiendaResponse = new ConfigGeneralesCajaTiendaRepository().GetConfigSinBotonera(codeBox, codeStore, 0);
                    // Objeto de petición de producto
                    ProductsRequest productRequest = new ProductsRequest();
                    Inspector inspector = new Inspector();
                    productRequest.Sku = configGeneralesCajaTiendaResponse.SkuPagoConValeMayorista.ToString();
                    // Buscar objeto especial pago mayorista y asignarlo                    
                    totalizarVentaResponse.ProductoPagoConValeMayorista = new ProductsRepository().Search(codeStore, productRequest)[0];
                    decimal porcentajePagoMayorista = (decimal.Parse(configGeneralesCajaTiendaResponse.PorcentajePagoConValeMayorista.ToString()) / decimal.Parse("100"));
                    totalizarVentaResponse.ProductoPagoConValeMayorista.Articulo.PrecioConImpuestos = inspector.TruncarValor(totalizarVentaRequest.cabeceraVentaRequest.ImporteVentaNeto * porcentajePagoMayorista);
                    totalizarVentaResponse.ProductoPagoConValeMayorista.Articulo.Impuesto1 = inspector.TruncarValor(totalizarVentaRequest.cabeceraVentaRequest.ImporteVentaImpuestos * porcentajePagoMayorista);
                    // Armar Linea Ticket con el objeto pago mayorista
                    LineaTicket lineaTicket = new LineaTicket();
                    lineaTicket.Secuencia = (totalizarVentaRequest.SecuenciaActual + 1);
                    lineaTicket.CantidadVendida = 1;
                    lineaTicket.CantidadDevuelta = 0;
                    lineaTicket.Articulo = new Articulo();
                    lineaTicket.Articulo.Sku = totalizarVentaResponse.ProductoPagoConValeMayorista.Articulo.Sku;
                    lineaTicket.Articulo.Upc = totalizarVentaResponse.ProductoPagoConValeMayorista.Articulo.Upc;
                    lineaTicket.Articulo.TasaImpuesto1 = totalizarVentaResponse.ProductoPagoConValeMayorista.Articulo.TasaImpuesto1;
                    lineaTicket.Articulo.TasaImpuesto2 = totalizarVentaResponse.ProductoPagoConValeMayorista.Articulo.TasaImpuesto2;
                    lineaTicket.Articulo.CodigoImpuesto1 = totalizarVentaResponse.ProductoPagoConValeMayorista.Articulo.CodigoImpuesto1;
                    lineaTicket.Articulo.CodigoImpuesto2 = totalizarVentaResponse.ProductoPagoConValeMayorista.Articulo.CodigoImpuesto2;
                    lineaTicket.Articulo.PrecioConImpuestos = totalizarVentaResponse.ProductoPagoConValeMayorista.Articulo.PrecioConImpuestos;
                    lineaTicket.ImporteVentaLineaImpuestos1 = totalizarVentaResponse.ProductoPagoConValeMayorista.Articulo.Impuesto1;
                    lineaTicket.ImporteVentaLineaImpuestos2 = totalizarVentaResponse.ProductoPagoConValeMayorista.Articulo.Impuesto2;
                    lineaTicket.ImporteVentaLineaNeto = totalizarVentaResponse.ProductoPagoConValeMayorista.Articulo.PrecioConImpuestos;
                    lineaTicket.ImporteVentaLineaBruto = (lineaTicket.ImporteVentaLineaNeto - lineaTicket.ImporteVentaLineaImpuestos1);
                    lineaTicket.TipoDetalleVenta = "2";
                    lineaTicket.cabeceraVentaRequest = totalizarVentaRequest.cabeceraVentaRequest;
                    this.AgregarLineaTicketVenta(codeStore, codeBox, codeEmployee, lineaTicket);
                    // Obtener el precio persistido y actualizar
                    Decimal[] cantidades = this.ObtenerLineaMayorista(totalizarVentaRequest.cabeceraVentaRequest.FolioOperacion, codeStore, codeBox);
                    totalizarVentaResponse.ProductoPagoConValeMayorista.Articulo.PrecioConImpuestos = cantidades[0];
                    totalizarVentaResponse.ProductoPagoConValeMayorista.Articulo.Impuesto1 = cantidades[1];
                }
                // Actualizar el estatus de la venta
                var parameters = new Dictionary<string, object>();
                parameters.Add("@FolioOperacion", totalizarVentaResponse.FolioOperacion);
                parameters.Add("@CodigoTienda", codeStore);
                parameters.Add("@CodigoCaja", codeBox);
                List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
                this.data.ExecuteProcedure("[dbo].[sp_vanti_TotalizarVenta]", parameters, parametersOut);

                // Validar si deben regresarse formas de pago para el front
                if (String.IsNullOrEmpty(totalizarVentaRequest.cabeceraVentaRequest.FolioDevolucion))
                {
                    // Se regresa información acerca de las formas de pago que deben mostrarse en el Front
                    totalizarVentaResponse.InformacionAsociadaFormasPago = formasPagoRepository.GetConfigFormasPago(codeBox, codeStore, totalizarVentaRequest.cabeceraVentaRequest.FolioOperacion, totalizarVentaRequest.cabeceraVentaRequest.FolioDevolucion, totalizarVentaRequest.cabeceraVentaRequest.TipoCabeceraVenta);
                    totalizarVentaResponse.InformacionAsociadaFormasPagoMonedaExtranjera = formasPagoRepository.GetConfigFormasPagoExt(codeBox, codeStore, totalizarVentaRequest.cabeceraVentaRequest.FolioOperacion, totalizarVentaRequest.cabeceraVentaRequest.FolioDevolucion, totalizarVentaRequest.cabeceraVentaRequest.TipoCabeceraVenta);
                }
                else
                {


                   int cantidadPromociones = (new DescuentosPromocionesRepository().ObtenerPromocionesVenta(totalizarVentaRequest.cabeceraVentaRequest.FolioOperacion, codeStore, codeBox, totalizarVentaRequest.cabeceraVentaRequest.NivelLealtad, totalizarVentaRequest.cabeceraVentaRequest.PrimeraCompraLealtad, (int)totalizarVentaRequest.cabeceraVentaRequest.CodigoClienteLealtad)).Length;

                    //int cantidadPromociones = (new DescuentosPromocionesRepository().ObtenerPromocionesVenta(totalizarVentaRequest.cabeceraVentaRequest.FolioOperacion, codeStore, codeBox)).Length;
                    if ((totalizarVentaRequest.cabeceraVentaRequest.ImporteVentaNetoOriginal >= totalizarVentaRequest.cabeceraVentaRequest.ImporteVentaNeto)
                        && (cantidadPromociones == 0))
                    {
                        totalizarVentaResponse.InformacionAsociadaDevolucion = this.FinalizarVentaDevolucion(totalizarVentaRequest.cabeceraVentaRequest.FolioOperacion, totalizarVentaRequest.cabeceraVentaRequest.FolioDevolucion,
                        totalizarVentaRequest.cabeceraVentaRequest.FolioVentaOriginal, codeBox, codeStore, totalizarVentaRequest.cabeceraVentaRequest.CodigoMayorista, totalizarVentaRequest.cabeceraVentaRequest.ImporteVentaDescuentos, totalizarVentaRequest.cabeceraVentaRequest.ImporteDevolucionImpuestos,
                        totalizarVentaRequest.cabeceraVentaRequest.ImporteVentaBruto, totalizarVentaRequest.cabeceraVentaRequest.ImporteVentaImpuestos, totalizarVentaRequest.cabeceraVentaRequest.ImporteVentaNeto,
                        totalizarVentaRequest.cabeceraVentaRequest.ImporteDevolucionNeto, totalizarVentaRequest.cabeceraVentaRequest.ImporteDevolucionImpuestos, totalizarVentaRequest.cabeceraVentaRequest.DevolucionSaldoAFavor, totalizarVentaRequest.cabeceraVentaRequest.ImporteDevolucionTotal);
                    }
                    else
                    {
                        // Se regresa información acerca de las formas de pago que deben mostrarse en el Front
                        totalizarVentaResponse.InformacionAsociadaFormasPago = formasPagoRepository.GetConfigFormasPago(codeBox, codeStore, totalizarVentaRequest.cabeceraVentaRequest.FolioOperacion, totalizarVentaRequest.cabeceraVentaRequest.FolioDevolucion, totalizarVentaRequest.cabeceraVentaRequest.TipoCabeceraVenta);
                        totalizarVentaResponse.InformacionAsociadaFormasPagoMonedaExtranjera = formasPagoRepository.GetConfigFormasPagoExt(codeBox, codeStore, totalizarVentaRequest.cabeceraVentaRequest.FolioOperacion, totalizarVentaRequest.cabeceraVentaRequest.FolioDevolucion, totalizarVentaRequest.cabeceraVentaRequest.TipoCabeceraVenta);
                    }
                }
                // Actualizar la cabecera                
                this.ActualizarCabeceraVenta(codeStore, codeBox, codeEmployee, totalizarVentaRequest.cabeceraVentaRequest);
                scope.Complete();
            }
            return totalizarVentaResponse;
        }

        /// <summary>
        /// Finalizar Devolución Sin Pagos
        /// </summary>
        /// <param name="folioVenta">Folio de la Venta</param>              
        /// <returns>Información asociada a la devolución</returns>
        public InformacionAsociadaDevolucion FinalizarVentaDevolucion(string folioVenta, string folioDevolucion, string folioVentaOriginal, int codigoCaja, int codigoTienda, int codigoMayorista, decimal importeVentaDescuentos,
            decimal importeVentaDescuentosImpuestos, decimal subTotal, decimal iva, decimal Total, decimal importeDevolucionNeto, decimal importeDevolucionImpuestos, decimal devolucionSaldoAFavor, decimal importeDevolucionTotal)
        {
            InformacionAsociadaDevolucion operationResponse = new InformacionAsociadaDevolucion();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioVenta", folioVenta);
            parameters.Add("@FolioDevolucion", folioDevolucion);
            parameters.Add("@FolioVentaOriginal", folioVentaOriginal);
            parameters.Add("@CodigoCaja", codigoCaja);
            parameters.Add("@CodigoTienda", codigoTienda);
            parameters.Add("@CodigoMayorista ", codigoMayorista);
            parameters.Add("ImporteVentaDescuentos", importeVentaDescuentos);
            parameters.Add("@ImporteVentaDescuentosImpuestos", importeVentaDescuentosImpuestos);
            parameters.Add("@SubTotal", subTotal);
            parameters.Add("@Iva", iva);
            parameters.Add("@Total", Total);
            parameters.Add("@TotalDevolucion", importeDevolucionNeto);
            parameters.Add("@IvaDevolucion", importeDevolucionImpuestos);
            parameters.Add("@DevolucionSaldoAFavor", devolucionSaldoAFavor);
            parameters.Add("@ImporteDevolucionTotal", importeDevolucionTotal);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_FinalizarTransaccionVentaDevolucion]", parameters, parametersOut);
            // Información referente a estatus de la operación
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }

        private OperationResponse FinalizarVentaDevolucionConPagos(string folioVenta, string folioDevolucion, string folioVentaOriginal, int codigoCaja, int codigoTienda, int codigoMayorista, decimal importeVentaDescuentos,
            decimal importeVentaDescuentosImpuestos, decimal subTotal, decimal iva, decimal Total, int totalPagosRealizados, decimal importeDevolucionNeto, decimal importeDevolucionImpuestos, decimal importeDevolucionTotal)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioVenta", folioVenta);
            parameters.Add("@FolioDevolucion", folioDevolucion);
            parameters.Add("@FolioVentaOriginal", folioVentaOriginal);
            parameters.Add("@CodigoCaja", codigoCaja);
            parameters.Add("@CodigoTienda", codigoTienda);
            parameters.Add("@CodigoMayorista ", codigoMayorista);
            parameters.Add("ImporteVentaDescuentos", importeVentaDescuentos);
            parameters.Add("@ImporteVentaDescuentosImpuestos", importeVentaDescuentosImpuestos);
            parameters.Add("@SubTotal", subTotal);
            parameters.Add("@Iva", iva);
            parameters.Add("@Total", Total);
            parameters.Add("@TotalPagosRealizados", totalPagosRealizados);
            parameters.Add("@TotalDevolucion", importeDevolucionNeto);
            parameters.Add("@IvaDevolucion", importeDevolucionImpuestos);
            parameters.Add("@ImporteDevolucionTotal", importeDevolucionTotal);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeEfectivoMaximo", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@EfectivoMaximoCaja", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Decimal, Scale = 2 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@DotacionInicial", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Decimal, Scale = 2 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MontoActualCaja", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Decimal, Scale = 2 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MostrarAlertaRetiroEfectivo", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Bit });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@PermitirIgnorar", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Bit });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_FinalizarTransaccionVentaDevolucionConPagos]", parameters, parametersOut);

            // Información referente a retiro de Efectivo
            InformacionAsociadaRetiroEfectivo informacionAsociadaRetiroEfectivo = new InformacionAsociadaRetiroEfectivo();
            informacionAsociadaRetiroEfectivo.MensajeEfectivoMaximo = result["@MensajeEfectivoMaximo"].ToString();
            informacionAsociadaRetiroEfectivo.EfectivoMaximoPermitidoCaja = Convert.ToDecimal(result["@EfectivoMaximoCaja"]);
            informacionAsociadaRetiroEfectivo.DotacionInicialCaja = Convert.ToDecimal(result["@DotacionInicial"]);
            informacionAsociadaRetiroEfectivo.EfectivoActualCaja = Convert.ToDecimal(result["@MontoActualCaja"]);
            informacionAsociadaRetiroEfectivo.MostrarAlertaRetiroEfectivo = Convert.ToBoolean(result["@MostrarAlertaRetiroEfectivo"]);
            informacionAsociadaRetiroEfectivo.PermitirIgnorarAlertaRetiroEfectivo = Convert.ToBoolean(result["@PermitirIgnorar"]);
            operationResponse.informacionAsociadaRetiroEfectivo = informacionAsociadaRetiroEfectivo;
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }

        private string AgregarCabeceraVenta(int codigoTienda, int codigoCaja, int codigoEmpleadoCajero, CabeceraVentaRequest cabeceraVentaRequest, String codigoTipoDetalleVenta)
        {
            var parameters = new Dictionary<string, object>();
            String prefijoFolioCabeceraVenta = "VE";
            parameters.Add("@CodigoTienda", codigoTienda);
            parameters.Add("@CodigoCaja", codigoCaja);
            parameters.Add("@CodigoEmpleado", codigoEmpleadoCajero);
            parameters.Add("@ImporteDescuento", cabeceraVentaRequest.ImporteVentaDescuentos);
            parameters.Add("@SubTotal", cabeceraVentaRequest.ImporteVentaBruto);
            parameters.Add("@Iva", cabeceraVentaRequest.ImporteVentaImpuestos);
            parameters.Add("@Total", cabeceraVentaRequest.ImporteVentaNeto);
            parameters.Add("@TipoTransaccion", cabeceraVentaRequest.TipoCabeceraVenta);
            if (cabeceraVentaRequest.CodigoEmpleadoVendedor > 0)
            {
                parameters.Add("@CodigoEmpleadoVendedor", cabeceraVentaRequest.CodigoEmpleadoVendedor);
            }
            if (cabeceraVentaRequest.NumeroNominaVentaEmpleado > 0)
            {
                parameters.Add("@NumMembresia", cabeceraVentaRequest.NumeroNominaVentaEmpleado);
                parameters.Add("@NombreMembresia", cabeceraVentaRequest.NombreMembresia);
            }
            if (cabeceraVentaRequest.CodigoMayorista > 0)
            {
                parameters.Add("@NumMembresia", cabeceraVentaRequest.CodigoMayorista);
                parameters.Add("@NombreMembresia", cabeceraVentaRequest.NombreMembresia);
            }
            if (cabeceraVentaRequest.CodigoMayoristaCredito > 0)
            {
                parameters.Add("@NumMembresia", cabeceraVentaRequest.CodigoMayoristaCredito);
                parameters.Add("@NombreMembresia", cabeceraVentaRequest.NombreMembresia);
            }
            if (codigoTipoDetalleVenta  == "48")
            {
                string[] infoPagoWebx = cabeceraVentaRequest.NombreMembresia.Split('|');
                parameters.Add("@NumMembresia", infoPagoWebx[0]);
                parameters.Add("@NombreMembresia", $"{infoPagoWebx[1]}|{infoPagoWebx[2]}");
            }
            if (codigoTipoDetalleVenta == "46")
            {
                prefijoFolioCabeceraVenta = "MM";
            }
            if (codigoTipoDetalleVenta == "43")
            {
                prefijoFolioCabeceraVenta = "PM";
            }
            if (codigoTipoDetalleVenta == "48")
            {
                prefijoFolioCabeceraVenta = "PW";
            }
            parameters.Add("@PrefijoFolioCabeceraVenta", prefijoFolioCabeceraVenta);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@FolioVenta", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_AgregarCabeceraVenta]", parameters, parametersOut);
            return result["@FolioVenta"].ToString();
        }

        private void AgregarDetalleVenta(string folioVenta, int codigoTienda, int codigoCaja, string tipoCabeceraVenta, LineaTicket lineaTicket)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioVenta", folioVenta);
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
            parameters.Add("@ImporteSubtotal", lineaTicket.ImporteVentaLineaBruto);
            parameters.Add("@ImporteTotal", lineaTicket.ImporteVentaLineaNeto);
            parameters.Add("@DigitoVerificadorCorrecto", lineaTicket.Articulo.DigitoVerificadorArticulo.DigitoVerificadorCorrecto);
            parameters.Add("@DigitoVerificadorLeido", lineaTicket.Articulo.DigitoVerificadorArticulo.DigitoVerificadorActual);
            //Si se trata de una venta de Tiempo Aire se agrega la referencia
            if (tipoCabeceraVenta == "5")
            {
                parameters.Add("@Referencia", lineaTicket.Articulo.InformacionProveedorExternoTA.SkuCompania);
            }

            if (lineaTicket.Articulo.InformacionTarjetaRegalo != null)
            {
                parameters.Add("@FolioTarjetaRegalo", 0);
                parameters.Add("@DescripcionTarjetaRegalo", "");
            }

            if (lineaTicket.Articulo.InformacionProveedorExternoTA != null)
            {
                parameters.Add("@NumeroTelefonico", lineaTicket.Articulo.InformacionProveedorExternoTA.NumeroTelefonico);
                parameters.Add("@SkuEmpresaTelefonica", lineaTicket.Articulo.InformacionProveedorExternoTA.SkuCompania);
            }
            if (tipoCabeceraVenta == "6")// si se trata de pago de servicios
            {
                parameters.Add("@CuentaPagoServicio", lineaTicket.Articulo.InformacionProveedorExternoAsociadaPS.Cuenta);
                parameters.Add("@SkuPagoServicio", lineaTicket.Articulo.InformacionProveedorExternoAsociadaPS.SkuCompania);
                string xmlInfoAdicional = this.ObtenerXmlInfoAdicionalPagoServicios(lineaTicket.Articulo.InformacionProveedorExternoAsociadaPS.InfoAdicional);
                parameters.Add("@InfoAdicionalPagoServicio", xmlInfoAdicional);
            }
            if (lineaTicket.Articulo.InformacionPagoTCMM != null)
            {
                parameters.Add("@numeroTCMM", lineaTicket.Articulo.InformacionPagoTCMM.NumeroTarjeta);

            }

            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            data.ExecuteProcedure("[dbo].[sp_vanti_AgregarLineaTicketVenta]", parameters, parametersOut);
        }

        private string ObtenerXmlInfoAdicionalPagoServicios(PagoServiciosInfoAdicional info)
        {
            string xml = "";
            XmlSerializer xmlSerializer = new XmlSerializer(info.GetType());
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, info);
                xml = textWriter.ToString();
            }
            return xml;
        }


        /// <summary>
        /// Actualizar la cabecera de Venta
        /// </summary>
        /// <param name="cabeceraVentaRequest">Información de la cabecera de venta</param>        
        /// <param name="codigoTienda">Código de tienda</param>      
        /// <param name="codigoCaja">Código de caja</param>      
        /// <param name="codigoEmpleadoCajero">Código de empleado cajero</param>          
        public void ActualizarCabeceraVenta(int codigoTienda, int codigoCaja, int codigoEmpleadoCajero, CabeceraVentaRequest cabeceraVentaRequest)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioOperacion", cabeceraVentaRequest.FolioOperacion);
            parameters.Add("@CodigoTienda", codigoTienda);
            parameters.Add("@CodigoCaja", codigoCaja);
            parameters.Add("@CodigoEmpleado", codigoEmpleadoCajero);
            parameters.Add("@ImporteDescuento", cabeceraVentaRequest.ImporteVentaDescuentos);
            parameters.Add("@SubTotal", cabeceraVentaRequest.ImporteVentaBruto);
            parameters.Add("@Iva", cabeceraVentaRequest.ImporteVentaImpuestos);
            parameters.Add("@Total", cabeceraVentaRequest.ImporteVentaNeto);
            if (cabeceraVentaRequest.CodigoEmpleadoVendedor > 0)
            {
                parameters.Add("@CodigoEmpleadoVendedor", cabeceraVentaRequest.CodigoEmpleadoVendedor);
            }
            if (cabeceraVentaRequest.NumeroNominaVentaEmpleado > 0)
            {
                parameters.Add("@NumMembresia", cabeceraVentaRequest.NumeroNominaVentaEmpleado);
            }
            if (cabeceraVentaRequest.CodigoMayorista > 0)
            {
                parameters.Add("@NumMembresia", cabeceraVentaRequest.CodigoMayorista);
            }
            if (cabeceraVentaRequest.CodigoMayoristaCredito > 0)
            {
                parameters.Add("@NumMembresia", cabeceraVentaRequest.CodigoMayoristaCredito);
            }
            
            //OCG: 
            if (cabeceraVentaRequest.TipoCabeceraVenta == "48")
            {
                parameters.Add("@NumMembresia", cabeceraVentaRequest.NombreMembresia);
            }


            // Específico para devoluciones

            if (String.IsNullOrEmpty(cabeceraVentaRequest.FolioDevolucion))
            {
                cabeceraVentaRequest.FolioDevolucion = "";
            }
            parameters.Add("@FolioDevolucion", cabeceraVentaRequest.FolioDevolucion);
            parameters.Add("@ImporteDevolucionTotal", cabeceraVentaRequest.ImporteDevolucionTotal);
            // Específico para devoluciones
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            this.data.ExecuteProcedure("[dbo].[sp_vanti_ActualizarCabeceraVenta]", parameters, parametersOut);
        }


        /* FUNCIONES PARA LA FINALIZACIÓN DE VENTAS */

        /// <summary>
        /// Finalizar Venta
        /// </summary>
        /// <param name="finalizarVentaRequest">Petición de finalización de venta</param>        
        /// <param name="codeStore">Código de tienda</param>      
        /// <param name="codeBox">Código de caja</param>      
        /// <param name="codeEmployee">Código de empleado cajero</param>      
        /// <param name="clasificacionVenta">Código de empleado cajero</param>      
        /// <returns>Respuesta de transacción</returns>
        public OperationResponse FinalizarVenta(int codeStore, int codeBox, int codeEmployee, FinalizarVentaRequest finalizarVentaRequest, string clasificacionVenta)
        {
            OperationResponse operationResponse = new OperationResponse();

            new FormasPagoRepository().AsociarFormasPago(codeStore, codeBox, codeEmployee, finalizarVentaRequest.FolioVenta, finalizarVentaRequest.FormasPagoUtilizadas, clasificacionVenta);

            if (finalizarVentaRequest.InformacionFoliosTarjeta != null)
            {
                operationResponse = this.AsociarFoliosTarjeta(codeStore, codeBox, finalizarVentaRequest);
            }

            operationResponse = this.FinalizarTransaccionVenta(codeStore, codeBox, finalizarVentaRequest, finalizarVentaRequest.FormasPagoUtilizadas.Length);

            // Si la venta fue procesada exitosamente se almacena el log de transacción
            if (operationResponse.CodeNumber.Equals("332"))
            {
                if (finalizarVentaRequest.LineasConDigitoVerificadorIncorrecto != null)
                {
                    //OCG
                    this.RegistrarArticulosConIncidenciaDigitoVerificador(finalizarVentaRequest.FolioVenta, codeStore, codeBox, finalizarVentaRequest.LineasConDigitoVerificadorIncorrecto);
                }
            }
            return operationResponse;
        }

        /// <summary>
        /// Finalizar Venta
        /// </summary>
        /// <param name="finalizarVentaRequest">Petición de finalización de venta</param>        
        /// <param name="codeStore">Código de tienda</param>      
        /// <param name="codeBox">Código de caja</param>      
        /// <param name="codeEmployee">Código de empleado cajero</param>      
        /// <returns>Respuesta de transacción</returns>
		public OperationResponse FinalizarVentaProductoDeApartadoLiquidado(int codeStore, int codeBox, int codeEmployee, FinalizarVentaRequest finalizarVentaRequest, int numeroFomasPago)
        {
            OperationResponse operationResponse = new OperationResponse();
            operationResponse = this.FinalizarTransaccionVenta(codeStore, codeBox, finalizarVentaRequest, numeroFomasPago);
            // Si la venta fue procesada exitosamente se almacena el log de transacción
            if (operationResponse.CodeNumber.Equals("332"))
            {
                // TODO: Falta llenar el objeto transacción
                //TransactionLogRepository<Transaccion> transactionLogRepository = new TransactionLogRepository<Transaccion>(new TokenDto(codeStore, codeBox));
                //Transaccion ventaFinalizada = new Transaccion();
                this.RegistrarArticulosConIncidenciaDigitoVerificador(finalizarVentaRequest.FolioVenta, codeStore, codeBox, finalizarVentaRequest.LineasConDigitoVerificadorIncorrecto);
                //transactionLogRepository.Add(ventaFinalizada, codeStore + "." + codeBox + "." + finalizarVentaRequest.FolioVenta);
            }
            return operationResponse;
        }


        public TipoCambioActualizado DivisaActualizada()
        {
            TipoCambioActualizado tipoCambioResponse = new TipoCambioActualizado();

            var parameters = new Dictionary<string, object>();

            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CambioMaximo", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Money });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@ReciboMaximo", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Money });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@TipoCambio", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Money });

            var result = data.ExecuteProcedure("sp_cnfDivisaActulizada", parameters, parametersOut);
            tipoCambioResponse.TipoCambio = Convert.ToDecimal(result["@TipoCambio"].ToString());
            tipoCambioResponse.ReciboMaximo = Convert.ToDecimal(result["@ReciboMaximo"].ToString());
            tipoCambioResponse.CambioMaximo = Convert.ToDecimal(result["@CambioMaximo"].ToString());

            return tipoCambioResponse;
        }

        private OperationResponse AsociarFoliosTarjeta(int codeStore, int codeBox, FinalizarVentaRequest request)
        {
            OperationResponse operationResponse = new OperationResponse();
            foreach (InformacionFoliosTarjeta info in request.InformacionFoliosTarjeta)
            {
                var parameters = new Dictionary<string, object>();
                parameters.Add("@FolioVenta", request.FolioVenta);
                parameters.Add("@Sku", info.Sku);
                parameters.Add("@CodigoTienda", codeStore);
                parameters.Add("@CodigoCaja", codeBox);
                parameters.Add("@FolioTarjeta", info.FolioTarjeta);
                List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
                parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
                parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
                var result = data.ExecuteProcedure("sp_vanti_FinalizarTransaccionVentaAsociarTarjetasRegalo", parameters, parametersOut);
                operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
                operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            }
            return operationResponse;
        }

        /// <summary>
        /// Registra el codigo de autorizacion del pago de TCMM
        /// </summary>
        /// <param name="folioVenta">Folio de la venta</param>
        /// <param name="numeroTarjeta">Numero de tarjeta</param>
        /// <param name="autorizacion">Autorizacion</param>
        /// <returns></returns>
        public OperationResponse RegistrarAutorizacionPagoTCMM(string folioVenta, string numeroTarjeta, string autorizacion)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();

            parameters.Add("@CodigoTarjeta", numeroTarjeta);
            parameters.Add("@FolioOperacion", folioVenta);
            parameters.Add("@Autorizacion", autorizacion);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_PagoTCMMRegistrarAutorizacion]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }


        public ResponseBussiness<ValidarFolioWebResponse> IndicarPagoFolioWeb(ValidarFolioWebRequest validarFolioWebRequest )
        {
            string url = $"https://credito.milano.com.mx/apishopify/api/Order/ProcessPayment";
            string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes("usrprodws" + ":" + "M1l4n0Pr0dWS*07"));

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("Authorization", "Basic " + svcCredentials);
            httpWebRequest.Headers.Add("UserName", "MMPOSCRED");
            httpWebRequest.Headers.Add("Password", "TriDXeLLP8F2Edlx1aZjf+gR2b0UBcXouKG0Ykrmopw=");

            // Agregar el cuerpo serializado
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(JsonConvert.SerializeObject(validarFolioWebRequest));
            }


            ValidarFolioWebResponse resp = new ValidarFolioWebResponse();
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                //result = streamReader.ReadToEnd();
                resp = JsonConvert.DeserializeObject<ValidarFolioWebResponse>(streamReader.ReadToEnd());
            }

            // OCG: Descomentar para simulación de respuesta
            /*ValidarFolioWebResponse resp = new ValidarFolioWebResponse();
            ValidarFolioWebDetalleResponse x = new ValidarFolioWebDetalleResponse();

            resp.ErrorCode = 0;
            resp.Message = "Si tiene saldo";
            x.Amount = 100;
            x.Currency = "MXN";
            x.CustomerName = "TEST 100";
            x.OrderId = "1";
            x.OrderNuber = "1";
            x.TransactionId = "20220120";

            resp.Response = x;*/

            return resp;

        }




         public ValidarFolioWebResponse RecuperarOrderIdTransactionID(string Folio) {

            ValidarFolioWebRequest validarFolioWebRequest = new ValidarFolioWebRequest();
            ValidarFolioWebResponse validarFolioWebResponse = new ValidarFolioWebResponse();

            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioVenta", Folio);

            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@Monto", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Decimal, Scale = 2 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@OrderId", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.VarChar, Size = 50 });

            var result = data.ExecuteProcedure("[dbo].[sp_RecuperarOrderIdPagoWeb]", parameters, parametersOut);

           string[] infoShopify = Convert.ToString(result["@OrderId"]).Split('|');

            validarFolioWebRequest.Currency = "MXN";
            validarFolioWebRequest.OrderId = Convert.ToInt64(infoShopify[0]);
            validarFolioWebRequest.TransactionId = Convert.ToInt64(infoShopify[1]);
            validarFolioWebRequest.Amount = Convert.ToDecimal(result["@Monto"]);

            validarFolioWebResponse =  IndicarPagoFolioWeb(validarFolioWebRequest);

            return validarFolioWebResponse;
        }

        private OperationResponse FinalizarTransaccionVenta(int codeStore, int codeBox, FinalizarVentaRequest finalizarVentaRequest, int totalPagosRealizados)
        {
            OperationResponse operationResponse = new OperationResponse();
            // Verificamos si se trata de una venta o de una devolución
            if (!String.IsNullOrEmpty(finalizarVentaRequest.cabeceraVentaRequest.FolioDevolucion))
            {
                // Se trata de una devolución
                operationResponse = this.FinalizarVentaDevolucionConPagos(finalizarVentaRequest.cabeceraVentaRequest.FolioOperacion, finalizarVentaRequest.cabeceraVentaRequest.FolioDevolucion,
                        finalizarVentaRequest.cabeceraVentaRequest.FolioVentaOriginal, codeBox, codeStore, finalizarVentaRequest.cabeceraVentaRequest.CodigoMayorista, finalizarVentaRequest.cabeceraVentaRequest.ImporteVentaDescuentos, finalizarVentaRequest.cabeceraVentaRequest.ImporteDevolucionImpuestos,
                        finalizarVentaRequest.cabeceraVentaRequest.ImporteVentaBruto, finalizarVentaRequest.cabeceraVentaRequest.ImporteVentaImpuestos, finalizarVentaRequest.cabeceraVentaRequest.ImporteVentaNeto, totalPagosRealizados,
                        finalizarVentaRequest.cabeceraVentaRequest.ImporteDevolucionNeto, finalizarVentaRequest.cabeceraVentaRequest.ImporteDevolucionImpuestos, finalizarVentaRequest.cabeceraVentaRequest.ImporteDevolucionTotal);
                return operationResponse;
            }
            else
            {
                // Se trata de una venta
                InformacionAsociadaRetiroEfectivo informacionAsociadaRetiroEfectivo = new InformacionAsociadaRetiroEfectivo();
                var parameters = new Dictionary<string, object>();
                parameters.Add("@CodigoTienda", codeStore);
                parameters.Add("@CodigoCaja", codeBox);
                parameters.Add("@FolioVenta", finalizarVentaRequest.FolioVenta);
                parameters.Add("@TotalPagosRealizados", totalPagosRealizados);
                if (finalizarVentaRequest.cabeceraVentaRequest == null)
                {
                    parameters.Add("@ImporteVentaDescuentos", 0);
                }
                else
                {
                    parameters.Add("@ImporteVentaDescuentos", finalizarVentaRequest.cabeceraVentaRequest.ImporteVentaDescuentos);
                }
                if (finalizarVentaRequest.InformacionMayorista != null)
                {
                    parameters.Add("@SubTotal", finalizarVentaRequest.InformacionMayorista.ImporteVentaBruto);
                    parameters.Add("@Iva", finalizarVentaRequest.InformacionMayorista.ImporteVentaImpuestos);
                    parameters.Add("@Total", finalizarVentaRequest.InformacionMayorista.ImporteVentaNeto);
                }
                else
                {
                    parameters.Add("@SubTotal", finalizarVentaRequest.cabeceraVentaRequest.ImporteVentaBruto);
                    parameters.Add("@Iva", finalizarVentaRequest.cabeceraVentaRequest.ImporteVentaImpuestos);
                    parameters.Add("@Total", finalizarVentaRequest.cabeceraVentaRequest.ImporteVentaNeto);
                }

                //parameters.Add("@versionPOS", finalizarVentaRequest.versionPOS); // OCG

                List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
                parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeEfectivoMaximo", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
                parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@EfectivoMaximoCaja", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Decimal, Scale = 2 });
                parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@DotacionInicial", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Decimal, Scale = 2 });
                parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MontoActualCaja", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Decimal, Scale = 2 });
                parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MostrarAlertaRetiroEfectivo", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Bit });
                parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@PermitirIgnorar", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Bit });
                parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
                parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });

                var result = data.ExecuteProcedure("[dbo].[sp_vanti_FinalizarTransaccionVenta]", parameters, parametersOut);

                // Llenar información referente a retiro de efectivo
                informacionAsociadaRetiroEfectivo.MensajeEfectivoMaximo = result["@MensajeEfectivoMaximo"].ToString();
                informacionAsociadaRetiroEfectivo.EfectivoMaximoPermitidoCaja = Convert.ToDecimal(result["@EfectivoMaximoCaja"]);
                informacionAsociadaRetiroEfectivo.DotacionInicialCaja = Convert.ToDecimal(result["@DotacionInicial"]);
                informacionAsociadaRetiroEfectivo.EfectivoActualCaja = Convert.ToDecimal(result["@MontoActualCaja"]);
                informacionAsociadaRetiroEfectivo.MostrarAlertaRetiroEfectivo = Convert.ToBoolean(result["@MostrarAlertaRetiroEfectivo"]);
                informacionAsociadaRetiroEfectivo.PermitirIgnorarAlertaRetiroEfectivo = Convert.ToBoolean(result["@PermitirIgnorar"]);
                operationResponse.informacionAsociadaRetiroEfectivo = informacionAsociadaRetiroEfectivo;

                // Llenar información referente a estatus de la operación
                operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
                operationResponse.CodeDescription = result["@MensajeResultado"].ToString();

            }
            return operationResponse;
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

        /// <summary>
        /// Metodo para guardar el registro exitoso de una venta de tiempo aire
        /// </summary>
        /// <param name="folioVenta">Folio de la venta</param>
        /// <param name="codigoCaja">Código de caja</param>
        /// <param name="codigoTienda">Código de tienda</param>
        /// <param name="numeroTelefonico">Numero telefonoico</param>
        /// <param name="montoRecarga">Monto de la recarga</param>
        /// <param name="idTransaccion">Id de la transaccion del servicio</param>
        /// <returns></returns>
        public OperationResponse RegistrarRecargaTelefonicaExitosa(string folioVenta, int codigoCaja, int codigoTienda, string numeroTelefonico, int montoRecarga, string idTransaccion)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioVenta", folioVenta);
            parameters.Add("@CodigoCaja", codigoCaja);
            parameters.Add("@CodigoTienda", codigoTienda);
            parameters.Add("@NumeroTelefonico", numeroTelefonico);
            parameters.Add("@MontoRecarga", montoRecarga);
            parameters.Add("@IdTransaccion", idTransaccion);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_FinalizarVentaTiempoAireExistosa]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }


        /* FUNCIONES PARA LA SUSPENSIÓN Y ANULACIÓN DE VENTAS */

        /// <summary>
        /// Suspender Venta
        /// </summary>
        /// <param name="suspenderVentaRequest">Petición de suspensión de venta</param>        
        /// <param name="codeStore">Código de tienda</param>      
        /// <param name="codeBox">Código de caja</param>      
        /// <param name="codeEmployee">Código de empleado cajero</param>  
        /// <returns>Folio de Venta</returns>
        public OperationResponse SuspenderVenta(SuspenderVentaRequest suspenderVentaRequest, int codeStore, int codeBox, int codeEmployee)
        {
            OperationResponse operationResponse = new OperationResponse();
            using (TransactionScope scope = new TransactionScope())
            {
                // Actualizar el estatus de la venta
                var parameters = new Dictionary<string, object>();
                parameters.Add("@FolioOperacion", suspenderVentaRequest.cabeceraVentaRequest.FolioOperacion);
                parameters.Add("@CodigoTienda", codeStore);
                parameters.Add("@CodigoCaja", codeBox);
                List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
                parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
                parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
                var result = this.data.ExecuteProcedure("[dbo].[sp_vanti_SuspenderVenta]", parameters, parametersOut);
                operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
                operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
                // Actualizar la cabecera                
                this.ActualizarCabeceraVenta(codeStore, codeBox, codeEmployee, suspenderVentaRequest.cabeceraVentaRequest);
                scope.Complete();
            }
            return operationResponse;
        }

        /// <summary>
        /// Anular Venta
        /// </summary>
        /// <param name="anularTotalizarVentaRequest">Parametro con el folio de la venta</param>
        /// <param name="token">Token del usuario activo</param>        
        /// <returns></returns>
        public OperationResponse AnularVenta(AnularTotalizarVentaRequest anularTotalizarVentaRequest, TokenDto token)
        {
            OperationResponse operationResponse = new OperationResponse();
            using (TransactionScope scope = new TransactionScope())
            {
                operationResponse = this.AnularTransaccionVenta(anularTotalizarVentaRequest, token);
                scope.Complete();
            }
            return operationResponse;
        }

        private OperationResponse AnularTransaccionVenta(AnularTotalizarVentaRequest anularTotalizarVentaRequest, TokenDto token)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioVenta", anularTotalizarVentaRequest.FolioVenta);
            parameters.Add("@CodigoTienda", token.CodeStore);
            parameters.Add("@CodigoCaja", token.CodeBox);
            parameters.Add("@CodigoRazon", anularTotalizarVentaRequest.CodigoRazon);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = this.data.ExecuteProcedure("[dbo].[sp_vanti_AnularTransaccionVenta]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }


        /* FUNCIONES PARA LA BUSQUEDA DE VENTAS */

        /// <summary>
        /// Busqueda de ventas por fechas y estatus
        /// </summary>
        /// <param name="busquedaTransaccionRequest">folio de venta</param>
        /// <returns>Lista de ventas que cumplen con el criterio</returns>
        public BusquedaTransaccionResponse[] ObtenerVentas(BusquedaTransaccionRequest busquedaTransaccionRequest)
        {
            List<BusquedaTransaccionResponse> lista = new List<BusquedaTransaccionResponse>();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioOperacion", busquedaTransaccionRequest.FolioOperacion);
            parameters.Add("@Estatus", busquedaTransaccionRequest.Estatus);
            parameters.Add("@FechaInicial", busquedaTransaccionRequest.FechaInicial);
            parameters.Add("@FechaFinal", busquedaTransaccionRequest.FechaFinal);
            foreach (var r in data.GetDataReader("[dbo].[sp_vanti_BuscarVentasCriterio]", parameters))
            {
                BusquedaTransaccionResponse busquedaTransaccionResponse = new BusquedaTransaccionResponse();
                busquedaTransaccionResponse.CodigoCaja = Convert.ToInt32(r.GetValue(0));
                busquedaTransaccionResponse.CodigoTienda = Convert.ToInt32(r.GetValue(1));
                busquedaTransaccionResponse.FolioOperacion = r.GetValue(2).ToString();
                busquedaTransaccionResponse.Fecha = r.GetValue(3).ToString();
                lista.Add(busquedaTransaccionResponse);
            }
            return lista.ToArray();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="solicitudAutorizacionDescuentoRequest"></param>
        /// <returns></returns>
        public SolicitudAutorizacionDescuentoResponse SolicitudAutorizacionDescuento(SolicitudAutorizacionDescuentoRequest solicitudAutorizacionDescuentoRequest)
        {
            SolicitudAutorizacionDescuentoResponse solicitudAutorizacionDescuentoResponse = new SolicitudAutorizacionDescuentoResponse();
            //var parameters = new Dictionary<string, object>();
            //parameters.Add("@FolioOperacion", busquedaTransaccionRequest.FolioOperacion);
            //parameters.Add("@Estatus", busquedaTransaccionRequest.Estatus);
            //parameters.Add("@FechaInicial", busquedaTransaccionRequest.FechaInicial);
            //parameters.Add("@FechaFinal", busquedaTransaccionRequest.FechaFinal);
            //foreach (var r in data.GetDataReader("[dbo].[sp_vanti_BuscarVentasCriterio]", parameters))
            //{
            //    BusquedaTransaccionResponse busquedaTransaccionResponse = new BusquedaTransaccionResponse();
            //    busquedaTransaccionResponse.CodigoCaja = Convert.ToInt32(r.GetValue(0));
            //    busquedaTransaccionResponse.CodigoTienda = Convert.ToInt32(r.GetValue(1));
            //    busquedaTransaccionResponse.FolioOperacion = r.GetValue(2).ToString();
            //    busquedaTransaccionResponse.Fecha = r.GetValue(3).ToString();
            //    lista.Add(busquedaTransaccionResponse);
            //}
            return solicitudAutorizacionDescuentoResponse;
        }




        /// <summary>
        /// Busqueda de venta por folio
        /// </summary>
        /// <param name="folio">folio de venta</param>
        /// <param name="esDevolucion">Indica si s etrata de una devolución</param>
        /// <returns>Venta por folio</returns>
        public VentaResponse ObtenerVentaPorFolio(string folio, int esDevolucion)
        {
            VentaResponse ventaResponse = new VentaResponse();
            ventaResponse = this.ObtenerVentaCabPorFolio(ventaResponse, folio, esDevolucion);
            ventaResponse = this.ObtenerVentaDetPorFolio(ventaResponse, folio, esDevolucion);
            ventaResponse = this.ObtenerConsecutivoSecuencia(ventaResponse, folio, esDevolucion);

            // Si es una devolución actualizamos los montos correspondientes de acuerdo a las lineas retornadas            
            if (esDevolucion == 1)
            {
                ventaResponse.ImporteVentaNeto = ventaResponse.Lineas.Sum(item => item.ImporteVentaLineaNeto);
                ventaResponse.ImporteVentaBruto = ventaResponse.Lineas.Sum(item => item.ImporteVentaLineaBruto);
                ventaResponse.ImporteVentaImpuestos = ventaResponse.Lineas.Sum(item => item.ImporteVentaLineaImpuestos1 + item.ImporteVentaLineaImpuestos2);
            }
            return ventaResponse;
        }

        private VentaResponse ObtenerVentaCabPorFolio(VentaResponse ventaResponse, string folio, int esDevolucion)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioVenta", folio);
            parameters.Add("@EsDevolucion", esDevolucion);
            foreach (var item in data.GetDataReader("[dbo].[sp_vanti_BuscarVentaCabPorFolio]", parameters))
            {
                ventaResponse.FolioVenta = folio;
                ventaResponse.Estatus = item.GetValue(1).ToString();
                ventaResponse.CodigoEmpleadoVendedor = Convert.ToInt32(item.GetValue(2));
                ventaResponse.TipoCabeceraVenta = item.GetValue(3).ToString();
                ventaResponse.ImporteVentaDescuentos = Convert.ToDecimal(item.GetValue(4));
                ventaResponse.ImporteVentaBruto = Convert.ToDecimal(item.GetValue(5));
                ventaResponse.ImporteVentaImpuestos = Convert.ToDecimal(item.GetValue(6));
                ventaResponse.ImporteVentaNeto = Convert.ToDecimal(item.GetValue(7));
                if (item.GetValue(9).ToString() == "MAYORISTA")
                {
                    ventaResponse.CodigoMayorista = Convert.ToInt32(item.GetValue(8));
                }
                if (item.GetValue(9).ToString() == "EMPLEADO")
                {
                    ventaResponse.NumeroNominaVentaEmpleado = Convert.ToInt32(item.GetValue(8));
                }
                ventaResponse.NumeroTransaccion = Convert.ToInt32(item.GetValue(10));
            }
            return ventaResponse;
        }

        private VentaResponse ObtenerVentaDetPorFolio(VentaResponse ventaResponse, string folio, int esDevolucion)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioVenta", folio);
            parameters.Add("@EsDevolucion", esDevolucion);
            List<LineaTicket> lineas = new List<LineaTicket>();
            foreach (var item in data.GetDataReader("[dbo].[sp_vanti_BuscarVentaDetPorFolio]", parameters))
            {
                LineaTicket linea = new LineaTicket();
                linea.Secuencia = Convert.ToInt32(item.GetValue(0));
                linea.CantidadVendida = Convert.ToInt32(item.GetValue(1));
                linea.CantidadDevuelta = Convert.ToInt32(item.GetValue(2));
                linea.ImporteVentaLineaImpuestos1 = Convert.ToDecimal(item.GetValue(3));
                linea.ImporteVentaLineaImpuestos2 = Convert.ToDecimal(item.GetValue(4));
                linea.TipoDetalleVenta = item.GetValue(5).ToString();

                linea.ImporteVentaLineaBruto = Convert.ToDecimal(item.GetValue(6));
                linea.ImporteVentaLineaDescuentos = Convert.ToDecimal(item.GetValue(7));
                linea.ImporteVentaLineaNeto = Convert.ToDecimal(item.GetValue(8));

                linea.DescuentoDirectoLinea = new DescuentoDirectoLinea();
                linea.DescuentoDirectoLinea.PorcentajeDescuento = Convert.ToDecimal(item.GetValue(9));
                linea.DescuentoDirectoLinea.ImporteDescuento = Convert.ToDecimal(item.GetValue(10));
                linea.DescuentoDirectoLinea.TipoDescuento = item.GetValue(11).ToString().Trim();

                linea.Articulo = new Articulo();
                linea.Articulo.Sku = Convert.ToInt32(item.GetValue(12));
                linea.Articulo.Upc = item.GetValue(13).ToString();
                linea.Articulo.Impuesto1 = Convert.ToDecimal(item.GetValue(14));
                linea.Articulo.Impuesto2 = Convert.ToDecimal(item.GetValue(15));
                linea.Articulo.CodigoImpuesto1 = item.GetValue(16).ToString();
                linea.Articulo.CodigoImpuesto2 = item.GetValue(17).ToString();
                linea.Articulo.TasaImpuesto1 = Convert.ToDecimal(item.GetValue(18));
                linea.Articulo.TasaImpuesto2 = Convert.ToDecimal(item.GetValue(19));

                linea.Articulo.PrecioConImpuestos = Convert.ToDecimal(item.GetValue(20));
                linea.Articulo.PrecioCambiadoConImpuestos = Convert.ToDecimal(item.GetValue(21));
                linea.Articulo.precioCambiadoImpuesto1 = Convert.ToDecimal(item.GetValue(22));
                linea.Articulo.precioCambiadoImpuesto2 = Convert.ToDecimal(item.GetValue(23));
                linea.Articulo.Estilo = item.GetValue(24).ToString();
                linea.Articulo.DigitoVerificadorArticulo = new DigitoVerificadorArticulo();
                linea.Articulo.DigitoVerificadorArticulo.DigitoVerificadorCorrecto = item.GetValue(25).ToString();
                linea.Articulo.DigitoVerificadorArticulo.DigitoVerificadorActual = item.GetValue(26).ToString();
                linea.Articulo.DigitoVerificadorArticulo.Inconsistencia = item.GetValue(27).ToString() == "1";

                if (item.GetValue(28).ToString() != "")
                {
                    linea.Articulo.InformacionTarjetaRegalo = new InformacionTarjetaRegalo();
                    linea.Articulo.InformacionTarjetaRegalo.FolioTarjeta = Convert.ToInt32(item.GetValue(28).ToString());
                    if (item.GetValue(28).ToString() == "-1")
                    {
                        linea.Articulo.InformacionTarjetaRegalo.FolioTarjeta = 0;
                    }
                    linea.Articulo.InformacionTarjetaRegalo.Descripcion = item.GetValue(29).ToString();
                    linea.Articulo.EsTarjetaRegalo = true;
                }

                if (item.GetValue(30).ToString() != "")
                {
                    linea.Articulo.InformacionProveedorExternoTA = new InformacionProveedorExternoTA();
                    linea.Articulo.InformacionProveedorExternoTA.NumeroTelefonico = item.GetValue(30).ToString();
                    linea.Articulo.InformacionProveedorExternoTA.SkuCompania = item.GetValue(31).ToString();
                }
                if (item.GetValue(32).ToString() != "")
                {
                    linea.Articulo.InformacionProveedorExternoAsociadaPS = new InformacionProveedorExternoAsociadaPS();
                    linea.Articulo.InformacionProveedorExternoAsociadaPS.Cuenta = item.GetValue(32).ToString();
                    linea.Articulo.InformacionProveedorExternoAsociadaPS.SkuCompania = item.GetValue(33).ToString();
                    linea.Articulo.InformacionProveedorExternoAsociadaPS.InfoAdicional = this.ObtenerdeXml(item.GetValue(34).ToString());
                }

                linea.DescuentoDirectoLinea.CodigoRazonDescuento = Convert.ToInt32(item.GetValue(35));
                linea.Articulo.EsTarjetaRegalo = Convert.ToBoolean(item.GetValue(36));

                linea.Articulo.RutaImagenLocal = item.GetValue(37).ToString();
                linea.Articulo.RutaImagenRemota = item.GetValue(38).ToString();
                linea.Articulo.InformacionPagoTCMM = new InformacionPagoTCMM();
                linea.Articulo.InformacionPagoTCMM.NumeroTarjeta = item.GetValue(39).ToString();
                linea.DescuentoDirectoLinea.DescripcionRazonDescuento = item.GetValue(40).ToString().Trim();

                lineas.Add(linea);
            }
            ventaResponse.Lineas = lineas.ToArray();
            return ventaResponse;
        }

        private VentaResponse ObtenerConsecutivoSecuencia(VentaResponse ventaResponse, string folio, int esDevolucion)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioVenta", folio);
            parameters.Add("@EsDevolucion", esDevolucion);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@ConsecutivoActual", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            var result = this.data.ExecuteProcedure("[dbo].[sp_vanti_BuscarConsecutivoActualPorFolio]", parameters, parametersOut);
            ventaResponse.ConsecutivoSecuencia = Convert.ToInt32(result["@ConsecutivoActual"].ToString()) + 1;
            return ventaResponse;
        }

        private PagoServiciosInfoAdicional ObtenerdeXml(string xml)
        {
            PagoServiciosInfoAdicional tipo = new PagoServiciosInfoAdicional();
            XmlSerializer xmlSerializer = new XmlSerializer(tipo.GetType());
            using (StringReader textReader = new StringReader(xml))
            {
                return (PagoServiciosInfoAdicional)xmlSerializer.Deserialize(textReader);
            }
        }

        /// <summary>
        /// Obtener fecha por folio
        /// </summary>
        /// <param name="folio"></param>
        /// <returns></returns>
        public DateTime ObtenerFecha(string folio)
        {
            DateTime fecha = new DateTime();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@folioVenta", folio);
            foreach (var c in data.GetDataReader("[dbo].[sp_vanti_ObtenerFechaVenta]", parameters))
                fecha = DateTime.Parse(c.GetValue(0).ToString());
            return fecha;
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="solicitudAutorizacionDescuentoRequest"></param>
        /// <returns></returns>
        public SolicitudAutorizacionDescuentoRequest ObtenInfoDescuentoSku(SolicitudAutorizacionDescuentoRequest solicitudAutorizacionDescuentoRequest)
        {
            SolicitudAutorizacionDescuentoRequest _solicitudAutorizacionDescuentoRequest = new SolicitudAutorizacionDescuentoRequest();
            var parameters = new Dictionary<string, object>();

            parameters.Add("@sku", solicitudAutorizacionDescuentoRequest.Sku);

            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();

            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@codigotienda", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@codigocaja", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@secuencia", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@fecha", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Date });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@folioOperacion", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.VarChar, Size = 50 });

            var result = data.ExecuteProcedure("[dbo].[spRecuperaFolioAutorizacion]", parameters, parametersOut);

            _solicitudAutorizacionDescuentoRequest.CodigoTienda = Convert.ToInt32(result["@codigotienda"]);
            _solicitudAutorizacionDescuentoRequest.CodigoCaja = Convert.ToInt32(result["@codigocaja"]);
            _solicitudAutorizacionDescuentoRequest.Linea = Convert.ToInt32(result["@secuencia"]);
            _solicitudAutorizacionDescuentoRequest.Fecha = Convert.ToDateTime(result["@fecha"]);
            _solicitudAutorizacionDescuentoRequest.FolioVenta = Convert.ToString(result["@folioOperacion"]);

            return _solicitudAutorizacionDescuentoRequest;
        }


        /// <summary>
        /// OCG: Recupera la url de un endpoint por id
        /// </summary>
        /// <param name="id">id del endpoint en geninformacionserviciosexternos</param>
        /// <returns></returns>
        public string EndPointUrl(int id)
        {
            string urls;
            var parameters = new Dictionary<string, object>();

            parameters.Add("@Id", id);

            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();

            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@Url", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.VarChar, Size = 200 });

            var result = data.ExecuteProcedure("[dbo].[spEndPointUrl]", parameters, parametersOut);
            urls = Convert.ToString(result["@Url"]);

            return urls;
        }

    }
}