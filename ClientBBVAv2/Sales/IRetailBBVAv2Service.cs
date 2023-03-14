using BBVALogic.DTO.Retail;
using DTOPos.ApiResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Services;

namespace ClientBBVAv2.Sales
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IRetailBBVAv2Service" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    
    public interface IRetailBBVAv2Service
    {
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/api/venta")]
        ResponseBussiness<SaleResponse> MultitiendaVenta(SaleRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, 
          ResponseFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.Bare,
          UriTemplate = "/api/readCard")]
        ResponseBussiness<Card> epointReadCard(SaleRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.Bare,
          UriTemplate = "/api/completeSale")]
        ResponseBussiness<SaleResponse> epointCompleteSale(SaleRequest request);
    }



}
