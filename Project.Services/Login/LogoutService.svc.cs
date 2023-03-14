using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Milano.BackEnd.Business.Security;
using Milano.BackEnd.Dto;
using Project.Services;

//namespace Project.ServicesSecurityWCF
namespace Project.Services.Login
{

    /// <summary>
    /// Servicio de Seguridad para Logout
    /// </summary>
    [ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class LogoutService
    {

        /// <summary>
        /// Metodo de hacer Logout
        /// </summary>        
        /// <returns>Regresa resultado del cierre de sesion</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/logout")]
        public ResponseBussiness<OperationResponse> Logout()
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<OperationResponse> response = new SecurityBusiness(token).Logout();
            return response;
        }

    }
}
