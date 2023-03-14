using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Repository;
using Milano.BackEnd.Repository.General;
using Milano.BackEnd.Dto.General;

namespace Milano.BackEnd.Business
{

    /// <summary>
    /// Admnistracion de tarjetas de regalo
    /// </summary>
    public class TarjetaRegalosBusiness : BaseBusiness
    {
        ProxyTarjetasRegalo.wsTarjetasRegaloSoapClient proxy;
        InformacionServiciosExternosRepository externosRepository;
        TarjetasRegaloRepository repository;
        InfoService inforService;
        TokenDto token;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token"></param>
        public TarjetaRegalosBusiness(TokenDto token)
        {
            inforService = new InfoService();
            externosRepository = new InformacionServiciosExternosRepository();
            inforService = externosRepository.ObtenerInfoServicioExterno(16);
            proxy = new ProxyTarjetasRegalo.wsTarjetasRegaloSoapClient();
            proxy.Endpoint.Address = new System.ServiceModel.EndpointAddress(inforService.UrlService);
            repository = new TarjetasRegaloRepository();
            this.token = token;
        }

        /// <summary>
        /// Busqueda de tarjetas de regalo dentro de la lineas ticket por folio de venta
        /// </summary>
        /// <param name="folioVenta"></param>
        /// <returns></returns>
        public ResponseBussiness<BusquedaTarjetasRegalo[]> BusquedaTarjetaLineaTicketPorFolioVenta(string folioVenta)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return this.repository.BusquedaTarjetaLineaTicketPorFolioVenta(folioVenta);
            });
        }

        /// <summary>
        /// Busqueda de tarjetas
        /// </summary>
        /// <param name="folioTarjeta"></param>
        /// <returns></returns>
        public ResponseBussiness<ProductsResponse[]> Busqueda(string folioTarjeta)
        {
            return tryCatch.SafeExecutor(() =>
            {
                ProxyTarjetasRegalo.Respuesta respuesta = new ProxyTarjetasRegalo.Respuesta();                
                respuesta = proxy.ConsultaTarjeta(this.token.CodeStore, this.token.CodeBox, folioTarjeta);                
                ConfigGeneralesCajaTiendaRepository repositoryConfiguracion = new ConfigGeneralesCajaTiendaRepository();
                Dto.General.ConfigGeneralesCajaTiendaResponse config = repositoryConfiguracion.GetConfigSinBotonera(this.token.CodeBox, this.token.CodeStore, 0);
                List<ProductsResponse> list = new List<ProductsResponse>();
                if (respuesta.sError == "")
                {
                    ProductsResponse product = new ProductsResponse();
                    product.Articulo.Sku = config.SkuTarjetaRegalo;
                    product.Articulo.PrecioConImpuestos = respuesta.dSaldo;
                    product.Articulo.InformacionTarjetaRegalo.Estatus = respuesta.sEstatus;
                    product.Articulo.InformacionTarjetaRegalo.Descripcion = respuesta.sMensaje;
                    product.Articulo.InformacionTarjetaRegalo.FolioTarjeta = int.Parse(folioTarjeta);
                    list.Add(product);
                }
                return list.ToArray();
            });
        }


        /// <summary>
        /// Activacion de tarjeta
        /// </summary>
        /// <param name="codigoCajero">Código del cajero</param>

        /// <param name="folioTarjeta"></param>
        /// <param name="folioVenta"></param>
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> ActivarTarjeta(int codigoCajero, string folioTarjeta, string folioVenta)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return this.GuardarInformacionTarjeta(codigoCajero, folioTarjeta, folioVenta);
            });

        }

        private OperationResponse GuardarInformacionTarjeta(int codigoCajero, string folioTarjeta, string folioVenta)
        {
            ProxyTarjetasRegalo.Respuesta respuesta = new ProxyTarjetasRegalo.Respuesta();
            OperationResponse operation = new OperationResponse();
            operation = this.repository.GuardarActivacionTarjeta(int.Parse(folioTarjeta), folioVenta, "I", this.token.CodeStore, this.token.CodeBox, "");
            int transaccion = Convert.ToInt32(operation.CodeDescription);
            try
            {
                DateTime fechaVenta = DateTime.Now;
                respuesta = proxy.ActivaTarjeta(this.token.CodeStore, this.token.CodeBox, codigoCajero, fechaVenta, folioTarjeta, transaccion);
                if (respuesta.sError == "")
                    operation = this.repository.GuardarActivacionTarjeta(int.Parse(folioTarjeta), folioVenta, "A", this.token.CodeStore, this.token.CodeBox, respuesta.sMensaje);
                else
                {
                    operation = this.repository.GuardarActivacionTarjeta(int.Parse(folioTarjeta), folioVenta, "E", this.token.CodeStore, this.token.CodeBox, respuesta.sError);
                    operation.CodeDescription = respuesta.sError;
                    operation.CodeNumber = "0";
                }
            }
            catch (Exception ex)
            {
                operation = this.repository.GuardarActivacionTarjeta(int.Parse(folioTarjeta), folioVenta, "P", this.token.CodeStore, this.token.CodeBox, ex.Message);
            }
            return operation;
        }

        /// <summary>
        /// Cobro con tarjeta de regalo
        /// </summary>
        /// <param name="codigoCajero">Código del cajero</param>
        /// <param name="folioTarjeta">Folio de la tarjeta</param>
        /// <param name="codigoTransaccion">Transaccion de la venta</param>
        /// <param name="folioVenta">Folio de venta</param>
        /// <param name="montoPagado">Monto a pagado</param>
        /// <returns></returns>
        public OperationResponse Cobro(int codigoCajero, string folioTarjeta, int codigoTransaccion, string folioVenta, decimal montoPagado)
        {
            OperationResponse operation = new OperationResponse();
            ProxyTarjetasRegalo.Respuesta respuesta = new ProxyTarjetasRegalo.Respuesta();
            try
            {
                DateTime fechaVenta = DateTime.Now;
                respuesta = proxy.RealizarVenta(this.token.CodeStore, this.token.CodeBox, codigoCajero, fechaVenta, folioVenta, codigoTransaccion, folioTarjeta, montoPagado);
                if (respuesta.sError == "")
                {
                    operation.CodeNumber = respuesta.sEstatus;
                    operation.CodeDescription = respuesta.sMensaje;
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

    }
}