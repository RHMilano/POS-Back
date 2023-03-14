using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milano.BackEnd.Dto;
using System.Collections;
using Milano.BackEnd.Repository;
using Milano.BackEnd.Dto.Sales;
using System.Transactions;
using Milano.BackEnd.Repository.General;
using Milano.BackEnd.Dto.FormasPago;
using Milano.BackEnd.Business;
using Milano.BackEnd.Business.External;
using Milano.BackEnd.Business.Sales;
using Milano.BackEnd.Business.LogMonitor;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Dto.BBVAv2;

namespace Milano.BackEnd.Business
{
    /// <summary>
    /// Servicio para gestionar operaciones de cobros y cancelaciones de cobros
    /// </summary>
    public class PaymentProcessingBusiness : BaseBusiness
    {
        addEvent s = new addEvent();
        /// <summary>
        /// Repositorio para el procesamiento de pagos
        /// </summary>
        protected PaymentProcessingRepository repository;
        protected FuncionesGeneralesCajaRepository funcionesRepository;

        /// <summary>
        /// Atributo del token usuario
        /// </summary>
        protected TokenDto token;

        /// <summary>
        /// Constructor por default
        /// xxxxxxxxxxxxx
        /// </summary>
        public PaymentProcessingBusiness(TokenDto token)
        {
            this.repository = new PaymentProcessingRepository();
            this.funcionesRepository = new FuncionesGeneralesCajaRepository();
            this.token = token;
        }

        ///// <summary>
        ///// Metodo para procesar tarjetas bancarias VISA/MASTERCARD
        ///// </summary>	 
        ///// <param name="request">Movimiento tarjeta bancaria</param>
        ///// <returns>Respuesta de la operación</returns>
        //public ResponseBussiness<PagoBancarioResponse> ProcesarTarjetaBancariaVisaMaster(
        //        ProcesarMovimientoTarjetaBancariaRequest pmtb_Request)
        //{
        //    return tryCatch.SafeExecutor(() =>
        //    {
        //        PagoBancarioResponse respuesta = new PagoBancarioResponse();
        //        using (TransactionScope scope = new TransactionScope())
        //        {
        //            TarjetaBancariaBusiness tarjeta = new TarjetaBancariaBusiness(this.token);
        //            var funcionesCaja = funcionesRepository.GetFunctions(token.CodeBox, token.CodeStore);

        //            if (funcionesCaja != null)
        //            {
        //                // Se procesan las promociones por venta
        //                foreach (var item in pmtb_Request.DescuentosPromocionalesPorVentaAplicados.DescuentoPromocionesAplicados)
        //                {
        //                    OperationResponse response = new OperationResponse();
        //                    response = repository.PersistirPromocionesVenta(pmtb_Request.Venta.FolioOperacionAsociada, token.CodeStore, token.CodeBox, item.ImporteDescuento
        //                                                            , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);

        //                }
        //                // Se procesan las promociones por línea de venta
        //                foreach (var item in pmtb_Request.DescuentosPromocionalesPorLineaAplicados.DescuentoPromocionesAplicados)
        //                {
        //                    OperationResponse response = new OperationResponse();
        //                    response = repository.PersistirPromocionesLineaVenta(pmtb_Request.Venta.FolioOperacionAsociada, token.CodeStore, token.CodeBox, item.Secuencia, item.ImporteDescuento
        //                                                            , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);
        //                }

        //                // OCG: Proceso modificado para no invocar a la versión 1.5 de la pinpad
        //               tarjeta.CobroVisaMasterCard(pmtb_Request,token.CodeStore, token.CodeBox);

        //                respuesta.SePuedePagarConPuntos = pmtb_Request.saleRequestBBVA.PayPoints;
        //                respuesta.SePuedeRetirar = false; // Actualmente no hay retiros
        //                respuesta.TipoTarjeta = pmtb_Request.card.Producto;
        //                respuesta.CardNumber = pmtb_Request.card.Pan;
        //                respuesta.Authorization = "";
        //                respuesta.CodeDescription = "";

        //                // OCG: Se fija esta respuesta por que ya se ejecuto la operación                        respuesta.CodeNumber = 0; 
        //                // de forma exitosa en le primera petición
        //                respuesta.CodeNumber = 0;

        //                //if (!respuesta.SePuedePagarConPuntos && !respuesta.SePuedeRetirar)
        //                //{
        //                    if (respuesta.CodeNumber == 0)
        //                    {
        //                        //pmtb_Request.Venta.CodigoFormaPagoImporte = respuesta.TipoTarjeta;
        //                        var repuestaLocal = repository.ProcesarMovimientoTarjetaBancariaVenta(token.CodeStore, token.CodeBox, token.CodeEmployee, respuesta.Authorization, respuesta.CardNumber, pmtb_Request);
        //                        scope.Complete();
        //                    }
        //                //}
        //            }
        //            else
        //            {
        //                respuesta.CodeNumber = 103;
        //                respuesta.CodeDescription = "No existe funciones asociadas a la caja, revise las funciones registradas de la caja";
        //            }
        //        }
        //        return respuesta;
        //    });
        //}

