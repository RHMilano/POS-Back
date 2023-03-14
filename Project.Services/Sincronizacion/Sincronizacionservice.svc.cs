using Milano.BackEnd.Business.Sincronizacion;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.Sincronizacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Services;

namespace Project.Services.Sincronizacion
{

    /// <summary>
    /// Servicio que realiza operaciones referentes a la sincronización de datos
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Sincronizacionservice
    {

        /// <summary>
        /// Ejecutar proceso de Sincronización
        /// </summary>        
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/ejecutarProcesoSincronizacion")]
        public ResponseBussiness<ResultadoSincronizacion> EjecutarProcesoSincronizacion(SincronizacionRequest sincronizacionRequest)
        {
            try
            {
                return new SincronizacionBusiness().ProcesarPeticionSincronizacion(sincronizacionRequest);
            }
            catch (Exception e)
            {
                string cja = sincronizacionRequest.CodigoCajaOrigen.ToString();
                string tda = sincronizacionRequest.CodigoTiendaOrigen.ToString();
                new SincronizacionBusiness().ErrorWebService("EjecutarProcesoSincronizacion, tda: " + tda + " caja: " + cja, e.Message);
                throw e;
            }

            //return new SincronizacionBusiness().ProcesarPeticionSincronizacion(sincronizacionRequest);
        }

    }
}
