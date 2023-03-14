using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{

    /// <summary>
    /// Clase que devuelve información referente al retiro de efectivo
    /// </summary>
    [DataContract]
    public class InformacionAsociadaRetiroEfectivo
    {

        /// <summary>
        /// Mensaje Efectivo Máximo
        /// </summary>
        [DataMember(Name = "mensajeEfectivoMaximo")]
        public string MensajeEfectivoMaximo { get; set; }

        /// <summary>
        /// Efectivo Máximo Permitido en Caja
        /// </summary>
        [DataMember(Name = "efectivoMaximoPermitidoCaja")]
        public decimal EfectivoMaximoPermitidoCaja { get; set; }

        /// <summary>
        /// Dotación Inicial en Caja
        /// </summary>
        [DataMember(Name = "dotacionInicialCaja")]
        public decimal DotacionInicialCaja { get; set; }

        /// <summary>
        /// Dotación Actual en Caja
        /// </summary>
        [DataMember(Name = "efectivoActualCaja")]
        public decimal EfectivoActualCaja { get; set; }

        /// <summary>
        /// Bandera para indicar si debe mostrarse la alerta
        /// </summary>
        [DataMember(Name = "mostrarAlertaRetiroEfectivo")]
        public Boolean MostrarAlertaRetiroEfectivo { get; set; }

        /// <summary>
        /// Bandera para indicar si debe permitirse ignorar la alerta
        /// </summary>
        [DataMember(Name = "permitirIgnorarAlertaRetiroEfectivo")]
        public Boolean PermitirIgnorarAlertaRetiroEfectivo { get; set; }

    }
}
