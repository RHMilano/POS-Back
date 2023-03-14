using System;
using System.Collections.Generic;
using System.Linq;


namespace CargaLlavesBBVAv2
{
    /// <summary>
    /// Respuesta de la PinPad almacenada
    /// </summary>
    public class SaleResponseBBVA
    {
        /// <summary>
        /// binExcepcion
        /// </summary>
        public bool binExcepcion { get; set; }

        /// <summary>
        /// afiliacion
        /// </summary>
        public string afiliacion { get; set; }

        /// <summary>
        /// afiliacionAMEX
        /// </summary>
        public string afiliacionAMEX { get; set; }

        /// <summary>
        /// moneda
        /// </summary>
        public string moneda { get; set; }
        
        /// <summary>
        /// razonSocial
        /// </summary>
        public string razonSocial { get; set; }

        /// <summary>
        /// direccion
        /// </summary>
        public string direccion { get; set; }

        /// <summary>
        /// leyenda de respuesta de la pinpad
        /// </summary>
        public string leyenda { get; set; }

        /// <summary>
        /// Número de la termial registrada en BBVA
        /// </summary>
        public string numeroTermial { get; set; }

        /// <summary>
        /// Número de la Mac Registrada en BBVA
        /// </summary>
        public string macTerminal { get; set; }

        /// <summary>
        /// codigoOperacion
        /// </summary>
        public string codigoOperacion { get; set; }

        /// <summary>
        /// numeroAutorizacion
        /// </summary>
        public string numeroAutorizacion { get; set; }

        /// <summary>
        /// nombreTransaccion
        /// </summary>
        public string nombreTransaccion { get; set; }

        /// <summary>
        /// importeTransaccion
        /// </summary>
        public decimal importeTransaccion { get; set; }

        /// <summary>
        /// referenciaComercio
        /// </summary>
        public string referenciaComercio { get; set; }

        /// <summary>
        /// importeRetiro
        /// </summary>
        public decimal importeRetiro { get; set; }

        /// <summary>
        /// comisionRetiro
        /// </summary>
        public decimal comisionRetiro { get; set; }

        /// <summary>
        /// financiamiento
        /// </summary>
        public Int16 financiamiento { get; set; }

        /// <summary>
        /// parcializacion
        /// </summary>
        public Int16 parcializacion { get; set; }

        /// <summary>
        /// codigoPromocion
        /// </summary>
        public string codigoPromocion { get; set; }

        /// <summary>
        /// vigenciaPromocionExponencial
        /// </summary>
        public string vigenciaPromocionExponencial { get; set; }

        /// <summary>
        /// saldoPuntosDisponibles
        /// </summary>
        public string saldoPuntosDisponibles { get; set; }

        /// <summary>
        /// saldoRedimidoExponencialPesos
        /// </summary>
        public string saldoRedimidoExponencialPesos { get; set; }

        /// <summary>
        /// saldoAnteriorPesos
        /// </summary>
        public string saldoAnteriorPesos { get; set; }

        /// <summary>
        /// saldoRedimidoExponencialPuntos
        /// </summary>
        public string saldoRedimidoExponencialPuntos { get; set; }

        /// <summary>
        /// pesosRedimidos
        /// </summary>
        public string pesosRedimidos { get; set; }

        /// <summary>
        /// saldoDisponiblePesos
        /// </summary>
        public string saldoDisponiblePesos { get; set; }

        /// <summary>
        /// factorExponenciacion
        /// </summary>
        public string factorExponenciacion { get; set; }

        /// <summary>
        /// saldoDisponibleExponencialPuntos
        /// </summary>
        public string saldoDisponibleExponencialPuntos { get; set; }

        /// <summary>
        /// puntosRedimidos
        /// </summary>
        public string puntosRedimidos { get; set; }

        /// <summary>
        /// saldoAnteriorPuntos
        /// </summary>
        public string saldoAnteriorPuntos { get; set; }

        /// <summary>
        /// saldoDisponibleExponencialPesos
        /// </summary>
        public string saldoDisponibleExponencialPesos { get; set; }

        /// <summary>
        /// criptogramaTarjeta
        /// </summary>
        public string criptogramaTarjeta { get; set; }

        /// <summary>
        /// numeroTarjeta
        /// </summary>
        public string numeroTarjeta { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string tarjetaHabiente { get; set; }

        /// <summary>
        /// trackii
        /// </summary>
        public string trackii { get; set; }

        /// <summary>
        /// tracki
        /// </summary>
        public string tracki { get; set; }

        /// <summary>
        /// modeloLectura
        /// </summary>
        public string modeloLectura { get; set; }

        /// <summary>
        /// productoTarjeta
        /// </summary>
        public string productoTarjeta { get; set; }

        /// <summary>
        /// emisorTarjeta
        /// </summary>
        public string emisorTarjeta { get; set; }

        /// <summary>
        /// modoLectura
        /// </summary>
        public string modoLectura { get; set; }

        /// <summary>
        /// aplicacionTarjeta
        /// </summary>
        public string aplicacionTarjeta { get; set; }

        /// <summary>
        /// idAplicacionTarjeta
        /// </summary>
        public string idAplicacionTarjeta { get; set; }

        /// <summary>
        /// referenciaFinaciera
        /// </summary>
        public string referenciaFinaciera { get; set; }

        /// <summary>
        /// secuenciaTransaccion
        /// </summary>
        public string secuenciaTransaccion { get; set; }

        /// <summary>
        /// operador
        /// </summary>
        public string operador { get; set; }

        /// <summary>
        /// firma
        /// </summary>
        public string firma { get; set; }

        /// <summary>
        /// fechaHoraTransaccion
        /// </summary>
        public string fechaHoraTransaccion { get; set; }

        /// <summary>
        /// codigoRespuesta
        /// </summary>
        public string codigoRespuesta { get; set; }

        /// <summary>
        /// monedaLocal
        /// </summary>
        public string monedaLocal { get; set; }

        /// <summary>
        /// codigoMonedaLocal
        /// </summary>
        public string codigoMonedaLocal { get; set; }

        /// <summary>
        /// nombreMonedaLocal
        /// </summary>
        public string nombreMonedaLocal { get; set; }

        /// <summary>
        /// monedaExtranjera
        /// </summary>
        public string monedaExtranjera { get; set; }

        /// <summary>
        /// importe
        /// </summary>
        public string importe { get; set; }

        /// <summary>
        /// codigoMonedaExtranjera
        /// </summary>
        public string codigoMonedaExtranjera { get; set; }

        /// <summary>
        /// montoMonedaExtranjera
        /// </summary>
        public string montoMonedaExtranjera { get; set; }

        /// <summary>
        /// nombreMonedaExtranjera
        /// </summary>
        public string nombreMonedaExtranjera { get; set; }

        /// <summary>
        /// folioExtranjero
        /// </summary>
        public string folioExtranjero { get; set; }

        /// <summary>
        /// tipoCambio
        /// </summary>
        public string tipoCambio { get; set; }

        /// <summary>
        /// comision
        /// </summary>
        public string comision { get; set; }

        /// <summary>
        /// AceptoDcc
        /// </summary>
        public string AceptoDcc { get; set; }

        /// <summary>
        /// tipoRespuesta
        /// </summary>
        public string tipoRespuesta { get; set; }

        /// <summary>
        /// cash
        /// </summary>
        public string cash { get; set; }

        /// <summary>
        /// cashComision
        /// </summary>
        public string cashComision { get; set; }

        /// <summary>
        /// referenciaFinanciera
        /// </summary>
        public string referenciaFinanciera { get; set; }

    }
}
