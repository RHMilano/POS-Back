using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.Configuracion;
using Milano.BackEnd.Dto.General;

namespace Milano.BackEnd.Repository.General
{

    /// <summary>
    /// Repositorio de configuraciones generales de las cajas y tiendas
    /// </summary>
    public class ConfigGeneralesCajaTiendaRepository : BaseRepository
    {
        private ConfigGeneralesCajaTiendaResponse GetConfiguraciones(int CodigoCaja, int CodigoTienda, int codigoEmpleado, int registrarTransaccionLogin)
        {
            ConfigGeneralesCajaTiendaResponse cnf = new ConfigGeneralesCajaTiendaResponse();
            ConfigGeneralesCajaTiendaImpuesto cnfImpuestos = new ConfigGeneralesCajaTiendaImpuesto();
            ConfigGeneralesCajaTiendaFormaPago cnfFormasPago = new ConfigGeneralesCajaTiendaFormaPago();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", CodigoTienda);
            parameters.Add("@CodigoCaja", CodigoCaja);
            parameters.Add("@CodigoEmpleado", codigoEmpleado);
            parameters.Add("@RegistrarTransaccionLogin", registrarTransaccionLogin);
            foreach (var c in data.GetDataReader("dbo.sp_vanti_ConfiguracionesGeneralesCajaTienda", parameters))
            {
                cnf.ColorVentaRegular = c.GetValue(0).ToString();
                cnf.ColorVentaEmpleaado = c.GetValue(1).ToString();
                cnf.ColorVentaMayorista = c.GetValue(2).ToString();
                cnf.ColorDevoluciones = c.GetValue(3).ToString();
                cnf.ColorFormasDePago = c.GetValue(4).ToString();
                cnf.RutaLogTransacciones = c.GetValue(5).ToString();
                cnf.MontoMaximoCambioVales = Convert.ToDecimal(c.GetValue(6));
                cnf.SkuTarjetaRegalo = Convert.ToInt32(c.GetValue(7));
                cnf.SkuPagoConValeMayorista = Convert.ToInt32(c.GetValue(8));
                cnf.SkuComisionPagoServicios = Convert.ToInt32(c.GetValue(9));
                cnf.PorcentajePagoConValeMayorista = Convert.ToInt32(c.GetValue(10));
                cnf.MontoMinimoAbonoApartado = Convert.ToDecimal(c.GetValue(11));
                cnf.MontoMinimoPorcentajeApartado = Convert.ToDecimal(c.GetValue(12));
                cnf.PorcentajeMaximoDescuentoDirecto = Convert.ToDecimal(c.GetValue(13));
                cnf.SkuPagoTCMM = Convert.ToInt32(c.GetValue(14));
                cnf.SkuPagoMayorista = Convert.ToInt32(c.GetValue(15));
                cnf.SkuPagoComisionMayorista = Convert.ToInt32(c.GetValue(16));
                cnfImpuestos.CodigoImpuesto = c.GetValue(17).ToString();
                cnfImpuestos.PorcentajeImpuesto = Convert.ToDecimal(c.GetValue(18));
                cnf.InformacionAsociadaImpuestos = cnfImpuestos;
                cnf.PosModoConsulta = Convert.ToBoolean(c.GetValue(19));
            }
            return cnf;
        }

        /// <summary>
        /// Busca y regresa las configuraciones generales de la caja
        /// </summary>
        /// <param name="CodigoCaja">Número de la caja</param>
        /// <param name="CodigoTienda">Codigo de la tienda a buscar</param>
        /// <param name="codigoEmpleado">Codigo del empleado</param>
        /// <param name="versionPOS">Version parches Milano</param>
        /// <returns>Configuraciones de la tienda </returns>
        /// 
        public ConfigGeneralesCajaTiendaResponse GetConfig(int CodigoCaja, int CodigoTienda, int codigoEmpleado, string versionPOS = "")
        {
            OperationResponse operationResponseFinDia = new OperationResponse();
            OperationResponse operationResponseInicioDia = new OperationResponse();
            OperationResponse responseValidarFecha = new OperationResponse();
            ConfigGeneralesCajaTiendaResponse cnf = GetConfiguraciones(CodigoCaja, CodigoTienda, codigoEmpleado, 1);
            cnf.ConfiguracionBotonera = GetBotonCnfg(CodigoCaja, CodigoTienda);
            cnf.InformacionCatalogoRecursos = GetRecursosCnfg();
            // Si se trata de Caja 0 
            if (CodigoCaja == 0)
            {
                cnf.PosModoConsulta = true;
                cnf.ConfiguracionBotonera.ConfiguracionBotones = this.GetBotonConsulta(cnf.ConfiguracionBotonera.ConfiguracionBotones, true, true, false, true);
                return cnf;
            }
            
            //OCG: Integracion de la version del POS
            responseValidarFecha = ValidarFechasInicioDia(CodigoCaja, CodigoTienda, codigoEmpleado, versionPOS);

            // En caso de abrir POS normal 408
            if (responseValidarFecha.CodeNumber == "408")
            {
                cnf.PosModoConsulta = false;
                cnf.ConfiguracionBotonera.ConfiguracionBotones = this.GetBotonConsulta(cnf.ConfiguracionBotonera.ConfiguracionBotones, false, false, false, false);
                return cnf;
            }
            // En caso de abrir POS en modo consulta
            else
            {
                cnf.PosModoConsulta = true;
                cnf.ConfiguracionBotonera.ConfiguracionBotones = this.GetBotonConsulta(cnf.ConfiguracionBotonera.ConfiguracionBotones, true, true, true, false);
                return cnf;
            }
        }

