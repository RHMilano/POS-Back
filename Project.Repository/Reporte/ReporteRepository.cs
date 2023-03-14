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
using Milano.BackEnd.Dto.Reportes;
using Milano.BackEnd.Dto.InicioFinDia;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Dto.Impresion;

namespace Milano.BackEnd.Repository
{

    /// <summary>
    /// Repositorio de reportes
    /// </summary>
    public class ReporteRepository : BaseRepository
    {

        /// <summary>
        /// Reporte de Ventas por Departamento
        /// </summary>
        /// <param name="fechaInicial">Código de la tienda</param>
        /// <param name="fechaFinal">Código de la caja</param>        
        /// <returns>Respuesta de la operación</returns>
        public ReporteVentaDepartamentoResponse[] ReporteVentaDepartamento(string fechaInicial, string fechaFinal)
        {
            List<ReporteVentaDepartamentoResponse> list = new List<ReporteVentaDepartamentoResponse>();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FechaInicial", fechaInicial);
            parameters.Add("@FechaFinal", fechaFinal);
            foreach (var r in data.GetDataReader("[dbo].[sp_vanti_ReporteVentasPorDepartamento]", parameters))
            {
                ReporteVentaDepartamentoResponse response = new ReporteVentaDepartamentoResponse();
                response.Departamento = r.GetValue(0).ToString();
                response.SubDepartamento = r.GetValue(1).ToString();
                response.CantidadVendidaActual = Convert.ToInt32(r.GetValue(2));
                response.VentasConIvaActual = Convert.ToDecimal(r.GetValue(3));
                response.VentasSinIvaActual = Convert.ToDecimal(r.GetValue(4));
                response.DevolucionConIvaActual = Convert.ToDecimal(r.GetValue(5));
                response.DevolucionSinIvaActual = Convert.ToDecimal(r.GetValue(6));
                response.VentaNetaConIvaActual = Convert.ToDecimal(r.GetValue(7));
                response.ContribucionVsTotalActual = Convert.ToDecimal(r.GetValue(8));
                response.CantidadVendidaAnterior = Convert.ToInt32(r.GetValue(9));
                response.VentasConIvaAnterior = Convert.ToDecimal(r.GetValue(10));
                response.VentasSinIvaAnterior = Convert.ToDecimal(r.GetValue(11));
                response.DevolucionConIvaAnterior = Convert.ToDecimal(r.GetValue(12));
                response.DevolucionSinIvaAnterior = Convert.ToDecimal(r.GetValue(13));
                response.VentaNetaConIvaAnterior = Convert.ToDecimal(r.GetValue(14));
                response.ContribucionVsTotalAnterior = Convert.ToDecimal(r.GetValue(15));
                list.Add(response);
            }
            return list.ToArray();
        }

        /// <summary>
        /// Reporte de Ventas por SKU
        /// </summary>
        /// <param name="fechaInicial">Código de la tienda</param>
        /// <param name="fechaFinal">Código de la caja</param>
        /// <param name="codeStore">Código del empleado</param>
        /// <returns>Respuesta de la operación</returns>
        public ReporteVentaSKUResponse[] ReporteVentaSKU(string fechaInicial, string fechaFinal, int codeStore)
        {
            List<ReporteVentaSKUResponse> list = new List<ReporteVentaSKUResponse>();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FechaInicial", fechaInicial);
            parameters.Add("@FechaFinal", fechaFinal);
            parameters.Add("@CodigoTienda", codeStore);
            foreach (var r in data.GetDataReader("[dbo].[sp_vanti_ReporteVentaPorSKU]", parameters))
            {
                ReporteVentaSKUResponse response = new ReporteVentaSKUResponse();
                response.SKU = r.GetValue(0).ToString();
                response.Descripcion = r.GetValue(1).ToString();
                response.Proveedor = Convert.ToInt32(r.GetValue(2));
                response.Estilo = r.GetValue(3).ToString();
                response.Cant = Convert.ToInt32(r.GetValue(4));
                response.VentasNetas = Convert.ToDecimal(r.GetValue(5));
                list.Add(response);
            }
            return list.ToArray();
        }

