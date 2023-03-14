using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Repository;
using Milano.BackEnd.Dto.Sales;
using Milano.BackEnd.Repository.Sales;
using Project.Services.Utils;

namespace Milano.BackEnd.Business
{

    /// <summary>
    /// Servicio para gestionar operaciones de descuentos por mercancía dañada y picos de mercancía
    /// </summary>
    public class DescuentoMercanciaDaniadaBusiness : BaseBusiness
    {

        DescuentoMercanciaDañadaRepository descuentoMercanciaDañadaRepository;
        ProxyDescuentoMercanciaDaniada.wsMercanciaDanadaSoapClient proxyMercanciaDaniada;
        ProxyDescuentoPicoMercancia.wsPicosMercanciaSoapClient proxyMercanciaPico;

        InfoService infoService1;
        InfoService infoService2;
        TokenDto token;

        /// <summary>
        /// Constructor por default
        /// </summary>
        public DescuentoMercanciaDaniadaBusiness(TokenDto token)
        {
            InformacionServiciosExternosRepository externosRepository = new InformacionServiciosExternosRepository();

            infoService1 = new InfoService();
            infoService2 = new InfoService();

            infoService1 = externosRepository.ObtenerInfoServicioExterno(18);
            infoService2 = externosRepository.ObtenerInfoServicioExterno(19);

            this.proxyMercanciaDaniada = new ProxyDescuentoMercanciaDaniada.wsMercanciaDanadaSoapClient();
            proxyMercanciaDaniada.Endpoint.Address = new System.ServiceModel.EndpointAddress(infoService1.UrlService);
            this.proxyMercanciaPico = new ProxyDescuentoPicoMercancia.wsPicosMercanciaSoapClient();
            proxyMercanciaPico.Endpoint.Address = new System.ServiceModel.EndpointAddress(infoService2.UrlService);
            this.descuentoMercanciaDañadaRepository = new DescuentoMercanciaDañadaRepository();
            this.token = token;
        }

        /// <summary>
        /// Metodo para aplicar descuentos
        /// </summary>
        public void ProcesarDescuentosExternosPicosMercancia(String folio)
        {
            DescuentoMercanciaDañada[] lineasConDescuento = descuentoMercanciaDañadaRepository.DescuentosMercancia(folio, this.token.CodeBox, this.token.CodeStore);
            foreach (var item in lineasConDescuento)
            {
                if (item.CodigoRazonDescuento == 9)
                {
                    this.VenderMercanciaDaniada(folio, item);
                }
                else if (item.CodigoRazonDescuento == 13)
                {
                    this.VenderMercanciaDaniadaPico(folio, item);
                }
            }
        }

        /// <summary>
        /// Consulta si el descuento es aplicable
        /// </summary>
        /// <param name="consulta">Objeto de peticion para realizar la consulta</param>
        /// <returns>Resultado de la operación</returns>
        public ResponseBussiness<DescuentoMercanciaDaniadaResponse> ConsultarDescuentoMercanciaDaniada(ConsultaDescuentoRequest consulta)
        {
            return tryCatch.SafeExecutor(() =>
            {
                DescuentoMercanciaDaniadaResponse response = new DescuentoMercanciaDaniadaResponse();                
                ProxyDescuentoMercanciaDaniada.MercanciaResponse responseServicio = this.proxyMercanciaDaniada.ConsultarSKU(this.token.CodeStore, this.token.CodeBox, this.token.CodeEmployee, consulta.Cantidad, consulta.Sku.ToString());                
                response.Error = responseServicio.sError;
                response.Mensaje = responseServicio.sMensaje;
                response.PorcentanjeDescuento = responseServicio.dPtjDescuento;
                response.ULSession = responseServicio.uLSesion.ToString();
                response.CodigoRazon = 9;
                return response;
            }
            );
        }

        /// <summary>
        /// Consulta si el descuento es aplicable
        /// </summary>
        /// <param name="consulta">Objeto de peticion para realizar la consulta</param>
        /// <returns>Resultado de la operación</returns>
        public ResponseBussiness<DescuentoMercanciaDaniadaResponse> ConsultarDescuentoMercanciaPico(ConsultaDescuentoRequest consulta)
        {
            return tryCatch.SafeExecutor(() =>
            {
                DescuentoMercanciaDaniadaResponse response = new DescuentoMercanciaDaniadaResponse();                
                ProxyDescuentoPicoMercancia.MercanciaResponse responseServicio = this.proxyMercanciaPico.ConsultarSKU(this.token.CodeStore, this.token.CodeBox, this.token.CodeEmployee, consulta.Cantidad, consulta.Sku.ToString());                
                response.Error = responseServicio.sError;
                response.Mensaje = responseServicio.sMensaje;
                response.PorcentanjeDescuento = responseServicio.dPtjDescuento;
                response.ULSession = responseServicio.uLSesion.ToString();
                response.CodigoRazon = 13;
                return response;
            }
            );
        }

        private void VenderMercanciaDaniada(String folioOperacion, DescuentoMercanciaDañada descuento)
        {
            try
            {                
                this.proxyMercanciaDaniada.VenderSKU(Convert.ToUInt64(descuento.Sesion), descuento.Cantidad, descuento.SKU.ToString(), descuento.Transaccion);                
                descuentoMercanciaDañadaRepository.RegistrarDescuentoMercanciaDaniada(folioOperacion, descuento.Sesion, descuento.SecuenciaDetalleVenta, "F");
            }
            catch (Exception exception)
            {
                // El estatus permanece en P
                TryCatchBusinessExecutor tryCatch = new TryCatchBusinessExecutor();
                tryCatch.AddErrorLog<OperationResponse>(exception.Message, exception.StackTrace, "Negocio", exception.ToString(), "Error al aplicar un descuento directo");
            }
        }

        private void VenderMercanciaDaniadaPico(String folioOperacion, DescuentoMercanciaDañada descuento)
        {
            try
            {                
                this.proxyMercanciaPico.VenderSKU(Convert.ToUInt64(descuento.Sesion), descuento.Cantidad, descuento.SKU.ToString(), descuento.Transaccion);                
                descuentoMercanciaDañadaRepository.RegistrarDescuentoMercanciaDaniada(folioOperacion, descuento.Sesion, descuento.SecuenciaDetalleVenta, "F");
            }
            catch (Exception exception)
            {
                // El estatus permanece en P
                TryCatchBusinessExecutor tryCatch = new TryCatchBusinessExecutor();
                tryCatch.AddErrorLog<OperationResponse>(exception.Message, exception.StackTrace, "Negocio", exception.ToString(), "Error al aplicar un descuento directo");
            }
        }

    }
}
