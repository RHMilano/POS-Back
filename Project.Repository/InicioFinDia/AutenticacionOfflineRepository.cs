using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.InicioFinDia;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Milano.BackEnd.Repository.InicioFinDia
{

    /// <summary>
    /// Metodo donde se hacen las transacciones de login offline
    /// </summary>
    public class AutenticacionOfflineRepository : BaseRepository
    {

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public ValidacionOperacionResponse LoginOffline(TokenDto token, AutenticacionOfflineRequest autenticacionOfflineRequest)
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
            var resultadoInfo = data.ExecuteProcedure("[dbo].[sp_vanti_Obtener_Informacion_LoginOffline]", parametros, parametrosOutInfo);
            var fechaOperacion = Convert.ToDateTime(resultadoInfo["@FechaOperacion"]);
            var folio = Convert.ToInt32(resultadoInfo["@Folio"]);

            //Obtener cadena del algoritmo
            string cadenaAlgoritmo = this.getCodeAlgorithm(fechaOperacion.ToString("yyyyMMdd"), fechaOperacion.ToString("yyyy-MM-dd"), token.CodeStore, token.CodeBox, folio);

            //Validar cadena generada con el algoritmo vs la clave que nos envian desde cliente
            if (cadenaAlgoritmo == autenticacionOfflineRequest.Clave)
            {
                // Agegar fecha operacion
                parametros.Add("@FechaOperacion", fechaOperacion);
                List<System.Data.SqlClient.SqlParameter> parametrosOut = new List<System.Data.SqlClient.SqlParameter>();
                parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
                parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
                var resultado = data.ExecuteProcedure("[dbo].[sp_vanti_LoginOffline_InicioDia]", parametros, parametrosOut);
                validacionOperacionResponse.CodeNumber = resultado["@CodigoResultado"].ToString();
                validacionOperacionResponse.CodeDescription = resultado["@MensajeResultado"].ToString();
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
