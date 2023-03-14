using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Repository;
using System.Data;
using System.Configuration;
using Milano.BackEnd.Utils;
using Milano.BackEnd.Dto.General;
using System.Transactions;
using System.Globalization;

namespace Milano.BackEnd.Repository
{

    /// <summary>
    /// Repositorio de lecturas XZ
    /// </summary>
    public class LecturaRepository : BaseRepository
    {

        /// <summary>
        /// Regresa el listado de importes por forma de pago registrados en la caja
        /// </summary>
        /// <param name="codeStore">Código de la tienda</param>
        /// <param name="codeBox">Código de la caja</param>        
        /// <returns>Listado de importes</returns>
        public LecturaTotalDetalleFormaPago[] ObtenerTotalesPorFormaPago(int codeStore, int codeBox)
        {
            List<LecturaTotalDetalleFormaPago> list = new List<LecturaTotalDetalleFormaPago>();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@CodigoCaja", codeBox);
            foreach (var r in data.GetDataReader("sp_vanti_ReporteTotalesPorFormaPago", parameters))
            {
                LecturaTotalDetalleFormaPago lectura = new LecturaTotalDetalleFormaPago();
                lectura.ImporteFisico = 0;
                lectura.ImporteRetiro = 0;
                lectura.ImporteTeorico = Convert.ToDecimal(r.GetValue(0));
                lectura.InformacionAsociadaFormasPago = new ConfigGeneralesCajaTiendaFormaPago();
                lectura.InformacionAsociadaFormasPago.IdentificadorFormaPago = r.GetValue(1).ToString();
                lectura.InformacionAsociadaFormasPago.CodigoFormaPago = r.GetValue(2).ToString();
                lectura.InformacionAsociadaFormasPago.DescripcionFormaPago = r.GetValue(3).ToString();
                lectura.TotalIngresosConRetirosParciales = lectura.ImporteTeorico;
                lectura.TotalIngresosConRetirosParcialesConFondoFijo = lectura.ImporteTeorico;
                // Agregar información referente a retiros parciales
                if (lectura.InformacionAsociadaFormasPago.CodigoFormaPago == "CA")
                {
                    List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
                    parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@TotalRetirosParciales", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Decimal, Scale = 2 });
                    var resultado = data.ExecuteProcedure("[dbo].[sp_vanti_ReporteTotalesRetirosParciales]", parameters, parametersOut);
                    lectura.TotalRetirosParciales = Convert.ToDecimal(resultado["@TotalRetirosParciales"]);
                    lectura.TotalIngresosConRetirosParciales = lectura.TotalIngresosConRetirosParciales + lectura.TotalRetirosParciales;
                    // Ajuste para fondo fijo
                    List<System.Data.SqlClient.SqlParameter> parametersFFOut = new List<System.Data.SqlClient.SqlParameter>();
                    parametersFFOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@TotalFondoFijo", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Decimal, Scale = 2 });
                    var resultadoFF = data.ExecuteProcedure("[dbo].[sp_vanti_ReporteTotalesFondoFijo]", parameters, parametersFFOut);
                    lectura.TotalFondoFijo = Convert.ToDecimal(resultadoFF["@TotalFondoFijo"]);
                    lectura.TotalIngresosConRetirosParcialesConFondoFijo = lectura.TotalIngresosConRetirosParciales + lectura.TotalFondoFijo;
                }
                list.Add(lectura);
            }
            return list.ToArray();
        }

