using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Milano.BackEnd.Business.External
{
    /// <summary>
    /// 
    /// </summary>
    public class NotaCreditoBusiness : BaseBusiness
    {
        InfoService infoService = new InfoService();
        //ProxyNotaCredito.wsNotaCreditoSoapClient wsNotaCreditoSoapClient; // OCG Integracion de transferencias
        TokenDto token;
        InformacionServiciosExternosRepository externosRepository;
        ProxyNotaCredito.wsNotaCreditoSoapClient proxy;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenDto"></param>
        public NotaCreditoBusiness(TokenDto tokenDto)
        {

            this.token = tokenDto;
            externosRepository = new InformacionServiciosExternosRepository();
            infoService = externosRepository.ObtenerInfoServicioExterno(20);
            proxy = new ProxyNotaCredito.wsNotaCreditoSoapClient();
            proxy.Endpoint.Address = new System.ServiceModel.EndpointAddress(infoService.UrlService);
        }

        /// <summary>
        /// Cobro con tarjeta de regalo
        /// </summary>
        /// <param name="codigoCajero">Código del cajero</param>
        /// <param name="folioTarjeta">Folio de la nota credito</param>
        /// <param name="codigoTransaccion">Transaccion de la venta</param>
        /// <param name="folioVenta">Folio de venta</param>
        /// <param name="codigoTipoTrxCab"></param>
        /// <param name="montoPagado">Monto a pagado</param>
        /// <returns></returns>
        public OperationResponse Cobro(int codigoCajero, string folioTarjeta, int codigoTransaccion, string folioVenta, string codigoTipoTrxCab, decimal montoPagado)
        {
            OperationResponse operation = new OperationResponse();
            ProxyNotaCredito.Respuesta respuesta = new ProxyNotaCredito.Respuesta();
            try
            {
                respuesta = proxy.RealizarVenta(this.token.CodeStore, this.token.CodeBox, codigoTipoTrxCab, folioVenta, this.token.CodeEmployee, folioTarjeta, codigoTransaccion, montoPagado);
                if (respuesta.sError == "")
                {
                    operation.CodeNumber = respuesta.sEstatus;
                    operation.CodeDescription = respuesta.sMensaje + ". Nuevo saldo: " + respuesta.dSaldo;
                }
                else
                {
                    operation.CodeNumber = "0";
                    operation.CodeDescription = respuesta.sError;
                }
            }
            catch (Exception ex)
            {
                operation.CodeNumber = "0";
                operation.CodeDescription = ex.Message;
            }
            return operation;
        }

        /// <summary>
        /// OCG Validar que la
        /// </summary>
        /// <param name="tienda"></param>
        /// <param name="importeVenta"></param>
        /// <param name="referencia"></param>
        /// <returns></returns>
        public ValidaTransferenciaResponse ValidarTransferencia(int tienda,  decimal importeVenta, string referencia)
        {

            ProxyNotaCredito.VerificacionTransferenciaResponse verificaTransferencia = proxy.VerificaTransferenciaBancaria(tienda, importeVenta, referencia);
            ValidaTransferenciaResponse respuesta = new ValidaTransferenciaResponse();

            //ProxyMayoristas.InfoVale infoVale = wsVentaMayoristaSoapClient.ConsultarVale(this.token.CodeStore, this.token.CodeBox, this.token.CodeEmployee, codigoVale);
            //InfoValeResponse infoValeResponse = new InfoValeResponse();

            respuesta.Error = verificaTransferencia.sError;
            respuesta.Mensaje = verificaTransferencia.sMensaje;

            //if (respuesta.Error == "0")
            //{
            //   respuesta.Error
            //}
            return respuesta;
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="codigoTienda"></param>
       /// <param name="importeVenta"></param>
       /// <param name="codigoEmpleado"></param>
       /// <param name="folioVenta"></param>
       /// <returns></returns>
        public ValidaTransferenciaResponse AplicaPagoTransferencia(int codigoTienda, decimal importeVenta, int codigoEmpleado, string folioVenta, string referencia)
        {
            ProxyNotaCredito.VerificacionTransferenciaResponse verificaTransferencia = proxy.AplicaTransferenciaBancaria(codigoTienda, importeVenta, codigoEmpleado, folioVenta, referencia);
            ValidaTransferenciaResponse respuesta = new ValidaTransferenciaResponse();

            //ProxyMayoristas.InfoVale infoVale = wsVentaMayoristaSoapClient.ConsultarVale(this.token.CodeStore, this.token.CodeBox, this.token.CodeEmployee, codigoVale);
            //InfoValeResponse infoValeResponse = new InfoValeResponse();

            respuesta.Error = verificaTransferencia.sError;
            respuesta.Mensaje = verificaTransferencia.sMensaje;

            //if (respuesta.Error == "0")
            //{
            //   respuesta.Error
            //}
            return respuesta;
        }

    }
}