        /// <summary>
        /// Reporte de Devoluciones por SKU
        /// </summary>
        /// <param name="fechaInicial">Código de la tienda</param>
        /// <param name="fechaFinal">Código de la caja</param>
        /// <param name="codeStore">Código del empleado</param>
        /// <returns>Respuesta de la operación</returns>
        public ReporteDevolucionesSKUResponse[] ReporteDevolucionesSKU(string fechaInicial, string fechaFinal, int codeStore)
        {
            List<ReporteDevolucionesSKUResponse> list = new List<ReporteDevolucionesSKUResponse>();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FechaInicial", fechaInicial);
            parameters.Add("@FechaFinal", fechaFinal);
            parameters.Add("@CodigoTienda", codeStore);
            foreach (var r in data.GetDataReader("[dbo].[sp_vanti_ReporteDevolucionesPorSKU]", parameters))
            {
                ReporteDevolucionesSKUResponse response = new ReporteDevolucionesSKUResponse();
                response.SKU = r.GetValue(0).ToString();
                response.Descripcion = r.GetValue(1).ToString();
                response.Proveedor = Convert.ToInt32(r.GetValue(2));
                response.Estilo = r.GetValue(3).ToString();
                response.Cant = Convert.ToInt32(r.GetValue(4));
                response.Importe = Convert.ToDecimal(r.GetValue(5));
                list.Add(response);
            }
            return list.ToArray();
        }

        /// <summary>
        /// Reporte de Ventas por Vendedor
        /// </summary>
        /// <param name="fechaInicial">Código de la tienda</param>
        /// <param name="fechaFinal">Código de la caja</param>
        /// <param name="codeStore">Código del empleado</param>
        /// <returns>Respuesta de la operación</returns>
        public ReporteVentaVendedorResponse[] ReporteVentaVendedor(string fechaInicial, string fechaFinal, int codeStore)
        {
            List<ReporteVentaVendedorResponse> list = new List<ReporteVentaVendedorResponse>();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FechaInicial", fechaInicial);
            parameters.Add("@FechaFinal", fechaFinal);
            parameters.Add("@CodigoTienda", codeStore);
            foreach (var r in data.GetDataReader("[dbo].[sp_vanti_ReporteVentaPorVendedor]", parameters))
            {
                ReporteVentaVendedorResponse response = new ReporteVentaVendedorResponse();
                response.NumeroVendedor = r.GetValue(0).ToString();
                response.NombreVendedor = r.GetValue(1).ToString();
                response.VentasBrutas = Convert.ToDecimal(r.GetValue(2));
                response.Devoluciones = Convert.ToInt32(r.GetValue(3));
                response.VentasNetas = Convert.ToDecimal(r.GetValue(4));
                response.NumPzas = Convert.ToInt32(r.GetValue(5));
                response.NumTransacciones = Convert.ToInt32(r.GetValue(6));
                response.PPP = Convert.ToInt32(r.GetValue(7));
                response.IndiceVta = Convert.ToInt32(r.GetValue(8));
                response.TickProm = Convert.ToInt32(r.GetValue(9));
                list.Add(response);
            }
            return list.ToArray();
        }

        /// <summary>
        /// Reporte de Ventas por Caja
        /// </summary>
        /// <param name="fechaInicial">Código de la tienda</param>
        /// <param name="fechaFinal">Código de la caja</param>
        /// <param name="codeStore">Código del empleado</param>
        /// <returns>Respuesta de la operación</returns>
        public ReporteVentaCajaResponse[] ReporteVentaCaja(string fechaInicial, string fechaFinal, int codeStore)
        {
            List<ReporteVentaCajaResponse> list = new List<ReporteVentaCajaResponse>();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FechaInicial", fechaInicial);
            parameters.Add("@FechaFinal", fechaFinal);
            parameters.Add("@CodigoTienda", codeStore);
            foreach (var r in data.GetDataReader("[dbo].[sp_vanti_ReporteVentaPorCaja]", parameters))
            {
                ReporteVentaCajaResponse response = new ReporteVentaCajaResponse();
                response.Caja = Convert.ToInt32(r.GetValue(0));
                response.VentaTotal = Convert.ToDecimal(r.GetValue(1));
                response.Devolucion = Convert.ToInt32(r.GetValue(2));
                response.VentaNeta = Convert.ToDecimal(r.GetValue(3));
                response.NumTran = Convert.ToInt32(r.GetValue(4));
                response.TickProm = Convert.ToInt32(r.GetValue(5));
                list.Add(response);
            }
            return list.ToArray();
        }

