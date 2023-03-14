using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Dto.Recovery;
using Milano.BackEnd.Dto.Sales;
using Milano.BackEnd.Repository.General;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;
namespace Milano.BackEnd.Repository
{

    /// <summary>
    /// Repositorio de Devoluciones y Descuentos
    /// </summary>
    public class DescuentosDevolucionesRepository : BaseRepository
    {

        /// <summary>
        /// Validar si es posible hacer una devolución
        /// </summary>        
        /// <param name="codeStore">Código de tienda</param>      
        /// <param name="codeBox">Código de caja</param>      
        /// <param name="codeEmployee">Código de empleado cajero</param>          
        /// <param name="folioVenta">Folio de la venta</param>  
        /// <returns>Resultado de la operación</returns>
        public OperationResponse ValidarDevolucionVenta(int codeStore, int codeBox, int codeEmployee, string folioVenta)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@CodigoCaja", codeBox);
            parameters.Add("@FolioVenta", folioVenta);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = this.data.ExecuteProcedure("[dbo].[sp_vanti_server_ValidarPosibleDevolucion]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }

        /// <summary>
        /// Generar una devolución
        /// </summary>        
        /// <param name="codeStore">Código de tienda</param>      
        /// <param name="codeBox">Código de caja</param>      
        /// <param name="codeEmployee">Código de empleado cajero</param>          
        /// <param name="codigoTipoTransaccion">Código tipo transacción</param>        
        /// <param name="folioVentaOriginal">Folio de la venta</param>        
        /// <param name="numeroNominaVentaEmpleado">Número nómina venta empleado</param>  
        /// <param name="codigoMayorista">Código de Mayorista</param>  
        /// <returns>Resultado de la operación</returns>
        public DevolucionRespose GenerarDevolucion(int codeStore, int codeBox, int codeEmployee, String codigoTipoTransaccion,
            String folioVentaOriginal, int numeroNominaVentaEmpleado, int codigoMayorista)
        {
            DevolucionRespose devolucionRespose = new DevolucionRespose();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@CodigoCaja", codeBox);
            parameters.Add("@CodigoEmpleado", codeEmployee);
            parameters.Add("@CodigoTipoTransaccion", codigoTipoTransaccion);
            parameters.Add("@FolioVentaOriginal", folioVentaOriginal);
            parameters.Add("@NumeroNominaVentaEmpleado", numeroNominaVentaEmpleado);
            parameters.Add("@CodigoMayorista", codigoMayorista);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@FolioVenta", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 17 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@FolioDevolucion", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 17 });
            var result = this.data.ExecuteProcedure("[dbo].[sp_vanti_GenerarDevolucion]", parameters, parametersOut);
            devolucionRespose.FolioDevolucion = result["@FolioDevolucion"].ToString();
            devolucionRespose.FolioVenta = result["@FolioVenta"].ToString();
            return devolucionRespose;
        }

        /// <summary>
        /// Cambiar piezas para una devolución
        /// </summary>        
        /// <param name="codeStore">Código de tienda</param>      
        /// <param name="codeBox">Código de caja</param>      
        /// <param name="codeEmployee">Código de empleado cajero</param>          
        /// <param name="devolverArticuloRequest">Linea correspondiente a la devolución</param>  
        /// <returns>Resultado de la operación</returns>
        public OperationResponse CambiarPiezasArticuloLineaTicketDevolucion(int codeStore, int codeBox, int codeEmployee, DevolverArticuloRequest devolverArticuloRequest)
        {
            OperationResponse operationResponse = new OperationResponse();
            using (TransactionScope scope = new TransactionScope())
            {
                var parameters = new Dictionary<string, object>();
                parameters.Add("@CodigoTienda", codeStore);
                parameters.Add("@CodigoCaja", codeBox);
                parameters.Add("@CodigoEmpleado", codeEmployee);
                parameters.Add("@CodigoTipoTransaccionCabecera", devolverArticuloRequest.LineaTicket.cabeceraVentaRequest.TipoCabeceraVenta);
                parameters.Add("@FolioVenta", devolverArticuloRequest.LineaTicket.cabeceraVentaRequest.FolioOperacion);
                parameters.Add("@FolioVentaOriginal", devolverArticuloRequest.LineaTicket.cabeceraVentaRequest.FolioVentaOriginal);
                parameters.Add("@FolioDevolucion", devolverArticuloRequest.LineaTicket.cabeceraVentaRequest.FolioDevolucion);
                parameters.Add("@Secuencia", devolverArticuloRequest.LineaTicket.Secuencia);
                parameters.Add("@CantidadVendida", devolverArticuloRequest.LineaTicket.CantidadVendida);
                parameters.Add("@CantidadDevuelta", devolverArticuloRequest.LineaTicket.CantidadDevuelta);
                parameters.Add("@CodigoImpuesto1", devolverArticuloRequest.LineaTicket.Articulo.CodigoImpuesto1);
                parameters.Add("@CodigoImpuesto2", devolverArticuloRequest.LineaTicket.Articulo.CodigoImpuesto2);
                parameters.Add("@TasaImpuesto1", devolverArticuloRequest.LineaTicket.Articulo.TasaImpuesto1);
                parameters.Add("@TasaImpuesto2", devolverArticuloRequest.LineaTicket.Articulo.TasaImpuesto2);
                parameters.Add("@ImporteImpuesto1", devolverArticuloRequest.LineaTicket.ImporteVentaLineaImpuestos1);
                parameters.Add("@ImporteImpuesto2", devolverArticuloRequest.LineaTicket.ImporteVentaLineaImpuestos2);
                parameters.Add("@CostoUnitario", devolverArticuloRequest.LineaTicket.Articulo.PrecioConImpuestos);
                parameters.Add("@ImporteImpuestoDevolucion1", devolverArticuloRequest.LineaTicket.ImporteDevolucionLineaImpuestos1);
                parameters.Add("@ImporteImpuestoDevolucion2", devolverArticuloRequest.LineaTicket.ImporteDevolucionLineaImpuestos2);
                parameters.Add("@ImporteSubtotal", devolverArticuloRequest.LineaTicket.ImporteVentaLineaBruto);
                parameters.Add("@ImporteTotal", devolverArticuloRequest.LineaTicket.ImporteVentaLineaNeto);
                parameters.Add("@ImporteDevolucionSubtotal", devolverArticuloRequest.LineaTicket.ImporteDevolucionLineaBruto);
                parameters.Add("@ImporteDevolucionIva", devolverArticuloRequest.LineaTicket.ImporteDevolucionLineaImpuestos);
                parameters.Add("@ImporteDevolucionTotal", devolverArticuloRequest.LineaTicket.ImporteDevolucionLineaNeto);
                parameters.Add("@ImporteDescuento", devolverArticuloRequest.LineaTicket.ImporteVentaLineaDescuentos);
                parameters.Add("@CodigoRazon", devolverArticuloRequest.CodigoRazon);
                // Propiedades de cabecera devolución
                parameters.Add("@ImporteVentaDevolucionImpuestos", devolverArticuloRequest.LineaTicket.cabeceraVentaRequest.ImporteDevolucionImpuestos);
                parameters.Add("@ImporteVentaDevolucionTotal", devolverArticuloRequest.LineaTicket.cabeceraVentaRequest.ImporteDevolucionNeto);
                // Propiedades de cabecera devolución
                List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
                parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
                parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
                var result = this.data.ExecuteProcedure("[dbo].[sp_vanti_CambiarPiezasArticuloLineaTicketDevolucion]", parameters, parametersOut);
                operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
                operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
                // Actualizar la cabecera de Venta
                new SalesRepository().ActualizarCabeceraVenta(codeStore, codeBox, codeEmployee, devolverArticuloRequest.LineaTicket.cabeceraVentaRequest);
                scope.Complete();
            }
            return operationResponse;
        }

