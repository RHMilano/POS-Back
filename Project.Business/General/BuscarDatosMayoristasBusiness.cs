using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.General;
/// <summary>
/// Espacio de nombres de negocio para logica que se ocupa en forma general, no son propias de un modulo
/// </summary>
namespace Milano.BackEnd.Business.General
{
    /// <summary>
    /// Clase de negocios de las busquedas de mayoristas
    /// </summary>
    public class BuscarDatosMayoristasBusiness : BaseBusiness
    {

        /// <summary>
        /// Constructor por default
        /// </summary>
        public BuscarDatosMayoristasBusiness()
        {
        }

        /// <summary>
        /// Llama al servicio de busqueda de mayorista
        /// </summary>
        /// <param name="buscarDatosMayoristasRequest"> Objeto de peticion de mayorista por codigo o nombre</param>
        /// <returns></returns>
        public ResponseBussiness<BuscarDatosMayoristasResponse> searchMayorista(BuscarDatosMayoristasRequest buscarDatosMayoristasRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                BuscarDatosMayoristasResponse buscarDatosMayoristasResponse = new BuscarDatosMayoristasResponse();
                return buscarDatosMayoristasResponse;
            });
        }

    }
}
