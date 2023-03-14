using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Milano.BackEnd.Dto.Sales
{
    public class AutorizaCancelacionTransaccionResponse
    {
        public AutorizaCancelacionTransaccionResponse()
        {
            hayError = 0;
            autorizado = 0;
            sMensaje = "Sin resultados";
        }
        public int hayError { get; set; }
        public int autorizado { get; set; }
        public string sMensaje { get; set; }
    }
}
