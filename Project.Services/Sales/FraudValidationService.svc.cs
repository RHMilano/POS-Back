using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using Milano.BackEnd.Business.General;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.Sales;
using System.ServiceModel.Web;
using Milano.BackEnd.Business.Sales;

namespace Project.Services.Sales
{
    /// <summary>
    /// Servicio que valida fraude en Tiempo Aire
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class FraudValidationService
    {
        /// <summary>
        ///Servicio de validacion de fraude en tiempo aire
        /// </summary>
        /// <param name="fraudValidationRequest">Objeto que representa el parámetro de numero telefonico</param>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/fraudValidation")]
        public ResponseBussiness<OperationResponse> FraudValidationTA(FraudValidationRequest fraudValidationRequest)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<OperationResponse> response = new FraudValidationBusiness(token).FraudValidationTA(fraudValidationRequest);
            return response;
        }
    }
}