        /// <summary>
        /// Metodo para procesar tarjetas bancarias VISA/MASTERCARD
        /// </summary>	 
        /// <param name="request">Movimiento tarjeta bancaria</param>
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<PagoBancarioResponse> ProcesarTarjetaBancariaVisaMaster(ProcesarMovimientoTarjetaBancariaRequest request)
        {

            return tryCatch.SafeExecutor(() =>
            {
                PagoBancarioResponse respuesta = new PagoBancarioResponse();
                using (TransactionScope scope = new TransactionScope())
                {
                    TarjetaBancariaBusiness tarjeta = new TarjetaBancariaBusiness(this.token);
                    var funcionesCaja = funcionesRepository.GetFunctions(token.CodeBox, token.CodeStore);

                    if (funcionesCaja != null)
                    {

                        // Se procesan las promociones por venta
                        foreach (var item in request.DescuentosPromocionalesPorVentaAplicados.DescuentoPromocionesAplicados)
                        {
                            OperationResponse response = new OperationResponse();
                            response = repository.PersistirPromocionesVenta(request.Venta.FolioOperacionAsociada, token.CodeStore, token.CodeBox, item.ImporteDescuento
                                                                    , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);

                        }
                        // Se procesan las promociones por línea de venta
                        foreach (var item in request.DescuentosPromocionalesPorLineaAplicados.DescuentoPromocionesAplicados)
                        {
                            OperationResponse response = new OperationResponse();
                            response = repository.PersistirPromocionesLineaVenta(request.Venta.FolioOperacionAsociada, token.CodeStore, token.CodeBox, item.Secuencia, item.ImporteDescuento
                                                                    , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);
                        }

                        respuesta = tarjeta.CobroVisaMasterCard(funcionesCaja.UrlLecturaBancaria, token.CodeStore, request.Venta.FolioOperacionAsociada, request.Venta.MesesFinanciamiento, request.Venta.MesesParcialidades, request.Venta.CodigoPromocion, request.Venta.ImporteVentaTotal, request.Venta.SecuenciaFormaPagoImporte);

                        if (!respuesta.SePuedePagarConPuntos && !respuesta.SePuedeRetirar)
                        {
                            if (respuesta.CodeNumber == 0)
                            {
                                request.Venta.CodigoFormaPagoImporte = respuesta.TipoTarjeta;
                                var repuestaLocal = repository.ProcesarMovimientoTarjetaBancariaVenta(token.CodeStore, token.CodeBox, token.CodeEmployee, respuesta.Authorization, respuesta.CardNumber, request);
                                scope.Complete();
                            }
                        }
                    }
                    else
                    {
                        respuesta.CodeNumber = 103;
                        respuesta.CodeDescription = "No existe funciones asociadas a la caja, revise las funciones registradas de la caja";
                    }
                }
                return respuesta;
            });
        }






        /// <summary>
        /// Metodo para procesar tarjetas bancarias VISA/MASTERCARD
        /// </summary>	 
        /// <param name="monto">Monto total de la venta</param>
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<ConfiguracionMSI> obtenerConfiguiracionMsi()
        {
            return tryCatch.SafeExecutor(() =>
            {
                ConfiguracionMSI configuracionMSI = new ConfiguracionMSI();
                 
                using (TransactionScope scope = new TransactionScope())
                {
                    configuracionMSI = repository.ObtenerConfiguracionMSI();
                    scope.Complete();
                }

                return configuracionMSI;
            });
        }












