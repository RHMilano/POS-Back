using Milano.BackEnd.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Milano.BackEnd.Repository
{
    /// <summary>
    /// Repositorio tarjetas de regalo
    /// </summary>
    public class TarjetasRegaloRepository : BaseRepository
    {
        /// <summary>
        /// Guardar el estado de la activacion de la tarjeta
        /// </summary>
        /// <param name="folioTarjeta"></param>
        /// <param name="folioVenta"></param>
        /// <param name="status"></param>
        /// <param name="codigoTienda"></param>
        /// <param name="codigoCaja"></param>
        /// <param name="descripcion"></param>
        /// <returns></returns>
        public OperationResponse GuardarActivacionTarjeta(int folioTarjeta, string folioVenta, string status, int codigoTienda, int codigoCaja, string descripcion)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioTarjeta", folioTarjeta);
            parameters.Add("@FolioVenta", folioVenta);
            parameters.Add("@Estatus", status);
            parameters.Add("@CodigoTienda", codigoTienda);
            parameters.Add("@CodigoCaja", codigoCaja);
            parameters.Add("@Descripcion", descripcion);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_TarjetaRegaloActivar]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }


        /// <summary>
        /// Busqueda de tarjetas por folio de venta
        /// </summary>
        /// <param name="folioVenta"></param>
        /// <returns></returns>
        public BusquedaTarjetasRegalo[] BusquedaTarjetaLineaTicketPorFolioVenta(string folioVenta)
        {
            List<BusquedaTarjetasRegalo> lista = new List<BusquedaTarjetasRegalo>();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioVenta", folioVenta);
            foreach (var r in data.GetDataReader("[dbo].[sp_vanti_BuscarTarjetasRegaloPorFolio]", parameters))
            {
                BusquedaTarjetasRegalo busquedaTransaccionResponse = new BusquedaTarjetasRegalo();
                busquedaTransaccionResponse.FolioTarjetaRegalo = Convert.ToInt32(r.GetValue(0));
                busquedaTransaccionResponse.Estatus = r.GetValue(1).ToString();
                lista.Add(busquedaTransaccionResponse);
            }
            return lista.ToArray();
        }

    }
}
