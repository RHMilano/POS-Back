using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milano.BackEnd.Repository.BBVAv2 
{
    /// <summary>
    /// RequestRepository
    /// </summary>
    public class RequestRepository : BaseRepository
    {

        public Request_v1_5 GetLastRequestBySequence(int terminalNumber, int sessionNumber)
        {
            Request_v1_5 request = null;
            var parameters = new Dictionary<string, object>();
            parameters.Add("@NumeroTermial", terminalNumber);
            parameters.Add("@NumeroSesion", sessionNumber);
            foreach (var row in data.GetDataReader("dbo.sp_vanti_BuscarUltimaPeticionPinPadPorSequencia", parameters))
            {
                request = new Request_v1_5();
                request.TransactionCode = (Request_v1_5.TransactionCodes)Convert.ToInt32(row.GetValue(0));
                request.TerminalNumber = Convert.ToInt32(row.GetValue(1));
                request.SessionNumber = Convert.ToInt32(row.GetValue(2));
                request.TransactionSequence = Convert.ToInt32(row.GetValue(3));
                request.TransactionAmount = Convert.ToDecimal(row.GetValue(4));
                request.Tip = Convert.ToDecimal(row.GetValue(5)); 
                request.Folio = Convert.ToInt32(row.GetValue(6));
                request.EMVCapacity = Convert.ToInt32(row.GetValue(7));
                request.CardReaderType = Convert.ToInt32(row.GetValue(8));
                request.CVV2Capacity = Convert.ToInt32(row.GetValue(9));
                request.FinancialMonths = Convert.ToInt16(row.GetValue(10));
                request.PaymentsPartial = Convert.ToInt16(row.GetValue(11));
                request.Promotion = Convert.ToInt16(row.GetValue(12));
                request.TypeCurrency = Convert.ToInt32(row.GetValue(13));
                request.Authorization = Convert.ToString(row.GetValue(14));
                request.CashBackAmount = Convert.ToDecimal(row.GetValue(15));
                request.CommerceDateTime = Convert.ToDateTime(row.GetValue(16));
                request.CommerceReference = Convert.ToString(row.GetValue(17));
                request.AmountOther = Convert.ToDecimal(row.GetValue(18));
                request.OperatorKey = Convert.ToString(row.GetValue(19));
                request.Affiliation = Convert.ToInt32(row.GetValue(20));
                request.RoomNumber = Convert.ToString(row.GetValue(21));
                request.FinancialReference = Convert.ToInt32(row.GetValue(22));
                request.Message = Convert.ToString(row.GetValue(23));
            }
            return request;
        }

        public void InsertRequest(Request_v1_5 request, int CodigoTienda, int CodigoCaja)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTransaccion", Convert.ToInt32(request.TransactionCode));
            parameters.Add("@NumeroTermial", request.TerminalNumber);
            parameters.Add("@NumeroSesion", request.SessionNumber);
            parameters.Add("@SecuenciaTransaccion", request.TransactionSequence);
            parameters.Add("@SecuenciaPos", request.SecuenciaPos);
            parameters.Add("@ImporteTransaccion", request.TransactionAmount);
            parameters.Add("@Propina", request.Tip);
            parameters.Add("@Folio", request.Folio);
            parameters.Add("@CapacidadEMV", request.EMVCapacity);
            parameters.Add("@TipoLectorTarjeta", request.CardReaderType);
            parameters.Add("@CapacidadCVV2", request.CVV2Capacity);
            parameters.Add("@MesesFinanciamiento", request.FinancialMonths);
            parameters.Add("@ParcializacionPagos", request.PaymentsPartial);
            parameters.Add("@Promociones", request.Promotion);
            parameters.Add("@TipoMoneda", request.TypeCurrency);
            parameters.Add("@Autorizacion", request.Authorization);
            parameters.Add("@ImporteCashBack", request.CashBackAmount);
            parameters.Add("@FechaHoraComercio", request.CommerceDateTime);
            parameters.Add("@ReferenciaComercio", request.CommerceReference);
            parameters.Add("@OtroImporte", request.AmountOther);
            parameters.Add("@ClaveOperador", request.OperatorKey);
            parameters.Add("@Afiliacion", request.Affiliation);
            parameters.Add("@NumeroCuarto", request.RoomNumber);
            parameters.Add("@ReferenciaFinanciera", request.FinancialReference);
            parameters.Add("@Mensaje", "");
            //parameters.Add("@CodigoTienda", CodigoTienda);
            //parameters.Add("@CodigoCaja", CodigoCaja);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            //var result = data.ExecuteProcedure("[dbo].[sp_vanti_AgregarPeticionPinPadv2]", parameters, parametersOut);
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_AgregarPeticionPinPad]", parameters, parametersOut);
        }


    }
}
