using Milano.BackEnd.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using Milano.BackEnd.Business;
namespace Project.Services.Catalogs
{
    /// <summary>
    /// Servicio que obtiene el catalogo de los codigos de razón para cancelar la transacción
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]

    public class ReasonsCodesService
    {
        /// <summary>
        ///Servicio que obtiene el catalogo de los codigos de razón para cancelar la transacción
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/getReasonCode")]
        public ResponseBussiness<ReasonsCodesTransactionResponse[]> CatalogoReasonsCodesTransaction(ReasonsCodesTransactionRequest reasonsCodesRequest)
        {
            ResponseBussiness<ReasonsCodesTransactionResponse[]> response = new ReasonsCodesTransactionBusiness().CatalogoReasonsCodesTransaction(reasonsCodesRequest);
            return response;
        }
    }
}
