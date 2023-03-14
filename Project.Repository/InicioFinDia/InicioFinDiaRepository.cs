using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Dto.InicioFinDia;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Milano.BackEnd.Repository.InicioFinDia
{
    /// <summary>
    /// 
    /// </summary>
    public class InicioFinDiaRepository : BaseRepository
    {
        /// <summary>
        /// Método para validar si se require Autenticación Offline o no
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public InicioDiaResponse InicioDia(TokenDto token)
        {
            InicioDiaResponse inicioDiaResponse = new InicioDiaResponse();
            ValidacionOperacionResponse operacionResponse = new ValidacionOperacionResponse();
            var parametros = new Dictionary<string, object>();
            parametros.Add("@CodigoTienda", token.CodeStore);
            parametros.Add("@CodigoCaja", token.CodeBox);
            parametros.Add("@CodigoEmpleado", token.CodeEmployee);

            // Obtener la informacion de folio y fecha de operacion
            List<System.Data.SqlClient.SqlParameter> parametrosOutInfo = new List<System.Data.SqlClient.SqlParameter>();
            parametrosOutInfo.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@FechaOperacion", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Date });
            parametrosOutInfo.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@Folio", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            var resultadoInfo = data.ExecuteProcedure("[dbo].[sp_vanti_Obtener_Informacion_LoginOffline]", parametros, parametrosOutInfo);
            var fechaOperacion = Convert.ToDateTime(resultadoInfo["@FechaOperacion"]);
            var folio = Convert.ToInt32(resultadoInfo["@Folio"]);

            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", token.CodeStore);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@InicioDiaPermitido", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Bit });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@RequiereCapturaLuz", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Bit });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@RequiereAutenticacionOffline", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Bit });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeAsociado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 500 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_server_Obtener_Conexion_InicioDia]", parameters, parametersOut);
            inicioDiaResponse.InicioDiaPermitido = Convert.ToBoolean(result["@InicioDiaPermitido"]);
            inicioDiaResponse.RequiereCapturarLuz = Convert.ToBoolean(result["@RequiereCapturaLuz"]);
            inicioDiaResponse.RequiereAutenticacionOffline = Convert.ToBoolean(result["@RequiereAutenticacionOffline"]);
            inicioDiaResponse.MensajeAsociado = result["@MensajeAsociado"].ToString();
            inicioDiaResponse.FolioOperacion = folio;
            inicioDiaResponse.FechaOperacion = fechaOperacion.ToString("yyyy-MM-dd");
            return inicioDiaResponse;
        }


        /// <summary>
        /// OCG: Solo descomentar cuando se desea forzar el fin de día sin tomar en cuenta las validaciones de las
        /// cajas, es decir que todas las validaciones pasan.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public FinDiaResponse FinDia(TokenDto token)
        {
            FinDiaResponse finDiaResponse = new FinDiaResponse();
            finDiaResponse.FinDiaPermitido = true;
            finDiaResponse.MensajeAsociado = "Es posible realizar una operación Fin de Día";

            //// Obtener parámetro de captura de Luz
            var parametrosCL = new Dictionary<string, object>();

            parametrosCL.Add("@CodigoTienda", token.CodeStore);

            List<System.Data.SqlClient.SqlParameter> parametrosCLOut = new List<System.Data.SqlClient.SqlParameter>();

            parametrosCLOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@RequiereCapturaLuz", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Bit });

            var resultadoCL = data.ExecuteProcedure("[dbo].[sp_vanti_ObtenerConfiguracionCapturaLuz]", parametrosCL, parametrosCLOut);

            finDiaResponse.RequiereCapturarLuz = Convert.ToBoolean(resultadoCL["@RequiereCapturaLuz"]);

            //finDiaResponse.RequiereCapturarLuz = false;//OCG: valor por omisión para pruebas

            // Obtener información de las cajas
            List<ResultadoValidacionCaja> resultadoValidacionCajas = new List<ResultadoValidacionCaja>();

            var parametersCD = new Dictionary<string, object>();

            // Validar Fin de Día por cada Caja 
            foreach (var item in data.GetDataReader("[dbo].[sp_vanti_server_ObtenerCajasDestino]", parametersCD))
            {
                var parametrosCaja = new Dictionary<string, object>();

                parametrosCaja.Add("@ServidorBaseDatos", item.GetValue(0));
                parametrosCaja.Add("@CodigoTienda", token.CodeStore);
                parametrosCaja.Add("@CodigoEmpleado", token.CodeEmployee);
                parametrosCaja.Add("@CodigoCaja", Convert.ToInt16(item.GetValue(1)));

                List<System.Data.SqlClient.SqlParameter> parametrosCajaOut = new List<System.Data.SqlClient.SqlParameter>();

                parametrosCajaOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@LecturaZRealizada", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Bit });
                parametrosCajaOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@LogoutRealizado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Bit });
                parametrosCajaOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@SincronizacionTerminada", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Bit });
                parametrosCajaOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@FinDiaEsPermitido", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Bit });
                parametrosCajaOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@IdControlInicioFinDia", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
                parametrosCajaOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@RequiereCapturarLuzInicioDia", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Bit });

                var resultadoCaja = data.ExecuteProcedure("[dbo].[sp_vanti_server_ValidarFinDiaCaja]", parametrosCaja, parametrosCajaOut,true);

                //Verificamos si se realizo la captura de luz de inicio de dia
                finDiaResponse.RequiereCapturarLuzInicioDia = Convert.ToBoolean(resultadoCaja["@RequiereCapturarLuzInicioDia"]);

                ResultadoValidacionCaja resultadoValidacionCaja = new ResultadoValidacionCaja();

                resultadoValidacionCaja.CodigoCaja = Convert.ToInt16(item.GetValue(1));
                resultadoValidacionCaja.ProcesoValidolecturaZ = Convert.ToBoolean(resultadoCaja["@LecturaZRealizada"]);
                resultadoValidacionCaja.ProcesoValidoLogout = Convert.ToBoolean(resultadoCaja["@LogoutRealizado"]);
                resultadoValidacionCaja.ProcesoValidoSincronizacionDatos = Convert.ToBoolean(resultadoCaja["@SincronizacionTerminada"]);
                resultadoValidacionCaja.ProcesoValidoFinDia = Convert.ToBoolean(resultadoCaja["@FinDiaEsPermitido"]);
                resultadoValidacionCaja.IdControlInicioFinDia = Convert.ToInt16(resultadoCaja["@IdControlInicioFinDia"]);

                resultadoValidacionCajas.Add(resultadoValidacionCaja);
            }

            finDiaResponse.ResultadosValidacionesCajas = resultadoValidacionCajas.ToArray();

            // Validar si el Fin de Día en general es permitido  de acuerdo a los resultados de cada caja           
            foreach (var caja in finDiaResponse.ResultadosValidacionesCajas)
            {
                if (caja.IdControlInicioFinDia == 0)
                {
                    finDiaResponse.FinDiaPermitido = false;
                    finDiaResponse.MensajeAsociado = "No es posible realizar una operación Fin de Día. No existe un proceso de Inicio de Día pendiente.";
                    return finDiaResponse;
                }
                if (!caja.ProcesoValidoFinDia)
                {
                    finDiaResponse.FinDiaPermitido = false;
                    finDiaResponse.MensajeAsociado = "No es posible realizar una operación Fin de Día. Las Cajas no están listas.";
                    return finDiaResponse;
                }
            }

            return finDiaResponse;
        }


        //public FinDiaResponse FinDia(TokenDto token)
        //{
        //    FinDiaResponse finDiaResponse = new FinDiaResponse();
        //    finDiaResponse.FinDiaPermitido = true;
        //    finDiaResponse.MensajeAsociado = "Es posible realizar una operación Fin de Día";

        //    //Obtener parámetro de captura de Luz
        //    var parametrosCL = new Dictionary<string, object>();

        //    parametrosCL.Add("@CodigoTienda", token.CodeStore);

        //    List<System.Data.SqlClient.SqlParameter> parametrosCLOut = new List<System.Data.SqlClient.SqlParameter>();

        //    parametrosCLOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@RequiereCapturaLuz", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Bit });

        //    var resultadoCL = data.ExecuteProcedure("[dbo].[sp_vanti_ObtenerConfiguracionCapturaLuz]", parametrosCL, parametrosCLOut);

        //    finDiaResponse.RequiereCapturarLuz = Convert.ToBoolean(resultadoCL["@RequiereCapturaLuz"]);

        //    finDiaResponse.RequiereCapturarLuz = false;//OCG: valor por omisión para pruebas

        //    //Obtener información de las cajas
        //    List<ResultadoValidacionCaja> resultadoValidacionCajas = new List<ResultadoValidacionCaja>();

        //    var parametersCD = new Dictionary<string, object>();

        //    //Validar Fin de Día por cada Caja
        //    foreach (var item in data.GetDataReader("[dbo].[sp_vanti_server_ObtenerCajasDestino]", parametersCD))
        //    {
        //        var parametrosCaja = new Dictionary<string, object>();

        //        parametrosCaja.Add("@ServidorBaseDatos", item.GetValue(0));
        //        parametrosCaja.Add("@CodigoTienda", token.CodeStore);
        //        parametrosCaja.Add("@CodigoEmpleado", token.CodeEmployee);
        //        parametrosCaja.Add("@CodigoCaja", Convert.ToInt16(item.GetValue(1)));

        //        List<System.Data.SqlClient.SqlParameter> parametrosCajaOut = new List<System.Data.SqlClient.SqlParameter>();

        //        parametrosCajaOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@LecturaZRealizada", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Bit });
        //        parametrosCajaOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@LogoutRealizado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Bit });
        //        parametrosCajaOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@SincronizacionTerminada", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Bit });
        //        parametrosCajaOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@FinDiaEsPermitido", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Bit });
        //        parametrosCajaOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@IdControlInicioFinDia", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
        //        parametrosCajaOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@RequiereCapturarLuzInicioDia", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Bit });

        //        var resultadoCaja = data.ExecuteProcedure("[dbo].[sp_vanti_server_ValidarFinDiaCaja]", parametrosCaja, parametrosCajaOut);

        //        //Verificamos si se realizo la captura de luz de inicio de dia
        //        finDiaResponse.RequiereCapturarLuzInicioDia = Convert.ToBoolean(resultadoCaja["@RequiereCapturarLuzInicioDia"]);

        //        ResultadoValidacionCaja resultadoValidacionCaja = new ResultadoValidacionCaja();

        //        resultadoValidacionCaja.CodigoCaja = Convert.ToInt16(item.GetValue(1));
        //        resultadoValidacionCaja.ProcesoValidolecturaZ = Convert.ToBoolean(resultadoCaja["@LecturaZRealizada"]);
        //        resultadoValidacionCaja.ProcesoValidoLogout = Convert.ToBoolean(resultadoCaja["@LogoutRealizado"]);
        //        resultadoValidacionCaja.ProcesoValidoSincronizacionDatos = Convert.ToBoolean(resultadoCaja["@SincronizacionTerminada"]);
        //        resultadoValidacionCaja.ProcesoValidoFinDia = Convert.ToBoolean(resultadoCaja["@FinDiaEsPermitido"]);
        //        resultadoValidacionCaja.IdControlInicioFinDia = Convert.ToInt16(resultadoCaja["@IdControlInicioFinDia"]);
        //        resultadoValidacionCajas.Add(resultadoValidacionCaja);
        //    }

        //    finDiaResponse.ResultadosValidacionesCajas = resultadoValidacionCajas.ToArray();


        //    //Validar si el Fin de Día en general es permitido  de acuerdo a los resultados de cada caja
        //    foreach (var caja in finDiaResponse.ResultadosValidacionesCajas)
        //    {
        //        if (caja.IdControlInicioFinDia == 0)
        //        {
        //            finDiaResponse.FinDiaPermitido = false;
        //            finDiaResponse.MensajeAsociado = "No es posible realizar una operación Fin de Día. No existe un proceso de Inicio de Día pendiente.";
        //            return finDiaResponse;
        //        }
        //        if (!caja.ProcesoValidoFinDia)
        //        {
        //            finDiaResponse.FinDiaPermitido = false;
        //            finDiaResponse.MensajeAsociado = "No es posible realizar una operación Fin de Día. Las Cajas no están listas.";
        //            return finDiaResponse;
        //        }
        //    }

        //    return finDiaResponse;
        //}

        /// <summary>
        /// Metodo para obtener las monedas extranjeras
        /// </summary>
        /// <param name="codeStore"></param>
        /// <returns></returns>
        public List<ConfigGeneralesCajaTiendaFormaPago> ObtenerMonedasExtranjeras(int codeStore)
        {
            List<ConfigGeneralesCajaTiendaFormaPago> formaPagos = new List<ConfigGeneralesCajaTiendaFormaPago>();
            var parametros = new Dictionary<string, object>();
            parametros.Add("@CodigoTienda", codeStore);
            foreach (var item in data.GetDataReader("[dbo].[sp_vanti_ObtenerFormasPagoMonedaExtEfectivo]", parametros))
            {
                ConfigGeneralesCajaTiendaFormaPago monedaReturn = new ConfigGeneralesCajaTiendaFormaPago();
                monedaReturn.CodigoFormaPago = item.GetValue(0).ToString();
                monedaReturn.DescripcionFormaPago = item.GetValue(1).ToString();

                formaPagos.Add(monedaReturn);
            }

            return formaPagos;
        }

        /// <summary>
        /// Método para obtener una nueva Relación de Caja
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public RelacionCaja ObtenerRelacionCaja(TokenDto token)
        {
            RelacionCaja relacionCaja = new RelacionCaja();
            relacionCaja.CodigoTienda = token.CodeStore;
            List<GrupoRelacionCaja> gruposIncluidosRelacionCaja = new List<GrupoRelacionCaja>();
            relacionCaja.GruposRelacionCaja = gruposIncluidosRelacionCaja.ToArray();
            var grupos = new Dictionary<String, GrupoRelacionCaja>();
            var secciones = new Dictionary<String, SeccionRelacionCaja>();
            List<GrupoRelacionCaja> gruposRelacionCaja = new List<GrupoRelacionCaja>();
            List<SeccionRelacionCaja> seccionesRelacionCaja = new List<SeccionRelacionCaja>();
            decimal iva = this.ObtenerImpuestoTienda(token.CodeStore);
            var parametros = new Dictionary<string, object>();
            parametros.Add("@CodigoTienda", token.CodeStore);
            parametros.Add("@CodigoCaja", token.CodeBox);
            foreach (var item in data.GetDataReader("[dbo].[sp_vanti_server_ObtenerRelacionDeCaja]", parametros, true))
            {
                // Llenado de Desglose
                String encabezadoGrupo = item.GetValue(0).ToString();
                String encabezadoSeccion = item.GetValue(1).ToString();
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
            // Asignar Totales                        
            foreach (var grupo in relacionCaja.GruposRelacionCaja)
            {
                foreach (var seccion in grupo.SeccionesRelacionCaja)
                {
                    foreach (var desglose in seccion.DesgloseRelacionCaja)
                    {
                        //if (seccion.Encabezado.Equals("OTROS INGRESOS"))
                        //{
                        //    seccion.TotalConIVA = 0.00M;
                        //    seccion.TotalSinIVA = 0.00M;
                        //    seccion.IVA = 0.00M;
                        //}
                        //else
                        //{
                            seccion.TotalConIVA = seccion.TotalConIVA + desglose.TotalConIVA;
                            seccion.TotalSinIVA = (seccion.TotalConIVA / (1 + iva));
                            seccion.IVA = (seccion.TotalConIVA - seccion.TotalSinIVA);
                        //}
                    }
                    grupo.TotalConIVA = grupo.TotalConIVA + seccion.TotalConIVA;
                    grupo.TotalSinIVA = (grupo.TotalConIVA / (1 + iva));
                    grupo.IVA = (grupo.TotalConIVA - grupo.TotalSinIVA);
                }
                relacionCaja.TotalConIVA = relacionCaja.TotalConIVA + grupo.TotalConIVA;
                relacionCaja.TotalSinIVA = (relacionCaja.TotalConIVA / (1 + iva));
                relacionCaja.IVA = (relacionCaja.TotalConIVA - relacionCaja.TotalSinIVA);
            }

            // Asignar los Depositos            
            List<DepositoAsociado> depositosAsociados = new List<DepositoAsociado>();
            var parametrosDepositos = new Dictionary<string, object>();
            parametrosDepositos.Add("@CodigoTienda", token.CodeStore);
            parametrosDepositos.Add("@CodigoCaja", token.CodeBox);
            foreach (var item in data.GetDataReader("[dbo].[sp_vanti_server_ObtenerDepositosRelacionDeCaja]", parametrosDepositos))
            {
                DepositoAsociado depositoAsociado = new DepositoAsociado();
                depositoAsociado.TotalConIVA = Convert.ToDecimal(item.GetValue(0)); ;
                ConfigGeneralesCajaTiendaFormaPago configGeneralesCajaTiendaFormaPago = new ConfigGeneralesCajaTiendaFormaPago();
                configGeneralesCajaTiendaFormaPago.IdentificadorFormaPago = item.GetValue(1).ToString();
                configGeneralesCajaTiendaFormaPago.CodigoFormaPago = item.GetValue(2).ToString();
                configGeneralesCajaTiendaFormaPago.DescripcionFormaPago = item.GetValue(3).ToString();
                depositoAsociado.InformacionAsociadaFormasPago = configGeneralesCajaTiendaFormaPago;
                depositosAsociados.Add(depositoAsociado);
            }
            relacionCaja.DepositosAsociados = depositosAsociados.ToArray();
            return relacionCaja;
        }

        private decimal ObtenerImpuestoTienda(int codeStore)
        {
            decimal valorImpuesto = 0.00M;
            var parametrosImpuestos = new Dictionary<string, object>();
            parametrosImpuestos.Add("@CodigoTienda", codeStore);
            foreach (var resultado in data.GetDataReader("[dbo].[sp_vanti_ObtenerImpuestoTienda]", parametrosImpuestos))
            {
                var respuesta = resultado.GetValue(0).ToString();
                valorImpuesto = Convert.ToDecimal(resultado.GetValue(1));
            }
            return valorImpuesto / 100;
        }

        /// <summary>
        /// Método para ejecutar la sincronizacion
        /// </summary>
        /// <param name="informacionCajaRequest"></param>
        /// <returns></returns>
        public ValidacionOperacionResponse EjecutarSincronizacion(InformacionCajaRequest informacionCajaRequest)
        {
            ValidacionOperacionResponse operacionResponse = new ValidacionOperacionResponse();
            var parameters = new Dictionary<string, object>();
            using (TransactionScope scope = new TransactionScope())
            {
                foreach (var item in data.GetDataReader("SP_A_Utilizar", parameters))
                {
                    operacionResponse.CodeNumber = item.GetValue(0).ToString();
                    operacionResponse.CodeNumber = item.GetValue(1).ToString();
                }
                scope.Complete();
            }
            return operacionResponse;
        }

        /// <summary>
        /// Genera la lectura z offline
        /// </summary>
        /// <param name="informacionCajaRequest"></param>
        /// <returns></returns>
        public ValidacionOperacionResponse GenerarLecturaZOffline(InformacionCajaRequest informacionCajaRequest)
        {
            ValidacionOperacionResponse operacionResponse = new ValidacionOperacionResponse();
            var parameters = new Dictionary<string, object>();
            using (TransactionScope scope = new TransactionScope())
            {
                foreach (var item in data.GetDataReader("SP_A_Utilizar", parameters))
                {
                    operacionResponse.CodeNumber = item.GetValue(0).ToString();
                    operacionResponse.CodeNumber = item.GetValue(1).ToString();
                }
                scope.Complete();
            }
            return operacionResponse;
        }

        /// <summary>
        /// Método para obtener CashOut de las Cajas
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public CompendioCashOut ObtenerCashOut(TokenDto token)
        {
            CompendioCashOut compendioCashOut = new CompendioCashOut();
            List<CashOutCaja> cashOutCajas = new List<CashOutCaja>();
            compendioCashOut.CodigoTienda = token.CodeStore;
            // Procesar información de las Cajas
            var parametrosCD = new Dictionary<string, object>();
            foreach (var caja in data.GetDataReader("[dbo].[sp_vanti_server_ObtenerCajasDestino]", parametrosCD))
            {
                // Procesar información de las Lecturas de cada Caja
                CashOutCaja cashOutCaja = new CashOutCaja();
                cashOutCaja.CodigoCaja = Convert.ToInt16(caja.GetValue(1));
                List<LecturaZ> lecturasZ = new List<LecturaZ>();
                var parametrosLZ = new Dictionary<string, object>();
                parametrosLZ.Add("@CodigoTienda", compendioCashOut.CodigoTienda);
                parametrosLZ.Add("@CodigoCaja", cashOutCaja.CodigoCaja);
                foreach (var lectura in data.GetDataReader("[dbo].[sp_vanti_server_ObtenerLecturasZPorCaja]", parametrosLZ))
                {
                    LecturaZ lecturaZ = new LecturaZ();
                    lecturaZ.FolioCorte = lectura.GetValue(0).ToString();
                    // Procesar información del detalle de la Lectura
                    List<DetalleLecturaFormaPago> detallesLecturaFormaPago = new List<DetalleLecturaFormaPago>();
                    var parametrosDLZ = new Dictionary<string, object>();
                    parametrosDLZ.Add("@FolioCorteZ", lecturaZ.FolioCorte);
                    foreach (var detalleLectura in data.GetDataReader("[dbo].[sp_vanti_server_ObtenerDetalleLecturaZ]", parametrosDLZ))
                    {
                        DetalleLecturaFormaPago detalleLecturaFormaPago = new DetalleLecturaFormaPago();
                        detalleLecturaFormaPago.ImporteFisico = 0.00M;
                        detalleLecturaFormaPago.ImporteTeorico = Convert.ToDecimal(detalleLectura.GetValue(0));
                        detalleLecturaFormaPago.FondoFijoActual = Convert.ToDecimal(detalleLectura.GetValue(1));
                        detalleLecturaFormaPago.TotalRetirosParciales = Convert.ToDecimal(detalleLectura.GetValue(2));
                        ConfigGeneralesCajaTiendaFormaPago configGeneralesCajaTiendaFormaPago = new ConfigGeneralesCajaTiendaFormaPago();
                        configGeneralesCajaTiendaFormaPago.IdentificadorFormaPago = detalleLectura.GetValue(3).ToString();
                        configGeneralesCajaTiendaFormaPago.CodigoFormaPago = detalleLectura.GetValue(4).ToString();
                        configGeneralesCajaTiendaFormaPago.DescripcionFormaPago = detalleLectura.GetValue(5).ToString();
                        detalleLecturaFormaPago.InformacionAsociadaFormasPago = configGeneralesCajaTiendaFormaPago;
                        detallesLecturaFormaPago.Add(detalleLecturaFormaPago);
                    }
                    lecturaZ.DetallesLecturaFormaPago = detallesLecturaFormaPago.ToArray();
                    lecturasZ.Add(lecturaZ);
                }
                cashOutCaja.LecturasZ = lecturasZ.ToArray();
                cashOutCajas.Add(cashOutCaja);
            }
            compendioCashOut.CashOutCajas = cashOutCajas.ToArray();
            return compendioCashOut;
        }

        /// <summary>
        /// Método para persistir el Compendio de CashOut
        /// </summary>
        /// <param name="codigoTienda">Código de Tienda</param>
        /// <returns></returns>
        public int AgregarCompendioCashOut(int codigoTienda)
        {
            var parametros = new Dictionary<string, object>();
            parametros.Add("@CodigoTienda", codigoTienda);
            List<System.Data.SqlClient.SqlParameter> parametrosOut = new List<System.Data.SqlClient.SqlParameter>();
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@Id", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            var resultado = data.ExecuteProcedure("[dbo].[sp_vanti_server_AgregarCompendioCashOut]", parametros, parametrosOut);
            return Convert.ToInt16(resultado["@Id"]);
        }

        /// <summary>
        /// Método para persistir el CashOut
        /// </summary>
        /// <param name="idCompendioCashOut">Id del compendio CashOut correspondiente</param>
        /// <param name="codigoCaja">Código de la Caja</param>
        /// <returns></returns>
        public int AgregarCashOut(int idCompendioCashOut, int codigoCaja)
        {
            var parametros = new Dictionary<string, object>();
            parametros.Add("@IdCompendioCashOut", idCompendioCashOut);
            parametros.Add("@CodigoCaja", codigoCaja);
            List<System.Data.SqlClient.SqlParameter> parametrosOut = new List<System.Data.SqlClient.SqlParameter>();
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@Id", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            var resultado = data.ExecuteProcedure("[dbo].[sp_vanti_server_AgregarCashOut]", parametros, parametrosOut);
            return Convert.ToInt16(resultado["@Id"]);
        }

        /// <summary>
        /// Método para persistir la Lectura Z de un CashOut
        /// </summary>
        /// <param name="idCashOut">Id del CashOut correspondiente</param>       
        /// <param name="importeFisico">Importe físico</param>   
        /// <param name="importeTeorico">Importe teórico</param>
        /// <param name="importeMonedaExtranjera"></param>   
        /// <param name="totalRetirosParciales">Total de retiros parciales</param>   
        /// <param name="fondoFijoActual">Fondo fijo actual</param>   
        /// <param name="identificadorFormaPago">Identificador d ela forma de pago</param>   
        /// <param name="codigoFormaPago">Código de la forma de pago</param>   
        /// <param name="descripcionFormaPago">Descripción de la forma de pago</param>   
        /// <returns></returns>
        public void AgregarLecturaZCashOut(int idCashOut, decimal importeFisico, decimal importeTeorico, decimal importeMonedaExtranjera, decimal totalRetirosParciales, decimal fondoFijoActual,
            string identificadorFormaPago, string codigoFormaPago, string descripcionFormaPago)
        {
            var parametros = new Dictionary<string, object>();
            parametros.Add("@IdCashOut", idCashOut);
            parametros.Add("@ImporteFisico", importeFisico);
            parametros.Add("@ImporteTeorico", importeTeorico);
            parametros.Add("@ImporteMonedaExtranjera", importeMonedaExtranjera);
            parametros.Add("@TotalRetirosParciales", totalRetirosParciales);
            parametros.Add("@FondoFijoActual", fondoFijoActual);
            parametros.Add("@IdentificadorFormaPago", identificadorFormaPago);
            parametros.Add("@CodigoFormaPago", codigoFormaPago);
            parametros.Add("@DescripcionFormaPago", descripcionFormaPago);
            List<System.Data.SqlClient.SqlParameter> parametrosOut = new List<System.Data.SqlClient.SqlParameter>();
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            data.ExecuteProcedure("[dbo].[sp_vanti_server_AgregarLecturaZCashOut]", parametros, parametrosOut, true);
        }

        /// <summary>
        /// Método para persistir la relación de Caja
        /// </summary>        
        /// <param name="codigoTienda">Código de Tienda</param>
        /// <param name="totalConIva">Total con IVa de la relación de Caja</param>
        /// <returns></returns>
        public int AgregarRelacionCaja(int codigoTienda, decimal totalConIva, decimal totalSinIva, decimal iva)
        {
            var parametros = new Dictionary<string, object>();
            parametros.Add("@CodigoTienda", codigoTienda);
            parametros.Add("@TotalConIva", totalConIva);
            parametros.Add("@TotalSinIva", totalSinIva);
            parametros.Add("@Iva", iva);
            List<System.Data.SqlClient.SqlParameter> parametrosOut = new List<System.Data.SqlClient.SqlParameter>();
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@Id", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            var resultado = data.ExecuteProcedure("[dbo].[sp_vanti_server_AgregarRelacionCaja]", parametros, parametrosOut);
            return Convert.ToInt16(resultado["@Id"]);
        }

        /// <summary>
        /// Método para persistir un depósito de una Relación de Caja
        /// </summary>        
        /// <param name="idRelacionCaja">Id de la relación de Caja asociada</param>
        /// <param name="totalConIva">Total con IVa de la relación de Caja</param>
        /// <param name="codigoFormaPago">Código de la forma de pago</param>
        /// <param name="descripcionFormaPago">Descripción de la forma de pago</param>
        /// <returns></returns>
        public void AgregarDepositoRelacionCaja(int idRelacionCaja, decimal totalConIva, string codigoFormaPago, string descripcionFormaPago)
        {
            var parametros = new Dictionary<string, object>();
            parametros.Add("@IdRelacionCaja", idRelacionCaja);
            parametros.Add("@TotalConIva", totalConIva);
            parametros.Add("@CodigoFormaPago", codigoFormaPago);
            parametros.Add("@DescripcionFormaPago", descripcionFormaPago);
            List<System.Data.SqlClient.SqlParameter> parametrosOut = new List<System.Data.SqlClient.SqlParameter>();
            var resultado = data.ExecuteProcedure("[dbo].[sp_vanti_server_AgregarDepositoRelacionCaja]", parametros, parametrosOut);
        }

        /// <summary>
        /// Método para persistir una sección de una Relación de Caja
        /// </summary>        
        /// <param name="idRelacionCaja">Id de la relación de Caja asociada</param>
        /// <param name="totalConIva">Total con IVa de la relación de Caja</param>
        /// <param name="encabezado">Encabezado de la sección</param>
        /// <returns></returns>
        public int AgregarGrupoRelacionCaja(int idRelacionCaja, decimal totalConIva, string encabezado)
        {
            var parametros = new Dictionary<string, object>();
            parametros.Add("@IdRelacionCaja", idRelacionCaja);
            parametros.Add("@TotalConIva", totalConIva);
            parametros.Add("@Encabezado", encabezado);
            List<System.Data.SqlClient.SqlParameter> parametrosOut = new List<System.Data.SqlClient.SqlParameter>();
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@Id", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            var resultado = data.ExecuteProcedure("[dbo].[sp_vanti_server_AgregarGrupoRelacionCaja]", parametros, parametrosOut);
            return Convert.ToInt16(resultado["@Id"]);
        }

        /// <summary>
        /// Método para persistir una sección de una Relación de Caja
        /// </summary>        
        /// <param name="idRelacionGrupo">Id del grupo de la relación de caja asociada</param>
        /// <param name="totalConIva">Total con IVa de la relación de Caja</param>
        /// <param name="encabezado">Encabezado de la sección</param>
        /// <returns></returns>
        public int AgregarSeccionRelacionCaja(int idRelacionGrupo, decimal totalConIva, string encabezado, decimal totalSinIva, decimal iva)
        {
            var parametros = new Dictionary<string, object>();
            parametros.Add("@IdRelacionGrupo", idRelacionGrupo);
            parametros.Add("@TotalConIva", totalConIva);
            parametros.Add("@TotalSinIva", totalSinIva);
            parametros.Add("@Iva", iva);
            parametros.Add("@Encabezado", encabezado);
            List<System.Data.SqlClient.SqlParameter> parametrosOut = new List<System.Data.SqlClient.SqlParameter>();
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@Id", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            var resultado = data.ExecuteProcedure("[dbo].[sp_vanti_server_AgregarSeccionRelacionCaja]", parametros, parametrosOut);
            return Convert.ToInt16(resultado["@Id"]);
        }

        /// <summary>
        /// Método para persistir un desglose de una sección de una Relación de Caja
        /// </summary>        
        /// <param name="idSeccionRelacionCaja">Id de la sección de la relación de Caja asociada</param>
        /// <param name="totalConIva">Total con IVa de la relación de Caja</param>
        /// <param name="descripcion">Descripción del desglose de la sección</param>
        /// <returns></returns>
        public ValidacionOperacionResponse AgregarDesgloseSeccionRelacionCaja(int idSeccionRelacionCaja, decimal totalConIva, string descripcion)
        {
            ValidacionOperacionResponse validacionOperacionResponse = new ValidacionOperacionResponse();
            var parametros = new Dictionary<string, object>();
            parametros.Add("@IdSeccionRelacionCaja", idSeccionRelacionCaja);
            parametros.Add("@TotalConIva", totalConIva);
            parametros.Add("@Descripcion", descripcion);
            List<System.Data.SqlClient.SqlParameter> parametrosOut = new List<System.Data.SqlClient.SqlParameter>();
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var resultado = data.ExecuteProcedure("[dbo].[sp_vanti_server_AgregarDesgloseSeccionRelacionCaja]", parametros, parametrosOut);
            validacionOperacionResponse.CodeNumber = resultado["@CodigoResultado"].ToString();
            validacionOperacionResponse.CodeDescription = resultado["@MensajeResultado"].ToString();
            return validacionOperacionResponse;
        }

        /// <summary>
        /// Método para registrar un fin de día exitosamente
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public ValidacionOperacionResponse RegistrarFinDia(TokenDto token)
        {
            ValidacionOperacionResponse validacionOperacionResponse = new ValidacionOperacionResponse();
            var parametros = new Dictionary<string, object>();
            parametros.Add("@CodigoTienda", token.CodeStore);
            parametros.Add("@CodigoCaja", token.CodeBox);
            parametros.Add("@CodigoEmpleado", token.CodeEmployee);
            List<System.Data.SqlClient.SqlParameter> parametrosOut = new List<System.Data.SqlClient.SqlParameter>();
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var resultado = data.ExecuteProcedure("[dbo].[sp_vanti_server_RegistrarFinDia]", parametros, parametrosOut);
            validacionOperacionResponse.CodeNumber = resultado["@CodigoResultado"].ToString();
            validacionOperacionResponse.CodeDescription = resultado["@MensajeResultado"].ToString();
            return validacionOperacionResponse;
        }

        /// <summary>
        /// Método para hacer un Logout
        /// </summary>
        /// <param name="token">Token de la sesión</param>
        /// <param name="codigoCaja">Código de la caja sobre la cual se hará el Logout</param>
        /// <returns></returns>
        public ValidacionOperacionResponse RegistrarLogoutCaja(TokenDto token)
        {
            ValidacionOperacionResponse validacionOperacionResponse = new ValidacionOperacionResponse();
            var parametros = new Dictionary<string, object>();
            parametros.Add("@CodigoTienda", token.CodeStore);
            parametros.Add("@CodigoCaja", token.CodeBox);
            parametros.Add("@CodigoEmpleado", token.CodeEmployee);
            List<System.Data.SqlClient.SqlParameter> parametrosOut = new List<System.Data.SqlClient.SqlParameter>();
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var resultado = data.ExecuteProcedure("[dbo].[sp_vanti_SegLogout]", parametros, parametrosOut);
            validacionOperacionResponse.CodeNumber = resultado["@CodigoResultado"].ToString();
            validacionOperacionResponse.CodeDescription = resultado["@MensajeResultado"].ToString();
            return validacionOperacionResponse;
        }

        /// <summary>
        /// Registrar la Lectura Z Offline
        /// </summary>
        /// <returns>Mensaje de actualización de datos</returns>
        public OperationResponse RegistrarLecturaZOffline(int codigoTienda, int codigoCaja, string folioCorte)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", codigoTienda);
            parameters.Add("@CodigoCaja", codigoCaja);
            parameters.Add("@FolioCorte", folioCorte);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var resultado = data.ExecuteProcedure("[dbo].[sp_vanti_server_RegistrarLecturaZOffline]", parameters, parametersOut);
            operationResponse.CodeNumber = resultado["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = resultado["@MensajeResultado"].ToString();
            return operationResponse;
        }

        /// <summary>
        /// Se valida una clave para autorizar hacer un corte Z Offline
        /// </summary>
        /// <returns></returns>
        public ValidacionOperacionResponse ValidarCorteOffline(TokenDto token, AutenticacionOfflineRequest autenticacionOfflineRequest)
        {
            ValidacionOperacionResponse validacionOperacionResponse = new ValidacionOperacionResponse();
            var parametros = new Dictionary<string, object>();
            parametros.Add("@CodigoTienda", token.CodeStore);
            parametros.Add("@CodigoCaja", token.CodeBox);
            parametros.Add("@CodigoEmpleado", token.CodeEmployee);

            // Obtener la informacion de folio y fecha de operacion
            List<System.Data.SqlClient.SqlParameter> parametrosOutInfo = new List<System.Data.SqlClient.SqlParameter>();
            parametrosOutInfo.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@FechaOperacion", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Date });
            parametrosOutInfo.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@Folio", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            var resultadoInfo = data.ExecuteProcedure("[dbo].[sp_vanti_Obtener_Informacion_CorteOffline]", parametros, parametrosOutInfo);
            var fechaOperacion = Convert.ToDateTime(resultadoInfo["@FechaOperacion"]);
            var folio = Convert.ToInt32(resultadoInfo["@Folio"]);

            //Obtener cadena del algoritmo
            string cadenaAlgoritmo = this.getCodeAlgorithm(fechaOperacion.ToString("yyyyMMdd"), fechaOperacion.ToString("yyyy-MM-dd"), token.CodeStore, token.CodeBox, folio);

            //Validar cadena generada con el algoritmo vs la clave que nos envian desde cliente
            if (cadenaAlgoritmo == autenticacionOfflineRequest.Clave)
            {
                validacionOperacionResponse.CodeNumber = "400";
                validacionOperacionResponse.CodeDescription = "Los datos son correctos";
            }
            else
            {
                validacionOperacionResponse.CodeNumber = "409";
                validacionOperacionResponse.CodeDescription = "No se ha podido iniciar sesion, verifique la clave de acceso";
            }
            return validacionOperacionResponse;
        }

        // Metodo para crear el codigo del algoritmo en base a los datos que requiere
        private string getCodeAlgorithm(string fecha, string fechadate, Int64 tienda, int caja, int folio)
        {
            int fec = 0, t = 0, fo = 0;
            fec = int.Parse(fecha.Substring(fecha.Length - 6, 6));
            fo = folio.ToString().Length > 4 ? int.Parse(folio.ToString().Substring(folio.ToString().Length - 4, 4)) : folio;
            Int64 ProductoCruz = caja * fec * tienda * fo;
            t = int.Parse(tienda.ToString().Substring(tienda.ToString().Length - 2, 2));
            fec = int.Parse(fecha.Substring(0, 4).Substring(fecha.Substring(0, 4).Length - 2, 2));

            var a = t * fec;
            var b = Convert.ToDateTime(fechadate).Month * caja;
            var c = ProductoCruz / int.Parse(fecha);
            var d = t * caja + int.Parse(fecha.Substring(fecha.Length - 1, 1));
            var ee = caja * int.Parse(fecha.Substring(fecha.Length - 2, 2));
            var redondo = Math.Round(Convert.ToDecimal(fecha) / Convert.ToDecimal(tienda), 5) - (int.Parse(fecha) / tienda);
            var sustituir = redondo.ToString().Replace(".", "");
            var f = sustituir.Length > 3 ? sustituir.Substring(0, 3) : sustituir;
            string ConcatenacionGeneral = a.ToString() + b.ToString() + c.ToString() + d.ToString() + ee.ToString() + f.ToString();
            string NumeroFinal = ConcatenacionGeneral.Length > 15 ? ConcatenacionGeneral.Substring(0, 15).Substring(ConcatenacionGeneral.Substring(0, 15).Length - 10, 10) : ConcatenacionGeneral.Substring(0, 10);

            return NumeroFinal;
        }
    }
}
