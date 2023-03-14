using Milano.BackEnd.Business.InicioFinDia;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.InicioFinDia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace Project.Services.InicioFinDia
{

    /// <summary>
    /// Servicio que realiza operaciones referentes a la captura de Luz
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class CapturaLuzService
    {

        /// <summary>
        /// Capturar Luz al Inicio de Día
        /// </summary>
        /// <param name="capturaLuzRequest"></param>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/capturarLuzInicioDia")]
        public ResponseBussiness<ValidacionOperacionResponse> CapturarLecturaLuzInicioDia(CapturaLuzRequest capturaLuzRequest)
        {
            TokenDto token = new TokenService().Get();
            return new LecturaLuzBusiness(token).CapturaLecturaLuzInicioDia(capturaLuzRequest);
        }

        /// <summary>
        /// Capturar Luz al Fin de Día
        /// </summary>
        /// <param name="capturaLuzRequest"></param>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/capturarLuzFinDia")]
        public ResponseBussiness<ValidacionOperacionResponse> CapturarLecturaLuzFinDia(CapturaLuzRequest capturaLuzRequest)
        {
            TokenDto token = new TokenService().Get();
            return new LecturaLuzBusiness(token).CapturaLecturaLuzFinDia(capturaLuzRequest);
        }

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/confirmacionFinDia")]
        public ResponseBussiness<String> ConfirmacionFinDia()
        {
            TokenDto token = new TokenService().Get();
            return new LecturaLuzBusiness(token).ConfirmacionFinDia();
            
        }
    }
}
