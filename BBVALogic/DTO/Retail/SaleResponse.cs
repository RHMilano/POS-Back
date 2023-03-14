using System;
using System.Collections.Generic;
using System.Linq;


namespace BBVALogic.DTO.Retail
{
    public class SaleResponse
    {
        public bool binExcepcion { get; set; }
        public string afiliacion { get; set; }
        public string afiliacionAMEX { get; set; }
        public string moneda { get; set; }
        public string razonSocial { get; set; }
        public string direccion { get; set; }
        public string leyenda { get; set; }
        public string numeroTermial { get; set; }
        public string macTerminal { get; set; }
        public string codigoOperacion { get; set; }
        public string numeroAutorizacion { get; set; }
        public string nombreTransaccion { get; set; }
        public decimal importeTransaccion { get; set; }
        public string referenciaComercio { get; set; }
        public decimal importeRetiro { get; set; }
        public decimal comisionRetiro { get; set; }
        public Int16 financiamiento { get; set; }
        public Int16 parcializacion { get; set; }
        public string codigoPromocion { get; set; }
        public string vigenciaPromocionExponencial { get; set; }
        public string saldoPuntosDisponibles { get; set; }
        public string saldoRedimidoExponencialPesos { get; set; }
        public string saldoAnteriorPesos { get; set; }
        public string saldoRedimidoExponencialPuntos { get; set; }
        public string pesosRedimidos { get; set; }
        public string saldoDisponiblePesos { get; set; }
        public string factorExponenciacion { get; set; }
        public string saldoDisponibleExponencialPuntos { get; set; }
        public string puntosRedimidos { get; set; }
        public string saldoAnteriorPuntos { get; set; }
        public string saldoDisponibleExponencialPesos { get; set; }
        public string criptogramaTarjeta { get; set; }
        public string numeroTarjeta { get; set; }
        public string tarjetaHabiente { get; set; }
        public string trackii { get; set; }
        public string tracki { get; set; }
        public string modeloLectura { get; set; }
        public string productoTarjeta { get; set; }
        public string emisorTarjeta { get; set; }
        public string modoLectura { get; set; }
        public string aplicacionTarjeta { get; set; }
        public string idAplicacionTarjeta { get; set; }
        public string referenciaFinaciera { get; set; }
        public string secuenciaTransaccion { get; set; }
        public string operador { get; set; }
        public string firma { get; set; }
        public string fechaHoraTransaccion { get; set; }
        public string codigoRespuesta { get; set; }
        public string monedaLocal { get; set; }
        public string codigoMonedaLocal { get; set; }
        public string nombreMonedaLocal { get; set; }
        public string monedaExtranjera { get; set; }
        public string importe { get; set; }
        public string codigoMonedaExtranjera { get; set; }
        public string montoMonedaExtranjera { get; set; }
        public string nombreMonedaExtranjera { get; set; }
        public string folioExtranjero { get; set; }
        public string tipoCambio { get; set; }
        public string comision { get; set; }
        public string AceptoDcc { get; set; }
        public string tipoRespuesta { get; set; }
        public string cash { get; set; }
        public string cashComision { get; set; }
        public string referenciaFinanciera { get; set; }
        public int secuenciaPos { get; set; }
        public DateTime fechaHoraComercio { get; set; }



    }
}
