using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milano.BackEnd.Repository.BBVAv2
{
    /// <summary>
    /// 
    /// </summary>
    public class Request_v1_5
    {
        /// <summary>
        /// Enum de las TransactionCodes
        /// </summary>
        public enum TransactionCodes
        {
            /// <summary>
            /// Valor de la constante de venta
            /// </summary>
            Sale = 1
        }

        /// <summary>
        /// SecuenciaPos
        /// </summary>
        public int SecuenciaPos = 0;

        /// <summary>
        /// TransactionCode
        /// </summary>
        public TransactionCodes TransactionCode { get; set; }

        /// <summary>
        /// TerminalNumber
        /// </summary>
        public int TerminalNumber { get; set; }

        /// <summary>
        /// SessionNumber
        /// </summary>
        public int SessionNumber { get; set; }

        /// <summary>
        /// TransactionSequence
        /// </summary>
        public int TransactionSequence { get; set; }

        /// <summary>
        /// TransactionAmount
        /// </summary>
        public decimal TransactionAmount { get; set; }
        
        /// <summary>
        /// Tip
        /// </summary>
        public decimal Tip { get; set; }

        /// <summary>
        /// Folio
        /// </summary>
        public int Folio { get; set; }

        /// <summary>
        /// EMVCapacity
        /// </summary>
        public int EMVCapacity { get; set; }

        /// <summary>
        /// CardReaderType
        /// </summary>
        public int CardReaderType { get; set; }

        /// <summary>
        /// CVV2Capacity
        /// </summary>
        public int CVV2Capacity { get; set; }

        /// <summary>
        /// FinancialMonths
        /// </summary>
        public short FinancialMonths { get; set; }

        /// <summary>
        /// PaymentsPartial
        /// </summary>
        public short PaymentsPartial { get; set; }

        /// <summary>
        /// Promotion
        /// </summary>
        public short Promotion { get; set; }

        /// <summary>
        /// TypeCurrency
        /// </summary>
        public int TypeCurrency { get; set; }

        /// <summary>
        /// Authorization
        /// </summary>
        public string Authorization { get; set; }

        /// <summary>
        /// IncomingCardMode
        /// </summary>
        public string IncomingCardMode { get; set; }

        /// <summary>
        /// CVV2
        /// </summary>
        private string CVV2 { get; set; }

        /// <summary>
        /// Track2
        /// </summary>
        private string Track2 { get; set; }

        /// <summary>
        /// CardNumberSequence
        /// </summary>
        private string CardNumberSequence { get; set; }

        /// <summary>
        /// CashBackAmount
        /// </summary>
        public decimal CashBackAmount { get; set; }

        /// <summary>
        /// CommerceDateTime
        /// </summary>
        public DateTime CommerceDateTime { get; set; }

        /// <summary>
        /// CommerceReference
        /// </summary>
        public string CommerceReference { get; set; }

        /// <summary>
        /// AmountOther
        /// </summary>
        public decimal AmountOther { get; set; }

        /// <summary>
        /// OperatorKey
        /// </summary>
        public string OperatorKey { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int Affiliation { get; set; }

        /// <summary>
        /// RoomNumber
        /// </summary>
        public string RoomNumber { get; set; }

        /// <summary>
        /// FinancialReference
        /// </summary>
        public int FinancialReference { get; set; }

        /// <summary>
        /// ChipConditionalCode
        /// </summary>
        private int ChipConditionalCode { get; set; }

        /// <summary>
        /// Filler1
        /// </summary>
        private int Filler1 { get; set; }

        /// <summary>
        /// Filler2
        /// </summary>
        private int Filler2 { get; set; }

        /// <summary>
        /// PaymentsLength
        /// </summary>
        private int PaymentsLength { get; set; }

        /// <summary>
        /// VariableData
        /// </summary>
        public string VariableData { get; set; }

        /// <summary>
        /// DefaultLength
        /// </summary>
        private int DefaultLength { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Constructor para los datos que se van almacenar 
        /// </summary>
        public Request_v1_5()
        {
            EMVCapacity = 3;
            CardReaderType = 8;
            CVV2Capacity = 1;
            ChipConditionalCode = 0;
            Filler1 = 0;
            Filler2 = 0;
            PaymentsLength = 0;
            DefaultLength = 0;
            Folio = 0;
            Authorization = string.Empty;
            IncomingCardMode = string.Empty;
            CVV2 = string.Empty;
            Track2 = string.Empty;
            CardNumberSequence = string.Empty;
            CommerceReference = string.Empty;
            OperatorKey = string.Empty;
            RoomNumber = string.Empty;
            VariableData = string.Empty;
            CommerceDateTime = DateTime.Now;
        }

        /// <summary>
        /// Arma un mensaje
        /// </summary>
        public void BuildMessage()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(Convert.ToUInt32(TransactionCode).ToString().PadLeft(3, '0'));
            stringBuilder.Append(TerminalNumber.ToString().PadLeft(8, '0'));
            stringBuilder.Append(SessionNumber.ToString().PadLeft(4, '0'));
            stringBuilder.Append(TransactionSequence.ToString().PadLeft(4, '0'));
            stringBuilder.Append(GetStringFormatted(TransactionAmount));
            stringBuilder.Append(GetStringFormatted(Tip));

            stringBuilder.Append(Folio.ToString().PadLeft(7, '0'));
            stringBuilder.Append(EMVCapacity.ToString().PadLeft(1, '0'));
            stringBuilder.Append(CardReaderType.ToString().PadLeft(1, '0'));
            stringBuilder.Append(CVV2Capacity.ToString().PadLeft(1, '0'));

            stringBuilder.Append(FinancialMonths.ToString().PadLeft(2, '0'));
            stringBuilder.Append(PaymentsPartial.ToString().PadLeft(2, '0'));
            stringBuilder.Append(Promotion.ToString().PadLeft(2, '0'));
            stringBuilder.Append(TypeCurrency.ToString().PadLeft(1, '0'));

            stringBuilder.Append(Authorization.PadLeft(6, ' '));
            stringBuilder.Append(IncomingCardMode.PadLeft(2, ' '));
            stringBuilder.Append(CVV2.PadLeft(4, ' '));
            stringBuilder.Append(Track2.PadLeft(40, ' '));
            stringBuilder.Append(CardNumberSequence.PadLeft(3, ' '));
            stringBuilder.Append(GetStringFormatted(CashBackAmount));

            stringBuilder.Append(CommerceDateTime.ToString("yyMMddhhmmss"));
            stringBuilder.Append(CommerceReference.PadLeft(45, ' '));
            stringBuilder.Append(GetStringFormatted(AmountOther));

            stringBuilder.Append(OperatorKey.PadLeft(6, '9'));
            stringBuilder.Append(Affiliation.ToString().PadLeft(8, '0'));
            stringBuilder.Append(RoomNumber.PadLeft(4, '0'));
            stringBuilder.Append(FinancialReference.ToString().PadLeft(8, '0'));

            stringBuilder.Append(ChipConditionalCode.ToString().PadLeft(1, '0'));
            stringBuilder.Append(Filler1.ToString().PadLeft(3, '0'));
            stringBuilder.Append(Filler2.ToString().PadLeft(3, '0'));
            stringBuilder.Append(PaymentsLength.ToString().PadLeft(4, '0'));
            stringBuilder.Append(VariableData.PadLeft(4, '0'));
            stringBuilder.Append(PaymentsLength.ToString().PadLeft(4, '0'));

            Message = stringBuilder.ToString();
        }

        /// <summary>
        /// GetCashBackFormatted
        /// </summary>
        /// <returns></returns>
        public string GetCashBackFormatted()
        {
            return GetStringFormatted(CashBackAmount);
        }

        /// <summary>
        /// GetFinancialMonthsFormatted
        /// </summary>
        /// <returns></returns>
        public string GetFinancialMonthsFormatted()
        {
            return FinancialMonths.ToString().PadLeft(2, '0');
        }

        /// <summary>
        /// GetPaymentsPartialFormatted
        /// </summary>
        /// <returns></returns>
        public string GetPaymentsPartialFormatted()
        {
            return PaymentsPartial.ToString().PadLeft(2, '0');
        }

        /// <summary>
        /// GetPromotionFormatted
        /// </summary>
        /// <returns></returns>
        public string GetPromotionFormatted()
        {
            return Promotion.ToString().PadLeft(2, '0');
        }

        /// <summary>
        /// GetStringFormatted
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetStringFormatted(decimal value)
        {
            string stringFormatted = string.Empty;
            if (value > 0)
            {
                string[] decimalParts = value.ToString().Split(new string[] { "." }, StringSplitOptions.None);
                stringFormatted = decimalParts[0].PadLeft(10, '0');
                if (decimalParts.Length > 1)
                {
                    stringFormatted += decimalParts[1].PadRight(2, '0');
                }
                else
                {
                    stringFormatted += "0".PadLeft(2, '0');
                }
            }
            else
            {
                stringFormatted += "0".PadLeft(12, '0');
            }

            return stringFormatted;
        }
    }
}
