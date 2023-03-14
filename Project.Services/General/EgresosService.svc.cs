using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using Milano.BackEnd.Business.General;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Business.Security;

namespace Project.Services.General
{

    /// <summary>
    /// Servicio que realiza operaciones referentes a Egresos
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class EgresosService
    {

        /// <summary>
        /// Función para hacer retiros parciales de efectivo
        /// </summary>
        /// <param name="retiroParcialEfectivo">Objeto que representa el retiro parcial de efectivo</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/retiroParcialEfectivo")]
        public ResponseBussiness<OperationResponse> RetiroParcialEfectivo(RetiroParcialEfectivo retiroParcialEfectivo)
        {
            new SecurityBusiness().ValidarPermisos("retiroParcialEfectivo", "E");
            TokenDto token = new TokenService().Get();
            ResponseBussiness<OperationResponse> response = new EgresosBusiness(token).RetiroParcialEfectivo(retiroParcialEfectivo);
            return response;
        }

        /// <summary>
        /// Servicio para ignorar Retiro
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/ignorarRetiro")]
        public ResponseBussiness<OperationResponse> IgnorarRetiro()
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<OperationResponse> response = new EgresosBusiness(token).IgnorarRetiroEfectivo();
            return response;
        }

        /// <summary>
        /// Función para hacer egresos de efectivo
        /// </summary>
        /// <param name="egreso">Objeto que representa el egreso de efectivo</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/egreso")]
        public ResponseBussiness<OperationResponse> Egreso(Egreso egreso)
        {
            new SecurityBusiness().ValidarPermisos("egreso", "E");
            TokenDto token = new TokenService().Get();
            ResponseBussiness<OperationResponse> response = new EgresosBusiness(token).ProcesarEgreso(egreso);
            return response;
        }

    }
}
