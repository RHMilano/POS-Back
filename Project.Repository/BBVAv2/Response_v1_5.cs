using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milano.BackEnd.Repository.BBVAv2
{
	public class Response_v1_5
	{
		public enum TransactionCodes
		{
			Sale = 1
		}

		public enum ResponseCodes
		{
			R1 = -1,
			NoSePudoLeerLaTarjeta = -2,
			Approved = 0,
			CallTransmitter = 1,
			InvalidTerminal = 3,
			HoldCall = 4,
			Declined = 5,
			TransactionNotCorrespondAffiliation = 8,
			InvalidAmount = 12,
			InvalidCard = 14,
			Retry = 19,
			ServiceNotAvailable = 28,
			ErrorFormat = 30,
			ExpiredCard = 33,
			PromotionsNotAllowed = 45,
			LowerAmountThatMinimiumPromotion = 46,
			TransactionNotRealizedComeOffice = 47,
			JoinSecurityCode = 48,
			ErrorSecurityCode = 49,
			OvercomeLimitTransacionNumber = 50,
			InsufficientFounds = 51,
			IncorrectPin = 55,
			RecordNotFound = 56,
			TransactionCashBackNotAllowed = 57,
			InvalidOperation = 58,
			LimitExceeded = 61,
			CashBackExceeded = 65,
			TransmitterNotAvailable = 81,
			ServiceNotAvailable2 = 82,
			OkProcessed = 87,
			Reversed = 88,

		}

		public string ReferenciaComercio { get; set; }
		public TransactionCodes TransactionCode { get; set; }
		public string TerminalNumber { get; set; }
		public int SessionNumber { get; set; }
		public int TransactionSequence { get; set; }
		public ResponseCodes ResponseCode { get; set; }
		public string Authorization { get; set; }
		public string Affiliation { get; set; }
		public decimal TransactionAmount { get; set; }
		public decimal Tip { get; set; }
		public string Card { get; set; }
		public string OperatorKey { get; set; }
		public string RoomNumber { get; set; }
		public string Folio { get; set; }
		public int LegendLength { get; set; }
		public string LegendResponse { get; set; }
		public string FinancialReference { get; set; }
		public string Message { get; set; }


		public string DebitoCredito { get; set; }
		public bool FirmaAutografa { get; set; }
		public bool FirmaElectronica { get; set; }
		public bool FirmaQps { get; set; }
		public string ModoIngreso { get; set; }

		public string NombreAplicacion { get; set; }
		public string Criptograma { get; set; }
		public string AID { get; set; }
		public string NombreCliente { get; set; }
		public bool PagoConPuntos { get; set; }



		//6
		public string MontoPagadoConPuntos { get; set; }
		//7
		public string SaldoAnteriorPuntos { get; set; }
		//8
		public decimal ImportePesosAnterior { get; set; }
		//9
		public string SaldoRedimidoPuntos { get; set; }
		//10
		public decimal ImporteRedimidoPesos { get; set; }
		//11
		public string SaldoActualPuntos { get; set; }
		//12
		public decimal SaldoPuntosImportePesos { get; set; }
		//13
		public bool RequirioCashBack { get; set; }
		//14
		public string DisponibleEfectivo { get; set; }
		//15
		public decimal TotalPagoCash { get; set; }


		public int SecuenciaPos = 0;

		public Response_v1_5()
		{
			this.NombreAplicacion = "";
			this.Criptograma = "";
			this.AID = "";
			this.NombreCliente = "";



			//6
			this.MontoPagadoConPuntos = "";
			//7
			this.SaldoAnteriorPuntos = "";
			//9
			this.SaldoRedimidoPuntos = "";
			//11
			this.SaldoActualPuntos = "";
			//12
			this.DisponibleEfectivo = "";

			this.Authorization = "";


		}

		public void Build(string message)
		{
			Message = message;
			try
			{
				TransactionCode = (TransactionCodes)Convert.ToInt32(Message.Substring(0, 3));
				TerminalNumber = Message.Substring(3, 8);
				SessionNumber = Convert.ToInt32(Message.Substring(11, 4));
				TransactionSequence = Convert.ToInt32(Message.Substring(15, 4));
				if (message.Substring(19, 2).ToLower() == "R1".ToLower())
				{

					ResponseCode = (ResponseCodes)(-1);
					this.LegendResponse = "R1";

				}
				else
				{
					ResponseCode = (ResponseCodes)Convert.ToInt32(message.Substring(19, 2));
				}
				Authorization = Message.Substring(21, 6).Trim();
				Affiliation = Message.Substring(27, 8);

				string transactionAmountParts = message.Substring(47, 12);
				int transactionAmountPartInt = Convert.ToInt32(transactionAmountParts.Substring(0, 10));
				decimal transactionAmountPartDecimal = Convert.ToDecimal(transactionAmountParts.Substring(10, 2)) / 100;

				TransactionAmount = transactionAmountPartInt + transactionAmountPartDecimal;

				string tipParts = message.Substring(59, 12);
				int tipPartInt = Convert.ToInt32(tipParts.Substring(0, 10));
				decimal tipPartDecimal = Convert.ToInt32(tipParts.Substring(10, 2)) / 100;

				Tip = tipPartInt + tipPartDecimal;

				Card = message.Substring(71, 16);
				OperatorKey = Message.Substring(87, 6);
				RoomNumber = Message.Substring(93, 4);
				Folio = Message.Substring(97, 7);
				LegendLength = Convert.ToInt32(Message.Substring(104, 2));
				LegendResponse = Message.Substring(106, LegendLength);

				int index = 106 + LegendLength;

				FinancialReference = Message.Substring(index, 8);
			}
			catch (Exception ex)
			{
				ResponseCode = (ResponseCodes)(-2);
				this.LegendResponse = "Error de lectura";
			}
		}
	}
}
