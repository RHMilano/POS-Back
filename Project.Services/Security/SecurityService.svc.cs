using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using Milano.BackEnd.Business;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Business.Security;
using System.Security.Permissions;

namespace Project.Services.Security
{

    /// <summary>
    /// Servicio que gestiona las operaciones de seguridad
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SecurityService
    {
        /// <summary>
        /// Método para cambiar el password
        /// </summary>
        /// <param name="userRequest">Objeto petición de tipo empleado</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/changePassword")]
        public ResponseBussiness<OperationResponse> ChangePassword(UserRequest userRequest)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<OperationResponse> response = new SecurityBusiness(token).ChangePassword(userRequest.NumberEmployee, userRequest.Password);
            return response;
        }

        /// <summary>
        /// Servicio de validación de usuario
        /// </summary>
        /// <param name="userRequest">Objeto con datos del usuario</param>
        /// <returns>Resultado de la operacion</returns>
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/validacionUsuario")]
        public ResponseBussiness<OperationResponse> LoginValidacion(UserRequest userRequest)
        {
            TokenDto token = new TokenService().Get();
            var response = new SecurityBusiness().LoginValidacion(userRequest.NumberEmployee, userRequest.Password, token.CodeStore, token.CodeBox);
            return response;
        }

    }
}
