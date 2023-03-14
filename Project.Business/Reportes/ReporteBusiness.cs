using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milano.BackEnd.Dto;
using System.Collections;
using Milano.BackEnd.Repository;
using Milano.BackEnd.Dto.Reportes;
using Milano.BackEnd.Dto.InicioFinDia;
using Milano.BackEnd.Dto.Impresion;

namespace Milano.BackEnd.Business
{
    /// <summary>
    /// Clase para generar los reportes
    /// </summary>
    public class ReporteBusiness : BaseBusiness
    {

        /// <summary>
        /// Repositorio de reportes
        /// </summary>
        protected ReporteRepository repository;

        /// <summary>
        ///Atributo del token usuario
        /// </summary>
        protected TokenDto token;

        /// <summary>
        /// Constructor por default
        /// </summary>
        public ReporteBusiness()
        {
            this.repository = new ReporteRepository();
        }

        /// <summary>
        /// Constructor con token
        /// </summary>
        public ReporteBusiness(TokenDto token)
        {
            this.repository = new ReporteRepository();
            this.token = token;
        }

        /// <summary>
        /// Reporte de Ventas por Departamento
        /// </summary>
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<ReporteVentaDepartamentoResponse[]> ReporteVentasPorDepartamento(ReporteVentaDepartamentoRequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.ReporteVentaDepartamento(request.FechaInicial, request.FechaFinal);
            });
        }

        /// <summary>
        /// Reporte de Ventas por SKU
        /// </summary>
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<ReporteVentaSKUResponse[]> ReporteVentasPorSKU(ReporteVentaSKURequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.ReporteVentaSKU(request.FechaInicial, request.FechaFinal, token.CodeStore);
            });
        }

        /// <summary>
        /// Reporte de Devoluciones por SKU
        /// </summary>
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<ReporteDevolucionesSKUResponse[]> ReporteDevolucionesPorSKU(ReporteDevolucionesSKURequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.ReporteDevolucionesSKU(request.FechaInicial, request.FechaFinal, token.CodeStore);
            });
        }
        /// <summary>
        /// Reporte de Ventas por Vendedor
        /// </summary>
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<ReporteVentaVendedorResponse[]> ReporteVentasPorVendedor(ReporteVentaVendedorRequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.ReporteVentaVendedor(request.FechaInicial, request.FechaFinal, token.CodeStore);
            });
        }

        /// <summary>
        /// Reporte de Ventas por Caja
        /// </summary>
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<ReporteVentaCajaResponse[]> ReporteVentasPorCaja(ReporteVentaCajaRequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.ReporteVentaCaja(request.FechaInicial, request.FechaFinal, token.CodeStore);
            });
        }

        /// <summary>
        /// Reporte de Ventas por Hora
        /// </summary>
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<ReporteVentasPorHoraResponse[]> ReporteVentasPorHora(ReporteVentasPorHoraRequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.ReporteVentasPorHora(request.FechaInicial, request.FechaFinal, token.CodeStore);
            });
        }

        /// <summary>
        /// Reporte de Apartados sin Detalle
        /// </summary>
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<ReporteApartadosSinDetalleResponse[]> ReporteApartadosSinDetalle(ReporteApartadosSinDetalleRequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.ReporteApartadosSinDetalle(request.FechaInicial, request.FechaFinal, token.CodeStore);
            });
        }

        /// <summary>
        /// Reporte de Apartados con Detalle
        /// </summary>
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<ReporteApartadosDetalleResponse[]> ReporteApartadosDetalle(ReporteApartadosDetalleRequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.ReporteApartadosDetalle(request.FechaInicial, request.FechaFinal, token.CodeStore);
            });
        }

        /// <summary>
        /// Reporte de Ingresos y Egresos
        /// </summary>
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<ReporteIngresosEgresosResponse[]> ReporteIngresosEgresos(ReporteIngresosEgresosRequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.ReporteIngresosEgresos(request.FechaInicial, request.FechaFinal, token.CodeStore);
            });

        }

        /// <summary>
        /// Reporte de Relacion de Caja
        /// </summary>
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<RelacionCaja[]> ReporteRelacionCaja(ReporteRelacionCajaRequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.ReporteRelacionCaja(token.CodeStore, request.FechaInicial, request.FechaFinal, request.NumeroPagina, request.RegistrosPorPagina);
            });
        }

    }
}
