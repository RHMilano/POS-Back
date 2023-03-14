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
using Milano.BackEnd.Business.Sales;

namespace Project.Services.Sales
{
    /// <summary>
    /// Servicio que gestiona las operaciones referentes a la venta
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SalesService
    {

        /// <summary>
        ///Servicio para guardar una linea de venta
        /// </summary>
        /// <param name="lineaTicket">Objeto que representa la linea del Ticket que es guardada</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/agregarLineaTicketVenta")]
          public ResponseBussiness<OperacionLineaTicketVentaResponse> AgregarLineaTicketVenta(LineaTicket lineaTicket)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<OperacionLineaTicketVentaResponse> response = new SalesBusiness(token).AgregarLineaTicketVenta(lineaTicket);
            return response;
        }

        /// <summary>
        /// Elimina la linea de comision de mayorista 
        /// </summary>
        /// <param name="cabecera"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/eliminarLineaMayorista")]
        public ResponseBussiness<OperationResponse> EliminarLineaMayorista(CabeceraVentaRequest cabecera)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<OperationResponse> response = new SalesBusiness(token).EliminarLineaMayorista(cabecera);
            return response;
        }

        /// <summary>
        ///Servicio para cambiar piezas de una linea de venta
        /// </summary>
        /// <param name="lineaTicket">Objeto que representa la linea del Ticket que es cambiada en piezas</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/cambiarPiezasLineaTicketVenta")]
        public ResponseBussiness<OperationResponse> CambiarPiezasLineaTicketVenta(LineaTicket lineaTicket)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<OperationResponse> response = new SalesBusiness(token).CambiarPiezasLineaTicketVenta(lineaTicket);
            return response;
        }

        /// <summary>
        ///Servicio para cambiar el precio de una linea de venta
        /// </summary>
        /// <param name="cambiarPrecioRequest">Objeto que representa la linea del Ticket que es cambiada en precio</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/cambiarPrecioLineaTicketVenta")]
        public ResponseBussiness<OperationResponse> CambiarPrecioLineaTicketVenta(CambiarPrecioRequest cambiarPrecioRequest)
        {
            new SecurityBusiness().ValidarPermisos("cambiarPrecioLineaTicketVenta", "E");
            TokenDto token = new TokenService().Get();
            ResponseBussiness<OperationResponse> response = new SalesBusiness(token).CambiarPrecioLineaTicketVenta(cambiarPrecioRequest);
            return response;
        }

        /// <summary>
        ///Servicio para eliminar una linea de venta
        /// </summary>
        /// <param name="eliminarLineaTicketRequest">Objeto que representa la linea del Ticket que es eliminada</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/eliminarLineaTicketVenta")]
        public ResponseBussiness<OperationResponse> EliminarLineaTicketVenta(EliminarLineaTicketRequest eliminarLineaTicketRequest)
        {
            new SecurityBusiness().ValidarPermisos("eliminarLineaTicketVenta", "E");
            TokenDto token = new TokenService().Get();
            ResponseBussiness<OperationResponse> response = new SalesBusiness(token).EliminarLineaTicketVenta(eliminarLineaTicketRequest.SecuenciaOriginalLineaTicket, eliminarLineaTicketRequest.LineaTicket, eliminarLineaTicketRequest.CodigoRazon);
            return response;
        }

        /// <summary>
        ///Servicio para totalizar una venta
        /// </summary>
        /// <param name="totalizarVentaRequest">Objeto que representa el parámetro de venta a totalizar</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/totalizarVenta")]
        public ResponseBussiness<TotalizarVentaResponse> TotalizarVenta(TotalizarVentaRequest totalizarVentaRequest)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<TotalizarVentaResponse> response = new SalesBusiness(token).TotalizarVenta(totalizarVentaRequest);
            return response;
        }

        /// <summary>
        ///Servicio para suspender una venta
        /// </summary>
        /// <param name="suspenderVentaRequest">Objeto que representa el parámetro de venta a suspender</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/suspenderVenta")]
        public ResponseBussiness<OperationResponse> SuspenderVenta(SuspenderVentaRequest suspenderVentaRequest)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<OperationResponse> response = new SalesBusiness(token).SuspenderVenta(suspenderVentaRequest);
            return response;
        }


        /// <summary>
        ///Servicio para finalizar una venta
        /// </summary>
        /// <param name="finalizarVentaRequest">Objeto que representa el parámetro de venta a finalizar</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/finalizarVenta")]
        public ResponseBussiness<OperationResponse> FinalizarVenta(FinalizarVentaRequest finalizarVentaRequest)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<OperationResponse> response = new SalesBusiness(token).FinalizarVenta(finalizarVentaRequest);
            return response;
        }



        /// <summary>
        /// OCG: Enviar solicitud de autorizacion para el gerente
        /// </summary>
        /// <param name="solicitudAutorizacionDescuento">Objeto que representa el parámetro de venta a finalizar</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/solicitudAutorizacionDescuento")]
        public ResponseBussiness<OperationResponse> SolicitudAutorizacionDescuento(SolicitudAutorizacionDescuentoRequest solicitudAutorizacionDescuento)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<OperationResponse> response = new SalesBusiness(token).SolicitudAutorizacionDescuento(solicitudAutorizacionDescuento,token.CodeStore, token.CodeBox);
            return response;
        }

        /// <summary>
        /// Servivicio para anular una venta
        /// </summary>
        /// <param name="anularTotalizarVentaRequest">parametro con el folio de venta y formas de pago</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/anularVenta")]
        public ResponseBussiness<OperationResponse> AnularTotalizarVenta(AnularTotalizarVentaRequest anularTotalizarVentaRequest)
        {
            new SecurityBusiness().ValidarPermisos("anularVenta", "E");
            TokenDto token = new TokenService().Get();
            ResponseBussiness<OperationResponse> response = new SalesBusiness(token).AnularTotalizarVenta(anularTotalizarVentaRequest);
            return response;
        }

        /// <summary>
        /// Servivicio para post-anular la venta
        /// </summary>
        /// <param name="postAnularVentaRequest">Parámetro con el folio de venta y formas de pago</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/postAnularVenta")]
        public ResponseBussiness<OperationResponse> PostAnularVenta(PostAnularVentaRequest postAnularVentaRequest)
        {
            new SecurityBusiness().ValidarPermisos("postAnularVenta", "E");
            TokenDto token = new TokenService().Get();
            ResponseBussiness<OperationResponse> response = new SalesBusiness(token).PostAnularVenta(postAnularVentaRequest);
            return response;
        }


        /* FUNCIONES PARA LA BÚSQUEDA DE VENTAS */

        /// <summary>
        /// Verifica si se puede realizar la venta a empleado
        /// </summary>		
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/validarVentaEmpleado")]
        public ResponseBussiness<OperationResponse> ValidarVentaEmpleado()
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<OperationResponse> response = new AdministracionVentaEmpleadoBusiness().ValidarVentaEmpleado();
            return response;
        }

        
       /// <summary>
       /// 
       /// </summary>
       /// <param name="solicitudAutorizacionDescuentoRequest"></param>
       /// <returns></returns>
        [OperationContract]
        [WebInvoke( Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/solicitudAutorizacion")]
        public ResponseBussiness<SolicitudAutorizacionDescuentoResponse> SolicitudAutorizacion(SolicitudAutorizacionDescuentoRequest solicitudAutorizacionDescuentoRequest)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<SolicitudAutorizacionDescuentoResponse> response = new SalesBusiness(token).SolicitudAutorizacion(solicitudAutorizacionDescuentoRequest);
            return response;
        }

        /// <summary>
        /// Busqueda de transacciones
        /// </summary>
        /// <param name="busquedaTransaccionRequest">Parámetros de la búsqueda como folio y fechas de inicio y fin</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/getTransacciones")]
        public ResponseBussiness<BusquedaTransaccionResponse[]> BuscarVentasPorFolioFecha(BusquedaTransaccionRequest busquedaTransaccionRequest)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<BusquedaTransaccionResponse[]> response = new SalesBusiness(token).BuscarVentasPorFolioFecha(busquedaTransaccionRequest);
            return response;
        }

        /// <summary>
        /// Busqueda de una venta por folio
        /// </summary>
        /// <param name="folio"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, 
            BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/venta/{folio}")]
        public ResponseBussiness<VentaResponse> BuscarVentaPorFolio(string folio)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<VentaResponse> response = new SalesBusiness(token).BuscarVentaPorFolio(folio, 0);
            return response;
        }

        /// <summary>
        /// Busqueda de clientes
        /// </summary>
        /// <param name="clienteRequest">parametro de busqueda</param>
        /// <returns>Lista de clientes</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/clientes")]
        public ResponseBussiness<ClienteResponse[]> BusquedaCliente(ClienteRequest clienteRequest)
        {
            TokenDto token = new TokenService().Get();
            return new ClientesBusiness(token).BusquedaClientes(clienteRequest);

        }

        /// <summary>
        /// Servicio de alta de clientes
        /// </summary>
        /// <param name="altaClienteRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/altaClientes")]
        public ResponseBussiness<AltaClientesResponse> AltaClientes(AltaClienteRequest altaClienteRequest)
        {
            TokenDto token = new TokenService().Get();
            return new ClientesBusiness(token).AltaClientes(altaClienteRequest);

        }

        /// <summary>
        /// Busqueda de mayorista
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/busquedaMayoristas")]
        public ResponseBussiness<BusquedaMayoristasResponse[]> BusquedaMayoristas(BusquedaMayoristasRequest request)
        {
            TokenDto token = new TokenService().Get();
            return new MayoristasBusiness(token).BusquedaMayoristas(request);
        }

        /// <summary>
        /// Pago de mayorista
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/pagoMayoristas")]
        public ResponseBussiness<BusquedaMayoristaResponse> BusquedaMayorista(BusquedaMayoristaRequest request)
        {
            TokenDto token = new TokenService().Get();
            return new MayoristasBusiness(token).BusquedaMayorista(request);
        }

        /// <summary>
        /// ObtenerPagosServicios
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/pagoServiciosOpciones")]
        public ResponseBussiness<PagoServiciosResponse> ObtenerOpcionesPagoServicios(InfoElementosRequest request)
        {
            TokenDto token = new TokenService().Get();
            return new AdministracionPagoServiciosBusiness(token).OpcionesAdicionales(request);
        }

        /// <summary>
        /// Verificar y obtener saldo de cupon
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/validarCupon")]
        public ResponseBussiness<CuponRedimirResponse> ValidarCupon(ValidarCuponRequest request)
        {
            TokenDto token = new TokenService().Get();
            return new CuponesRedimirBusiness(token).SaldoRedimir(request);
        }

        /// <summary>
        /// Autoriza Cancelacion Transaccion
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/autorizaCancelacionTransaccion")]
        public ResponseBussiness<AutorizaCancelacionTransaccionResponse> AutorizaCancelacionTransaccion(AutorizaCancelacionTransaccionRequest request)
        {
            TokenDto token = new TokenService().Get();            
            return new AutorizaCancelacionTransaccionBusiness(token).AutorizaCancelacionTransaccion(request);
        }


        /// <summary>
        /// Busqueda de una venta por folio
        /// </summary>
        /// <param name="folio"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/verificarPagoWeb/{folio}")]
        public ResponseBussiness<ValidarFolioWebResponse> verificarPagoWeb(string folio)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<ValidarFolioWebResponse> response = new SalesBusiness(token).ValidarFolioWeb(folio);
            return response;
        }




    }
}
