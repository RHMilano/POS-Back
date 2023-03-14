using Milano.BackEnd.Business.General;
using Milano.BackEnd.Business.ImpresionMM;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Dto.InicioFinDia;
using Milano.BackEnd.Repository.Actualizador;
using Milano.BackEnd.Repository.InicioFinDia;
using System;
using System.IO;
using System.Net;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Milano.BackEnd.Dto.Actualizador;
using System.ServiceProcess;
using System.Security.Cryptography;
//using Ionic.Zip;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;

namespace Milano.BackEnd.Business.Actualizador
{
    /// <summary>
    /// 
    /// </summary>
    public class ActualizadorBusiness : BaseBusiness
    {

        // Directorios
        private const String DIRECTORIO_ROOT_OPERACION = @"C:\PosMilano";
        private const String DIRECTORIO_ROOT_RESPALDOS = @"C:\PosMilano\PUP_Backup";
        private const String DIRECTORIO_ROOT_DESCARGAS = @"C:\PosMilano\PUP_Download";
        private const String DIRECTORIO_ROOT_IIS_FRONT = @"C:\inetpub\wwwroot";

        // Archivos comprimidos
        private const String ARCHIVO_FRONT_COMPRIMIDO = "Front.zip";
        private const String ARCHIVO_SERVICIOS_COMPRIMIDO = "PosServiciosMilano.zip";
        private const String ARCHIVO_SEGURIDAD_COMPRIMIDO = "PosSeguridadMilano.zip";
        private const String ARCHIVO_SINCRONIZADOR_COMPRIMIDO = "SincronizadorMilano.zip";
        private const String ARCHIVO_PINPAD_COMPRIMIDO = "PinPadPagos.zip";
        private const String ARCHIVO_DATABASE_COMPRIMIDO = "Database.zip";

        // Directorios operativos confirmarlos
        private const String RUTA_FRONT = "Front";
        private const String RUTA_SERVICIOS = "PosServiciosMilano";
        private const String RUTA_SEGURIDAD = "PosSeguridadMilano";
        private const String RUTA_SINCRONIZADOR = "SincronizadorMilano";
        private const String RUTA_PINPAD = "PinPadPagos";
        private const String RUTA_DATABASE = "Database";
        private const String RUTA_DATABASE_UPDATE = "Update";

        // Información Updater
        private const String RUTA_UPDATER = "UpdaterMilano";
        private const String ARCHIVO_UPDATER_OPEN = "Open.bat";
        private const String ARCHIVO_UPDATER_CLOSE = "Close.bat";

        // Repositorio correspondiente
        private ActualizadorRepository actualizadorRepository;

        /// <summary>
        /// Metodo constructor que crea una instancia de su repositorio
        /// </summary>
        public ActualizadorBusiness()
        {
            actualizadorRepository = new ActualizadorRepository();
        }

        /// <summary>
        /// Metodo que comprueba la versión actual del Software y hacer validaciones para realizar el Update
        /// </summary>       
        /// <returns>Objeto respuesta</returns>
        public ResponseBussiness<ProcesoActualizacionSoftwareResponse> ActualizarVersionSoftware(String idVersionMaximaActualizacion, int invocacionBajoDemanda)
        {
            return tryCatch.SafeExecutor(() =>
            {
                ActualizacionSoftwareResponse actualizacionSoftwareResponse = new ActualizacionSoftwareResponse();
                ProcesoActualizacionSoftwareResponse procesoActualizacionSoftwareResponse = new ProcesoActualizacionSoftwareResponse();
                actualizacionSoftwareResponse = this.ComprobarVersionSoftwareActual(Convert.ToInt32(idVersionMaximaActualizacion), invocacionBajoDemanda);
                return this.ActualizarVersionSoftware(actualizacionSoftwareResponse.InformacionVersionesSoftwarePendientesPorInstalar);
            });
        }

        /// <summary>
        /// Metodo que comprueba la versión actual del Software y hacer validaciones para realizar el Update
        /// </summary>       
        /// <returns>Objeto respuesta</returns>
        public ResponseBussiness<ActualizacionSoftwareResponse> ComprobarVersionSoftwareActual(int idVersionMaximaActualizacion, int invocacionBajoDemanda)
        {
            return tryCatch.SafeExecutor(() =>
            {
                ActualizacionSoftwareResponse actualizacionSoftwareResponse = actualizadorRepository.
                ValidarExistenciaActualizacionesPendientes(idVersionMaximaActualizacion, invocacionBajoDemanda);
                // Si existen actualizaciones pendientes, ir por la lista de ellas
                if (actualizacionSoftwareResponse.CodeNumber.Equals("903"))
                {
                    actualizacionSoftwareResponse.InformacionVersionesSoftwarePendientesPorInstalar = actualizadorRepository.ObtenerListaActualizacionesPendientes(
                        actualizacionSoftwareResponse.IdVersionBase, actualizacionSoftwareResponse.IdVersionMaximaActualizacion);
                }
                return actualizacionSoftwareResponse;
            });
        }

