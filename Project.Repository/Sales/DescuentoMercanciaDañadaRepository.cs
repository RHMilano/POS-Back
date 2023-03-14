using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Dto.Recovery;
using Milano.BackEnd.Dto.Sales;
using Milano.BackEnd.Repository.General;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;


namespace Milano.BackEnd.Repository.Sales
{
    /// <summary>
    /// Repositorio de Mercancia Dañada
    /// </summary>
    public class DescuentoMercanciaDañadaRepository : BaseRepository
    {

        /// <summary>
		/// Búsqueda de datos para el descuento
		/// </summary>
		/// <param name="folio">Folio de Venta</param>
		/// <param name="caja">Código de la caja</param>
		/// <param name="tienda">Codigo de tienda</param>
		/// <returns>Lista de empleados</returns>
        public DescuentoMercanciaDañada[] DescuentosMercancia(string folio, int caja, int tienda)
        {
            List<DescuentoMercanciaDañada> list = new List<DescuentoMercanciaDañada>();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioOperacion", folio);
            parameters.Add("@Caja", caja);
            parameters.Add("@Tienda", tienda);
            foreach (var r in data.GetDataReader("sp_vanti_ObtenerLineasMercanciaPicos", parameters))
            {
                DescuentoMercanciaDañada mercancia = new DescuentoMercanciaDañada();
                mercancia.Sesion = r.GetValue(0).ToString();
                mercancia.Cantidad = Convert.ToInt32(r.GetValue(1));
                mercancia.SKU = Convert.ToInt32(r.GetValue(2));
                mercancia.Transaccion = Convert.ToInt32(r.GetValue(3));
                mercancia.CodigoRazonDescuento = Convert.ToInt32(r.GetValue(4));
                mercancia.SecuenciaDetalleVenta = Convert.ToInt32(r.GetValue(5));
                list.Add(mercancia);
            }
            return list.ToArray();
        }

        /// <summary>
		/// Busqueda de Datos para mercancia dañada
		/// </summary>
		/// <param name="folioVenta">Folio de venta</param>
		/// <param name="sku">Sku</param>
		/// <param name="numeroSesion">Numero de sesion</param>
        /// <param name="numeroSecuencia">Numero de secuencia/param>
        /// <param name="estatus">Estatus A/P</param>
		/// <returns>Lista de empleados</returns>
        public void RegistrarDescuentoMercanciaDaniada(string folioVenta, string numeroSesion, int numeroSecuencia, string estatus)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioVenta", folioVenta);
            parameters.Add("@NumeroSesion", numeroSesion);
            parameters.Add("@NumeroSecuencia", numeroSecuencia);
            parameters.Add("@Estatus", estatus);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            var result = data.ExecuteProcedure("sp_vanti_RegistroDescuentosMercanciaDaniada", parameters, parametersOut);
        }

    }
}

