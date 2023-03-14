using BBVALogic.DTO.Retail;
using BBVALogic.Retail;
using DTOPos.ApiResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using Tools;

namespace ClientBBVAv2.Sales
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class RetailBBVAv2Service : IRetailBBVAv2Service
    {
        public ResponseBussiness<SaleResponse> MultitiendaVenta(SaleRequest request)
        {
            Token token = new TokenService().Get();
            ResponseBussiness<SaleResponse> response = new ProcessSale(token).ProcessNewSale(request);
            return response;
        }

        /// <summary>
        /// EndPoint para leer los datos de la tarjeta bancaria para saber si es de debito, 
        /// crédito, se paga con puntos
        /// </summary>
        /// <param name="request">Datos de la venta</param>
        /// <returns></returns>
      
        public ResponseBussiness<Card> epointReadCard(SaleRequest request)
        {
            Token token = new TokenService().Get();
            ResponseBussiness<Card> response = new ProcessSale(token).TryReadCard(request);
            return response;
        }

        public ResponseBussiness<SaleResponse> epointCompleteSale(SaleRequest request)
        {
            Token token = new TokenService().Get();
            ResponseBussiness<SaleResponse> response = new ProcessSale(token).TryCompleteSale(request);
            return response;
        }
    }
}
