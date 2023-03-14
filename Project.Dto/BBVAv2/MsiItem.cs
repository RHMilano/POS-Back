using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milano.BackEnd.Dto.BBVAv2
{
    /// <summary>
    /// Elemento para los meses sin intereses
    /// </summary>
    public class MsiItem
    {

        /// <summary>
        /// Texto a mostrar en el combo
        /// </summary>
        public string leyenda { get; set; }

        /// <summary>
        /// Número de meses promocionados
        /// </summary>
        public int promo { get; set; }


    }
}
