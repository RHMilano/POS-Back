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
    /// Servicio que realiza operaciones de autenticación offline
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class AutenticacionOfflineService
    {

        /// <summary>
        /// Metodo para solicitar clave de autenticacion si no se tiene conexion
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/autenticacionOffline")]
        public ResponseBussiness<ValidacionOperacionResponse> SolicitaAutenticacion(AutenticacionOfflineRequest autenticacionOfflineRequest)
        {
            TokenDto token = new TokenService().Get();
            return new AutenticacionOfflineBusiness(token).LoginOffline(autenticacionOfflineRequest);
        }
    }
}
