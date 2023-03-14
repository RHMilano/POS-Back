using Milano.BackEnd.Repository.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.Sales;
using Milano.BackEnd.Dto.MM;
using Milano.BackEnd.Repository;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.Transactions;
using Project.Services.Utils;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Repository.MM;
using Milano.BackEnd.Business.ImpresionMM;
using Milano.BackEnd.Business.MM;
using Milano.BackEnd.Dto.MilanoEntities;

namespace Milano.BackEnd.Business.MM
{
    /// <summary>
    /// Clase de negocios correspondiente a operaciones TCMM
    /// </summary>
    public class MelodyMilanoBusiness : BaseBusiness
    {

        /// <summary>
        /// Atributo del token usuario
        /// </summary>
        protected TokenDto token;
        CredencialesServicioExterno credenciales;
        PaymentProcessingRepository repository;
        DescuentosPromocionesRepository descuentosPromocionesRepository;
        MelodyMilanoRepository melodyMilanoRepository;

        /// <summary>
        /// Constructor por default de la clase
        /// </summary>
        public MelodyMilanoBusiness(TokenDto token)
        {
            this.repository = new PaymentProcessingRepository();
            this.melodyMilanoRepository = new MelodyMilanoRepository();
            this.token = token;
            {
                credenciales = new CredencialesServicioExterno();
                credenciales = new InformacionServiciosExternosBusiness().ObtenerCredencialesConsultaTCMM();
            }
        }

        /// <summary>
        /// Método para consultar la información de TCMM
        /// </summary>
        /// <param name="informacionTCMMRequest">Objeto petición con información de TCMM asociada</param>
        /// <param name="imprimirTicket">Parámetro que indica si el Ticket debe imprimirse</param>
        /// <returns></returns>
        public ResponseBussiness<InformacionTCMMResponse> ConsultarInformacionTCMM(InformacionTCMMRequest informacionTCMMRequest, Boolean imprimirTicket)
        {
            return tryCatch.SafeExecutor(() =>
            {
                InformacionTCMMResponse informacion = new InformacionTCMMResponse();
                Inspector inspector = new Inspector();
                InfoService infoService = new InfoService();
                InformacionServiciosExternosBusiness informacionServiciosExternosBusiness = new InformacionServiciosExternosBusiness();
                //NOTA: Unicamente se manda a llamar la URL ya que en el web.config se tiene como keys, y no se esta conectando el servicio mediante vs
                infoService = informacionServiciosExternosBusiness.ObtenerCadenaServicioExterno(6);
                var cadenaURL = infoService.UrlService;
                System.Net.HttpWebRequest webrequest = (HttpWebRequest)System.Net.WebRequest.Create(cadenaURL);
                //webrequest.Headers.Add ("Authorization","Basic dXNybW13czpNMWw0bjAqMjAxOA==");
                webrequest.Headers.Add("Authorization", credenciales.Licence);
                webrequest.Method = "POST";
                webrequest.ContentType = "application/json";

                using (var streamWriter = new StreamWriter(webrequest.GetRequestStream()))
                {
                    string json = "{\"User\":\"" + credenciales.UserName + "\"," +
                                  "\"Pwd\":\"" + credenciales.Password + "\"," +
                                    "\"CardId\":\"" + informacionTCMMRequest.NumeroTarjeta + "\"}";
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)webrequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    dynamic resultadoTemporal = JsonConvert.DeserializeObject(result);
                    
                    informacion.CodigoRespuestaTCMM = new CodigoRespuestaTCMM();
                    informacion.CodigoRespuestaTCMM.CodigoRespuesta = resultadoTemporal.ErrorCode;
                    informacion.CodigoRespuestaTCMM.MensajeRetorno = resultadoTemporal.Message;
                   
                    if (informacion.CodigoRespuestaTCMM.CodigoRespuesta == "0")
                    {
                        informacion.SaldoEnLinea = resultadoTemporal.Response.Balance;
                        informacion.SaldoAlCorte = resultadoTemporal.Response.LastBalance;
                        if (resultadoTemporal.Response.PaymentDate.ToString() != string.Empty)
                        {
                            informacion.FechaLimitePago = DateTime.Parse(resultadoTemporal.Response.PaymentDate.ToString()).ToString();
                        }
                        informacion.PagoMinimo = resultadoTemporal.Response.PeriodAmount;
                        informacion.SaldoEnPuntos = resultadoTemporal.Response.BalancePoints;
                        informacion.EquivalenteEnPuntos = resultadoTemporal.Response.BalancePointsEq;
                        informacion.PuntosAcumuladosUltimoCorte = resultadoTemporal.Response.PeriodPoints;
                        informacion.MontoPagoSinIntereses = resultadoTemporal.Response.NoInterestPaymentAmount;
                        // Realizar el proceso de Truncado
                        informacion.SaldoEnLinea = inspector.TruncarValor(informacion.SaldoEnLinea);
                        informacion.SaldoAlCorte = inspector.TruncarValor(informacion.SaldoAlCorte);
                        informacion.PagoMinimo = inspector.TruncarValor(informacion.PagoMinimo);
                        informacion.EquivalenteEnPuntos = inspector.TruncarValor(informacion.EquivalenteEnPuntos);
                        informacion.MontoPagoSinIntereses = inspector.TruncarValor(informacion.MontoPagoSinIntereses);
                    }

                }

                if (imprimirTicket)
                {
                    // Persistencia de Consulta de Saldo
                    melodyMilanoRepository.PersistirConsultarInformacionTCMM(token.CodeBox, token.CodeStore, token.CodeEmployee, informacion.SaldoEnLinea, informacion.SaldoAlCorte, informacion.FechaLimitePago, informacion.PagoMinimo, informacion.SaldoEnPuntos, informacion.EquivalenteEnPuntos, informacion.PuntosAcumuladosUltimoCorte);

                    //Impresión de Tickets de Consulta de Saldo 
                    PrintTicketConsultaSaldo printTicketConsultaSaldo = new PrintTicketConsultaSaldo(token);
                    OperationResponse operationResponse = new OperationResponse();
                    OperationResponse operation = printTicketConsultaSaldo.PrintNow(informacionTCMMRequest.NumeroTarjeta);
                }

                return informacion;
            });
        }

