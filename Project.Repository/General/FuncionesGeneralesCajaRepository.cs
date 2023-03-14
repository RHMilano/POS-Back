using Milano.BackEnd.Dto.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Milano.BackEnd.Repository.General
{

    /// <summary>
    /// Repositorio de funciones generales de las caja
    /// </summary>
    public class FuncionesGeneralesCajaRepository : BaseRepository
    {

        /// <summary>
        /// Busca y regresa las funciones generales de la caja
        /// </summary>
        /// <param name="CodigoCaja">Número de la caja</param>
        /// <param name="CodigoTienda">Codigo de la tienda a buscar</param>
        /// <returns>Configuraciones de la tienda </returns>
        /// 
        public FuncionGeneralCajaResponse GetFunctions(int CodigoCaja, int CodigoTienda)
        {
            FuncionGeneralCajaResponse funcion = null;
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", CodigoTienda);
            parameters.Add("@CodigoCaja", CodigoCaja);

            foreach (var c in data.GetDataReader("dbo.sp_vanti_ObtenerFuncionesGeneralesCaja", parameters))
            {
                funcion = new FuncionGeneralCajaResponse();
                funcion.CodigoTienda = Convert.ToInt32(c.GetValue(0));
                funcion.CodigoCaja = Convert.ToInt32(c.GetValue(1));
                funcion.PuertoImpresoraTickets = Convert.ToString(c.GetValue(2));
                funcion.RutaImpresoraTickets = Convert.ToString(c.GetValue(3));
                funcion.UrlImpresion = Convert.ToString(c.GetValue(4));
                funcion.UrlLecturaBancaria = Convert.ToString(c.GetValue(5));
            }
            return funcion;
        }

    }
}
