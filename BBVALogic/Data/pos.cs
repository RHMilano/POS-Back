using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BBVALogic.DTO;
using BBVALogic.DTO.Retail;
using BBVALogic.DTO.Setings;
using Tools;

namespace BBVALogic.Data
{
    public class Pos
    {
        string cnn = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;
        LogSet logSet = new LogSet();
        LogDTO logDTO = new LogDTO();

        public SettingsMilano GetBBVAConfig()
        {
            SettingsMilano settingsMilano = new SettingsMilano();
            //try
            //{
            using (SqlConnection cn = new SqlConnection(cnn))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand("sp_vanti_BuscarConfiguracionPinPadPorVersion2", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    settingsMilano.Logs = dr.GetBoolean(0);
                    settingsMilano.Operador = dr.GetString(1);
                    settingsMilano.ClaveLogs = dr.GetString(2);
                    settingsMilano.PinPadConexion = dr.GetString(3);
                    settingsMilano.PinPadTimeOut = dr.GetByte(4).ToString();
                    settingsMilano.PinPadPuertoWiFi = dr.GetString(5);
                    settingsMilano.PinPadMensaje = dr.GetString(6);
                    settingsMilano.ClaveBinesExcepcion = dr.GetString(7);
                    settingsMilano.HostUrl = dr.GetString(8);
                    settingsMilano.BinesUrl = dr.GetString(9);
                    settingsMilano.TokenUrl = dr.GetString(10);
                    settingsMilano.TelecargaUrl = dr.GetString(11);
                    settingsMilano.HostTimeOut = dr.GetByte(12).ToString();
                    settingsMilano.ComercioAfiliacion = dr.GetString(13);
                    settingsMilano.ComercioTerminal = dr.GetString(14);
                    settingsMilano.ComercioMac = dr.GetString(15);
                    settingsMilano.IdAplicacion = dr.GetString(16);
                    settingsMilano.ClaveSecreta = dr.GetString(17);
                    settingsMilano.PinPadContactless = dr.GetBoolean(18);
                    settingsMilano.FuncionalidadGaranti = dr.GetBoolean(19);
                    settingsMilano.FuncionalidadMoto = dr.GetBoolean(20);
                    settingsMilano.TecladoLiberado = dr.GetBoolean(21);
                    settingsMilano.Correcto = true;
                }
            }
            return settingsMilano;
            //}
            //catch (Exception ex)
            //{
            //    logDTO.EsError = true;
            //    logDTO.LogType = LogType.Bloque;
            //    logDTO.BBVASecuence = BBVASecuence.NoSecuence;
            //    logDTO.Message = ex.Message;
            //    logDTO.StackTrace = ex.StackTrace;

            //    logSet.Register(logDTO);

            //    settingsMilano.Correcto = false;

            //    return settingsMilano;
            //}
        }

