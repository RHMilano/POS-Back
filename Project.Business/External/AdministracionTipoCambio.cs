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
//using Milano.BackEnd.Dto.General;

namespace Milano.BackEnd.Business
{
    /// <summary>
    /// Administracion de tipos de cambio
    /// </summary>
    public class AdministracionTipoCambio : BaseBusiness
    {

        CredencialesServicioExterno credenciales;
        AdministracionTipoCambioRepository repository;
        InformacionServiciosExternosRepository externosRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public AdministracionTipoCambio()
        {
            credenciales = new CredencialesServicioExterno();
            credenciales = new InformacionServiciosExternosBusiness().ObtenerCredencialesCambioDivisa();
            repository = new AdministracionTipoCambioRepository();
            externosRepository = new InformacionServiciosExternosRepository();
        }

        /// <summary>
        /// Obtener el tipo de cambio
        /// </summary>
        /// <param name="tipoCambioRequest">codigo de moneda</param>
        /// <returns>valor del tipo de cambio</returns>
        public ResponseBussiness<TipoCambioResponse> ObtenerTipoCambio(TipoCambioRequest tipoCambioRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return this.GetTipoCambio(tipoCambioRequest);
            }, this.repository.ObtenerMensajeCambioDivisaNoDisponible().CodeDescription);
        }

        /// <summary>
        /// Servicio para generar una autorizacion en Punto Clave de una venta en Dolares
        /// </summary>
        /// <param name="ventaDolaresRequest"></param>
        /// <returns></returns>
        //public ResponseBussiness<VentaDolaresResponse> ObtenerAutorizacionVentaDolares(VentaDolaresRequest ventaDolaresRequest)
        //{
        //    return tryCatch.SafeExecutor(() =>
        //    {
        //        return this.VentaDolares(ventaDolaresRequest);
        //    }, this.repository.ObtenerMensajeCambioDivisaNoDisponible().CodeDescription);
        //}

        /// <summary>
        /// Catalogo de divisas
        /// </summary>
        /// <returns></returns>
        public ResponseBussiness<Divisa[]> ObtenerDivisas(int CodeStore)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return this.repository.ObtenerCatalogoDivisa(CodeStore);
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipoCambioRequest"></param>
        /// <returns></returns>
        public TipoCambioResponse GetTipoCambio(TipoCambioRequest tipoCambioRequest)
        {
            TipoCambioResponse tipoCambioResponse = new TipoCambioResponse();
            CambioDivisaMilano cambio = new CambioDivisaMilano();
            InfoService inforService = externosRepository.ObtenerInfoServicioExterno(14);
            InformacionServiciosExternosRepository repository = new InformacionServiciosExternosRepository();

            cambio = GetTipoCambioLocal(tipoCambioRequest);
            string codigoRespuesta = "000";
            decimal valorCambio = 0;

                TipoCambioActualizado tipoCambioActualizado = new TipoCambioActualizado();

                if (tipoCambioRequest.CodigoTipoDivisa == "US")
                {

                tipoCambioActualizado = repository.DivisaActualizada();

                if (tipoCambioActualizado.TipoCambio == 0)
                {
                    //Inicializamos el proxy que nos ayudara a hacer la llamada al metodo de Sale
                    ProxyTipoCambio.Service1SoapClient proxy = new ProxyTipoCambio.Service1SoapClient();
                    proxy.Endpoint.Address = new System.ServiceModel.EndpointAddress(inforService.UrlService);
                    string xml = "<ExchangeRateRequest><credentials><username>{0}</username>"
                             + "<password>{1}</password><clientCode>{2}</clientCode></credentials>" +
                             "<exchangeInfo><currencyCode>{3}</currencyCode></exchangeInfo></ExchangeRateRequest> ";
                    xml = string.Format(xml, this.credenciales.UserName, this.credenciales.Password, "", cambio.CodigoExterno);
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml);
                    XmlNode result = proxy.GetRate(doc);
                    codigoRespuesta = result.ChildNodes[0].ChildNodes[0].InnerText;

                    if (codigoRespuesta == "000")
                    {
                        if (!cambio.UsarMaximoValor)
                            valorCambio = decimal.Round(Convert.ToDecimal(result.ChildNodes[1].ChildNodes[5].ChildNodes[0].InnerText), 2);
                        else
                            valorCambio = decimal.Round(Convert.ToDecimal(result.ChildNodes[1].ChildNodes[5].ChildNodes[1].InnerText), 2);


                        tipoCambioResponse.Mensaje = "Ok";
                        tipoCambioResponse.TasaConversionVigente = valorCambio;
                        tipoCambioResponse.ImporteMonedaNacional = tipoCambioRequest.ImporteMonedaNacional;
                        tipoCambioResponse.MontoMaximoRecibir = decimal.Round(Convert.ToDecimal(result.ChildNodes[1].ChildNodes[3].InnerText), 2);
                        tipoCambioResponse.MontoMaximoCambio = decimal.Round(Convert.ToDecimal(result.ChildNodes[1].ChildNodes[4].InnerText), 2);
                        // Ajuste para redondear a modo de cubrir el monto mínimo
                        decimal multiplicador = Convert.ToDecimal(Math.Pow(10, Convert.ToDouble(2)));
                        tipoCambioResponse.ImporteMonedaExtranjera = Math.Ceiling((tipoCambioRequest.ImporteMonedaNacional / valorCambio) * multiplicador) / multiplicador;


                        //Actualizar las tablas del tipo de Cambio
                        repository.ActualizaDivisa(valorCambio, decimal.Round(Convert.ToDecimal(result.ChildNodes[1].ChildNodes[3].InnerText), 2), decimal.Round(Convert.ToDecimal(result.ChildNodes[1].ChildNodes[4].InnerText), 2));

                    }
                    else
                        throw new Exception(result.InnerText);

                }
                else {
                    tipoCambioResponse.Mensaje = "Ok";
                    tipoCambioResponse.TasaConversionVigente = tipoCambioActualizado.TipoCambio;
                    tipoCambioResponse.ImporteMonedaNacional = tipoCambioRequest.ImporteMonedaNacional;
                    // Ajuste para redondear a modo de cubrir el monto mínimo
                    decimal multiplicador = Convert.ToDecimal(Math.Pow(10, Convert.ToDouble(2)));
                    tipoCambioResponse.ImporteMonedaExtranjera = Math.Ceiling((tipoCambioRequest.ImporteMonedaNacional / tipoCambioActualizado.TipoCambio) * multiplicador) / multiplicador;
                    tipoCambioResponse.MontoMaximoRecibir = tipoCambioActualizado.ReciboMaximo;
                    tipoCambioResponse.MontoMaximoCambio = tipoCambioActualizado.CambioMaximo;
                    // Ajuste para redondear a modo de cubrir el monto mínimo
                }

            }
               
            return tipoCambioResponse;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipoCambioRequest"></param>
        /// <returns></returns>
        public ExchangeRateResponse GetRate(TipoCambioRequest tipoCambioRequest)
        {
            ExchangeRateResponse tipoCambioResponse = new ExchangeRateResponse();
            InfoService inforService = externosRepository.ObtenerInfoServicioExterno(14);
            CambioDivisaMilano cambio = new CambioDivisaMilano();

            cambio = GetTipoCambioLocal(tipoCambioRequest);

            if (tipoCambioRequest.CodigoTipoDivisa == "US")
            {
                //Inicializamos el proxy que nos ayudara a hacer la llamada al metodo de Sale
                ProxyTipoCambio.Service1SoapClient proxy = new ProxyTipoCambio.Service1SoapClient();
                proxy.Endpoint.Address = new System.ServiceModel.EndpointAddress(inforService.UrlService);
               
                string xml = "<ExchangeRateRequest><credentials><username>{0}</username>"
                         + "<password>{1}</password><clientCode>{2}</clientCode></credentials>" +
                         "<exchangeInfo><currencyCode>{3}</currencyCode></exchangeInfo></ExchangeRateRequest> ";

                xml = string.Format(xml, this.credenciales.UserName, this.credenciales.Password, "", cambio.CodigoExterno);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                XmlNode result = proxy.GetRate(doc);

                //OCG 202021026 Poner un timeout 

                //Recuperar informacion de la transaccion
                tipoCambioResponse.ResponseCode = result.ChildNodes[0].ChildNodes[0].InnerText;

                if (tipoCambioResponse.ResponseCode != "000")
                { 
                    tipoCambioResponse.ResponseMessage = result.ChildNodes[0].ChildNodes[1].InnerText;
                    throw new Exception(result.InnerText);
                }
                else
                    tipoCambioResponse.ResponseMessage = "Ok"; // Respuesta correcta

                //Recuperar datos del nodo ExchangeInfo
                tipoCambioResponse.Bank = result.ChildNodes[1].ChildNodes[0].InnerText;
                tipoCambioResponse.CurrencyCode = result.ChildNodes[1].ChildNodes[1].InnerText;
                tipoCambioResponse.MaxAmountPerSale = decimal.Round(Convert.ToDecimal(result.ChildNodes[1].ChildNodes[3].InnerText),2);
                tipoCambioResponse.MaxChangePerSale = decimal.Round(Convert.ToDecimal(result.ChildNodes[1].ChildNodes[4].InnerText), 2);
                tipoCambioResponse.MinExchangeRateBuy = decimal.Round(Convert.ToDecimal(result.ChildNodes[1].ChildNodes[5].ChildNodes[0].InnerText), 2);
                tipoCambioResponse.MaxExchangeRateBuy = decimal.Round(Convert.ToDecimal(result.ChildNodes[1].ChildNodes[5].ChildNodes[1].InnerText), 2);
                tipoCambioResponse.MinExchangeRateSell = decimal.Round(Convert.ToDecimal(result.ChildNodes[1].ChildNodes[6].ChildNodes[0].InnerText), 2);
                tipoCambioResponse.MaxExchangeRateSell = decimal.Round(Convert.ToDecimal(result.ChildNodes[1].ChildNodes[6].ChildNodes[1].InnerText), 2);
            }

            //OCG

            return tipoCambioResponse;
        }

        private CambioDivisaMilano GetTipoCambioLocal(TipoCambioRequest tipoCambioRequest)
        {
            return repository.ObtenerTipoCambio(tipoCambioRequest.CodigoTipoDivisa);
        }

        /// <summary>
        ///     Método para obtener información del servicio externo Sale, usamos la comunicación via proxy
        /// </summary>
        /// <param name = "ventaDolaresRequest" ></param >
        /// <returns ></returns >
        public SaleResponse Sale(SaleRequest ventaDolaresRequest)
        {
            //string estampaTiempo;
           
            SaleResponse ventaDolaresResponse = new SaleResponse();
           
            //Random random = new Random();
            //estampaTiempo = DateTime.Now.ToString("yyyyMMddHHmmss") + $"_{random.Next(0, 9)}{random.Next(1, 5)}{random.Next(5, 9)}{random.Next(0, 4)}{random.Next(4, 9)}{random.Next(0, 9)}";

            try
            {
               
                //Alcanzar el informacion servicos externos repository y alcanzar el metodo ObtenerInfoServicioExterno
                InformacionServiciosExternosRepository externosRepository = new InformacionServiciosExternosRepository();
              
                InfoService inforService = new InfoService();
              

                inforService = externosRepository.ObtenerInfoServicioExterno(14);
                ProxyTipoCambio.Service1SoapClient proxy = new ProxyTipoCambio.Service1SoapClient();
                proxy.Endpoint.Address = new System.ServiceModel.EndpointAddress(inforService.UrlService);

                string xml = $"<SaleRequest>" +
                "               <credentials>" +
                $"                   <username>{this.credenciales.UserName}</username>" +
                $"                   <password>{this.credenciales.Password}</password>" +
                $"                   <clientCode>TDA{ventaDolaresRequest.ClientCode}</clientCode>" +
                "               </credentials>" +
                "               <transaction>" +
                $"                   <storeTeller>{ventaDolaresRequest.StoreTeller}</storeTeller>" +
                $"                   <currencyCode>{ventaDolaresRequest.CurrencyCode}</currencyCode>" +
                $"                   <receivedAmount>{ventaDolaresRequest.ReceivedAmount}</receivedAmount>" +
                $"                   <purchaseAmount>{ventaDolaresRequest.PurchaseAmount}</purchaseAmount>" +
                $"                   <usedExchangeRate>{ventaDolaresRequest.UsedExchangeRate}</usedExchangeRate>" +
                "                   <purchaseAmountCurrency>1</purchaseAmountCurrency>" +
                $"                   <idMerchantTransaction>{ventaDolaresRequest.IdMerchantTransaction}</idMerchantTransaction>" +
                $"                   <posTimestamp>{ventaDolaresRequest.PosTimestamp}</posTimestamp>" +
                "               </transaction>" +
                "           </SaleRequest> ";

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                XmlNode resultSale = proxy.Sale(doc);

                ventaDolaresResponse.ResponseCode = resultSale.ChildNodes[0].ChildNodes[2].InnerText;

                if (ventaDolaresResponse.ResponseCode == "000")
                {
                    ventaDolaresResponse.IdTrans = resultSale.ChildNodes[0].ChildNodes[0].InnerText;
                    ventaDolaresResponse.AuthoNumber = resultSale.ChildNodes[0].ChildNodes[1].InnerText;
                    ventaDolaresResponse.responseMessage = "ok";
                }
                else
                {
                    ventaDolaresResponse.responseMessage = resultSale.ChildNodes[0].ChildNodes[3].InnerText;
                }

                return ventaDolaresResponse;

            }
            catch (Exception ex)
            {
                _ = ex.Message;

                ventaDolaresResponse.ResponseCode = "-111";
                ventaDolaresResponse.responseMessage = "El servicio no responde o no se encuentra disponible";
               
                return ventaDolaresResponse;

            }
          
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="estampaTiempo"></param>
        /// <param name="tienda"></param>
        /// <returns></returns>
        public VerifySale VerifySale(string ticket, string estampaTiempo , string tienda)
        {
            VerifySale verificacionVenta = new VerifySale();

            try
            {
                //Alcanzar el informacion servicos externos repository y alcanzar el metodo ObtenerInfoServicioExterno
                InformacionServiciosExternosRepository externosRepository = new InformacionServiciosExternosRepository();
             
                InfoService inforService = new InfoService();
                //Random random = new Random();

                inforService = externosRepository.ObtenerInfoServicioExterno(14);
                ProxyTipoCambio.Service1SoapClient proxy = new ProxyTipoCambio.Service1SoapClient();
                proxy.Endpoint.Address = new System.ServiceModel.EndpointAddress(inforService.UrlService);

                string xml = $"<VerifySaleRequest>" +
                "               <credentials>" +
                $"                   <username>{this.credenciales.UserName}</username>" +
                $"                   <password>{this.credenciales.Password}</password>" +
                $"                   <clientCode>TDA{tienda}</clientCode>" +
                "               </credentials>" +
                "               <transaction>" +
                $"                   <idMerchantTransaction>{ticket}1</idMerchantTransaction>" +
                $"                   <posTimestamp>{estampaTiempo}</posTimestamp>" +
                "               </transaction>" +
                "           </VerifySaleRequest> ";

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                XmlNode resultSale = proxy.VerifySale(doc);

                verificacionVenta.VerifyResponseCode = resultSale.ChildNodes[0].ChildNodes[0].InnerText;

                if (verificacionVenta.VerifyResponseCode == "000")
                {
                    verificacionVenta.AuthoNumber = resultSale.ChildNodes[1].ChildNodes[1].InnerText;
                    verificacionVenta.verifyResponseMessage = resultSale.ChildNodes[0].ChildNodes[1].InnerText;

                    verificacionVenta.TransactionResponseCode = resultSale.ChildNodes[1].ChildNodes[2].InnerText;
                    verificacionVenta.TransactionResponseMessage = resultSale.ChildNodes[1].ChildNodes[3].InnerText;
                }

                // Respuesta de error en la informcion
                if (verificacionVenta.VerifyResponseCode == "102")
                {
                    verificacionVenta.AuthoNumber = "0";
                    verificacionVenta.verifyResponseMessage = resultSale.ChildNodes[0].ChildNodes[1].InnerText;
                }

                return verificacionVenta;
            }
            catch (Exception)
            {
                verificacionVenta.VerifyResponseCode = "-111";
                verificacionVenta.verifyResponseMessage = "El servicio no responde o no se encuentra disponible";

                return verificacionVenta;
            }

           
        }

        public ExchangeRateResponse AplicaPagoTransfer(TipoCambioRequest tipoCambioRequest)
        {
            ExchangeRateResponse tipoCambioResponse = new ExchangeRateResponse();
            InfoService inforService = externosRepository.ObtenerInfoServicioExterno(14);
            CambioDivisaMilano cambio = new CambioDivisaMilano();

            cambio = GetTipoCambioLocal(tipoCambioRequest);

            if (tipoCambioRequest.CodigoTipoDivisa == "US")
            {
                //Inicializamos el proxy que nos ayudara a hacer la llamada al metodo de Sale
                ProxyTipoCambio.Service1SoapClient proxy = new ProxyTipoCambio.Service1SoapClient();
                proxy.Endpoint.Address = new System.ServiceModel.EndpointAddress(inforService.UrlService);

                string xml = "<ExchangeRateRequest><credentials><username>{0}</username>"
                         + "<password>{1}</password><clientCode>{2}</clientCode></credentials>" +
                         "<exchangeInfo><currencyCode>{3}</currencyCode></exchangeInfo></ExchangeRateRequest> ";

                xml = string.Format(xml, this.credenciales.UserName, this.credenciales.Password, "", cambio.CodigoExterno);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                XmlNode result = proxy.GetRate(doc);

                //OCG 202021026 Poner un timeout 

                //Recuperar informacion de la transaccion
                tipoCambioResponse.ResponseCode = result.ChildNodes[0].ChildNodes[0].InnerText;

                if (tipoCambioResponse.ResponseCode != "000")
                {
                    tipoCambioResponse.ResponseMessage = result.ChildNodes[0].ChildNodes[1].InnerText;
                    throw new Exception(result.InnerText);
                }
                else
                    tipoCambioResponse.ResponseMessage = "Ok"; // Respuesta correcta

                //Recuperar datos del nodo ExchangeInfo
                tipoCambioResponse.Bank = result.ChildNodes[1].ChildNodes[0].InnerText;
                tipoCambioResponse.CurrencyCode = result.ChildNodes[1].ChildNodes[1].InnerText;
                tipoCambioResponse.MaxAmountPerSale = decimal.Round(Convert.ToDecimal(result.ChildNodes[1].ChildNodes[3].InnerText), 2);
                tipoCambioResponse.MaxChangePerSale = decimal.Round(Convert.ToDecimal(result.ChildNodes[1].ChildNodes[4].InnerText), 2);
                tipoCambioResponse.MinExchangeRateBuy = decimal.Round(Convert.ToDecimal(result.ChildNodes[1].ChildNodes[5].ChildNodes[0].InnerText), 2);
                tipoCambioResponse.MaxExchangeRateBuy = decimal.Round(Convert.ToDecimal(result.ChildNodes[1].ChildNodes[5].ChildNodes[1].InnerText), 2);
                tipoCambioResponse.MinExchangeRateSell = decimal.Round(Convert.ToDecimal(result.ChildNodes[1].ChildNodes[6].ChildNodes[0].InnerText), 2);
                tipoCambioResponse.MaxExchangeRateSell = decimal.Round(Convert.ToDecimal(result.ChildNodes[1].ChildNodes[6].ChildNodes[1].InnerText), 2);
            }

            //OCG

            return tipoCambioResponse;
        }
    }
}
