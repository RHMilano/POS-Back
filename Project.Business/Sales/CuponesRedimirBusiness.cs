using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Dto.Sales;
using Milano.BackEnd.Repository;
using Milano.BackEnd.Repository.Sales;
using System;

namespace Milano.BackEnd.Business.Sales
{
    /// <summary>
    /// Redime el cupón 
    /// </summary>
    public class CuponesRedimirBusiness : BaseBusiness
    {

        TokenDto token;
        InfoService infoService;
        InformacionServiciosExternosRepository externosRepository;
        CuponesRedimirRepository repository;
        ProxyCupones.wsCuponesSoapClient proxyCupones;

        /// <summary>
        /// Constructor por Default
        /// </summary>
        /// <param name="tokenDto"></param>
        public CuponesRedimirBusiness(TokenDto tokenDto)
        {
            this.token = tokenDto;
            repository = new CuponesRedimirRepository();
            externosRepository = new InformacionServiciosExternosRepository();
            infoService = externosRepository.ObtenerInfoServicioExterno(23);
            proxyCupones = new ProxyCupones.wsCuponesSoapClient();
            proxyCupones.Endpoint.Address = new System.ServiceModel.EndpointAddress(infoService.UrlService);
        }

        /// <summary>
        /// Regresa el saldo maximo por el cual puede ser redimido el cupon
        /// </summary>
        /// <returns></returns>
        public CuponRedimirResponse SaldoRedimir(ValidarCuponRequest validarCuponRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                CuponRedimirRequest cuponRedimirRequest = new CuponRedimirRequest();
                CuponRedimirResponse cuponRedimirResponse = new CuponRedimirResponse();
                ProxyCupones.Respuesta consultaCuponResponseWs = new ProxyCupones.Respuesta();
 
                cuponRedimirRequest.FolioCupon = validarCuponRequest.FolioCupon;
                cuponRedimirRequest.FolioVenta = validarCuponRequest.FolioVenta;
                cuponRedimirRequest.CodigoCaja = this.token.CodeBox;
                cuponRedimirRequest.CodigoTienda = this.token.CodeStore;

                consultaCuponResponseWs = proxyCupones.ConsultaCupon(this.token.CodeStore, this.token.CodeBox, validarCuponRequest.FolioCupon);

                try
                {
                    //Si existe un error
                    if (consultaCuponResponseWs.sError != "")
                    {
                        cuponRedimirRequest.Sesion = Convert.ToInt32(consultaCuponResponseWs.lSesion);
                        // No encuentra el cupon en el WS de milano, busca localmente
                        if (consultaCuponResponseWs.sError == "El folio de cupón no existe; Verifique")
                        {
                            cuponRedimirRequest.SaldoCupon = 0;
                            cuponRedimirRequest.CodigoPromocion = 0;
                            cuponRedimirResponse = repository.ValidarSaldo(cuponRedimirRequest);
                        }
                        else
                        {
                            cuponRedimirResponse.MensajeRedencion = consultaCuponResponseWs.sError;
                            cuponRedimirResponse.Saldo = 0;
                        }
                    }
                    else // No hay errores
                    {
                        cuponRedimirRequest.SaldoCupon = Convert.ToDouble(consultaCuponResponseWs.dSaldo);
                        cuponRedimirRequest.CodigoPromocion = consultaCuponResponseWs.iCodigoPromocion;
                        cuponRedimirRequest.Sesion = Convert.ToInt32(consultaCuponResponseWs.lSesion);
                        cuponRedimirResponse = repository.ValidarSaldo(cuponRedimirRequest);
                    }

                    //// Verificar cupon primero localmente
                    //cuponRedimirResponse = repository.ValidarCuponLocal(validarCuponRequest.FolioCupon, this.token.CodeStore, this.token.CodeBox);

                    //if (cuponRedimirResponse.EsRedimibleHoy == 1)
                    //{
                    //    cuponRedimirRequest.SaldoCupon = Convert.ToDouble(cuponRedimirResponse.Saldo);
                    //    cuponRedimirRequest.CodigoPromocion = (cuponRedimirResponse.CodigoPromocion ?? 0);
                    //    cuponRedimirRequest.Sesion = cuponRedimirResponse.Transaccion;
                    //    cuponRedimirResponse = repository.ValidarSaldo(cuponRedimirRequest);
                    //}
                    //else
                    //{
                    //    consultaCuponResponseWs = proxyCupones.ConsultaCupon(this.token.CodeStore, this.token.CodeBox, validarCuponRequest.FolioCupon);

                    //    if (consultaCuponResponseWs.sError != "")
                    //    {
                    //        cuponRedimirRequest.Sesion = Convert.ToInt32(consultaCuponResponseWs.lSesion);
                    //        // No encuentra el cupon en el WS de milano, busca localmente
                    //        if (consultaCuponResponseWs.sError == "El folio de cupón no existe; Verifique")
                    //        {
                    //            cuponRedimirRequest.SaldoCupon = 0;
                    //            cuponRedimirRequest.CodigoPromocion = 0;
                    //            cuponRedimirResponse = repository.ValidarSaldo(cuponRedimirRequest);
                    //        }
                    //        else
                    //        {
                    //            cuponRedimirResponse.MensajeRedencion = consultaCuponResponseWs.sError;
                    //            cuponRedimirResponse.Saldo = 0;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        cuponRedimirRequest.SaldoCupon = Convert.ToDouble(consultaCuponResponseWs.dSaldo);
                    //        cuponRedimirRequest.CodigoPromocion = consultaCuponResponseWs.iCodigoPromocion;
                    //        cuponRedimirRequest.Sesion = Convert.ToInt32(consultaCuponResponseWs.lSesion);
                    //        cuponRedimirResponse = repository.ValidarSaldo(cuponRedimirRequest);
                    //    }
                    //}
                }
                catch (Exception exception)
                {
                    TryCatchBusinessExecutor tryCatch = new TryCatchBusinessExecutor();
                    tryCatch.AddErrorLog<OperationResponse>(exception.Message, exception.StackTrace, "Servicio milano", exception.ToString(), "Error de comunicacion servicios milano");
                    cuponRedimirRequest.SaldoCupon = 0;
                    cuponRedimirRequest.CodigoPromocion = 0;
                    cuponRedimirRequest.Sesion = 0;
                    cuponRedimirResponse = repository.ValidarSaldo(cuponRedimirRequest);
                }
                return cuponRedimirResponse;
            });
        }

    }
}
