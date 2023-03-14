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
namespace Project.Services.General
{

    /// <summary>
    /// Servicio que realiza operaciones referentes a Lectura X y Z
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class LecturaService
    {

        /// <summary>
        /// Función para recuperar los totales por formas de pago
        /// </summary>        
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/obtenerTotalesFormaPago")]
        public ResponseBussiness<LecturaTotalDetalleFormaPago[]> ObtenerTotalesPorFormaPago()
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<LecturaTotalDetalleFormaPago[]> response = new LecturaBusiness(token).ObtenerTotalesPorFormaPago();
            return response;
        }

        /// <summary>
        /// Función para recuperar los totales por formas de pago de una caja en particular
        /// </summary>        
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/obtenerTotalesFormaPagoCaja/{caja}")]
        public ResponseBussiness<LecturaTotalDetalleFormaPago[]> ObtenerTotalesPorFormaPagoCaja(String caja)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<LecturaTotalDetalleFormaPago[]> response = new LecturaBusiness(token).ObtenerTotalesPorFormaPago(Convert.ToInt32(caja));
            return response;
        }

        /// <summary>
        /// Función para recuperar las diferentes denominaciones
        /// </summary>        
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/obtenerDenominaciones")]
        public ResponseBussiness<Denominacion[]> ObtenerDenominaciones()
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<Denominacion[]> response = new LecturaBusiness(token).ObtenerDenominaciones();
            return response;
        }

        /// <summary>
        /// Función para guardar la información de Lectura X
        /// </summary>        
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/ejecutarArqueoCaja")]
        public ResponseBussiness<OperationResponse> EjecutarProcedimientoArqueoCaja(LecturaCaja lecturaCaja)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<OperationResponse> response = new LecturaBusiness(token).LecturaX(lecturaCaja);
            return response;
        }

        /// <summary>
        /// Función para guardar la información de Lectura Z
        /// </summary>        
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/ejecutarCorteCaja")]
        public ResponseBussiness<OperationResponse> EjecutarProcedimientoCorteCaja(LecturaCaja lecturaCaja)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<OperationResponse> response = new LecturaBusiness(token).LecturaZ(lecturaCaja);
            return response;
        }

        /// <summary>
        /// Función para validar el Business Date del POS
        /// </summary>        
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/validarBusinessDate")]
        public ResponseBussiness<OperationResponse> ValidarBusinessDate()
        {
            // TODO Implementación
            TokenDto token = new TokenService().Get();
            return new OperationResponse();
        }

    }
}
