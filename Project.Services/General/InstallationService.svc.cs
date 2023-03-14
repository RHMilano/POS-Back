using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using Milano.BackEnd.Business.General;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.General;

namespace Project.Services.General
{
    /// <summary>
    /// Servicio que guarda la configuración de caja
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ConfigurationService 
    {
        /// <summary>
        /// Servicio de configuración
        /// </summary>
        /// <param name="configurationServiceRequest">Objeto que representa el parámetro de inserción de la configuración de caja</param>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/newPOS")]
        public ResponseBussiness<OperationResponse> InsertConfigurationBox(ConfiguracionServiceRequest configurationServiceRequest)
        {
            ResponseBussiness<OperationResponse> response = new InstallationServiceBusiness().InsertConfigurationBox(configurationServiceRequest);
            return response;
        }
    }
}