        /// <summary>
        /// Reporte de Ventas por Hora
        /// </summary>
        /// <param name="fechaInicial">Código de la tienda</param>
        /// <param name="fechaFinal">Código de la caja</param>
        /// <param name="codeStore">Código del empleado</param>
        /// <returns>Respuesta de la operación</returns>
        public ReporteVentasPorHoraResponse[] ReporteVentasPorHora(string fechaInicial, string fechaFinal, int codeStore)
        {
            List<ReporteVentasPorHoraResponse> list = new List<ReporteVentasPorHoraResponse>();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FechaInicial", fechaInicial);
            parameters.Add("@FechaFinal", fechaFinal);
            parameters.Add("@CodigoTienda", codeStore);
            foreach (var r in data.GetDataReader("[dbo].[sp_vanti_ReporteVentasPorHora]", parameters))
            {
                ReporteVentasPorHoraResponse response = new ReporteVentasPorHoraResponse();
                response.Piezas = Convert.ToInt32(r.GetValue(0));
                response.Fecha = r.GetValue(1).ToString();
                response.Hora = r.GetValue(2).ToString();
                response.Venta = Convert.ToDecimal(r.GetValue(3));
                response.NumTransacciones = Convert.ToInt32(r.GetValue(4));
                response.TickProm = Convert.ToDecimal(r.GetValue(5));
                response.PPP = Convert.ToDecimal(r.GetValue(6));
                response.IndiceVta = Convert.ToDecimal(r.GetValue(7));
                list.Add(response);
            }
            return list.ToArray();
        }

        /// <summary>
        /// Reporte de Apartados Sin Detalle
        /// </summary>
        /// <param name="fechaInicial">Código de la tienda</param>
        /// <param name="fechaFinal">Código de la caja</param>
        /// <param name="codeStore">Código del empleado</param>
        /// <returns>Respuesta de la operación</returns>
        public ReporteApartadosSinDetalleResponse[] ReporteApartadosSinDetalle(string fechaInicial, string fechaFinal, int codeStore)
        {
            List<ReporteApartadosSinDetalleResponse> list = new List<ReporteApartadosSinDetalleResponse>();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FechaInicial", fechaInicial);
            parameters.Add("@FechaFinal", fechaFinal);
            parameters.Add("@CodigoTienda", codeStore);
            foreach (var r in data.GetDataReader("[dbo].[sp_vanti_ReporteApartadosSinDetalle]", parameters))
            {
                ReporteApartadosSinDetalleResponse response = new ReporteApartadosSinDetalleResponse();
                response.FolioApartado = r.GetValue(0).ToString();
                response.ImporteApartado = Convert.ToDecimal(r.GetValue(1));
                response.Saldo = Convert.ToDecimal(r.GetValue(2));
                response.FechaApertura = r.GetValue(3).ToString();
                response.Estatus = r.GetValue(4).ToString();
                response.FechaVencimiento = r.GetValue(5).ToString();
                response.NumTelefono = r.GetValue(6).ToString();
                list.Add(response);
            }
            return list.ToArray();
        }

