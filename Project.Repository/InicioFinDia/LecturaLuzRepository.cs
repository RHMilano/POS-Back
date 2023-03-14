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
    /// Repositorio de lectura luz
    /// </summary>
    public class LecturaLuzRepository : BaseRepository
    {

        /// <summary>
        /// Metodo para insertar la captura lectura de luz
        /// </summary>
        /// <param name="codeStore"></param>
        /// <param name="codeBox"></param>
        /// <param name="codeEmployee"></param>
        /// <param name="capturaLuzRequest"></param>
        /// <returns></returns>
        public FechaOperacionResponse CapturaLecturaLuzInicioDia(int codeStore, int codeBox, int codeEmployee, CapturaLuzRequest capturaLuzRequest)
        {
            //OCG: Referencia 3
            FechaOperacionResponse fechaOperacionResponse = new FechaOperacionResponse();
            var parametros = new Dictionary<string, object>();
            parametros.Add("@CodigoTienda", codeStore);
            parametros.Add("@CodigoCaja", codeBox);
            parametros.Add("@CodigoEmpleado", codeEmployee);
            parametros.Add("@ValorInicioDia", capturaLuzRequest.ValorLectura);
            parametros.Add("@versionPoxs", capturaLuzRequest.versionPos);
            List<System.Data.SqlClient.SqlParameter> parametrosOut = new List<System.Data.SqlClient.SqlParameter>();
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@FechaOperacion", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Date });
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var resultado = data.ExecuteProcedure("[dbo].[sp_vanti_server_CapturaLuzInicioDia]", parametros, parametrosOut);
            fechaOperacionResponse.FechaOperacion = Convert.ToDateTime(resultado["@FechaOperacion"]);
            fechaOperacionResponse.CodeNumber = resultado["@CodigoResultado"].ToString();
            fechaOperacionResponse.CodeDescription = resultado["@MensajeResultado"].ToString();
            return fechaOperacionResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeStore"></param>
        /// <param name="codeBox"></param>
        /// <param name="codeEmployee"></param>
        /// <param name="capturaLuzRequest"></param>
        /// <returns></returns>
        public FechaOperacionResponse CapturaLecturaLuzFinDia(int codeStore, int codeBox, int codeEmployee, CapturaLuzRequest capturaLuzRequest)
        {
            // OCG: Referencia 4
            FechaOperacionResponse fechaOperacionResponse = new FechaOperacionResponse();
            var parametros = new Dictionary<string, object>();
            parametros.Add("@CodigoTienda", codeStore);
            parametros.Add("@CodigoCaja", codeBox);
            parametros.Add("@CodigoEmpleado", codeEmployee);
            parametros.Add("@ValorLectura", capturaLuzRequest.ValorLectura);
            parametros.Add("@ValorLecturaAdicional", capturaLuzRequest.ValorLecturaAdicional);
            List<System.Data.SqlClient.SqlParameter> parametrosOut = new List<System.Data.SqlClient.SqlParameter>();
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@FechaOperacion", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Date });
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametrosOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var resultado = data.ExecuteProcedure("[dbo].[sp_vanti_server_CapturaLuzFinDia]", parametros, parametrosOut);
            fechaOperacionResponse.FechaOperacion = Convert.ToDateTime(resultado["@FechaOperacion"]);
            fechaOperacionResponse.CodeNumber = resultado["@CodigoResultado"].ToString();
            fechaOperacionResponse.CodeDescription = resultado["@MensajeResultado"].ToString();
            return fechaOperacionResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeStore"></param>      
        /// <returns></returns>
        public ControlInicioFinDeDia ObtenerLuzInicioDia(int codeStore)
        {
            ControlInicioFinDeDia controlInicioFinDeDia = new ControlInicioFinDeDia();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", codeStore);
            foreach (var item in data.GetDataReader("[dbo].[sp_vanti_server_ObtenerValorInicioDia]", parameters))
            {
                controlInicioFinDeDia.InicioDiaCapturaLuz = Convert.ToInt32(item.GetValue(0));
                controlInicioFinDeDia.FechaOperacion = Convert.ToDateTime(item.GetValue(1));
            }
            return controlInicioFinDeDia;
        }

        /// <summary>
        /// 
        /// </summary>    
        /// <returns></returns>
        public String ConfirmacionFinDia()
        {
            string salida = string.Empty;
            var parameters = new Dictionary<string, object>();
            foreach (var item in data.GetDataReader("[dbo].[SP_ConfirmacionFinDia]", parameters))
            {
                salida = item.GetValue(0).ToString();
            }
            return salida;
        }

    }
}
