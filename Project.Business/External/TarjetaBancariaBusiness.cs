using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Repository;
using Milano.BackEnd.Repository.General;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Business.LogMonitor;
using Milano.BackEnd.Repository.BBVAv2;
using Milano.BackEnd.Dto.Sales;

namespace Milano.BackEnd.Business
{
    /// <summary>
    /// Admnistracion de tarjetas de regalo
    /// </summary>
    public class TarjetaBancariaBusiness : BaseBusiness
    {
        ProxyTarjetasRegalo.wsTarjetasRegaloSoapClient proxy;
        InformacionServiciosExternosRepository externosRepository;
        TarjetasRegaloRepository repository;
        InfoService inforService;
        TokenDto token;
        addEvent s = new addEvent();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token"></param>
        public TarjetaBancariaBusiness(TokenDto token)
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
        /// Cobro con tarjeta bancaria
        /// </summary>
        /// <param name="url">URL del servicio de cobro</param>
        /// <param name="numeroSesion">Número de sesión</param>
        /// <param name="folio">Folio de operación</param>
        /// <param name="mesesFinanciamiento">Número de meses que se tiene antes de hacer el primer pago</param>
        /// <param name="mesesParcialidades">Número de meses parciales del pago</param>
        /// <param name="codigoPromocion">Código de promoción bancario</param>
        /// <param name="montoPagado">Monto a pagar</param>
        /// <returns></returns>
        public PagoBancarioResponse CobroVisaMasterCard_original(string url, int numeroSesion, string folio, int mesesFinanciamiento, int mesesParcialidades, int codigoPromocion, decimal montoPagado, int secuencia)
        {
            PagoBancarioResponse response = new PagoBancarioResponse();

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url + "/api/paymentpinpad/payvisamaster");

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("tiempoEsperaWebApi"));

