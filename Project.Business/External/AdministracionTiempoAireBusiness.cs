using Milano.BackEnd.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Milano.BackEnd.Repository;
namespace Milano.BackEnd.Business
{
    /// <summary>
    /// Clase administrar las opciones de tiempo aire
    /// </summary>
    public class AdministracionTiempoAireBusiness : BaseBusiness
    {

        /// <summary>
        /// Repositorio 
        /// </summary>
        PagoServiciosRepository repository;

        /// <summary>
        /// Constructor de tiempo aire
        /// </summary>
        public AdministracionTiempoAireBusiness(TokenDto token)
        {
            repository = new PagoServiciosRepository(token);

        }

        /// <summary>
        /// Obtenemos la lista de Empresas
        /// </summary>
        /// <returns>lista de empresas</returns>
        public ResponseBussiness<CompaniasPagoServiciosResponse[]> ObtenerListaEmpresas()
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.ObtenerListaEmpresasTA();
            });
        }

        /// <summary>
        /// Metodo para obtener los montos de recarga por compañia
        /// </summary>
        /// <param name="codigoEmpresa">codigo de la empresa</param>
        /// <returns>lista de montos</returns>
        public ResponseBussiness<ProductsResponse[]> ObtenerProductosTA(string codigoEmpresa)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.ObtenerProductosTA(codigoEmpresa);
            });
        }

    }
}