        public bool InsertSaleResponse(SaleResponse response)
        {
            using (SqlConnection cn = new SqlConnection(cnn))
            {

                SqlCommand cmd = new SqlCommand("sp_vanti_AgregarRespuestaPinPad2", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@binExcepcion", response.binExcepcion);
                cmd.Parameters.AddWithValue("@afiliacion", response.afiliacion);
                cmd.Parameters.AddWithValue("@afiliacionAMEX", response.afiliacionAMEX);
                cmd.Parameters.AddWithValue("@moneda", response.moneda);
                cmd.Parameters.AddWithValue("@razonSocial", response.razonSocial);
                cmd.Parameters.AddWithValue("@direccion", response.direccion);
                cmd.Parameters.AddWithValue("@leyenda", response.leyenda);
                cmd.Parameters.AddWithValue("@numeroTermial", response.numeroTermial);
                cmd.Parameters.AddWithValue("@macTerminal", response.macTerminal);
                cmd.Parameters.AddWithValue("@codigoOperacion", response.codigoOperacion);
                cmd.Parameters.AddWithValue("@numeroAutorizacion", response.numeroAutorizacion);
                cmd.Parameters.AddWithValue("@nombreTransaccion", response.nombreTransaccion);
                cmd.Parameters.AddWithValue("@importeTransaccion", response.importeTransaccion);
                cmd.Parameters.AddWithValue("@referenciaComercio", response.referenciaComercio);
                cmd.Parameters.AddWithValue("@importeRetiro", response.importeRetiro);
                cmd.Parameters.AddWithValue("@comisionRetiro", response.comisionRetiro);
                cmd.Parameters.AddWithValue("@financiamiento", response.financiamiento);
                cmd.Parameters.AddWithValue("@parcializacion", response.parcializacion);
                cmd.Parameters.AddWithValue("@codigoPromocion", response.codigoPromocion);
                cmd.Parameters.AddWithValue("@vigenciaPromocionExponencial", response.vigenciaPromocionExponencial);
                cmd.Parameters.AddWithValue("@saldoPuntosDisponibles", response.saldoPuntosDisponibles);
                cmd.Parameters.AddWithValue("@saldoRedimidoExponencialPesos", response.saldoRedimidoExponencialPesos);
                cmd.Parameters.AddWithValue("@saldoAnteriorPesos", response.saldoAnteriorPesos);
                cmd.Parameters.AddWithValue("@saldoRedimidoExponencialPuntos", response.saldoRedimidoExponencialPuntos);
                cmd.Parameters.AddWithValue("@pesosRedimidos", response.pesosRedimidos);
                cmd.Parameters.AddWithValue("@saldoDisponiblePesos", response.saldoDisponiblePesos);
                cmd.Parameters.AddWithValue("@factorExponenciacion", response.factorExponenciacion);
                cmd.Parameters.AddWithValue("@saldoDisponibleExponencialPuntos", response.saldoDisponibleExponencialPuntos);
                cmd.Parameters.AddWithValue("@puntosRedimidos", response.puntosRedimidos);
                cmd.Parameters.AddWithValue("@saldoAnteriorPuntos", response.saldoAnteriorPuntos);
                cmd.Parameters.AddWithValue("@saldoDisponibleExponencialPesos", response.saldoDisponibleExponencialPesos);
                cmd.Parameters.AddWithValue("@criptogramaTarjeta", response.criptogramaTarjeta);
                cmd.Parameters.AddWithValue("@numeroTarjeta", response.numeroTarjeta);
                cmd.Parameters.AddWithValue("@tarjetaHabiente", response.tarjetaHabiente);
                cmd.Parameters.AddWithValue("@trackii", response.trackii);
                cmd.Parameters.AddWithValue("@tracki", response.tracki);
                cmd.Parameters.AddWithValue("@modeloLectura", response.modeloLectura);
                cmd.Parameters.AddWithValue("@productoTarjeta", response.productoTarjeta);
                cmd.Parameters.AddWithValue("@emisorTarjeta", response.emisorTarjeta);
                cmd.Parameters.AddWithValue("@modoLectura", response.modoLectura);
                cmd.Parameters.AddWithValue("@aplicacionTarjeta", response.aplicacionTarjeta);
                cmd.Parameters.AddWithValue("@idAplicacionTarjeta", response.idAplicacionTarjeta);
                cmd.Parameters.AddWithValue("@referenciaFinaciera", response.referenciaFinaciera);
                cmd.Parameters.AddWithValue("@secuenciaTransaccion", response.secuenciaTransaccion);
                cmd.Parameters.AddWithValue("@operador", response.operador);
                cmd.Parameters.AddWithValue("@firma", response.firma);
                cmd.Parameters.AddWithValue("@fechaHoraTransaccion", response.fechaHoraTransaccion);
                cmd.Parameters.AddWithValue("@codigoRespuesta", response.codigoRespuesta);
                cmd.Parameters.AddWithValue("@monedaLocal", response.monedaLocal);
                cmd.Parameters.AddWithValue("@codigoMonedaLocal", response.codigoMonedaLocal);
                cmd.Parameters.AddWithValue("@nombreMonedaLocal", response.nombreMonedaLocal);
                cmd.Parameters.AddWithValue("@monedaExtranjera", response.monedaExtranjera);
                cmd.Parameters.AddWithValue("@importe", response.importe);
                cmd.Parameters.AddWithValue("@codigoMonedaExtranjera", response.codigoMonedaExtranjera);
                cmd.Parameters.AddWithValue("@montoMonedaExtranjera", response.montoMonedaExtranjera);
                cmd.Parameters.AddWithValue("@nombreMonedaExtranjera", response.nombreMonedaExtranjera);
                cmd.Parameters.AddWithValue("@folioExtranjero", response.folioExtranjero);
                cmd.Parameters.AddWithValue("@tipoCambio", response.tipoCambio);
                cmd.Parameters.AddWithValue("@comision", response.comision);
                cmd.Parameters.AddWithValue("@AceptoDcc", response.AceptoDcc);
                cmd.Parameters.AddWithValue("@tipoRespuesta", response.tipoRespuesta);
                cmd.Parameters.AddWithValue("@cash", response.cash);
                cmd.Parameters.AddWithValue("@cashComision", response.cashComision);
                cmd.Parameters.AddWithValue("@referenciaFinanciera", response.referenciaFinanciera);
                //cmd.Parameters.AddWithValue("@secuenciaPos", response.secuenciaPos);
                //cmd.Parameters.AddWithValue("@fechaHoraComercio", response.fechaHoraComercio);

                //SqlDataReader dr = cmd.ExecuteReader();
                cn.Open();
                cmd.ExecuteNonQuery();

                logDTO.EsError = false;
                logDTO.LogType = LogType.Bloque;
                logDTO.BBVASecuence = BBVASecuence.NoSecuence;
                logDTO.Message = "Se registro la respuesta de la PinPad.";

                logSet.Register(logDTO);
                return true;
            }
        }
    }
}
