using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.Sales;
using Milano.BackEnd.Repository;
using Milano.BackEnd.Repository.Sincronizacion;
using Milano.BackEnd.Dto.Sincronizacion;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Configuration;

namespace Milano.BackEnd.Business.Sincronizacion
{

    /// <summary>
    /// Clase de negocio para el Motor de Sincronización
    /// </summary>
    public class SincronizacionBusiness : BaseBusiness
    {

        SincronizacionRepository repository;

        /// <summary>
        /// Constructor
        /// </summary>
        public SincronizacionBusiness()
        {
            repository = new SincronizacionRepository();
        }

        /// <summary>
        /// Ejecutar proceso de Sincronización
        /// </summary>
        private ResponseBussiness<int> SincronizarDatos()
        {
            return tryCatch.SafeExecutor(() =>
            {
                return this.repository.SincronizarDatos();
            });
        }

        /// <summary>
        /// Ejecutar proceso de Purga
        /// </summary>
        public ResponseBussiness<int> PurgarDatos()
        {
            return tryCatch.SafeExecutor(() =>
            {
                return this.repository.PurgarDatos();
            });
        }

        /// <summary>
        /// Obtener periodicidad proceso de Sincronización
        /// </summary>
        public ResponseBussiness<Periodicidad> ObtenerPeriodicidad()
        {
            return tryCatch.SafeExecutor(() =>
            {
                return this.repository.ObtenerPeriodicidad();
            });
        }

        /// <summary>
        /// Procesar una petición de sincronización
        /// </summary>
        public ResponseBussiness<ResultadoSincronizacion> ProcesarPeticionSincronizacion(SincronizacionRequest sincronizacionRequest)
        {
            ResultadoSincronizacion resultadoSincronizacion = new ResultadoSincronizacion();
            resultadoSincronizacion = this.repository.EjecutarSincronizacion(sincronizacionRequest);
            return resultadoSincronizacion;
        }

        /// <summary>
        /// Ejecutar una petición de sincronización
        /// </summary>
        public ResponseBussiness<int> SincronizarInformacion()
        {
            return tryCatch.SafeExecutor(() =>
            {
                InformacionSincronizador informacionSincronizador = repository.ObtenerInformacionDiscreparSincronizador();
                //informacionSincronizador.IdDestinoSiguienteProcesar = 0;
                if ((informacionSincronizador.CodigoCajaOrigen == 0) && (informacionSincronizador.IdDestinoSiguienteProcesar == 0))
                {
                    // Invoca a sincronizador especial para BOOFICINAS
                    return EnviarPeticionSincronizacion();
                }
                else
                {
                    // Invoca a sincronizador primera version (el de siempre)
                    return SincronizarDatos();
                }
            });
        }

