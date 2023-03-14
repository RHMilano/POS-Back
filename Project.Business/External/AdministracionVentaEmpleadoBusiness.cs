using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Repository;
using Project.Services.Utils;
using System;

namespace Milano.BackEnd.Business
{
    /// <summary>
    /// Clase para la venta de empleados
    /// </summary>
    public class AdministracionVentaEmpleadoBusiness : BaseBusiness
    {

        ProxyVentaEmpleado.wsVentaEmpleadoSoapClient proxy;
        //ProxyInfoDescuento3.InfoDescuentoSoapClient proxy3;
        AdministracionVentaEmpleadoRepository repositorio;
        InformacionServiciosExternosRepository externosRepository;
        InfoService inforService;

        /// <summary>
        /// Constructor por default
        /// </summary>
        public AdministracionVentaEmpleadoBusiness()
        {
            inforService = new InfoService();
            externosRepository = new InformacionServiciosExternosRepository();
            inforService = externosRepository.ObtenerInfoServicioExterno(15);
            proxy = new ProxyVentaEmpleado.wsVentaEmpleadoSoapClient();
            proxy.Endpoint.Address = new System.ServiceModel.EndpointAddress(inforService.UrlService);
            this.repositorio = new AdministracionVentaEmpleadoRepository();
        }

        /// <summary>
        /// Metodo para buscar el monto de descuento de un empleado
        /// </summary>
        /// <param name="codigoEmpleado">codigo del empleado</param>
        /// <param name="codigoTienda">codigo de tienda</param>
        /// <param name="codigoCaja">codigo de la caja</param>
        /// <returns>Datos del empleado y su descuento</returns>
        public ResponseBussiness<EmpleadoMilanoResponse> Buscar(string codigoEmpleado, string codigoTienda, string codigoCaja)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return this.ObtenerEmpleado(codigoEmpleado, codigoTienda, codigoCaja);
            });
        }

        /// <summary>
        /// Registar pago de venta empleado
        /// </summary>
        /// <param name="venta">Dto con la información de venta a empleado</param>
        /// <param name="codigoTransaccion"><Código de transaccion/param>
        /// <returns>Resultado de la operación</returns>
        public OperationResponse RealizarVenta(ProcesarMovimientoVentaEmpleadoRequest venta, int codigoTransaccion, int codigoTienda, int codigoCaja)
        {
            DateTime fecha = new SalesRepository().ObtenerFecha(venta.FolioOperacionAsociada);
            int codigoEmpresa = repositorio.ObtenerCompania();
            OperationResponse operationResponse = new OperationResponse();
            try
            {
                ProxyVentaEmpleado.InfoVenta resultInfoVenta = proxy.RealizarVenta(codigoEmpresa, venta.CodigoEmpleado.ToString(), codigoTienda, codigoCaja, double.Parse(venta.ImporteVentaTotal.ToString()), venta.MontoFinanciado, venta.FolioOperacionAsociada, codigoTransaccion, fecha);
                if (resultInfoVenta.sError == "")
                {
                    operationResponse.CodeDescription = resultInfoVenta.sMensaje;
                    operationResponse.CodeNumber = "1";
                }
                else
                {
                    operationResponse.CodeDescription = resultInfoVenta.sError;
                    operationResponse.CodeNumber = "0";
                }
            }
            catch (Exception ex)
            {
                operationResponse.CodeDescription = ex.Message;
                operationResponse.CodeNumber = "0";
            }
            return operationResponse;
        }

        private EmpleadoMilanoResponse ObtenerEmpleado(string codigoEmpleado, string codigoTienda, string codigoCaja)
        {
            EmpleadoMilanoResponse empleado = new EmpleadoMilanoResponse();
            Inspector inspector = new Inspector();
            int compania = repositorio.ObtenerCompania();
            ProxyVentaEmpleado.InfoEmpleado info = proxy.ConsultarEmpleadoTCMM(compania, codigoEmpleado, int.Parse(codigoTienda), int.Parse(codigoCaja));
            
            //ProxyInfoDescuento3.WsPosResponseModel prox = proxy3.getAutorization(6011,)

            if (info.sError == "")
            {
                empleado.Codigo = info.iNumeroNomina;//int.Parse(codigoEmpleado);
                empleado.Nombre = info.sNombre;
                empleado.ApellidoPaterno = info.sPaterno;
                empleado.ApellidoMaterno = info.sMaterno;
                empleado.MontoCredito = inspector.TruncarValor(info.dCredito);
                empleado.Mensaje = info.sMensaje;
            } 
            else
            {
                empleado.Mensaje = info.sError;
            }

            return empleado;
        }

        /// <summary>
        /// Valida si se permite la venta a empleado
        /// </summary>
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> ValidarVentaEmpleado()
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repositorio.ValidarVentaEmpleado();
            });
        }

    }
}
