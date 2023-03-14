using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milano.BackEnd.Dto;
using System.Collections;
using Milano.BackEnd.Repository;
using Milano.BackEnd.Dto.Sales;
using System.Transactions;

namespace Milano.BackEnd.Business.Sales

{

    /// <summary>
    /// Servicio para gestionar operaciones de descuentos y devoluciones
    /// </summary>
    public class DescuentosDevolucionesBusiness : BaseBusiness
    {
        /// <summary>
        /// Repositorio de descuentos y devoluciones
        /// </summary>
        protected DescuentosDevolucionesRepository repository;

        /// <summary>
        /// Atributo del token usuario
        /// </summary>
        protected TokenDto token;

        /// <summary>
        /// Constructor por default
        /// </summary>
        public DescuentosDevolucionesBusiness(TokenDto token)
        {
            this.repository = new DescuentosDevolucionesRepository();
            this.token = token;
        }

        /// <summary>
        /// Método para validar si aplica una devolución por la fecha de la venta original
        /// </summary>
        /// <param name="folioVenta">Folio de la venta original</param>
        /// <returns>Resultado de la operación</returns>
        public ResponseBussiness<OperationResponse> ValidarDevolucionVenta(string folioVenta)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.ValidarDevolucionVenta(token.CodeStore, token.CodeBox, token.CodeEmployee, folioVenta);
            });
        }

        /// <summary>
        /// Método para generar una devolución
        /// </summary>
        /// <param name="folioVenta">Objeto de peticion de linea ticket de la venta</param>
        /// <returns>Resultado de la operación</returns>
        public ResponseBussiness<VentaResponse> GenerarDevolucion(String folioVenta)
        {
            return tryCatch.SafeExecutor(() =>
            {
                VentaResponse ventaResponse = new VentaResponse();
                using (TransactionScope scope = new TransactionScope())
                {
                    ventaResponse = new SalesBusiness(token).BuscarVentaPorFolio(folioVenta, 1);
                    // Asignar bandera de devolución a lineas existentes
                    foreach (var item in ventaResponse.Lineas)
                    {
                        item.PerteneceVentaOriginal = true;
                    }

                    // Generar la devolución                                        
                    DevolucionRespose devolucionRespose = repository.GenerarDevolucion(token.CodeStore, token.CodeBox, token.CodeEmployee,
                        ventaResponse.TipoCabeceraVenta, ventaResponse.FolioVenta, ventaResponse.NumeroNominaVentaEmpleado, ventaResponse.CodigoMayorista);
                    ventaResponse.FolioVentaOriginal = ventaResponse.FolioVenta;
                    ventaResponse.FolioDevolucion = devolucionRespose.FolioDevolucion;
                    ventaResponse.FolioVenta = devolucionRespose.FolioVenta;
                    ventaResponse.ImporteVentaNetoOriginal = ventaResponse.ImporteVentaNeto;
                    scope.Complete();
                }
                return ventaResponse;
            });
        }

        /// <summary>
        /// Método para aplicar un cambio de piezas sobre un artículo devuelto
        /// </summary>
        /// <param name="devolverArticuloRequest">Objeto de peticion de linea ticket de la venta</param>
        /// <returns>Resultado de la operación</returns>
        public ResponseBussiness<OperationResponse> CambiarPiezasArticuloLineaTicketDevolucion(DevolverArticuloRequest devolverArticuloRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.CambiarPiezasArticuloLineaTicketDevolucion(token.CodeStore, token.CodeBox, token.CodeEmployee, devolverArticuloRequest);
            });
        }

        /// <summary>
        /// Aplicación de un Descuento Directo
        /// </summary>
        /// <param name="lineaTicket">Objeto de peticion linea ticket de la venta</param>
        /// <returns>Resultado de la operación</returns>
        public ResponseBussiness<OperationResponse> AplicarDescuentoDirecto(LineaTicket lineaTicket)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.AplicarDescuentoDirecto(token.CodeStore, token.CodeBox, token.CodeEmployee, lineaTicket);
            });
        }

    }
}
