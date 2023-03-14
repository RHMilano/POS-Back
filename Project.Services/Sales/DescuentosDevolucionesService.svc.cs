using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Activation;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.Sales;
using System.ServiceModel.Web;
using Milano.BackEnd.Business.Sales;
using Milano.BackEnd.Business;
using Milano.BackEnd.Business.Security;

namespace Project.Services.Sales
{

    /// <summary>
    /// Servicio que realiza operaciones sobre el descuento y la devolución de artículos
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class DescuentosDevolucionesService
    {

        /// <summary>
        /// Busqueda de una venta por folio con el objetivo de regresar la información de los artículos susceptibles de ser devueltos
        /// </summary>
        /// <param name="folioVenta">Folio de la venta</param>
        /// <returns>La información respectiva de la devolución solicitada</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/validarDevolucion/{folioVenta}")]
        public ResponseBussiness<VentaResponse> ValidarDevolucionVenta(string folioVenta)
        {
            TokenDto token = new TokenService().Get();
            DescuentosDevolucionesBusiness descuentosDevolucionesBusiness = new DescuentosDevolucionesBusiness(token);
            // Validar si es posible hacer una devolución            
            OperationResponse operationResponse = descuentosDevolucionesBusiness.ValidarDevolucionVenta(folioVenta);

            // Regresar respuesta
            if ((operationResponse != null) && (operationResponse.CodeNumber.Equals("303")))
            {
                return descuentosDevolucionesBusiness.GenerarDevolucion(folioVenta);
            }
            else
            {
                VentaResponse ventaResponse = new VentaResponse();
                ventaResponse.Lineas = new List<LineaTicket>().ToArray();
                return ventaResponse;
            }
        }

        /// <summary>
        /// Método para ejecutar un cambio de piezas
        /// </summary>
        /// <param name="devolverArticuloRequest">Petición para devolver un artículo</param>
        /// <returns>LResultado de la operación</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/cambiarPiezasArticuloLineaTicketDevolucion")]
        public ResponseBussiness<OperationResponse> CambiarPiezasArticuloLineaTicketDevolucion(DevolverArticuloRequest devolverArticuloRequest)
        {
            TokenDto token = new TokenService().Get();
            DescuentosDevolucionesBusiness descuentosDevolucionesBusiness = new DescuentosDevolucionesBusiness(token);
            return descuentosDevolucionesBusiness.CambiarPiezasArticuloLineaTicketDevolucion(devolverArticuloRequest);
        }

        /// <summary>
        ///Servicio para aplicar un descuento directo en linea por porcentaje o por importe
        /// </summary>
        /// <param name="lineaTicket">Objeto que representa el descuento aplicado a la linea del Ticket de Venta</param>
        /// <returns>Resultado de la operación</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/descuentoDirectoVenta")]
        public ResponseBussiness<OperationResponse> AplicarDescuentoDirecto(LineaTicket lineaTicket)
        {
            new SecurityBusiness().ValidarPermisos("descuentoDirectoVenta", "E");
            TokenDto token = new TokenService().Get();
            ResponseBussiness<OperationResponse> response = new DescuentosDevolucionesBusiness(token).AplicarDescuentoDirecto(lineaTicket);
            return response;
        }

        /// <summary>
        ///Servicio para consultar la aplicación de un descuento por mercancía dañada
        /// </summary>
        /// <param name="consultaDescuentoRequest">Objeto que representa él sku y cantidad de artículos que deben consultarse</param>
        /// <returns>Resultado de la operación</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/descuentoMercanciaDaniada")]
        public ResponseBussiness<DescuentoMercanciaDaniadaResponse> ConsultaDescuentoMercanciaDaniada(ConsultaDescuentoRequest consultaDescuentoRequest)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<DescuentoMercanciaDaniadaResponse> response = new DescuentoMercanciaDaniadaBusiness(token).ConsultarDescuentoMercanciaDaniada(consultaDescuentoRequest);
            return response;
        }

        /// <summary>
        ///Servicio para consultar la aplicación de un descuento por picos de mercancía
        /// </summary>
        /// <param name="consultaDescuentoRequest">Objeto que representa él sku y cantidad de artículos que deben consultarse</param>
        /// <returns>Resultado de la operación</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/descuentoMercanciaPico")]
        public ResponseBussiness<DescuentoMercanciaDaniadaResponse> ConsultaDescuentoPicos(ConsultaDescuentoRequest consultaDescuentoRequest)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<DescuentoMercanciaDaniadaResponse> response = new DescuentoMercanciaDaniadaBusiness(token).ConsultarDescuentoMercanciaPico(consultaDescuentoRequest);
            return response;
        }

    }
}
