using Milano.BackEnd.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Milano.BackEnd.Repository
{
    /// <summary>
    /// Repositorio de tiempo aire
    /// </summary>
    public class PagoServiciosRepository : BaseRepository
    {

        TokenDto token;
        /// <summary>
        /// Constructor TiempoAireRepository
        /// </summary>
        /// <param name="token">Token de usuario</param>
        public PagoServiciosRepository(TokenDto token)
        {
            this.token = token;
        }

        /// <summary>
        /// Obtenemos la lista de Empresas tiempo aire
        /// </summary>
        /// <returns>lista de compañias</returns>
        public CompaniasPagoServiciosResponse[] ObtenerListaEmpresasTA()
        {
            List<CompaniasPagoServiciosResponse> list = new List<CompaniasPagoServiciosResponse>();
            var parameters = new Dictionary<string, object>();

            foreach (var r in data.GetDataReader("[dbo].[sp_vanti_TiempoAireObtenerCia]", parameters))
            {
                CompaniasPagoServiciosResponse item = new CompaniasPagoServiciosResponse();
                item.Codigo = r.GetValue(0).ToString();
                item.Marca = r.GetValue(1).ToString();
                item.Nombre = r.GetValue(2).ToString();
                list.Add(item);
            }
            return list.ToArray();
        }

        /// <summary>
        /// Metodo para obtener Lista de productos de tiempo aire
        /// </summary>
        /// <param name="codigoEmpresa"></param>
        /// <returns>lista de montos</returns>
        public ProductsResponse[] ObtenerProductosTA(string codigoEmpresa)
        {
            List<ProductsResponse> list = new List<ProductsResponse>();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoCompania", codigoEmpresa);
            parameters.Add("@CodigoTienda", this.token.CodeStore);
            foreach (var r in data.GetDataReader("[dbo].[sp_vanti_TiempoAireObtenerMontos]", parameters))
            {
                ProductsResponse item = new ProductsResponse();
                item.Articulo = new Articulo();
                item.Articulo.PrecioConImpuestos = Convert.ToInt32(r.GetValue(0).ToString());
                item.Articulo.Sku = Convert.ToInt32(r.GetValue(1).ToString());
                item.Articulo.InformacionProveedorExternoAsociadaPS = new InformacionProveedorExternoAsociadaPS();
                item.Articulo.InformacionTarjetaRegalo = new InformacionTarjetaRegalo();
                item.Articulo.InformacionProveedorExternoTA = new InformacionProveedorExternoTA();
                item.Articulo.InformacionProveedorExternoTA.SkuCompania = r.GetValue(2).ToString();
                item.Articulo.Impuesto1 = decimal.Parse(r.GetValue(5).ToString());
                item.Articulo.Estilo = r.GetValue(4).ToString();
                item.Articulo.Clase = "";
                item.Articulo.CodigoImpuesto1 = "";
                item.Articulo.CodigoImpuesto2 = "";
                item.Articulo.CodigoClase = 0;
                item.Articulo.CodigoDepto = 0;
                item.Articulo.CodigoMarca = 0;
                item.Articulo.CodigoSubClase = 0;
                item.Articulo.CodigoSubDepartamento = 0;
                item.Articulo.Depto = "";
                item.Articulo.DigitoVerificadorArticulo = new DigitoVerificadorArticulo();
                item.Articulo.DigitoVerificadorArticulo.DigitoVerificadorActual = "";
                item.Articulo.DigitoVerificadorArticulo.DigitoVerificadorCorrecto = "";
                item.Articulo.DigitoVerificadorArticulo.Inconsistencia = false;
                item.Articulo.RutaImagenLocal = "";
                item.Articulo.RutaImagenRemota = "";
                item.Articulo.SubClase = "";
                item.Articulo.SubDepartamento = "";
                item.Articulo.Impuesto2 = 0;
                item.Articulo.TasaImpuesto1 = decimal.Parse(r.GetValue(3).ToString());
                item.Articulo.TasaImpuesto2 = 0;
                item.Articulo.Upc = "";

                list.Add(item);
            }
            return list.ToArray();
        }

        /// <summary>
        /// Obtenemos la lista de Empresas Pago de Servicios
        /// </summary>
        /// <returns>lista de compañias</returns>
        public CompaniasPagoServiciosResponse[] ObtenerListaEmpresas()
        {
            List<CompaniasPagoServiciosResponse> list = new List<CompaniasPagoServiciosResponse>();
            var parameters = new Dictionary<string, object>();
            foreach (var r in data.GetDataReader("[dbo].[sp_vanti_PagoServiciosCia]", parameters))
            {
                CompaniasPagoServiciosResponse item = new CompaniasPagoServiciosResponse();
                item.Codigo = r.GetValue(0).ToString();
                item.Marca = r.GetValue(1).ToString();
                item.Nombre = r.GetValue(2).ToString();
                list.Add(item);
            }
            return list.ToArray();
        }

        /// <summary>
        /// Metodo para obtener Lista de productos de Pago de Servicios
        /// </summary>
        /// <param name="codigoEmpresa"></param>
        /// <returns>lista de montos</returns>
        public ProductsResponse[] ObtenerProductos(string codigoEmpresa)
        {
            List<ProductsResponse> list = new List<ProductsResponse>();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoCompania", codigoEmpresa);
            parameters.Add("@CodigoTienda", this.token.CodeStore);
            foreach (var r in data.GetDataReader("[dbo].[sp_vanti_PagoServiciosObtenerMontos]", parameters))
            {
                ProductsResponse item = new ProductsResponse();
                item.Articulo = new Articulo();
                item.Articulo.PrecioConImpuestos = Convert.ToInt32(r.GetValue(0).ToString());
                item.Articulo.Sku = Convert.ToInt32(r.GetValue(1).ToString());
                item.Articulo.InformacionProveedorExternoTA = new InformacionProveedorExternoTA();
                item.Articulo.InformacionTarjetaRegalo = new InformacionTarjetaRegalo();
                item.Articulo.InformacionProveedorExternoAsociadaPS = new InformacionProveedorExternoAsociadaPS();
                item.Articulo.InformacionProveedorExternoAsociadaPS.SkuCompania = r.GetValue(2).ToString().Trim();
                item.Articulo.InformacionProveedorExternoAsociadaPS.Cuenta = "";
                item.Articulo.Estilo = r.GetValue(3).ToString();
                item.Articulo.Clase = "";
                item.Articulo.CodigoImpuesto1 = "";
                item.Articulo.CodigoImpuesto2 = "";
                item.Articulo.CodigoClase = 0;
                item.Articulo.CodigoDepto = 0;
                item.Articulo.CodigoMarca = 0;
                item.Articulo.CodigoSubClase = 0;
                item.Articulo.CodigoSubDepartamento = 0;
                item.Articulo.Depto = "";
                item.Articulo.DigitoVerificadorArticulo = new DigitoVerificadorArticulo();
                item.Articulo.DigitoVerificadorArticulo.DigitoVerificadorActual = "";
                item.Articulo.DigitoVerificadorArticulo.DigitoVerificadorCorrecto = "";
                item.Articulo.DigitoVerificadorArticulo.Inconsistencia = false;
                item.Articulo.RutaImagenLocal = "";
                item.Articulo.RutaImagenRemota = "";
                item.Articulo.SubClase = "";
                item.Articulo.SubDepartamento = "";
                item.Articulo.Impuesto2 = 0;
                item.Articulo.Impuesto1 = 0;
                item.Articulo.TasaImpuesto2 = 0;
                item.Articulo.TasaImpuesto1 = 0;
                item.Articulo.Upc = "";
                list.Add(item);
            }
            return list.ToArray();
        }

        /// <summary>
        /// Registro de autorizacion pago de servicios
        /// </summary>
        /// <param name="skuPago"></param>
        /// <param name="sku"></param>
        /// <param name="folioOperacion"></param>
        /// <param name="autorizacion"></param>
        /// <returns></returns>
        public OperationResponse RegistrarAutorizacionPago(string skuPago, int sku, string folioOperacion, string autorizacion)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@skuPago", skuPago);
            parameters.Add("@sku", sku);
            parameters.Add("@FolioOperacion", folioOperacion);
            parameters.Add("@Autorizacion", autorizacion);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("sp_vanti_PagoServiciosRegistrarAutorizacion", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }

        /// <summary>
        /// Registro de autorizacion pago de tiempo aire
        /// </summary>
        /// <param name="skuPago"></param>
        /// <param name="sku"></param>
        /// <param name="folioOperacion"></param>
        /// <param name="autorizacion"></param>
        /// <returns></returns>
        public OperationResponse RegistrarAutorizacionPagoTiempoAire(string skuPago, int sku, string folioOperacion, string autorizacion)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@skuPago", skuPago);
            parameters.Add("@sku", sku);
            parameters.Add("@FolioOperacion", folioOperacion);
            parameters.Add("@Autorizacion", autorizacion);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_PagoTiempoAireRegistrarAutorizacion]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }

    }
}
