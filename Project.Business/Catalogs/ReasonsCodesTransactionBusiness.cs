using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milano.BackEnd.Dto;
using System.Collections;
using Milano.BackEnd.Repository;


namespace Milano.BackEnd.Business
{
    /// <summary>
    /// Clase de los codigos de razón para cancelar la transacción
    /// </summary>
    public class ReasonsCodesTransactionBusiness : BaseBusiness
    {
        /// <summary>
        /// Repositorio de codigos de razón para cancelar la transacción
        /// </summary>
        protected ReasonsCodesTransactionRepository repository;

        /// <summary>
        /// Constructor por default
        /// </summary>
        public ReasonsCodesTransactionBusiness()
        {
            this.repository = new ReasonsCodesTransactionRepository();
        }

        /// <summary>
        /// Búsqueda de codigos de razón para cancelar la transacción
        /// </summary>
        /// <param name="reasonsCodesRequest">Objeto de peticion que contiene nombre que contiene código de la razón para cancelar la transacción </param>
        /// <returns></returns>
        public ResponseBussiness<ReasonsCodesTransactionResponse[]> CatalogoReasonsCodesTransaction(ReasonsCodesTransactionRequest reasonsCodesRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.CatalogoReasonsCodesTransaction(reasonsCodesRequest);
            });
        }

    }
}
