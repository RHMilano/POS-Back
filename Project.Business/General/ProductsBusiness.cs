using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Milano.BackEnd.Repository.General;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Repository;
namespace Milano.BackEnd.Business.General
{
    /// <summary>
    /// Clase de negocio de Productos
    /// </summary>
    public class ProductsBusiness : BaseBusiness
    {
        /// <summary>
        /// Atributo de repositorio de productos
        /// </summary>
        protected ProductsRepository repository;

        /// <summary>
        ///Atributo del token usuario
        /// </summary>
        protected TokenDto token;

        /// <summary>
        /// Constructor sin parametros
        /// </summary>
        public ProductsBusiness(TokenDto token)
        {
            this.repository = new ProductsRepository();
            this.token = token;
        }

        /// <summary>
        /// Busqueda rápida de productos
        /// </summary>
        /// <param name="productsRequest"></param>
        /// <returns>Arreglo de productos</returns>
        public ResponseBussiness<ProductsResponse[]> Search(ProductsRequest productsRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.Search(token.CodeStore, productsRequest);
            });
        }

        /// <summary>
        /// Busqueda extendida o avanzada de productos
        /// </summary>
        /// <param name="productsRequest"></param>
        /// <returns>Arreglo de productos</returns>
        public ResponseBussiness<ProductsFindResponse> SearchAdvance(ProductsRequest productsRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.SearchAdvance(token.CodeStore, productsRequest);
            });
        }

    }
}
