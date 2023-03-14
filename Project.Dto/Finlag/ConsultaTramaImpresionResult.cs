using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Finlag
{

    /// <summary>
    /// Clase DTO para almacenar la información que debe imprimirse
    /// </summary>
    [DataContract]
    public class ConsultaTramaImpresionResult
    {

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "fecha")]
        public string Fecha { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "puntoVenta")]
        public string PuntoVenta { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "idDistribuidora")]
        public string IdDistribuidora { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "folioVale")]
        public string FolioVale { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "nombreCompleto")]
        public string NombreCompleto { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "calle")]
        public string Calle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "numeroExt")]
        public string NumeroExt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "colonia")]
        public string Colonia { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "CP")]
        public string CP { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "municipio")]
        public string Municipio { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "estado")]
        public string Estado { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "quincenas")]
        public string Quincenas { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "montoAplicado")]
        public string MontoAplicado { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "totalPagar")]
        public string TotalPagar { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "pagoQuincenal")]
        public string PagoQuincenal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "fechaPrimerPago")]
        public string FechaPrimerPago { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "pagare")]
        public string Pagare { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "tiendaAplica")]
        public string TiendaAplica { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "idCajaAplica")]
        public string IdCajaAplica { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "tipoVenta")]
        public string TipoVenta { get; set; }

        /// <summary>
        /// Puntos utilizados del solicitante del vale
        /// </summary>
        [DataMember(Name = "puntosUtilizados")]
        public string PuntosUtilizados { get; set; }

        /// <summary>
        /// Efectivo puntos del solicitante del vale
        /// </summary>
        [DataMember(Name = "efectivoPuntos")]
        public string EfectivoPuntos { get; set; }

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
