using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Dto.Impresion;
using Milano.BackEnd.Dto.Sales;
using Project.Dto.General;

namespace Milano.BackEnd.Repository.General
{

    /// <summary>
    /// Repositorio para obtener el ticket a imprimir
    /// </summary>
    public class PrintTicketRepository : BaseRepository
    {

        /// <summary>
        /// Trae la configuracion de la impresora
        /// </summary>
        /// <param name="printerConfigRequest"></param>
        /// <returns></returns>
        public PrinterConfigResponse getPrinterConfig(PrinterConfigRequest printerConfigRequest)
        {
            PrinterConfigResponse printerConfigResponse = new PrinterConfigResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoCaja", printerConfigRequest.CodigoCaja);
            parameters.Add("@CodigoTienda", printerConfigRequest.CodigoTienda);

            foreach (var c in data.GetDataReader("dbo.sp_vanti_ObtenerConfigImpresoraTicket", parameters))
            {
                printerConfigResponse.NombreImpresora = c.GetValue(0).ToString();
                printerConfigResponse.UrlImpresion = c.GetValue(1).ToString();
                printerConfigResponse.CodigoAperturaCajon = c.GetValue(2).ToString();

            }
            return printerConfigResponse;
        }


        /// <summary>
        /// Trae el ticket con su cabecera y pie para imprimir
        /// </summary>
        /// <param name="ticketRequest"></param>
        /// <returns></returns>
        public PrintTicketResponse getTicket(String folio)
        {
            PrintTicketResponse ticketResponse = new PrintTicketResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioOperacion", folio);

            foreach (var c in data.GetDataReader("dbo.sp_vanti_GetTicketForPrint", parameters))
            {
                ticketResponse.FolioOperacion = Convert.ToInt32(c.GetValue(0).ToString());
                ticketResponse.Cuerpo = c.GetValue(1).ToString();
                ticketResponse.Cabecera = c.GetValue(2).ToString();
                ticketResponse.Footer = c.GetValue(3).ToString();

            }

            return ticketResponse;
        }
        /// <summary>
        /// Trae las cabeceras del ticket
        /// </summary>
        /// <param name="folioVenta">Folio de la venta a imprimir</param>
        /// <returns></returns>
        public PrintTicketCabecerasResponse GetHeaders(string folioVenta)
        {
            PrintTicketCabecerasResponse printTicketCabecerasResponse = new PrintTicketCabecerasResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioOperacion", folioVenta);

            foreach (var c in data.GetDataReader("dbo.sp_vanti_GetHeadersTicket", parameters))
            {
                printTicketCabecerasResponse.FolioVenta = c.GetValue(0).ToString();
                printTicketCabecerasResponse.Cabecera = c.GetValue(1).ToString();
                printTicketCabecerasResponse.Footer = c.GetValue(2).ToString();
                printTicketCabecerasResponse.Tipo = c.GetValue(3).ToString();
                printTicketCabecerasResponse.NombreImpresora = c.GetValue(4).ToString();

            }

            return printTicketCabecerasResponse;
        }

        /// <summary>
        /// Trae los detalles del ticket para crear el cuerpo del ticket
        /// </summary>
        /// <param name="folioVenta"></param>
        /// <returns></returns>
        public PrintTicketItem[] GetDetalleVenta(string folioVenta)
        {
            List<PrintTicketItem> detalles = new List<PrintTicketItem>();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioOperacion", folioVenta);

            foreach (var c in data.GetDataReader("dbo.sp_vanti_GetDetailTicket", parameters))
            {
                PrintTicketItem tmpItem = new PrintTicketItem();

                tmpItem.Sku = c.GetValue(0).ToString();
                tmpItem.Cantidad = Convert.ToInt32(c.GetValue(1));
                tmpItem.CostoUnitario = Convert.ToInt32(c.GetValue(2));
                tmpItem.Total = Convert.ToInt32(c.GetValue(3));
                tmpItem.Descripcion = c.GetValue(4).ToString();
                tmpItem.CodigoCaja = Convert.ToInt32(c.GetValue(5).ToString());
                tmpItem.CodigoTienda = Convert.ToInt32(c.GetValue(6).ToString());
                detalles.Add(tmpItem);
            }
            return detalles.ToArray();
        }