        /// <summary>
        /// Ejecutar una petición de sincronización
        /// </summary>
        private ResponseBussiness<int> EnviarPeticionSincronizacion()
        {
            return tryCatch.SafeExecutor(() =>
            {
                // Leer la información que debe sincronizarse
                ResultadoSincronizacion resultadoSincronizacion = new ResultadoSincronizacion();
                SincronizacionRequest sincronizacionRequest = repository.ObtenerDatosSincronizacion();
                //String webServicePath = sincronizacionRequest.ServidorDestino + "/PosServiciosMIlano/Sincronizacion/Sincronizacionservice.svc/ejecutarProcesoSincronizacion";
                //String webServicePath = sincronizacionRequest.ServidorDestino + "/PosServiciosMIlano3/Sincronizacion/Sincronizacionservice.svc/ejecutarProcesoSincronizacion";
                String webServicePath = sincronizacionRequest.ServidorDestino;

                if (!String.IsNullOrEmpty(sincronizacionRequest.ServidorDestino))
                {
                    // ******************************* WS REMOTO
                    var webRequest = (HttpWebRequest)WebRequest.Create(webServicePath);
                    webRequest.Method = WebRequestMethods.Http.Post;
                    // Timeout de 30 minutos '1800000' a  5 miuntos '300000'
                    webRequest.Timeout = 300000;
                    webRequest.ContentType = "application/json";
                    var json = JsonConvert.SerializeObject(sincronizacionRequest);
                    try
                    {
                        // Hacer la petición al WS Remoto
                        using (var requestStream = webRequest.GetRequestStream())
                        {
                            using (var writer = new StreamWriter(requestStream))
                            {
                                writer.Write(json);
                            }
                        }

                        // Obtener la respuesta del WS Remoto
                        using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
                        {
                            using (var responseStream = webResponse.GetResponseStream())
                            {
                                using (var reader = new StreamReader(responseStream))
                                {
                                    var responseData = reader.ReadToEnd();
                                    dynamic resultadoSincronizacionWS = JsonConvert.DeserializeObject(responseData);
                                    resultadoSincronizacion.UltimoIdSincronizado = resultadoSincronizacionWS.data.ultimoIdSincronizado;
                                    resultadoSincronizacion.MensajeAsociado = resultadoSincronizacionWS.data.mensajeAsociado;
                                    resultadoSincronizacion.IdServidorDestino = resultadoSincronizacionWS.data.idServidorDestino;
                                    resultadoSincronizacion.ServidorDestino = resultadoSincronizacionWS.data.servidorDestino;
                                }
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        TryCatchBusinessExecutor tryCatch = new TryCatchBusinessExecutor();
                        tryCatch.AddErrorLog<OperationResponse>(exception.Message, exception.StackTrace, "Sincronización", exception.ToString(), "Error de sincronización");
                    }
                    // ******************************* WS REMOTO       

                    // Actualizar la información de tabla AUDITORIA_DESTINOS
                    repository.ActualizarAuditoriaDestinos(resultadoSincronizacion);

                   
                }

                GC.Collect();
                return 0;
            });
        }


        /// <summary>
        /// Ejecutar Proceso de respaldo
        /// </summary>
        public ResponseBussiness<int> ProcesoDeRespaldo()
        {
            return tryCatch.SafeExecutor(() =>
            {
                return this.repository.RespaldarDatos();
            });
        }

        /// <summary>
        /// Obtener periodicidad en minutos de respaldo
        /// </summary>
        public ResponseBussiness<int> ObtenerPeriodicidadRespaldo()
        {
            return tryCatch.SafeExecutor(() =>
            {
                return this.repository.ObtenerPeriodicidadRespaldo();
            });
        }

        /// <summary>
        /// Enviar informacion de maquina
        /// </summary>
        public ResponseBussiness<int> NotificarDetencionServicio()
        {
            return tryCatch.SafeExecutor(() =>
            {
                string msg = this.repository.getInfoDetencion();
                throw new Exception("Detencion. " + msg);
                return 1;
            });
        }


        /// <summary>
        /// Setear bandera
        /// </summary>
        public ResponseBussiness<int> setBandera()
        {
            return tryCatch.SafeExecutor(() =>
            {
                return this.repository.setBandera();
            });
        }

        /// <summary>
        /// actualizar version sincronizador
        /// </summary>
        public ResponseBussiness<int> setVersion(double version)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return this.repository.setVersion(version);
            });
        }

        /// <summary>
        /// Ejecutar mis pruebas
        /// </summary>
        public ResponseBussiness<int> PruebasAaron(string opt)
        {
            return tryCatch.SafeExecutor(() =>
            {
                this.repository.getPruebasAaron();
                //string path = @"C:\log";
                //string fileName = DateTime.Today.ToString("yyyyMMdd") + ".txt";
                //string fullpath = System.IO.Path.Combine(path, fileName);
                //FileInfo fi = new FileInfo(fullpath);

                //if (!Directory.Exists(path))
                //{
                //    Directory.CreateDirectory(path);
                //}

                //if (!fi.Exists)
                //{ 
                //    using (FileStream fs = fi.Create()) { }
                //}

                //string hora = DateTime.Now.ToString();
                //string output = string.Format("{0}: '{1}'", opt, hora);
                //File.AppendAllText(fullpath, string.Format("{0}{1}", output, Environment.NewLine));
                return 1;
            });
        }

        public void generar(string write)
        {
            string path = @"C:\PosMilano\LogsMilano";
            string fileName = DateTime.Today.ToString("yyyyMMdd") + ".txt";
            string fullpath = System.IO.Path.Combine(path, fileName);
            FileInfo fi = new FileInfo(fullpath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (!fi.Exists)
            {
                using (FileStream fs = fi.Create()) { }
            }

            File.AppendAllText(fullpath, string.Format("{0}{1}", write, Environment.NewLine));
        }

        public void ErrorWebService(string WS, string mesage)
        {
            string path = @"C:\PosMilano\LogsMilano";
            string fileName = "ErrorWCF_" + WS + "_" + DateTime.Today.ToString("yyyyMMdd") + ".txt";
            string fullpath = System.IO.Path.Combine(path, fileName);
            FileInfo fi = new FileInfo(fullpath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (!fi.Exists)
            {
                using (FileStream fs = fi.Create()) { }
            }

            File.AppendAllText(fullpath, string.Format("{0}{1}", mesage, Environment.NewLine));
        }
    }
}
