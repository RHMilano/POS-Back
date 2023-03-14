using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.MM
{
    /// <summary>
    /// DTO Información correspondiente a TCMM
    /// </summary>
    [DataContract]
    public class InformacionTCMMResponse
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public InformacionTCMMResponse()
        {

        }

        /// <summary>
        /// Codigo de respuesta
        /// </summary>
        [DataMember(Name = "codigoRespuestaTCMM")]
        public CodigoRespuestaTCMM CodigoRespuestaTCMM { get; set; }

        /// <summary>
        /// Saldo en linea
        /// </summary>
        [DataMember(Name = "saldoEnLinea")]
        public decimal SaldoEnLinea { get; set; }

        /// <summary>
        /// Saldo al corte
        /// </summary>
        [DataMember(Name = "saldoAlCorte")]
        public decimal SaldoAlCorte { get; set; }

        /// <summary>
        /// Fecha limite de pago
        /// </summary>
        [DataMember(Name = "fechaLimitePago")]
        public string FechaLimitePago { get; set; }

        /// <summary>
        /// Pago minimo
        /// </summary>
        [DataMember(Name = "pagoMinimo")]
        public decimal PagoMinimo { get; set; }

        /// <summary>
        /// Saldo en puntos
        /// </summary>
        [DataMember(Name = "saldoEnPuntos")]
        public int SaldoEnPuntos { get; set; }

        /// <summary>
        /// Saldo equivalente en puntos
        /// </summary>
        [DataMember(Name = "equivalenteEnPuntos")]
        public decimal EquivalenteEnPuntos { get; set; }

        /// <summary>
        /// Puntos acumulados en el ultimo corte
        /// </summary>
        [DataMember(Name = "puntosAcumuladosUltimoCorte")]
        public int PuntosAcumuladosUltimoCorte { get; set; }

        /// <summary>
        /// Pago para no generar intereses
        /// </summary>
        [DataMember(Name = "montoPagoSinIntereses")]
        public decimal MontoPagoSinIntereses { get; set; }
    }
}
