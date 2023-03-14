using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using Milano.BackEnd.Business.ImpresionMM;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.Impresion;
using Milano.BackEnd.Business.Security;

namespace Project.Services.General
{

    /// <summary>
    /// Servicio que realiza operación de impresión y reimpresión de Tickets
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ImprimirTicketService
    {

        /// <summary>
        /// Reimprimir Ticket de Venta
        /// </summary>
        /// <param name="folio">Folio de la venta o apartado</param>
        /// <returns>Respuesta de la operacion</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/imprimirTicket/{folio}")]
        public ResponseBussiness<OperationResponse> ReimprimirTicket(string folio)
        {
            new SecurityBusiness().ValidarPermisos("imprimirTicket", "E");
            TokenDto token = new TokenService().Get();
            ImprimeTicketsMM imprimeTicketsMM = new ImprimeTicketsMM(token);
            OperationResponse operation = imprimeTicketsMM.PrintTicket(folio, true);
            return operation;
        }

        /// <summary>
        /// Imprimir Ticket de Venta
        /// </summary>
        /// <param name="folio">Folio de la venta o apartado</param>
        /// <param name="tipo">Tipo de la lectura</param>
        /// <returns>Respuesta de la operacion</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/imprimirLectura/{folio}/{tipo}")]
        public ResponseBussiness<OperationResponse> PrintCorte(string folio, string tipo)
        {
            new SecurityBusiness().ValidarPermisos("imprimirTicket", "E");
            TokenDto token = new TokenService().Get();
            PrintTickectLecturaBusiness printTickectLectura = new PrintTickectLecturaBusiness(token);
            PrintLecturaRequest printLecturaRequest = new PrintLecturaRequest();
            printLecturaRequest.FolioCorte = folio;
            printLecturaRequest.TipoLectura = tipo;
            OperationResponse operation = printTickectLectura.PrintNow(printLecturaRequest);
            return operation;
        }

        /// <summary>
        /// Imprimir Ticket de retiro
        /// </summary>
        /// <param name="folio">Folio de la venta o apartado</param>
        /// <returns>Respuesta de la operacion</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/imprimirEgreso/{folio}")]
        public ResponseBussiness<OperationResponse> PrintRetiroParcial(string folio)
        {
            new SecurityBusiness().ValidarPermisos("imprimirTicket", "E");
            TokenDto token = new TokenService().Get();
            PrintTicketEgresosBusiness printTicketEgresos = new PrintTicketEgresosBusiness(token);
            PrintTicketEgresosRequest printTicketEgresosRequest = new PrintTicketEgresosRequest();
            printTicketEgresosRequest.FolioCorteZ = folio;
            OperationResponse operation = printTicketEgresos.PrintNow(printTicketEgresosRequest);
            return operation;
        }

        /// <summary>
        /// Imprimir Ticket emicion nota de credito
        /// </summary>
        /// <param name="folio">Folio de la venta o apartado</param>
        /// <returns>Respuesta de la operacion</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/imprimirEmisionNotaCredito/{folio}")]
        public ResponseBussiness<OperationResponse> PrintEmisionNotaCredito(string folio)
        {
            new SecurityBusiness().ValidarPermisos("imprimirTicket", "E");
            TokenDto token = new TokenService().Get();

            PrintTicketEmisionNotaCredito printTicketEmisionNotaCredito = new PrintTicketEmisionNotaCredito(token);

            OperationResponse operation = printTicketEmisionNotaCredito.PrintNow(folio);

            return operation;
        }

        /// <summary>
        /// Imprimir Ticket emicion nota de credito
        /// </summary>
        /// <param name="reporteId">id del reporte a imprimir</param>
        /// <returns>Respuesta de la operacion</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/imprimirRelacionCaja/{reporteId}")]
        public ResponseBussiness<OperationResponse> PrintRelacionCaja(string reporteId)
        {
            new SecurityBusiness().ValidarPermisos("imprimirTicket", "E");
            TokenDto token = new TokenService().Get();
            PrintRelacionCaja printRelacionCaja = new PrintRelacionCaja(token);
            OperationResponse operationResponse = printRelacionCaja.printReporte(Convert.ToInt32(reporteId));

            // Conversión para regresa información al Front
            if (operationResponse.CodeNumber == "100")
            {
                operationResponse.CodeNumber = "402";
                operationResponse.CodeDescription = "Impresión OK";
            }
            return operationResponse;
        }
    }
}
