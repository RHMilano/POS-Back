using Milano.BackEnd.Business;
using Milano.BackEnd.Business.MM;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.MM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace Project.Services.MM
{

    /// <summary>
    /// Servicio para procesar operaciones relacionadas con SUNNEL Melody Milano
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class MelodyMilanoService
    {

        /// <summary>
        /// Servicio para consultar la información de una TCMM
        /// </summary>
        /// <param name="informacionTCMMRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/consultarInformacionTCMM")]
        public ResponseBussiness<InformacionTCMMResponse> ConsultarInformacionTCMM(InformacionTCMMRequest informacionTCMMRequest)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<InformacionTCMMResponse> response = new MelodyMilanoBusiness(token).ConsultarInformacionTCMM(informacionTCMMRequest, informacionTCMMRequest.ImprimirTicket);
            return response;
        }

        /// <summary>
        /// Servicio para consultar los planes de financiamiento y descuento por primera compra en caso de aplicar
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/consultarPlanesFinanciamientoDescuentoPrimeraCompra")]
        public ResponseBussiness<PlanesFinanciamientoResponse> ConsultarPlanesFinanciamientoDescuentoPrimeraCompra(PlanesFinanciamientoRequest request)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<PlanesFinanciamientoResponse> response = new MelodyMilanoBusiness(token).ConsultarPlanesFinanciamientoDescuentoPrimeraCompra(request);
            return response;
        }

        /// <summary>
        /// OCG Servicio para registrar el cobro de la TCMM
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/finalizarCompraTCMM")]
        public ResponseBussiness<FinalizarCompraResponse> FinalizarCompraTCMM(FinalizarCompraRequest request)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<FinalizarCompraResponse> response = new MelodyMilanoBusiness(token).FinalizarCompraTCMM(request);
            return response;
        }
    }
}
