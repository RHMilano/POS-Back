using BBVALogic.DTO.Retail;
using BBVALogic.DTOCompatibility_1_5;
using DTOPos.ApiResponses;
using EGlobal.DemoTotalPosSDKNet.Settings;
using EGlobal.TotalPosSDKNet.Interfaz.Authorizer;
using EGlobal.TotalPosSDKNet.Interfaz.Catalog;
using EGlobal.TotalPosSDKNet.Interfaz.Layout;
using EGlobal.TotalPosSDKNet.Interfaz.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;
using Tools.TryCatchI;

namespace BBVALogic.Retail
{
    public class ProcessSaleCompatibility_1_5:BaseBusiness
    {
        private static Lazy<Peticion> _peticionPinPad = new Lazy<Peticion>(() => new Peticion());
        public static Peticion peticion_diferida
        {
            get
            {
                return _peticionPinPad.Value;
            }
        }

        LogSet logSet = new LogSet();
        LogDTO logDTO = new LogDTO();

        //public ProcessSaleCompatibility_1_5() { 
        
        //}

        string cnn = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

        public Request GetLastRequestBySequence(int terminalNumber, int sessionNumber)
        {
            Request request = null;

            using (SqlConnection cn = new SqlConnection(cnn))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand("dbo.sp_vanti_BuscarUltimaPeticionPinPadPorSequencia", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NumeroTermial", terminalNumber);
                cmd.Parameters.AddWithValue("@NumeroSesion", sessionNumber);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    request = new Request();
                    request.TransactionCode = (Request.TransactionCodes)Convert.ToInt32(dr.GetValue(0));
                    request.TerminalNumber = Convert.ToInt32(dr.GetValue(1));
                    request.SessionNumber = Convert.ToInt32(dr.GetValue(2));
                    request.TransactionSequence = Convert.ToInt32(dr.GetValue(3));
                    request.TransactionAmount = Convert.ToDecimal(dr.GetValue(4));
                    request.Tip = Convert.ToDecimal(dr.GetValue(5));
                    request.Folio = Convert.ToInt32(dr.GetValue(6));
                    request.EMVCapacity = Convert.ToInt32(dr.GetValue(7));
                    request.CardReaderType = Convert.ToInt32(dr.GetValue(8));
                    request.CVV2Capacity = Convert.ToInt32(dr.GetValue(9));
                    request.FinancialMonths = Convert.ToInt16(dr.GetValue(10));
                    request.PaymentsPartial = Convert.ToInt16(dr.GetValue(11));
                    request.Promotion = Convert.ToInt16(dr.GetValue(12));
                    request.TypeCurrency = Convert.ToInt32(dr.GetValue(13));
                    request.Authorization = Convert.ToString(dr.GetValue(14));
                    request.CashBackAmount = Convert.ToDecimal(dr.GetValue(15));
                    request.CommerceDateTime = Convert.ToDateTime(dr.GetValue(16));
                    request.CommerceReference = Convert.ToString(dr.GetValue(17));
                    request.AmountOther = Convert.ToDecimal(dr.GetValue(18));
                    request.OperatorKey = Convert.ToString(dr.GetValue(19));
                    request.Affiliation = Convert.ToInt32(dr.GetValue(20));
                    request.RoomNumber = Convert.ToString(dr.GetValue(21));
                    request.FinancialReference = Convert.ToInt32(dr.GetValue(22));
                    request.Message = Convert.ToString(dr.GetValue(23));
                }
            }
            
            return request;

        }

        /// <summary>
        /// Leer los datos de la tarjeta bancaria para saber si es de debito, 
        /// crédito, se paga con puntos
        /// </summary>
        /// <param name="request">Datos de la venta</param>
        /// <returns></returns>
        
        public PayVisaMasterCardResponse TryReadCard(PayVisaMasterCardRequest payVisaMasterCardRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                SaleRequest saleRequest = new SaleRequest();

                //ByteToNumbers msi = new ByteToNumbers();
                //msi.SelectNumbers(saleRequest.Promo);

                Settings.SaveSettings();
                Settings.LoadSettings();
                LoadPinPad();
                
                return ReadCard(payVisaMasterCardRequest, saleRequest);
            });
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

        /// <summary>
        /// Leer los datos de la tarjeta bancaria para saber si es de debito, 
        /// crédito, se paga con puntos
        /// </summary>
        /// <param name="request">Datos de la venta</param>
        /// <returns></returns>
        public PayVisaMasterCardResponse ReadCard(PayVisaMasterCardRequest payVisaMasterCardRequest, SaleRequest request)
        {
            Tarjeta tarjeta;
            Dictionary<ParametroOperacion, object> parametros;
            SaleResponse saleResponse = new SaleResponse();
            PayVisaMasterCardResponse payVisaMasterCardResponse = new PayVisaMasterCardResponse();
            Card card = new Card();

            parametros = new Dictionary<ParametroOperacion, object>();
            parametros.Add(ParametroOperacion.Importe, request.TransactionAmount);
            parametros.Add(ParametroOperacion.ReferenciaComercio, request.MerchanReference);

            peticion_diferida.SetAfiliacion(Settings.ComercioAfiliacion, request.Dollars ? Moneda.Dolares : Moneda.Pesos);
            peticion_diferida.SetTerminal(Settings.ComercioTerminal, Settings.ComercioMac);
            peticion_diferida.Operador = Settings.Operador;
            peticion_diferida.Fecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            peticion_diferida.Amex = request.Amex;
            peticion_diferida.SetOperacion(Operacion.Venta, parametros);

            tarjeta = peticion_diferida.LeerTarjeta();
            
            payVisaMasterCardResponse.cardNumber = tarjeta.Pan;
            payVisaMasterCardResponse.isCashBack = false;
            payVisaMasterCardResponse.isSaleWithPoints = false;
            payVisaMasterCardResponse.tipoTarjeta = tarjeta.Producto;
            payVisaMasterCardResponse.code = 0;

            logDTO.EsError = false;
            logDTO.LogType = LogType.BySecuence;
            logDTO.BBVASecuence = BBVASecuence.LeerTarjeta;

            logSet.Register(logDTO);

            return payVisaMasterCardResponse;
        }
    }
}
