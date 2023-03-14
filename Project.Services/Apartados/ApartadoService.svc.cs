using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Business;
using System.Text;
using Milano.BackEnd.Dto.Sales;
using Milano.BackEnd.Business.Security;

namespace Project.Services.Apartados
{
    /// <summary>
    /// Servicio de apartados
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ApartadoService
    {
        /// <summary>
        /// Cancelar apartado
        /// </summary>
        /// <param name="folio"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/apartadoCancelar/{folio}")]
        public ResponseBussiness<OperationResponse> CancelarApartado(string folio)
        {
            TokenDto token = new TokenService().Get();
            return new ApartadosBusiness(token).CancelarApartado(folio);
        }

        /// <summary>
        /// Abonar apartado
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/apartadoAbonar")]
        public ResponseBussiness<OperationResponse> AbonarApartado(AbonoApartadoRequest request)
        {
            TokenDto token = new TokenService().Get();
            return new ApartadosBusiness(token).Abonar(request);
        }

        /// <summary>
        /// Buscar apartado
        /// </summary>
        /// <param name="request">Parmaétro correspondiente ala búsqueda del apartado</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/apartadoBuscar")]
        public ResponseBussiness<ApartadoResponse> BuscarApartado(ApartadoBusquedaRequest request)
        {
            TokenDto token = new TokenService().Get();
            return new ApartadosBusiness(token).BuscarApartado(request);
        }


        /// <summary>
        ///Servicio para guardar una linea de apartdo
        /// </summary>
        /// <param name="lineaTicket">Objeto que representa la linea del Ticket que es guardada</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/agregarLineaTicketApartado")]
        public ResponseBussiness<OperacionLineaTicketVentaResponse> AgregarLineaTicketVenta(LineaTicket lineaTicket)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<OperacionLineaTicketVentaResponse> response = new ApartadosBusiness(token).AgregarLineaTicketVenta(lineaTicket);
            return response;
        }

        /// <summary>
        ///Servicio para cambiar piezas de una linea de venta
        /// </summary>
        /// <param name="lineaTicket">Objeto que representa la linea del Ticket que es cambiada en piezas</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/cambiarPiezasLineaTicketApartado")]
        public ResponseBussiness<OperationResponse> CambiarPiezasLineaTicketVenta(LineaTicket lineaTicket)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<OperationResponse> response = new ApartadosBusiness(token).CambiarPiezasLineaTicketVenta(lineaTicket);
            return response;
        }

        /// <summary>
        ///Servicio para cambiar el precio de una linea de venta
        /// </summary>
        /// <param name="cambiarPrecioRequest">Objeto que representa la linea del Ticket que es cambiada en precio</param>
        /// <returns></returns>
        [OperationContract]        
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/cambiarPrecioLineaTicketApartado")]
        public ResponseBussiness<OperationResponse> CambiarPrecioLineaTicketVenta(CambiarPrecioRequest cambiarPrecioRequest)
        {
            new SecurityBusiness().ValidarPermisos("cambiarPrecioLineaTicketApartado", "E");
            TokenDto token = new TokenService().Get();
            ResponseBussiness<OperationResponse> response = new ApartadosBusiness(token).CambiarPrecioLineaTicketVenta(cambiarPrecioRequest);
            return response;
        }

        /// <summary>
        ///Servicio para eliminar una linea de venta
        /// </summary>
        /// <param name="eliminarLineaTicketRequest">Objeto que representa la linea del Ticket que es eliminada</param>
        /// <returns></returns>
        [OperationContract]        
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/eliminarLineaTicketApartado")]
        public ResponseBussiness<OperationResponse> EliminarLineaTicketVenta(EliminarLineaTicketRequest eliminarLineaTicketRequest)
        {
            new SecurityBusiness().ValidarPermisos("eliminarLineaTicketApartado", "E");
            TokenDto token = new TokenService().Get();
            ResponseBussiness<OperationResponse> response = new ApartadosBusiness(token).EliminarLineaTicketVenta(eliminarLineaTicketRequest.SecuenciaOriginalLineaTicket, eliminarLineaTicketRequest.LineaTicket);
            return response;
        }

        /// <summary>
        /// Totalizar Apartado
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/apartadoTotalizar")]
        public ResponseBussiness<TotalizarApartadoResponse> TotalizarApartado(TotalizarApartadoRequest request)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<TotalizarApartadoResponse> response = new ApartadosBusiness(token).TotalizarApartado(request);
            return response;
        }


        /// <summary>
        /// Servivicio para anular una venta
        /// </summary>
        /// <param name="anularTotalizarApartadoRequest">parametro con el folio de venta y formas de pago</param>
        /// <returns></returns>
        [OperationContract]        
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/anularApartado")]
        public ResponseBussiness<OperationResponse> AnularTotalizarApartado(AnularTotalizarApartadoRequest anularTotalizarApartadoRequest)
        {
            new SecurityBusiness().ValidarPermisos("anularApartado", "E");
            TokenDto token = new TokenService().Get();
            ResponseBussiness<OperationResponse> response = new ApartadosBusiness(token).AnularTotalizarApartado(anularTotalizarApartadoRequest);
            return response;
        }

        /// <summary>
        ///Servicio para finalizar una venta
        /// </summary>
        /// <param name="request">Objeto que representa el parámetro de venta a finalizar</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/apartadoFinalizar")]
        public ResponseBussiness<OperationResponse> FinalizarApartado(FinalizarApartadoRequest request)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<OperationResponse> response = new ApartadosBusiness(token).FinalizarApartado(request);
            return response;
        }

        /// <summary>
        /// Obtener dias de vencimiento
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/diasVencimiento")]
        public ResponseBussiness<ApartadoPlazosResponse[]> ObtenerDiasVencimiento()
        {
            TokenDto token = new TokenService().Get();
            return new ApartadosBusiness(token).ObtenerDiasVencimiento();
        }

    }
}
