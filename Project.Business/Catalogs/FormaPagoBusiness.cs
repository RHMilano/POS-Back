using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Milano.BackEnd.Repository;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.Catalogs;
/// <summary>
/// Espacio de nombres de la logica de Negocio
/// </summary>
namespace Milano.BackEnd.Business
{
    /// <summary>
    /// Clase business Forma Pago
    /// </summary>
    public class FormaPagoBusiness : BaseBusiness
    {

        /// <summary>
        /// Repositorio de Formas de Pago
        /// </summary>
        protected FormasPagoRepository repository;
        /// <summary>
		/// Atributo del Token del usuario
		/// </summary>
		protected TokenDto token;

        /// <summary>
        /// Contructor por default
        /// </summary>
        /// <param name="token">Token del usuario</param>
        public FormaPagoBusiness(TokenDto token)
        {
            this.repository = new FormasPagoRepository();
            this.token = token;
        }

        /// <summary>
        /// Búsqueda de Forma de Pago de Tipo Vale
        /// </summary>        
        /// <returns></returns>
        public ResponseBussiness<FormaPagoResponse[]> GetFormasPagoVales()
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.getFormasPagoVales(this.token.CodeStore);
            });
        }

    }
}
