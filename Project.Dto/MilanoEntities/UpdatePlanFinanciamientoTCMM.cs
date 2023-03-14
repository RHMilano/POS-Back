using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Milano.BackEnd.Dto.MilanoEntities
{
    public class UpdatePlanFinanciamientoTCMM
    {
        public string folioOperacion { get; set; }
        public int codigoTienda { get; set; }
        public int codigoCaja { get; set; }
        public DateTime fechaActualizacion { get; set; }
        public string codigoFormaPago { get; set; }
        public string financiamientoId { get; set; }
        public string montoMensualidad { get; set; }
    }
}
