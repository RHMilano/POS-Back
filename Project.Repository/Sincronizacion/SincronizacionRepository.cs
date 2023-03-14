using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.Sales;
using Milano.BackEnd.Dto.Sincronizacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Milano.BackEnd.Repository.Sincronizacion
{

    /// <summary>
    /// Repositorio de Motor de Sincronización
    /// </summary>
    public class SincronizacionRepository : BaseRepository
    {

        /// <summary>
        /// Ejecutar Proceso de Purga de Datos
        /// </summary>
        public int PurgarDatos()
        {
            var parameters = new Dictionary<string, object>();
            var result = data.ExecuteProcedure("[sync].[sp_vanti_PurgarRegistros]", parameters, null);
            return 1;
        }

        /// <summary>
        /// Ejecutar Proceso de Sincronización
        /// </summary>
        public int SincronizarDatos()
        {
            var parameters = new Dictionary<string, object>();
            var result = data.ExecuteProcedure("[sync].[sp_vanti_Sincronizar_DML]", parameters, null);
            //data.pruebaAaron();
            GC.Collect();
            return 1;
        }

        /// <summary>
        /// Obtener Periodicidad Proceso de Sincronización
        /// </summary>
        public Periodicidad ObtenerPeriodicidad()
        {
            Periodicidad periodicidad = new Periodicidad();
            var parameters = new Dictionary<string, object>();
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@PeriodicidadMotorSincronizacion", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@PeriodicidadCederApartados", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_ObtenerPeriodicidadProcesosAutomaticos]", parameters, parametersOut);
            periodicidad.PeriodicidadProcesoMotorSincronizacion = Int32.Parse(result["@PeriodicidadMotorSincronizacion"].ToString());
            periodicidad.PeriodicidadProcesoCederApartados = Int32.Parse(result["@PeriodicidadCederApartados"].ToString());
            return periodicidad;
        }

        /// <summary>
        /// Obtener informacíón para invocar a uno u otro
        /// </summary>
        public InformacionSincronizador ObtenerInformacionDiscreparSincronizador()
        {
            InformacionSincronizador informacionSincronizador = new InformacionSincronizador();
            var parameters = new Dictionary<string, object>();
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoCajaOrigen", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@IdDestinoSiguienteProcesar", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            var result = data.ExecuteProcedure("[sync].[sp_vanti_ObtenerInformacionSincronizador]", parameters, parametersOut);
            informacionSincronizador.CodigoCajaOrigen = Int32.Parse(result["@CodigoCajaOrigen"].ToString());
            informacionSincronizador.IdDestinoSiguienteProcesar = Int32.Parse(result["@IdDestinoSiguienteProcesar"].ToString());
            return informacionSincronizador;
        }

        /// <summary>
        /// Actualizar la información de la tabla AUDITORIA_DESTINOS
        /// </summary>
        public void ActualizarAuditoriaDestinos(ResultadoSincronizacion resultadoSincronizacion)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@IdServidorDestino", resultadoSincronizacion.IdServidorDestino);
            parameters.Add("@ServidorDestino", resultadoSincronizacion.ServidorDestino);
            parameters.Add("@UltimoIdSincronizado", resultadoSincronizacion.UltimoIdSincronizado);
            parameters.Add("@MensajeAsociado", resultadoSincronizacion.MensajeAsociado);
            data.ExecuteProcedure("[sync].[sp_vanti_SincronizadorActualizarAuditoriaDestinos]", parameters, null);
        }

        /// <summary>
        /// Ejecutar el proceso para sincronizar los datos recibidos
        /// </summary>
        public ResultadoSincronizacion EjecutarSincronizacion(SincronizacionRequest sincronizacionRequest)
        {
            // Armar las sentencias
            String comandosSQL = "";
            int idAnterior = -1;

            for (int i = 0; i < sincronizacionRequest.SentenciasSQL.Length; i++)
            {
                if (sincronizacionRequest.SentenciasSQL[i].Id > idAnterior)
                {
                    comandosSQL += sincronizacionRequest.SentenciasSQL[i].Id + "|||SQLSYNCSENTENCE|||" + sincronizacionRequest.SentenciasSQL[i].Sentencia + "|||SQLSYNCID|||";
                    idAnterior = sincronizacionRequest.SentenciasSQL[i].Id;
                }
                else
                {
                    // Se aborta el proceso, esto no debería suceder
                    return new ResultadoSincronizacion();
                }
            }
            return this.Sincronizar(sincronizacionRequest, comandosSQL, -1);
        }

        private ResultadoSincronizacion Sincronizar(SincronizacionRequest sincronizacionRequest, String comandosSQL, int ignorarSentenciaConId)
        {
            ResultadoSincronizacion resultadoSincronizacion = new ResultadoSincronizacion();
            resultadoSincronizacion.IdServidorDestino = sincronizacionRequest.IdServidorDestino;
            resultadoSincronizacion.ServidorDestino = sincronizacionRequest.ServidorDestino;
            var parameters = new Dictionary<string, object>();
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parameters.Add("@DebeDetenerEnCasoNoEncontrados", sincronizacionRequest.DebeDetenerEnCasoNoEncontrados);
            parameters.Add("@CodigoTiendaOrigen", sincronizacionRequest.CodigoTiendaOrigen);
            parameters.Add("@CodigoCajaOrigen", sincronizacionRequest.CodigoCajaOrigen);
            parameters.Add("@IdSincronizacionMinimoBloque", sincronizacionRequest.IdSincronizacionMinimoBloque);
            parameters.Add("@ComandosSQL", comandosSQL);
            parameters.Add("@IgnorarSentenciaConId", ignorarSentenciaConId);
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@UltimoIdSincronizado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeAsociado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            
            //Experimento Aaron
            var result = data.ExecuteProcedure("[sync].[sp_vanti_SincronizarDatosLocal]", parameters, parametersOut);
            //var result = data.ExecuteProcedure("[sync].[sp_milano_SincronizarDatosLocal]", parameters, parametersOut);
            
            resultadoSincronizacion.UltimoIdSincronizado = Convert.ToInt32(result["@UltimoIdSincronizado"]);
            resultadoSincronizacion.MensajeAsociado = result["@MensajeAsociado"].ToString();
            return resultadoSincronizacion;
        }

        /// <summary>
        /// Ejecutar proceso para obtener los datos que deben Sincronizarse
        /// </summary>
        public SincronizacionRequest ObtenerDatosSincronizacion()
        {
            SincronizacionRequest sincronizacionRequest = new SincronizacionRequest();
            List<SentenciaSQL> listaSentenciasSQL = new List<SentenciaSQL>();
            var parameters = new Dictionary<string, object>();
            foreach (var r in data.GetDataReader("[sync].[sp_vanti_SincronizadorObtenerSentenciasSQL]", parameters))
            {
                // Meta información
                sincronizacionRequest.CodigoTiendaOrigen = Convert.ToInt32(r.GetValue(0));
                sincronizacionRequest.CodigoCajaOrigen = Convert.ToInt32(r.GetValue(1));
                sincronizacionRequest.IdServidorDestino = Convert.ToInt32(r.GetValue(2));
                sincronizacionRequest.ServidorDestino = r.GetValue(3).ToString();
                sincronizacionRequest.DebeDetenerEnCasoNoEncontrados = Convert.ToInt32(r.GetValue(4));
                sincronizacionRequest.DebeIgnorarLlaveDuplicada = Convert.ToInt32(r.GetValue(5));
                sincronizacionRequest.IdSincronizacionMinimoBloque = Convert.ToInt32(r.GetValue(6));
                // Sentencias
                SentenciaSQL sentenciaSQL = new SentenciaSQL();
                sentenciaSQL.Id = Convert.ToInt32(r.GetValue(7));
                sentenciaSQL.Sentencia = r.GetValue(8).ToString();
                listaSentenciasSQL.Add(sentenciaSQL);
            }
            sincronizacionRequest.SentenciasSQL = listaSentenciasSQL.ToArray();
            return sincronizacionRequest;
        }


        /// <summary>
        /// Ejecutar Proceso de Respaldo de transacciones
        /// </summary>
        public int RespaldarDatos()
        {
            var parameters = new Dictionary<string, object>();
            var result = data.ExecuteProcedure("[sync].[sp_respaldoTransacciones]", parameters, null);
            return 1;
        }

        /// <summary>
        /// Ejecutar proceso para obtener la periodicidad del respaldo
        /// </summary>
        public int ObtenerPeriodicidadRespaldo()
        {
            int periodicidad = 10;
            var parameters = new Dictionary<string, object>();
            foreach (var r in data.GetDataReader("[sync].[sp_obtenerPeriodicidadRespaldo]", parameters))
            {
                periodicidad = Convert.ToInt32(r.GetValue(0));
            }
            return periodicidad;
        }

        /// <summary>
        /// Ejecutar proceso para obtener informacion de la maquina
        /// </summary>
        public string getInfoDetencion()
        {
            return data.GetInfoMaquina();
        }

        /// <summary>
        /// Ejecutar proceso para setear la bandera de sincronizador
        /// </summary>
        public int setBandera()
        {
            var parameters = new Dictionary<string, object>();
            var result = data.ExecuteProcedure("[sync].[sp_setBanderaSincronizador]", parameters, null);
            return 1;
        }

        public int setVersion(double version)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@version", version);
            var result = data.ExecuteProcedure("[sync].[setVersionSincronizador]", parameters, null);
            return 1;
        }

        /// <summary>
        /// Ejecutar proceso de pruebas
        /// </summary>
        public void getPruebasAaron()
        {
            data.pruebaAaron();
        }
    }
}