        /// <summary>
        /// Reporte de Apartados con Detalle
        /// </summary>
        /// <param name="fechaInicial">Código de la tienda</param>
        /// <param name="fechaFinal">Código de la caja</param>
        /// <param name="codeStore">Código del empleado</param>
        /// <returns>Respuesta de la operación</returns>
        public ReporteApartadosDetalleResponse[] ReporteApartadosDetalle(string fechaInicial, string fechaFinal, int codeStore)
        {
            List<ReporteApartadosDetalleResponse> listaApartados = new List<ReporteApartadosDetalleResponse>();
            var reportesProcesados = new Dictionary<String, ReporteApartadosDetalleResponse>();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FechaInicial", fechaInicial);
            parameters.Add("@FechaFinal", fechaFinal);
            parameters.Add("@CodigoTienda", codeStore);
            foreach (var r in data.GetDataReader("[dbo].[sp_vanti_ReporteApartadosDetalle]", parameters))
            {
                String folioApartado = r.GetValue(0).ToString();

                // Se agrega el detalle del Apartado
                ReporteApartadosDetalleLineaResponse reporteApartadosDetalleLineaResponse = new ReporteApartadosDetalleLineaResponse();
                reporteApartadosDetalleLineaResponse.Sku = Convert.ToInt32(r.GetValue(9).ToString());
                reporteApartadosDetalleLineaResponse.DescripcionCorta = r.GetValue(10).ToString();
                reporteApartadosDetalleLineaResponse.TotalPiezas = Convert.ToInt32(r.GetValue(11).ToString());
                reporteApartadosDetalleLineaResponse.Total = Convert.ToDecimal(r.GetValue(12).ToString());
                

                // Se procesa
                ReporteApartadosDetalleResponse reporteApartadosDetalleResponse;
                if (reportesProcesados.ContainsKey(folioApartado))
                {
                    reporteApartadosDetalleResponse = reportesProcesados[folioApartado];
                    List<ReporteApartadosDetalleLineaResponse> listaDetalle = reporteApartadosDetalleResponse.ReporteApartadosDetalleLineaResponse.ToList();
                    listaDetalle.Add(reporteApartadosDetalleLineaResponse);
                    reporteApartadosDetalleResponse.ReporteApartadosDetalleLineaResponse = listaDetalle.ToArray();
                }
                else
                {
                    reporteApartadosDetalleResponse = new ReporteApartadosDetalleResponse();
                    reporteApartadosDetalleResponse.FolioApartado = r.GetValue(0).ToString();
                    reporteApartadosDetalleResponse.ImporteApartado = Convert.ToDecimal(r.GetValue(1));
                    reporteApartadosDetalleResponse.Saldo = Convert.ToDecimal(r.GetValue(2));
                    reporteApartadosDetalleResponse.FechaApertura = r.GetValue(3).ToString();
                    reporteApartadosDetalleResponse.Estatus = r.GetValue(4).ToString();
                    reporteApartadosDetalleResponse.FechaVencimiento = r.GetValue(5).ToString();
                    reporteApartadosDetalleResponse.NumTelefono = r.GetValue(6).ToString();
                    reporteApartadosDetalleResponse.Fecha = r.GetValue(7).ToString();
                    reporteApartadosDetalleResponse.Monto = Convert.ToDecimal(r.GetValue(8));
                    // Se agrega el detalle
                    List<ReporteApartadosDetalleLineaResponse> listaDetalle = new List<ReporteApartadosDetalleLineaResponse>();
                    //List<ReporteApartadosDetalleLineaResponse> skuList = new List<ReporteApartadosDetalleLineaResponse>();
                    listaDetalle.Add(reporteApartadosDetalleLineaResponse);
                    reporteApartadosDetalleResponse.ReporteApartadosDetalleLineaResponse = listaDetalle.ToArray();
                    reportesProcesados.Add(folioApartado, reporteApartadosDetalleResponse);
                    listaApartados.Add(reporteApartadosDetalleResponse);
                } 
            }
            return listaApartados.ToArray();
        }

        /// <summary>
        /// Reporte de Ingresos y Egresos
        /// </summary>
        /// <param name="fechaInicial">Código de la tienda</param>
        /// <param name="fechaFinal">Código de la caja</param>
        /// <param name="codeStore">Código del empleado</param>
        /// <returns>Respuesta de la operación</returns>
        public ReporteIngresosEgresosResponse[] ReporteIngresosEgresos(string fechaInicial, string fechaFinal, int codeStore)
        {
            List<ReporteIngresosEgresosResponse> list = new List<ReporteIngresosEgresosResponse>();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FechaInicial", fechaInicial);
            parameters.Add("@FechaFinal", fechaFinal);
            parameters.Add("@CodigoTienda", codeStore);
            foreach (var r in data.GetDataReader("[dbo].[sp_vanti_ReporteIngresosEgresos]", parameters))
            {
                ReporteIngresosEgresosResponse response = new ReporteIngresosEgresosResponse();
                response.Caja = Convert.ToInt32(r.GetValue(0));
                response.Transaccion = r.GetValue(1).ToString();
                response.Fecha = r.GetValue(2).ToString();
                response.Hora = r.GetValue(3).ToString();
                response.TipoTransaccion = r.GetValue(4).ToString();
                response.Razon = r.GetValue(5).ToString();
                response.Importe = Convert.ToDecimal(r.GetValue(6));
                list.Add(response);
            }
            return list.ToArray();
        }

