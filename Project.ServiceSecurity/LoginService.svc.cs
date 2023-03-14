using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Business;
using Milano.BackEnd.Business.Security;
using System.Web.Script.Serialization;
using Milano.BackEnd.Utils;
using System.Configuration;
using System.Web;
using System.ServiceModel.Channels;
using Milano.BackEnd.Dto.Actualizador;
using Milano.BackEnd.Business.Actualizador;

namespace Project.ServicesSecurityWCF
{
    /// <summary>
    /// Servicio de Seguridad para Login
    /// </summary>
    [ServiceContract]
    public class LoginService
    {

        /// <summary>
        /// Metodo de Login
        /// </summary>
        /// <param name="userRequest">Objeto que envia el usuario y contraseña</param>
        /// <returns>Nos regresa el token y los roles de autorización del usuario</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/login")]
        public ResponseBussiness<UserResponse> Login(UserRequest userRequest)
        {
            String ip = "::1";
            ResponseBussiness<UserResponse> response = new SecurityBusiness().Login(userRequest.NumberEmployee, userRequest.Password, userRequest.NumberAttempts, ip, userRequest.TokenDevice, userRequest.esLoginInicial);
            return response;
        }

        /// <summary>
        /// Metodo que obtiene la IP
        /// </summary>
        /// <returns></returns>
        private string GetIp()
        {
            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            return endpoint.Address;
        }

        /// <summary>
        /// Metodo que regresa la version de proyecto seguridad
        /// </summary>        
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/versionSeguridad")]
        public ResponseBussiness<String> VersionSeguridad()
        {
            return new ActualizadorBusiness().ObtenerVersionActual();
        }

        /// <summary>
        /// Metodo que regresa la version de proyecto servicios
        /// </summary>        
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/versionServicios")]
        public ResponseBussiness<String> VersionServicios()
        {
            return new ActualizadorBusiness().ObtenerVersionActual();
        }

        /// <summary>
        /// Metodo que verifica la version del pos front
        /// </summary>        
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/versionSoftware/{version}")]
        public ResponseBussiness<String> VersionSoftWare(string version)
        {
            return new ActualizadorBusiness().ObtenerVersionSoftware(version);
        }

    }
}
