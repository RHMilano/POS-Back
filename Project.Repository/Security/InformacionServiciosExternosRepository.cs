using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.General;

namespace Milano.BackEnd.Repository
{
    /// <summary>
    /// Informacion de servicios externos
    /// </summary>
    public class InformacionServiciosExternosRepository : BaseRepository
    {
        /// <summary>
        /// Obtiene las credencialese información asociada  del servicio.
        /// </summary>
        /// <param name="idServicio">Identificador del servicio</param>
        /// <returns></returns>
        public CredencialesServicioExterno ObtenerCredenciales(int idServicio)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@idServicio", idServicio);
            CredencialesServicioExterno credenciales = new CredencialesServicioExterno();
            
            foreach (var r in data.GetDataReader("[dbo].[sp_vanti_InformacionServicioExterno]", parameters))
            {
                credenciales.UserName = r.GetValue(0).ToString();
                credenciales.Password = r.GetValue(1).ToString();
                credenciales.NumeroIntentos = Convert.ToInt32(r.GetValue(2));
                credenciales.Licence = r.GetValue(3).ToString();
            }
            
            return credenciales;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TipoCambioActualizado DivisaActualizada()
        {
            TipoCambioActualizado tipoCambioResponse = new TipoCambioActualizado();

            var parameters = new Dictionary<string, object>();
            //parameters.Add("@FolioVenta", request.FolioVenta);
            //parameters.Add("@Sku", info.Sku);
            //parameters.Add("@CodigoTienda", codeStore);
            //parameters.Add("@CodigoCaja", codeBox);
            //parameters.Add("@FolioTarjeta", info.FolioTarjeta);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CambioMaximo", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Money });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@ReciboMaximo", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Money });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@TipoCambio", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Money });

            var result = data.ExecuteProcedure("sp_cnfDivisaActulizada", parameters, parametersOut);
            tipoCambioResponse.TipoCambio = Convert.ToDecimal(result["@TipoCambio"].ToString());
            tipoCambioResponse.ReciboMaximo = Convert.ToDecimal(result["@ReciboMaximo"].ToString());
            tipoCambioResponse.CambioMaximo = Convert.ToDecimal(result["@CambioMaximo"].ToString());

            return tipoCambioResponse;
        }



        /// <summary>
        /// Actualiza el tipo de cambio del dia en las tabla de la tienda
        /// </summary>
        /// <param name="tipoCambio">Tipo de Cambio</param>
        /// <param name="reciboMaximo">Monto maximo para recibiir en dolares</param>
        /// <param name="cambioMaximo">Monto maximo para dar cambio</param>
        /// <returns></returns>
        public bool ActualizaDivisa(decimal tipoCambio, decimal reciboMaximo, decimal cambioMaximo)
        {
            try
            {
                TipoCambioActualizado tipoCambioResponse = new TipoCambioActualizado();

                var parameters = new Dictionary<string, object>();

                parameters.Add("@TipoCambio", tipoCambio);
                parameters.Add("@ReciboMaximo", reciboMaximo);
                parameters.Add("@CambioMaximo", cambioMaximo);

                List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();

                var result = data.ExecuteProcedure("sp_cnfActulizaDivisa", parameters);

                return true;
            }
            catch (Exception)
            {
                return false;
                //throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public InfoService ObtenerInfoServicioExterno(int idServicio)
        {
            InfoService infoService = new InfoService();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@IdServicio", idServicio);
            foreach (var r in data.GetDataReader("[dbo].[sp_vanti_bo_ObtenerInfoServiciosExternosBackOfficeLoginServiceMilano]", parameters))
            {
                infoService.NameService = r.GetValue(0).ToString();
                infoService.UrlService = r.GetValue(1).ToString();
                infoService.UserName = r.GetValue(2).ToString();
                infoService.Password = r.GetValue(3).ToString();

             }
            return infoService;
        }
    }
}
