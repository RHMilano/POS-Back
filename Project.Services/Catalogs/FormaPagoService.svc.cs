using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Business;
using Milano.BackEnd.Dto.Catalogs;

namespace Project.Services.Catalogs
{
    /// <summary>
    /// Servicio que gestiona las operaciones referentes a temas genéricos de formas de pagos
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class FormaPagoService
    {

        /// <summary>
        /// Lista de formas de pago de tipo Vale
        /// </summary>        
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/getTiposVale")]
        public ResponseBussiness<FormaPagoResponse[]> GetFormasPagoVales()
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<FormaPagoResponse[]> response = new FormaPagoBusiness(token).GetFormasPagoVales();
            return response;
        }

    }
}
