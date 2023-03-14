using BBVALogic.DTO.Retail;
using BBVALogic.DTOCompatibility_1_5;
using DTOPos.ApiResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ClientBBVAv2.Sales
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IServiceCompatibility_1_5" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IServiceCompatibility_1_5
    {
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.Bare,
          UriTemplate = "/api/paymentpinpad/payvisamaster")]
        PayVisaMasterCardResponse PayVisaMasterCard(PayVisaMasterCardRequest request);
    }
}
