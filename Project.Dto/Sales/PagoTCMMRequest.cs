using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Milano.BackEnd.Dto
{
	public class PagoTCMMRequest
	{
		public string NumeroTarjeta { get; set; }


		public int NumeroTienda { get; set; }

		public int NumeroCaja { get; set; }

		public int Transaccion { get; set; }

		public decimal Importe { get; set; }

		public int ModoEntrada { get; set; }

	
	}
}
