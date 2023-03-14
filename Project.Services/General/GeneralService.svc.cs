using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using Milano.BackEnd.Business;
using Milano.BackEnd.Business.General;
using System.Threading;
using System.Globalization;
using System.Security.Cryptography;

namespace Project.Services.General
{

    /// <summary>
    /// Servicio que gestiona las operaciones referentes a temas genéricos
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class GeneralService
    {

        /// <summary>
        /// Obtiene la ip del cliente que hace la petición.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/ip")]
        public ResponseBussiness<OperationResponse> GetIp()
        {
            ResponseBussiness<OperationResponse> responseBussiness = new ResponseBussiness<OperationResponse>();
            try
            {
                OperationContext context = OperationContext.Current;
                MessageProperties prop = context.IncomingMessageProperties;
                RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                responseBussiness.Data = new OperationResponse();
                responseBussiness.Data.CodeNumber = "1";
                responseBussiness.Data.CodeDescription = endpoint.Address;
            }
            catch (Exception ex)
            {
                responseBussiness.Result = new EstatusRequest();
                responseBussiness.Result.Status = false;
                responseBussiness.Result.CodeNumber = "-1";
                responseBussiness.Result.CodeDescription = ex.Message;
            }
            return responseBussiness;
        }

        /// <summary>
        /// Servicio para cambio de divisas
        /// </summary>
        /// <param name="tipoCambioRequest">moneda nacional</param>
        /// <returns>Cambio a moneda extrangera</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/convertirDivisa")]
        public ResponseBussiness<TipoCambioResponse> CambioDivisa(TipoCambioRequest tipoCambioRequest)
        {
            return new AdministracionTipoCambio().ObtenerTipoCambio(tipoCambioRequest);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ventaDolaresRequest"></param>
        /// <returns></returns>
        //[OperationContract]
        //[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/ventaDolares")]
        //public ResponseBussiness<VentaDolaresResponse> VentaDolares(VentaDolaresRequest ventaDolaresRequest)
        //{
        //    return new AdministracionTipoCambio().ObtenerAutorizacionVentaDolares(ventaDolaresRequest);
        //}



        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/cultura")]
        public ResponseBussiness<OperationResponse> ObtenerCultura()
        {
            OperationResponse response = new OperationResponse();
            CultureInfo culture1 = CultureInfo.CurrentCulture;
            CultureInfo culture2 = Thread.CurrentThread.CurrentCulture;
            response.CodeNumber = culture1.Name + "-" + culture2.Name;
            return response;
        }

        /// <summary>
        /// Método para solicitar el almacenamiento de una imagen de artículo remota localmente
        /// </summary>
        /// <param name="almacenarImagenArticuloRequest">Información sobre la URL de la imagen remota</param>
        /// <returns>Resultado de la operación</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/almacenarImagenRemota")]
        public ResponseBussiness<OperationResponse> AlmacenarImagenArticulo(AlmacenarImagenArticuloRequest almacenarImagenArticuloRequest)
        {
            TokenDto token = new TokenService().Get();
            return new ProductImageHandler(token).AlmacenarImagenArticulo(almacenarImagenArticuloRequest);
        }

        /// <summary>
        /// Método para encriptar una cadena para los archvios de configuración de base de datos
        /// </summary>
        /// <param name="cadena">Cadena que desea encriptarse</param>
        /// <returns>Cadena encriptada</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/encriptarCadena/{cadena}")]
        public ResponseBussiness<String> EncriptarCadena(String cadena)
        {
            OperationResponse op = new OperationResponse();
            byte[] key = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
            byte[] iv = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };

            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateEncryptor(key, iv);
            byte[] inputbuffer = Encoding.Unicode.GetBytes(cadena);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return Convert.ToBase64String(outputBuffer);
        }

    }



}
