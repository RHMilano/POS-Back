using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Milano.BackEnd.Business.ImpresionMM;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.Apartados;
using Milano.BackEnd.Dto.Sales;
using Milano.BackEnd.Repository;
namespace Milano.BackEnd.Business
{

    /// <summary>
    /// Clase de negocio de apartados
    /// </summary>
    public class ApartadosBusiness : BaseBusiness
    {
        ApartadoRepository repository;
        DescuentosPromocionesRepository descuentosPromocionesRepository;
        TokenDto token;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApartadosBusiness(TokenDto token)
        {
            repository = new ApartadoRepository();
            this.token = token;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ApartadosBusiness()
        {
            repository = new ApartadoRepository();
            descuentosPromocionesRepository = new DescuentosPromocionesRepository();
        }

        /// <summary>
        /// Cancelar apartado
        /// </summary>
        /// <param name="folioApartado"></param>
        /// <returns>Respuesta</returns>
        public ResponseBussiness<OperationResponse> CancelarApartado(string folioApartado)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return tryCatch.SafeExecutor(() =>
                {
                    TransApartadoResponse transApartadoResponse = repository.CancelarApartado(folioApartado, this.token.CodeBox, this.token.CodeStore, this.token.CodeEmployee);
                    OperationResponse operationResponse = new OperationResponse();
                    // TODO: Invocar servicio de Lealtad
                    // Imprimir Cancelación de Apartado y Nota de Crédito en caso de Aplicar
                    ImprimeTicketsMM imprimeTicketsMM = new ImprimeTicketsMM(token);
                    imprimeTicketsMM.PrintTicket(transApartadoResponse.FolioVenta, false);
                    PrintTicketEmisionNotaCredito printTicketEmisionNotaCredito = new PrintTicketEmisionNotaCredito(token);
                    OperationResponse operation = printTicketEmisionNotaCredito.PrintNow(transApartadoResponse.FolioNotaCreditoGenerada);
                    // Regresar el resultado
                    operationResponse.CodeDescription = transApartadoResponse.CodeDescription;
                    operationResponse.CodeNumber = transApartadoResponse.CodeNumber;
                    return operationResponse;
                });
            });
        }

        /// <summary>
        /// Abonar a un apartado
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> Abonar(AbonoApartadoRequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                TransApartadoResponse response = new TransApartadoResponse();
                OperationResponse operationResponse = new OperationResponse();
                //OperationResponse operationResponseSW = new OperationResponse();
                //AdministracionTipoCambio administracionTipoCambio = new AdministracionTipoCambio();

                using (TransactionScope scope = new TransactionScope())
                {
                    response = new ApartadoAbonoRepository().Abonar(this.token.CodeStore, this.token.CodeBox, this.token.CodeEmployee, request, "APARTADO");
                    //operationResponseSW = administracionTipoCambio.GetSaleExternalService(request.FormasPagoUtilizadas, request.FolioApartado, this.token.CodeEmployee);
                    if (((response.CodeNumber == "349") || (response.CodeNumber == "350"))) //&& operationResponseSW.CodeNumber == "000")
                    {
                        scope.Complete();
                    }
                    //else
                    //{
                    //    operationResponse.CodeDescription = operationResponseSW.CodeDescription;
                    //    operationResponse.CodeNumber = operationResponseSW.CodeNumber;
                    //}
                }

                ImprimeTicketsMM imprimeTicketsMM = new ImprimeTicketsMM(token);
                imprimeTicketsMM.PrintTicket(response.FolioVenta, false);
                operationResponse.CodeDescription = response.CodeDescription;
                operationResponse.CodeNumber = response.CodeNumber;
                operationResponse.informacionAsociadaRetiroEfectivo = response.informacionAsociadaRetiroEfectivo;
                return operationResponse;
            });
        }

        /// <summary>
        /// Buscar apartado
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseBussiness<ApartadoResponse> BuscarApartado(ApartadoBusquedaRequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.BuscarPorFolio(request, this.token.CodeStore, this.token.CodeBox);
            });
        }

        /// <summary>
        /// Cambio de Precio de una Linea Ticket
        /// </summary>
        /// <param name="cambiarPrecioRequest">Objeto de peticion linea ticket de apartado</param>
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> CambiarPrecioLineaTicketVenta(CambiarPrecioRequest cambiarPrecioRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.CambiarPrecioLineaTicketVenta(token.CodeStore, token.CodeBox, token.CodeEmployee, cambiarPrecioRequest);
            });
        }

        /// <summary>
        /// Cambio de Piezas de una Linea Ticket
        /// </summary>
        /// <param name="lineaTicket">Objeto de peticion linea ticket de apartado</param>
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> CambiarPiezasLineaTicketVenta(LineaTicket lineaTicket)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.CambiarPiezasLineaTicketVenta(token.CodeStore, token.CodeBox, token.CodeEmployee, lineaTicket);
            });
        }

        /// <summary>
        /// Eliminación de una Linea Ticket
        /// </summary>
        /// <param name="secuenciaOriginalLineaTicket">Secuencia original de la línea eliminada</param>  
        /// <param name="lineaTicket">Objeto de peticion linea ticket del apartado</param>
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> EliminarLineaTicketVenta(int secuenciaOriginalLineaTicket, LineaTicket lineaTicket)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.EliminarLineaTicketApartado(token.CodeStore, token.CodeBox, token.CodeEmployee, secuenciaOriginalLineaTicket, lineaTicket);
            });
        }

        /// <summary>
        /// Almacenamiento de una Linea Ticket
        /// </summary>
        /// <param name="lineaTicket">Objeto de peticion linea ticket de la venta</param>
        /// <returns></returns>
        public ResponseBussiness<OperacionLineaTicketVentaResponse> AgregarLineaTicketVenta(LineaTicket lineaTicket)
        {
            return tryCatch.SafeExecutor(() =>
            {
                OperacionLineaTicketVentaResponse response = repository.AgregarLineaTicketVenta(token.CodeStore, token.CodeBox, token.CodeEmployee, lineaTicket);
                return response;
            });
        }


        /// <summary>
        /// Totalización de apartado
        /// </summary>
        /// <param name="request">Objeto de peticion del apartado a totalizar</param>
        /// <returns></returns>
        public ResponseBussiness<TotalizarApartadoResponse> TotalizarApartado(TotalizarApartadoRequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                TotalizarApartadoResponse response = repository.TotalizarApartado(request, token.CodeStore, token.CodeBox, token.CodeEmployee);
                return response;
            });
        }

        /// <summary>
        /// Anular el apartado despues de totalizar
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> AnularTotalizarApartado(AnularTotalizarApartadoRequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {

                TransApartadoResponse transApartadoResponse = repository.AnularApartado(request, this.token);
                OperationResponse operationResponse = new OperationResponse();

                operationResponse.CodeNumber = transApartadoResponse.CodeNumber;
                operationResponse.CodeDescription = transApartadoResponse.CodeDescription;

                ImprimeTicketsMM imprimeTicketsMM = new ImprimeTicketsMM(token);
                imprimeTicketsMM.PrintTicket(transApartadoResponse.FolioVenta, false);

                return operationResponse;
            });
        }

        /// <summary>
        /// Finalizar Apartado
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> FinalizarApartado(FinalizarApartadoRequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                TransApartadoResponse response = FinalizarApartadoInternal(request);
                OperationResponse operationResponse = new OperationResponse();

                ImprimeTicketsMM imprimeTicketsMM = new ImprimeTicketsMM(token);
                imprimeTicketsMM.PrintTicket(response.FolioVenta, false);

                operationResponse.CodeNumber = response.CodeNumber;
                operationResponse.CodeDescription = response.CodeDescription;
                operationResponse.informacionAsociadaRetiroEfectivo = response.informacionAsociadaRetiroEfectivo;
                return operationResponse;
            });
        }

        private TransApartadoResponse FinalizarApartadoInternal(FinalizarApartadoRequest request)
        {
            TransApartadoResponse result = new TransApartadoResponse();
            AdministracionTipoCambio administracionTipo = new AdministracionTipoCambio();
            //Obtenemos el folio apartado y el numero de empleado para usarlos en el metodo externo sale
            string folioVenta = request.FolioApartado;
            int codigoEmpleado = token.CodeEmployee;
            using (TransactionScope scope = new TransactionScope())
            {
                result = repository.FinalizarApartado(token.CodeStore, token.CodeBox, this.token.CodeEmployee, request);

                //Invocamos el servicio para obtener la respuesta del servicio de sale, si el numero de codigo es el indicado completa la transaccion
                //en caso de que el codigo no sea el indicado enviara el codigo de error y la descripcion
                //var resultSale = administracionTipo.GetSaleExternalService(request.FormasPagoUtilizadas, folioVenta, codigoEmpleado);
                if (result.CodeNumber.Equals("342")) //&& resultSale.CodeNumber.Equals("000"))
                {
                    var respuesta = this.ActivarTarjetaRegalo(request);
                    if (respuesta.CodeNumber == "1")
                    {
                        scope.Complete();
                    }
                }
            }
            return result;
        }

        private OperationResponse ActivarTarjetaRegalo(FinalizarApartadoRequest request)
        {
            OperationResponse response = new OperationResponse();
            response.CodeNumber = "1";
            TarjetaRegalosBusiness business = new TarjetaRegalosBusiness(this.token);
            foreach (InformacionFoliosTarjeta informacion in request.InformacionFoliosTarjeta)
            {
                var respuesta = business.ActivarTarjeta(this.token.CodeEmployee, informacion.FolioTarjeta.ToString(), request.FolioApartado);
                if (response.CodeNumber == "0")
                {
                    response.CodeDescription = respuesta.Data.CodeDescription;
                }
            }
            return response;
        }

        /// <summary>
        /// Obtener dias de vencimiento
        /// </summary>
        /// <returns></returns>
        public ResponseBussiness<ApartadoPlazosResponse[]> ObtenerDiasVencimiento()
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.ObtenerPlazos();
            });
        }

        /// <summary>
        /// Proceso para ceder apartados
        /// </summary>
        public ResponseBussiness<int> CederApartados()
        {
            return tryCatch.SafeExecutor(() =>
            {
                return this.repository.CederApartados();
            });

        }

    }
}