        /// <summary>
        /// Regresa el listado de importes por forma de pago registrados en la caja
        /// </summary>
        /// <param name="codeStore">Código de la tienda</param>
        /// <param name="codeBox">Código de la caja</param>        
        /// <param name="caja">Código de la caja sobre la cual se requiere la infromación</param>   
        /// <returns>Listado de importes</returns>
        public LecturaTotalDetalleFormaPago[] ObtenerTotalesPorFormaPago(int codeStore, int codeBox, int caja)
        {
            List<LecturaTotalDetalleFormaPago> list = new List<LecturaTotalDetalleFormaPago>();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@CodigoCaja", codeBox);
            parameters.Add("@CodigoCajaInformacion", caja);
            foreach (var r in data.GetDataReader("sp_vanti_server_ReporteTotalesPorFormaPago", parameters))
            {
                LecturaTotalDetalleFormaPago lectura = new LecturaTotalDetalleFormaPago();
                lectura.ImporteFisico = 0;
                lectura.ImporteRetiro = 0;
                lectura.ImporteTeorico = Convert.ToDecimal(r.GetValue(0));
                lectura.InformacionAsociadaFormasPago = new ConfigGeneralesCajaTiendaFormaPago();
                lectura.InformacionAsociadaFormasPago.IdentificadorFormaPago = r.GetValue(1).ToString();
                lectura.InformacionAsociadaFormasPago.CodigoFormaPago = r.GetValue(2).ToString();
                lectura.InformacionAsociadaFormasPago.DescripcionFormaPago = r.GetValue(3).ToString();
                lectura.TotalIngresosConRetirosParciales = lectura.ImporteTeorico;
                lectura.TotalIngresosConRetirosParcialesConFondoFijo = lectura.ImporteTeorico;
                // Agregar información referente a retiros parciales
                if (lectura.InformacionAsociadaFormasPago.CodigoFormaPago == "CA")
                {
                    List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
                    parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@TotalRetirosParciales", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Decimal, Scale = 2 });
                    var resultado = data.ExecuteProcedure("[dbo].[sp_vanti_server_ReporteTotalesRetirosParciales]", parameters, parametersOut);
                    lectura.TotalRetirosParciales = Convert.ToDecimal(resultado["@TotalRetirosParciales"]);
                    lectura.TotalIngresosConRetirosParciales = lectura.TotalIngresosConRetirosParciales + lectura.TotalRetirosParciales;
                    // Ajuste para fondo fijo
                    List<System.Data.SqlClient.SqlParameter> parametersFFOut = new List<System.Data.SqlClient.SqlParameter>();
                    parametersFFOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@TotalFondoFijo", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Decimal, Scale = 2 });
                    var resultadoFF = data.ExecuteProcedure("[dbo].[sp_vanti_server_ReporteTotalesFondoFijo]", parameters, parametersFFOut);
                    lectura.TotalFondoFijo = Convert.ToDecimal(resultadoFF["@TotalFondoFijo"]);
                    lectura.TotalIngresosConRetirosParcialesConFondoFijo = lectura.TotalIngresosConRetirosParciales + lectura.TotalFondoFijo;
                }
                list.Add(lectura);
            }
            return list.ToArray();
        }

        /// <summary>
        /// Obtener folios par una lectura X
        /// </summary>
        /// <param name="codeStore">Código de la tienda</param>
        /// <param name="codeBox">Código de la caja</param>
        /// <param name="codeEmployee">Código del empleado</param>       
        /// <param name="esLecturaOffline">Indica si se trata de una Lectura Offline</param>  
        /// <returns>Respuesta de la operación</returns>
        public LecturaX ObtenerFoliosLecturaX(int codeStore, int codeBox, int codeEmployee, int esLecturaOffline)
        {
            LecturaX lecturaX = new LecturaX();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@CodigoCaja", codeBox);
            parameters.Add("@CodigoEmpleado", codeEmployee);
            parameters.Add("@EsLecturaOffline", esLecturaOffline);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@FolioCorteParcial", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 17 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@UltimoFolioVenta", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 17 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@UltimoFolioDevolucion", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 17 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@UltimoFolioApartados", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 17 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@UltimoFolioRetiros", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 17 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@UltimoFolioTransacciones", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@UltimoFolioPagosMayorista", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 17 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@UltimoFolioPagosMM", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 17 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_ObtenerFoliosLecturaX]", parameters, parametersOut);
            lecturaX.FolioCorteParcial = result["@FolioCorteParcial"].ToString();
            lecturaX.UltimoFolioVenta = result["@UltimoFolioVenta"].ToString();
            lecturaX.UltimoFolioDevolucion = result["@UltimoFolioDevolucion"].ToString();
            lecturaX.UltimoFolioApartados = result["@UltimoFolioApartados"].ToString();
            lecturaX.UltimoFolioRetiros = result["@UltimoFolioRetiros"].ToString();
            lecturaX.UltimoFolioTransacciones = Convert.ToInt32(result["@UltimoFolioTransacciones"].ToString());
            lecturaX.UltimoFolioPagosMayorista = result["@UltimoFolioPagosMayorista"].ToString();
            lecturaX.UltimoFolioPagosMM = result["@UltimoFolioPagosMM"].ToString();
            return lecturaX;
        }

