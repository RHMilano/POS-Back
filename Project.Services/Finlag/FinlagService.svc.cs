using Milano.BackEnd.Business.Finlag;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.Finlag;
using Milano.BackEnd.Dto.Lealtad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace Project.Services.Finlag
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class FinlagService
    {
        /// <summary>
        /// Servicio que permite aplicar un vale en finlag y lo registra en el POS
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/procesarMovimientoAplicarValeFinlag")]
        public ResponseBussiness<OperationResponse> AplicaVale(AplicaValeRequest aplicaValeRequest)
        {
            TokenDto token = new TokenService().Get();
            return new FinlagBusiness(token).ProcesarAplicarValeFinlag(aplicaValeRequest);
        }

        /// <summary>
        /// Servicio que permite cancelar un vale que se haya aplicado a una venta de finlag
        /// </summary>
        /// <param name="cancelaAplicaVale"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/cancelarVale")]
        public ResponseBussiness<CancelaAplicaValeResponse> CancelaAplicaVale(CancelaAplicaValeRequest cancelaAplicaVale)
        {
            TokenDto token = new TokenService().Get();
            return new CancelaAplicaValeResponse();
        }

        /// <summary>
        /// Método para consultar un cliente de finlag
        /// </summary>
        /// <param name="clienteFinlag"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/consultaCliente")]
        public ResponseBussiness<ClienteFinlagResponse> ConsultaCliente(ClienteFinlag clienteFinlag)
        {
            TokenDto token = new TokenService().Get();
            return new FinlagBusiness(token).ConsultarCliente(clienteFinlag);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clienteFinlagRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/consultaMovimientos")]
        public ResponseBussiness<List<ConsultaValeFinlagResult>> ConsultaMovimientos(ConsultaMovientoPDVRequest clienteFinlagRequest)
        {
            TokenDto token = new TokenService().Get();
            return new FinlagBusiness(token).ConsultarMovimientos(clienteFinlagRequest);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/consultaMovimientosPDV")]
        public ResponseBussiness<ConsultaMovientoPDVResult> ConsultaMovimientoPDV(ConsultaMovientoPDVRequest consultaMovientoPDVRequest)
        {
            TokenDto token = new TokenService().Get();
            return new ConsultaMovientoPDVResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="consultaValeFinlagRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/consultaVale")]
        public ResponseBussiness<ConsultaValeFinlagResult> ConsultaValeFinlag(ConsultaValeFinlagRequest consultaValeFinlagRequest)
        {
            TokenDto token = new TokenService().Get();
            return new FinlagBusiness(token).ConsultarVale(consultaValeFinlagRequest);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tablaAmortizacionRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/obtenerTablaAmortizacion")]
        public ResponseBussiness<List<TablaAmortizacionResult>> ObtenerTablaAmortizacion(TablaAmortizacionRequest tablaAmortizacionRequest)
        {
            TokenDto token = new TokenService().Get();
            return new FinlagBusiness(token).ObtenerTablaAmortizacion(tablaAmortizacionRequest);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="consultaValeFinlagRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/obtenerTramaImpresion")]
        public ResponseBussiness<ConsultaTramaImpresionResult> ObtenerTramaImpresion(ConsultaValeFinlagRequest consultaValeFinlagRequest)
        {
            TokenDto token = new TokenService().Get();
            return new FinlagBusiness(token).ObtenerTramaImpresion(consultaValeFinlagRequest);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validaValeRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/validaVale")]
        public ResponseBussiness<ValidaValeResult> ValidaVale(ValidaValeRequest validaValeRequest)
        {
            TokenDto token = new TokenService().Get();
            return new FinlagBusiness(token).ValidarVale(validaValeRequest);
        }


        /// <summary>
        /// Registra como lealtad a un cliente
        /// </summary>
        /// <param name="registroLealtadRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/registrolealtad")]
        public ResponseBussiness<RegistroLealtadResponse> RegistroLealtad(RegistroLealtadRequest registroLealtadRequest)
        {
            TokenDto token = new TokenService().Get();
            return new FinlagBusiness(token).RegistrarClienteLealtad(registroLealtadRequest);
        }

        /// <summary>
        /// Registra como lealtad a un cliente
        /// </summary>
        /// <param name="consultaLealtadRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/consultalealtad")]
        public ResponseBussiness<ConsultaClienteLealtadResponse> ConsultaLealtad(ConsultaClienteLealtadRequest consultaLealtadRequest)
        {
            TokenDto token = new TokenService().Get();
            return new FinlagBusiness(token).ConsultarClienteLealtad(consultaLealtadRequest);
        }





    }
}
