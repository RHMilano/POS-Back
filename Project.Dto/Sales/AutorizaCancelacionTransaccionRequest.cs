using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Sales
{

    /// <summary>
    /// Peticion para el web service 
    /// </summary>
    public class AutorizaCancelacionTransaccionRequest
    {
        public int codigoTipoTrxCab { get; set; }
        public string folioVenta { get; set; }
        public int transaccion { get; set; }
        public string nombreCajero { get; set; }
        public int codigoCajeroAutorizo { get; set; }
        public decimal totalTransaccion { get; set; }
        public decimal totalTransaccionPositivo { get; set; }
        public int totalPiezas { get; set; }
        public int totalPiezasPositivas { get; set; }
        public string codigoRazonMMS { get; set; }
    }
}