        /// <summary>
        /// Funcionalidad para persistir una lectura X
        /// </summary>
        /// <param name="codeStore">Código de la tienda</param>
        /// <param name="codeBox">Código de la caja</param>
        /// <param name="codeEmployee">Código del empleado</param>
        /// <param name="codigoFormaPago">Código de la forma de pago</param>
        /// <param name="secuencia">Número de secuencia</param>
        /// <param name="importeFisico">Importe físico</param>
        /// <param name="importeTeorico">Importe teórico</param>
        /// <param name="importeRetiro">Importe retiro</param>
        /// <param name="lecturaX">Folios de Lectura X asociados</param>
        /// <returns>Respuesta de la operación</returns>
        public OperationResponse LecturaX(int codeStore, int codeBox, int codeEmployee, string codigoFormaPago, int secuencia,
            decimal importeFisico, decimal importeTeorico, decimal importeRetiro, LecturaX lecturaX)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@CodigoCaja", codeBox);
            parameters.Add("@CodigoEmpleado", codeEmployee);
            parameters.Add("@ImporteFisico", importeFisico);
            parameters.Add("@ImporteTeorico", importeTeorico);
            parameters.Add("@ImporteRetiro", importeRetiro);
            parameters.Add("@CodigoFormaPago", codigoFormaPago);
            parameters.Add("@Secuencia", secuencia);
            parameters.Add("@FolioLectura", lecturaX.FolioCorteParcial);
            parameters.Add("@UltimoFolioVenta", lecturaX.UltimoFolioVenta);
            parameters.Add("@UltimoFolioDevolucion", lecturaX.UltimoFolioDevolucion);
            parameters.Add("@UltimoFolioApartados", lecturaX.UltimoFolioApartados);
            parameters.Add("@UltimoFolioRetiros", lecturaX.UltimoFolioRetiros);
            parameters.Add("@UltimoFolioTransacciones", lecturaX.UltimoFolioTransacciones);
            parameters.Add("@UltimoFolioPagosMayorista", lecturaX.UltimoFolioPagosMayorista);
            parameters.Add("@UltimoFolioPagosMM", lecturaX.UltimoFolioPagosMM);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_LecturaX]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }

        /// <summary>
        /// Funcionalidad para persistir una lectura Z
        /// </summary>
        /// <param name="codeStore">Código de la tienda</param>
        /// <param name="codeBox">Código de la caja</param>
        /// <param name="codeEmployee">Código del empleado</param>
        /// <param name="folioCorteParcial">Folio del Corte Parcial</param>
        /// <param name="importeFisico">Importe físico</param>
        /// <param name="importeTeorico">Importe teórico</param>    
        /// <param name="importeRetiro">Importe retiro</param>
        /// <param name="esLecturaOffline">Indica si se trata de una Lectura Offline</param>
        /// <returns>Respuesta de la operación</returns>
        public LecturaZGuardarResponse LecturaZ(int codeStore, int codeBox, int codeEmployee, string folioCorteParcial, decimal importeFisico,
            decimal importeTeorico, decimal importeRetiro, int esLecturaOffline)
        {
            LecturaZGuardarResponse operationResponse = new LecturaZGuardarResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@CodigoCaja", codeBox);
            parameters.Add("@CodigoEmpleado", codeEmployee);
            parameters.Add("@ImporteFisico", importeFisico);
            parameters.Add("@ImporteTeorico", importeTeorico);
            parameters.Add("@ImporteRetiro", importeRetiro);
            parameters.Add("@FolioCorteParcial", folioCorteParcial);
            parameters.Add("@EsLecturaOffline", esLecturaOffline);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@FolioLecturaResult", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 17 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_LecturaZ]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            operationResponse.FolioCorte = result["@FolioLecturaResult"].ToString();
            return operationResponse;
        }

        /// <summary>
        /// Funcionalidad para persistir el detalle de las denominaciones
        /// </summary>
        /// <param name="folioRetiro">Folio del retiro</param>
        /// <param name="codigoFormaPago">Código de la Forma de Pago</param>
        /// <param name="denominacion">Denominacion</param>         
        /// <param name="cantidad">Cantidad</param>        
        public void PersistirDenominacionesRetiro(string folioRetiro, string codigoFormaPago, String denominacion, int cantidad)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioRetiro", folioRetiro);
            parameters.Add("@CodigoFormaDePago", codigoFormaPago);
            parameters.Add("@Denominacion", Convert.ToDecimal(denominacion));
            parameters.Add("@Cantidad", cantidad);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            data.ExecuteProcedure("[dbo].[sp_vanti_PersistirDenominacionesRetiro]", parameters, parametersOut);
        }

        /// <summary>
        /// Regresa el listado de importes por forma de pago registrados en la caja
        /// </summary>        
        /// <returns>Listado de importes</returns>
        public Denominacion[] ObtenerDenominacionesPorMarca(int codeStore)
        {
            //Creamos la lista donde se tendran todas las denominaciones que se encuentren en BD
            List<Denominacion> listaDenominaciones = new List<Denominacion>();
            var parameters = new Dictionary<string, object>();//Parametros de entrada al SP
            parameters.Add("@CodigoTienda", codeStore);//Mandamos como parametro el codigo de tienda que se obtiene en el token
            foreach (var r in data.GetDataReader("sp_vanti_ObtenerDenominaciones", parameters))//Leemos los datos que se obtenen en la ejecucion del SP
            {
                Denominacion denominacion = new Denominacion();//Se crea un objeto Denominacion por cada iteracion para guardarlo en la lista
                denominacion.CodigoFormaPago = r.GetValue(0).ToString();
                denominacion.Cantidad = 0;
                denominacion.ValorDenominacion = Convert.ToDecimal(r.GetValue(2));
                denominacion.TextoDenominacion = r.GetValue(2).ToString();

                listaDenominaciones.Add(denominacion);
            }
            return listaDenominaciones.ToArray();//Convertimos la lista en un arreglo.            
        }

    }

}