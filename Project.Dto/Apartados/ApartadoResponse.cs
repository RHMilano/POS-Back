using Milano.BackEnd.Dto.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// Dto de apartado
    /// </summary>
    [DataContract]
    public class ApartadoResponse
    {

        [DataMember(Name = "Apartados")]
        public ApartadosEncontradosResponse[] Apartados { get; set; }

        /// <summary>
        /// Información asociada de Formas de Pago que deben estar disponibles
        /// </summary>
        [DataMember(Name = "informacionAsociadaFormasPago")]
        public ConfigGeneralesCajaTiendaFormaPago[] InformacionAsociadaFormasPago { get; set; }

        /// <summary>
        /// Información asociada de Formas de Pago Moneda Extranjera que deben estar disponibles
        /// </summary>
        [DataMember(Name = "informacionAsociadaFormasPagoMonedaExtranjera")]
        public ConfigGeneralesCajaTiendaFormaPago[] InformacionAsociadaFormasPagoMonedaExtranjera { get; set; }

    }
}
