using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.Sales;
using Milano.BackEnd.Repository.Sales;

namespace Milano.BackEnd.Business.Sales
{
    /// <summary>
    /// Clase de negocios de validacion de fraude Tiempo Aire.
    /// </summary>
    public class FraudValidationBusiness : BaseBusiness
    {

        /// <summary>
        /// Repositorio de configuración de caja
        /// </summary>
        protected FraudValidationRepository repository;
        /// <summary>
		/// Atributo del token usuario
		/// </summary>
		protected TokenDto token;

        /// <summary>
        /// Constructor
        /// </summary>
        public FraudValidationBusiness(TokenDto token)
        {
            this.repository = new FraudValidationRepository();
            this.token = token;
        }

        /// <summary>
        /// Validar fraude de caja
        /// </summary>
        /// <param name="fraudValidation">Objeto de la validación contiene numero telefónico y código de tienda</param>
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> FraudValidationTA(FraudValidationRequest fraudValidation)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.FraudValidationTA(fraudValidation.NumeroTelefonico, token.CodeStore);
            });
        }
    }
}
