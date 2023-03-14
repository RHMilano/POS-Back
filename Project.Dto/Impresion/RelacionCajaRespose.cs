﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Impresion
{

    /// <summary>
    /// Datos para egresos o ingresos
    /// </summary>
    [DataContract]
    public class RelacionCajaRespose
    {
        /// <summary>
        /// Id de la relacion de caja
        /// </summary>
        [DataMember(Name = "id")]
        public int Id { get; set; }
        /// <summary>
        /// Id de la relacion de caja
        /// </summary>
        [DataMember(Name = "descripcion")]
        public string Descripcion { get; set; }
        /// <summary>
        /// Id de la relacion de caja
        /// </summary>
        [DataMember(Name = "totalConIva")]
        public decimal TotalConIva { get; set; }
        /// <summary>
        /// Id de la relacion de caja
        /// </summary>
        [DataMember(Name = "totalSinIva")]
        public decimal TotalSinIva { get; set; }
        /// <summary>
        /// Id de la relacion de caja
        /// </summary>
        [DataMember(Name = "iva")]
        public decimal Iva { get; set; }
        /// <summary>
        /// Id de la relacion de caja
        /// </summary>
        [DataMember(Name = "seccion")]
        public List<RelacionCajaDesgloseRespose> Seccion { get; set; }
    }
}