        /// <summary>
        /// Metodo para procesar tarjetas bancarias AMERICAN EXPRESS
        /// </summary>	 
        /// <param name="request">Movimiento tarjeta bancaria</param>
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<PagoBancarioResponse> ProcesarTarjetaBancariaAmericanExpress(ProcesarMovimientoTarjetaBancariaRequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                PagoBancarioResponse respuesta = new PagoBancarioResponse();
                using (TransactionScope scope = new TransactionScope())
                {
                    TarjetaBancariaBusiness tarjeta = new TarjetaBancariaBusiness(this.token);
                    var funcionesCaja = funcionesRepository.GetFunctions(token.CodeBox, token.CodeStore);

                    if (funcionesCaja != null)
                    {
                        // Se procesan las promociones por venta
                        foreach (var item in request.DescuentosPromocionalesPorVentaAplicados.DescuentoPromocionesAplicados)
                        {
                            OperationResponse response = new OperationResponse();
                            response = repository.PersistirPromocionesVenta(request.Venta.FolioOperacionAsociada, token.CodeStore, token.CodeBox, item.ImporteDescuento
                                                                    , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);

                        }
                        // Se procesan las promociones por línea de venta
                        foreach (var item in request.DescuentosPromocionalesPorLineaAplicados.DescuentoPromocionesAplicados)
                        {
                            OperationResponse response = new OperationResponse();
                            response = repository.PersistirPromocionesLineaVenta(request.Venta.FolioOperacionAsociada, token.CodeStore, token.CodeBox, item.Secuencia, item.ImporteDescuento
                                                                    , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);
                        }
                        respuesta = tarjeta.CobroAmericanExpress(funcionesCaja.UrlLecturaBancaria, token.CodeStore, request.Venta.FolioOperacionAsociada, request.Venta.MesesFinanciamiento, request.Venta.MesesParcialidades, request.Venta.CodigoPromocion, request.Venta.ImporteVentaTotal, request.Venta.SecuenciaFormaPagoImporte);

                        if (respuesta.CodeNumber == 0)
                        {
                            var movimientoVenta = repository.ProcesarMovimientoTarjetaBancariaVenta(token.CodeStore, token.CodeBox, token.CodeEmployee, respuesta.Authorization, respuesta.CardNumber, request);
                            scope.Complete();
                        }
                    }
                    else
                    {
                        respuesta.CodeNumber = 103;
                        respuesta.CodeDescription = "No existe funciones asociadas a la caja, revise las funciones registradas de la caja";
                    }
                }

                return respuesta;
            });
        }

        /// <summary>
        /// Metodo para procesar tarjetas bancarias con pago y retiro de efectivo
        /// </summary>	 
        /// <param name="request">Movimiento tarjeta bancaria</param>
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<PagoBancarioResponse> ProcesarTarjetaBancariaCashBack(ProcesarMovimientoTarjetaBancariaRequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                PagoBancarioResponse respuesta = new PagoBancarioResponse();
                using (TransactionScope scope = new TransactionScope())
                {
                    TarjetaBancariaBusiness tarjeta = new TarjetaBancariaBusiness(this.token);
                    var funcionesCaja = funcionesRepository.GetFunctions(token.CodeBox, token.CodeStore);

                    if (funcionesCaja != null)
                    {
                        // Se procesan las promociones por venta
                        foreach (var item in request.DescuentosPromocionalesPorVentaAplicados.DescuentoPromocionesAplicados)
                        {
                            OperationResponse response = new OperationResponse();
                            response = repository.PersistirPromocionesVenta(request.Venta.FolioOperacionAsociada, token.CodeStore, token.CodeBox, item.ImporteDescuento
                                                                    , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);

                        }
                        // Se procesan las promociones por línea de venta
                        foreach (var item in request.DescuentosPromocionalesPorLineaAplicados.DescuentoPromocionesAplicados)
                        {
                            OperationResponse response = new OperationResponse();
                            response = repository.PersistirPromocionesLineaVenta(request.Venta.FolioOperacionAsociada, token.CodeStore, token.CodeBox, item.Secuencia, item.ImporteDescuento
                                                                    , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);
                        }
                        if (request.Retiro.Retirar)
                        {
                            respuesta = tarjeta.CobroConCashBack(funcionesCaja.UrlLecturaBancaria, token.CodeStore, request.Venta.FolioOperacionAsociada, request.Venta.MesesFinanciamiento, request.Venta.MesesParcialidades, request.Venta.CodigoPromocion, request.Venta.ImporteVentaTotal, request.Retiro.ImporteCashBack, request.Venta.SecuenciaFormaPagoImporte);

                            if (respuesta.CodeNumber == 0)
                            {
                                request.Venta.CodigoFormaPagoImporte = respuesta.TipoTarjeta;
                                var movimientoVenta = repository.ProcesarMovimientoTarjetaBancariaVenta(token.CodeStore, token.CodeBox, token.CodeEmployee, respuesta.Authorization, respuesta.CardNumber, request);
                                var movimientoCashBack = repository.ProcesarMovimientoTarjetaBancariaRetiro(token.CodeStore, token.CodeBox, token.CodeEmployee, respuesta.Authorization, respuesta.CardNumber, request);
                                scope.Complete();
                            }
                        }
                        else
                        {
                            respuesta = tarjeta.CobroConCashBack(funcionesCaja.UrlLecturaBancaria, token.CodeStore, request.Venta.FolioOperacionAsociada, request.Venta.MesesFinanciamiento, request.Venta.MesesParcialidades, request.Venta.CodigoPromocion, request.Venta.ImporteVentaTotal, 0, request.Venta.SecuenciaFormaPagoImporte);
                            if (respuesta.CodeNumber == 0)
                            {
                                request.Venta.CodigoFormaPagoImporte = respuesta.TipoTarjeta;
                                var movimientoVenta = repository.ProcesarMovimientoTarjetaBancariaVenta(token.CodeStore, token.CodeBox, token.CodeEmployee, respuesta.Authorization, respuesta.CardNumber, request);
                                scope.Complete();
                            }
                        }
                    }
                    else
                    {
                        respuesta.CodeNumber = 103;
                        respuesta.CodeDescription = "No existe funciones asociadas a la caja, revise las funciones registradas de la caja";
                    }
                }

                return respuesta;
            });
        }

        /// <summary>
        /// Metodo para procesar tarjetas bancarias con puntos
        /// </summary>	 
        /// <param name="request">Movimiento tarjeta bancaria</param>
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<PagoBancarioResponse> ProcesarTarjetaBancariaConPuntos(ProcesarMovimientoTarjetaBancariaRequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                PagoBancarioResponse respuesta = new PagoBancarioResponse();
                using (TransactionScope scope = new TransactionScope())
                {
                    TarjetaBancariaBusiness tarjeta = new TarjetaBancariaBusiness(this.token);
                    var funcionesCaja = funcionesRepository.GetFunctions(token.CodeBox, token.CodeStore);

                    if (funcionesCaja != null)
                    {
                        // Se procesan las promociones por venta
                        foreach (var item in request.DescuentosPromocionalesPorVentaAplicados.DescuentoPromocionesAplicados)
                        {
                            OperationResponse response = new OperationResponse();
                            response = repository.PersistirPromocionesVenta(request.Venta.FolioOperacionAsociada, token.CodeStore, token.CodeBox, item.ImporteDescuento
                                                                     , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);

                        }
                        // Se procesan las promociones por línea de venta
                        foreach (var item in request.DescuentosPromocionalesPorLineaAplicados.DescuentoPromocionesAplicados)
                        {
                            OperationResponse response = new OperationResponse();
                            response = repository.PersistirPromocionesLineaVenta(request.Venta.FolioOperacionAsociada, token.CodeStore, token.CodeBox, item.Secuencia, item.ImporteDescuento
                                                                    , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);
                        }
                        respuesta = tarjeta.CobroConPuntos(funcionesCaja.UrlLecturaBancaria, token.CodeStore, request.Venta.FolioOperacionAsociada, request.Venta.ImporteVentaTotal, request.Puntos.PagarConPuntos, request.Venta.SecuenciaFormaPagoImporte);
                        if (respuesta.CodeNumber == 0)
                        {
                            request.Venta.CodigoFormaPagoImporte = respuesta.TipoTarjeta;
                            request.Puntos.CodigoFormaPagoImporte = respuesta.TipoTarjeta;
                            var movimientoVenta = repository.ProcesarMovimientoTarjetaBancariaPuntos(token.CodeStore, token.CodeBox, token.CodeEmployee, respuesta.Authorization, respuesta.CardNumber, request);
                            scope.Complete();
                        }
                    }
                    else
                    {
                        respuesta.CodeNumber = 103;
                        respuesta.CodeDescription = "No existe funciones asociadas a la caja, revise las funciones registradas de la caja";
                    }
                }

                return respuesta;
            });
        }

        /// <summary>
        /// Metodo para procesar tarjetas bancarias con retiro de efectivo
        /// </summary>	 
        /// <param name="request">Movimiento tarjeta bancaria</param>
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<PagoBancarioResponse> ProcesarTarjetaBancariaCashBackAdvance(ProcesarMovimientoTarjetaBancariaRequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                PagoBancarioResponse respuesta = new PagoBancarioResponse();
                using (TransactionScope scope = new TransactionScope())
                {
                    TarjetaBancariaBusiness tarjeta = new TarjetaBancariaBusiness(this.token);
                    var funcionesCaja = funcionesRepository.GetFunctions(token.CodeBox, token.CodeStore);

                    if (funcionesCaja != null)
                    {
                        respuesta = tarjeta.CashBackAdvance(funcionesCaja.UrlLecturaBancaria, token.CodeStore, request.Retiro.FolioOperacionAsociada, request.Retiro.ImporteCashBack, 1);
                        if (respuesta.CodeNumber == 0)
                        {
                            var movimientoVenta = repository.ProcesarMovimientoTarjetaBancariaRetiro(token.CodeStore, token.CodeBox, token.CodeEmployee, respuesta.Authorization, respuesta.CardNumber, request);
                            scope.Complete();
                        }
                    }
                    else
                    {
                        respuesta.CodeNumber = 103;
                        respuesta.CodeDescription = "No existe funciones asociadas a la caja, revise las funciones registradas de la caja";
                    }
                }

                return respuesta;
            });
        }

        /// <summary>
        /// Metodo para ejecutar una carga de llaves
        /// </summary>	         
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<OperationResponse> CargarLlaves()
        {
            return tryCatch.SafeExecutor(() =>
            {
                OperationResponse respuesta = new OperationResponse();
                TarjetaBancariaBusiness tarjeta = new TarjetaBancariaBusiness(this.token);
                var funcionesCaja = funcionesRepository.GetFunctions(token.CodeBox, token.CodeStore);
                if (funcionesCaja != null)
                {
                    respuesta = tarjeta.CargarLlaves(funcionesCaja.UrlLecturaBancaria);
                }
                else
                {
                    respuesta.CodeNumber = "103";
                    respuesta.CodeDescription = "No existe funciones asociadas a la caja, revise las funciones registradas de la caja";
                }
                return respuesta;
            });
        }

        /// <summary>
        /// Metodo para cancelar la operación bancaria de cobro
        /// </summary>	 
        /// <param name="request">Movimiento tarjeta bancaria</param>
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<PagoBancarioResponse> ProcesarTarjetaBancariaCancelar(ProcesarMovimientoTarjetaBancariaRequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                PagoBancarioResponse respuesta = new PagoBancarioResponse();
                TarjetaBancariaBusiness tarjeta = new TarjetaBancariaBusiness(this.token);
                var funcionesCaja = funcionesRepository.GetFunctions(token.CodeBox, token.CodeStore);
                if (funcionesCaja != null)
                {
                    respuesta = tarjeta.Cancelar(funcionesCaja.UrlLecturaBancaria);
                }
                else
                {
                    respuesta.CodeNumber = 103;
                    respuesta.CodeDescription = "No existe funciones asociadas a la caja, revise las funciones registradas de la caja";
                }
                return respuesta;
            });
        }

        /// <summary>
        /// Metodo para obtener tarjetas bancarias Melody Milano
        /// </summary>	 
        /// <param name="request">Movimiento tarjeta bancaria</param>
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<PagoBancarioResponse> ObtenerTarjetaBancariaMelody(ProcesarMovimientoTarjetaBancariaRequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                PagoBancarioResponse respuesta = new PagoBancarioResponse();
                using (TransactionScope scope = new TransactionScope())
                {
                    TarjetaBancariaBusiness tarjeta = new TarjetaBancariaBusiness(this.token);
                    var funcionesCaja = funcionesRepository.GetFunctions(token.CodeBox, token.CodeStore);

                    if (funcionesCaja != null)
                    {
                        respuesta = tarjeta.ObtenerTarjeta(funcionesCaja.UrlLecturaBancaria, token.CodeStore, request.Venta.FolioOperacionAsociada);
                        if (respuesta.CodeNumber == 0)
                        {
                            scope.Complete();
                        }
                    }
                    else
                    {
                        respuesta.CodeNumber = 103;
                        respuesta.CodeDescription = "No existe funciones asociadas a la caja, revise las funciones registradas de la caja";
                    }
                }
                return respuesta;
            });
        }

        /// <summary>
        /// Metodo para procesar tarjetas de regalo
        /// </summary>	 
        /// <param name="request">Movimiento tarjeta de regalo</param>
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<OperationResponse> ProcesarTarjetaRegalo(ProcesarMovimientoTarjetaRegaloRequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    TarjetaRegalosBusiness tarjeta = new TarjetaRegalosBusiness(this.token);
                    // Se procesan las promociones por venta
                    foreach (var item in request.DescuentosPromocionalesPorVentaAplicados.DescuentoPromocionesAplicados)
                    {
                        OperationResponse response = new OperationResponse();
                        response = repository.PersistirPromocionesVenta(request.FolioOperacionAsociada, token.CodeStore, token.CodeBox, item.ImporteDescuento
                                                                , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);

                    }
                    // Se procesan las promociones por línea de venta
                    foreach (var item in request.DescuentosPromocionalesPorLineaAplicados.DescuentoPromocionesAplicados)
                    {
                        OperationResponse response = new OperationResponse();
                        response = repository.PersistirPromocionesLineaVenta(request.FolioOperacionAsociada, token.CodeStore, token.CodeBox, item.Secuencia, item.ImporteDescuento
                                                                , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);

                    }
                    // Se procesa el pago en base de datos localmente
                    var repuestaLocal = repository.ProcesarMovimientoTarjetaRegalo(token.CodeStore, token.CodeBox, token.CodeEmployee, request);
                    if (repuestaLocal.CodeNumber == "353")
                    {
                        var transaccion = int.Parse(repuestaLocal.CodeDescription);
                        var respuesta = tarjeta.Cobro(this.token.CodeEmployee, request.FolioTarjeta, transaccion, request.FolioOperacionAsociada, request.ImporteVentaTotal);
                        if (respuesta.CodeNumber != "0")
                        {
                            scope.Complete();
                        }
                        return respuesta;
                    }
                    else
                    {
                        return repuestaLocal;
                    }
                }
            });
        }

        /// <summary>
        /// Procesar pago por venta a empleado
        /// </summary>		
        /// <param name="request">Movimiento de venta a empleado</param>
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<OperationResponse> ProcesarVentaEmpleado(ProcesarMovimientoVentaEmpleadoRequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    // Se procesan las promociones por venta
                    foreach (var item in request.DescuentosPromocionalesPorVentaAplicados.DescuentoPromocionesAplicados)
                    {
                        OperationResponse response = new OperationResponse();
                        response = repository.PersistirPromocionesVenta(request.FolioOperacionAsociada, token.CodeStore, token.CodeBox, item.ImporteDescuento
                                                                , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);

                    }
                    // Se procesan las promociones por línea de venta
                    foreach (var item in request.DescuentosPromocionalesPorLineaAplicados.DescuentoPromocionesAplicados)
                    {
                        OperationResponse response = new OperationResponse();
                        response = repository.PersistirPromocionesLineaVenta(request.FolioOperacionAsociada, token.CodeStore, token.CodeBox, item.Secuencia, item.ImporteDescuento
                                                                , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);

                    }
                    // Se procesa el pago en base de datos localmente
                    OperationResponse responseLocal = repository.ProcesarMovimientoVentaEmpleado(token.CodeStore, token.CodeBox, token.CodeEmployee, request);
                    if (responseLocal.CodeNumber == "352")
                    {
                        var codigoTransaccion = int.Parse(responseLocal.CodeDescription);
                        var responseVentaEmpleado = new AdministracionVentaEmpleadoBusiness().RealizarVenta(request, codigoTransaccion, this.token.CodeStore, this.token.CodeBox);
                        if (responseVentaEmpleado.CodeNumber == "1")
                        {
                            scope.Complete();
                        }
                        return responseVentaEmpleado;
                    }
                    else
                    {
                        return responseLocal;
                    }
                }
            });
        }

        /// <summary>
        /// Procesar pago por venta a mayorista
        /// </summary>
        /// <param name="request">Movimiento venta a mayorista</param>
        /// <returns>Resultado de la operación</returns>
        public ResponseBussiness<OperationResponse> ProcesarVentaMayorista(ProcesarMovimientoMayorista request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    InfoValeResponse infoVale = new MayoristasBusiness(this.token).ValidarVale(request.NumeroVale);
                    if (infoVale.Estatus == "D")
                    {
                        // Se procesan las promociones por venta
                        foreach (var item in request.DescuentosPromocionalesPorVentaAplicados.DescuentoPromocionesAplicados)
                        {
                            OperationResponse response = new OperationResponse();
                            response = repository.PersistirPromocionesVenta(request.FolioOperacionAsociada, token.CodeStore, token.CodeBox, item.ImporteDescuento
                                                                    , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);

                        }
                        // Se procesan las promociones por línea de venta
                        foreach (var item in request.DescuentosPromocionalesPorLineaAplicados.DescuentoPromocionesAplicados)
                        {
                            OperationResponse response = new OperationResponse();
                            response = repository.PersistirPromocionesLineaVenta(request.FolioOperacionAsociada, token.CodeStore, token.CodeBox, item.Secuencia, item.ImporteDescuento
                                                                    , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);

                        }
                        // Se procesa el pago en base de datos localmente
                        OperationResponse responseLocal = repository.ProcesarMovimientoMayorista(token.CodeStore, token.CodeBox, token.CodeEmployee, request);
                        if (responseLocal.CodeNumber == "354")
                        {
                            var codigoTransaccion = int.Parse(responseLocal.CodeDescription);
                            var responseVentaEmpleado = new MayoristasBusiness(this.token).PagoVentaMayorista(request, codigoTransaccion);
                            if (responseVentaEmpleado.CodeNumber == "1")
                            {
                                scope.Complete();
                            }
                            return responseVentaEmpleado;
                        }
                        else
                        {
                            return responseLocal;
                        }
                    }
                    else
                    {
                        OperationResponse operationVale = new OperationResponse();
                        operationVale.CodeNumber = "0";
                        operationVale.CodeDescription = this.repository.ObtenerMensajeValeNoDisponible().CodeDescription;
                        return operationVale;

                    }
                }
            });
        }

        /// <summary>
        /// Metodo para procesar una nota de crédito
        /// </summary>	 
        /// <param name="procesarMovimientoNotaCreditoRequest">Movimiento Nota de Crédito</param>
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<OperationResponse> ProcesarNotaCredito(ProcesarMovimientoNotaCreditoRequest procesarMovimientoNotaCreditoRequest)
        {
            OperationResponse respuestaValidacion = new OperationResponse();
            return tryCatch.SafeExecutor(() =>
            {
                //Validar que la nota si la nota de credito se quiere cobrar el mismo día que se redimio
                respuestaValidacion = EsNotaCreditoOffline(token.CodeStore, token.CodeBox, procesarMovimientoNotaCreditoRequest.FolioNotaCredito);

                using (TransactionScope scope = new TransactionScope())
                {
                    //TarjetaRegalosBusiness tarjeta = new TarjetaRegalosBusiness(this.token);
                    NotaCreditoBusiness notaCredito = new NotaCreditoBusiness(token);
                    // Se procesan las promociones por venta
                    foreach (var item in procesarMovimientoNotaCreditoRequest.DescuentosPromocionalesPorVentaAplicados.DescuentoPromocionesAplicados)
                    {
                        OperationResponse response = new OperationResponse();
                        response = repository.PersistirPromocionesVenta(procesarMovimientoNotaCreditoRequest.FolioOperacionAsociada, token.CodeStore, token.CodeBox, item.ImporteDescuento
                                                                , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);

                    }
                    // Se procesan las promociones por línea de venta
                    foreach (var item in procesarMovimientoNotaCreditoRequest.DescuentosPromocionalesPorLineaAplicados.DescuentoPromocionesAplicados)
                    {
                        OperationResponse response = new OperationResponse();
                        response = repository.PersistirPromocionesLineaVenta(procesarMovimientoNotaCreditoRequest.FolioOperacionAsociada, token.CodeStore, token.CodeBox, item.Secuencia, item.ImporteDescuento
                                                                , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);

                    }

                    // Se procesa el pago en base de datos localmente
                    OperationResponse respuestaLocal = new OperationResponse();
                    // Se procesa el pago en Web Service Externo Milano                    
                    if (respuestaValidacion.CodeNumber == "0")
                    {
                        respuestaLocal = repository.PersistirMovimientoNotaCredito(token.CodeStore, token.CodeBox, token.CodeEmployee, procesarMovimientoNotaCreditoRequest, 0);
                        var respuesta = notaCredito.Cobro(this.token.CodeEmployee, procesarMovimientoNotaCreditoRequest.FolioNotaCredito, respuestaLocal.Transaccion, procesarMovimientoNotaCreditoRequest.FolioOperacionAsociada,
                                                            respuestaLocal.CodigoTipoTrxCab, procesarMovimientoNotaCreditoRequest.ImporteVentaTotal);
                        if (respuesta.CodeNumber != "0")
                        {
                            respuesta.CodeNumber = respuestaLocal.CodeNumber;
                            scope.Complete();
                        }
                        return respuesta;
                    }
                    // Se procesa el pago con Redención Local
                    if (respuestaValidacion.CodeNumber == "100")
                    {
                        respuestaLocal = repository.PersistirMovimientoNotaCredito(token.CodeStore, token.CodeBox, token.CodeEmployee, procesarMovimientoNotaCreditoRequest, 1);
                        if (respuestaLocal.CodeNumber == "353")
                        {
                            scope.Complete();
                            return respuestaLocal;
                        }
                        else
                        {
                            // Ocurrió algún error
                            return respuestaLocal;
                        }
                    }
                    // No es posible procesar el pago
                    else
                    {
                        return respuestaValidacion;
                    }
                }
            });
        }

        private OperationResponse EsNotaCreditoOffline(int codeStore, int codeBox, string folioNotaCredito)
        {
            return repository.ValidacionRedimirNotaCredito(codeStore, codeBox, folioNotaCredito);
        }

        /// <summary>
        /// Metodo para procesar redenciones de cupones promocionales
        /// </summary>	 
        /// <param name="request">Movimiento redención cupon promocional</param>
        /// <returns>Respuesta de la operación y saldo aplicado asociado</returns>
        public ResponseBussiness<ProcesarMovimientoRedencionCuponResponse> ProcesarRedencionCuponPromocional(ProcesarMovimientoRedencionCuponRequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    // Se procesan las promociones por venta
                    foreach (var item in request.DescuentosPromocionalesPorVentaAplicados.DescuentoPromocionesAplicados)
                    {
                        OperationResponse response = new OperationResponse();
                        response = repository.PersistirPromocionesVenta(request.FolioOperacionAsociada, token.CodeStore, token.CodeBox, item.ImporteDescuento
                                                                , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);
                    }
                    
                    // Se procesan las promociones por línea de venta
                    foreach (var item in request.DescuentosPromocionalesPorLineaAplicados.DescuentoPromocionesAplicados)
                    {
                        OperationResponse response = new OperationResponse();
                        response = repository.PersistirPromocionesLineaVenta(request.FolioOperacionAsociada, token.CodeStore, token.CodeBox, item.Secuencia, item.ImporteDescuento
                                                                , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);
                    }

                    // Se procesa el cupón para obtener el monto que aplica
                    RedencionCuponPromocionalBusiness redencionCuponPromocionalBusiness = new RedencionCuponPromocionalBusiness(token);
                    
                    CuponesRedimirBusiness cuponesRedimirBusiness = new CuponesRedimirBusiness(this.token);
                    CuponRedimirResponse cuponRedimirResponse = new CuponRedimirResponse();
                    ValidarCuponRequest validarCuponRequest = new ValidarCuponRequest();
                    validarCuponRequest.FolioCupon = request.FolioCuponPromocional;
                    validarCuponRequest.FolioVenta = request.FolioOperacionAsociada;

                    // OCG: Consulta el cupon en el web service
                    cuponRedimirResponse = cuponesRedimirBusiness.SaldoRedimir(validarCuponRequest);

                    // Se procesa el pago en base de datos localmente y llamando a Ws Milano si el saldo es superior a 0.00 pesos
                    ProcesarMovimientoRedencionCuponResponse procesarMovimientoRedencionCuponResponse = new ProcesarMovimientoRedencionCuponResponse();

                    if (cuponRedimirResponse == null)
                    {
                        procesarMovimientoRedencionCuponResponse.CodeDescription = "Servicio de CUPONES no disponible por el momento.";
                        procesarMovimientoRedencionCuponResponse.CodeNumber = "999";
                        return procesarMovimientoRedencionCuponResponse;
                    }
                    
                    request.ImporteVentaTotal = cuponRedimirResponse.Saldo;

                    if (cuponRedimirResponse.Saldo == 0)
                    {
                        procesarMovimientoRedencionCuponResponse.CodeNumber = "703";
                        procesarMovimientoRedencionCuponResponse.CodeDescription = cuponRedimirResponse.MensajeRedencion;
                    }
                    else
                    {
                        OperationResponse responseRealizarVenta = redencionCuponPromocionalBusiness.RealizarVentaCuponPromocional(token.CodeEmployee, request.FolioOperacionAsociada, request.FolioCuponPromocional, cuponRedimirResponse.Transaccion, request.ImporteVentaTotal, cuponRedimirResponse.CodigoTipoTrxCab);
                        if (responseRealizarVenta.CodeNumber == "0")
                        {
                            // El cupon se puede redimir el mismo dia de la creacion 
                            if (cuponRedimirResponse.EsRedimibleHoy == 1 && responseRealizarVenta.CodeDescription == "El folio de venta no existe")
                            {
                                procesarMovimientoRedencionCuponResponse = repository.ProcesarMovimientoRedencionCuponPromocional(token.CodeStore, token.CodeBox, token.CodeEmployee, request);
                                procesarMovimientoRedencionCuponResponse.CodeDescription = cuponRedimirResponse.MensajeRedencion;
                                scope.Complete();
                            }
                            else
                            {
                                procesarMovimientoRedencionCuponResponse.CodeNumber = "703";
                                procesarMovimientoRedencionCuponResponse.CodeDescription = responseRealizarVenta.CodeDescription;
                            }
                        }
                        else
                        {
                            procesarMovimientoRedencionCuponResponse = repository.ProcesarMovimientoRedencionCuponPromocional(token.CodeStore, token.CodeBox, token.CodeEmployee, request);
                            procesarMovimientoRedencionCuponResponse.CodeDescription = cuponRedimirResponse.MensajeRedencion;
                            scope.Complete();
                        }
                    }
                    return procesarMovimientoRedencionCuponResponse;
                }
            });
        }

        /// <summary>
        /// Metodo para procesar pago con PinPad Móvil
        /// </summary>	 
        /// <param name="request">Movimiento tarjeta de regalo</param>
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<OperationResponse> ProcesarPagoPinPadMovil(ProcesarMovimientoPagoPinPadMovilRequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    // Se procesan las promociones por venta
                    foreach (var item in request.DescuentosPromocionalesPorVentaAplicados.DescuentoPromocionesAplicados)
                    {
                        OperationResponse response = new OperationResponse();
                        response = repository.PersistirPromocionesVenta(request.FolioOperacionAsociada, token.CodeStore, token.CodeBox, item.ImporteDescuento
                                                                , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, 
                                                                item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);

                    }

                    // Se procesan las promociones por línea de venta
                    foreach (var item in request.DescuentosPromocionalesPorLineaAplicados.DescuentoPromocionesAplicados)
                    {
                        OperationResponse response = new OperationResponse();
                        response = repository.PersistirPromocionesLineaVenta(request.FolioOperacionAsociada, token.CodeStore, token.CodeBox, item.Secuencia, item.ImporteDescuento
                                                                , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento,
                                                                item.FormaPagoCodigoPromocionAplicado);

                    }

                    //Verificar si cuenta con credito la tienda
                    //ValidaTransferenciaResponse infoVale = new MayoristasBusiness(this.token).ValidarTransferencia(this.token.CodeStore);

                    ValidaTransferenciaResponse infoVale = new NotaCreditoBusiness(this.token).ValidarTransferencia(this.token.CodeStore, request.ImporteVentaTotal, request.FolioAutorizacionPinPadMovil);

                    OperationResponse preRespuestaLocal = new OperationResponse();

                    if (infoVale.Error == "999")
                    {
                        preRespuestaLocal.CodeNumber = "999";
                        preRespuestaLocal.CodeDescription = "La tienda no cuenta con saldo disponible.";
                        return preRespuestaLocal;
                    }
                    else {
                        // Se procesa el pago en base de datos localmente
                        OperationResponse respuestaLocal = repository.ProcesarPagoPinPadMovil(token.CodeStore, token.CodeBox, token.CodeEmployee, request);
                        scope.Complete();
                        return respuestaLocal;
                    }
                }
            });
        }



        /// <summary>
        ///                                                                                                                                                                                                                                                                                                                        
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> ProcesarPagoTransferencia(ProcesarMovimientoPagoTransferenciaRequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    // Se procesan las promociones por venta
                    foreach (var item in request.DescuentosPromocionalesPorVentaAplicados.DescuentoPromocionesAplicados)
                    {
                        OperationResponse response = new OperationResponse();
                        response = repository.PersistirPromocionesVenta(request.FolioOperacionAsociada, token.CodeStore, token.CodeBox, item.ImporteDescuento
                                                                , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);

                    }

                    // Se procesan las promociones por línea de venta
                    foreach (var item in request.DescuentosPromocionalesPorLineaAplicados.DescuentoPromocionesAplicados)
                    {
                        OperationResponse response = new OperationResponse();
                        response = repository.PersistirPromocionesLineaVenta(request.FolioOperacionAsociada, token.CodeStore, token.CodeBox, item.Secuencia, item.ImporteDescuento
                                                                , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);

                    }

                    // Se procesa el pago en base de datos localmente
                    OperationResponse respuestaLocal = repository.ProcesarPagoTransferencia(token.CodeStore, token.CodeBox, token.CodeEmployee, request);
                    scope.Complete();
                    return respuestaLocal;
                }
            });
        }

    }
}
