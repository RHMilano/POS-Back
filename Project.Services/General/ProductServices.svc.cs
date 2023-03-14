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
using Milano.BackEnd.Business.General;
using Milano.BackEnd.Utils;
using Newtonsoft.Json;
using Milano.BackEnd.Dto.General;

namespace Project.Services.General
{

    /// <summary>
    /// Servicio de productos
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ProductServices
    {
        /// <summary>
        /// Busqueda rápida de productos
        /// </summary>
        /// <param name="productsRequest">filtros de busqueda</param>
        /// <returns>Arreglo de productos</returns>
        [ValidateParameterInspector("ProductsRequest")]
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/getProduct")]
        public ResponseBussiness<ProductsResponse[]> Search(ProductsRequest productsRequest)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<ProductsResponse[]> response = new ProductsBusiness(token).Search(productsRequest);
            return response;
        }

        /// <summary>
        /// Busqueda de tarjeta de regalo
        /// </summary>
        /// <param name="folioTarjeta"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/getTarjetaRegalo/{folioTarjeta}")]
        public ResponseBussiness<ProductsResponse[]> SearchTarjetaRegalo(string folioTarjeta)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<ProductsResponse[]> response = new TarjetaRegalosBusiness(token).Busqueda(folioTarjeta);
            return response;
        }

        /// <summary>
        /// Busqueda extendida de productos
        /// </summary>
        /// <param name="productsRequest">filtros de busqueda</param>
        /// <returns>Arreglo de productos</returns>
        [ValidateParameterInspector("ProductsRequest")]
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/getProductAdvanced")]
        public ResponseBussiness<ProductsFindResponse> SearchAdvanced(ProductsRequest productsRequest)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<ProductsFindResponse> response = new ProductsBusiness(token).SearchAdvance(productsRequest);
            return response;
        }

    }
}
