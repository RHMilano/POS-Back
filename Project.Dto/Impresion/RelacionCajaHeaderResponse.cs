using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Impresion
{
    /// <summary>
    /// Datos para el header de la relacion de caja 
    /// </summary>
    [DataContract]
   public class RelacionCajaHeaderResponse
    {
        /// <summary>
        /// Codigo de la tienda 
        /// </summary>
        [DataMember(Name = "codigoTienda")]
        public int CodigoTienda { get; set; }
        /// <summary>
        /// Nombre de la marca
        /// </summary>
        [DataMember(Name = "marca")]
        public string Marca { get; set; }
        /// <summary>
        /// Direccion de la tienda
        /// </summary>
        [DataMember(Name = "direccion")]
        public string Direccion { get; set; }
        /// <summary>
        /// Telefono de la tienda
        /// </summary>
        [DataMember(Name = "telefono")]
        public string Telefono { get; set; }
        /// <summary>
        /// Fecha de emision del reporte
        /// </summary>
        [DataMember(Name = "fecha")]
        public string Fecha { get; set; }
        /// <summary>
        /// Descripcion de la tienda
        /// </summary>
        [DataMember(Name = "descripcionTienda")]
        public string DescripcionTienda { get; set; }

        /// <summary>
        /// Fecha de operacion
        /// </summary>
        [DataMember(Name = "fechaOperacion")]
        public string FechaOperacion { get; set; }


        /// <summary>
        /// Fecha hora de inicio de dia
        /// </summary>
        [DataMember(Name = "fechaHoraInicioDedia")]
        public string FechaHoraInicioDedia { get; set; }

        /// <summary>
        /// Fecha hora de inicio de dia
        /// </summary>
        [DataMember(Name = "fechaCorte")]
        public string FechaCorte { get; set; }


    }
}
