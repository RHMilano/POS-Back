using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Milano.BackEnd.Repository
{
    /// <summary>
    /// Repositorio para imagenes Locales
    /// </summary>
    public class ProductImageHandlerRepository : BaseRepository
    {

        /// <summary>
        /// Obtenemos la ruta donde se guardaran las imagenes locales	
        /// </summary>
        /// <returns></returns>
        public string ObtenerRutaImagenes()
        {
            String ruta = "";
            var parameters = new Dictionary<string, object>();
            foreach (var c in data.GetDataReader("[dbo].[sp_vanti_ObtenerRutalLocalImagen]", parameters))
            {
                ruta = c.GetValue(0).ToString();
            }
            return ruta;
        }

    }
}
