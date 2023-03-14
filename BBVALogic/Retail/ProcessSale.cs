using EGlobal.TotalPosSDKNet.Interfaz.Authorizer;
using EGlobal.TotalPosSDKNet.Interfaz.Catalog;
using EGlobal.TotalPosSDKNet.Interfaz.Layout;
using EGlobal.TotalPosSDKNet.Interfaz.Exceptions;
using EGlobal.TotalPosSDKNet.Interfaz.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BBVALogic.DTO;
using BBVALogic.DTO.Retail;
using BBVALogic.DTO.Setings;
using BBVALogic.Data;
using System.IO;
using EGlobal.DemoTotalPosSDKNet.Settings;
using Tools;
using Newtonsoft.Json;
using Tools.TryCatchI;
using DTOPos.ApiResponses;

namespace BBVALogic.Retail
{
    public class ProcessSale : BaseBusiness
    {
        private static Lazy<Peticion> _peticionPinPad = new Lazy<Peticion>(() => new Peticion());
        public static Peticion InstancePinPad
        {
            get
            {
                return _peticionPinPad.Value;
            }
        }

        LogSet logSet = new LogSet();
        LogDTO logDTO = new LogDTO();
        Pos pos = new Pos();

        protected Token token;

        public ProcessSale(Token token)
        {
            //  1 OCG

            this.token = token;
        }

        public ResponseBussiness<SaleResponse> ProcessNewSale(SaleRequest request)
        {
            // Cargar la configuración del archivo xml
            return tryCatch.SafeExecutor(() =>
            {
                ByteToNumbers sxsx = new ByteToNumbers();

                sxsx.SelectNumbers(request.Promo);

                Settings.SaveSettings();
                Settings.LoadSettings();
                LoadPinPad();
                return Sale2(request);
            });
        }

