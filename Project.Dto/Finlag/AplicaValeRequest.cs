using Milano.BackEnd.Dto.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Finlag
{
    /// <summary>
    /// Clase que ayuda a construir una peticion de para aplicar vale
    /// </summary>
    [DataContract]
    public class AplicaValeRequest
    {

        // -------------------------------------------------------------------------
        // Parámetros del POS

        /// <summary>
        /// Folio de operación asociada al pago
        /// </summary>
        [DataMember(Name = "folioOperacionAsociada")]
        public string FolioOperacionAsociada { get; set; }

        /// <summary>
        /// Codigo forma de pago para el pago correspondiente
        /// </summary>
        [DataMember(Name = "codigoFormaPagoImporte")]
        public string CodigoFormaPagoImporte { get; set; }

        /// <summary>
        /// Importe venta total
        /// </summary>
        [DataMember(Name = "importeVentaTotal")]
        public decimal ImporteVentaTotal { get; set; }

        /// <summary>
        /// Estatus del movimiento
        /// </summary>
        [DataMember(Name = "estatus")]
        public string Estatus { get; set; }

        /// <summary>
        /// Secuencia del importe
        /// </summary>
        [DataMember(Name = "secuenciaFormaPagoImporte")]
        public int SecuenciaFormaPagoImporte { get; set; }

        /// <summary>
        /// Descuentos Promocionales Aplicados por Venta para esta Forma de Pago
        /// </summary>
        [DataMember(Name = "descuentosPromocionalesPorVentaAplicados")]
        public DescuentoPromocionalesAplicados DescuentosPromocionalesPorVentaAplicados { get; set; }

        /// <summary>
        /// Descuentos Promocionales Aplicados por Linea para esta Forma de Pago
        /// </summary>
        [DataMember(Name = "descuentosPromocionalesPorLineaAplicados")]
        public DescuentoPromocionalesAplicados DescuentosPromocionalesPorLineaAplicados { get; set; }

        // -------------------------------------------------------------------------
        // Parámetros de FINLAG

        /// <summary>
        /// El folio del vale de entrada
        /// </summary>
        [DataMember(Name = "folioVale")]
        public string FolioVale { get; set; }

        /// <summary>
        /// Id de la empresa distribuidora del vale
        /// </summary>
        [DataMember(Name = "idDistribuidora")]
        public int IdDistribuidora { get; set; }

        /// <summary>
        /// Monto del vale
        /// </summary>
        [DataMember(Name = "montoVale")]
        public decimal MontoVale { get; set; }

        /// <summary>
        /// El número de quincenas en la que se dividira el vale
        /// </summary>
        [DataMember(Name = "quincenas")]
        public int Quincenas { get; set; }

        /// <summary>
        /// El tipo de pago del vale
        /// </summary>
        [DataMember(Name = "tipoPago")]
        public string TipoPago { get; set; }

        /// <summary>
        /// Fecha de nacimiento del solicitante del vale
        /// </summary>
        [DataMember(Name = "fechaNacimiento")]
        public string FechaNacimiento { get; set; }

        /// <summary>
        /// Numero de INE del solicitante del vale
        /// </summary>
        [DataMember(Name = "INE")]
        public string INE { get; set; }

        /// <summary>
        /// Nombre del solicitante del vale
        /// </summary>
        [DataMember(Name = "nombre")]
        public string Nombre { get; set; }

        /// <summary>
        /// Apellido Paterno del solicitante del vale
        /// </summary>
        [DataMember(Name = "aPaterno")]
        public string Apaterno { get; set; }

        /// <summary>
        /// Apellido Materno del solicitante del vale
        /// </summary>
        [DataMember(Name = "aMaterno")]
        public string Amaterno { get; set; }

        /// <summary>
        /// Calle de la dirección del solicitante del vale
        /// </summary>
        [DataMember(Name = "calle")]
        public string Calle { get; set; }

        /// <summary>
        /// Numero exterior de la dirección del solicitante del vale
        /// </summary>
        [DataMember(Name = "numExt")]
        public string NumExt { get; set; }

        /// <summary>
        /// Colonia de la dirección del solicitante del vale
        /// </summary>
        [DataMember(Name = "colonia")]
        public string Colonia { get; set; }

        /// <summary>
        /// Estado de la dirección del solicitante del vale
        /// </summary>
        [DataMember(Name = "estado")]
        public string Estado { get; set; }

        /// <summary>
        /// Municipio de la dirección del solicitante del vale
        /// </summary>
        [DataMember(Name = "municipio")]
        public string Municipio { get; set; }

        /// <summary>
        /// Código Postal del solicitante del vale
        /// </summary>
        [DataMember(Name = "cP")]
        public string CP { get; set; }

        /// <summary>
        /// Sexo del solicitante del vale
        /// </summary>
        [DataMember(Name = "sexo")]
        public string Sexo { get; set; }

        /// <summary>
        /// Descripción 
        /// </summary>
        [DataMember(Name = "descripcion")]
        public string Descripcion { get; set; }

        /// <summary>
        /// Puntos utilizados del solicitante del vale
        /// </summary>
        [DataMember(Name = "puntosUtilizados")]
        public string puntosUtilizados { get; set; }

        /// <summary>
        /// Efectivo puntos del solicitante del vale
        /// </summary>
        [DataMember(Name = "efectivoPuntos")]
        public string efectivoPuntos { get; set; }
    }
}
