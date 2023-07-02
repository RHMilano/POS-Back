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
using Milano.BackEnd.Business;
using Milano.BackEnd.Business.MM;
using Milano.BackEnd.Business.ImpresionMM;
using Milano.BackEnd.Business.Finlag;
using System.Xml;
using Milano.BackEnd.Dto.General;
using System.Net.Mail;
using System.Net;
using Project.Services.Utils;
using Milano.BackEnd.Business.External;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using Milano.BackEnd.Dto.Lealtad;
using System.Security.Policy;

namespace Milano.BackEnd.Business
{
    /// <summary>
    /// Servicio para gestionar operaciones de venta
    /// </summary>
    public class SalesBusiness : BaseBusiness
    {
        // ProxyInfoDescuento3.InfoDescuentoSoapClient proxy;

        /// <summary>
        /// Repositorio de ventas
        /// </summary>
        protected SalesRepository repository;

        /// <summary>
        /// Clase Business para ejecutar descuentos por mercancía dañada y picos de mercancía
        /// </summary>
        protected DescuentoMercanciaDaniadaBusiness descuentoMercanciaDaniadaBusiness;

        /// <summary>
        /// Repositorio para la obtención de promociones
        /// </summary>
        protected DescuentosPromocionesRepository descuentosPromocionesRepository;

        /// <summary>
        /// Atributo del token usuario
        /// </summary>
        protected TokenDto token;

        /// <summary>
        /// Constructor por default
        /// </summary>
        public SalesBusiness(TokenDto token)
        {
            //  1 OCG
            this.repository = new SalesRepository();
            this.descuentosPromocionesRepository = new DescuentosPromocionesRepository();
            this.descuentoMercanciaDaniadaBusiness = new DescuentoMercanciaDaniadaBusiness(token);
            this.token = token;
        }

        /// <summary>
        /// Almacenamiento de una Linea Ticket
        /// </summary>
        /// <param name="lineaTicket">Objeto de peticion linea ticket de la venta</param>
        /// <returns></returns>
        public ResponseBussiness<OperacionLineaTicketVentaResponse> AgregarLineaTicketVenta(LineaTicket lineaTicket)
        {
            return tryCatch.SafeExecutor(() =>
            {
                OperacionLineaTicketVentaResponse response = repository.AgregarLineaTicketVenta(token.CodeStore, token.CodeBox, token.CodeEmployee, lineaTicket);
                return response;
            });
        }

        /// <summary>
        /// Cambio de Piezas de una Linea Ticket
        /// </summary>
        /// <param name="lineaTicket">Objeto de peticion linea ticket de la venta</param>
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> CambiarPiezasLineaTicketVenta(LineaTicket lineaTicket)
        {
            return tryCatch.SafeExecutor(() =>
            {
                OperationResponse response = repository.CambiarPiezasLineaTicketVenta(token.CodeStore, token.CodeBox, token.CodeEmployee, lineaTicket);
                return response;
            });
        }

