using Milano.BackEnd.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Dto.Security;
using Milano.BackEnd.Business.General;

namespace Project.Services.General
{
    /// <summary>
    /// Servicio que regresa las configuraciones generales de la caja y de la tienda como colores
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ConfigGeneralesCajaTiendaService
    {

        /// <summary>
        ///Servicio que regresa las configuraciones de acuerdo a tienda y caja
        /// </summary>        
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/getConfiguration")]
        public ResponseBussiness<ConfigGeneralesCajaTiendaResponse> getGeneralConfig(VersionRequest versionRequest )
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<ConfigGeneralesCajaTiendaResponse> response = new ConfigGeneralesCajaTiendaBusiness(token).getConfigs(versionRequest.versionPOS);
            return response;
        }

    }
}