        private ConfiguracionBoton[] GetBotonConsulta(ConfiguracionBoton[] configuracionBotones, Boolean aplicarModoConsulta, Boolean incluirBackOfficeBtn, Boolean incluirInicioDiaBtn, Boolean incluirFinDiaBtn)
        {
            List<ConfiguracionBoton> configuracionBotons = new List<ConfiguracionBoton>();
            //Se activan solamente los botones de consulta
            foreach (var boton in configuracionBotones)
            {
                // Deshabilitar botón
                if (aplicarModoConsulta)
                {
                    boton.Habilitado = false;
                }
                else
                {
                    if (boton.Identificador == "botonBackoffice")
                    {
                        boton.Habilitado = false;
                    }
                }
                // Habilitar botones
                if ((boton.Identificador == "buscarPrecio") && (aplicarModoConsulta))
                {
                    boton.Habilitado = true;
                }

                if ((boton.Identificador == "gerente") && (aplicarModoConsulta))
                {
                    boton.Habilitado = true;
                }

                if ((boton.Identificador == "botonBackoffice") && (incluirBackOfficeBtn))
                {
                    boton.Habilitado = true;
                }
                foreach (var subBoton in boton.ConfiguracionSubBotones)
                {
                    // Deshabilitar botón
                    if (aplicarModoConsulta)
                    {
                        subBoton.Habilitado = false;
                    }
                    else
                    {
                        if (subBoton.Identificador == "botonInicioDia")
                        {
                            subBoton.Habilitado = false;
                        }
                        if (subBoton.Identificador == "botonFinDia")
                        {
                            subBoton.Habilitado = false;
                        }
                    }
                    // Habilitar botones
                    if ((subBoton.Identificador == "botonInicioDia") && (incluirInicioDiaBtn))
                    {
                        subBoton.Habilitado = true;
                    }
                    if ((subBoton.Identificador == "botonFinDia") && (incluirFinDiaBtn))
                    {
                        subBoton.Habilitado = true;
                    }

                    if ((subBoton.Identificador == "reportes") && (aplicarModoConsulta))
                    {
                        subBoton.Habilitado = true;
                    }

                }
                configuracionBotons.Add(boton);
            }
            return configuracionBotons.ToArray();
        }

        private OperationResponse ValidarFechasInicioDia(int codigoCaja, int codigoTienda, int codigoEmpleado, string versionPOS = "")
        {
            OperationResponse responseValidaFechas = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoCaja", codigoCaja);
            parameters.Add("@CodigoTienda", codigoTienda);
            parameters.Add("@CodigoEmpleado", codigoEmpleado);
            parameters.Add("@versionPOXS", versionPOS);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_server_ValidarFechasOperacionInicioDia]", parameters, parametersOut);
            responseValidaFechas.CodeNumber = result["@CodigoResultado"].ToString();
            responseValidaFechas.CodeDescription = result["@MensajeResultado"].ToString();
            return responseValidaFechas;
        }

