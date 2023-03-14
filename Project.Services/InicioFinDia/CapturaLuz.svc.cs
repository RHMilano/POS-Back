using Milano.BackEnd.Business.InicioFinDia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace Project.Services.InicioFinDia
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class CapturaLuz
    {
        /// <summary>
        /// Servicio para captura y lectura de luz
        /// </summary>
        /// <param name="pantallaLuz"></param>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/capturarLecturaLuz")]
        public string CapturarLecturaLuz(int pantallaLuz)
        {
            //Se manejara la pantalla de luz y se regresara 
            //la lectura de luz capturada
            return new LecturaLuzBusiness().CapturaLecturaLuz(pantallaLuz);
        }

        /// <summary>
        /// Servicio para obtener la lectura de luz
        /// </summary>
        /// <param name="webServiceResponse"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/obtenerLecturaLuz")]
        public string ObtenerLecturaLuz()
        {
            // De acuerdo a lo que obtengamos como respuesta del web service
            // si la bandera de luz esta inhabilitada se debera mandar a llamar con ceros
            // en el monto de captura sin una pantalla para el usuario
            return new LecturaLuzBusiness().ObtenerLecturaLuz();
        }
    }
}
