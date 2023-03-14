using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// DTO Respuesta Busqueda Transaccion
    /// </summary>
    [DataContract]
    public class BusquedaTransaccionResponse
    {

        /// <summary>
        /// Folio de la Operación
        /// </summary>
        [DataMember(Name = "folioOperacion")]
        public string FolioOperacion { get; set; }

        /// <summary>
        /// Código de Tienda
        /// </summary>
        [DataMember(Name = "codigoTienda")]
        public int CodigoTienda { get; set; }

        /// <summary>
        /// Código de Caja
        /// </summary>
        [DataMember(Name = "codigoCaja")]
        public int CodigoCaja { get; set; }

        /// <summary>
		/// Fecha y Hora de la Venta
		/// </summary>
		[DataMember(Name = "fecha")]
        public string Fecha { get; set; }

    }
}