        /// <summary>
        /// Cambio de Precio de una Linea Ticket
        /// </summary>
        /// <param name="cambiarPrecioRequest">Objeto de peticion linea ticket de la venta</param>
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> CambiarPrecioLineaTicketVenta(CambiarPrecioRequest cambiarPrecioRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.CambiarPrecioLineaTicketVenta(token.CodeStore, token.CodeBox, token.CodeEmployee, cambiarPrecioRequest);
            });
        }

        /// <summary>
        /// Eliminación de una Linea Ticket
        /// </summary>
        /// <param name="secuenciaOriginalLineaTicket">Secuencia original de la línea eliminada</param>  
        /// <param name="lineaTicket">Objeto de peticion linea ticket de la venta</param>
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> EliminarLineaTicketVenta(int secuenciaOriginalLineaTicket, LineaTicket lineaTicket, int codigoRazon)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.EliminarLineaTicketVenta(token.CodeStore, token.CodeBox, token.CodeEmployee, secuenciaOriginalLineaTicket, lineaTicket, codigoRazon);
            });
        }

        /// <summary>
        /// Cerrar una devolución sin pagos
        /// </summary>
        /// <param name="totalizarVentaRequest">Objeto de peticion de la venta a totalizar</param>        
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> CerrarDevolucionSinPagos(TotalizarVentaRequest totalizarVentaRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                ImprimeTicketsMM imprimeTicketsMM = new ImprimeTicketsMM(token);
                return imprimeTicketsMM.PrintTicket(totalizarVentaRequest.cabeceraVentaRequest.FolioOperacion, false);
            });
        }

        /// <summary>
        /// Totalización de venta
        /// </summary>
        /// <param name="totalizarVentaRequest">Objeto de peticion de la venta a totalizar</param>
        /// <returns></returns>
        public ResponseBussiness<TotalizarVentaResponse> TotalizarVenta(TotalizarVentaRequest totalizarVentaRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                // Variables de control y respuesta
                TotalizarVentaResponse response = new TotalizarVentaResponse();
                decimal totalDescuentosAplicadosPorMotorPromociones = 0.0M;

                using (TransactionScope scope = new TransactionScope())
                {

                    // Se invoca a Totalizar la Venta/Devolución
                    response = repository.TotalizarVenta(totalizarVentaRequest, token.CodeStore, token.CodeBox, token.CodeEmployee);

                    // Se aplican promociones por motor de promociones 
                    List<DescuentoPromocionalVenta> DescuentosPromocionalesPosiblesVenta = new List<DescuentoPromocionalVenta>();
                    List<DescuentoPromocionalVenta> DescuentosPromocionalesAplicadosVenta = new List<DescuentoPromocionalVenta>();
                    List<DescuentoPromocionalVenta> DescuentosPromocionalesPosiblesLinea = new List<DescuentoPromocionalVenta>();
                    List<DescuentoPromocionalVenta> DescuentosPromocionalesAplicadosLinea = new List<DescuentoPromocionalVenta>();

                    // Eliminar promociones previas otorgados por motor de promociones
                    repository.EliminarPromocionesCab(response.FolioOperacion, token.CodeStore, token.CodeBox);
                    repository.EliminarPromocionesDet(response.FolioOperacion, token.CodeStore, token.CodeBox);

                    // Obtener las promociones vigentes
                    DescuentoPromocionalVenta[] descuentosPromocionalesEncontrados = descuentosPromocionesRepository.ObtenerPromocionesVenta(response.FolioOperacion, token.CodeStore, token.CodeBox, totalizarVentaRequest.cabeceraVentaRequest.NivelLealtad, totalizarVentaRequest.cabeceraVentaRequest.PrimeraCompraLealtad, (int)totalizarVentaRequest.cabeceraVentaRequest.CodigoClienteLealtad);

                    // Agregar descuentos promocionales existentes en motor de promociones                                    
                    foreach (DescuentoPromocionalVenta descuentoPromocionalVenta in descuentosPromocionalesEncontrados)
                    {
                        // Validar si se trata de descuento promocional por linea
                        if (descuentoPromocionalVenta.Secuencia > 0)
                        {
                            if (descuentoPromocionalVenta.DescuentosPromocionalesFormaPago != null && descuentoPromocionalVenta.DescuentosPromocionalesFormaPago.Length > 0)
                            {
                                DescuentosPromocionalesPosiblesLinea.Add(descuentoPromocionalVenta);
                            }
                            else
                            {
                                DescuentosPromocionalesAplicadosLinea.Add(descuentoPromocionalVenta);
                            }
                        }
                        else
                        {
                            if (descuentoPromocionalVenta.DescuentosPromocionalesFormaPago != null && descuentoPromocionalVenta.DescuentosPromocionalesFormaPago.Length > 0)
                            {
                                DescuentosPromocionalesPosiblesVenta.Add(descuentoPromocionalVenta);
                            }
                            else
                            {
                                DescuentosPromocionalesAplicadosVenta.Add(descuentoPromocionalVenta);
                            }
                        }
                    }

                    response.DescuentosPromocionalesAplicadosVenta = DescuentosPromocionalesAplicadosVenta.ToArray();
                    response.DescuentosPromocionalesPosiblesVenta = DescuentosPromocionalesPosiblesVenta.ToArray();
                    response.DescuentosPromocionalesAplicadosLinea = DescuentosPromocionalesAplicadosLinea.ToArray();
                    response.DescuentosPromocionalesPosiblesLinea = DescuentosPromocionalesPosiblesLinea.ToArray();

                    // Se persisten las promociones aplicadas por venta                
                    foreach (var item in response.DescuentosPromocionalesAplicadosVenta)
                    {
                        OperationResponse responsePersist = new OperationResponse();
                        responsePersist = repository.PersistirPromocionesVenta(response.FolioOperacion, token.CodeStore, token.CodeBox, item.ImporteDescuento
                                                                , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, string.Empty);
                        totalDescuentosAplicadosPorMotorPromociones = totalDescuentosAplicadosPorMotorPromociones + item.ImporteDescuento;
                    }

                    // Se persisten las promociones aplicadas por linea                    
                    foreach (var item in response.DescuentosPromocionalesAplicadosLinea)
                    {
                        OperationResponse responsePersist = new OperationResponse();
                        responsePersist = repository.PersistirPromocionesLineaVenta(response.FolioOperacion, token.CodeStore, token.CodeBox, item.Secuencia, item.ImporteDescuento
                                                                , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, string.Empty);
                        totalDescuentosAplicadosPorMotorPromociones = totalDescuentosAplicadosPorMotorPromociones + item.ImporteDescuento;
                    }

                    // Se agrega ajuste necesario para DEVOLUCIONES-PROMOCIONES 
                    if (!(String.IsNullOrEmpty(totalizarVentaRequest.cabeceraVentaRequest.FolioDevolucion)))
                    {
                        // Validar si la devolución debe terminarse
                        if ((response.InformacionAsociadaDevolucion == null) &&
                        ((totalizarVentaRequest.cabeceraVentaRequest.DevolucionSaldoAFavor >= 0) &&
                        (totalizarVentaRequest.cabeceraVentaRequest.ClienteTieneSaldoPendientePagar == false)))
                        {
                            response.InformacionAsociadaDevolucion = repository.FinalizarVentaDevolucion(totalizarVentaRequest.cabeceraVentaRequest.FolioOperacion, totalizarVentaRequest.cabeceraVentaRequest.FolioDevolucion,
                            totalizarVentaRequest.cabeceraVentaRequest.FolioVentaOriginal, token.CodeBox, token.CodeStore, totalizarVentaRequest.cabeceraVentaRequest.CodigoMayorista, totalizarVentaRequest.cabeceraVentaRequest.ImporteVentaDescuentos, totalizarVentaRequest.cabeceraVentaRequest.ImporteDevolucionImpuestos,
                            totalizarVentaRequest.cabeceraVentaRequest.ImporteVentaBruto, totalizarVentaRequest.cabeceraVentaRequest.ImporteVentaImpuestos, totalizarVentaRequest.cabeceraVentaRequest.ImporteVentaNeto,
                            totalizarVentaRequest.cabeceraVentaRequest.ImporteDevolucionNeto, totalizarVentaRequest.cabeceraVentaRequest.ImporteDevolucionImpuestos, totalizarVentaRequest.cabeceraVentaRequest.DevolucionSaldoAFavor, totalizarVentaRequest.cabeceraVentaRequest.ImporteDevolucionTotal);
                        }

                        // Hacer los ajustes a las propiedades si la devolución fue cerrada
                        if (response.InformacionAsociadaDevolucion != null)
                        {
                            // No se regresan promociones
                            response.DescuentosPromocionalesAplicadosVenta = new DescuentoPromocionalVenta[0];
                            response.DescuentosPromocionalesPosiblesVenta = new DescuentoPromocionalVenta[0];
                            response.DescuentosPromocionalesAplicadosLinea = new DescuentoPromocionalVenta[0];
                            response.DescuentosPromocionalesPosiblesLinea = new DescuentoPromocionalVenta[0];
                            // No se regresan formas de pago
                            response.InformacionAsociadaFormasPago = null;
                            response.InformacionAsociadaFormasPagoMonedaExtranjera = null;
                        }
                    }

                    // Se termina el bloque de transacción
                    scope.Complete();
                }

                // Validar si debe imprimirse ticket por una devolución que no pasa a formas de pago                
                if (!(String.IsNullOrEmpty(totalizarVentaRequest.cabeceraVentaRequest.FolioDevolucion)))
                {
                    if (response.InformacionAsociadaDevolucion != null)
                    {
                        ImprimeTicketsMM imprimeTicketsMM = new ImprimeTicketsMM(token);
                        imprimeTicketsMM.PrintTicket(totalizarVentaRequest.cabeceraVentaRequest.FolioOperacion, false);
                    }
                }

                // Se regresa la respuesta final
                return response;
            });
        }

        /// <summary>
        /// Finalización de venta
        /// </summary>
        /// <param name="finalizarVentaRequest">Objeto de peticion de la venta a finalizar</param>
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> FinalizarVenta(FinalizarVentaRequest finalizarVentaRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return FinalizarVentaInternal(finalizarVentaRequest);
            });
        }

        /// <summary>
        /// Finalización de venta
        /// </summary>
        /// <param name="redencionPuntosLealtadRequest">Objeto de peticion de la venta a finalizar</param>
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> AplicarRedencionPuntosLealtad(RedencionPuntosLealtadRequest redencionPuntosLealtadRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return AplicarRedencionPuntosLealtadInternal(redencionPuntosLealtadRequest);
            });
        }


        private ResponseBussiness<OperationResponse> AplicarRedencionPuntosLealtadInternal(RedencionPuntosLealtadRequest redencionPuntosLealtadRequest)
        {

            RedencionPuntosLealtadRequest request = new RedencionPuntosLealtadRequest();
            RedencionPuntosLealtadResponse response = new RedencionPuntosLealtadResponse();
            FinlagBusiness v = new FinlagBusiness(token);

            // Agregar descuentos promocionales existentes en motor de promociones                                    

            request.ssCodigoBarras = redencionPuntosLealtadRequest.ssCodigoBarras;
            request.iiCodigoEmpleado = redencionPuntosLealtadRequest.iiCodigoEmpleado;
            request.iiCodigoTienda = redencionPuntosLealtadRequest.iiCodigoTienda;
            request.iiTransaccion = this.repository.ObtenerTrxPorFolio(redencionPuntosLealtadRequest.ssFolioVenta);
            request.ssFolioVenta = redencionPuntosLealtadRequest.ssFolioVenta;
            request.ddMonto = redencionPuntosLealtadRequest.ddMonto;
            request.iiCodigoCaja = redencionPuntosLealtadRequest.iiCodigoCaja;

            response = v.RedimirPuntosLealtad(request);

            OperationResponse operacionResponse = new OperationResponse();

            operacionResponse.CodeDescription = response.ssMensaje;
            operacionResponse.CodeNumber = response.bbError ? "9999" : "0";
            operacionResponse.CodigoTipoTrxCab = response.ssSesion.ToString();
            //operacionResponse.Transaccion = ;

            return operacionResponse;

        }

        private ResponseBussiness<OperationResponse> FinalizarVentaInternal(FinalizarVentaRequest finalizarVentaRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                OperationResponse result = new OperationResponse();
                string mensajeVentaFinalizada = "Venta finalizada exitosamente. ";
                ExchangeRateResponse exchangeRateResponse = new ExchangeRateResponse();
                SaleRequest saleRequest = new SaleRequest();
                SaleResponse saleResponse = new SaleResponse();
                AdministracionTipoCambio administracionTipoCambio = new AdministracionTipoCambio();
                NotaCreditoBusiness notaCredito = new NotaCreditoBusiness(this.token);
                ValidaTransferenciaResponse validaTransferencia = new ValidaTransferenciaResponse();
                Boolean esTransferencia = false;

                Random random = new Random();
                string estampaTiempo;
                estampaTiempo = DateTime.Now.ToString("yyyyMMddHHmmss") + $"_{random.Next(0, 9)}{random.Next(1, 5)}{random.Next(5, 9)}{random.Next(0, 4)}{random.Next(4, 9)}{random.Next(0, 9)}";
                VerifySale verificacionVenta = new VerifySale();

                using (TransactionScope scope = new TransactionScope())
                {
                    // OCG
                    //Verifica si tiene formas de pago por transaccion
                    if (finalizarVentaRequest.FormasPagoUtilizadas.Length > 0)
                    {
                        for (int i = 0; i < finalizarVentaRequest.FormasPagoUtilizadas.Length; i++)
                        {
                            // Recuperar los montos pagados en USD
                            if (finalizarVentaRequest.FormasPagoUtilizadas[i].CodigoFormaPagoImporte == "TR")
                            {
                                esTransferencia = true;
                                validaTransferencia = notaCredito.AplicaPagoTransferencia(this.token.CodeStore, finalizarVentaRequest.FormasPagoUtilizadas[i].ImporteMonedaNacional, this.token.CodeEmployee, finalizarVentaRequest.FolioVenta, "200");
                            }
                            else
                            {
                                esTransferencia = false;
                            }
                        }
                    }

                    if (esTransferencia)
                    {
                        if (validaTransferencia.Error == "999")
                        {
                            result.CodeNumber = validaTransferencia.Error;
                            result.CodeDescription = "La tienda no cuenta con crédito asignado.";
                            return result;
                        }
                    }

                    //Verificar si tiene formas de pago en dolares
                    if (finalizarVentaRequest.FormasPagoUtilizadas.Length > 0)
                    {
                        for (int i = 0; i < finalizarVentaRequest.FormasPagoUtilizadas.Length; i++)
                        {
                            // Recuperar los montos pagados en USD
                            if (finalizarVentaRequest.FormasPagoUtilizadas[i].CodigoFormaPagoImporte == "US")
                            {
                                TipoCambioActualizado tipoCambioActualizado = new TipoCambioActualizado();

                                tipoCambioActualizado = repository.DivisaActualizada();

                                if (tipoCambioActualizado.TipoCambio == 0)// Tipo de cambio no actualizado a la fecha actual
                                {
                                    TipoCambioRequest tipoCambioRequest = new TipoCambioRequest();

                                    tipoCambioRequest.CodigoTipoDivisa = "US";
                                    tipoCambioRequest.ImporteMonedaNacional = 0;

                                    exchangeRateResponse = administracionTipoCambio.GetRate(tipoCambioRequest);
                                }
                                else// Arriba y primero
                                {
                                    exchangeRateResponse.MaxExchangeRateBuy = tipoCambioActualizado.TipoCambio;
                                    exchangeRateResponse.ResponseCode = "000";
                                    exchangeRateResponse.MaxAmountPerSale = tipoCambioActualizado.ReciboMaximo;
                                    exchangeRateResponse.MaxChangePerSale = tipoCambioActualizado.CambioMaximo;
                                }

                                if (exchangeRateResponse.ResponseCode == "000")
                                {
                                    saleRequest.CurrencyCode = "840";
                                    //Convertir el monto recibido de la compra en dolares
                                    saleRequest.ReceivedAmount = decimal.Round(finalizarVentaRequest.FormasPagoUtilizadas[i].ImporteMonedaNacional / exchangeRateResponse.MaxExchangeRateBuy, 2);
                                    saleRequest.IdMerchantTransaction = finalizarVentaRequest.FolioVenta;
                                    //Convertir el monto de la venta en dolares
                                    saleRequest.PurchaseAmount = decimal.Round((finalizarVentaRequest.FormasPagoUtilizadas[i].ImporteMonedaNacional - finalizarVentaRequest.FormasPagoUtilizadas[i].ImporteCambioMonedaNacional) / exchangeRateResponse.MaxExchangeRateBuy, 2);
                                    saleRequest.StoreTeller = token.CodeBox; // Caja
                                    saleRequest.ClientCode = token.CodeStore; // Tienda

                                    decimal step = (decimal)Math.Pow(10, 2);
                                    decimal tmp = Math.Truncate(step * exchangeRateResponse.MaxExchangeRateBuy);
                                    //return tmp / step;

                                    saleRequest.UsedExchangeRate = tmp / step; //exchangeRateResponse.MaxExchangeRateBuy;
                                }

                                saleRequest.PosTimestamp = estampaTiempo;

                                // INICIO DE LAS VALIDACIONES
                                // Solicitar autorizacion de la venta
                                int contador = 1;
                            IniciarVentaUSD:

                                saleResponse = administracionTipoCambio.Sale(saleRequest);

                                //La venta no se realizo, se va a validar por el metodo de verificacion, 1ra vuelta
                                if (saleResponse.ResponseCode == "-111")
                                {
                                    // Ejecutar verificacion 1ra vuelta
                                    verificacionVenta = administracionTipoCambio.VerifySale(finalizarVentaRequest.FolioVenta, estampaTiempo, token.CodeStore.ToString());

                                    // Si primera verificacion no disponible
                                    if (verificacionVenta.VerifyResponseCode == "-111")
                                    {
                                        verificacionVenta = null;
                                        // Ejecutar una segunda verificacion
                                        verificacionVenta = administracionTipoCambio.VerifySale(finalizarVentaRequest.FolioVenta, estampaTiempo, token.CodeStore.ToString());

                                        // Resultado segunda verificacion, no responde
                                        if (verificacionVenta.VerifyResponseCode == "-111")
                                        {
                                            // Cancelar la venta
                                            result.CodeDescription = "-111";
                                            result.CodeDescription = "El servicio no se encuentra disponible. Intentar con otra forma de pago";

                                            return result;
                                        }

                                        // Ya hubo respuesta
                                        if (verificacionVenta.VerifyResponseCode == "000")
                                        {
                                            // Venta localizada
                                            if (verificacionVenta.TransactionResponseCode == "000")
                                            {
                                                finalizarVentaRequest.FormasPagoUtilizadas[i].Autorizacion = verificacionVenta.AuthoNumber;
                                            }

                                            // Venta no localizada, iniciar otra autorizacion de venta
                                            if (verificacionVenta.TransactionResponseCode == "100" && contador <= 1)
                                            {
                                                contador = contador + 1;
                                                goto IniciarVentaUSD;
                                            }
                                            else
                                            {
                                                result.CodeDescription = "La validacion se ejecuto 2 veces y su ultimo resultado es LA TRANSACCION NO FUE ENCONTRADA, intente con otra forma de pago ";
                                                return result;
                                            }
                                        }

                                        // Resultado segunda verificacion, Se debe de volver a consultar la verifcacion
                                        if (verificacionVenta.VerifyResponseCode == "102")
                                        {
                                            // Cancelar la venta
                                            result.CodeDescription = "-111";
                                            result.CodeDescription = $"Codigo recibido: {verificacionVenta.VerifyResponseCode}, en segunda ejecucion. {verificacionVenta.verifyResponseMessage} Intente con otra forma de pago";

                                        }

                                    }

                                    // Si primera verifiacion es erronea, volver a validar
                                    if (verificacionVenta.VerifyResponseCode == "102")
                                    {
                                        verificacionVenta = null;
                                        verificacionVenta = administracionTipoCambio.VerifySale(finalizarVentaRequest.FolioVenta, estampaTiempo, token.CodeStore.ToString());

                                        // Si servicio no responde o se mantiene el error, cancelar venta
                                        if (verificacionVenta.VerifyResponseCode == "-111")
                                        {
                                            // Cancelar la venta
                                            result.CodeDescription = "-111";
                                            result.CodeDescription = "El servicio no se encuentra disponible. Intentar con otra forma de pago";

                                            return result;
                                        }

                                        // Si servicio no responde o se mantiene el error, cancelar venta
                                        if (verificacionVenta.VerifyResponseCode == "102")
                                        {
                                            // Cancelar la venta
                                            result.CodeDescription = "-111";
                                            result.CodeDescription = $"Codigo recibido: {verificacionVenta.VerifyResponseCode}, en segunda ejecucion. {verificacionVenta.verifyResponseMessage} Intente con otra forma de pago";

                                            return result;
                                        }

                                        // El servicio ya responde
                                        if (verificacionVenta.VerifyResponseCode == "000")
                                        {
                                            // Validar si encontro el ticket
                                            if (verificacionVenta.TransactionResponseCode == "100" && contador == 1)
                                            {
                                                //Enviar de nuevo la venta
                                                contador = contador + 1;
                                                goto IniciarVentaUSD;
                                            }
                                            if (verificacionVenta.TransactionResponseCode == "000")
                                            {
                                                finalizarVentaRequest.FormasPagoUtilizadas[i].Autorizacion = verificacionVenta.AuthoNumber;
                                            }
                                            else
                                            {
                                                // Devolver mensaje de error
                                                result.CodeDescription = verificacionVenta.VerifyResponseCode;
                                                result.CodeDescription = verificacionVenta.verifyResponseMessage;

                                                return result;
                                            }

                                        }
                                        else
                                        {
                                            // Devolver mensaje de error
                                            result.CodeDescription = verificacionVenta.VerifyResponseCode;
                                            result.CodeDescription = verificacionVenta.verifyResponseMessage;

                                            return result;
                                        }

                                    }

                                    // La primera vuelta ya responde el servicio de verificacion
                                    if (verificacionVenta.VerifyResponseCode == "000")
                                    {
                                        // Si la venta no fue encontrada, disparar otra autorizacion de venta
                                        if (verificacionVenta.TransactionResponseCode == "100" && contador <= 1)
                                        {
                                            // Se debe de volvver a lanzar la venta
                                            contador = contador + 1;
                                            goto IniciarVentaUSD;

                                        }

                                        // Si la venta no fue encontrada y es la segunda vez, finalizar la venta
                                        if (verificacionVenta.TransactionResponseCode == "100" && contador == 2)
                                        {
                                            //Finalizar venta
                                            result.CodeDescription = verificacionVenta.TransactionResponseMessage;

                                            return result;

                                        }

                                        // Si la venta fue encontrada, recuperar folio autorizacion
                                        if (verificacionVenta.TransactionResponseCode == "000")
                                        {
                                            // Recuperar el numero de autorizacion de la venta
                                            finalizarVentaRequest.FormasPagoUtilizadas[i].Autorizacion = verificacionVenta.AuthoNumber;
                                        }
                                    }

                                }

                                if (saleResponse.ResponseCode == "102")
                                {
                                    verificacionVenta = null;
                                    verificacionVenta = administracionTipoCambio.VerifySale(finalizarVentaRequest.FolioVenta, estampaTiempo, token.CodeStore.ToString());

                                    // 
                                    // Si primera verifiacion es erronea, volver a validar
                                    if (verificacionVenta.VerifyResponseCode == "102")
                                    {
                                        verificacionVenta = null;
                                        verificacionVenta = administracionTipoCambio.VerifySale(finalizarVentaRequest.FolioVenta, estampaTiempo, token.CodeStore.ToString());

                                        // Si servicio no responde o se mantiene el error, cancelar venta
                                        if (verificacionVenta.VerifyResponseCode == "-111")
                                        {
                                            // Cancelar la venta
                                            result.CodeDescription = "-111";
                                            result.CodeDescription = "El servicio no se encuentra disponible. Intentar con otra forma de pago";

                                            return result;
                                        }

                                        // Si servicio no responde o se mantiene el error, cancelar venta
                                        if (verificacionVenta.VerifyResponseCode == "102")
                                        {
                                            // Cancelar la venta
                                            result.CodeDescription = "-111";
                                            result.CodeDescription = $"Codigo recibido: {verificacionVenta.VerifyResponseCode}, en segunda ejecucion. {verificacionVenta.verifyResponseMessage} Intente con otra forma de pago";

                                            return result;
                                        }

                                        // El servicio ya responde
                                        if (verificacionVenta.VerifyResponseCode == "000")
                                        {
                                            // Validar si encontro el ticket
                                            if (verificacionVenta.TransactionResponseCode == "100" && contador == 1)
                                            {
                                                //Enviar de nuevo la venta
                                                contador = contador + 1;
                                                goto IniciarVentaUSD;
                                            }
                                            if (verificacionVenta.TransactionResponseCode == "000")
                                            {
                                                finalizarVentaRequest.FormasPagoUtilizadas[i].Autorizacion = verificacionVenta.AuthoNumber;
                                            }
                                            else
                                            {
                                                // Devolver mensaje de error
                                                result.CodeDescription = verificacionVenta.VerifyResponseCode;
                                                result.CodeDescription = verificacionVenta.verifyResponseMessage;

                                                return result;
                                            }

                                        }
                                        else
                                        {
                                            // Devolver mensaje de error
                                            result.CodeDescription = verificacionVenta.VerifyResponseCode;
                                            result.CodeDescription = verificacionVenta.verifyResponseMessage;

                                            return result;
                                        }
                                    }
                                }

                                if (saleResponse.ResponseCode != "000" && saleResponse.ResponseCode != "-111")
                                {
                                    result.CodeDescription = saleResponse.responseMessage;

                                    return result;
                                }

                                if (saleResponse.ResponseCode == "000")
                                {
                                    //Pasar el folio de autorizacion
                                    finalizarVentaRequest.FormasPagoUtilizadas[i].Autorizacion = saleResponse.AuthoNumber;
                                }
                            }
                        }
                    }

                    // FIN DE LAS VALIDACIONES

                    result = repository.FinalizarVenta(token.CodeStore, token.CodeBox, token.CodeEmployee, finalizarVentaRequest, "REGULAR");

                    // Procesamos promociones que generan Cupones                    
                    CuponPromocionalVenta[] cuponesPromocionalesEncontrados = descuentosPromocionesRepository.ProcesarPromocionesCupones(finalizarVentaRequest.cabeceraVentaRequest.FolioOperacion, token.CodeStore, token.CodeBox, finalizarVentaRequest.cabeceraVentaRequest.NivelLealtad, finalizarVentaRequest.cabeceraVentaRequest.PrimeraCompraLealtad, (int)finalizarVentaRequest.cabeceraVentaRequest.CodigoClienteLealtad);
                    if ((int)finalizarVentaRequest.cabeceraVentaRequest.CodigoClienteLealtad == 0)
                    {
                        foreach (var cupon in cuponesPromocionalesEncontrados)
                        {
                            CuponPersistirResponse cuponPersistirResponse = new CuponPersistirResponse();
                            cuponPersistirResponse = repository.PersistirCuponPromocionalGenerado(cupon);
                            mensajeVentaFinalizada += "Cupón Generado: " + cupon.MensajeCupon + " $" + cupon.ImporteDescuento + ". ";
                        }
                    }


                    // ACUMULACION DE PUNTOS DE LEALTAD
                    if ((int)finalizarVentaRequest.cabeceraVentaRequest.CodigoClienteLealtad > 0)
                    {
                        AcumularPuntosDescuentosRequest x = new AcumularPuntosDescuentosRequest();
                        FinlagBusiness v = new FinlagBusiness(token);

                        foreach (var cupon in cuponesPromocionalesEncontrados)
                        {
                            x.ssFecha = finalizarVentaRequest.cabeceraVentaRequest.FechaLealtad;
                            x.iiCodigoCliente = (int)finalizarVentaRequest.cabeceraVentaRequest.CodigoClienteLealtad;
                            x.iiCodigoTienda = token.CodeStore;
                            x.iiCodigoCaja = token.CodeBox;
                            x.iiCodigoEmpleado = token.CodeEmployee;
                            x.ssFolioVenta = finalizarVentaRequest.cabeceraVentaRequest.FolioOperacion;
                            x.iiCodigoPromocion = cupon.CodigoPromocionAplicado;

                            x.ddVentaSinIVA = cupon.MercanciaSinIva;
                            x.ddIVA = cupon.Iva;

                            x.iiTransaccion = cupon.Transaccion;
                            x.iiCodigoTipoPuntos = 1;

                            x.ddPuntosAcumulados = 0;
                            x.ddImporteDescuento = 0;

                            if (cupon.TipoAcumulacion == "D")
                            {
                                x.ddImporteDescuento = Convert.ToDouble(cupon.ImporteDescuento);
                            }
                            else if (cupon.TipoAcumulacion == "P")
                            {
                                x.ddPuntosAcumulados = Convert.ToDouble(cupon.ImporteDescuento);
                            }

                            v.AcumulaPuntosDescuentoLealtad(x);
                        }
                    }


                    // Procesamos descuentos por mercancía dañada o picos de mercancía
                    descuentoMercanciaDaniadaBusiness.ProcesarDescuentosExternosPicosMercancia(finalizarVentaRequest.FolioVenta);

                    // ******************************************* INVOCAR A MÉTODOS DE ACUERDO A CADA TIPO DE VENTA FINALIZADA


                    // TIPO DE VENTA/DEVOLUCIÓN REGULAR CONSIDERANDO TARJETAS DE REGALO
                    if (result.CodeNumber.Equals("332") && (finalizarVentaRequest.TipoCabeceraVenta == "1"))
                    {
                        OperationResponse respuesta = this.ActivarTarjetaRegalo(finalizarVentaRequest);
                        if (respuesta.CodeNumber == "1")
                        {
                            result.CodeDescription = mensajeVentaFinalizada;
                            ImprimeTicketsMM imprimeTicketsMM = new ImprimeTicketsMM(token);
                            imprimeTicketsMM.PrintTicket(finalizarVentaRequest.FolioVenta, false);
                            scope.Complete();
                        }
                        else
                        {
                            result.CodeDescription = respuesta.CodeDescription;
                        }
                    }

                    // TIPO DE VENTA PAGO TCMM
                    if (result.CodeNumber.Equals("332") && finalizarVentaRequest.TipoCabeceraVenta == "46")
                    {
                        OperationResponse respuesta = this.PagoTCMM(finalizarVentaRequest);
                        if (respuesta.CodeNumber == "1")
                        {
                            result.CodeDescription = mensajeVentaFinalizada;
                            ImprimeTicketsMM imprimeTicketsMM = new ImprimeTicketsMM(token);
                            imprimeTicketsMM.PrintTicket(finalizarVentaRequest.FolioVenta, false);
                            scope.Complete();
                        }
                        else
                        {
                            result.CodeDescription = respuesta.CodeDescription;
                        }
                    }

                    // TIPO DE VENTA MAYORISTA CONSIDERANDO TARJETAS DE REGALO
                    else if (result.CodeNumber.Equals("332") && finalizarVentaRequest.TipoCabeceraVenta == "2")
                    {
                        var respuesta = this.PagoVentaCreditoMayorista(finalizarVentaRequest);
                        if (respuesta.CodeNumber == "1")
                        {
                            // Se procesan las tarjetas de regalo en caso de aplicar
                            OperationResponse operationResponse = this.ActivarTarjetaRegalo(finalizarVentaRequest);
                            if (operationResponse.CodeNumber == "1")
                            {
                                result.CodeDescription = mensajeVentaFinalizada;
                                ImprimeTicketsMM imprimeTicketsMM = new ImprimeTicketsMM(token);
                                imprimeTicketsMM.PrintTicket(finalizarVentaRequest.FolioVenta, false);
                                scope.Complete();
                            }
                            else
                            {
                                result.CodeDescription = respuesta.CodeDescription;
                            }
                        }
                        else
                        {
                            result.CodeDescription = respuesta.CodeDescription;
                        }
                    }

                    // TIPO DE VENTA EMPLEADO CONSIDERANDO TARJETAS DE REGALO
                    else if (result.CodeNumber.Equals("332") && finalizarVentaRequest.TipoCabeceraVenta == "4")
                    {
                        OperationResponse respuesta = this.ActivarTarjetaRegalo(finalizarVentaRequest);
                        if (respuesta.CodeNumber == "1")
                        {
                            result.CodeDescription = mensajeVentaFinalizada;
                            ImprimeTicketsMM imprimeTicketsMM = new ImprimeTicketsMM(token);
                            imprimeTicketsMM.PrintTicket(finalizarVentaRequest.FolioVenta, false);
                            scope.Complete();
                        }
                        else
                        {
                            result.CodeDescription = respuesta.CodeDescription;
                        }
                    }

                    // TIPO DE VENTA TIEMPO AIRE
                    else if (result.CodeNumber.Equals("332") && finalizarVentaRequest.TipoCabeceraVenta == "5")
                    {
                        var respuesta = this.TiempoAire(finalizarVentaRequest);
                        if (respuesta.CodeNumber == "1")
                        {
                            ImprimeTicketsMM imprimeTicketsMM = new ImprimeTicketsMM(token);
                            imprimeTicketsMM.PrintTicket(finalizarVentaRequest.FolioVenta, false);
                            scope.Complete();
                        }
                        else
                        {
                            result.CodeNumber = respuesta.CodeNumber;
                            result.CodeDescription = respuesta.CodeDescription;
                        }
                    }

                    // TIPO DE VENTA PAGO DE SERVICIOS
                    else if (result.CodeNumber.Equals("332") && finalizarVentaRequest.TipoCabeceraVenta == "6")
                    {
                        var respuesta = this.PagoServicios(finalizarVentaRequest);
                        if (respuesta.CodeNumber == "1")
                        {
                            ImprimeTicketsMM imprimeTicketsMM = new ImprimeTicketsMM(token);
                            imprimeTicketsMM.PrintTicket(finalizarVentaRequest.FolioVenta, false);
                            scope.Complete();
                        }
                        else
                        {
                            result.CodeNumber = respuesta.CodeNumber;
                            result.CodeDescription = respuesta.CodeDescription;
                        }
                    }

                    // OCG:  TIPO DE VENTA PAGO WEB
                    else if (result.CodeNumber.Equals("332") && finalizarVentaRequest.TipoCabeceraVenta == "48")
                    {
                        var respuesta = this.PagoWeb(finalizarVentaRequest);
                        if (respuesta.CodeNumber == "1")
                        {
                            ImprimeTicketsMM imprimeTicketsMM = new ImprimeTicketsMM(token);
                            imprimeTicketsMM.PrintTicket(finalizarVentaRequest.FolioVenta, false);
                            scope.Complete();
                        }
                        else
                        {
                            result.CodeNumber = respuesta.CodeNumber;
                            result.CodeDescription = respuesta.CodeDescription;
                        }
                    }

                    // SE TRATA DE UNA DEVOLUCIÓN CONSIDERANDO TARJETAS DE REGALO
                    else if (result.CodeNumber.Equals("399"))
                    {
                        OperationResponse respuesta = this.ActivarTarjetaRegalo(finalizarVentaRequest);
                        if (respuesta.CodeNumber == "1")
                        {
                            result.CodeDescription = mensajeVentaFinalizada;
                            ImprimeTicketsMM imprimeTicketsMM = new ImprimeTicketsMM(token);
                            imprimeTicketsMM.PrintTicket(finalizarVentaRequest.FolioVenta, false);
                            scope.Complete();
                        }
                        else
                        {
                            result.CodeDescription = respuesta.CodeDescription;
                        }
                    }

                }

                return result;
            });
        }

        /// <summary>
        /// Iniciar la solicitud de autorizacion
        /// </summary>
        /// <param name="solicitudAutorizacionDescuentoRequest">Objeto de peticion de la venta a finalizar</param>
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> SolicitudAutorizacionDescuento(SolicitudAutorizacionDescuentoRequest solicitudAutorizacionDescuentoRequest, int Tienda, int Caja)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return SolicitudAutorizacionDescuentoInternal(solicitudAutorizacionDescuentoRequest, Tienda, Caja);
            });
        }

        private ResponseBussiness<OperationResponse> SolicitudAutorizacionDescuentoInternal(SolicitudAutorizacionDescuentoRequest solicitudAutorizacionDescuentoRequest, int Tienda, int Caja)
        {
            return tryCatch.SafeExecutor(() =>
            {
                OperationResponse result = new OperationResponse();

                MailMessage email = new MailMessage();
                email.To.Add(new MailAddress("omar.cervantes@milano.com"));
                email.From = new MailAddress("omar.cervantes@milano.com");
                email.Subject = $"VALIDAR DESCUENTO TDA: {Tienda}, CAJA: {Caja}, Fecha {solicitudAutorizacionDescuentoRequest.Fecha} ";
                //email.Body = "<b> Se requiere de su autorizacion para aplicar o denegar el siguiente descuento solicitado <b> <br><br>" +

                //$"<b>Sku: </b> {solicitudAutorizacionDescuentoRequest.Sku} <br>"+
                //$"<b>Precio original: </b> {solicitudAutorizacionDescuentoRequest.Precio} <br>" +
                //$"<b>Descuento solicitado: </b> {solicitudAutorizacionDescuentoRequest.Descuento} <br>" +
                //$"<b>Precio final: </b> {solicitudAutorizacionDescuentoRequest.PrecioFinal} <br>" +
                //$"<b>Descripción descuento: </b> {solicitudAutorizacionDescuentoRequest.Descripcion} <br>" +
                //$"<br> <br>" +
                //$"<b>AUTORIZAR DESCUENTO: </b> <a href=\"https://www.w3schools.com\">Haga click aqui para <b>autorizar<\b> el descuento</a> <br>" +
                //$"<br> <br>" +
                //$"<br> <br>" +
                //$"<b>DENEGAR DESCUENTO: </b> <a href=\"https://www.w3schools.com\">Haga click aqui para <b>denegar<\b> el descuento</a> <br>";

                email.IsBodyHtml = true;
                email.Priority = MailPriority.Normal;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "mail.melody-milano.com.mx";
                smtp.Port = 25;
                smtp.EnableSsl = false;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("omar.cervantes@milano.com", "VenAMilano3131");

                string output = null;

                smtp.Send(email);
                email.Dispose();
                output = "Corre electrónico fue enviado satisfactoriamente.";

                return result;
            });
        }


        private OperationResponse PagoVentaCreditoMayorista(FinalizarVentaRequest request)
        {
            OperationResponse response = new OperationResponse();
            response.CodeNumber = "1";
            VentaResponse venta = this.repository.ObtenerVentaPorFolio(request.FolioVenta, 0);
            foreach (LineaTicket linea in venta.Lineas)
            {
                if (linea.TipoDetalleVenta == "43")
                {
                    PagoCreditoMayoristaRequest pago = new PagoCreditoMayoristaRequest();
                    pago.CodigoMayorista = venta.CodigoMayorista;
                    pago.FolioOperacionAsociada = venta.FolioVenta;
                    pago.ImportePago = linea.Articulo.PrecioConImpuestos;
                    response = new MayoristasBusiness(this.token).PagoCreditoMayorista(pago, venta.NumeroTransaccion);
                    response.CodeNumber = "1";
                }
            }
            return response;
        }

        private OperationResponse PagoTCMM(FinalizarVentaRequest request)
        {
            OperationResponse response = new OperationResponse();
            response.CodeNumber = "1";
            VentaResponse venta = this.repository.ObtenerVentaPorFolio(request.FolioVenta, 0);
            foreach (LineaTicket linea in venta.Lineas)
            {
                if (linea.TipoDetalleVenta == "46")
                {
                    PagoTCMMRequest pago = new PagoTCMMRequest();
                    pago.ModoEntrada = 12;
                    pago.NumeroCaja = token.CodeBox;
                    pago.NumeroTienda = token.CodeStore;
                    pago.Transaccion = venta.NumeroTransaccion;
                    pago.Importe = linea.Articulo.PrecioConImpuestos;
                    pago.NumeroTarjeta = linea.Articulo.InformacionPagoTCMM.NumeroTarjeta;
                    response = new MelodyMilanoBusiness(this.token).RealizarPago(pago);
                    if (response.CodeNumber == "1")
                    {
                        OperationResponse operationResponseAutoriacion = this.repository.RegistrarAutorizacionPagoTCMM(request.FolioVenta, pago.NumeroTarjeta, response.CodeDescription);
                    }
                }
            }
            return response;
        }

        //OCG: Finalizar el pago web
        private OperationResponse PagoWeb(FinalizarVentaRequest request)
        {
            OperationResponse response = new OperationResponse();
            response.CodeNumber = "1";
            VentaResponse venta = this.repository.ObtenerVentaPorFolio(request.FolioVenta, 0);

            foreach (LineaTicket linea in venta.Lineas)
            {
                if (linea.TipoDetalleVenta == "48")
                {
                    ValidarFolioWebResponse validarFolioWebResponse = new ValidarFolioWebResponse();
                    validarFolioWebResponse = repository.RecuperarOrderIdTransactionID(request.FolioVenta);
                    if (validarFolioWebResponse.ErrorCode == 0)
                        response.CodeNumber = "1";
                    else
                        response.CodeNumber = "0";
                }
            }
            return response;
        }

        private OperationResponse PagoServicios(FinalizarVentaRequest request)
        {
            OperationResponse response = new OperationResponse();
            response.CodeNumber = "1";
            VentaResponse venta = this.repository.ObtenerVentaPorFolio(request.FolioVenta, 0);
            PagoServiciosRequest pago = new PagoServiciosRequest();
            pago.Cuenta = venta.Lineas[0].Articulo.InformacionProveedorExternoAsociadaPS.Cuenta;

            pago.SkuCodePagoServicio = venta.Lineas[0].Articulo.InformacionProveedorExternoAsociadaPS.SkuCompania;
            pago.SkuCode = venta.Lineas[0].Articulo.Sku;
            pago.InfoAdicional = venta.Lineas[0].Articulo.InformacionProveedorExternoAsociadaPS.InfoAdicional;

            ResponseBussiness<OperationResponse> resultTA = new AdministracionPagoServiciosBusiness(this.token).PagoServicio(pago, float.Parse(venta.Lineas[0].Articulo.PrecioConImpuestos.ToString()), request.FolioVenta);

            if (resultTA.Data.CodeNumber != "1")
            {
                response.CodeNumber = resultTA.Data.CodeNumber;
                response.CodeDescription = resultTA.Data.CodeDescription;
            }
            return response;

        }

        private OperationResponse TiempoAire(FinalizarVentaRequest request)
        {
            OperationResponse response = new OperationResponse();
            response.CodeNumber = "1";
            VentaResponse venta = this.repository.ObtenerVentaPorFolio(request.FolioVenta, 0);
            TiempoAireRequest tiempoAireRequest = new TiempoAireRequest();
            tiempoAireRequest.Monto = float.Parse(venta.Lineas[0].ImporteVentaLineaNeto.ToString());
            tiempoAireRequest.Telefono = venta.Lineas[0].Articulo.InformacionProveedorExternoTA.NumeroTelefonico;
            tiempoAireRequest.SkuCode = venta.Lineas[0].Articulo.InformacionProveedorExternoTA.SkuCompania;
            ResponseBussiness<OperationResponse> resultTA = new TiempoAireBusiness(this.token).AddTiempoAire(tiempoAireRequest, venta.Lineas[0].Articulo.Sku, venta.FolioVenta);

            if (resultTA.Data.CodeNumber == "1")
            {
                repository.RegistrarRecargaTelefonicaExitosa(request.FolioVenta, this.token.CodeBox, this.token.CodeStore, tiempoAireRequest.Telefono, int.Parse(tiempoAireRequest.Monto.ToString()), resultTA.Data.CodeDescription);

            }
            else
            {
                response.CodeNumber = resultTA.Data.CodeNumber;
                response.CodeDescription = resultTA.Data.CodeDescription;
            }

            return response;
        }

        private OperationResponse ActivarTarjetaRegalo(FinalizarVentaRequest request)
        {
            OperationResponse response = new OperationResponse();
            response.CodeNumber = "1";
            TarjetaRegalosBusiness business = new TarjetaRegalosBusiness(this.token);
            foreach (InformacionFoliosTarjeta informacion in request.InformacionFoliosTarjeta)
            {
                var respuesta = business.ActivarTarjeta(this.token.CodeEmployee, informacion.FolioTarjeta.ToString(), request.FolioVenta);
                if (response.CodeNumber == "0")
                {
                    response.CodeDescription = respuesta.Data.CodeDescription;
                }
            }
            return response;
        }

        /// <summary>
        /// Anular una Venta
        /// </summary>
        /// <param name="anularTotalizarVentaRequest">Folio de venta y razón</param>
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> AnularTotalizarVenta(AnularTotalizarVentaRequest anularTotalizarVentaRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                OperationResponse operationResponse = repository.AnularVenta(anularTotalizarVentaRequest, this.token);
                ImprimeTicketsMM imprimeTicketsMM = new ImprimeTicketsMM(token);
                imprimeTicketsMM.PrintTicket(anularTotalizarVentaRequest.FolioVenta, false);
                return operationResponse;
            });
        }

        /// <summary>
        /// Eliminamos la linea de mayorista
        /// </summary>
        /// <param name="cabecera"></param>
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> EliminarLineaMayorista(CabeceraVentaRequest cabecera)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.EliminarLineaMayorista(cabecera.FolioOperacion, this.token.CodeStore, this.token.CodeBox);
            });
        }

        /// <summary>
        /// Post-anular una Venta
        /// </summary>
        /// <param name="postAnularVentaRequest">Folio de venta y razón</param>
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> PostAnularVenta(PostAnularVentaRequest postAnularVentaRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                AnularTotalizarVentaRequest anularTotalizarVentaRequest = new AnularTotalizarVentaRequest();
                anularTotalizarVentaRequest.FolioVenta = postAnularVentaRequest.FolioVenta;
                anularTotalizarVentaRequest.CodigoRazon = postAnularVentaRequest.CodigoRazon;
                OperationResponse operationResponse = repository.AnularVenta(anularTotalizarVentaRequest, this.token);
                ImprimeTicketsMM imprimeTicketsMM = new ImprimeTicketsMM(token);
                imprimeTicketsMM.PrintTicket(postAnularVentaRequest.FolioVenta, false);
                return operationResponse;
            });
        }

        /// <summary>
        /// Suspender una Venta
        /// </summary>
        /// <param name="suspenderVentaRequest">Objeto de peticion de la venta a suspender</param>
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> SuspenderVenta(SuspenderVentaRequest suspenderVentaRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                OperationResponse operationResponse = repository.SuspenderVenta(suspenderVentaRequest, token.CodeStore, token.CodeBox, token.CodeEmployee);
                ImprimeTicketsMM imprimeTicketsMM = new ImprimeTicketsMM(token);
                imprimeTicketsMM.PrintTicket(suspenderVentaRequest.cabeceraVentaRequest.FolioOperacion, false);
                return operationResponse;
            });
        }

        /// <summary>
        /// Busqueda de ventas por folio y fechas
        /// </summary>
        /// <param name="busquedaTransaccionRequest">Parámetros de búsqueda para las transacciones</param>
        /// <returns>Lista de ventas que cumplen con el criterio</returns>
        public ResponseBussiness<BusquedaTransaccionResponse[]> BuscarVentasPorFolioFecha(BusquedaTransaccionRequest busquedaTransaccionRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.ObtenerVentas(busquedaTransaccionRequest);
            });
        }

        /// <summary>
        /// Solicita autorización de descuento
        /// </summary>
        /// <param name="solicitudAutorizacionDescuentoRequest"></param>
        /// <returns></returns>
        public ResponseBussiness<SolicitudAutorizacionDescuentoResponse> SolicitudAutorizacion(SolicitudAutorizacionDescuentoRequest solicitudAutorizacionDescuentoRequest)
        {
            ProxyInfoDescuento.InfoDescuentoSoapClient proxy2;
            SolicitudAutorizacionDescuentoRequest complement = new SolicitudAutorizacionDescuentoRequest();
            proxy2 = new ProxyInfoDescuento.InfoDescuentoSoapClient();
            string url;

            url = repository.EndPointUrl(24);  // InfoDescuento.asmx
            proxy2.Endpoint.Address = new System.ServiceModel.EndpointAddress(url);

            //proxy2.Endpoint.Address = new System.ServiceModel.EndpointAddress("http://localhost:42758/InfoDescuento.asmx");
            //proxy2.Endpoint.Address = new System.ServiceModel.EndpointAddress("http://172.16.50.91/wspostest/infodescuento.asmx");

            DateTime fecha;

            complement = repository.ObtenInfoDescuentoSku(solicitudAutorizacionDescuentoRequest);

            solicitudAutorizacionDescuentoRequest.CodigoTienda = complement.CodigoTienda;
            solicitudAutorizacionDescuentoRequest.CodigoCaja = complement.CodigoCaja;
            solicitudAutorizacionDescuentoRequest.FolioVenta = complement.FolioVenta;
            fecha = ((DateTime)((complement.Fecha == null) ? DateTime.Now : complement.Fecha));

            SolicitudAutorizacionDescuentoResponse solicitudAutorizacionDescuentoResponse = new SolicitudAutorizacionDescuentoResponse();
            //return tryCatch.SafeExecutor(() =>
            //{
            ProxyInfoDescuento.WsPosResponseModel info = proxy2.getAutorizationV2(
                solicitudAutorizacionDescuentoRequest.CodigoTienda,
                solicitudAutorizacionDescuentoRequest.FolioVenta,
                solicitudAutorizacionDescuentoRequest.MontoVenta,
                solicitudAutorizacionDescuentoRequest.CodigoCaja,
                fecha,
                solicitudAutorizacionDescuentoRequest.CodigoRazonDescuento,
                solicitudAutorizacionDescuentoRequest.OpcionDescuento,
                solicitudAutorizacionDescuentoRequest.TipoDescuento,
                solicitudAutorizacionDescuentoRequest.MontoDescuento,
                solicitudAutorizacionDescuentoRequest.Linea
                , "");

            //if (info.status == 200)
            //{
            solicitudAutorizacionDescuentoResponse.Mensaje = info.mensaje;
            solicitudAutorizacionDescuentoResponse.CodeNumber = info.codeNumber;
            // }

            return solicitudAutorizacionDescuentoResponse;
            //});
        }














        /// <summary>
        /// Busqueda de venta por folio
        /// </summary>
        /// <param name="folio"></param>
        /// <param name="esDevolucion"></param>
        /// <returns>Venta correspondiente</returns>
        public ResponseBussiness<VentaResponse> BuscarVentaPorFolio(string folio, int esDevolucion)
        {
            return tryCatch.SafeExecutor(() =>
            {
                VentaResponse ventaResponse = repository.ObtenerVentaPorFolio(folio, esDevolucion);
                if (ventaResponse.NumeroNominaVentaEmpleado != 0)
                {
                    ventaResponse.InformacionEmpleadoMilano = new AdministracionVentaEmpleadoBusiness().Buscar(ventaResponse.NumeroNominaVentaEmpleado.ToString(), this.token.CodeStore.ToString(), this.token.CodeBox.ToString());
                }
                else if (ventaResponse.CodigoMayorista != 0)
                {
                    BusquedaMayoristaRequest busquedaMayorista = new BusquedaMayoristaRequest();
                    busquedaMayorista.CodigoMayorista = ventaResponse.CodigoMayorista;
                    busquedaMayorista.Nombre = "";
                    busquedaMayorista.SoloActivos = true;
                    ventaResponse.InformacionMayorista = new MayoristasBusiness(this.token).BusquedaMayorista(busquedaMayorista);
                }
                if (ventaResponse.CodigoEmpleadoVendedor > 0)
                {
                    EmployeeRequest employeeRequest = new EmployeeRequest();
                    employeeRequest.Code = ventaResponse.CodigoEmpleadoVendedor;
                    employeeRequest.Name = "";
                    ventaResponse.InformacionEmpleadoVendedor = new EmployeeBusiness(this.token).SearchEmployee(employeeRequest).Data[0];
                }
                return ventaResponse;
            });
        }

        /// <summary>
        /// Valida la existencia del FolioWeb para ser pagado
        /// </summary>
        /// <param name="folio">Número de folio Shopify</param>
        /// <returns></returns>
        public ResponseBussiness<ValidarFolioWebResponse> ValidarFolioWeb(string folio)
        {
            string url = $"https://credito.milano.com.mx/apishopify/api/Order/PaymentInf/{folio.ToUpper()}";//ConfigurationManager.AppSettings["url"];
            string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes("usrprodws" + ":" + "M1l4n0Pr0dWS*07"));

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add("Authorization", "Basic " + svcCredentials);
            httpWebRequest.Headers.Add("UserName", "MMPOSCRED");
            httpWebRequest.Headers.Add("Password", "TriDXeLLP8F2Edlx1aZjf+gR2b0UBcXouKG0Ykrmopw=");

            ValidarFolioWebResponse resp = new ValidarFolioWebResponse();
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                resp = JsonConvert.DeserializeObject<ValidarFolioWebResponse>(streamReader.ReadToEnd());
            }

            return resp;

        }

    }
}