        /// <summary>
        /// Valida primera compra de TCMM
        /// </summary>
        /// <param name="informacionTCMMRequest"></param>
        /// <returns></returns>
        public ResponseBussiness<PrimeraCompraResponse> ValidarPrimeraCompra(PlanesFinanciamientoRequest informacionTCMMRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                PrimeraCompraResponse informacion = new PrimeraCompraResponse();
                InfoService infoService = new InfoService();
                InformacionServiciosExternosBusiness informacionServiciosExternosBusiness = new InformacionServiciosExternosBusiness();
                //NOTA: Unicamente se manda a llamar la URL ya que en el web.config se tiene como keys, y no se esta conectando el servicio mediante vs
                infoService = informacionServiciosExternosBusiness.ObtenerCadenaServicioExterno(9);
                var cadenaURL = infoService.UrlService;
                System.Net.HttpWebRequest webrequest = (HttpWebRequest)System.Net.WebRequest.Create(cadenaURL);//ConfigurationManager.AppSettings["servicioTxCMMPrimerCompra"].ToString());
                //webrequest.Headers.Add ("Authorization","Basic dXNybW13czpNMWw0bjAqMjAxOA==");
                webrequest.Headers.Add("Authorization", credenciales.Licence);
                webrequest.Method = "POST";
                webrequest.ContentType = "application/json";
                using (var streamWriter = new StreamWriter(webrequest.GetRequestStream()))
                {
                    string json = "{\"User\":\"" + credenciales.UserName + "\"," +
                                  "\"Pwd\":\"" + credenciales.Password + "\"," +
                                    "\"CardId\":\"" + informacionTCMMRequest.NumeroTarjeta + "\"}";
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var httpResponse = (HttpWebResponse)webrequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    dynamic resultadoTemporal = JsonConvert.DeserializeObject(result);

                    informacion.CodigoError = resultadoTemporal.ErrorCode;
                    informacion.Mensaje = resultadoTemporal.Message;
                    informacion.Respuesta = resultadoTemporal.Response;

                }
                return informacion;
            });

        }

        /// <summary>
        /// Método para consultar los planes de financiamiento
        /// </summary>
        /// <param name="planesFinanciamientoRequest">Objeto petición con información de TCMM asociada</param>
        /// <returns></returns>
        public ResponseBussiness<PlanesFinanciamientoResponse> ConsultarPlanesFinanciamientoDescuentoPrimeraCompra(PlanesFinanciamientoRequest planesFinanciamientoRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                descuentosPromocionesRepository = new DescuentosPromocionesRepository();
                PlanesFinanciamientoResponse response = new PlanesFinanciamientoResponse();
                InfoService infoService = new InfoService();
                InformacionServiciosExternosBusiness informacionServiciosExternosBusiness = new InformacionServiciosExternosBusiness();
                //NOTA: Unicamente se manda a llamar la URL ya que en el web.config se tiene como keys, y no se esta conectando el servicio mediante vs
                infoService = informacionServiciosExternosBusiness.ObtenerCadenaServicioExterno(8);
                var cadenaURL = infoService.UrlService;
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(cadenaURL);//ConfigurationManager.AppSettings["servicioTxCMMEsquemas"]);
                httpWebRequest.Headers.Add("Authorization", credenciales.Licence);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json";
                var serializer = new Newtonsoft.Json.JsonSerializer();
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    using (var textWriter = new Newtonsoft.Json.JsonTextWriter(streamWriter))
                    {
                        serializer.Serialize(textWriter,
                            new
                            {
                                User = credenciales.UserName,
                                Pwd = credenciales.Password,
                                CardId = planesFinanciamientoRequest.NumeroTarjeta
                            });
                    }
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    JToken token = JObject.Parse(responseText);
                    response.CodigoRespuestaTCMM = new CodigoRespuestaTCMM();
                    response.CodigoRespuestaTCMM.CodigoRespuesta = Convert.ToString(token.SelectToken("ErrorCode"));
                    response.CodigoRespuestaTCMM.MensajeRetorno = Convert.ToString(token.SelectToken("Message"));

                    if (response.CodigoRespuestaTCMM.CodigoRespuesta == "0")
                    {
                        response.PlanesFinanciamiento = new List<PlanFinanciamientoResponse>();
                        var array = token.SelectToken("Response").Value<JArray>();

                        foreach (JObject jObject in array.Children<JObject>())
                        {
                            PlanFinanciamientoResponse planFinanciamiento = new PlanFinanciamientoResponse();
                            planFinanciamiento.Id = Convert.ToInt32(jObject.SelectToken("FinancingSchemeId"));
                            planFinanciamiento.Descripcion = Convert.ToString(jObject.SelectToken("Description"));
                            planFinanciamiento.Periodo = Convert.ToInt32(jObject.SelectToken("PaybackPeriod"));

                            response.PlanesFinanciamiento.Add(planFinanciamiento);
                        }
                    }
                }

                // Validar si existe promoción primera compra con TCMM                
                DescuentoPromocionalVenta[] descuentoPromocionalEncontrado = descuentosPromocionesRepository.ObtenerDescuentoMMPrimeraCompra(planesFinanciamientoRequest.FolioOperacionAsociada, token.CodeStore, token.CodeBox);
                if (descuentoPromocionalEncontrado != null && descuentoPromocionalEncontrado.Length > 0)
                {
                    // Si existe la promoción se valida si se trata de la primera compra TCMM
                    // Respuesta = T ES Primera compra
                    // Respuesta = F NO es primera compra
                    PrimeraCompraResponse validar = this.ValidarPrimeraCompra(planesFinanciamientoRequest).Data;
                    if (validar.Respuesta == "T")
                    {
                        response.DescuentoPromocionalPrimeraCompra = descuentoPromocionalEncontrado[0];
                    }
                }
                return response;

            });
        }

        /// <summary>
        /// Método para finalizar la compra por tarjeta de MM
        /// </summary>
        /// <param name="request">Objeto petición con información de TCMM asociada</param>
        /// <returns></returns>
        public ResponseBussiness<FinalizarCompraResponse> FinalizarCompraTCMM(FinalizarCompraRequest request)
        {
            //request.PlanFinanciamiento: ID del plan de financiamiento
            //request.ImporteVentaTotal: Monto autorizado con la TCMM
            string mesesFinanciados;
            string montoMensualidad;

            //OCG: Recuparar número de meses para el financiamiento
            mesesFinanciados = request.MesesFinanciados.ToString();

            return tryCatch.SafeExecutor(() =>
            {
                FinalizarCompraResponse response = new FinalizarCompraResponse();
                InfoService infoService = new InfoService();
                InformacionServiciosExternosBusiness informacionServiciosExternosBusiness = new InformacionServiciosExternosBusiness();
                infoService = informacionServiciosExternosBusiness.ObtenerCadenaServicioExterno(7);
                var cadenaURL = infoService.UrlService;
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(cadenaURL + "/getPurchaseAuthorization");
                httpWebRequest.Headers.Add("Authorization", credenciales.Licence);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json";
                var serializer = new Newtonsoft.Json.JsonSerializer();

                // Inicia la Transaccion
                using (TransactionScope scope = new TransactionScope())
                {
                    // Se procesan las promociones por venta
                    foreach (var item in request.DescuentosPromocionalesPorVentaAplicados.DescuentoPromocionesAplicados)
                    {
                        repository.PersistirPromocionesVenta(request.FolioOperacionAsociada, token.CodeStore, token.CodeBox, item.ImporteDescuento
                                                                , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);
                    }
                    // Se procesan las promociones por línea de venta
                    foreach (var item in request.DescuentosPromocionalesPorLineaAplicados.DescuentoPromocionesAplicados)
                    {
                        repository.PersistirPromocionesLineaVenta(request.FolioOperacionAsociada, token.CodeStore, token.CodeBox, item.Secuencia, item.ImporteDescuento
                                                                , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);
                    }
                    
                    //OCG:
                    // Se procesa la persistencia localmente
                    var movimientoVenta = repository.ProcesarMovimientoTarjetaMelodyMilano(token.CodeStore, token.CodeBox, token.CodeEmployee, "", request.NumeroTarjeta, request);

                    // Se invoca el Servicio de Milano
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        using (var textWriter = new Newtonsoft.Json.JsonTextWriter(streamWriter))
                        {
                            serializer.Serialize(textWriter,
                                new
                                {
                                    User = credenciales.UserName,
                                    Pwd = credenciales.Password,
                                    CardId = request.NumeroTarjeta,
                                    BranchId = token.CodeStore,
                                    TerminalNumber = token.CodeBox,
                                    VoucherNumber = movimientoVenta.Transaccion,
                                    TransactionAmount = request.ImporteVentaTotal,
                                    EntryMode = 12,
                                    FinancingSchemeId = request.PlanFinanciamiento
                                });
                        }
                    }
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var responseText = streamReader.ReadToEnd();
                        JToken responseJObject = JObject.Parse(responseText);
                        response.CodigoRespuestaTCMM = new CodigoRespuestaTCMM();
                        response.CodigoRespuestaTCMM.CodigoRespuesta = Convert.ToString(responseJObject.SelectToken("ErrorCode"));
                        response.CodigoRespuestaTCMM.MensajeRetorno = Convert.ToString(responseJObject.SelectToken("Message"));

                        //OCG: Recuperar los valores del financiamiento para poder actualizar la tabla
                        UpdatePlanFinanciamientoTCMM planesFinanciamiento  = new UpdatePlanFinanciamientoTCMM();

                        planesFinanciamiento.folioOperacion = request.FolioOperacionAsociada;
                        planesFinanciamiento.montoMensualidad = Convert.ToString(responseJObject.SelectToken("Response.MonthlyPayment"));
                        planesFinanciamiento.financiamientoId = request.MesesFinanciados.ToString();
                        planesFinanciamiento.codigoFormaPago = request.CodigoFormaPagoImporte;
                        planesFinanciamiento.codigoTienda = token.CodeStore;
                        planesFinanciamiento.fechaActualizacion = DateTime.Now;
                        planesFinanciamiento.codigoCaja = token.CodeBox;

                        repository.spMilano_ActualizaPlazoFinanciamiento(planesFinanciamiento);
                        //--


                        if (response.CodigoRespuestaTCMM.CodigoRespuesta == "0")
                        {
                            // Actualizar la autorización regresada por WS Milano
                            response.Authorization = Convert.ToString(responseJObject.SelectToken("Response").SelectToken("AuthorizationNumber"));
                            repository.ActualizarMovimientoTarjetaMelodyMilano(token.CodeStore, token.CodeBox, movimientoVenta.Transaccion, response.Authorization, request);
                            scope.Complete();
                        }
                    }
                }
                // Finalizar la Transacción  

                return response;
            });
        }

        /// <summary>
        /// Método para realizar un pago a una TCMM
        /// </summary>
        /// <param name="request">Objeto petición con información de Pago TCMM asociada</param>
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> RealizarPago(PagoTCMMRequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                OperationResponse operationResponse = new OperationResponse();
                InfoService infoService = new InfoService();
                operationResponse.CodeNumber = "0";
                InformacionServiciosExternosBusiness informacionServiciosExternosBusiness = new InformacionServiciosExternosBusiness();
                //NOTA: Unicamente se manda a llamar la URL ya que en el web.config se tiene como keys, y no se esta conectando el servicio mediante vs
                infoService = informacionServiciosExternosBusiness.ObtenerCadenaServicioExterno(10);
                var cadenaURL = infoService.UrlService;
                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(cadenaURL);
                //webrequest.Headers.Add ("Authorization","Basic dXNybW13czpNMWw0bjAqMjAxOA==");
                webrequest.Headers.Add("Authorization", credenciales.Licence);
                webrequest.Method = "POST";
                webrequest.ContentType = "application/json";
                using (var streamWriter = new StreamWriter(webrequest.GetRequestStream()))
                {
                    string json = "{\"User\":\"" + credenciales.UserName + "\"," +
                                  "\"Pwd\":\"" + credenciales.Password + "\"," +
                                   "\"CardId\":\"" + request.NumeroTarjeta + "\"," +
                                    "\"BranchId\":\"" + request.NumeroTienda + "\"," +
                                     "\"TerminalNumber\":\"" + request.NumeroCaja + "\"," +
                                      "\"VoucherNumber\":\"" + request.Transaccion + "\"," +
                                       "\"TransactionAmount\":\"" + request.Importe + "\"," +
                                    "\"EntryMode\":\"" + request.ModoEntrada + "\"}";
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var httpResponse = (HttpWebResponse)webrequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    dynamic resultadoTemporal = JsonConvert.DeserializeObject(result);
                    if (resultadoTemporal.ErrorCode == 0)
                    {
                        operationResponse.CodeNumber = "1";
                        operationResponse.CodeDescription = resultadoTemporal.Response;
                    }
                    else
                    {
                        operationResponse.CodeDescription = resultadoTemporal.Message;
                    }
                }
                return operationResponse;
            });
        }

        /// <summary>
        /// Método para finalizar el pago de una TCMM
        /// </summary>
        /// <param name="finalizarPagoTCMMRequest">Objeto petición con información de finalización de una transacción de pago TCMM</param>
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> FinalizarPagoTCMM(FinalizarPagoTCMMRequest finalizarPagoTCMMRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                // TODO: Falta la implementación
                OperationResponse operationResponse = new OperationResponse();
                operationResponse.CodeNumber = "100";
                operationResponse.CodeDescription = "Operación exitosa";
                return operationResponse;
            });
        }

    }

}
