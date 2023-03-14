using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Business;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;

namespace Project.Services.Catalogs
{

    /// <summary>
    /// Servicio para listas
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class CatologoServicio
    {

        /// <summary>
        /// Lista de proveeores 
        /// </summary>
        /// <returns>Arreglo de proveedores</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/proveedores")]
        public ResponseBussiness<Elemento[]> Proveedores()
        {
            return new ElementosBusiness().Proveedores();
        }

        /// <summary>
        /// Lista de estilos por proveedor
        /// </summary>
        /// <param name="codigoProveedor">Código de proveedor</param>
        /// <returns>Arreglo de estilos</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/estilos/{codigoProveedor}")]
        public ResponseBussiness<EstiloDto[]> Estilos(string codigoProveedor)
        {
            return new ElementosBusiness().Estilos(Convert.ToInt32(codigoProveedor));
        }

        /// <summary>
        /// Lista de departamentos
        /// </summary>
        /// <returns>Arreglo de departamentos</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/departamentos")]
        public ResponseBussiness<Elemento[]> Departamentos()
        {
            return new ElementosBusiness().Departamentos();
        }

        /// <summary>
        /// Lista de subdepartamentos
        /// </summary>
        /// <param name="codigoDepartamento">codigo del departamento</param>
        /// <returns>Arreglo de departamentos</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/subDepartamentos/{codigoDepartamento}")]
        public ResponseBussiness<Elemento[]> SubDepartamentos(string codigoDepartamento)
        {
            return new ElementosBusiness().Subdepartamentos(Convert.ToInt32(codigoDepartamento));
        }

        /// <summary>
        /// Lista de clases
        /// </summary>
        /// <param name="codigoDepartamento">Codigo del departamento</param>
        /// <param name="codigoSubDepartamento">Codigo del subdepartamento</param>
        /// <returns>Arreglo de clases</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/clases/{codigoDepartamento}/{codigoSubDepartamento}")]
        public ResponseBussiness<Elemento[]> Clases(string codigoDepartamento, string codigoSubDepartamento)
        {
            return new ElementosBusiness().Clases(Convert.ToInt32(codigoDepartamento), Convert.ToInt32(codigoSubDepartamento));
        }

        /// <summary>
        /// Servicio de catalogo de divisas
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/divisas")]
        public ResponseBussiness<Divisa[]> CatalogoDivisas()
        {
            TokenDto token = new TokenService().Get();
            return new AdministracionTipoCambio().ObtenerDivisas(token.CodeStore);
        }

        /// <summary>
        /// Lista de subclases
        /// </summary>
        /// <param name="codigoDepartamento">Codigo del departamento</param>
        /// <param name="codigoSubDepartamento">Codigo del subdepartamento</param>
        /// <param name="codigoClase">Codigo de clases</param>
        /// <returns>Arreglo de subclases</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/subclases/{codigoDepartamento}/{codigoSubDepartamento}/{codigoClase}")]
        public ResponseBussiness<Elemento[]> SubClases(string codigoDepartamento, string codigoSubDepartamento, string codigoClase)
        {
            return new ElementosBusiness().SubClases(Convert.ToInt32(codigoDepartamento), Convert.ToInt32(codigoSubDepartamento), Convert.ToInt32(codigoClase));
        }

        /// <summary>
        /// Servicio de busqueda de cliente final
        /// </summary>
        /// <param name="busquedaClienteFinalRequest">Dto con parametros de busqueda</param>
        /// <returns>Resultado de la operación</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/busquedaClienteFinal")]
        public ResponseBussiness<BusquedaClienteFinalResponse[]> BusquedaClienteFinal(BusquedaClienteFinalRequest busquedaClienteFinalRequest)
        {
            TokenDto token = new TokenService().Get();
            return new MayoristasBusiness(token).BuscarClienteFinal(busquedaClienteFinalRequest);

        }

        /// <summary>
        /// Alta de cliente final
        /// </summary>
        /// <param name="altaClienteFinalRequest">Dto del cliente final</param>
        /// <returns>Resultado de la operación</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/altaClienteFinal")]
        public ResponseBussiness<BusquedaClienteFinalResponse> AltaClienteFinal(AltaClienteFinalRequest altaClienteFinalRequest)
        {
            TokenDto token = new TokenService().Get();
            return new MayoristasBusiness(token).AgregarCliente(altaClienteFinalRequest);

        }

    }
}
