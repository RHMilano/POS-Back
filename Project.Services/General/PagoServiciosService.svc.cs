using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Business;
using System.ServiceModel.Web;

namespace Project.Services.General
{

    /// <summary>
    /// Servicio de tiempo aire
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class PagoServiciosService
    {

        /// <summary>
        /// Servicio para obtener la lista de compañias
        /// </summary>
        /// <returns>Lista de compañias</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/empresasTA")]
        public ResponseBussiness<CompaniasPagoServiciosResponse[]> ObtenerEmpresasTA()
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<CompaniasPagoServiciosResponse[]> response = new AdministracionTiempoAireBusiness(token).ObtenerListaEmpresas();
            return response;
        }

        /// <summary>
        /// Servicio para obtener montos de la compania
        /// </summary>
        /// <param name="codigoEmpresa">codigo de la compania</param>
        /// <returns>Lista de montos</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/empresasTA/{codigoEmpresa}")]
        public ResponseBussiness<ProductsResponse[]> ObtenerProductosTA(string codigoEmpresa)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<ProductsResponse[]> response = new AdministracionTiempoAireBusiness(token).ObtenerProductosTA(codigoEmpresa);
            return response;
        }

        /// <summary>
        /// Servicio para obtener la lista de empresas
        /// </summary>
        /// <returns>Lista de compañias</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/empresasServicios")]
        public ResponseBussiness<CompaniasPagoServiciosResponse[]> ObtenerEmpresas()
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<CompaniasPagoServiciosResponse[]> response = new AdministracionPagoServiciosBusiness(token).ObtenerListaEmpresas();
            return response;
        }

        /// <summary>
        /// Servicio para obtener productos
        /// </summary>
        /// <param name="codigoEmpresa">codigo de la empresa</param>
        /// <returns>Lista de montos</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/empresasServicios/{codigoEmpresa}")]
        public ResponseBussiness<ProductsResponse[]> ObtenerProductos(string codigoEmpresa)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<ProductsResponse[]> response = new AdministracionPagoServiciosBusiness(token).ObtenerProductos(codigoEmpresa);
            return response;
        }

    }
}
