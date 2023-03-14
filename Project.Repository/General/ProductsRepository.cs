using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.General;
using Project.Services.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Milano.BackEnd.Repository.General
{

    /// <summary>
    /// Repositorio de productos 
    /// </summary>
    public class ProductsRepository : BaseRepository
    {

        /// <summary>
        /// Busqueda rápida de productos
        /// </summary>
        /// <param name="codeStore">Código de la tienda</param>
        /// <param name="productRequest">Parámetros de búsqueda</param>
        /// <returns>Arreglo de productos</returns>
        public ProductsResponse[] Search(int codeStore, ProductsRequest productRequest)
        {
            ConfigGeneralesCajaTiendaResponse emp = new ConfigGeneralesCajaTiendaResponse();
            return ObterArticulo(codeStore, productRequest);
        }

        /// <summary>
        /// Busqueda extendida o avanzada de productos
        /// </summary>
        /// <param name="codeStore">Código de la tienda</param>
        /// <param name="productRequest">Parámetros de búsqueda</param>
        /// <returns>Arreglo de productos</returns>
        public ProductsFindResponse SearchAdvance(int codeStore, ProductsRequest productRequest)
        {
            ProductsFindResponse productsFindResponse = new ProductsFindResponse();
            productsFindResponse.NumeroRegistros = ObtenerTotalArticulos(codeStore, productRequest);
            productsFindResponse.Products = ObterArticulos(codeStore, productRequest);
            return productsFindResponse;
        }

        private int ObtenerTotalArticulos(int codeStore, ProductsRequest productRequest)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@Sku", productRequest.Sku);
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@Estilo", productRequest.CodigoEstilo);
            parameters.Add("@CodigoProveedor", productRequest.CodeProvider);
            parameters.Add("@CodigoDepartamento", productRequest.CodeDepartment);
            parameters.Add("@CodigoSubDepartamento", productRequest.CodeSubDepartment);
            parameters.Add("@CodigoClase", productRequest.CodeClass);
            parameters.Add("@CodigoSubClase", productRequest.CodeSubClass);
            parameters.Add("@Descripcion", productRequest.Description);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@NumeroRegistros", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_ProductosBusquedaTotal]", parameters, parametersOut);
            return Convert.ToInt32(result["@NumeroRegistros"]);
        }

        private ProductsResponse[] ObterArticulo(int codeStore, ProductsRequest productRequest)
        
        
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@Sku", productRequest.Sku);
            parameters.Add("@CodigoTienda", codeStore);
            List<ProductsResponse> list = new List<ProductsResponse>();
            Inspector inspector = new Inspector();
            foreach (var item in data.GetDataReader("[dbo].[sp_vanti_ProductoBusqueda]", parameters))
            {
                ProductsResponse product = new ProductsResponse();
                product.Articulo = new Articulo();
                product.Articulo.DigitoVerificadorArticulo = new DigitoVerificadorArticulo();

                product.Articulo.DigitoVerificadorArticulo.DigitoVerificadorActual = item.GetValue(0).ToString();
                product.Articulo.DigitoVerificadorArticulo.DigitoVerificadorCorrecto = item.GetValue(1).ToString();
                product.Articulo.DigitoVerificadorArticulo.Inconsistencia = Convert.ToInt32(item.GetValue(2)) == 1;
                product.Articulo.Sku = Convert.ToInt32(item.GetValue(3));
                product.Articulo.Estilo = item.GetValue(4).ToString();
                product.Articulo.Upc = item.GetValue(5).ToString();
                product.Articulo.RutaImagenLocal = item.GetValue(6).ToString();
                product.Articulo.RutaImagenRemota = item.GetValue(7).ToString();
                product.Articulo.TasaImpuesto1 = inspector.TruncarValor(Convert.ToDecimal(item.GetValue(8).ToString()));
                product.Articulo.TasaImpuesto2 = inspector.TruncarValor(Convert.ToDecimal(item.GetValue(9).ToString()));
                product.Articulo.PrecioConImpuestos = inspector.TruncarValor(Convert.ToDecimal(item.GetValue(10).ToString()));
                product.Articulo.CodigoImpuesto1 = item.GetValue(11).ToString();
                product.Articulo.CodigoImpuesto2 = item.GetValue(12).ToString();
                product.Articulo.Impuesto1 = inspector.TruncarValor(Convert.ToDecimal(item.GetValue(13)));
                product.Articulo.Impuesto2 = inspector.TruncarValor(Convert.ToDecimal(item.GetValue(14)));
                product.Articulo.EsTarjetaRegalo = item.GetValue(15).ToString() == "1";
                list.Add(product);
            }
            return list.ToArray();
        }

        private ProductsResponse[] ObterArticulos(int codeStore, ProductsRequest productRequest)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@Sku", productRequest.Sku);
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@Estilo", productRequest.CodigoEstilo);
            parameters.Add("@CodigoProveedor", productRequest.CodeProvider);
            parameters.Add("@CodigoDepartamento", productRequest.CodeDepartment);
            parameters.Add("@CodigoSubDepartamento", productRequest.CodeSubDepartment);
            parameters.Add("@CodigoClase", productRequest.CodeClass);
            parameters.Add("@CodigoSubClase", productRequest.CodeSubClass);
            parameters.Add("@Descripcion", productRequest.Description);
            if (productRequest.NumeroPagina != 0)
            {
                parameters.Add("@NumeroPagina", productRequest.NumeroPagina);
                parameters.Add("@RegistrosPorPagina", productRequest.RegistrosPorPagina);
            }
            List<ProductsResponse> list = new List<ProductsResponse>();
            Inspector inspector = new Inspector();
            foreach (var item in data.GetDataReader("[dbo].[sp_vanti_ProductosBusqueda]", parameters))
            {
                ProductsResponse product = new ProductsResponse();
                product.Articulo = new Articulo();
                product.Articulo.DigitoVerificadorArticulo = new DigitoVerificadorArticulo();

                product.Articulo.DigitoVerificadorArticulo.DigitoVerificadorActual = item.GetValue(0).ToString();
                product.Articulo.DigitoVerificadorArticulo.DigitoVerificadorCorrecto = item.GetValue(1).ToString();
                product.Articulo.DigitoVerificadorArticulo.Inconsistencia = Convert.ToInt32(item.GetValue(2)) == 1;
                product.Articulo.Sku = Convert.ToInt32(item.GetValue(3));
                product.Articulo.Estilo = item.GetValue(4).ToString();
                product.Articulo.Upc = item.GetValue(5).ToString();
                product.Articulo.RutaImagenLocal = item.GetValue(6).ToString();
                product.Articulo.RutaImagenRemota = item.GetValue(7).ToString();
                product.Articulo.TasaImpuesto1 = inspector.TruncarValor(Convert.ToDecimal(item.GetValue(8).ToString()));
                product.Articulo.TasaImpuesto2 = inspector.TruncarValor(Convert.ToDecimal(item.GetValue(9).ToString()));
                product.Articulo.PrecioConImpuestos = inspector.TruncarValor(Convert.ToDecimal(item.GetValue(10).ToString()));
                product.Articulo.CodigoImpuesto1 = item.GetValue(11).ToString();
                product.Articulo.CodigoImpuesto2 = item.GetValue(12).ToString();
                product.Articulo.Impuesto1 = inspector.TruncarValor(Convert.ToDecimal(item.GetValue(13)));
                product.Articulo.Impuesto2 = inspector.TruncarValor(Convert.ToDecimal(item.GetValue(14)));

                product.Articulo.CodigoDepto = Convert.ToInt32(item.GetValue(15));
                product.Articulo.CodigoSubDepartamento = Convert.ToInt32(item.GetValue(16));
                product.Articulo.CodigoClase = Convert.ToInt32(item.GetValue(17));
                product.Articulo.CodigoSubClase = Convert.ToInt32(item.GetValue(18));
                product.Articulo.CodigoMarca = Convert.ToInt32(item.GetValue(19));

                product.Articulo.Depto = item.GetValue(20).ToString();
                product.Articulo.SubDepartamento = item.GetValue(21).ToString();
                product.Articulo.Clase = item.GetValue(22).ToString();
                product.Articulo.SubClase = item.GetValue(23).ToString();
                product.Articulo.DescripcionProveedor = item.GetValue(24).ToString();
                list.Add(product);
            }
            return list.ToArray();
        }

    }
}
