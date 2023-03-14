using Milano.BackEnd.Business.General;
using Milano.BackEnd.Business.ImpresionMM;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Dto.InicioFinDia;
using Milano.BackEnd.Repository.InicioFinDia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Milano.BackEnd.Business.InicioFinDia
{
    /// <summary>
    /// 
    /// </summary>
    public class InicioFinDiaBusiness : BaseBusiness
    {
        InicioFinDiaRepository inicioFinDiaRepository;

        /// <summary>
        /// Metodo constructor que crea una instancia de su repositorio
        /// </summary>
        public InicioFinDiaBusiness()
        {
            inicioFinDiaRepository = new InicioFinDiaRepository();
        }
        /// <summary>
        /// Metodo para realizar el inicio de día
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public InicioDiaResponse RealizarInicioDia(TokenDto token)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return RealizarInicioDiaInternal(token);
            });
        }

        private InicioDiaResponse RealizarInicioDiaInternal(TokenDto token)
        {
            InicioDiaResponse inicioDiaResponse = new InicioDiaResponse();
            inicioDiaResponse = inicioFinDiaRepository.InicioDia(token);
            return inicioDiaResponse;
        }

        /// <summary>
        /// Metodo para realizar el fin de día
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public ResponseBussiness<FinDiaResponse> RealizarFinDia(TokenDto token)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return inicioFinDiaRepository.FinDia(token);
            });
        }

        /// <summary>
        /// Metodo para obtener el cashout
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public ResponseBussiness<CompendioCashOut> ObtenerCashOut(TokenDto token)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return inicioFinDiaRepository.ObtenerCashOut(token);
            });
        }

        /// <summary>
        /// Metodo para actualizar el CashOut al registrar el fin de día
        /// </summary>
        /// <param name="token">Token</param>
        /// <param name="compendioCashOut">Objeto que contiene el compendio de CashOut</param>
        /// <returns></returns>
        public ResponseBussiness<RelacionCaja> PersistirCashOut(TokenDto token, CompendioCashOut compendioCashOut)
        {
            return tryCatch.SafeExecutor(() =>
            {
                RelacionCaja relacionCaja = new RelacionCaja();
                using (TransactionScope scope = new TransactionScope())
                {
                    // Persistir el Cash Out                    
                    int idCompendioCashOut = inicioFinDiaRepository.AgregarCompendioCashOut(compendioCashOut.CodigoTienda);
                    foreach (var compendio in compendioCashOut.CashOutCajas)
                    {
                        int idCashOut = inicioFinDiaRepository.AgregarCashOut(idCompendioCashOut, compendio.CodigoCaja);
                        foreach (var lecturaZ in compendio.LecturasZ)
                        {
                            foreach (var detalleLecturaZ in lecturaZ.DetallesLecturaFormaPago)
                            {

                                //OCG: Comentado por que no se debe de hacer ninguna diferencia cuando se paga con dolares
                                // comenta Carlos Guzmán que probablemente Vanti dejo la validación de forma inicial y con
                                // los cambios de reglas se quedo.

                                //if (detalleLecturaZ.InformacionAsociadaFormasPago.CodigoFormaPago == "US")
                                //{
                                //    //Se crea administracion tipo cambio para poder obtener el tipo de cambio
                                //    AdministracionTipoCambio tipoCambio = new AdministracionTipoCambio();

                                //    //Se crea la peticion tipo cambio para crear el objeto con el que se obtendra el cambio de moneda
                                //    TipoCambioRequest tipoCambioRequest = new TipoCambioRequest();

                                //    //Se crea el response que nos retorna la conversion de la moneda extranjera.
                                //    TipoCambioResponse tipoCambioResponse = new TipoCambioResponse();
                                //    decimal importeConversionDolar = 0.0M;
                                //    tipoCambioRequest.CodigoTipoDivisa = detalleLecturaZ.InformacionAsociadaFormasPago.CodigoFormaPago;
                                //    tipoCambioRequest.ImporteMonedaNacional = detalleLecturaZ.ImporteFisico;
                                //    tipoCambioResponse = tipoCambio.GetTipoCambio(tipoCambioRequest);

                                //    //Convertimos la cantidad de dolares a pesos.
                                //    importeConversionDolar = ConvertirDolaresPesos(tipoCambioResponse.TasaConversionVigente, detalleLecturaZ.ImporteFisico);

                                //    //En el importe teorico añadiremos el importe de moneda nacional
                                //    inicioFinDiaRepository.AgregarLecturaZCashOut(idCashOut, importeConversionDolar, detalleLecturaZ.ImporteTeorico, detalleLecturaZ.ImporteFisico,
                                //    detalleLecturaZ.TotalRetirosParciales, detalleLecturaZ.FondoFijoActual, detalleLecturaZ.InformacionAsociadaFormasPago.IdentificadorFormaPago,
                                //    detalleLecturaZ.InformacionAsociadaFormasPago.CodigoFormaPago, detalleLecturaZ.InformacionAsociadaFormasPago.DescripcionFormaPago);
                                //}
                                //else

                                //{
                                    inicioFinDiaRepository.AgregarLecturaZCashOut(idCashOut, detalleLecturaZ.ImporteFisico, detalleLecturaZ.ImporteTeorico, detalleLecturaZ.ImporteFisico,
                                    detalleLecturaZ.TotalRetirosParciales, detalleLecturaZ.FondoFijoActual, detalleLecturaZ.InformacionAsociadaFormasPago.IdentificadorFormaPago,
                                    detalleLecturaZ.InformacionAsociadaFormasPago.CodigoFormaPago, detalleLecturaZ.InformacionAsociadaFormasPago.DescripcionFormaPago);
                                //}
                            }
                        }
                    }
                    relacionCaja = inicioFinDiaRepository.ObtenerRelacionCaja(token);
                    // Terminar la transacción
                    scope.Complete();
                }
                return relacionCaja;
            });
        }

        private decimal ConvertirDolaresPesos(decimal tasaConversionVigente, decimal importeTeorico)
        {
            return importeTeorico * tasaConversionVigente;
        }

        /// <summary>
        /// Metodo para actualizar la relacion de caja al registrar el fin de día
        /// </summary>
        /// <param name="token">Token</param>
        /// <param name="relacionCaja">Objeto que contiene la Relación de Caja</param>
        /// <returns></returns>
        public ResponseBussiness<ValidacionOperacionResponse> PersistirRelacionCajaFinDia(TokenDto token, RelacionCaja relacionCaja)
        {
            return tryCatch.SafeExecutor(() =>
            {
                ValidacionOperacionResponse validacionOperacionResponse = new ValidacionOperacionResponse();
                int idRelacionCaja = -1;
                using (TransactionScope scope = new TransactionScope())
                {
                    // Persistir relacion de caja
                    idRelacionCaja = inicioFinDiaRepository.AgregarRelacionCaja(relacionCaja.CodigoTienda, relacionCaja.TotalConIVA, relacionCaja.TotalSinIVA, relacionCaja.IVA);
                    // Persistir depositos
                    foreach (var deposito in relacionCaja.DepositosAsociados)
                    {
                        inicioFinDiaRepository.AgregarDepositoRelacionCaja(idRelacionCaja, deposito.TotalConIVA,
                            deposito.InformacionAsociadaFormasPago.CodigoFormaPago, deposito.InformacionAsociadaFormasPago.DescripcionFormaPago);
                    }
                    // Persistir grupos
                    foreach (var grupo in relacionCaja.GruposRelacionCaja)
                    {
                        int idGrupo = inicioFinDiaRepository.AgregarGrupoRelacionCaja(idRelacionCaja, grupo.TotalConIVA, grupo.Encabezado);
                        // Persistir secciones
                        foreach (var seccion in grupo.SeccionesRelacionCaja)
                        {
                            int idSeccion = inicioFinDiaRepository.AgregarSeccionRelacionCaja(idGrupo, seccion.TotalConIVA, seccion.Encabezado, seccion.TotalSinIVA, seccion.IVA);
                            foreach (var desglose in seccion.DesgloseRelacionCaja)
                            {
                                inicioFinDiaRepository.AgregarDesgloseSeccionRelacionCaja(idSeccion, desglose.TotalConIVA, desglose.Descripcion);
                            }
                        }
                    }
                    // Registrar Fin de Día exitosamente
                    validacionOperacionResponse = inicioFinDiaRepository.RegistrarFinDia(token);
                    // Terminar la transacción
                    scope.Complete();
                }

                // Imprimir el Reporte de la Relación de Caja
                PrintRelacionCaja printRelacionCaja = new PrintRelacionCaja(token);
                printRelacionCaja.printReporte(idRelacionCaja);

                return validacionOperacionResponse;
            });
        }

        /// <summary>
        /// Metodo que ejecuta la sincronización
        /// </summary>
        /// <param name="informacionCajaRequest"></param>
        /// <returns></returns>
        public ResponseBussiness<ValidacionOperacionResponse> EjecutarSincronizacion(InformacionCajaRequest informacionCajaRequest)
        {
            // TODO: Implementación
            ValidacionOperacionResponse validacionOperacionResponse = new ValidacionOperacionResponse();
            validacionOperacionResponse = inicioFinDiaRepository.EjecutarSincronizacion(informacionCajaRequest);
            return validacionOperacionResponse;
        }

        /// <summary>
        /// Metodo que genera Lectura Z y Logout Offline
        /// </summary>
        /// <param name="token"></param>
        /// <param name="lecturaCaja"></param>
        /// <returns></returns>
        public ResponseBussiness<ValidacionOperacionResponse> GenerarLecturaZOffline(TokenDto token, LecturaCaja lecturaCaja)
        {
            return tryCatch.SafeExecutor(() =>
            {
                ValidacionOperacionResponse validacionOperacionResponse = new ValidacionOperacionResponse();
                using (TransactionScope scope = new TransactionScope())
                {
                    token.CodeBox = lecturaCaja.CodigoCaja;
                    LecturaZGuardarResponse lecturaZGuardarResponse = new LecturaBusiness(token).LecturaZOffline(lecturaCaja);
                    inicioFinDiaRepository.RegistrarLecturaZOffline(token.CodeStore, token.CodeBox, lecturaZGuardarResponse.FolioCorte);
                    validacionOperacionResponse = inicioFinDiaRepository.RegistrarLogoutCaja(token);
                    // Asignamos códigos de Lectura Z
                    validacionOperacionResponse.CodeNumber = lecturaZGuardarResponse.CodeNumber;
                    validacionOperacionResponse.CodeDescription = lecturaZGuardarResponse.CodeDescription;
                    token.CodeBox = 0;
                    scope.Complete();
                }
                return validacionOperacionResponse;
            });
        }

        /// <summary>
        /// Business para validar operación de Corte Z Offline
        /// </summary>
        public ResponseBussiness<ValidacionOperacionResponse> ValidarCorteOffline(TokenDto token, AutenticacionOfflineRequest autenticacionOfflineRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return inicioFinDiaRepository.ValidarCorteOffline(token, autenticacionOfflineRequest);
            });
        }
    }
}
