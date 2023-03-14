using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Repository;
using System.Data;
using System.Configuration;
using Milano.BackEnd.Utils;
namespace Milano.BackEnd.Repository
{
    /// <summary>
    /// Repositorio de  códigos de razones 
    /// </summary>
    public class ReasonsCodesTransactionRepository : BaseRepository
    {
        /// <summary>
        /// Búsqueda de códigos de razones 
        /// </summary>
        /// <param name="reasonsCodesRequest">Código de la razón de anular transacción</param>
        /// <returns>Lista de códigos de razones para anular transacción</returns>
        public ReasonsCodesTransactionResponse[] CatalogoReasonsCodesTransaction(ReasonsCodesTransactionRequest reasonsCodesRequest)
        {
            List<ReasonsCodesTransactionResponse> list = new List<ReasonsCodesTransactionResponse>();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoRazon", reasonsCodesRequest.CodigoTipoRazonMMS);
            foreach (var r in data.GetDataReader("[dbo].[sp_vanti_CodigosRazones]", parameters))
            {
                ReasonsCodesTransactionResponse reasonsCode = new ReasonsCodesTransactionResponse();
                reasonsCode.CodigoRazon = Convert.ToInt32(r.GetValue(0));
                reasonsCode.CodigoRazonMMS = r.GetValue(1).ToString();
                reasonsCode.DescripcionRazon = r.GetValue(2).ToString();
                list.Add(reasonsCode);
            }
            return list.ToArray();
        }
    }
}
