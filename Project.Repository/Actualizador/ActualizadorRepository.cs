using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.Actualizador;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Dto.InicioFinDia;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Milano.BackEnd.Repository.Actualizador
{
    /// <summary>
    /// 
    /// </summary>
    public class ActualizadorRepository : BaseRepository
    {

        /// <summary>
        /// Método para validar si existen actualizaciones de Software pendientes e información de control asociada
        /// </summary>        
        /// <returns></returns>
        public ActualizacionSoftwareResponse ValidarExistenciaActualizacionesPendientes(int idVersionMaximaActualizacion, int invocacionBajoDemanda)
        {
            ActualizacionSoftwareResponse actualizacionSoftwareResponse = new ActualizacionSoftwareResponse();
            var parametros = new Dictionary<string, object>();
            parametros.Add("@IdVersionMaximaActualizacion", idVersionMaximaActualizacion);
            parametros.Add("@InvocacionBajoDemanda", invocacionBajoDemanda);
            List<System.Data.SqlClient.SqlParameter> parametrosOut = new List<System.Data.SqlClient.SqlParameter>();
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@IdVersionBase", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@IdVersionMaximaActualizacionOut", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@EtiquetaLanzamientoVersionBase", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@EtiquetaLanzamientoVersionMaximaActualizacion", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var resultado = data.ExecuteProcedure("[dbo].[sp_vanti_Updater_ValidarExistenciaActualizacionesPendientes]", parametros, parametrosOut);
            actualizacionSoftwareResponse.CodeNumber = resultado["@CodigoResultado"].ToString();
            actualizacionSoftwareResponse.CodeDescription = resultado["@MensajeResultado"].ToString();
            actualizacionSoftwareResponse.IdVersionBase = Convert.ToInt32(resultado["@IdVersionBase"]);
            actualizacionSoftwareResponse.EtiquetaLanzamientoVersionBase = resultado["@EtiquetaLanzamientoVersionBase"].ToString();
            actualizacionSoftwareResponse.IdVersionMaximaActualizacion = Convert.ToInt32(resultado["@IdVersionMaximaActualizacionOut"]);
            actualizacionSoftwareResponse.EtiquetaLanzamientoVersionMaximaActualizacion = resultado["@EtiquetaLanzamientoVersionMaximaActualizacion"].ToString();
            return actualizacionSoftwareResponse;
        }

        /// <summary>
        /// Obtener la lista de actualizaciones pendientes
        /// </summary>        
        /// <returns>Lista de actualizaciones pendientes</returns>
        public InformacionVersionSoftware[] ObtenerListaActualizacionesPendientes(int idVersionBase, int idVersionMaximaActualizacion)
        {
            List<InformacionVersionSoftware> informacionVersionesSoftwarePendientes = new List<InformacionVersionSoftware>();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@IdVersionBase", idVersionBase);
            parameters.Add("@IdVersionMaximaActualizacion", idVersionMaximaActualizacion);
            foreach (var datos in data.GetDataReader("[dbo].[sp_vanti_Updater_ObtenerListaActualizacionesPendientes]", parameters))
            {
                InformacionVersionSoftware informacionVersionSoftware = new InformacionVersionSoftware();
                informacionVersionSoftware.IdVersion = Convert.ToInt32(datos.GetValue(0));
                informacionVersionSoftware.TipoActualizacion = datos.GetValue(1).ToString();
                informacionVersionSoftware.EtiquetaLanzamientro = datos.GetValue(2).ToString();
                informacionVersionSoftware.Descripcion = datos.GetValue(3).ToString();
                informacionVersionSoftware.FechaLanzamiento = datos.GetValue(4).ToString();
                informacionVersionSoftware.IdentificadorRepositorio = datos.GetValue(5).ToString();
                informacionVersionSoftware.IdVersionMinimaRequerida = Convert.ToInt32(datos.GetValue(6));
                informacionVersionesSoftwarePendientes.Add(informacionVersionSoftware);
            }
            return informacionVersionesSoftwarePendientes.ToArray();
        }

        /// <summary>
        /// Método para generar una nueva petición de actualización
        /// </summary>        
        /// <returns></returns>
        public void GenerarPeticionActualizacion(int idVersion, string estatusProceso)
        {
            var parametros = new Dictionary<string, object>();
            parametros.Add("@IdVersion", idVersion);
            parametros.Add("@EstatusProceso", estatusProceso);
            List<System.Data.SqlClient.SqlParameter> parametrosOut = new List<System.Data.SqlClient.SqlParameter>();
            var resultado = data.ExecuteProcedure("[dbo].[sp_vanti_Updater_GenerarPeticionActualizacion]", parametros, parametrosOut);
        }

        /// <summary>
        /// Método para obtener el idVersion máximo solicitado para una actualización
        /// </summary>        
        /// <returns></returns>
        public int ObtenerIdVersionMaximaActualizacionPedida()
        {
            var parametros = new Dictionary<string, object>();
            List<System.Data.SqlClient.SqlParameter> parametrosOut = new List<System.Data.SqlClient.SqlParameter>();
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@IdVersionMaximaActualizacionPedida", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            var resultado = data.ExecuteProcedure("[dbo].[sp_vanti_Updater_ObtenerIdVersionMaximaActualizacionPedida]", parametros, parametrosOut);
            return Convert.ToInt32(resultado["@IdVersionMaximaActualizacionPedida"]);
        }

        /// <summary>
        /// Método para actualizar el estatus de un proceso de actualización en curso
        /// </summary>        
        /// <returns></returns>
        public ProcesoActualizacionSoftwareResponse ActualizarEstatusProcesoActualizacionEnCurso(int idVersion, string estatusProceso,
            string descripcionEstatusProceso, string logProceso)
        {
            ProcesoActualizacionSoftwareResponse procesoActualizacionSoftwareResponse = new ProcesoActualizacionSoftwareResponse();
            var parametros = new Dictionary<string, object>();
            parametros.Add("@IdVersion", idVersion);
            parametros.Add("@EstatusProceso", estatusProceso);
            parametros.Add("@DescripcionEstatusProceso", descripcionEstatusProceso);
            parametros.Add("@LogProceso", logProceso);
            List<System.Data.SqlClient.SqlParameter> parametrosOut = new List<System.Data.SqlClient.SqlParameter>();
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var resultado = data.ExecuteProcedure("[dbo].[sp_vanti_Updater_ActualizarEstatusProcesoActualizacion]", parametros, parametrosOut);
            procesoActualizacionSoftwareResponse.CodeNumber = resultado["@CodigoResultado"].ToString();
            procesoActualizacionSoftwareResponse.CodeDescription = resultado["@MensajeResultado"].ToString();
            return procesoActualizacionSoftwareResponse;
        }

        /// <summary>
        /// Obtener el estatus actual de una actualización en curso si existe
        /// </summary>                
        public EstatusActualizacionSoftwareResponse ObtenerEstatusProcesoActualizacionEnCurso(int invocacionBajoDemanda)
        {
            EstatusActualizacionSoftwareResponse estatusActualizacionSoftwareResponse = new EstatusActualizacionSoftwareResponse();
            var parametros = new Dictionary<string, object>();
            parametros.Add("@InvocacionBajoDemanda", invocacionBajoDemanda);
            List<System.Data.SqlClient.SqlParameter> parametrosOut = new List<System.Data.SqlClient.SqlParameter>();
            foreach (var datos in data.GetDataReader("[dbo].[sp_vanti_Updater_ObtenerEstatusProcesoActualizacionEnCurso]", parametros))
            {
                estatusActualizacionSoftwareResponse.IdVersion = Convert.ToInt32(datos.GetValue(0));
                estatusActualizacionSoftwareResponse.EstatusProceso = datos.GetValue(1).ToString();
                estatusActualizacionSoftwareResponse.DescripcionEtatusProceso = datos.GetValue(2).ToString();
                estatusActualizacionSoftwareResponse.LogProceso = datos.GetValue(3).ToString();
                estatusActualizacionSoftwareResponse.ProcesoActualizacionEnCurso = Convert.ToInt32(datos.GetValue(4));
                estatusActualizacionSoftwareResponse.FechaInicioProcesoActualizacion = datos.GetValue(5).ToString();
                estatusActualizacionSoftwareResponse.FechaFinProcesoActualizacion = datos.GetValue(6).ToString();
                estatusActualizacionSoftwareResponse.FechaUltimaActualizacion = datos.GetValue(7).ToString();
            }
            return estatusActualizacionSoftwareResponse;
        }

        /// <summary>
        /// Obtener el estatus actual de la bandera de reinicio del navegador
        /// </summary>                
        public String ObtenerEstatusReinicioNavegador()
        {
            var parametros = new Dictionary<string, object>();
            List<System.Data.SqlClient.SqlParameter> parametrosOut = new List<System.Data.SqlClient.SqlParameter>();
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@BanderaReinicioNavegador", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            var resultado = data.ExecuteProcedure("[dbo].[sp_vanti_Updater_ObtenerBanderaNavegador]", parametros, parametrosOut);
            return resultado["@BanderaReinicioNavegador"].ToString();
        }

        /// <summary>
        /// Actualizar el estatus actual de la bandera de reinicio del navegador
        /// </summary>                
        public String ActualizarEstatusReinicioNavegador()
        {
            var parametros = new Dictionary<string, object>();
            List<System.Data.SqlClient.SqlParameter> parametrosOut = new List<System.Data.SqlClient.SqlParameter>();
            data.ExecuteProcedure("[dbo].[sp_vanti_Updater_ActualizarBanderaNavegador]", parametros, parametrosOut);
            return "OK";
        }

        /// <summary>
        /// Actualizar el estatus actual de la bandera de reinicio del navegador
        /// </summary>                
        public String EstatusForzarReinicioNavegador(int invocacionBajoDemanda)
        {
            var parametros = new Dictionary<string, object>();
            parametros.Add("@InvocacionBajoDemanda", invocacionBajoDemanda);
            List<System.Data.SqlClient.SqlParameter> parametrosOut = new List<System.Data.SqlClient.SqlParameter>();
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeOperativo", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var resultado = data.ExecuteProcedure("[dbo].[sp_vanti_Updater_EstatusForzarReinicioNavegador]", parametros, parametrosOut);
            return resultado["@MensajeOperativo"].ToString();
        }

        /// <summary>
        /// Ejecutar un script completo sobre la base de datos
        /// </summary>                
        public void EjecutarScriptDB(String script)
        {
            data.ExecuteDBScript(script);
        }

        /// <summary>
        /// Obtener la version actual del POS
        /// </summary>                
        public VersionActualPOS ObtenerVersionActual()
        {
            VersionActualPOS versionActualPOS = new VersionActualPOS();
            var parameters = new Dictionary<string, object>();
            foreach (var item in data.GetDataReader("[dbo].[sp_vanti_Updater_ObtenerVersionActualPOS]", parameters))
            {
                versionActualPOS.IdVersion = Convert.ToInt32(item.GetValue(0).ToString());
                versionActualPOS.EtiquetaLanzamientoVersionBase = item.GetValue(1).ToString();
            }
            return versionActualPOS;
        }

        /// <summary>
		/// Obtener Configuracion Servidor Actualizaciones
		/// </summary>
		/// <returns>Regresa una configuracion del servidor</returns>
        public ObtenerConfiguracionServidor ObtenerServidorActualizaciones()
        {
            ObtenerConfiguracionServidor configuracionServidor = new ObtenerConfiguracionServidor();
            var parameters = new Dictionary<string, object>();
            foreach (var item in data.GetDataReader("[dbo].[sp_vanti_Updater_ObtenerConfiguracionServidorActualizaciones]", parameters))
            {
                configuracionServidor.NombreServidorActualizaciones = item["NombreServidorActualizaciones"].ToString();
            }
            return configuracionServidor;
        }


        public String verficaSoftwareVersion(string posversion)
        {
            var parametros = new Dictionary<string, object>();
            parametros.Add("@posversionAngular", posversion);
            List<System.Data.SqlClient.SqlParameter> parametrosOut = new List<System.Data.SqlClient.SqlParameter>();
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@success", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@mensage", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var resultado = data.ExecuteProcedure("[dbo].[sp_VerificaSoftware]", parametros, parametrosOut);
            return resultado["@success"].ToString() + "," + resultado["@mensage"].ToString();
        }
    }
}