            var serializer = new Newtonsoft.Json.JsonSerializer();
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                using (var textWriter = new Newtonsoft.Json.JsonTextWriter(streamWriter))
                {
                    serializer.Serialize(textWriter,
                        new
                        {
                            sessionNumber = numeroSesion,
                            commerceReference = folio,
                            transactionAmount = montoPagado,
                            financialMonths = mesesFinanciamiento,
                            paymentsPartial = mesesParcialidades,
                            promotion = codigoPromocion,
                            secuenciaPos = secuencia
                        });
                }
            }

            try
            {

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    JToken token = JObject.Parse(responseText);
                    response.CodeNumber = Convert.ToInt32(token.SelectToken("code"));
                    response.CodeDescription = Convert.ToString(token.SelectToken("message"));
                    response.Authorization = Convert.ToString(token.SelectToken("authorization"));
                    response.CardNumber = Convert.ToString(token.SelectToken("cardNumber"));
                    response.TipoTarjeta = Convert.ToString(token.SelectToken("tipoTarjeta"));
                    response.SePuedeRetirar = Convert.ToBoolean(token.SelectToken("isCashBack"));
                    response.SePuedePagarConPuntos = Convert.ToBoolean(token.SelectToken("isSaleWithPoints"));
                }
            }
            catch (WebException webException)
            {
                var httpResponse = (HttpWebResponse)webException.Response;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    JToken token = JObject.Parse(responseText);
                    response.CodeNumber = Convert.ToInt32(token.SelectToken("code"));
                    response.CodeDescription = Convert.ToString(token.SelectToken("message"));
                }
            }

            return response;
        }

        public PagoBancarioResponse CobroVisaMasterCard(string url, int numeroSesion, string folio, int mesesFinanciamiento, int mesesParcialidades, int codigoPromocion, decimal montoPagado, int secuencia)
        {
            PagoBancarioResponse response = new PagoBancarioResponse();

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url + "/api/paymentpinpad/payvisamaster");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("tiempoEsperaWebApi"));

            var serializer = new Newtonsoft.Json.JsonSerializer();
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                using (var textWriter = new Newtonsoft.Json.JsonTextWriter(streamWriter))
                {
                    serializer.Serialize(textWriter,
                        new
                        {
                            sessionNumber = numeroSesion,
                            commerceReference = folio,
                            transactionAmount = montoPagado,
                            financialMonths = mesesFinanciamiento,
                            paymentsPartial = mesesParcialidades,
                            promotion = codigoPromocion,
                            secuenciaPos = secuencia
                        });
                }
            }

            try
            {

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    JToken token = JObject.Parse(responseText);
                    response.CodeNumber = Convert.ToInt32(token.SelectToken("code"));
                    response.CodeDescription = Convert.ToString(token.SelectToken("message"));
                    response.Authorization = Convert.ToString(token.SelectToken("authorization"));
                    response.CardNumber = Convert.ToString(token.SelectToken("cardNumber"));
                    response.TipoTarjeta = Convert.ToString(token.SelectToken("tipoTarjeta"));
                    response.SePuedeRetirar = Convert.ToBoolean(token.SelectToken("isCashBack"));
                    response.SePuedePagarConPuntos = Convert.ToBoolean(token.SelectToken("isSaleWithPoints"));
                }
            }
            catch (WebException webException)
            {
                var httpResponse = (HttpWebResponse)webException.Response;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    JToken token = JObject.Parse(responseText);
                    response.CodeNumber = Convert.ToInt32(token.SelectToken("code"));
                    response.CodeDescription = Convert.ToString(token.SelectToken("message"));
                }
            }

            return response;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="pmtb_Request"></param>
        /// <param name="codigoTienda"></param>
        /// <param name="codigoCaja"></param>
        /// <returns></returns>
        //public PagoBancarioResponse CobroVisaMasterCard(ProcesarMovimientoTarjetaBancariaRequest pmtb_Request, int codigoTienda, int codigoCaja)
        //{

        //PagoBancarioResponse response = new PagoBancarioResponse();

        //Request_v1_5 request_v1_5 = new Request_v1_5();
        //Response_v1_5 response_v1_5 = new Response_v1_5();

        //request_v1_5.SecuenciaPos = 0; // Falta
        //request_v1_5.TransactionCode = Request_v1_5.TransactionCodes.Sale;
        //request_v1_5.SessionNumber = codigoTienda;
        //request_v1_5.TerminalNumber = codigoCaja;
        //request_v1_5.CommerceReference = pmtb_Request.saleRequestBBVA.MerchanReference;
        //request_v1_5.TransactionAmount = pmtb_Request.saleRequestBBVA.TransactionAmount;
        //request_v1_5.FinancialMonths = 0;
        //request_v1_5.PaymentsPartial = Convert.ToInt16(pmtb_Request.saleRequestBBVA.Promo);
        //request_v1_5.Promotion = Convert.ToInt16(pmtb_Request.saleRequestBBVA.Promo);
        //request_v1_5.Authorization = "";

        //RequestRepository requestRepository = new RequestRepository();

        //Request_v1_5 lastRequest = requestRepository.GetLastRequestBySequence(3, 3215);

        //if (lastRequest != null)
        //{
        //    request_v1_5.TransactionSequence = lastRequest.TransactionSequence + 1;
        //}

        //requestRepository.InsertRequest(request_v1_5, codigoTienda, codigoCaja);

        //response.CodeNumber = 100;
        //response.CodeDescription = "";
        //response.Authorization = "";
        //response.CardNumber = "";
        //response.TipoTarjeta = "";
        //response.SePuedeRetirar = true;
        //response.SePuedePagarConPuntos = true;

        //return response;
        //}

        /// <summary>
        /// Cobro con tarjeta bancaria
        /// </summary>
        /// <param name="url">URL del servicio de cobro</param>
        /// <param name="numeroSesion">Número de sesión</param>
        /// <param name="folio">Folio de operación</param>
        /// <param name="mesesFinanciamiento">Número de meses que se tiene antes de hacer el primer pago</param>
        /// <param name="mesesParcialidades">Número de meses parciales del pago</param>
        /// <param name="codigoPromocion">Código de promoción bancario</param>
        /// <param name="montoPagado">Monto a pagar</param>
        /// <returns></returns>
        public PagoBancarioResponse CobroAmericanExpress(string url, int numeroSesion, string folio, int mesesFinanciamiento, int mesesParcialidades, int codigoPromocion, decimal montoPagado, int secuencia)
        {
            PagoBancarioResponse response = new PagoBancarioResponse();

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url + "/api/paymentpinpad/payamex");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("tiempoEsperaWebApi"));
            var serializer = new Newtonsoft.Json.JsonSerializer();
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                using (var textWriter = new Newtonsoft.Json.JsonTextWriter(streamWriter))
                {
                    serializer.Serialize(textWriter,
                        new
                        {
                            sessionNumber = numeroSesion,
                            commerceReference = folio,
                            transactionAmount = montoPagado,
                            financialMonths = mesesFinanciamiento,
                            paymentsPartial = mesesParcialidades,
                            promotion = codigoPromocion,
                            secuenciaPos = secuencia
                        });
                }
            }
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    JToken token = JObject.Parse(responseText);
                    response.CodeNumber = Convert.ToInt32(token.SelectToken("code"));
                    response.CodeDescription = Convert.ToString(token.SelectToken("message"));
                    response.Authorization = Convert.ToString(token.SelectToken("authorization"));
                    response.CardNumber = Convert.ToString(token.SelectToken("cardNumber"));
                    response.TipoTarjeta = Convert.ToString(token.SelectToken("tipoTarjeta"));
                    response.SePuedeRetirar = Convert.ToBoolean(token.SelectToken("isCashBack"));
                    response.SePuedePagarConPuntos = Convert.ToBoolean(token.SelectToken("isSaleWithPoints"));
                }
            }
            catch (WebException webException)
            {
                var httpResponse = (HttpWebResponse)webException.Response;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    JToken token = JObject.Parse(responseText);
                    response.CodeNumber = Convert.ToInt32(token.SelectToken("code"));
                    response.CodeDescription = Convert.ToString(token.SelectToken("message"));
                }
            }

            return response;
        }

        /// <summary>
        /// Cobro con tarjeta bancaria y retiro en efectivo
        /// </summary>
        /// <param name="url">URL del servicio de cobro</param>
        /// <param name="numeroSesion">Número de sesión</param>
        /// <param name="folio">Folio de operación</param>
        /// <param name="mesesFinanciamiento">Número de meses que se tiene antes de hacer el primer pago</param>
        /// <param name="mesesParcialidades">Número de meses parciales del pago</param>
        /// <param name="codigoPromocion">Código de promoción bancario</param>
        /// <param name="montoPagado">Monto a pagar</param>
        /// <param name="montoCashBack">Monto a retirar</param>
        /// <returns></returns>
        public PagoBancarioResponse CobroConCashBack(string url, int numeroSesion, string folio, int mesesFinanciamiento, int mesesParcialidades, int codigoPromocion, decimal montoPagado, decimal montoCashBack, int secuencia)
        {
            PagoBancarioResponse response = new PagoBancarioResponse();

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url + "/api/paymentpinpad/paywithcashback");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("tiempoEsperaWebApi"));
            var serializer = new Newtonsoft.Json.JsonSerializer();
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                using (var textWriter = new Newtonsoft.Json.JsonTextWriter(streamWriter))
                {
                    serializer.Serialize(textWriter,
                        new
                        {
                            sessionNumber = numeroSesion,
                            commerceReference = folio,
                            transactionAmount = montoPagado,
                            cashBackAmount = montoCashBack,
                            financialMonths = mesesFinanciamiento,
                            paymentsPartial = mesesParcialidades,
                            promotion = codigoPromocion,
                            secuenciaPos = secuencia
                        });
                }
            }

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    JToken token = JObject.Parse(responseText);
                    response.CodeNumber = Convert.ToInt32(token.SelectToken("code"));
                    response.CodeDescription = Convert.ToString(token.SelectToken("message"));
                    response.Authorization = Convert.ToString(token.SelectToken("authorization"));
                    response.CardNumber = Convert.ToString(token.SelectToken("cardNumber"));
                    response.TipoTarjeta = Convert.ToString(token.SelectToken("tipoTarjeta"));
                }
            }
            catch (WebException webException)
            {
                var httpResponse = (HttpWebResponse)webException.Response;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    JToken token = JObject.Parse(responseText);
                    response.CodeNumber = Convert.ToInt32(token.SelectToken("code"));
                    response.CodeDescription = Convert.ToString(token.SelectToken("message"));
                }
            }

            return response;
        }

        /// <summary>
        /// Cobro con puntos bancarios
        /// </summary>
        /// <param name="url">URL del servicio de cobro</param>
        /// <param name="numeroSesion">Número de la sesión</param>
        /// <param name="folio">Folio de operación</param>
        /// <param name="montoPagado">Monto a pagar</param>
        /// <param name="pagarConPuntos">Indica si se desea pagar con puntos</param>
        /// <returns></returns>
        public PagoBancarioResponse CobroConPuntos(string url, int numeroSesion, string folio, decimal montoPagado, bool pagarConPuntos, int secuencia)
        {
            PagoBancarioResponse response = new PagoBancarioResponse();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url + "/api/paymentpinpad/paywithpoints");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("tiempoEsperaWebApi"));
            var serializer = new Newtonsoft.Json.JsonSerializer();
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                using (var textWriter = new Newtonsoft.Json.JsonTextWriter(streamWriter))
                {
                    serializer.Serialize(textWriter,
                        new
                        {
                            sessionNumber = numeroSesion,
                            commerceReference = folio,
                            transactionAmount = montoPagado,
                            payWithPoints = pagarConPuntos,
                            secuenciaPos = secuencia
                        });
                }
            }

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    JToken token = JObject.Parse(responseText);
                    response.CodeNumber = Convert.ToInt32(token.SelectToken("code"));
                    response.CodeDescription = Convert.ToString(token.SelectToken("message"));
                    response.Authorization = Convert.ToString(token.SelectToken("authorization"));
                    response.CardNumber = Convert.ToString(token.SelectToken("cardNumber"));
                    response.TipoTarjeta = Convert.ToString(token.SelectToken("tipoTarjeta"));
                }
            }
            catch (WebException webException)
            {
                var httpResponse = (HttpWebResponse)webException.Response;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    JToken token = JObject.Parse(responseText);
                    response.CodeNumber = Convert.ToInt32(token.SelectToken("code"));
                    response.CodeDescription = Convert.ToString(token.SelectToken("message"));
                }
            }

            return response;
        }

        /// <summary>
        /// Cobro con tarjeta bancaria y retiro en efectivo
        /// </summary>
        /// <param name="url">URL del servicio de cobro</param>
        /// <param name="numeroSesion">Número de sesión</param>
        /// <param name="folio">Folio de operación</param>
        /// <param name="montoCashBack">Monto a retirar</param>
        /// <returns></returns>
        public PagoBancarioResponse CashBackAdvance(string url, int numeroSesion, string folio, decimal montoCashBack, int secuencia)
        {
            PagoBancarioResponse response = new PagoBancarioResponse();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url + "/api/paymentpinpad/cashback");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("tiempoEsperaWebApi"));
            var serializer = new Newtonsoft.Json.JsonSerializer();
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                using (var textWriter = new Newtonsoft.Json.JsonTextWriter(streamWriter))
                {
                    serializer.Serialize(textWriter,
                        new
                        {
                            sessionNumber = numeroSesion,
                            commerceReference = folio,
                            cashBackAmount = montoCashBack,
                            secuenciaPos = secuencia
                        });
                }
            }

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    JToken token = JObject.Parse(responseText);
                    response.CodeNumber = Convert.ToInt32(token.SelectToken("code"));
                    response.CodeDescription = Convert.ToString(token.SelectToken("message"));
                    response.Authorization = Convert.ToString(token.SelectToken("authorization"));
                    response.CardNumber = Convert.ToString(token.SelectToken("cardNumber"));
                    response.TipoTarjeta = Convert.ToString(token.SelectToken("tipoTarjeta"));
                }
            }
            catch (WebException webException)
            {
                var httpResponse = (HttpWebResponse)webException.Response;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    JToken token = JObject.Parse(responseText);
                    response.CodeNumber = Convert.ToInt32(token.SelectToken("code"));
                    response.CodeDescription = Convert.ToString(token.SelectToken("message"));
                }
            }

            return response;
        }

        /// <summary>
        /// Cancela la operación de cobro
        /// </summary>
        /// <param name="url">URL del servicio de cobro</param>
        /// <returns></returns>
        public PagoBancarioResponse Cancelar(string url)
        {
            PagoBancarioResponse response = new PagoBancarioResponse();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url + "/api/paymentpinpad/cancelpayment");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    JToken token = JObject.Parse(responseText);
                    response.CodeNumber = Convert.ToInt32(token.SelectToken("code"));
                    response.CodeDescription = Convert.ToString(token.SelectToken("message"));
                }
            }
            catch (WebException webException)
            {
                var httpResponse = (HttpWebResponse)webException.Response;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    JToken token = JObject.Parse(responseText);
                    response.CodeNumber = Convert.ToInt32(token.SelectToken("code"));
                    response.CodeDescription = Convert.ToString(token.SelectToken("message"));
                }
            }

            return response;
        }

        /// <summary>
        /// Ejecuta una carga de Lalaves
        /// </summary>
        /// <param name="url">URL del servicio de cobro</param>                
        /// <returns>Resultado de la operación</returns>
        public OperationResponse CargarLlaves(string url)
        {
            OperationResponse response = new OperationResponse();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url + "/api/paymentpinpad/loadkey");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    JToken token = JObject.Parse(responseText);
                    response.CodeNumber = Convert.ToString(token.SelectToken("code"));
                    response.CodeDescription = Convert.ToString(token.SelectToken("message"));
                }
            }
            catch (WebException webException)
            {
                var httpResponse = (HttpWebResponse)webException.Response;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    JToken token = JObject.Parse(responseText);
                    response.CodeNumber = Convert.ToString(token.SelectToken("code"));
                    response.CodeDescription = Convert.ToString(token.SelectToken("message"));
                }
            }

            return response;
        }

        /// <summary>
        /// Obtiene el número de tarjeta mediante la pin pad
        /// </summary>
        /// <param name="url">URL del servicio de cobro</param>
        /// <param name="numeroSesion">Número de sesión</param>
        /// <param name="folio">Folio de operación</param>
        /// <returns></returns>
        public PagoBancarioResponse ObtenerTarjeta(string url, int numeroSesion, string folio)
        {
            PagoBancarioResponse response = new PagoBancarioResponse();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url + "/api/paymentpinpad/getcard");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("tiempoEsperaWebApi"));

            var serializer = new Newtonsoft.Json.JsonSerializer();
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                using (var textWriter = new Newtonsoft.Json.JsonTextWriter(streamWriter))
                {
                    serializer.Serialize(textWriter,
                        new
                        {
                            sessionNumber = numeroSesion,
                            commerceReference = folio
                        });
                }
            }


            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    JToken token = JObject.Parse(responseText);
                    response.CodeNumber = Convert.ToInt32(token.SelectToken("code"));
                    response.CodeDescription = Convert.ToString(token.SelectToken("message"));
                    response.CardNumber = Convert.ToString(token.SelectToken("cardNumber"));
                    response.TipoTarjeta = Convert.ToString(token.SelectToken("tipoTarjeta"));
                }
            }
            catch (WebException webException)
            {
                var httpResponse = (HttpWebResponse)webException.Response;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    JToken token = JObject.Parse(responseText);
                    response.CodeNumber = Convert.ToInt32(token.SelectToken("code"));
                    response.CodeDescription = Convert.ToString(token.SelectToken("message"));
                }
            }

            return response;
        }
    }
}