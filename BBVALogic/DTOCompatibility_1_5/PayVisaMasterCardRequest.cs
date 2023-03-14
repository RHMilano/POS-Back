using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBVALogic.DTOCompatibility_1_5
{
    public class PayVisaMasterCardRequest
    {
        public int sessionNumber { get; set; }
        public string commerceReference { get; set; }
        public decimal transactionAmount { get; set; }
        public short financialMonths { get; set; }
        public short paymentsPartial { get; set; }
        public short promotion { get; set; }
        public int secuenciaPos { get; set; }
    }
}
