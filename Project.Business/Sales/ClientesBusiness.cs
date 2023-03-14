using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Repository;
namespace Milano.BackEnd.Business
{
    /// <summary>
    /// Clase de negocios de Clientes
    /// </summary>
    public class ClientesBusiness : BaseBusiness
    {
        TokenDto token;
        ClientesRepository repository;
        /// <summary>
        /// Constructor
        /// </summary>
        public ClientesBusiness(TokenDto token)
        {
            this.repository = new ClientesRepository();
            this.token = token;
        }
        /// <summary>
        /// Busqueda de clientes
        /// </summary>
        /// <param name="clienteRequest">parametros de busqueda</param>
        /// <returns>Lista de clientes encontrados</returns>
        public ResponseBussiness<ClienteResponse[]> BusquedaClientes(ClienteRequest clienteRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.BuscarClientes(clienteRequest);
            });
        }

        /// <summary>
        /// Alta de clientes
        /// </summary>
        /// <param name="altaClienteRequest">objeto cliente</param>
        /// <returns>respuesta de la operacion</returns>
        public ResponseBussiness<AltaClientesResponse> AltaClientes(AltaClienteRequest altaClienteRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.AltaClientes(altaClienteRequest, this.token.CodeStore, this.token.CodeBox);
            });
        }

    }
}
