using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Business.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace Project.Services.General
{
    /// <summary>
    /// Servicio que busca la información de los Mayoristas
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class MayoristasService
    {
        /// <summary>
        /// Servicio de busqueda de mayoristas
        /// </summary>
        /// <param name="buscarDatosMayoristasRequest">Objeto de busqueda de mayorista</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/getMayorista")]
        public ResponseBussiness<BuscarDatosMayoristasResponse> searchMayoristas(BuscarDatosMayoristasRequest buscarDatosMayoristasRequest)
        {
            ResponseBussiness<BuscarDatosMayoristasResponse> response = new BuscarDatosMayoristasBusiness().searchMayorista(buscarDatosMayoristasRequest);
            return response;
        }
    }
}
