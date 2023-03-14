using Milano.BackEnd.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using Milano.BackEnd.Business;
using Milano.BackEnd.Dto.Reportes;
using Milano.BackEnd.Dto.InicioFinDia;
using Milano.BackEnd.Dto.Impresion;

namespace Project.Services.Reportes
{

    /// <summary>
    /// Servicio encargado de generar los reportes
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ReporteService
    {

        /// <summary>
        /// Reporte que devuelve las relaciones de caja
        /// </summary>        
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/obtenRelacionesCaja")]
        public ResponseBussiness<RelacionCaja[]> ReporteRelacionesCaja(ReporteRelacionCajaRequest reporteRelacionCajaRequest)
        {
            // TODO: Generar capas, lógica de negocio
            TokenDto token = new TokenService().Get();
            return new RelacionCaja[1] { new RelacionCaja() };
        }

        /// <summary>
        /// Reporte de ventas por departamento
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/ventasDepartamento")]
        public ResponseBussiness<ReporteVentaDepartamentoResponse[]> ReporteVentasDepartamento(ReporteVentaDepartamentoRequest request)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<ReporteVentaDepartamentoResponse[]> response = new ReporteBusiness(token).ReporteVentasPorDepartamento(request);
            return response;
        }

        /// <summary>
        /// Reporte de ventas por SKU
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/ventasSKU")]
        public ResponseBussiness<ReporteVentaSKUResponse[]> ReporteVentasSKU(ReporteVentaSKURequest request)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<ReporteVentaSKUResponse[]> response = new ReporteBusiness(token).ReporteVentasPorSKU(request);
            return response;
        }

        /// <summary>
        /// Reporte de devoluciones por SKU
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/devolucionesSKU")]
        public ResponseBussiness<ReporteDevolucionesSKUResponse[]> ReporteDevolucionesSKU(ReporteDevolucionesSKURequest request)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<ReporteDevolucionesSKUResponse[]> response = new ReporteBusiness(token).ReporteDevolucionesPorSKU(request);
            return response;
        }

        /// <summary>
        /// Reporte de ventas por Vendedor
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/ventasVendedor")]
        public ResponseBussiness<ReporteVentaVendedorResponse[]> ReporteVentasVendedor(ReporteVentaVendedorRequest request)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<ReporteVentaVendedorResponse[]> response = new ReporteBusiness(token).ReporteVentasPorVendedor(request);
            return response;
        }

        /// <summary>
        /// Reporte de ventas por Caja
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/ventasCaja")]
        public ResponseBussiness<ReporteVentaCajaResponse[]> ReporteVentasCaja(ReporteVentaCajaRequest request)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<ReporteVentaCajaResponse[]> response = new ReporteBusiness(token).ReporteVentasPorCaja(request);
            return response;
        }

        /// <summary>
        /// Reporte de ventas por Hora
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/ventasPorHora")]
        public ResponseBussiness<ReporteVentasPorHoraResponse[]> ReporteVentasPorHora(ReporteVentasPorHoraRequest request)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<ReporteVentasPorHoraResponse[]> response = new ReporteBusiness(token).ReporteVentasPorHora(request);
            return response;
        }

        /// <summary>
        /// Reporte de Apartados sin Detalle
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/apartadosSinDetalle")]
        public ResponseBussiness<ReporteApartadosSinDetalleResponse[]> ReporteApartadosSinDetalle(ReporteApartadosSinDetalleRequest request)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<ReporteApartadosSinDetalleResponse[]> response = new ReporteBusiness(token).ReporteApartadosSinDetalle(request);
            return response;
        }

        /// <summary>
        /// Reporte de Apartados con Detalle
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/apartadosDetalle")]
        public ResponseBussiness<ReporteApartadosDetalleResponse[]> ReporteApartadosDetalle(ReporteApartadosDetalleRequest request)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<ReporteApartadosDetalleResponse[]> response = new ReporteBusiness(token).ReporteApartadosDetalle(request);
            return response;
        }

        /// <summary>
        /// Reporte de Ingresos y Egresos
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/ingresosEgresos")]
        public ResponseBussiness<ReporteIngresosEgresosResponse[]> ReporteIngresosEgresos(ReporteIngresosEgresosRequest request)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<ReporteIngresosEgresosResponse[]> response = new ReporteBusiness(token).ReporteIngresosEgresos(request);
            return response;
        }

        /// <summary>
        /// Reporte de Relacion de Caja
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/relacionCaja")]
        public ResponseBussiness<RelacionCaja[]> ReporteRelacionCaja(ReporteRelacionCajaRequest request)
        {
            TokenDto token = new TokenService().Get();
            return new ReporteBusiness(token).ReporteRelacionCaja(request);
        }

    }
}
