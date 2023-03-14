using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Milano.BackEnd.Dto;
namespace Milano.BackEnd.Repository
{
    /// <summary>
    /// Repositorio de clientes
    /// </summary>
    public class ClientesRepository : BaseRepository
    {
        /// <summary>
        /// Busqueda de clientes
        /// </summary>
        /// <param name="clienteRequest"></param>
        /// <returns></returns>
        public ClienteResponse[] BuscarClientes(ClienteRequest clienteRequest)
        {
            List<ClienteResponse> list = new List<ClienteResponse>();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoCliente", clienteRequest.CodigoCliente);
            parameters.Add("@Nombre", clienteRequest.Nombre);
            parameters.Add("@Telefono", clienteRequest.Telefono);
            foreach (var r in data.GetDataReader("[dbo].[sp_vanti_server_BusquedaClientes]", parameters))
            {
                ClienteResponse cliente = new ClienteResponse();
                cliente.Nombre = r.GetValue(0).ToString();
                cliente.CodigoCliente = Convert.ToInt64(r.GetValue(1));
                cliente.Telefono = r.GetValue(2).ToString();
                cliente.CodigoTienda = Convert.ToInt32(r.GetValue(3));
                cliente.ApellidoPaterno = r.GetValue(4).ToString();
                cliente.ApellidoMaterno = r.GetValue(5).ToString();
                cliente.Calle = r.GetValue(6).ToString();
                cliente.NoExterior = r.GetValue(7).ToString();
                cliente.NoInterior = r.GetValue(8).ToString();
                cliente.Ciudad = r.GetValue(9).ToString();
                cliente.Estado = r.GetValue(10).ToString();
                cliente.CodigoPostal = r.GetValue(11).ToString();
                cliente.Email = r.GetValue(12).ToString();
                cliente.CodigoCaja = Convert.ToInt32(r.GetValue(13));
                list.Add(cliente);
            }
            return list.ToArray();
        }

        /// <summary>
        /// Alta de clientes
        /// </summary>
        /// <param name="altaClienteRequest"></param>
        /// <returns></returns>
        public AltaClientesResponse AltaClientes(AltaClienteRequest altaClienteRequest, int codigoTienda, int codigoCaja)
        {
            AltaClientesResponse operationResponse = new AltaClientesResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", codigoTienda);
            parameters.Add("@CodigoCliente", altaClienteRequest.CodigoCliente);
            parameters.Add("@CodigoCaja", codigoCaja);
            parameters.Add("@Telefono", altaClienteRequest.Telefono);
            parameters.Add("@Paterno", altaClienteRequest.ApellidoPaterno);
            parameters.Add("@Materno", altaClienteRequest.ApellidoMaterno);
            parameters.Add("@Nombre", altaClienteRequest.Nombre);
            parameters.Add("@Calle", altaClienteRequest.Calle);
            parameters.Add("@NoExterior", altaClienteRequest.NoExterior);
            parameters.Add("@NoInterior", altaClienteRequest.NoInterior);
            parameters.Add("@Ciudad", altaClienteRequest.Ciudad);
            parameters.Add("@Estado", altaClienteRequest.Estado);
            parameters.Add("@CodigoPostal", altaClienteRequest.CodigoPostal);
            parameters.Add("@Email", altaClienteRequest.Email);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoClienteGenerado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.BigInt });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_AltaClientes]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            operationResponse.CodigoCliente = Convert.ToInt64(result["@CodigoClienteGenerado"]);
            return operationResponse;
        }
    }


}