        /// <summary>
        /// Aplicar descuento directo sobre línea
        /// </summary>        
        /// <param name="codeStore">Código de tienda</param>      
        /// <param name="codeBox">Código de caja</param>      
        /// <param name="codeEmployee">Código de empleado cajero</param>  
        /// <param name="lineaTicket">Objeto de peticion linea ticket de la venta</param>        
        /// <returns>Resultado de la operación</returns>
        public OperationResponse AplicarDescuentoDirecto(int codeStore, int codeBox, int codeEmployee, LineaTicket lineaTicket)
        {
            OperationResponse operationResponse = new OperationResponse();
            using (TransactionScope scope = new TransactionScope())
            {
                var parameters = new Dictionary<string, object>();
                parameters.Add("@FolioOperacion", lineaTicket.cabeceraVentaRequest.FolioOperacion);
                parameters.Add("@CodigoTienda", codeStore);
                parameters.Add("@CodigoCaja", codeBox);
                parameters.Add("@SecuenciaLineaTicket", lineaTicket.Secuencia);
                parameters.Add("@PorcentajeDescuento", lineaTicket.DescuentoDirectoLinea.PorcentajeDescuento);
                parameters.Add("@ImporteDescuento", lineaTicket.DescuentoDirectoLinea.ImporteDescuento);
                parameters.Add("@CodigoRazonDescuento", lineaTicket.DescuentoDirectoLinea.CodigoRazonDescuento);
                parameters.Add("@TipoDescuento", lineaTicket.DescuentoDirectoLinea.TipoDescuento);
                parameters.Add("@CantidadVendida", lineaTicket.CantidadVendida);
                parameters.Add("@CantidadDevuelta", lineaTicket.CantidadDevuelta);
                parameters.Add("@ImporteImpuesto1", lineaTicket.ImporteVentaLineaImpuestos1);
                parameters.Add("@ImporteImpuesto2", lineaTicket.ImporteVentaLineaImpuestos2);
                parameters.Add("@ImporteVentaLineaDescuentos", lineaTicket.ImporteVentaLineaDescuentos);
                parameters.Add("@ImporteSubtotal", lineaTicket.ImporteVentaLineaBruto);
                parameters.Add("@ImporteTotal", lineaTicket.ImporteVentaLineaNeto);
                if ((lineaTicket.DescuentoDirectoLinea.ULSession != null) && (lineaTicket.DescuentoDirectoLinea.ULSession.Length > 10))
                {
                    parameters.Add("@ULSession", lineaTicket.DescuentoDirectoLinea.ULSession);
                    parameters.Add("@EstatusDescuento", "P");
                }
                else
                {
                    parameters.Add("@ULSession", "");
                    parameters.Add("@EstatusDescuento", "F");
                }
                List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
                parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
                parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
                var result = this.data.ExecuteProcedure("[dbo].[sp_vanti_ProcesarDescuentosDet]", parameters, parametersOut);
                operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
                operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
                // Actualizar la cabecera de Venta
                new SalesRepository().ActualizarCabeceraVenta(codeStore, codeBox, codeEmployee, lineaTicket.cabeceraVentaRequest);
                scope.Complete();
            }
            return operationResponse;
        }

    }
}