using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBVALogic.DTOCompatibility_1_5
{
    public class Request
    {
        public enum TransactionCodes
        {
            Sale = 1
        }

        public int SecuenciaPos = 0;

        public TransactionCodes TransactionCode { get; set; }
        public int TerminalNumber { get; set; }
        public int SessionNumber { get; set; }
        public int TransactionSequence { get; set; }
        public decimal TransactionAmount { get; set; }
        public decimal Tip { get; set; }
        public int Folio { get; set; }
        public int EMVCapacity { get; set; }
        public int CardReaderType { get; set; }
        public int CVV2Capacity { get; set; }
        public short FinancialMonths { get; set; }
        public short PaymentsPartial { get; set; }
        public short Promotion { get; set; }
        public int TypeCurrency { get; set; }
        public string Authorization { get; set; }
        public string IncomingCardMode { get; set; }
        private string CVV2 { get; set; }
        private string Track2 { get; set; }
        private string CardNumberSequence { get; set; }
        public decimal CashBackAmount { get; set; }
        public DateTime CommerceDateTime { get; set; }
        public string CommerceReference { get; set; }
        public decimal AmountOther { get; set; }
        public string OperatorKey { get; set; }
        public int Affiliation { get; set; }
        public string RoomNumber { get; set; }
        public int FinancialReference { get; set; }
        private int ChipConditionalCode { get; set; }
        private int Filler1 { get; set; }
        private int Filler2 { get; set; }
        private int PaymentsLength { get; set; }
        public string VariableData { get; set; }
        private int DefaultLength { get; set; }
        public string Message { get; set; }

        public Request()
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

        public string GetCashBackFormatted()
        {
            return GetStringFormatted(CashBackAmount);
        }

        public string GetFinancialMonthsFormatted()
        {
            return FinancialMonths.ToString().PadLeft(2, '0');
        }

        public string GetPaymentsPartialFormatted()
        {
            return PaymentsPartial.ToString().PadLeft(2, '0');
        }

        public string GetPromotionFormatted()
        {
            return Promotion.ToString().PadLeft(2, '0');
        }

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
