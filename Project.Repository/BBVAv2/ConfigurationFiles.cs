using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.BBVAv2;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milano.BackEnd.Repository.BBVAv2
{
    /// <summary>
    /// Clase para registrar la respuesta de la pinpad en la base de datos y
    /// recuperar la configuración de la misma al macenada en la base de datos
    /// </summary>
    public class ConfigurationFiles : BaseRepository
    {
        /// <summary>
        /// Inserta la respuesta de la pinpad
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public OperationResponse InsertSaleResponse(SaleResponseBBVA response)
        {
            OperationResponse operationResponse = new OperationResponse();

            var parameters = new Dictionary<string, object>();
            parameters.Add("@binExcepcion", response.binExcepcion);
            parameters.Add("@afiliacion", response.afiliacion);
            parameters.Add("@afiliacionAMEX", response.afiliacionAMEX);
            parameters.Add("@moneda", response.moneda);
            parameters.Add("@razonSocial", response.razonSocial);
            parameters.Add("@direccion", response.direccion);
            parameters.Add("@leyenda", response.leyenda);
            parameters.Add("@numeroTermial", response.numeroTermial);
            parameters.Add("@macTerminal", response.macTerminal);
            parameters.Add("@codigoOperacion", response.codigoOperacion);
            parameters.Add("@numeroAutorizacion", response.numeroAutorizacion);
            parameters.Add("@nombreTransaccion", response.nombreTransaccion);
            parameters.Add("@importeTransaccion", response.importeTransaccion);
            parameters.Add("@referenciaComercio", response.referenciaComercio);
            parameters.Add("@importeRetiro", response.importeRetiro);
            parameters.Add("@comisionRetiro", response.comisionRetiro);
            parameters.Add("@financiamiento", response.financiamiento);
            parameters.Add("@parcializacion", response.parcializacion);
            parameters.Add("@codigoPromocion", response.codigoPromocion);
            parameters.Add("@vigenciaPromocionExponencial", response.vigenciaPromocionExponencial);
            parameters.Add("@saldoPuntosDisponibles", response.saldoPuntosDisponibles);
            parameters.Add("@saldoRedimidoExponencialPesos", response.saldoRedimidoExponencialPesos);
            parameters.Add("@saldoAnteriorPesos", response.saldoAnteriorPesos);
            parameters.Add("@saldoRedimidoExponencialPuntos", response.saldoRedimidoExponencialPuntos);
            parameters.Add("@pesosRedimidos", response.pesosRedimidos);
            parameters.Add("@saldoDisponiblePesos", response.saldoDisponiblePesos);
            parameters.Add("@factorExponenciacion", response.factorExponenciacion);
            parameters.Add("@saldoDisponibleExponencialPuntos", response.saldoDisponibleExponencialPuntos);
            parameters.Add("@puntosRedimidos", response.puntosRedimidos);
            parameters.Add("@saldoAnteriorPuntos", response.saldoAnteriorPuntos);
            parameters.Add("@saldoDisponibleExponencialPesos", response.saldoDisponibleExponencialPesos);
            parameters.Add("@criptogramaTarjeta", response.criptogramaTarjeta);
            parameters.Add("@numeroTarjeta", response.numeroTarjeta);
            parameters.Add("@tarjetaHabiente", response.tarjetaHabiente);
            parameters.Add("@trackii", response.trackii);
            parameters.Add("@tracki", response.tracki);
            parameters.Add("@modeloLectura", response.modeloLectura);
            parameters.Add("@productoTarjeta", response.productoTarjeta);
            parameters.Add("@emisorTarjeta", response.emisorTarjeta);
            parameters.Add("@modoLectura", response.modoLectura);
            parameters.Add("@aplicacionTarjeta", response.aplicacionTarjeta);
            parameters.Add("@idAplicacionTarjeta", response.idAplicacionTarjeta);
            parameters.Add("@referenciaFinaciera", response.referenciaFinaciera);
            parameters.Add("@secuenciaTransaccion", response.secuenciaTransaccion);
            parameters.Add("@operador", response.operador);
            parameters.Add("@firma", response.firma);
            parameters.Add("@fechaHoraTransaccion", response.fechaHoraTransaccion);
            parameters.Add("@codigoRespuesta", response.codigoRespuesta);
            parameters.Add("@monedaLocal", response.monedaLocal);
            parameters.Add("@codigoMonedaLocal", response.codigoMonedaLocal);
            parameters.Add("@nombreMonedaLocal", response.nombreMonedaLocal);
            parameters.Add("@monedaExtranjera", response.monedaExtranjera);
            parameters.Add("@importe", response.importe);
            parameters.Add("@codigoMonedaExtranjera", response.codigoMonedaExtranjera);
            parameters.Add("@montoMonedaExtranjera", response.montoMonedaExtranjera);
            parameters.Add("@nombreMonedaExtranjera", response.nombreMonedaExtranjera);
            parameters.Add("@folioExtranjero", response.folioExtranjero);
            parameters.Add("@tipoCambio", response.tipoCambio);
            parameters.Add("@comision", response.comision);
            parameters.Add("@AceptoDcc", response.AceptoDcc);
            parameters.Add("@tipoRespuesta", response.tipoRespuesta);
            parameters.Add("@cash", response.cash);
            parameters.Add("@cashComision", response.cashComision);
            parameters.Add("@referenciaFinanciera", response.referenciaFinanciera);

            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_AgregarRespuestaPinPad2]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }

        /// <summary>
        /// Recupera la configuración de la PinPad para escribirla en el archivo de
        /// configuración
        /// </summary>
        public SettingsMilano GetBBVAConfig()
        {
            SettingsMilano settingsMilano = new SettingsMilano();
            var parameters = new Dictionary<string, object>();

            foreach (var c in data.GetDataReader("dbo.sp_vanti_BuscarConfiguracionPinPadPorVersion2", parameters))
            {

                settingsMilano.Logs = c.GetBoolean(0);
                settingsMilano.Operador = c.GetString(1);
                settingsMilano.ClaveLogs = c.GetString(2);
                settingsMilano.PinPadConexion = c.GetString(3);
                settingsMilano.PinPadTimeOut = c.GetByte(4).ToString();
                settingsMilano.PinPadPuertoWiFi = c.GetString(5);
                settingsMilano.PinPadMensaje = c.GetString(6);
                settingsMilano.ClaveBinesExcepcion = c.GetString(7);
                settingsMilano.HostUrl = c.GetString(8);
                settingsMilano.BinesUrl = c.GetString(9);
                settingsMilano.TokenUrl = c.GetString(10);
                settingsMilano.TelecargaUrl = c.GetString(11);
                settingsMilano.HostTimeOut = c.GetByte(12).ToString();
                settingsMilano.ComercioAfiliacion = c.GetString(13);
                settingsMilano.ComercioTerminal = c.GetString(14);
                settingsMilano.ComercioMac = c.GetString(15);
                settingsMilano.IdAplicacion = c.GetString(16);
                settingsMilano.ClaveSecreta = c.GetString(17);
                settingsMilano.PinPadContactless = c.GetBoolean(18);
                settingsMilano.FuncionalidadGaranti = c.GetBoolean(19);
                settingsMilano.FuncionalidadMoto = c.GetBoolean(20);
                settingsMilano.TecladoLiberado = c.GetBoolean(21);
                settingsMilano.Correcto = true;
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

        /// <summary>
        /// Retorna la lectura de la tarjeta y la configuración de MSI 
        /// para tiendas milano
        /// </summary>
        /// <param name="emisor">Número de emisor (Identificador Banco)</param>
        /// <returns></returns>
        public ConfigMSI ConfigMSIMilano(int emisor)
        {
            ConfigMSI configMSI = new ConfigMSI();

            var parameters = new Dictionary<string, object>();
            parameters.Add("@Emisor", emisor);

            foreach (var c in data.GetDataReader("spcnfBancosMSI", parameters))
            {
                configMSI.MesesSinInteresesVisa = c.GetInt32(2);
                configMSI.MontoMinimoVisa = c.GetInt32(3);
                configMSI.MontoMaximoVisa = c.GetInt32(4);
            }

            return configMSI;
        }
    }
}
