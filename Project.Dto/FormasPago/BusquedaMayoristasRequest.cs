using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// Dto para busqueda de mayorista
    /// </summary>
    [DataContract]
    public class BusquedaMayoristasRequest
    {
        /// <summary>
        /// Código de mayorista
        /// </summary>
        [DataMember(Name = "codigoMayorista")]
        public int CodigoMayorista { get; set; }
        /// <summary>
        /// Nombre del mayorista
        /// </summary>
        [DataMember(Name = "nombre")]
        public string Nombre { get; set; }
        /// <summary>
        /// Bandera de busqueda para solo activos
        /// </summary>
        [DataMember(Name = "soloActivos")]
        public bool SoloActivos { get; set; }
        /// <summary>
        /// Bandera de busqueda para solo activos
        /// </summary>
        [DataMember(Name = "soloTiendaActual")]
        public bool SoloTiendaActual { get; set; }

    }
}