        /// <summary>
        /// Método que regresa la información de los recursos de seguridad disponibles
        /// </summary>
        /// <returns></returns>
        public ConfigGeneralesRecurso[] GetRecursosCnfg()
        {
            List<ConfigGeneralesRecurso> listaRecursos = new List<ConfigGeneralesRecurso>();
            var parameters = new Dictionary<string, object>();
            foreach (var item in data.GetDataReader("[dbo].[sp_vanti_ConfiguracionCatalogoRecursos]", parameters))
            {
                ConfigGeneralesRecurso configGeneralesRecurso = new ConfigGeneralesRecurso();
                configGeneralesRecurso.Endpoint = item.GetValue(0).ToString();
                configGeneralesRecurso.Descripcion = item.GetValue(1).ToString();
                listaRecursos.Add(configGeneralesRecurso);
            }
            return listaRecursos.ToArray();
        }

        /// <summary>
        /// Busca y regresa las configuraciones generales de la caja
        /// </summary>
        /// <param name="CodigoCaja">Número de la caja</param>
        /// <param name="CodigoTienda">Codigo de la tienda a buscar</param>
        /// <param name="codigoEmpleado">Codigo del empleado</param>
        /// <returns>Configuraciones de la tienda </returns>
        /// 
		public ConfigGeneralesCajaTiendaResponse GetConfigSinBotonera(int CodigoCaja, int CodigoTienda, int codigoEmpleado)
        {
            return GetConfiguraciones(CodigoCaja, CodigoTienda, codigoEmpleado, 0);
        }

        /// <summary>
        /// Método que regresa la configuración de la botonera
        /// </summary>
        /// <returns></returns>
        /// 
        public ConfiguracionBotonera GetBotonCnfg(int CodigoCaja, int CodigoTienda)
        {
            ConfiguracionBotonera configuracionBotonera = new ConfiguracionBotonera();
            int menuId = 0;
            List<ConfiguracionBoton> confBotonesLst = new List<ConfiguracionBoton>();
            ConfiguracionBoton mainConfigButtom;
            var parameters = new Dictionary<string, object>();
            parameters.Add("@MenuId", menuId);
            parameters.Add("@CodigoTienda", CodigoTienda);
            parameters.Add("@CodigoCaja", CodigoCaja);
            foreach (var result in data.GetDataReader("dbo.sp_vanti_ConfiguracionBotones", parameters))
            {
                mainConfigButtom = new ConfiguracionBoton();
                mainConfigButtom.Orden = Convert.ToInt32(result.GetValue(1));
                mainConfigButtom.Identificador = result.GetValue(2).ToString();
                mainConfigButtom.TextoDescripcion = result.GetValue(3).ToString();
                mainConfigButtom.RutaImagen = result.GetValue(4).ToString();
                mainConfigButtom.Habilitado = Convert.ToBoolean(result.GetValue(5).ToString());
                mainConfigButtom.Visible = Convert.ToBoolean(result.GetValue(6).ToString());
                mainConfigButtom.TeclaAccesoRapido = result.GetValue(7).ToString();
                mainConfigButtom.ConfiguracionSubBotones = GetSuMenu(Convert.ToInt32(result.GetValue(0)), CodigoCaja, CodigoTienda);
                confBotonesLst.Add(mainConfigButtom);
            }
            
            configuracionBotonera.ConfiguracionBotones = confBotonesLst.ToArray();
            return configuracionBotonera;
        }

        private ConfiguracionBoton[] GetSuMenu(int idPadre, int CodigoCaja, int CodigoTienda)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@menuId", idPadre);
            parameters.Add("@CodigoTienda", CodigoTienda);
            parameters.Add("@CodigoCaja", CodigoCaja);
            List<ConfiguracionBoton> arrayList = new List<ConfiguracionBoton>();
            foreach (var result in data.GetDataReader("dbo.sp_vanti_ConfiguracionBotones", parameters))
            {
                ConfiguracionBoton mainConfigButtom = new ConfiguracionBoton();
                mainConfigButtom = new ConfiguracionBoton();
                mainConfigButtom.Orden = Convert.ToInt32(result.GetValue(1));
                mainConfigButtom.Identificador = result.GetValue(2).ToString();
                mainConfigButtom.TextoDescripcion = result.GetValue(3).ToString();
                mainConfigButtom.RutaImagen = result.GetValue(4).ToString();
                mainConfigButtom.Habilitado = Convert.ToBoolean(result.GetValue(5).ToString());
                mainConfigButtom.Visible = Convert.ToBoolean(result.GetValue(6).ToString());
                mainConfigButtom.TeclaAccesoRapido = result.GetValue(7).ToString();
                mainConfigButtom.ConfiguracionSubBotones = GetSuMenu(Convert.ToInt32(result.GetValue(0)), CodigoCaja, CodigoTienda);
                arrayList.Add(mainConfigButtom);
            }
            return arrayList.ToArray();
        }
    }
}