        /// <summary>
        /// Leer los datos de la tarjeta bancaria para saber si es de debito, 
        /// crédito, se paga con puntos
        /// </summary>
        /// <param name="request">Datos de la venta</param>
        /// <returns></returns>
        public ResponseBussiness<Card> TryReadCard(SaleRequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                ByteToNumbers msi = new ByteToNumbers();
                msi.SelectNumbers(request.Promo);

                Settings.SaveSettings();
                Settings.LoadSettings();
                LoadPinPad();
                return ReadCard(request);
            });
        }


        public ResponseBussiness<SaleResponse> TryCompleteSale(SaleRequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                Settings.LoadSettings();
                return CompleteSale(request);
            });
        }

        public SaleResponse CompleteSale(SaleRequest request)
        {

            //Peticion peticion;
            Tarjeta tarjeta;
            Dictionary<ParametroOperacion, object> parametros;
            Respuesta respuesta;
            SaleResponse saleResponse = new SaleResponse();


            respuesta = InstancePinPad.Autorizar();

            //InstancePinPad.FinalizarLecturaTarjeta(respuesta.CodigoRespuesta, respuesta.Autorizacion, "", respuesta.ModoLectura);
            logDTO.EsError = false;
            logDTO.LogType = LogType.BySecuence;
            logDTO.BBVASecuence = BBVASecuence.Autorizar;

            logSet.Register(logDTO);

            if (respuesta.BinExcepcion)
            {
                // Lógica general de envío de transacción al host autorizador por parte del comercio.
                try
                {
                    // Datos de tarjeta entregados por el Pin Pad.
                    string numeroTarjeta = respuesta.NumeroTarjeta;
                    string nombreTarjetahabiente = respuesta.Tarjetahabiente;
                    string track2 = respuesta.TrackII;
                    string track1 = respuesta.TrackI;
                    string codigoSeguridad = respuesta.Cvv;
                    string modoLectura = respuesta.ModoLectura;

                    // Lógica del comercio para envío de transaccíón sin cifrar al host y respuesta del mismo.
                    //////////////////////////////////////////////////////////////////////////////////////////
                    /////////////////////////////////////////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////////////////////////////////////////
                    ///////////////////////////////////////////////////////////////////////////////////////

                    //Lógica para finalizar transacción con Pin Pad.
                    string codigoRespuesta = "";    // Código respuesta del host al que el comercio dispara su transacción sin cifrar.
                    string numeroAutorizacion = ""; // Número de autorización entregado por el host al que el comercio dispara su transacción sin cifrar.
                    string datosAutenticacion = ""; // Datos EMV entregados por el host al que el comercio dispara su transacción sin cifrar.


                    //InstancePinPad.FinalizarLecturaTarjeta(codigoRespuesta, numeroAutorizacion, datosAutenticacion, modoLectura);
                }
                catch (DatosEmvException emvEx)
                {
                    _ = emvEx.ToString();
                    logDTO.EsError = true;
                    logDTO.LogType = LogType.BySecuence;
                    logDTO.BBVASecuence = BBVASecuence.Sale;
                    logDTO.Message = emvEx.Message;
                    logDTO.StackTrace = emvEx.StackTrace;


                    logSet.Register(logDTO);
                }
                catch (PeticionException ex)
                {
                    _ = ex.Message;
                    logDTO.EsError = true;
                    logDTO.LogType = LogType.BySecuence;
                    logDTO.BBVASecuence = BBVASecuence.Sale;
                    logDTO.Message = ex.Message;
                    logDTO.StackTrace = ex.StackTrace;


                    logSet.Register(logDTO);
                }
            }
            else
            {
                if (respuesta.CodigoRespuesta == "D1" && respuesta.TipoRespuesta == "1")
                {
                    bool bAceptaDcc = false;

                    //FrmDcc frmDcc;
                    //Dcc dcc;

                    //frmDcc = new FrmDcc();
                    //frmDcc.CodigoMonedaLocal = respuesta.MonedaLocal;
                    //frmDcc.NombreMonedaLocal = respuesta.NombreMonedaLocal;
                    //frmDcc.MontoMonedaLocal = respuesta.Importe;
                    //frmDcc.CodigoMonedaExtranjera = respuesta.MonedaExtranjera;
                    //frmDcc.NombreMonedaExtranjera = respuesta.NombreMonedaExtranjera;
                    //frmDcc.MontoMonedaExtranjera = respuesta.MontoMonedaExtranjera;
                    //frmDcc.TipoCambio = respuesta.TipoCambio;
                    //frmDcc.Comision = respuesta.Comision;

                    //if (frmDcc.ShowDialog(this) == DialogResult.Yes)
                    //{
                    //    bAceptaDcc = true;
                    //}

                    //dcc = new Dcc();
                    //dcc.MonedaLocal = respuesta.MonedaLocal;
                    //dcc.CodigoMonedaLocal = respuesta.CodigoMonedaLocal;
                    //dcc.NombreMonedaLocal = respuesta.NombreMonedaLocal;
                    //dcc.MonedaExtranjera = respuesta.MonedaExtranjera;
                    //dcc.CodigoMonedaExtranjera = respuesta.CodigoMonedaExtranjera;
                    //dcc.NombreMonedaExtranjera = respuesta.NombreMonedaExtranjera;
                    //dcc.MontoMonedaExtranjera = respuesta.MontoMonedaExtranjera;
                    //dcc.FolioExtranjero = respuesta.FolioExtranjero;
                    //dcc.TipoDeCambio = respuesta.TipoCambio;
                    //dcc.Comision = respuesta.Comision;
                    //dcc.TipoRespuestaDcc = respuesta.TipoRespuesta;
                    //dcc.Aceptar = bAceptaDcc;

                    //peticion.SetDcc(dcc);

                    //respuesta = peticion.Autorizar();
                }

                if (respuesta.CodigoRespuesta == "00")
                {
                    //MessageBox.Show(this, respuesta.Leyenda, "E-Global TotalPos SDK.Net Demo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //MessageBox.Show(this, respuesta.Leyenda, "E-Global TotalPos SDK.Net Demo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            

                saleResponse.binExcepcion = respuesta.BinExcepcion;
                saleResponse.afiliacion = respuesta.Afiliacion;
                saleResponse.afiliacionAMEX = respuesta.AfiliacionAmex;
                saleResponse.moneda = respuesta.Moneda;
                saleResponse.razonSocial = respuesta.RazonSocial;
                saleResponse.direccion = respuesta.Direccion;
                saleResponse.leyenda = respuesta.Leyenda;
                saleResponse.numeroTermial = respuesta.NumeroTerminal;
                saleResponse.macTerminal = respuesta.MacTerminal;
                saleResponse.codigoOperacion = respuesta.CodigoOperacion;
                saleResponse.numeroAutorizacion = respuesta.Autorizacion;
                saleResponse.nombreTransaccion = respuesta.NombreTransaccion;
                saleResponse.importeTransaccion = Convert.ToDecimal(respuesta.Importe);
                saleResponse.referenciaComercio = respuesta.ReferenciaDelComercio;
                saleResponse.importeRetiro = Convert.ToDecimal(respuesta.Importe);
                saleResponse.comisionRetiro = respuesta.Comision != "" ? Convert.ToDecimal(respuesta.Comision) : 0;

                short _financiamiento = 1;
                short _parcializacion = 0;

                short.TryParse(respuesta.Financiamiento, out _financiamiento);
                saleResponse.financiamiento = _financiamiento;

                short.TryParse(respuesta.Parcializacion, out _parcializacion);
                saleResponse.parcializacion = _parcializacion;

                saleResponse.codigoPromocion = respuesta.CodigoPromocion;
                saleResponse.vigenciaPromocionExponencial = respuesta.VigenciaPromocionExponencial;
                saleResponse.saldoPuntosDisponibles = respuesta.SaldoPuntosDisponibles;
                saleResponse.saldoRedimidoExponencialPesos = respuesta.SaldoRedimidoExponencialPesos;
                saleResponse.saldoAnteriorPesos = respuesta.SaldoAnteriorPesos;
                saleResponse.saldoRedimidoExponencialPuntos = respuesta.SaldoRedimidoExponencialPuntos;
                saleResponse.pesosRedimidos = respuesta.PesosRedimidos;
                saleResponse.saldoDisponiblePesos = respuesta.SaldoDisponiblePesos;
                saleResponse.factorExponenciacion = respuesta.FactorExponenciacion;
                saleResponse.saldoDisponibleExponencialPuntos = respuesta.SaldoDisponibleExponencialPuntos;
                saleResponse.puntosRedimidos = respuesta.PuntosRedimidos;
                saleResponse.saldoAnteriorPuntos = respuesta.SaldoAnteriorPuntos;
                saleResponse.saldoDisponibleExponencialPesos = respuesta.SaldoDisponibleExponencialPesos;
                saleResponse.criptogramaTarjeta = respuesta.CriptogramaTarjeta;
                saleResponse.numeroTarjeta = respuesta.NumeroTarjeta;
                saleResponse.tarjetaHabiente = respuesta.Tarjetahabiente;
                saleResponse.trackii = respuesta.TrackII;
                saleResponse.tracki = respuesta.TrackI;
                saleResponse.modeloLectura = respuesta.ModoLectura;
                saleResponse.productoTarjeta = respuesta.ProductoTarjeta;
                saleResponse.emisorTarjeta = respuesta.EmisorTarjeta;
                saleResponse.modoLectura = respuesta.ModoLectura;
                saleResponse.aplicacionTarjeta = respuesta.AplicacionTarjeta;
                saleResponse.idAplicacionTarjeta = respuesta.IdAplicacionTarjeta;
                saleResponse.referenciaFinaciera = respuesta.ReferenciaFinanciera;
                saleResponse.secuenciaTransaccion = respuesta.SecuenciaTransaccion;
                saleResponse.operador = respuesta.Operador;
                saleResponse.firma = "";// respuesta.Firma;
                saleResponse.fechaHoraTransaccion = respuesta.FechaHora;
                saleResponse.codigoRespuesta = respuesta.CodigoRespuesta;
                saleResponse.monedaLocal = respuesta.MonedaLocal;
                saleResponse.codigoMonedaLocal = respuesta.CodigoMonedaLocal;
                saleResponse.nombreMonedaLocal = respuesta.NombreMonedaLocal;
                saleResponse.monedaExtranjera = respuesta.MonedaExtranjera;
                saleResponse.importe = respuesta.Importe;
                saleResponse.codigoMonedaExtranjera = respuesta.CodigoMonedaExtranjera;
                saleResponse.montoMonedaExtranjera = respuesta.MontoMonedaExtranjera;
                saleResponse.nombreMonedaExtranjera = respuesta.NombreMonedaExtranjera;
                saleResponse.folioExtranjero = respuesta.FolioExtranjero;
                saleResponse.tipoCambio = respuesta.TipoCambio;
                saleResponse.comision = respuesta.Comision;
                saleResponse.AceptoDcc = respuesta.AceptoDcc;
                saleResponse.tipoRespuesta = respuesta.TipoRespuesta;
                saleResponse.cash = respuesta.Cash;
                saleResponse.cashComision = respuesta.CashComision;
                saleResponse.referenciaFinanciera = respuesta.ReferenciaFinanciera;
                saleResponse.secuenciaPos = 100;
                //saleResponse.fechaHoraComercio = respuesta.fechaHoraComercio;
                //saleResponse = JsonConvert.DeserializeObject<SaleResponse>(JsonConvert.SerializeObject(respuesta));

                pos.InsertSaleResponse(saleResponse);

                return saleResponse;
            }
           
            return saleResponse;
        }

        public SaleResponse Sale2(SaleRequest request)
        {

            Peticion peticion;
            Tarjeta tarjeta;

            Dictionary<ParametroOperacion, object> parametros;
            Respuesta respuesta;
            SaleResponse saleResponse = new SaleResponse();

            //try
            //{
            parametros = new Dictionary<ParametroOperacion, object>();
            //parametros.Add(ParametroOperacion.Importe, txtTransactionAmount.Text);
            parametros.Add(ParametroOperacion.Importe, request.TransactionAmount);
            //parametros.Add(ParametroOperacion.ReferenciaComercio, txtMerchanReference.Text);
            parametros.Add(ParametroOperacion.ReferenciaComercio, request.MerchanReference);

            peticion = new Peticion();
            //peticion.SetAfiliacion(Settings.ComercioAfiliacion, chkDollars.Checked ? Moneda.Dolares : Moneda.Pesos);
            peticion.SetAfiliacion(Settings.ComercioAfiliacion, request.Dollars ? Moneda.Dolares : Moneda.Pesos);
            peticion.SetTerminal(Settings.ComercioTerminal, Settings.ComercioMac);
            peticion.Operador = Settings.Operador;
            peticion.Fecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            //peticion.Amex = chkAmex.Checked;
            peticion.Amex = request.Amex;
            peticion.SetOperacion(Operacion.Venta, parametros);

            //if (Interfaz.Instance.Configuracion.FuncionalidadMoto)
            //{
            //    string numeroTarjeta = "";
            //    string fechaExpiracion = "";
            //    string codigoSeguridad = "";
            //    string nombreTarjetahabiente = "";

            //    FrmRetailMotoPopUp motoPopUp;

            //    motoPopUp = new FrmRetailMotoPopUp();
            //    motoPopUp.ShowDialog(this);

            //    numeroTarjeta = motoPopUp.NumeroTarjeta;
            //    fechaExpiracion = motoPopUp.FechaExpiracion;
            //    codigoSeguridad = motoPopUp.CodigoSeguridad;
            //    nombreTarjetahabiente = motoPopUp.NombreTrajetahabiente;

            //    motoPopUp.Dispose();

            //    tarjeta = peticion.LeerTarjeta(numeroTarjeta, fechaExpiracion, codigoSeguridad, nombreTarjetahabiente);
            //}
            //else
            //{
            //    tarjeta = peticion.LeerTarjeta();
            //}



            tarjeta = peticion.LeerTarjeta();

            logDTO.EsError = false;
            logDTO.LogType = LogType.BySecuence;
            logDTO.BBVASecuence = BBVASecuence.LeerTarjeta;

            logSet.Register(logDTO);

            //if (cmbPromo.SelectedIndex == 0)
            if (request.Promo == 0)
            {
                if (tarjeta.Producto == "c")
                {
                    if (tarjeta.Emisor == "12")
                    {
                        //DialogResult dr;

                        //dr = MessageBox.Show(this, "¿Desea pagar con puntos?", "E-Global TotalPos SDK.Net Demo", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);

                        //if (dr == DialogResult.Yes)
                        //{
                        //    peticion.AddParametroOperacion(ParametroOperacion.Puntos, true);
                        //}
                        //else if (dr == DialogResult.Cancel)
                        //{
                        //    peticion.SincronizacionFinal();
                        //    throw new Exception("Operación cancelada.");
                        //}
                    }
                }
                else if (tarjeta.Producto == "d")
                {
                    //decimal dAmount;

                    //bool bOfferCash = false;

                    //Decimal.TryParse(txtTransactionAmount.Text, out dAmount);

                    //if (dAmount > 0)
                    //{
                    //    if (tarjeta.Nip)
                    //    {
                    //        bOfferCash = true;
                    //    }
                    //}
                    //else
                    //{
                    //    if (tarjeta.Emisor == "12")
                    //    {
                    //        bOfferCash = true;
                    //    }
                    //}

                    //if (bOfferCash)
                    //{
                    //    DialogResult dr;

                    //    dr = MessageBox.Show(this, "¿Desea realizar retiro de efectivo?\n Cómisión por retiro: $7.00.", "E-Global TotalPos SDK.Net Demo", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);

                    //    if (dr == DialogResult.Yes)
                    //    {
                    //        string sCashAmount = "";

                    //        FrmCash cash = new FrmCash();

                    //        cash = new FrmCash();
                    //        cash.ShowDialog(this);

                    //        if (cash.DialogResult == DialogResult.OK)
                    //        {
                    //            sCashAmount = cash.CashAmount;
                    //            cash.Dispose();

                    //            peticion.AddParametroOperacion(ParametroOperacion.Cash, sCashAmount);
                    //            peticion.AddParametroOperacion(ParametroOperacion.CashComision, "7.00");
                    //        }
                    //    }
                    //    else if (dr == DialogResult.Cancel)
                    //    {
                    //        peticion.SincronizacionFinal();
                    //        throw new Exception("Operación cancelada.");
                    //    }
                    //}
                }
            }
            else
            {
                //if (cmbPromo.SelectedIndex == 1)
                if (request.Promo == 1)
                {
                    peticion.SetPromocionMeses(Promocion.MesesSinIntereses, 0, 3);
                }
                else if (request.Promo == 2)
                {
                    peticion.SetPromocionMeses(Promocion.MesesConIntereses, 0, 9);
                }
                else if (request.Promo == 3)
                {
                    peticion.SetPromocionSkipPayment(6);
                }
                else if (request.Promo == 4)
                {
                    peticion.SetPromocionMeses(Promocion.MesesSinIntereses, 4, 12);
                }
            }


            respuesta = peticion.Autorizar();
            logDTO.EsError = false;
            logDTO.LogType = LogType.BySecuence;
            logDTO.BBVASecuence = BBVASecuence.Autorizar;

            logSet.Register(logDTO);





            if (respuesta.BinExcepcion)
            {
                // Lógica general de envío de transacción al host autorizador por parte del comercio.
                try
                {
                    // Datos de tarjeta entregados por el Pin Pad.
                    string numeroTarjeta = respuesta.NumeroTarjeta;
                    string nombreTarjetahabiente = respuesta.Tarjetahabiente;
                    string track2 = respuesta.TrackII;
                    string track1 = respuesta.TrackI;
                    string codigoSeguridad = respuesta.Cvv;
                    string modoLectura = respuesta.ModoLectura;

                    // Lógica del comercio para envío de transaccíón sin cifrar al host y respuesta del mismo.
                    //////////////////////////////////////////////////////////////////////////////////////////
                    /////////////////////////////////////////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////////////////////////////////////////
                    ///////////////////////////////////////////////////////////////////////////////////////

                    //Lógica para finalizar transacción con Pin Pad.
                    string codigoRespuesta = "";    // Código respuesta del host al que el comercio dispara su transacción sin cifrar.
                    string numeroAutorizacion = ""; // Número de autorización entregado por el host al que el comercio dispara su transacción sin cifrar.
                    string datosAutenticacion = ""; // Datos EMV entregados por el host al que el comercio dispara su transacción sin cifrar.


                    peticion.FinalizarLecturaTarjeta(codigoRespuesta, numeroAutorizacion, datosAutenticacion, modoLectura);
                }
                catch (DatosEmvException emvEx)
                {
                    _ = emvEx.ToString();
                    logDTO.EsError = true;
                    logDTO.LogType = LogType.BySecuence;
                    logDTO.BBVASecuence = BBVASecuence.Sale;
                    logDTO.Message = emvEx.Message;
                    logDTO.StackTrace = emvEx.StackTrace;


                    logSet.Register(logDTO);
                }
                catch (PeticionException ex)
                {
                    _ = ex.Message;
                    logDTO.EsError = true;
                    logDTO.LogType = LogType.BySecuence;
                    logDTO.BBVASecuence = BBVASecuence.Sale;
                    logDTO.Message = ex.Message;
                    logDTO.StackTrace = ex.StackTrace;


                    logSet.Register(logDTO);
                }
            }
            else
            {
                if (respuesta.CodigoRespuesta == "D1" && respuesta.TipoRespuesta == "1")
                {
                    bool bAceptaDcc = false;

                    //FrmDcc frmDcc;
                    //Dcc dcc;

                    //frmDcc = new FrmDcc();
                    //frmDcc.CodigoMonedaLocal = respuesta.MonedaLocal;
                    //frmDcc.NombreMonedaLocal = respuesta.NombreMonedaLocal;
                    //frmDcc.MontoMonedaLocal = respuesta.Importe;
                    //frmDcc.CodigoMonedaExtranjera = respuesta.MonedaExtranjera;
                    //frmDcc.NombreMonedaExtranjera = respuesta.NombreMonedaExtranjera;
                    //frmDcc.MontoMonedaExtranjera = respuesta.MontoMonedaExtranjera;
                    //frmDcc.TipoCambio = respuesta.TipoCambio;
                    //frmDcc.Comision = respuesta.Comision;

                    //if (frmDcc.ShowDialog(this) == DialogResult.Yes)
                    //{
                    //    bAceptaDcc = true;
                    //}

                    //dcc = new Dcc();
                    //dcc.MonedaLocal = respuesta.MonedaLocal;
                    //dcc.CodigoMonedaLocal = respuesta.CodigoMonedaLocal;
                    //dcc.NombreMonedaLocal = respuesta.NombreMonedaLocal;
                    //dcc.MonedaExtranjera = respuesta.MonedaExtranjera;
                    //dcc.CodigoMonedaExtranjera = respuesta.CodigoMonedaExtranjera;
                    //dcc.NombreMonedaExtranjera = respuesta.NombreMonedaExtranjera;
                    //dcc.MontoMonedaExtranjera = respuesta.MontoMonedaExtranjera;
                    //dcc.FolioExtranjero = respuesta.FolioExtranjero;
                    //dcc.TipoDeCambio = respuesta.TipoCambio;
                    //dcc.Comision = respuesta.Comision;
                    //dcc.TipoRespuestaDcc = respuesta.TipoRespuesta;
                    //dcc.Aceptar = bAceptaDcc;

                    //peticion.SetDcc(dcc);

                    //respuesta = peticion.Autorizar();
                }

                if (respuesta.CodigoRespuesta == "00")
                {
                    //MessageBox.Show(this, respuesta.Leyenda, "E-Global TotalPos SDK.Net Demo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //MessageBox.Show(this, respuesta.Leyenda, "E-Global TotalPos SDK.Net Demo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                //txtMerchantId.Text = respuesta.Afiliacion;
                //txtMerchantAmexId.Text = respuesta.AfiliacionAmex;
                //txtCurrency.Text = respuesta.Moneda;
                //txtMerchantName.Text = respuesta.RazonSocial;
                //txtMerchantAddress.Text = respuesta.Direccion;
                //txtResponseLegend.Text = respuesta.Leyenda;
                //txtPosNumber.Text = respuesta.NumeroTerminal;
                //txtPosMac.Text = respuesta.MacTerminal;
                //txtOperationCode.Text = respuesta.CodigoOperacion;
                //txtAuthNumber.Text = respuesta.Autorizacion;
                //txtTransactionName.Text = respuesta.NombreTransaccion;
                //txtTransactionAmountR.Text = respuesta.Importe;
                //txtMerchantReferenceR.Text = respuesta.ReferenciaDelComercio;
                //txtCash.Text = respuesta.Cash;
                //txtCashFee.Text = respuesta.CashComision;
                //txtFinancing.Text = respuesta.Financiamiento;
                //txtPart.Text = respuesta.Parcializacion;
                //txtPromoCode.Text = respuesta.CodigoPromocion;
                //txtPromoExponentialExpiration.Text = respuesta.VigenciaPromocionExponencial;
                //txtAvailablePoints.Text = respuesta.SaldoPuntosDisponibles;
                //txtRedeemedExponentialMxn.Text = respuesta.SaldoRedimidoExponencialPesos;
                //txtBeforeMxn.Text = respuesta.SaldoAnteriorPesos;
                //txtRedeemedExponentialPoints.Text = respuesta.SaldoRedimidoExponencialPuntos;
                //txtRedeemedMxn.Text = respuesta.PesosRedimidos;
                //txtAvailableMxn.Text = respuesta.SaldoDisponiblePesos;
                //txtExponentialFactor.Text = respuesta.FactorExponenciacion;
                //txtAvailableExponentialPoints.Text = respuesta.SaldoDisponibleExponencialPuntos;
                //txtRedeemedPoints.Text = respuesta.PuntosRedimidos;
                //txtBeforePoints.Text = respuesta.SaldoAnteriorPuntos;
                //txtAvailableExponentialMxn.Text = respuesta.SaldoDisponibleExponencialPesos;
                //txtCriptogram.Text = respuesta.CriptogramaTarjeta;
                //txtCardNumberR.Text = respuesta.NumeroTarjeta;
                //txtProduct.Text = respuesta.ProductoTarjeta;
                //txtIssuer.Text = respuesta.EmisorTarjeta;
                //txtEntryMode.Text = respuesta.ModoLectura;
                //txtCardAppName.Text = respuesta.AplicacionTarjeta;
                //txtCardAppId.Text = respuesta.IdAplicacionTarjeta;
                //txtFinancialReference.Text = respuesta.ReferenciaFinanciera;
                //txtTransactionSequence.Text = respuesta.SecuenciaTransaccion;
                //txtOperator.Text = respuesta.Operador;
                //txtSignType.Text = respuesta.Firma.ToString();
                //txtTransactionDateTime.Text = respuesta.FechaHora;
                //txtResponseCode.Text = respuesta.CodigoRespuesta;
                //txtLocalCurrency.Text = respuesta.MonedaLocal;
                //txtForeignCurrency.Text = respuesta.MonedaExtranjera;
                //txtForeignCurrencyAmount.Text = respuesta.MontoMonedaExtranjera;
                //txtExchangeRate.Text = respuesta.TipoCambio != "" ? String.Format("1 {0} = {1} {2}", respuesta.MonedaExtranjera, respuesta.TipoCambio, respuesta.MonedaLocal) : respuesta.TipoCambio;
                //txtMarkUp.Text = respuesta.Comision == "" ? respuesta.Comision : respuesta.Comision + "%";
                //txtAcceptDcc.Text = respuesta.AceptoDcc;

                saleResponse.binExcepcion = respuesta.BinExcepcion;
                saleResponse.afiliacion = respuesta.Afiliacion;
                saleResponse.afiliacionAMEX = respuesta.AfiliacionAmex;
                saleResponse.moneda = respuesta.Moneda;
                saleResponse.razonSocial = respuesta.RazonSocial;
                saleResponse.direccion = respuesta.Direccion;
                saleResponse.leyenda = respuesta.Leyenda;
                saleResponse.numeroTermial = respuesta.NumeroTerminal;
                saleResponse.macTerminal = respuesta.MacTerminal;
                saleResponse.codigoOperacion = respuesta.CodigoOperacion;
                saleResponse.numeroAutorizacion = respuesta.Autorizacion;
                saleResponse.nombreTransaccion = respuesta.NombreTransaccion;
                saleResponse.importeTransaccion = Convert.ToDecimal(respuesta.Importe);
                saleResponse.referenciaComercio = respuesta.ReferenciaDelComercio;
                saleResponse.importeRetiro = Convert.ToDecimal(respuesta.Importe);
                saleResponse.comisionRetiro = respuesta.Comision != "" ? Convert.ToDecimal(respuesta.Comision) : 0;

                short _financiamiento = 1;
                short _parcializacion = 0;

                short.TryParse(respuesta.Financiamiento, out _financiamiento);
                saleResponse.financiamiento = _financiamiento;

                short.TryParse(respuesta.Parcializacion, out _parcializacion);
                saleResponse.parcializacion = _parcializacion;

                saleResponse.codigoPromocion = respuesta.CodigoPromocion;
                saleResponse.vigenciaPromocionExponencial = respuesta.VigenciaPromocionExponencial;
                saleResponse.saldoPuntosDisponibles = respuesta.SaldoPuntosDisponibles;
                saleResponse.saldoRedimidoExponencialPesos = respuesta.SaldoRedimidoExponencialPesos;
                saleResponse.saldoAnteriorPesos = respuesta.SaldoAnteriorPesos;
                saleResponse.saldoRedimidoExponencialPuntos = respuesta.SaldoRedimidoExponencialPuntos;
                saleResponse.pesosRedimidos = respuesta.PesosRedimidos;
                saleResponse.saldoDisponiblePesos = respuesta.SaldoDisponiblePesos;
                saleResponse.factorExponenciacion = respuesta.FactorExponenciacion;
                saleResponse.saldoDisponibleExponencialPuntos = respuesta.SaldoDisponibleExponencialPuntos;
                saleResponse.puntosRedimidos = respuesta.PuntosRedimidos;
                saleResponse.saldoAnteriorPuntos = respuesta.SaldoAnteriorPuntos;
                saleResponse.saldoDisponibleExponencialPesos = respuesta.SaldoDisponibleExponencialPesos;
                saleResponse.criptogramaTarjeta = respuesta.CriptogramaTarjeta;
                saleResponse.numeroTarjeta = respuesta.NumeroTarjeta;
                saleResponse.tarjetaHabiente = respuesta.Tarjetahabiente;
                saleResponse.trackii = respuesta.TrackII;
                saleResponse.tracki = respuesta.TrackI;
                saleResponse.modeloLectura = respuesta.ModoLectura;
                saleResponse.productoTarjeta = respuesta.ProductoTarjeta;
                saleResponse.emisorTarjeta = respuesta.EmisorTarjeta;
                saleResponse.modoLectura = respuesta.ModoLectura;
                saleResponse.aplicacionTarjeta = respuesta.AplicacionTarjeta;
                saleResponse.idAplicacionTarjeta = respuesta.IdAplicacionTarjeta;
                saleResponse.referenciaFinaciera = respuesta.ReferenciaFinanciera;
                saleResponse.secuenciaTransaccion = respuesta.SecuenciaTransaccion;
                saleResponse.operador = respuesta.Operador;
                saleResponse.firma = "";// respuesta.Firma;
                saleResponse.fechaHoraTransaccion = respuesta.FechaHora;
                saleResponse.codigoRespuesta = respuesta.CodigoRespuesta;
                saleResponse.monedaLocal = respuesta.MonedaLocal;
                saleResponse.codigoMonedaLocal = respuesta.CodigoMonedaLocal;
                saleResponse.nombreMonedaLocal = respuesta.NombreMonedaLocal;
                saleResponse.monedaExtranjera = respuesta.MonedaExtranjera;
                saleResponse.importe = respuesta.Importe;
                saleResponse.codigoMonedaExtranjera = respuesta.CodigoMonedaExtranjera;
                saleResponse.montoMonedaExtranjera = respuesta.MontoMonedaExtranjera;
                saleResponse.nombreMonedaExtranjera = respuesta.NombreMonedaExtranjera;
                saleResponse.folioExtranjero = respuesta.FolioExtranjero;
                saleResponse.tipoCambio = respuesta.TipoCambio;
                saleResponse.comision = respuesta.Comision;
                saleResponse.AceptoDcc = respuesta.AceptoDcc;
                saleResponse.tipoRespuesta = respuesta.TipoRespuesta;
                saleResponse.cash = respuesta.Cash;
                saleResponse.cashComision = respuesta.CashComision;
                saleResponse.referenciaFinanciera = respuesta.ReferenciaFinanciera;
                saleResponse.secuenciaPos = 100;
                //saleResponse.fechaHoraComercio = respuesta.fechaHoraComercio;
                //saleResponse = JsonConvert.DeserializeObject<SaleResponse>(JsonConvert.SerializeObject(respuesta));

                pos.InsertSaleResponse(saleResponse);

                return saleResponse;
            }
            //}
            //catch (PeticionException ex)
            //{
            //    //MessageBox.Show(this, egEx.Message, "E-Global TotalPos SDK.Net Demo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    logDTO.EsError = true;
            //    logDTO.LogType = LogType.BySecuence;
            //    logDTO.BBVASecuence = BBVASecuence.Sale;
            //    logDTO.Message = ex.Message;
            //    logDTO.StackTrace = ex.StackTrace;


            //    logSet.Register(logDTO);
            //    return saleResponse;

            //}
            //catch (Exception ex)
            //{
            //    logDTO.EsError = true;
            //    logDTO.LogType = LogType.BySecuence;
            //    logDTO.BBVASecuence = BBVASecuence.Sale;
            //    logDTO.Message = ex.Message;
            //    logDTO.StackTrace = ex.StackTrace;


            //    logSet.Register(logDTO);
            //    return saleResponse;
            //    //MessageBox.Show(this, ex.Message, "E-Global TotalPos SDK.Net Demo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            return saleResponse;
        }

        /// <summary>
        /// Leer los datos de la tarjeta bancaria para saber si es de debito, 
        /// crédito, se paga con puntos
        /// </summary>
        /// <param name="request">Datos de la venta</param>
        /// <returns></returns>
        public Card ReadCard(SaleRequest request)
        {
            Tarjeta tarjeta;
            Dictionary<ParametroOperacion, object> parametros;
            SaleResponse saleResponse = new SaleResponse();
            Card card = new Card();

            parametros = new Dictionary<ParametroOperacion, object>();
            parametros.Add(ParametroOperacion.Importe, request.TransactionAmount);
            parametros.Add(ParametroOperacion.ReferenciaComercio, request.MerchanReference);

            InstancePinPad.SetAfiliacion(Settings.ComercioAfiliacion, request.Dollars ? Moneda.Dolares : Moneda.Pesos);
            InstancePinPad.SetTerminal(Settings.ComercioTerminal, Settings.ComercioMac);
            InstancePinPad.Operador = Settings.Operador;
            InstancePinPad.Fecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            InstancePinPad.Amex = request.Amex;
            InstancePinPad.SetOperacion(Operacion.Venta, parametros);

            tarjeta = InstancePinPad.LeerTarjeta();

            //card.Emisor = tarjeta.Emisor;
            //card.Pan = tarjeta.Pan;
            //card.Pin = tarjeta.Nip.ToString();
            //card.Producto = tarjeta.Producto;
            //card.TarjetaHabiente = tarjeta.Tarjetahabiente;

            logDTO.EsError = false;
            logDTO.LogType = LogType.BySecuence;
            logDTO.BBVASecuence = BBVASecuence.LeerTarjeta;

            logSet.Register(logDTO);
           
            return card;
        }

        /// <summary>
        /// Inicia e instancia la comunicación con la pinpad
        /// </summary>
        private void LoadPinPad()
        {

            Configuracion configuracion;

            configuracion = new Configuracion
            {
                Logs = Settings.Logs,
                ClaveLogs = Settings.ClaveLogs,

                PinPadConexion = Settings.PinPadConexion,
                PinPadTimeOut = Settings.PinPadTimeOut,
                PinPadPuerto = Settings.PinPadPuertoWiFi,
                PinPadMensaje = Settings.PinPadMensaje,
                PinPadContactless = Settings.PinPadContactless,
                ClaveBines = Settings.ClaveBinesExcepcion,
                HostUrl = Settings.HostUrl,
                BinesUrl = Settings.BinesUrl,
                TokenUrl = Settings.TokenUrl,
                TelecargaUrl = Settings.TelecargaUrl,
                HostTimeOut = Settings.HostTimeOut,

                FuncionalidadGaranti = Settings.FuncionalidadGaranti,
                FuncionalidadMoto = Settings.FuncionalidadMoto,
                TecladoLiberado = Settings.TecladoLiberado,

                ComercioAfiliacion = Settings.ComercioAfiliacion,
                ComercioTerminal = Settings.ComercioTerminal,
                ComercioMac = Settings.ComercioMac,

                IdAplicacion = Settings.IdAplicacion,
                ClaveSecreta = Settings.ClaveSecreta
            };

            Interfaz.Instance.Configuracion = configuracion;
            Interfaz.Instance.Inicializar();

            logDTO.LogType = LogType.BySecuence;
            logDTO.BBVASecuence = BBVASecuence.LoadPinPad;

            logSet.Register(logDTO);

        }




    }
}