        /// <summary>
        /// Metodo que devuelve el estao de un proceso de actualización en curso en caso de existir
        /// </summary>       
        /// <returns>Objeto resultado del proceso</returns>
        public ResponseBussiness<EstatusActualizacionSoftwareResponse> ObtenerEstatusProcesoActualizacionEnCurso(int invocacionBajoDemanda)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return actualizadorRepository.ObtenerEstatusProcesoActualizacionEnCurso(invocacionBajoDemanda);
            });
        }

        /// <summary>
        /// Metodo que devuelve el estado de la bandera de reinicio del navegador
        /// </summary>       
        /// <returns>Objeto resultado del proceso</returns>
        public ResponseBussiness<String> ObtenerEstatusReinicioNavegador()
        {
            return tryCatch.SafeExecutor(() =>
            {
                return actualizadorRepository.ObtenerEstatusReinicioNavegador();
            });
        }

        /// <summary>
        /// Metodo que actualiza el estado de la bandera de reinicio del navegador
        /// </summary>       
        /// <returns>Objeto resultado del proceso</returns>
        public ResponseBussiness<String> ActualizarEstatusReinicioNavegador()
        {
            return tryCatch.SafeExecutor(() =>
            {
                return actualizadorRepository.ActualizarEstatusReinicioNavegador();
            });
        }

        /// <summary>
        /// Metodo que regresa informació sobre si debeforzarse un reinicio del navegador
        /// </summary>       
        /// <returns>Objeto resultado del proceso</returns>
        public ResponseBussiness<String> EstatusForzarReinicioNavegador(int invocacionBajoDemanda)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return actualizadorRepository.EstatusForzarReinicioNavegador(invocacionBajoDemanda);
            });
        }

        private enum EstatusProcesoActualizacion
        {
            Iniciado, Validando, Respaldando, Descargando, ActualizandoArchivos, ActualizandoBaseDeDatos, Finalizado, ErrorNoGrave, ErrorGrave
        }

        private enum EstatusMetainformacionActualizacion
        {
            Pendiente, Aplicado, Error
        }

        /// <summary>
        /// Metodo que genera peticiones de actualización de acuerdo a lo solicitado
        /// </summary>       
        /// <returns>ProcesoActualizacionSoftwareResponse</returns>
        public ResponseBussiness<ProcesoActualizacionSoftwareResponse> ActualizarVersionSoftware(InformacionVersionSoftware[] versionesSoftwareSolicitadas)
        {
            return tryCatch.SafeExecutor(() =>
            {
                // Enviar peticiones de procesamiento de actualizaciones
                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (var versionSoftwareSolicitada in versionesSoftwareSolicitadas)
                    {
                        actualizadorRepository.GenerarPeticionActualizacion(versionSoftwareSolicitada.IdVersion, EstatusMetainformacionActualizacion.Pendiente.ToString());
                    }
                    scope.Complete();
                }

                // Se recibe una petición exitosa
                ProcesoActualizacionSoftwareResponse procesoActualizacionSoftwareResponse = new ProcesoActualizacionSoftwareResponse();
                procesoActualizacionSoftwareResponse.CodeNumber = "911";
                procesoActualizacionSoftwareResponse.CodeDescription = "Se han iniciado un proceso de actualización exitosamente";
                return procesoActualizacionSoftwareResponse;
            });
        }

        /// <summary>
        /// Metodo principal que realiza el proceso de actualización de Software de acuerdo a las peticiones de actualización solicitadas
        /// </summary>               
        public void ProcesarPeticionesPendientesActualizacionSoftware()
        {

            try
            {
                // Validar si debe continuar con la ejecución del flujo normal 
                String mensajeContinuarFlujoEjecucion = this.EstatusForzarReinicioNavegador(1);
                if ((mensajeContinuarFlujoEjecucion.Equals("CONTINUAR_ACTUALIZACION")) || (mensajeContinuarFlujoEjecucion.Equals("FORZAR_ACTUALIZACION")))
                {
                    // Validar que no se encuentre un proceso de actualización ejecutándose
                    EstatusActualizacionSoftwareResponse estatusActualizacionSoftwareResponse = this.ObtenerEstatusProcesoActualizacionEnCurso(1);

                    if (estatusActualizacionSoftwareResponse.ProcesoActualizacionEnCurso == 0)
                    {
                        // Traer el IdVersion máximo solicitado si existe
                        int idVersionMaximaActualizacionPedida = actualizadorRepository.ObtenerIdVersionMaximaActualizacionPedida();

                        if (idVersionMaximaActualizacionPedida != -1)
                        {
                            // Procesar las actualizaciones solicitadas
                            ActualizacionSoftwareResponse actualizacionSoftwareResponse = this.ComprobarVersionSoftwareActual(idVersionMaximaActualizacionPedida, 1);

                            foreach (var versionSoftwareSolicitada in actualizacionSoftwareResponse.InformacionVersionesSoftwarePendientesPorInstalar)
                            {
                                // Variables de control
                                String errorDB = "";
                                ValidationResponse validationResponseLocal = null;

                                // Comprobar que no se trata de un codigo 902. Un 902 indica que no puede procesarse la solicitud por obsolecencia
                                if (actualizacionSoftwareResponse.CodeNumber.Equals("902"))
                                {
                                    break;
                                }

                                // Iniciar el proceso
                                actualizadorRepository.ActualizarEstatusProcesoActualizacionEnCurso(versionSoftwareSolicitada.IdVersion,
                                    EstatusProcesoActualizacion.Iniciado.ToString(), "Iniciando un nuevo proceso de actualización", "En Proceso...");

                                // --------- PASO 1. VALIDAR CONECTIVIDAD CON EL SERVIDOR DE ACTUALIZACIONES
                                actualizadorRepository.ActualizarEstatusProcesoActualizacionEnCurso(versionSoftwareSolicitada.IdVersion,
                                    EstatusProcesoActualizacion.Validando.ToString(), "Validando que exista conectividad para descargar actualizaciones", "En Proceso...");
                                validationResponseLocal = this.ValidarConexionServidorActualizaciones();
                                if (validationResponseLocal.CodeNumber.Equals("-1"))
                                {
                                    actualizadorRepository.ActualizarEstatusProcesoActualizacionEnCurso(versionSoftwareSolicitada.IdVersion,
                                    EstatusProcesoActualizacion.ErrorNoGrave.ToString(), "Validando que exista conectividad para descargar actualizaciones", validationResponseLocal.CodeDescription);
                                    break;
                                }

                                // --------- PASO 2. RESPALDAR SISTEMA DE ARCHIVOS ACTUAL
                                actualizadorRepository.ActualizarEstatusProcesoActualizacionEnCurso(versionSoftwareSolicitada.IdVersion,
                                    EstatusProcesoActualizacion.Respaldando.ToString(), "Respaldando la versión actual del POS", "En Proceso...");
                                validationResponseLocal = this.RespaldarArchivosActualesPOS(versionSoftwareSolicitada.IdentificadorRepositorio);
                                if (validationResponseLocal.CodeNumber.Equals("-1"))
                                {
                                    actualizadorRepository.ActualizarEstatusProcesoActualizacionEnCurso(versionSoftwareSolicitada.IdVersion,
                                    EstatusProcesoActualizacion.ErrorNoGrave.ToString(), "Respaldando la versión actual del POS", validationResponseLocal.CodeDescription);
                                    break;
                                }

                                // --------- PASO 3. DESCARGAR ACTUALIZACIONES
                                actualizadorRepository.ActualizarEstatusProcesoActualizacionEnCurso(versionSoftwareSolicitada.IdVersion,
                                    EstatusProcesoActualizacion.Descargando.ToString(), "Descargando las nuevas actualizaciones", "En Proceso...");
                                validationResponseLocal = this.DescargarArchivosActualizacionPOS(versionSoftwareSolicitada.IdentificadorRepositorio);
                                if (validationResponseLocal.CodeNumber.Equals("-1"))
                                {
                                    actualizadorRepository.ActualizarEstatusProcesoActualizacionEnCurso(versionSoftwareSolicitada.IdVersion,
                                    EstatusProcesoActualizacion.ErrorNoGrave.ToString(), "Descargando las nuevas actualizaciones", validationResponseLocal.CodeDescription);
                                    break;
                                }

                                // --------- PASO 4. ACTUALIZAR EL SISTEMA DE ARCHIVOS
                                actualizadorRepository.ActualizarEstatusProcesoActualizacionEnCurso(versionSoftwareSolicitada.IdVersion,
                                    EstatusProcesoActualizacion.ActualizandoArchivos.ToString(), "Actualizando el sistema de archivos", "En Proceso...");
                                validationResponseLocal = this.ActualizarSistemaArchivosPOS(versionSoftwareSolicitada.IdentificadorRepositorio);
                                if (validationResponseLocal.CodeNumber.Equals("-1"))
                                {
                                    actualizadorRepository.ActualizarEstatusProcesoActualizacionEnCurso(versionSoftwareSolicitada.IdVersion,
                                    EstatusProcesoActualizacion.ErrorGrave.ToString(), "Actualizando el sistema de archivos", validationResponseLocal.CodeDescription);
                                    break;
                                }

                                // --------- PASO 5. ACTUALIZAR LA BASE DE DATOS
                                actualizadorRepository.ActualizarEstatusProcesoActualizacionEnCurso(versionSoftwareSolicitada.IdVersion,
                                    EstatusProcesoActualizacion.ActualizandoBaseDeDatos.ToString(), "Actualizando la base de datos", "En Proceso...");
                                try
                                {
                                    // Ejecutar el Script de actualización 
                                    String pathDirectorioScriptsUpdate = Path.Combine(new string[] { DIRECTORIO_ROOT_DESCARGAS, versionSoftwareSolicitada.IdentificadorRepositorio, RUTA_DATABASE, RUTA_DATABASE, RUTA_DATABASE_UPDATE });
                                    validationResponseLocal = this.EjecutarScriptsSQLEnDirectorio(pathDirectorioScriptsUpdate);
                                    if (validationResponseLocal.CodeNumber.Equals("-1"))
                                    {
                                        throw new Exception(validationResponseLocal.CodeDescription);
                                    }
                                    actualizadorRepository.ActualizarEstatusProcesoActualizacionEnCurso(versionSoftwareSolicitada.IdVersion,
                                            EstatusProcesoActualizacion.Finalizado.ToString(), "La versión ha sido actualizada correctamente", "OK");

                                    // Limpiar los archivos que ya no son necesarios
                                    validationResponseLocal = this.EliminarArchivosDescargadosRespaldoPOS(versionSoftwareSolicitada.IdentificadorRepositorio);
                                    if (validationResponseLocal.CodeNumber.Equals("-1"))
                                    {
                                        actualizadorRepository.ActualizarEstatusProcesoActualizacionEnCurso(versionSoftwareSolicitada.IdVersion,
                                        EstatusProcesoActualizacion.Finalizado.ToString(), "La versión ha sido actualizada correctamente. No ha sido posible hacer la limpieza de los archivos que ya no se utilizan", validationResponseLocal.CodeDescription);
                                    }
                                }
                                catch (Exception exceptionOuter)
                                {
                                    // Ejecutar Rollback de cambios
                                    try
                                    {
                                        errorDB = "ERROR_UPDATE: " + exceptionOuter.ToString();

                                        // Ejecutar Rollback del Sistema de Archivos
                                        validationResponseLocal = this.RollBackSistemaArchivosPOS(versionSoftwareSolicitada.IdentificadorRepositorio);
                                        if (validationResponseLocal.CodeNumber.Equals("-1"))
                                        {
                                            actualizadorRepository.ActualizarEstatusProcesoActualizacionEnCurso(versionSoftwareSolicitada.IdVersion,
                                            EstatusProcesoActualizacion.ErrorGrave.ToString(), "Ha ocurrido un error grave en la actualización. Contactar a Soporte", errorDB + " " + validationResponseLocal.CodeDescription);
                                            break;
                                        }

                                        // Actualizar el estatus de la actualización
                                        actualizadorRepository.ActualizarEstatusProcesoActualizacionEnCurso(versionSoftwareSolicitada.IdVersion,
                                            EstatusProcesoActualizacion.ErrorGrave.ToString(), "Ha ocurrido un error grave en la actualización provocado por Base de Datos. Se hizo rollback del sistema de archivos. Contactar a Soporte", errorDB);
                                    }
                                    catch (Exception exceptionInner)
                                    {
                                        errorDB += " ERROR_ROLLBACK: " + exceptionInner.ToString();
                                        actualizadorRepository.ActualizarEstatusProcesoActualizacionEnCurso(versionSoftwareSolicitada.IdVersion,
                                            EstatusProcesoActualizacion.ErrorGrave.ToString(), "ROLLBACK. Ha ocurrido un error grave en la actualización. Contactar a Soporte", errorDB);
                                        break;
                                    }
                                }
                            }

                        }
                    }
                }

            }
            catch (Exception exception)
            {
                TryCatchBusinessExecutor tryCatch = new TryCatchBusinessExecutor();
                tryCatch.AddErrorLog<OperationResponse>(exception.Message, exception.StackTrace, "POSUpdater", exception.ToString(), "Error Inesperado POSUpdater");
            }

        }

        private ValidationResponse EjecutarScriptsSQLEnDirectorio(String pathDirectorioDatabase)
        {
            try
            {
                // Verificar si el directorio existe
                if (Directory.Exists(pathDirectorioDatabase))
                {
                    // Obtener los scripts SQL disponibles
                    string[] files = Directory.GetFiles(pathDirectorioDatabase, "*.sql", SearchOption.AllDirectories);
                    if (null == files || files.Length == 0)
                    {
                        return new ValidationResponse("1", "OK. No existen archvios .SQL que deban ejecutarse");
                    }

                    // Ejecutar los scripts SQL disponibles
                    var regexGO = new Regex(Environment.NewLine + "GO", RegexOptions.IgnoreCase);
                    var regexANSIOn = new Regex("SET ANSI_NULLS ON", RegexOptions.IgnoreCase);
                    var regexANSIOff = new Regex("SET ANSI_NULLS OFF", RegexOptions.IgnoreCase);
                    var regexQUOTEDOn = new Regex("SET QUOTED_IDENTIFIER ON", RegexOptions.IgnoreCase);
                    var regexQUOTEDOff = new Regex("SET QUOTED_IDENTIFIER OFF", RegexOptions.IgnoreCase);
                    using (TransactionScope scope = new TransactionScope())
                    {
                        foreach (string nombreCompletoArchivo in files)
                        {
                            string contenido = File.ReadAllText(nombreCompletoArchivo);
                            contenido = regexGO.Replace(contenido, string.Empty);
                            contenido = regexANSIOn.Replace(contenido, string.Empty);
                            contenido = regexANSIOff.Replace(contenido, string.Empty);
                            contenido = regexQUOTEDOn.Replace(contenido, string.Empty);
                            contenido = regexQUOTEDOff.Replace(contenido, string.Empty);
                            actualizadorRepository.EjecutarScriptDB(contenido);
                        }
                        scope.Complete();
                    }
                    return new ValidationResponse("1", "OK");
                }
                else
                {
                    return new ValidationResponse("1", "OK. No existen archvios .SQL que deban ejecutarse");
                }
            }
            catch (Exception exception)
            {
                // Regresar el error correspondiente
                return new ValidationResponse("-1", "ERROR_UPDATE: " + exception);
            }
        }

        private ValidationResponse EliminarArchivosDescargadosRespaldoPOS(String identificadorRepositorio)
        {
            try
            {
                // Variables de instrucción
                ValidationResponse validationResponseLocal = null;
                String pathDirectorio = "";

                // Eliminar contenidos FRONT                
                pathDirectorio = Path.Combine(new string[] { DIRECTORIO_ROOT_DESCARGAS, identificadorRepositorio, RUTA_FRONT });
                validationResponseLocal = this.EliminarContenidoCompletoDirectorio(pathDirectorio);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }
                pathDirectorio = Path.Combine(new string[] { DIRECTORIO_ROOT_RESPALDOS, identificadorRepositorio, RUTA_FRONT });
                validationResponseLocal = this.EliminarContenidoCompletoDirectorio(pathDirectorio);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }

                // Eliminar contenidos SERVICIOS
                pathDirectorio = Path.Combine(new string[] { DIRECTORIO_ROOT_DESCARGAS, identificadorRepositorio, RUTA_SERVICIOS });
                validationResponseLocal = this.EliminarContenidoCompletoDirectorio(pathDirectorio);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }
                pathDirectorio = Path.Combine(new string[] { DIRECTORIO_ROOT_RESPALDOS, identificadorRepositorio, RUTA_SERVICIOS });
                validationResponseLocal = this.EliminarContenidoCompletoDirectorio(pathDirectorio);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }

                // Eliminar contenidos SEGURIDAD
                pathDirectorio = Path.Combine(new string[] { DIRECTORIO_ROOT_DESCARGAS, identificadorRepositorio, RUTA_SEGURIDAD });
                validationResponseLocal = this.EliminarContenidoCompletoDirectorio(pathDirectorio);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }
                pathDirectorio = Path.Combine(new string[] { DIRECTORIO_ROOT_RESPALDOS, identificadorRepositorio, RUTA_SEGURIDAD });
                validationResponseLocal = this.EliminarContenidoCompletoDirectorio(pathDirectorio);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }

                // Eliminar contenidos SINCRONIZADOR
                pathDirectorio = Path.Combine(new string[] { DIRECTORIO_ROOT_DESCARGAS, identificadorRepositorio, RUTA_SINCRONIZADOR });
                validationResponseLocal = this.EliminarContenidoCompletoDirectorio(pathDirectorio);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }
                pathDirectorio = Path.Combine(new string[] { DIRECTORIO_ROOT_RESPALDOS, identificadorRepositorio, RUTA_SINCRONIZADOR });
                validationResponseLocal = this.EliminarContenidoCompletoDirectorio(pathDirectorio);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }

                // Eliminar contenidos PINPAD
                pathDirectorio = Path.Combine(new string[] { DIRECTORIO_ROOT_DESCARGAS, identificadorRepositorio, RUTA_PINPAD });
                validationResponseLocal = this.EliminarContenidoCompletoDirectorio(pathDirectorio);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }
                pathDirectorio = Path.Combine(new string[] { DIRECTORIO_ROOT_RESPALDOS, identificadorRepositorio, RUTA_PINPAD });
                validationResponseLocal = this.EliminarContenidoCompletoDirectorio(pathDirectorio);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }

                // Eliminar contenidos DATABASE
                pathDirectorio = Path.Combine(new string[] { DIRECTORIO_ROOT_DESCARGAS, identificadorRepositorio, RUTA_DATABASE });
                validationResponseLocal = this.EliminarContenidoCompletoDirectorio(pathDirectorio);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }
                pathDirectorio = Path.Combine(new string[] { DIRECTORIO_ROOT_RESPALDOS, identificadorRepositorio, RUTA_DATABASE });
                validationResponseLocal = this.EliminarContenidoCompletoDirectorio(pathDirectorio);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }

                return new ValidationResponse("1", "OK");
            }
            catch (Exception exception)
            {
                return new ValidationResponse("-1", "ERROR_UPDATE: " + exception);
            }
        }

        private ValidationResponse RollBackSistemaArchivosPOS(String identificadorRepositorio)
        {
            try
            {
                // Variables de instrucción
                ValidationResponse validationResponseLocal = null;
                String pathOrigen = "";
                String pathDestino = "";

                // Rollback FRONT
                pathOrigen = Path.Combine(new string[] { DIRECTORIO_ROOT_RESPALDOS, identificadorRepositorio, RUTA_FRONT });
                pathDestino = DIRECTORIO_ROOT_IIS_FRONT;
                // Eliminar antes el contenido existente                
                validationResponseLocal = this.EliminarContenidoCompletoDirectorio(pathDestino);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }
                if (validationResponseLocal.CodeNumber.Equals("1"))
                {
                    // Se procede a actualizar el directorio
                    validationResponseLocal = this.CopiarContenidoCompletoDirectorio(pathOrigen, pathDestino);
                    if (validationResponseLocal.CodeNumber.Equals("-1"))
                    {
                        return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                    }
                }

                // Rollback SERVICIOS
                pathOrigen = Path.Combine(new string[] { DIRECTORIO_ROOT_RESPALDOS, identificadorRepositorio, RUTA_SERVICIOS });
                pathDestino = Path.Combine(new string[] { DIRECTORIO_ROOT_OPERACION, RUTA_SERVICIOS });
                // Eliminar antes el contenido existente                
                validationResponseLocal = this.EliminarContenidoCompletoDirectorio(pathDestino);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }
                if (validationResponseLocal.CodeNumber.Equals("1"))
                {
                    // Se procede a actualizar el directorio
                    validationResponseLocal = this.CopiarContenidoCompletoDirectorio(pathOrigen, pathDestino);
                    if (validationResponseLocal.CodeNumber.Equals("-1"))
                    {
                        return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                    }
                }

                // Rollback SEGURIDAD
                pathOrigen = Path.Combine(new string[] { DIRECTORIO_ROOT_RESPALDOS, identificadorRepositorio, RUTA_SEGURIDAD });
                pathDestino = Path.Combine(new string[] { DIRECTORIO_ROOT_OPERACION, RUTA_SEGURIDAD });
                // Eliminar antes el contenido existente                
                validationResponseLocal = this.EliminarContenidoCompletoDirectorio(pathDestino);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }
                if (validationResponseLocal.CodeNumber.Equals("1"))
                {
                    // Se procede a actualizar el directorio
                    validationResponseLocal = this.CopiarContenidoCompletoDirectorio(pathOrigen, pathDestino);
                    if (validationResponseLocal.CodeNumber.Equals("-1"))
                    {
                        return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                    }
                }

                // Rollback SINCRONIZADOR
                pathOrigen = Path.Combine(new string[] { DIRECTORIO_ROOT_RESPALDOS, identificadorRepositorio, RUTA_SINCRONIZADOR });
                pathDestino = Path.Combine(new string[] { DIRECTORIO_ROOT_OPERACION, RUTA_SINCRONIZADOR });
                validationResponseLocal = this.DetenerWindowsService("SincronizadorMilano", 20000);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }
                else
                {
                    // Eliminar antes el contenido existente                
                    validationResponseLocal = this.EliminarContenidoCompletoDirectorio(pathDestino);
                    if (validationResponseLocal.CodeNumber.Equals("-1"))
                    {
                        return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                    }
                    if (validationResponseLocal.CodeNumber.Equals("1"))
                    {
                        // Se procede a actualizar el directorio
                        validationResponseLocal = this.CopiarContenidoCompletoDirectorio(pathOrigen, pathDestino);
                        if (validationResponseLocal.CodeNumber.Equals("-1"))
                        {
                            return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                        }
                        else
                        {
                            validationResponseLocal = this.IniciarWindowsService("SincronizadorMilano", 20000);
                            if (validationResponseLocal.CodeNumber.Equals("-1"))
                            {
                                return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                            }
                        }
                    }
                }

                // Rollback PINPAD
                pathOrigen = Path.Combine(new string[] { DIRECTORIO_ROOT_RESPALDOS, identificadorRepositorio, RUTA_PINPAD });
                pathDestino = Path.Combine(new string[] { DIRECTORIO_ROOT_OPERACION, RUTA_PINPAD });
                validationResponseLocal = this.DetenerWindowsService("ServicioPinPad", 20000);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }
                else
                {
                    // Eliminar antes el contenido existente                
                    validationResponseLocal = this.EliminarContenidoCompletoDirectorio(pathDestino);
                    if (validationResponseLocal.CodeNumber.Equals("-1"))
                    {
                        return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                    }
                    if (validationResponseLocal.CodeNumber.Equals("1"))
                    {
                        // Se procede a actualizar el directorio
                        validationResponseLocal = this.CopiarContenidoCompletoDirectorio(pathOrigen, pathDestino);
                        if (validationResponseLocal.CodeNumber.Equals("-1"))
                        {
                            return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                        }
                        else
                        {
                            validationResponseLocal = this.IniciarWindowsService("ServicioPinPad", 20000);
                            if (validationResponseLocal.CodeNumber.Equals("-1"))
                            {
                                return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                            }
                        }
                    }
                }

                return new ValidationResponse("1", "OK");
            }
            catch (Exception exception)
            {
                // Regresar el error correspondiente
                return new ValidationResponse("-1", "ERROR_UPDATE: " + exception);
            }
        }

        private ValidationResponse ActualizarSistemaArchivosPOS(String identificadorRepositorio)
        {
            try
            {
                // Variables de instrucción
                ValidationResponse validationResponseLocal = null;
                String pathArchivoComprimido = "";
                String pathOrigen = "";
                String pathDestino = "";

                // ---- Actualizar FRONT
                // Descomprimir archivo
                pathArchivoComprimido = Path.Combine(new string[] { DIRECTORIO_ROOT_DESCARGAS, identificadorRepositorio, RUTA_FRONT, ARCHIVO_FRONT_COMPRIMIDO });
                pathDestino = Path.Combine(new string[] { DIRECTORIO_ROOT_DESCARGAS, identificadorRepositorio, RUTA_FRONT, RUTA_FRONT });
                validationResponseLocal = this.DescomprimirArchivoZip(pathArchivoComprimido, pathDestino);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }
                if (validationResponseLocal.CodeNumber.Equals("1"))
                {
                    // Actualizar directorio con nueva versión si el archivo ZIP existe
                    pathOrigen = pathDestino;
                    pathDestino = DIRECTORIO_ROOT_IIS_FRONT;
                    // Para FRONT es necesario eliminar antes el contenido existente
                    validationResponseLocal = this.EliminarContenidoCompletoDirectorio(pathDestino);
                    if (validationResponseLocal.CodeNumber.Equals("-1"))
                    {
                        return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                    }
                    if (validationResponseLocal.CodeNumber.Equals("1"))
                    {
                        // Se procede a actualizar el directorio
                        validationResponseLocal = this.CopiarContenidoCompletoDirectorio(pathOrigen, pathDestino);
                        if (validationResponseLocal.CodeNumber.Equals("-1"))
                        {
                            return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                        }
                    }
                }

                // ---- Actualizar SERVICIOS
                // Descomprimir archivo
                pathArchivoComprimido = Path.Combine(new string[] { DIRECTORIO_ROOT_DESCARGAS, identificadorRepositorio, RUTA_SERVICIOS, ARCHIVO_SERVICIOS_COMPRIMIDO });
                pathDestino = Path.Combine(new string[] { DIRECTORIO_ROOT_DESCARGAS, identificadorRepositorio, RUTA_SERVICIOS, RUTA_SERVICIOS });
                validationResponseLocal = this.DescomprimirArchivoZip(pathArchivoComprimido, pathDestino);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }
                if (validationResponseLocal.CodeNumber.Equals("1"))
                {
                    // Actualizar directorio con nueva versión si el archivo ZIP existe
                    pathOrigen = pathDestino;
                    pathDestino = Path.Combine(new string[] { DIRECTORIO_ROOT_OPERACION, RUTA_SERVICIOS }); ;
                    validationResponseLocal = this.CopiarContenidoCompletoDirectorio(pathOrigen, pathDestino);
                    if (validationResponseLocal.CodeNumber.Equals("-1"))
                    {
                        return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                    }
                }

                // ---- Actualizar SEGURIDAD
                // Descomprimir archivo
                pathArchivoComprimido = Path.Combine(new string[] { DIRECTORIO_ROOT_DESCARGAS, identificadorRepositorio, RUTA_SEGURIDAD, ARCHIVO_SEGURIDAD_COMPRIMIDO });
                pathDestino = Path.Combine(new string[] { DIRECTORIO_ROOT_DESCARGAS, identificadorRepositorio, RUTA_SEGURIDAD, RUTA_SEGURIDAD });
                validationResponseLocal = this.DescomprimirArchivoZip(pathArchivoComprimido, pathDestino);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }
                if (validationResponseLocal.CodeNumber.Equals("1"))
                {
                    // Actualizar directorio con nueva versión si el archivo ZIP existe
                    pathOrigen = pathDestino;
                    pathDestino = Path.Combine(new string[] { DIRECTORIO_ROOT_OPERACION, RUTA_SEGURIDAD }); ;
                    validationResponseLocal = this.CopiarContenidoCompletoDirectorio(pathOrigen, pathDestino);
                    if (validationResponseLocal.CodeNumber.Equals("-1"))
                    {
                        return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                    }
                }

                // ---- Actualizar SINCRONIZADOR
                // Descomprimir archivo
                pathArchivoComprimido = Path.Combine(new string[] { DIRECTORIO_ROOT_DESCARGAS, identificadorRepositorio, RUTA_SINCRONIZADOR, ARCHIVO_SINCRONIZADOR_COMPRIMIDO });
                pathDestino = Path.Combine(new string[] { DIRECTORIO_ROOT_DESCARGAS, identificadorRepositorio, RUTA_SINCRONIZADOR, RUTA_SINCRONIZADOR });
                validationResponseLocal = this.DescomprimirArchivoZip(pathArchivoComprimido, pathDestino);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }
                if (validationResponseLocal.CodeNumber.Equals("1"))
                {
                    // Actualizar directorio con nueva versión si el archivo ZIP existe
                    pathOrigen = pathDestino;
                    pathDestino = Path.Combine(new string[] { DIRECTORIO_ROOT_OPERACION, RUTA_SINCRONIZADOR });
                    validationResponseLocal = this.DetenerWindowsService("SincronizadorMilano", 20000);
                    if (validationResponseLocal.CodeNumber.Equals("-1"))
                    {
                        return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                    }
                    else
                    {
                        validationResponseLocal = this.CopiarContenidoCompletoDirectorio(pathOrigen, pathDestino);
                        if (validationResponseLocal.CodeNumber.Equals("-1"))
                        {
                            return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                        }
                        else
                        {
                            validationResponseLocal = this.IniciarWindowsService("SincronizadorMilano", 20000);
                            if (validationResponseLocal.CodeNumber.Equals("-1"))
                            {
                                return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                            }
                        }
                    }
                }

                // ---- Actualizar PINPAD
                // Descomprimir archivo
                pathArchivoComprimido = Path.Combine(new string[] { DIRECTORIO_ROOT_DESCARGAS, identificadorRepositorio, RUTA_PINPAD, ARCHIVO_PINPAD_COMPRIMIDO });
                pathDestino = Path.Combine(new string[] { DIRECTORIO_ROOT_DESCARGAS, identificadorRepositorio, RUTA_PINPAD, RUTA_PINPAD });
                validationResponseLocal = this.DescomprimirArchivoZip(pathArchivoComprimido, pathDestino);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }
                if (validationResponseLocal.CodeNumber.Equals("1"))
                {
                    // Actualizar directorio con nueva versión si el archivo ZIP existe
                    pathOrigen = pathDestino;
                    pathDestino = Path.Combine(new string[] { DIRECTORIO_ROOT_OPERACION, RUTA_PINPAD });
                    validationResponseLocal = this.DetenerWindowsService("ServicioPinPad", 20000);
                    if (validationResponseLocal.CodeNumber.Equals("-1"))
                    {
                        return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                    }
                    else
                    {
                        validationResponseLocal = this.CopiarContenidoCompletoDirectorio(pathOrigen, pathDestino);
                        if (validationResponseLocal.CodeNumber.Equals("-1"))
                        {
                            return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                        }
                        else
                        {
                            validationResponseLocal = this.IniciarWindowsService("ServicioPinPad", 20000);
                            if (validationResponseLocal.CodeNumber.Equals("-1"))
                            {
                                return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                            }
                        }
                    }
                }

                // ---- Descomprimir archivo DATABASE                
                pathArchivoComprimido = Path.Combine(new string[] { DIRECTORIO_ROOT_DESCARGAS, identificadorRepositorio, RUTA_DATABASE, ARCHIVO_DATABASE_COMPRIMIDO });
                pathDestino = Path.Combine(new string[] { DIRECTORIO_ROOT_DESCARGAS, identificadorRepositorio, RUTA_DATABASE, RUTA_DATABASE });
                validationResponseLocal = this.DescomprimirArchivoZip(pathArchivoComprimido, pathDestino);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }

                return new ValidationResponse("1", "OK");
            }
            catch (Exception exception)
            {
                return new ValidationResponse("-1", "ERROR_UPDATE: " + exception);
            }
        }

        private ValidationResponse DescargarArchivosActualizacionPOS(String identificadorRepositorio)
        {
            try
            {
                // Variables de instrucción
                ObtenerConfiguracionServidor obtenerConfiguracionServidor = actualizadorRepository.ObtenerServidorActualizaciones();
                ValidationResponse validationResponseLocal = null;
                String urlOrigen = "";
                String pathDestino = "";
                String pathDescargas = "";

                // Eliminar antes el direcotrio general de descargas
                pathDescargas = Path.Combine(new string[] { DIRECTORIO_ROOT_DESCARGAS, identificadorRepositorio });
                validationResponseLocal = this.EliminarContenidoCompletoDirectorio(pathDescargas);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }

                // Descargar FRONT
                urlOrigen = obtenerConfiguracionServidor.NombreServidorActualizaciones + "/" + identificadorRepositorio + "/" + ARCHIVO_FRONT_COMPRIMIDO;
                pathDestino = Path.Combine(new string[] { DIRECTORIO_ROOT_DESCARGAS, identificadorRepositorio, RUTA_FRONT });
                validationResponseLocal = this.DescargarArchivo(urlOrigen, pathDestino);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }

                // Descargar SERVICIOS
                urlOrigen = obtenerConfiguracionServidor.NombreServidorActualizaciones + "/" + identificadorRepositorio + "/" + ARCHIVO_SERVICIOS_COMPRIMIDO;
                pathDestino = Path.Combine(new string[] { DIRECTORIO_ROOT_DESCARGAS, identificadorRepositorio, RUTA_SERVICIOS });
                validationResponseLocal = this.DescargarArchivo(urlOrigen, pathDestino);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }

                // Descargar SEGURIDAD
                urlOrigen = obtenerConfiguracionServidor.NombreServidorActualizaciones + "/" + identificadorRepositorio + "/" + ARCHIVO_SEGURIDAD_COMPRIMIDO;
                pathDestino = Path.Combine(new string[] { DIRECTORIO_ROOT_DESCARGAS, identificadorRepositorio, RUTA_SEGURIDAD });
                validationResponseLocal = this.DescargarArchivo(urlOrigen, pathDestino);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }

                // Descargar SINCRONIZADOR
                urlOrigen = obtenerConfiguracionServidor.NombreServidorActualizaciones + "/" + identificadorRepositorio + "/" + ARCHIVO_SINCRONIZADOR_COMPRIMIDO;
                pathDestino = Path.Combine(new string[] { DIRECTORIO_ROOT_DESCARGAS, identificadorRepositorio, RUTA_SINCRONIZADOR });
                validationResponseLocal = this.DescargarArchivo(urlOrigen, pathDestino);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }

                // Descargar PINPAD
                urlOrigen = obtenerConfiguracionServidor.NombreServidorActualizaciones + "/" + identificadorRepositorio + "/" + ARCHIVO_PINPAD_COMPRIMIDO;
                pathDestino = Path.Combine(new string[] { DIRECTORIO_ROOT_DESCARGAS, identificadorRepositorio, RUTA_PINPAD });
                validationResponseLocal = this.DescargarArchivo(urlOrigen, pathDestino);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }

                // Descargar DATABASE
                urlOrigen = obtenerConfiguracionServidor.NombreServidorActualizaciones + "/" + identificadorRepositorio + "/" + ARCHIVO_DATABASE_COMPRIMIDO;
                pathDestino = Path.Combine(new string[] { DIRECTORIO_ROOT_DESCARGAS, identificadorRepositorio, RUTA_DATABASE });
                validationResponseLocal = this.DescargarArchivo(urlOrigen, pathDestino);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }

                return new ValidationResponse("1", "OK");
            }
            catch (Exception exception)
            {
                return new ValidationResponse("-1", "ERROR_UPDATE: " + exception);
            }
        }

        private ValidationResponse RespaldarArchivosActualesPOS(String identificadorRepositorio)
        {
            try
            {
                // Variables de instrucción
                ValidationResponse validationResponseLocal = null;
                String pathOrigen = "";
                String pathDestino = "";

                // Respaldar FRONT
                pathOrigen = DIRECTORIO_ROOT_IIS_FRONT;
                pathDestino = Path.Combine(new string[] { DIRECTORIO_ROOT_RESPALDOS, identificadorRepositorio, RUTA_FRONT });
                validationResponseLocal = this.CopiarContenidoCompletoDirectorio(pathOrigen, pathDestino);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }

                // Respaldar SERVICIOS
                pathOrigen = Path.Combine(new string[] { DIRECTORIO_ROOT_OPERACION, RUTA_SERVICIOS });
                pathDestino = Path.Combine(new string[] { DIRECTORIO_ROOT_RESPALDOS, identificadorRepositorio, RUTA_SERVICIOS });
                validationResponseLocal = this.CopiarContenidoCompletoDirectorio(pathOrigen, pathDestino);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }

                // Respaldar SEGURIDAD
                pathOrigen = Path.Combine(new string[] { DIRECTORIO_ROOT_OPERACION, RUTA_SEGURIDAD });
                pathDestino = Path.Combine(new string[] { DIRECTORIO_ROOT_RESPALDOS, identificadorRepositorio, RUTA_SEGURIDAD });
                validationResponseLocal = this.CopiarContenidoCompletoDirectorio(pathOrigen, pathDestino);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }

                // Respaldar SINCRONIZADOR
                pathOrigen = Path.Combine(new string[] { DIRECTORIO_ROOT_OPERACION, RUTA_SINCRONIZADOR });
                pathDestino = Path.Combine(new string[] { DIRECTORIO_ROOT_RESPALDOS, identificadorRepositorio, RUTA_SINCRONIZADOR });
                validationResponseLocal = this.CopiarContenidoCompletoDirectorio(pathOrigen, pathDestino);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }

                // Respaldar PINPAD
                pathOrigen = Path.Combine(new string[] { DIRECTORIO_ROOT_OPERACION, RUTA_PINPAD });
                pathDestino = Path.Combine(new string[] { DIRECTORIO_ROOT_RESPALDOS, identificadorRepositorio, RUTA_PINPAD });
                validationResponseLocal = this.CopiarContenidoCompletoDirectorio(pathOrigen, pathDestino);
                if (validationResponseLocal.CodeNumber.Equals("-1"))
                {
                    return new ValidationResponse("-1", validationResponseLocal.CodeDescription);
                }

                return new ValidationResponse("1", "OK");
            }
            catch (Exception exception)
            {
                // Regresar el error correspondiente
                return new ValidationResponse("-1", "ERROR_UPDATE: " + exception);
            }
        }

        private ValidationResponse ValidarConexionServidorActualizaciones()
        {
            try
            {
                ObtenerConfiguracionServidor obtenerConfiguracionServidor = actualizadorRepository.ObtenerServidorActualizaciones();
                using (var client = new WebClient())
                {
                    using (client.OpenRead(obtenerConfiguracionServidor.NombreServidorActualizaciones))
                    {
                        return new ValidationResponse("1", "Si existe conectividad hacia el servidor de actualizaciones");
                    }
                }
            }
            catch (Exception exception)
            {
                // Regresar el error correspondiente
                return new ValidationResponse("-1", "ERROR_UPDATE: No existe conectividad hacia el servidor de actualizaciones. " + exception);
            }
        }

        private ValidationResponse CopiarContenidoCompletoDirectorio(String directorioOrigen, String directorioDestino)
        {
            try
            {
                //Crea el directorio destino si no existe
                if (!Directory.Exists(directorioDestino))
                {
                    Directory.CreateDirectory(directorioDestino);
                }
                // Copiar estructura de directorios
                foreach (string sourceSubFolder in Directory.GetDirectories(directorioOrigen, "*", SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(sourceSubFolder.Replace(directorioOrigen, directorioDestino));
                }
                // Copiar archivos
                foreach (string sourceFile in Directory.GetFiles(directorioOrigen, "*", SearchOption.AllDirectories))
                {
                    bool exito = false;
                    string destinationFile = sourceFile.Replace(directorioOrigen, directorioDestino);
                    while (!exito)
                    {
                        try
                        {
                            File.Copy(sourceFile, destinationFile, true);
                            exito = true;
                        }
                        catch (IOException exception)
                        {
                            if ((exception.ToString().Contains("file used by another process")) || (exception.ToString().Contains("siendo utilizado en otro proceso")))
                            {
                                exito = false;
                            }
                            else
                            {
                                exito = true;
                            }
                        }
                    }
                }
                return new ValidationResponse("1", "OK");
            }
            catch (Exception exception)
            {
                // Regresar el error correspondiente
                return new ValidationResponse("-1", "ERROR_UPDATE: " + exception);
            }
        }

        private ValidationResponse DescomprimirArchivoZip(String pathArchivo, String pathDirectorioDestino)
        {
            try
            {
                if (!File.Exists(pathArchivo))
                {
                    return new ValidationResponse("2", "OK. El archivo comprimido solicitado no existe porque no forma parte de la actualización");
                }
                if (!Directory.Exists(pathDirectorioDestino))
                {
                    Directory.CreateDirectory(pathDirectorioDestino);
                }
                //using (ZipFile zipFile = ZipFile.Read(pathArchivo))
                //{
                //    zipFile.ExtractAll(pathDirectorioDestino, ExtractExistingFileAction.OverwriteSilently);
                //}
                return new ValidationResponse("1", "OK");
            }
            catch (Exception exception)
            {
                // Regresar el error correspondiente
                return new ValidationResponse("-1", "ERROR_UPDATE: " + exception);
            }
        }

        private String CalcularHashMD5Archivo(String pathArchivo)
        {
            using (MD5 md5 = MD5.Create())
            {
                using (FileStream stream = File.OpenRead(pathArchivo))
                {
                    byte[] hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash);
                }
            }
        }

        private ValidationResponse EliminarContenidoCompletoDirectorio(String pathDirectorio)
        {
            try
            {
                if (!Directory.Exists(pathDirectorio))
                {
                    return new ValidationResponse("1", "OK. El directorio no existe, no existe nada que eliminar");
                }
                Directory.Delete(pathDirectorio, true);
                while (Directory.Exists(pathDirectorio))
                {
                    Thread.Sleep(100);
                }
                return new ValidationResponse("1", "OK");
            }
            catch (Exception exception)
            {
                // Regresar el error correspondiente
                return new ValidationResponse("-1", "ERROR_UPDATE: " + exception);
            }
        }

        private ValidationResponse DescargarArchivo(String urlArchivoDescarga, String directorioDestino)
        {
            try
            {
                // Crear el directorio destino si no existe
                if (!Directory.Exists(directorioDestino))
                {
                    Directory.CreateDirectory(directorioDestino);
                }

                // Descargar el archivo               
                using (WebClient webClient = new WebClient())
                {
                    Uri uriArchivoZip = new Uri(urlArchivoDescarga);
                    String nombreArchivoZip = Path.GetFileName(uriArchivoZip.LocalPath);
                    String pathArchivoZip = Path.Combine(directorioDestino, nombreArchivoZip);
                    webClient.DownloadFile(uriArchivoZip, pathArchivoZip);
                    return new ValidationResponse("1", "OK");
                }
            }
            catch (WebException exception)
            {
                if (exception.Status == WebExceptionStatus.ProtocolError)
                {
                    if (((HttpWebResponse)exception.Response).StatusCode == HttpStatusCode.NotFound)
                    {
                        return new ValidationResponse("1", "OK. El archivo no existe por lo tanto no forma parte de esta actualización");
                    }
                    else
                    {
                        return new ValidationResponse("-1", "ERROR_UPDATE: " + exception);
                    }
                }
                else
                {
                    return new ValidationResponse("-1", "ERROR_UPDATE: " + exception);
                }
            }
            catch (Exception exception)
            {
                return new ValidationResponse("-1", "ERROR_UPDATE: " + exception);
            }
        }

        private ValidationResponse IniciarWindowsService(string nombreServicio, int tiempoMilisegundos)
        {
            ServiceController servicio = new ServiceController(nombreServicio);
            try
            {
                if (this.ServicioExiste(nombreServicio, "localhost"))
                {
                    if (servicio.Status == ServiceControllerStatus.Stopped)
                    {
                        TimeSpan timeout = TimeSpan.FromMilliseconds(20000);
                        servicio.Start();
                        servicio.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    }
                }
                return new ValidationResponse("1", "OK");
            }
            catch (Exception exception)
            {
                // Regresar el error correspondiente
                return new ValidationResponse("-1", "ERROR_UPDATE: " + exception);
            }
        }

        private ValidationResponse DetenerWindowsService(string nombreServicio, int tiempoMilisegundos)
        {
            ServiceController servicio = new ServiceController(nombreServicio);
            try
            {
                if (this.ServicioExiste(nombreServicio, "localhost"))
                {
                    if (servicio.Status == ServiceControllerStatus.Running)
                    {
                        TimeSpan timeout = TimeSpan.FromMilliseconds(20000);
                        servicio.Stop();
                        servicio.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                    }
                }
                return new ValidationResponse("1", "OK");
            }
            catch (Exception exception)
            {
                // Regresar el error correspondiente
                return new ValidationResponse("-1", "ERROR_UPDATE: " + exception);
            }
        }

        private bool ServicioExiste(string nombreServicio, string nombreMaquina)
        {
            ServiceController[] services = ServiceController.GetServices(nombreMaquina);
            var service = services.FirstOrDefault(s => s.ServiceName == nombreServicio);
            return service != null;
        }

        /// <summary>
        /// Obtener Ultima Version del POS
        /// </summary>
        public ResponseBussiness<String> ObtenerVersionActual()
        {
            return tryCatch.SafeExecutor(() =>
            {
                VersionActualPOS versionActualPOS = actualizadorRepository.ObtenerVersionActual();
                return versionActualPOS.EtiquetaLanzamientoVersionBase;
            });
        }

        public ResponseBussiness<String> ObtenerVersionSoftware(string posversion)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return actualizadorRepository.verficaSoftwareVersion(posversion);
            });
        }

    }
}
