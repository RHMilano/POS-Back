using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Dto.Sales;
using Milano.BackEnd.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Milano.BackEnd.Business.Sales
{
    /// <summary>
    /// Autoriza Cancelacion de Transaccion
    /// </summary>
    public class AutorizaCancelacionTransaccionBusiness : BaseBusiness
    {
        TokenDto token;
        InfoService infoService;
        InformacionServiciosExternosRepository externosRepository;
        ProxyAutorizaCancelacion.wsAutorizacionCancelacionTrxSoapClient proxyAutorizacion;

        /// <summary>
        /// Constructor por Default
        /// </summary>
        /// <param name="tokenDto"></param>
        public AutorizaCancelacionTransaccionBusiness(TokenDto tokenDto)
        {
            try
            {
                this.token = tokenDto;
                externosRepository = new InformacionServiciosExternosRepository();
                infoService = externosRepository.ObtenerInfoServicioExterno(26);
                proxyAutorizacion = new ProxyAutorizaCancelacion.wsAutorizacionCancelacionTrxSoapClient();
                proxyAutorizacion.Endpoint.Address = new System.ServiceModel.EndpointAddress(infoService.UrlService);
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        /// <summary>
        /// Regresa respuesta de autorizacion
        /// </summary>
        /// <returns></returns>
        public AutorizaCancelacionTransaccionResponse AutorizaCancelacionTransaccion(AutorizaCancelacionTransaccionRequest datos)
        {
            return tryCatch.SafeExecutor(() => {
                ProxyAutorizaCancelacion.Respuesta respuesta = new ProxyAutorizaCancelacion.Respuesta();
                respuesta = proxyAutorizacion.SolicitarCancelacionTrx(token.CodeStore, token.CodeBox, datos.codigoTipoTrxCab, datos.folioVenta, datos.transaccion, datos.nombreCajero, datos.codigoCajeroAutorizo, datos.totalTransaccion, datos.totalPiezasPositivas, datos.totalPiezas, datos.totalPiezasPositivas, datos.codigoRazonMMS);

                return new AutorizaCancelacionTransaccionResponse()
                {
                    hayError = respuesta.hayError,
                    autorizado = respuesta.autorizado,
                    sMensaje = respuesta.sMensaje
                };
            });
        }
    }
}
