using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBVALogic.DTOCompatibility_1_5
{
    public class PayVisaMasterCardResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public string authorization { get; set; }
        public string cardNumber { get; set; }
        public string tipoTarjeta { get; set; }
        public bool isCashBack { get; set; }
        public bool isSaleWithPoints { get; set; }
    }
}
