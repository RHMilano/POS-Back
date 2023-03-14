using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// DTO Petición Busqueda Transaccion
    /// </summary>
    [DataContract]
    public class BusquedaTransaccionRequest
    {

        /// <summary>
        /// Folio de la Operación
        /// </summary>
        [DataMember(Name = "folioOperacion")]
        public string FolioOperacion { get; set; }

        /// <summary>
        /// Estatus
        /// </summary>
        [DataMember(Name = "estatus")]
        public string Estatus { get; set; }

        /// <summary>
        /// Fecha Inicial
        /// </summary>
        [DataMember(Name = "fechaInicial")]
        public string FechaInicial { get; set; }

        /// <summary>
		/// Fecha Final
		/// </summary>
		[DataMember(Name = "fechaFinal")]
        public string FechaFinal { get; set; }

    }
}
