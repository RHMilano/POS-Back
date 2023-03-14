using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.General
{

    /// <summary>
    /// Clase que representa una lectura X
    /// </summary>
    [DataContract]
    public class LecturaX
    {

        /// <summary>
        /// Folio Corte Parcial
        /// </summary>
        [DataMember(Name = "folioCorteParcial")]
        public string FolioCorteParcial { get; set; }

        /// <summary>
        /// Ultimo Folio Venta
        /// </summary>
        [DataMember(Name = "ultimoFolioVenta")]
        public string UltimoFolioVenta { get; set; }

        /// <summary>
        /// Ultimo Folio Devolucion
        /// </summary>
        [DataMember(Name = "ultimoFolioDevolucion")]
        public string UltimoFolioDevolucion { get; set; }

        /// <summary>
        /// Ultimo Folio Apartados
        /// </summary>
        [DataMember(Name = "ultimoFolioApartados")]
        public string UltimoFolioApartados { get; set; }

        /// <summary>
        /// Ultimo Folio Retiros
        /// </summary>
        [DataMember(Name = "ultimoFolioRetiros")]
        public string UltimoFolioRetiros { get; set; }

        /// <summary>
        /// Ultimo Folio Transacciones
        /// </summary>
        [DataMember(Name = "ultimoFolioTransacciones")]
        public int UltimoFolioTransacciones { get; set; }

        /// <summary>
        /// Ultimo Folio Pagos Mayorista
        /// </summary>
        [DataMember(Name = "ultimoFolioPagosMayorista")]
        public string UltimoFolioPagosMayorista { get; set; }

        /// <summary>
        /// Ultimo Folio Pagos MM
        /// </summary>
        [DataMember(Name = "ultimoFolioPagosMM")]
        public string UltimoFolioPagosMM { get; set; }

    }
}
