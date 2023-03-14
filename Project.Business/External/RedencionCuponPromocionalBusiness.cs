using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Dto.Sales;
using Milano.BackEnd.Repository;
using Milano.BackEnd.Repository.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Milano.BackEnd.Business.External
{

    /// <summary>
    /// 
    /// </summary>
    public class RedencionCuponPromocionalBusiness : BaseBusiness
    {

        InfoService infoService;
        TokenDto token;
        InformacionServiciosExternosRepository externosRepository;
        CuponesRedimirRepository repository;
        ProxyCupones.wsCuponesSoapClient proxyCupones;

        /// <summary>
        /// Constructor por Default
        /// </summary>
        /// <param name="tokenDto"></param>
        public RedencionCuponPromocionalBusiness(TokenDto tokenDto)
        {
            token = tokenDto;
            repository = new CuponesRedimirRepository();
            externosRepository = new InformacionServiciosExternosRepository();
            infoService = externosRepository.ObtenerInfoServicioExterno(23);
            proxyCupones = new ProxyCupones.wsCuponesSoapClient();
            proxyCupones.Endpoint.Address = new System.ServiceModel.EndpointAddress(infoService.UrlService);
        }

        /// <summary>
        /// Realizar Venta de Cupón Promocional
        /// </summary>
        /// <param name="codigoCajero">Código del cajero</param>
        /// <param name="codigoTipoTrxCab">Código del tipo de transacción</param>
        /// <param name="folioVenta">Folio de la operacióna</param>
        /// <param name="folioCupon">Folio del cupón</param>
        /// <param name="transaccion">Transacción del sistema</param>
        /// <param name="montoPagado">Monto pagado</param>
        /// <returns></returns>
        public OperationResponse RealizarVentaCuponPromocional(int codigoCajero, string folioVenta, string folioCupon, int transaccion, decimal montoPagado, string codigoTipoTrxCab)
        {
            OperationResponse operation = new OperationResponse();
            ProxyCupones.Respuesta respuesta = new ProxyCupones.Respuesta();
            try
            {
                respuesta = proxyCupones.RealizarVenta(this.token.CodeStore, this.token.CodeBox, Int32.Parse(codigoTipoTrxCab), folioVenta, codigoCajero, folioCupon, transaccion, montoPagado);
                if (respuesta.sError == "")
                {
                    CuponPersistirRequest cuponPersistirRequest = new CuponPersistirRequest();
                    cuponPersistirRequest.CodigoCaja = this.token.CodeBox;
                    cuponPersistirRequest.CodigoTienda = this.token.CodeStore;
                    cuponPersistirRequest.CodigoEmpleado = this.token.CodeEmployee;
                    cuponPersistirRequest.FolioCupon = folioCupon;
                    cuponPersistirRequest.FolioVenta = folioVenta;
                    cuponPersistirRequest.Transaccion = transaccion;
                    cuponPersistirRequest.MaximoRedencion = montoPagado;
                    cuponPersistirRequest.Sesion = Convert.ToInt64(respuesta.lSesion);

                    // OCG: Se comento por que las consultas y redención de cupones son web
                    //repository.PersistirCupon(cuponPersistirRequest);
                    
                    operation.CodeNumber = respuesta.sEstatus;
                    operation.CodeDescription = respuesta.sMensaje;
                }
                else
                {
                    operation.CodeNumber = "0";
                    operation.CodeDescription = respuesta.sError;
                }
            }
            catch (Exception ex)
            {
                operation.CodeNumber = "0";
                operation.CodeDescription = ex.Message;
            }
            return operation;
        }

    }
}