        /// <summary>
        /// Guarda en la base datos el ticket generado
        /// </summary>
        /// <param name="ticketSaveRequest"></param>
        public OperationResponse SaveTicket(PrintTicketSaveRequest ticketSaveRequest)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@folioOperacion", ticketSaveRequest.FolioOperacion);
            parameters.Add("@codigoTienda", ticketSaveRequest.CodigoTienda);
            parameters.Add("@CodigoCaja", ticketSaveRequest.CodigoCaja);
            parameters.Add("@TipoTicket", ticketSaveRequest.TipoTicket);
            parameters.Add("@Cuerpo", ticketSaveRequest.Cuerpo);

            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_InsertarTicketGenerado]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }

        /// <summary>
        /// Extrae la informacion de la lectura X y Z
        /// </summary>
        /// <param name="printLecturaRequest"></param>
        public PrintLecturaResponse[] getLecturaDatos(PrintLecturaRequest printLecturaRequest)
        {
            List<PrintLecturaResponse> printLecturaResponseList = new List<PrintLecturaResponse>();
            var parameters = new Dictionary<string, object>();

            parameters.Add("@FolioCorte", printLecturaRequest.FolioCorte);
            parameters.Add("@TipoLectura", printLecturaRequest.TipoLectura);

            foreach (var c in data.GetDataReader("dbo.sp_vanti_DatosTicketLecturas", parameters))
            {
                PrintLecturaResponse printLecturaResponse = new PrintLecturaResponse();
                printLecturaResponse.Texto = c.GetValue(1).ToString();
                printLecturaResponse.Formato = c.GetValue(0).ToString();
                printLecturaResponseList.Add(printLecturaResponse);
            }
            return printLecturaResponseList.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cuponFolio"></param>
        /// <returns></returns>
        public PrintLecturaResponse[] getCupon(string cuponFolio)
        {
            List<PrintLecturaResponse> printLecturaResponseList = new List<PrintLecturaResponse>();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioCupon", cuponFolio);
            foreach (var c in data.GetDataReader("dbo.sp_vanti_DatosTicketLecturas", parameters))
            {
                PrintLecturaResponse printLecturaResponse = new PrintLecturaResponse();
                printLecturaResponse.Texto = c.GetValue(1).ToString();
                printLecturaResponse.Formato = c.GetValue(0).ToString();
                printLecturaResponseList.Add(printLecturaResponse);
            }
            return printLecturaResponseList.ToArray();
        }

        /// <summary>
        /// Extrae la informacion de la lectura X y Z
        /// </summary>
        /// <param name="printTicketEgresosRequest"></param>
        public PrintTicketEgresosResponse[] getEgresosTicket(PrintTicketEgresosRequest printTicketEgresosRequest)
        {
            List<PrintTicketEgresosResponse> printTicketEgresosResponsList = new List<PrintTicketEgresosResponse>();
            var parameters = new Dictionary<string, object>();
            if (string.IsNullOrEmpty(printTicketEgresosRequest.FolioCorteZ))
            {
                printTicketEgresosRequest.FolioCorteZ = "";
            }
            else
            {
                printTicketEgresosRequest.FolioRetiro = "";
            }
            parameters.Add("@FolioRetiro", printTicketEgresosRequest.FolioRetiro);
            parameters.Add("@FolioCorteZ", printTicketEgresosRequest.FolioCorteZ);

            foreach (var c in data.GetDataReader("dbo.sp_vanti_DatosTicketEgresosParciales", parameters))
            {
                PrintTicketEgresosResponse printTicketEgresos = new PrintTicketEgresosResponse();
                printTicketEgresos.Texto = c.GetValue(1).ToString();
                printTicketEgresos.Formato = c.GetValue(0).ToString();
                printTicketEgresosResponsList.Add(printTicketEgresos);

            }

            return printTicketEgresosResponsList.ToArray();
        }

        /// <summary>
        /// Extrae la informacion de la lectura X y Z
        /// </summary>
        /// <param name="folioNotaCredito"></param>
        public PrintLecturaResponse[] getNotaCredito(string folioNotaCredito)
        {
            List<PrintLecturaResponse> printLecturaResponseList = new List<PrintLecturaResponse>();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@folioNotaCredito", folioNotaCredito);
            foreach (var c in data.GetDataReader("dbo.sp_vanti_NotaCredito_EmicionTicket", parameters))
            {
                PrintLecturaResponse printLecturaResponse = new PrintLecturaResponse();
                printLecturaResponse.Texto = c.GetValue(1).ToString();
                printLecturaResponse.Formato = c.GetValue(0).ToString();
                printLecturaResponseList.Add(printLecturaResponse);
            }
            return printLecturaResponseList.ToArray();
        }

        /// <summary>
        /// Extrae la informacion de la Consulta de Saldo
        /// </summary>
        /// <param name="numeroTarjeta"></param>
        public PrintLecturaResponse[] getConsultaSaldo(string numeroTarjeta)
        {
            List<PrintLecturaResponse> printLecturaResponseList = new List<PrintLecturaResponse>();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@NumeroTarjeta", numeroTarjeta);
            foreach (var c in data.GetDataReader("sp_vanti_GetTicketConsultaSaldoMM", parameters))
            {
                PrintLecturaResponse printLecturaResponse = new PrintLecturaResponse();
                printLecturaResponse.Texto = c.GetValue(1).ToString();
                printLecturaResponse.Formato = c.GetValue(0).ToString();
                printLecturaResponseList.Add(printLecturaResponse);
            }
            return printLecturaResponseList.ToArray();
        }

    }
}