        /// <summary>
        /// Reporte de Relacion de Caja
        /// </summary>
        /// <param name="codeStore">Código del empleado</param>
        /// <param name="fechaInicial">Fecha inicial</param>
        /// <param name="fechaFinal">Fecha final</param>        
        /// <param name="numeroPagina">Números por página</param> 
        /// <param name="registrosPorPagina">Registros por página</param> 
        /// <returns>Respuesta de la operación</returns>
        public RelacionCaja[] ReporteRelacionCaja(int codeStore, string fechaInicial, string fechaFinal, int numeroPagina, int registrosPorPagina)
        {
            ConfigGeneralesCajaTiendaFormaPago InformacionFormaPago = new ConfigGeneralesCajaTiendaFormaPago();

            List<RelacionCaja> relacionesCaja = new List<RelacionCaja>();
            List<RelacionCajaRespose> relacionCajas = new List<RelacionCajaRespose>();

            // Objeto y lista de DepositosAsociados
            DepositoAsociado depositoAso = new DepositoAsociado();
            List<DepositoAsociado> depositosAso = new List<DepositoAsociado>();

            var parameters = new Dictionary<string, object>();
            parameters.Add("@FechaInicial", fechaInicial);
            parameters.Add("@FechaFinal", fechaFinal);
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@NumeroPagina", numeroPagina);
            parameters.Add("@RegistrosPorPagina", registrosPorPagina);
            foreach (var r in data.GetDataReader("[dbo].[sp_vanti_server_ReporteRelacionesCaja]", parameters))
            {

                // Obtengo datos de relacion de caja
                RelacionCaja relacionCaja = new RelacionCaja();
                relacionCaja.TotalRegistros = Convert.ToInt32(r.GetValue(0));
                relacionCaja.IdRelacionCaja = Convert.ToInt32(r.GetValue(1));
                relacionCaja.CodigoTienda = Convert.ToInt32(r.GetValue(2));
                relacionCaja.TotalConIVA = Convert.ToDecimal(r.GetValue(3));
                relacionCaja.TotalSinIVA = Convert.ToDecimal(r.GetValue(4));
                relacionCaja.IVA = Convert.ToDecimal(r.GetValue(5));
                relacionCaja.Fecha = Convert.ToString(r.GetValue(6));

                var parametersRelacion = new Dictionary<string, object>();
                parametersRelacion.Add("@CodigoRelacion", Convert.ToInt32(r.GetValue(1)));
                foreach (var a in data.GetDataReader("dbo.sp_vanti_server_ReporteRelacionCajaDeposito", parametersRelacion))
                {
                    //Obtenemos depositos asociados al id de relacion de caja.
                    DepositoAsociado depositoAsociado = new DepositoAsociado();
                    depositoAsociado.TotalConIVA = Convert.ToDecimal(a.GetValue(0));

                    //Se debe crear el tipo de dato infomacionAsociadaFormaPago
                    ConfigGeneralesCajaTiendaFormaPago tiendaFormaPago = new ConfigGeneralesCajaTiendaFormaPago();
                    tiendaFormaPago.CodigoFormaPago = Convert.ToString(a.GetValue(1));
                    tiendaFormaPago.IdentificadorFormaPago = Convert.ToString(a.GetValue(1));
                    tiendaFormaPago.DescripcionFormaPago = Convert.ToString(a.GetValue(2));
                    depositoAsociado.InformacionAsociadaFormasPago = tiendaFormaPago;
                    depositosAso.Add(depositoAsociado);
                }
                relacionCaja.DepositosAsociados = depositosAso.ToArray();

                List<GrupoRelacionCaja> gruposIncluidosRelacionCaja = new List<GrupoRelacionCaja>();
                relacionCaja.GruposRelacionCaja = gruposIncluidosRelacionCaja.ToArray();
                var grupos = new Dictionary<String, GrupoRelacionCaja>();
                var secciones = new Dictionary<String, SeccionRelacionCaja>();
                List<GrupoRelacionCaja> gruposRelacionCaja = new List<GrupoRelacionCaja>();
                List<SeccionRelacionCaja> seccionesRelacionCaja = new List<SeccionRelacionCaja>();

                var parametros = new Dictionary<string, object>();
                parametros.Add("@IdRelacionCaja", relacionCaja.IdRelacionCaja);
                foreach (var item in data.GetDataReader("[dbo].[sp_vanti_server_ObtenerReporteRelacionDeCaja]", parametros))
                {
                    // Informacion Grupo
                    String encabezadoGrupo = item.GetValue(0).ToString();
                    // Información Sección
                    String encabezadoSeccion = item.GetValue(1).ToString();

                    // Información Desglose
                    Desglose desglose = new Desglose();
                    desglose.Descripcion = item.GetValue(2).ToString();
                    desglose.TotalConIVA = Convert.ToDecimal(item.GetValue(3));

                    // Llenado de Sección               
                    SeccionRelacionCaja seccionRelacionCajaActual;
                    if (secciones.ContainsKey(encabezadoGrupo + encabezadoSeccion))
                    {
                        seccionRelacionCajaActual = secciones[encabezadoGrupo + encabezadoSeccion];
                        List<Desglose> desgloses = seccionRelacionCajaActual.DesgloseRelacionCaja.ToList();
                        desgloses.Add(desglose);
                        seccionRelacionCajaActual.DesgloseRelacionCaja = desgloses.ToArray();
                    }
                    else
                    {
                        seccionRelacionCajaActual = new SeccionRelacionCaja();
                        seccionRelacionCajaActual.Encabezado = encabezadoSeccion;
                        seccionRelacionCajaActual.IdSeccion = Convert.ToInt32(item.GetValue(4));
                        seccionRelacionCajaActual.TotalConIVA = Convert.ToDecimal(item.GetValue(5));
                        seccionRelacionCajaActual.TotalSinIVA = Convert.ToDecimal(item.GetValue(6));
                        seccionRelacionCajaActual.IVA = Convert.ToInt32(item.GetValue(7));
                        List<Desglose> desgloses = new List<Desglose>();
                        desgloses.Add(desglose);
                        seccionRelacionCajaActual.DesgloseRelacionCaja = desgloses.ToArray();
                        secciones.Add(encabezadoGrupo + encabezadoSeccion, seccionRelacionCajaActual);
                        // Llenado de Grupo                    
                        GrupoRelacionCaja grupoRelacionCajaActual;
                        if (grupos.ContainsKey(encabezadoGrupo))
                        {
                            grupoRelacionCajaActual = grupos[encabezadoGrupo];
                            List<SeccionRelacionCaja> seccionesRelacionCajaGrupo = grupoRelacionCajaActual.SeccionesRelacionCaja.ToList();
                            seccionesRelacionCajaGrupo.Add(seccionRelacionCajaActual);
                            grupoRelacionCajaActual.SeccionesRelacionCaja = seccionesRelacionCajaGrupo.ToArray();
                        }
                        else
                        {
                            grupoRelacionCajaActual = new GrupoRelacionCaja();
                            grupoRelacionCajaActual.Encabezado = encabezadoGrupo;
                            grupoRelacionCajaActual.IdGrupo = Convert.ToInt32(item.GetValue(8));
                            grupoRelacionCajaActual.TotalConIVA = Convert.ToDecimal(item.GetValue(9));
                            grupoRelacionCajaActual.TotalSinIVA = Convert.ToDecimal(item.GetValue(10));
                            grupoRelacionCajaActual.IVA = Convert.ToDecimal(item.GetValue(11));
                            List<SeccionRelacionCaja> seccionesRelacionCajaGrupo = new List<SeccionRelacionCaja>();
                            seccionesRelacionCajaGrupo.Add(seccionRelacionCajaActual);
                            grupoRelacionCajaActual.SeccionesRelacionCaja = seccionesRelacionCajaGrupo.ToArray();
                            grupos.Add(encabezadoGrupo, grupoRelacionCajaActual);
                            // Agregar Grupo hacia Relacion Caja                                                   
                            List<GrupoRelacionCaja> gruposActuales = relacionCaja.GruposRelacionCaja.ToList();
                            gruposActuales.Add(grupoRelacionCajaActual);
                            relacionCaja.GruposRelacionCaja = gruposActuales.ToArray();
                        }
                    }
                }
                relacionesCaja.Add(relacionCaja);
            }
            return relacionesCaja.ToArray();
        }

    }
}
