using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.Finlag;
using Milano.BackEnd.Dto.Lealtad;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Repository;
using Milano.BackEnd.Repository.Finlag;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Transactions;
using Milano.BackEnd.Business.ProxyMayoristas;
using Milano.BackEnd.Business.ProxyLealtad;
using System.Drawing;
//using Project.Services.Utils;

namespace Milano.BackEnd.Business.Finlag
{
    /// <summary>
    /// 
    /// </summary>
    public class FinlagBusiness : BaseBusiness
    {

        TokenDto token;
        InfoService infoService;
        FinlagRepository finlagRepository;
        InformacionServiciosExternosRepository externosRepository;
        PaymentProcessingRepository paymentProcessingRepository;
        ProxyFinlag.VentasFinlagSoapClient proxy;

        //Declaraciom del proxy para el servicio de lealtad
        ProxyLealtad.wsLealtadSoapClient proxyLealtad;
        //--

        //ProxyFinlag.VentasMilanoSoapClient proxy;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        public FinlagBusiness(TokenDto token)
        {
            infoService = new InfoService();
            externosRepository = new InformacionServiciosExternosRepository();
            paymentProcessingRepository = new PaymentProcessingRepository();
            finlagRepository = new FinlagRepository();
            infoService = externosRepository.ObtenerInfoServicioExterno(22);
            proxy = new ProxyFinlag.VentasFinlagSoapClient();
            proxy.Endpoint.Address = new System.ServiceModel.EndpointAddress(infoService.UrlService);

            //Configuracion para la seccion de lealtad
            infoService = externosRepository.ObtenerInfoServicioExterno(27);
            proxyLealtad = new wsLealtadSoapClient();
            proxyLealtad.Endpoint.Address = new System.ServiceModel.EndpointAddress(infoService.UrlService);
            //--

            this.token = token;

        }

