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
using Milano.BackEnd.Dto.InicioFinDia;
using Milano.BackEnd.Business.InicioFinDia;

namespace Project.Services.InicioFinDia
{

    /// <summary>
    /// Servicio que realiza operaciones referentes a Inicio y Fin de Día
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class InicioFinDiaService
    {

        /// <summary>
        /// Función para generar el Inicio de Día
        /// </summary>        
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/iniciarDia")]
        public ResponseBussiness<InicioDiaResponse> InicioDeDia()
        {
            TokenDto token = new TokenService().Get();
            return new InicioFinDiaBusiness().RealizarInicioDia(token);
        }

        /// <summary>
        /// Función para generar el Fin de Día
        /// </summary>        
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/finalizarDia")]
        public ResponseBussiness<FinDiaResponse> FinDeDia()
        {
            //new SecurityBusiness().ValidarPermisos("xxx", "E");
            TokenDto token = new TokenService().Get();
            return new InicioFinDiaBusiness().RealizarFinDia(token);
        }

        /// <summary>
        /// Función para obtener el Cash Out
        /// </summary>        
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/obtenCashOut")]
        public ResponseBussiness<CompendioCashOut> ObtenCashOut()
        {
            TokenDto token = new TokenService().Get();
            return new InicioFinDiaBusiness().ObtenerCashOut(token);
        }

        /// <summary>
        /// Función para persistir el Cash Out
        /// </summary>        
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/actualizarCashOut")]
        public ResponseBussiness<RelacionCaja> ActualizarCashOut(CompendioCashOut compendioCashOut)
        {
            TokenDto token = new TokenService().Get();
            return new InicioFinDiaBusiness().PersistirCashOut(token, compendioCashOut);
        }

        /// <summary>
        /// Función para persistir la relación de caja
        /// </summary>        
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/actualizarRelacionCajaFinDia")]
        public ResponseBussiness<ValidacionOperacionResponse> ActualizarRelacionCajaFinDia(RelacionCaja relacionCaja)
        {
            TokenDto token = new TokenService().Get();
            return new InicioFinDiaBusiness().PersistirRelacionCajaFinDia(token, relacionCaja);
        }

        /// <summary>
        /// Función para invocar el servicio de sincronización sobre una caja
        /// </summary>        
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/ejecutarSincronizacionDatos")]
        public ResponseBussiness<ValidacionOperacionResponse> EjecutarSincronizacionDatos(InformacionCajaRequest informacionCajaRequest)
        {
            //new SecurityBusiness().ValidarPermisos("xxx", "E");
            TokenDto token = new TokenService().Get();
            return new InicioFinDiaBusiness().EjecutarSincronizacion(informacionCajaRequest);
        }

        /// <summary>
        /// Función para generar una lectura Z y un Logout de forma Offline sobre una caja
        /// </summary>        
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/ejecutarLecturaZLogoutOffline")]
        public ResponseBussiness<ValidacionOperacionResponse> GenerarLecturaZLogoutOffline(LecturaCaja lecturaCaja)
        {
            //new SecurityBusiness().ValidarPermisos("xxx", "E");
            TokenDto token = new TokenService().Get();
            return new InicioFinDiaBusiness().GenerarLecturaZOffline(token, lecturaCaja);
        }

        /// <summary>
        /// Servicio que realiza operaciones de autenticación en un Corte Z Offline
        /// </summary>        
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/validarLecturaZLogoutOffline")]
        public ResponseBussiness<ValidacionOperacionResponse> ValidarCorteOffline(AutenticacionOfflineRequest autenticacionOfflineRequest)
        {
            TokenDto token = new TokenService().Get();
            return new InicioFinDiaBusiness().ValidarCorteOffline(token, autenticacionOfflineRequest);
        }
    }
}
