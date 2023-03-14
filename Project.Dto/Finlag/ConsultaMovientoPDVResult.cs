using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Finlag
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class ConsultaMovientoPDVResult
    {

        [DataMember(Name = "tiendaAplica")]
        public int TiendaAplica { get; set; }

        [DataMember(Name = "folioVale")]
        public string FolioVale { get; set; }

        [DataMember(Name = "fechaAplicacion")]
        public DateTime FechaAplicacion { get; set; }

        [DataMember(Name = "folioVenta")]
        public int FolioVenta { get; set; }

        [DataMember(Name = "idDistribuidora")]
        public string IdDistribuidora { get; set; }

        [DataMember(Name = "puntosUtilizados")]
        public int PuntosUtilizados { get; set; }

        [DataMember(Name = "efectivoEquivalente")]
        public int EfectivoEquivalente { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "NumeroCodigo")]
        public int NumeroCodigo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "DescripcionCodigo")]
        public string DescripcionCodigo { get; set; }
    }
}
