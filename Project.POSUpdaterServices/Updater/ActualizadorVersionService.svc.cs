using Milano.BackEnd.Business.Actualizador;
using Milano.BackEnd.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace Project.POSUpdaterServices.Updater
{

    /// <summary>
    /// Servicio que realiza operaciones referentes a la actuailzación del POS
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ActualizadorVersionService
    {

        /// <summary>
        /// PASO 1: Método usado por CAJA o BACKOFFICE localmente para comprobar la versión actual de software que tienen aplicada y las actualizaciones pendientes
        /// </summary>   
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/local/comprobarVersionSoftwareActual")]
        public ResponseBussiness<ActualizacionSoftwareResponse> ComprobarVersionSoftwareActual()
        {
            // 900 => POS tiene últma versión [PASA]
            // 904 => POS no está configurado para actualizarse automáticamente desde Front [PASA]
            // 901 => No se permite hacer un downgrade [NO PASA]
            // 902 => POS esta obsoleto [NO PASA]
            // 903 => Se requiere hacer una actualización. Iniciar proceso de Upgrade [NO PASA]   
            // 905 => Ya se encuentra activo un proceso de actualización [NO PASA]
            // 906 [NO PASA]
            // 907 [NO PASA]
            return new ActualizadorBusiness().ComprobarVersionSoftwareActual(-1, 0);
        }

        /// <summary>
        /// PASO 2: Método cliente usado por CAJA o BACKOFFICE para lanzar un proceso de peticiones de actualización
        /// </summary>   
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/local/actualizarVersionSoftware")]
        public ResponseBussiness<ProcesoActualizacionSoftwareResponse> ActualizarVersionSoftwareLocal(InformacionVersionSoftware[] versionesSoftwarePendientes)
        {
            // 911 => Indica una petición exitosa            
            return new ActualizadorBusiness().ActualizarVersionSoftware(versionesSoftwarePendientes);
        }

        /// <summary>
        /// Método usado por CAJA o BACKOFFICE o EXTERNO para preguntar el estatus de una actualización en caso de existir alguna en proceso
        /// </summary>   
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/local/estatusActualizacionVersionSoftware")]
        public ResponseBussiness<EstatusActualizacionSoftwareResponse> EstatusActualizacionVersionSoftware()
        {
            return new ActualizadorBusiness().ObtenerEstatusProcesoActualizacionEnCurso(0);
        }

        /// <summary>
        /// Método cliente usado por SISTEMA EXTERNO bajo demanda para iniciar un proceso de actualización, actualiza a la versión solicitada IdVersion o actualiza a la versión más reciente si se omite
        /// </summary>   
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/local/actualizarVersionSoftware/{idMaximaVersionSolicitada}")]
        public ResponseBussiness<ProcesoActualizacionSoftwareResponse> ActualizarVersionSoftwareExterno(String idMaximaVersionSolicitada)
        {
            // 911 => Indica una petición exitosa
            return new ActualizadorBusiness().ActualizarVersionSoftware(idMaximaVersionSolicitada, 1);
        }

        /***
         * Estos métodos son usados por el VBS-BATCH
         * **/

        /// <summary>
        /// 
        /// </summary>   
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/local/estatusReinicioNavegador")]
        public String EstatusReinicioNavegador()
        {
            return new ActualizadorBusiness().ObtenerEstatusReinicioNavegador();
        }

        /// <summary>
        /// 
        /// </summary>   
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/local/informarReinicioNavegador")]
        public String InformarReinicioNavegador()
        {
            return new ActualizadorBusiness().ActualizarEstatusReinicioNavegador();
        }

        /// <summary>
        /// 
        /// </summary>   
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/local/estatusForzarReinicioNavegador")]
        public String EstatusForzarReinicioNavegador()
        {
            return new ActualizadorBusiness().EstatusForzarReinicioNavegador(0);
        }

    }
}