        /// <summary>
        /// Método de la capa business para aplicar el vale de finlag
        /// </summary>
        /// <param name="aplicaValeRequest"></param>
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> ProcesarAplicarValeFinlag(AplicaValeRequest aplicaValeRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    OperationResponse operationResponse = new OperationResponse();

                    // Se procesa en el servicio externo de Finlag para procesar la venta
                    var respuestaWS = AplicarValeInternal(aplicaValeRequest);
                    // Se procesa el movimiento Pago localmente
                    if (respuestaWS.NumeroCodigo == 100)
                    {
                        // Si resultado OK se solicita y persiste la trama de impresión                        
                        ConsultaValeFinlagRequest consultaValeFinlagRequest = new ConsultaValeFinlagRequest();
                        consultaValeFinlagRequest.FolioVale = aplicaValeRequest.FolioVale;
                        ConsultaTramaImpresionResult consultaTramaImpresionResult = this.ObtenerTramaImpresion(consultaValeFinlagRequest);
                        finlagRepository.ProcesarTramaImpresion(token.CodeStore, token.CodeBox, token.CodeEmployee, aplicaValeRequest.FolioOperacionAsociada, consultaTramaImpresionResult);

                        // -------------- Si resultado OK se procesa el pago Finlag en base de datos localmente      

                        // Se procesan las promociones por venta
                        foreach (var item in aplicaValeRequest.DescuentosPromocionalesPorVentaAplicados.DescuentoPromocionesAplicados)
                        {
                            OperationResponse response = new OperationResponse();
                            response = paymentProcessingRepository.PersistirPromocionesVenta(aplicaValeRequest.FolioOperacionAsociada, token.CodeStore, token.CodeBox, item.ImporteDescuento
                                                                    , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);

                        }
                        // Se procesan las promociones por línea de venta
                        foreach (var item in aplicaValeRequest.DescuentosPromocionalesPorLineaAplicados.DescuentoPromocionesAplicados)
                        {
                            OperationResponse response = new OperationResponse();
                            response = paymentProcessingRepository.PersistirPromocionesLineaVenta(aplicaValeRequest.FolioOperacionAsociada, token.CodeStore, token.CodeBox, item.Secuencia, item.ImporteDescuento
                                                                    , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);

                        }

                        // Se persiste la información del cobro
                        operationResponse = finlagRepository.ProcesarMovimientoAplicarVale(token.CodeStore, token.CodeBox, token.CodeEmployee, aplicaValeRequest);
                        scope.Complete();
                        return operationResponse;
                    }
                    else
                    {
                        // Si resultado ERROR se regresa
                        operationResponse.CodeNumber = respuestaWS.NumeroCodigo.ToString();
                        operationResponse.CodeDescription = respuestaWS.DescripcionCodigo;
                        return operationResponse;
                    }
                }
            });
        }

        private AplicaValeResponse AplicarValeInternal(AplicaValeRequest aplicaValeRequest)
        {
            AplicaValeResponse aplicaValeResponse = new AplicaValeResponse();
            DateTime dateTime = Convert.ToDateTime(aplicaValeRequest.FechaNacimiento);
            // Invocar servicio externo
            var tablaRespuestaProxy = proxy.AplicaVale(aplicaValeRequest.FolioVale,
                aplicaValeRequest.IdDistribuidora,
                aplicaValeRequest.ImporteVentaTotal,
                aplicaValeRequest.Quincenas,
                aplicaValeRequest.TipoPago,
                token.CodeBox,
                token.CodeStore,
                dateTime,
                aplicaValeRequest.INE,
                aplicaValeRequest.Nombre,
                aplicaValeRequest.Apaterno,
                aplicaValeRequest.Amaterno,
                aplicaValeRequest.Calle,
                aplicaValeRequest.NumExt,
                aplicaValeRequest.Colonia,
                aplicaValeRequest.Estado,
                aplicaValeRequest.Municipio,
                aplicaValeRequest.CP,
                aplicaValeRequest.Sexo,
                aplicaValeRequest.FolioOperacionAsociada,
                infoService.UserName, infoService.Password,
                aplicaValeRequest.puntosUtilizados, aplicaValeRequest.efectivoPuntos);
            // Procesar la respuesta del servicio externo
            aplicaValeResponse = ConvertToAplicaVale(tablaRespuestaProxy).FirstOrDefault();
            if (aplicaValeResponse.IdTransaccion == "")
            {
                // En caso de que el WS regrese un error
                aplicaValeResponse.NumeroCodigo = 201;
                aplicaValeResponse.DescripcionCodigo = aplicaValeResponse.EstatusMovimiento;
            }
            return aplicaValeResponse;
        }

        private List<AplicaValeResponse> ConvertToAplicaVale(DataTable tablaRespuestaProxy)
        {
            var convertedList = (from rw in tablaRespuestaProxy.AsEnumerable()
                                 select new AplicaValeResponse()
                                 {
                                     EstatusMovimiento = Convert.ToString(rw["EstatusVale"]),
                                     IdTransaccion = Convert.ToString(rw["IdTransaccion"]),
                                     NumeroCodigo = 100,
                                     DescripcionCodigo = "OK"
                                 }).ToList();
            return convertedList;
        }

        /// <summary>
        /// Método para obtener el cliente en la capa Business
        /// </summary>
        /// <param name="clienteFinlag"></param>
        /// <returns></returns>
        public ResponseBussiness<ClienteFinlagResponse> ConsultarCliente(ClienteFinlag clienteFinlag)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return ConsultarClienteInternal(clienteFinlag);
            });
        }

        private ClienteFinlagResponse ConsultarClienteInternal(ClienteFinlag clienteFinlag)
        {
            ClienteFinlagResponse clienteFinlagResponse = new ClienteFinlagResponse();
            DataTable tablaRespuestaProxy = proxy.ConsultaCliente(clienteFinlag.Nombre, clienteFinlag.Apaterno, clienteFinlag.Amaterno, clienteFinlag.FechaNacimiento, infoService.UserName, infoService.Password);
            try
            {
                clienteFinlagResponse = testCliente(tablaRespuestaProxy).FirstOrDefault();
                if (clienteFinlagResponse.Nombre == "" || clienteFinlagResponse == null)
                {
                    clienteFinlagResponse.EstatusCliente = false.ToString();
                }
                else
                {
                    clienteFinlagResponse.NumeroCodigo = 100;
                    clienteFinlagResponse.DescripcionCodigo = "OK";
                    clienteFinlagResponse.EstatusCliente = true.ToString();
                }
            }
            catch (Exception exception)
            {
                clienteFinlagResponse = ObtenerEstatusCliente(tablaRespuestaProxy).FirstOrDefault();
                clienteFinlagResponse.DescripcionCodigo = clienteFinlagResponse.EstatusCliente.ToString();
                string[] words = clienteFinlagResponse.DescripcionCodigo.Split('|');
                clienteFinlagResponse.NumeroCodigo = Convert.ToInt32(words[0]);
                clienteFinlagResponse.DescripcionCodigo = words[1];
                clienteFinlagResponse.EstatusCliente = false.ToString();
            }
            return clienteFinlagResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clienteFinlagRequest"></param>
        /// <returns></returns>
        public ResponseBussiness<List<ConsultaValeFinlagResult>> ConsultarMovimientos(ConsultaMovientoPDVRequest clienteFinlagRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return ConsultarMovimientoInternal(clienteFinlagRequest);
            });
        }

        private List<ConsultaValeFinlagResult> ConsultarMovimientoInternal(ConsultaMovientoPDVRequest clienteFinlagRequest)
        {
            DateTime fechaInicial = Convert.ToDateTime(clienteFinlagRequest.FechaInicial);
            DateTime fechaFinal = Convert.ToDateTime(clienteFinlagRequest.FechaFinal);
            var tablaRespuestaProxy = proxy.ConsultaMovimientos(clienteFinlagRequest.IdTienda, fechaInicial, fechaFinal, infoService.UserName, infoService.Password);
            return ConvertConsultaVale(tablaRespuestaProxy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="consultaValeFinlagRequest"></param>
        /// <returns></returns>
        public ResponseBussiness<ConsultaValeFinlagResult> ConsultarVale(ConsultaValeFinlagRequest consultaValeFinlagRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return ConsultarValeInternal(consultaValeFinlagRequest);
            });
        }

        private ConsultaValeFinlagResult ConsultarValeInternal(ConsultaValeFinlagRequest consultaValeFinlagRequest)
        {
            ConsultaValeFinlagResult consultaValeFinlagResult = new ConsultaValeFinlagResult();
            var tablaRespuestaProxy = proxy.ConsultaVale(consultaValeFinlagRequest.FolioVale, infoService.UserName, infoService.Password);
            try
            {
                consultaValeFinlagResult = ConvertConsultaVale(tablaRespuestaProxy).FirstOrDefault();
            }
            catch (Exception exception)
            {
                consultaValeFinlagResult = ConvertEstatusVale(tablaRespuestaProxy).FirstOrDefault();
            }
            return consultaValeFinlagResult;
        }

        private List<ConsultaValeFinlagResult> ConvertEstatusVale(DataTable tablaRespuestaProxy)
        {
            var convertedList = (from rw in tablaRespuestaProxy.AsEnumerable()
                                 select new ConsultaValeFinlagResult()
                                 {
                                     TiendaAplica = 0,
                                     IdCajaAplica = 0,
                                     FechaAplicacion = "",
                                     FolioVenta = "",
                                     FolioVale = "",
                                     DV = "",
                                     Nombre = "",
                                     Apaterno = "",
                                     Amaterno = "",
                                     INE = "",
                                     FechaNacimiento = "",
                                     Calle = "",
                                     NumExt = "",
                                     Colonia = "",
                                     Estado = "",
                                     Municipio = "",
                                     CP = "",
                                     Sexo = "",
                                     MontoAplicado = 0,
                                     Concepto = "",
                                     Quincenas = 0,
                                     Total = 0,
                                     EstatusVale = false.ToString(),
                                     DescripcionCodigo = Convert.ToString(rw["EstatusVale"]),
                                     NumeroCodigo = 201
                                 }).ToList();
            return convertedList;
        }

        private List<ConsultaValeFinlagResult> ConvertConsultaVale(DataTable tablaRespuestaProxy)
        {
            var convertedList = (from rw in tablaRespuestaProxy.AsEnumerable()
                                 select new ConsultaValeFinlagResult()
                                 {
                                     TiendaAplica = Convert.ToInt32(rw["TiendaAplica"]),
                                     IdCajaAplica = Convert.ToInt32(rw["IdCajaAplica"]),
                                     FechaAplicacion = Convert.ToString(rw["FechaAplica"]),
                                     FolioVenta = Convert.ToString(rw["FolioVenta"]),
                                     FolioVale = Convert.ToString(rw["FolioVale"]),
                                     DV = Convert.ToString(rw["DV"]),
                                     Nombre = Convert.ToString(rw["Nombre"]),
                                     Apaterno = Convert.ToString(rw["Apaterno"]),
                                     Amaterno = Convert.ToString(rw["Amaterno"]),
                                     INE = Convert.ToString(rw["INE"]),
                                     FechaNacimiento = Convert.ToString(rw["FechaNacimiento"]),
                                     Calle = Convert.ToString(rw["Calle"]),
                                     NumExt = Convert.ToString(rw["NumExt"]),
                                     Colonia = Convert.ToString(rw["Colonia"]),
                                     Estado = Convert.ToString(rw["Estado"]),
                                     Municipio = Convert.ToString(rw["Municipio"]),
                                     CP = Convert.ToString(rw["CP"]),
                                     Sexo = Convert.ToString(rw["Sexo"]),
                                     MontoAplicado = Convert.ToDecimal(rw["MontoAplicado"]),
                                     Concepto = Convert.ToString(rw["Concepto"]),
                                     Quincenas = Convert.ToInt32(rw["Quincenas"]),
                                     Total = Convert.ToDecimal(rw["Total"]),
                                     EstatusVale = true.ToString(),
                                     NumeroCodigo = 100,
                                     DescripcionCodigo = "OK"
                                 }).ToList();
            return convertedList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tablaAmortizacionRequest"></param>
        /// <returns></returns>        
        public ResponseBussiness<List<TablaAmortizacionResult>> ObtenerTablaAmortizacion(TablaAmortizacionRequest tablaAmortizacionRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return ObtenerTablaAmortizacionInternal(tablaAmortizacionRequest);
            });
        }

        private List<TablaAmortizacionResult> ObtenerTablaAmortizacionInternal(TablaAmortizacionRequest tablaAmortizacionRequest)
        {
            List<TablaAmortizacionResult> tablaAmortizacionResults = new List<TablaAmortizacionResult>();
            DateTime dateTime = Convert.ToDateTime(tablaAmortizacionRequest.FechaNacimiento);
            TablaAmortizacionResult tablaAmortizacionResult = new TablaAmortizacionResult();
            var tablaRespuestaProxy = proxy.TablaAmortizacion(tablaAmortizacionRequest.IdDistribuidora, tablaAmortizacionRequest.FolioVale, tablaAmortizacionRequest.MontoVenta, tablaAmortizacionRequest.Nombre,
                                            tablaAmortizacionRequest.Apaterno, tablaAmortizacionRequest.Amaterno, dateTime, tablaAmortizacionRequest.Calle, tablaAmortizacionRequest.NumExt,
                                            tablaAmortizacionRequest.Colonia, tablaAmortizacionRequest.CP, tablaAmortizacionRequest.Municipio, tablaAmortizacionRequest.Estado, infoService.UserName, infoService.Password);
            try
            {
                tablaAmortizacionResults = ConvertToTablaAmortizacion(tablaRespuestaProxy);
            }
            catch (Exception exception)
            {
                tablaAmortizacionResults = ObtenerEstatusVale(tablaRespuestaProxy);
            }
            return tablaAmortizacionResults;
        }

        /// <summary>
        /// Método para obtener la trama de impresión de Finlag
        /// </summary>
        /// <param name="consultaValeFinlagRequest"></param>
        /// <returns></returns>
        public ResponseBussiness<ConsultaTramaImpresionResult> ObtenerTramaImpresion(ConsultaValeFinlagRequest consultaValeFinlagRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return ObtenerTramaImpresionInternal(consultaValeFinlagRequest);
            });
        }

        private ConsultaTramaImpresionResult ObtenerTramaImpresionInternal(ConsultaValeFinlagRequest consultaValeFinlagRequest)
        {
            ConsultaTramaImpresionResult consultaValeFinlagResult = new ConsultaTramaImpresionResult();
            DataTable tablaRespuestaProxy = proxy.TramaImpresion(consultaValeFinlagRequest.FolioVale, infoService.UserName, infoService.Password);
            consultaValeFinlagResult = ConvertTramaImpresion(tablaRespuestaProxy).FirstOrDefault();
            return consultaValeFinlagResult;
        }

        private List<ConsultaTramaImpresionResult> ConvertTramaImpresion(DataTable tablaRespuestaProxy)
        {
            var convertedList = (from rw in tablaRespuestaProxy.AsEnumerable()
                                 select new ConsultaTramaImpresionResult()
                                 {
                                     Fecha = Convert.ToString(rw["Fecha"]),
                                     PuntoVenta = Convert.ToString(rw["PuntoVenta"]),
                                     IdDistribuidora = Convert.ToString(rw["IdDistribuidora"]),
                                     FolioVale = Convert.ToString(rw["FolioVale"]),
                                     NombreCompleto = Convert.ToString(rw["NombreCompleto"]),
                                     Calle = Convert.ToString(rw["Calle"]),
                                     NumeroExt = Convert.ToString(rw["NumeroExt"]),
                                     Colonia = Convert.ToString(rw["Colonia"]),
                                     CP = Convert.ToString(rw["CP"]),
                                     Municipio = Convert.ToString(rw["Municipio"]),
                                     Estado = Convert.ToString(rw["Estado"]),
                                     Quincenas = Convert.ToString(rw["Quincenas"]),
                                     MontoAplicado = Convert.ToString(rw["MontoAplicado"]),
                                     TotalPagar = Convert.ToString(rw["TotalPagar"]),
                                     PagoQuincenal = Convert.ToString(rw["PagoQuincenal"]),
                                     FechaPrimerPago = Convert.ToString(rw["FechaPrimerPago"]),
                                     Pagare = Convert.ToString(rw["Pagare"]),
                                     TiendaAplica = Convert.ToString(rw["TiendaAplica"]),
                                     IdCajaAplica = Convert.ToString(rw["IdCajaAplica"]),
                                     TipoVenta = Convert.ToString(rw["TipoVenta"]),
                                     PuntosUtilizados = Convert.ToString(rw["PuntosUtilizados"]),
                                     EfectivoPuntos = Convert.ToString(rw["EfectivoEquivalente"]),
                                 }).ToList();
            return convertedList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validaValeRequest"></param>
        /// <returns></returns>
        public ResponseBussiness<ValidaValeResult> ValidarVale(ValidaValeRequest validaValeRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return ValidarValeInternal(validaValeRequest);
            });
        }

        private ValidaValeResult ValidarValeInternal(ValidaValeRequest validaValeRequest)
        {
            ValidaValeResult validaValeResult = new ValidaValeResult();
            DataTable tablaRespuestaProxy = proxy.ValidaVale(validaValeRequest.IdDistribuidora, validaValeRequest.FolioVale, validaValeRequest.Monto, infoService.UserName, infoService.Password);
            validaValeResult = test(tablaRespuestaProxy).FirstOrDefault();
            if (String.IsNullOrEmpty(validaValeResult.Nombre))
            {
                validaValeResult.EstatusCliente = false;
            }
            else
            {
                validaValeResult.EstatusCliente = true;
                validaValeResult.DescripcionCodigo = validaValeResult.DescripcionCodigo;
                validaValeResult.NumeroCodigo = validaValeResult.NumeroCodigo;
            }
            return validaValeResult;
        }


        /// <summary>
        /// Registrar cliente de lealtad para credito
        /// </summary>
        /// <param name="RegistrarClienteLealtad"></param>
        /// <returns></returns>
        public ResponseBussiness<RegistroLealtadResponse> RegistrarClienteLealtad(RegistroLealtadRequest registroLealtadRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return RegistrarClienteLealtadInternal(registroLealtadRequest);
            });
        }

        private RegistroLealtadResponse RegistrarClienteLealtadInternal(RegistroLealtadRequest registroLealtadRequest)
        {
            RegistroLealtadResponse registroLealtadResponse = new RegistroLealtadResponse();

            registroLealtadRequest.ssFechaNacimiento = registroLealtadRequest.ssFechaNacimiento.Replace("-", "");
            //registroLealtadRequest.ssGenero = "";

            ProxyLealtad.RespuestaRegistrarCliente xxx = proxyLealtad.RegistrarCliente(
                registroLealtadRequest.iiCodigoClienteSistemaCredito,
                registroLealtadRequest.iiCodigoEmpleado,
                registroLealtadRequest.iiCodigoClienteWeb,
                registroLealtadRequest.ssTelefono,
                registroLealtadRequest.ssPaterno,
                registroLealtadRequest.ssMaterno,
                registroLealtadRequest.ssNombre,
                registroLealtadRequest.ssGenero,
                registroLealtadRequest.ssFechaNacimiento,
                registroLealtadRequest.ssFolioVenta,
                registroLealtadRequest.iiCodigoTiendaRegistra,
                registroLealtadRequest.iiCodigoCajaRegistra,
                registroLealtadRequest.iiCodigoEmpleadoRegistra,
                registroLealtadRequest.ssEmail);

            registroLealtadResponse.ssMensaje = xxx.sMensaje;
            registroLealtadResponse.ssNivel = xxx.sNivel;
            registroLealtadResponse.iiCodigoCliente = xxx.iCodigoCliente;
            registroLealtadResponse.bbPrimeraCompra = xxx.bPrimeraCompra;
            registroLealtadResponse.bbError = xxx.bError;

            return registroLealtadResponse;

        }


        /// <summary>
        /// Registrar cliente de lealtad para credito
        /// </summary>
        /// <param name="consultaClienteLealtadRequest"></param>
        /// <returns>ConsultarClienteResponse</returns>
        public ResponseBussiness<ConsultaClienteLealtadResponse> ConsultarClienteLealtad(ConsultaClienteLealtadRequest consultaClienteLealtadRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return ConsultaClienteLealtadInternal(consultaClienteLealtadRequest);
            });
        }


        private ConsultaClienteLealtadResponse ConsultaClienteLealtadInternal(ConsultaClienteLealtadRequest consultaClienteLealtadRequest)
        {
            ConsultaClienteLealtadResponse consultaClienteResponse = new ConsultaClienteLealtadResponse();
            List<rInfoClientesCRM> listrenglon = new List<rInfoClientesCRM>();

            ProxyLealtad.RespuestaConsultarCliente info = proxyLealtad.ConsultarCliente(
                consultaClienteLealtadRequest.iiCodigoCliente,
                consultaClienteLealtadRequest.iiCodigoClienteSistemaCredito,
                consultaClienteLealtadRequest.iiCodigoEmpleado,
                consultaClienteLealtadRequest.iiCodigoClienteWeb,
                consultaClienteLealtadRequest.ssTelefono,
                consultaClienteLealtadRequest.ssPaterno,
                consultaClienteLealtadRequest.ssMaterno,
                consultaClienteLealtadRequest.ssNombre,
                consultaClienteLealtadRequest.ssFechaNacimiento,
                consultaClienteLealtadRequest.iiCodigoTiendaRegistro,
                consultaClienteLealtadRequest.ssEmail,
                consultaClienteLealtadRequest.iiCodigoTienda,
                consultaClienteLealtadRequest.iiCodigoCaja);

            if (info.infoClientesCRMs != null)
            {
                consultaClienteResponse.bbCantidadLimitada = info.bCantidadLimitada;
                consultaClienteResponse.iiCantidadClientes = info.iCantidadClientes;
                consultaClienteResponse.ssMensajeError = info.sMensajeError;

                foreach (var r in info.infoClientesCRMs)
                {
                    rInfoClientesCRM renglon = new rInfoClientesCRM();
                    renglon.iiCodigoCliente = r.iCodigoCliente;
                    renglon.iiCodigoClienteSistemaCredito = r.iCodigoClienteSistemaCredito;
                    renglon.iiCodigoEmpleado = r.iCodigoEmpleado;
                    renglon.iiCodigoClienteWeb = r.iCodigoClienteWeb;
                    renglon.ssNivel = r.sNivel;
                    renglon.bbPrimeraCompra = r.bPrimeraCompra;
                    renglon.ssTelefono = r.sTelefono;
                    renglon.ssPaterno = r.sPaterno;
                    renglon.ssMaterno = r.sMaterno;
                    renglon.ssNombre = r.sNombre;
                    renglon.ssGenero = r.sGenero;
                    renglon.ssFechaNacimiento = r.sFechaNacimiento;
                    renglon.ddSaldo = r.dSaldo;
                    renglon.ssFechaRegistro = r.sFechaRegistro;
                    renglon.ssEmail = r.sEmail;
                    listrenglon.Add(renglon);
                }
            }

            consultaClienteResponse.IInfoClientesCRM = listrenglon.ToArray();

            return consultaClienteResponse;

        }

        //private List<ConsultaClienteLealtadResponse> ConvertToTableConsultaLealtad(DataTable dt)
        //{
        //    var convertedList = (from rw in dt.AsEnumerable()
        //                         select new ConsultaClienteLealtadResponse()
        //                         {
        //                             iiCodigoCliente = Convert.ToInt32(rw["iCodigoCliente"]),
        //                             iiCodigoClienteSistemaCredito = Convert.ToInt32(rw["iCodigoClienteSistemaCredito"]),
        //                             iiCodigoEmpleado = Convert.ToInt32(rw["iCodigoEmpleado"]),
        //                             iiCodigoClienteWeb = Convert.ToInt32(rw["iCodigoClienteWeb"]),
        //                             ssNivel = Convert.ToString(rw["sNivel"]),
        //                             bbPrimeraCompra = Convert.ToBoolean(rw["bPrimeraCompra"]),
        //                             ssTelefono = Convert.ToString(rw["sTelefono"]),
        //                             ssPaterno = Convert.ToString(rw["sPaterno"]),
        //                             ssMaterno = Convert.ToString(rw["sMaterno"]),
        //                             ssNombre = Convert.ToString(rw["sNombre"]),
        //                             ssGenero = Convert.ToString(rw["sGenero"]),
        //                             ssFechaNacimiento = Convert.ToString(rw["sFechaNacimiento"]),
        //                             ddSaldo = Convert.ToDouble(rw["dSaldo"]),
        //                             ssFechaRegistro = Convert.ToString(rw["sFechaRegistro"]),
        //                             ssEmail = Convert.ToString(rw["sEmail"])
        //                         }).ToList();

        //    return convertedList;
        //}

        private List<ValidaValeResult> test(DataTable dt)
        {
            var convertedList = (from rw in dt.AsEnumerable()
                                 select new ValidaValeResult()
                                 {
                                     EstatusVale = Convert.ToString(rw["EstatusVale"]),
                                     NombreDistribuidora = Convert.ToString(rw["NombreDistribuidora"]),
                                     CreditoVale = Convert.ToString(rw["CreditoVale"]),
                                     EstatusDV = Convert.ToString(rw["EstatusDV"]),
                                     FirmaDV = Convert.ToString(rw["FirmaDV"]),
                                     IdDistribuidora = Convert.ToString(rw["IdDistribuidora"]),
                                     Foliovale = Convert.ToString(rw["Foliovale"]),
                                     Quincenas = Convert.ToString(rw["Quincenas"]),
                                     Nombre = Convert.ToString(rw["Nombre"]),
                                     Paterno = Convert.ToString(rw["Paterno"]),
                                     Materno = Convert.ToString(rw["Materno"]),
                                     FechaNacimiento = Convert.ToString(rw["Fechanacimiento"]),
                                     Calle = Convert.ToString(rw["Calle"]),
                                     Numero = Convert.ToString(rw["Numero"]),
                                     Colonia = Convert.ToString(rw["Colonia"]),
                                     CP = Convert.ToString(rw["CP"]),
                                     Municipio = Convert.ToString(rw["Ciudad"]),
                                     Estado = Convert.ToString(rw["Estado"]),
                                     NumeroCodigo = 100,
                                     DescripcionCodigo = "OK"
                                 }
                                 ).ToList();
            foreach (var elemento in convertedList)
            {
                String data = convertedList[0].EstatusVale;
                string[] words = data.Split('|');
                convertedList[0].NumeroCodigo = Convert.ToInt32(words[0]);
                convertedList[0].DescripcionCodigo = words[1];
            }
            return convertedList;
        }

        private List<TablaAmortizacionResult> ConvertToTablaAmortizacion(DataTable dt)
        {
            var convertedList = (from rw in dt.AsEnumerable()
                                 select new TablaAmortizacionResult()
                                 {
                                     FechaPago = Convert.ToString(rw["fechaPago"]),
                                     NumeroPago = Convert.ToString(rw["numPago"]),
                                     Capital = Convert.ToString(rw["capital"]),
                                     Total = Convert.ToDecimal(rw["total"]),
                                     TotalQuincenal = Convert.ToDecimal(rw["totalQuincenal"]),
                                     TipoPago = Convert.ToString(rw["tipoPago"]),
                                     EstatusVale = Convert.ToString(rw["EstatusVale"]),
                                     EfectivoPuntos = Convert.ToString(rw["EfectivoPuntos"]),
                                     Puntos = Convert.ToString(rw["Puntos"]),
                                 }).ToList();
            // Asignar estatus a elementos de la lista
            foreach (var elemento in convertedList)
            {
                String data = elemento.EstatusVale;
                if (String.IsNullOrEmpty(data))
                {
                    data = "201|El estatus del Vale no se encontró";
                }
                String[] words = data.Split('|');
                elemento.NumeroCodigo = Convert.ToInt32(words[0]);
                elemento.DescripcionCodigo = words[1];
            }
            return convertedList;
        }

        private List<ClienteFinlagResponse> testCliente(DataTable dt)
        {
            var convertedList = (from rw in dt.AsEnumerable()
                                 select new ClienteFinlagResponse()
                                 {
                                     FechaNacimiento = Convert.ToString(rw["FechaNacimiento"]),
                                     INE = Convert.ToString(rw["INE"]),
                                     Nombre = Convert.ToString(rw["Nombre"]),
                                     Apaterno = Convert.ToString(rw["Apaterno"]),
                                     Amaterno = Convert.ToString(rw["Amaterno"]),
                                     Calle = Convert.ToString(rw["Calle"]),
                                     NumExt = Convert.ToString(rw["NumExt"]),
                                     Colonia = Convert.ToString(rw["Colonia"]),
                                     Estado = Convert.ToString(rw["Estado"]),
                                     Municipio = Convert.ToString(rw["Municipio"]),
                                     CP = Convert.ToString(rw["CP"]),
                                     Sexo = Convert.ToString(rw["Sexo"])
                                 }).ToList();
            return convertedList;
        }

        private List<ClienteFinlagResponse> ObtenerEstatusCliente(DataTable dt)
        {

            var convertedList = (from rw in dt.AsEnumerable()
                                 select new ClienteFinlagResponse()
                                 {
                                     EstatusCliente = Convert.ToString(rw["EstatusCliente"])
                                 }).ToList();
            return convertedList;
        }

        private List<TablaAmortizacionResult> ObtenerEstatusVale(DataTable dt)
        {
            var convertedList = (from rw in dt.AsEnumerable()
                                 select new TablaAmortizacionResult()
                                 {
                                     EstatusVale = Convert.ToString(rw["EstatusVale"]),
                                     DescripcionCodigo = Convert.ToString(rw["EstatusVale"]),
                                     NumeroCodigo = 201
                                 }).ToList();
            // Asignar estatus a elementos de la lista
            foreach (var elemento in convertedList)
            {
                String data = elemento.EstatusVale;
                String[] words = data.Split('|');
                elemento.NumeroCodigo = Convert.ToInt32(words[0]);
                elemento.DescripcionCodigo = words[1];
            }
            return convertedList;
        }
    }
}