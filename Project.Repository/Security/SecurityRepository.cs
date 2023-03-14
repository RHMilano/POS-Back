using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Repository;
using System.Data;
using System.Configuration;
using Milano.BackEnd.Utils;
using System.Transactions;
using Milano.BackEnd.Repository;
namespace Milano.BackEnd.Repository.Security
{
    /// <summary>
    /// Clase repositorio de seguridad
    /// </summary>
    public class SecurityRepository : BaseRepository
    {

        /// <summary>
        /// Método para validar permisos sobre un recurso a través de credenciales
        /// </summary>
        /// <param name="numberEmployee">Número de empleado</param>
        /// <param name="password">Contraseña de empleado</param>
        /// <param name="resourceName">Nombre del recurso sobre el cual se requiere el permiso</param>
        /// <param name="permissionType">Tipo de permiso sobre el recurso CRUDE</param>
        /// <returns></returns>
        public OperationResponse ValidatePermissionWithCredentials(int numberEmployee, string password, string resourceName, string permissionType)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoEmpleado", numberEmployee);
            parameters.Add("@Password", password);
            parameters.Add("@NombreRecurso", resourceName);
            parameters.Add("@TipoPermiso", permissionType);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("sp_vanti_SegValidarPermisoConCredenciales", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }


        /// <summary>
        /// Método de autorización y autenticación del empleado
        /// </summary>
        /// <param name="numberEmployee">Número de empleado</param>
        /// <param name="password">Contraseña de empleado</param>
        /// <param name="numberAttempts">Número de intentos</param>        
        /// <param name="ip">Ip del cliente</param>
        /// <param name="tokenDevice">tokenDevice</param>
        /// <param name="esLoginInicial">esLoginInicial</param>               
        /// <returns></returns>
        //public UserResponse Login(int numberEmployee, string password, int numberAttempts, string ip)
        //{
        //    UserResponse user = new UserResponse();
        //    var parameters = new Dictionary<string, object>();
        //    parameters.Add("@CodigoEmpleado", numberEmployee);
        //    parameters.Add("@Password", password);
        //    parameters.Add("@ContadorIntentos", numberAttempts);
        //    parameters.Add("@IpEstaticaCaja", ip);
        //    foreach (var r in data.GetDataReader("sp_vanti_SegLogin", parameters))
        //    {
        //        user.CodeEstatus = Convert.ToInt32(r.GetValue(0));
        //        user.Estatus = r.GetValue(1).ToString();
        //        user.NumberEmployee = numberEmployee;
        //        int codeStore = Convert.ToInt32(r.GetValue(2));
        //        int codeBox = Convert.ToInt32(r.GetValue(3));
        //        int expirationTime = Convert.ToInt32(r.GetValue(4));
        //        user.NumeroCaja = codeBox;
        //        user.Nombre = r.GetValue(5).ToString();
        //        if (user.CodeEstatus == 100)
        //            user.Accesstoken = this.GenerateToken(numberEmployee, codeStore, codeBox, expirationTime);
        //    }
        //    if (user.CodeEstatus == 101)
        //        user.NumberAttempts++;
        //    return user;
        //}




        //AHC:
        public UserResponse Login(int numberEmployee, string password, int numberAttempts, string ip, string tokenDevice, int esLoginInicial)
        {
            UserResponse user = new UserResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoEmpleado", numberEmployee);
            parameters.Add("@Password", password);
            parameters.Add("@ContadorIntentos", numberAttempts);
            parameters.Add("@IpEstaticaCaja", ip);
            parameters.Add("@tokenDevice", tokenDevice);
            parameters.Add("@esLoginInicial", esLoginInicial);

            foreach (var r in data.GetDataReader("sp_vanti_SegLogin", parameters))
            {
                user.CodeEstatus = Convert.ToInt32(r.GetValue(0));
                user.Estatus = r.GetValue(1).ToString();
                user.NumberEmployee = numberEmployee;
                int codeStore = Convert.ToInt32(r.GetValue(2));
                int codeBox = Convert.ToInt32(r.GetValue(3));
                int expirationTime = Convert.ToInt32(r.GetValue(4));
                user.NumeroCaja = codeBox;
                user.Nombre = r.GetValue(5).ToString();
                if (user.CodeEstatus == 100)
                    user.Accesstoken = this.GenerateToken(numberEmployee, codeStore, codeBox, expirationTime);

                user.vencioPassword = Convert.ToInt32(r.GetValue(6));
            }
            if (user.CodeEstatus == 101)
                user.NumberAttempts++;
            return user;
        }

        /// <summary>
        /// Login SUDO
        /// </summary>
        /// <param name="numberEmployee"></param>
        /// <param name="password"></param>
        /// <param name="numberAttempts"></param>
        /// <param name="ip"></param>
        /// <returns></returns>

        public UserResponse LoginSUDO(int numberEmployee, string password, int numberAttempts, string ip)
        {
            UserResponse user = new UserResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoEmpleado", numberEmployee);
            parameters.Add("@Password", password);
            parameters.Add("@ContadorIntentos", numberAttempts);
            parameters.Add("@IpEstaticaCaja", ip);
            foreach (var r in data.GetDataReader("sp_vanti_SegLogin", parameters))
            {
                user.CodeEstatus = Convert.ToInt32(r.GetValue(0));
                user.Estatus = r.GetValue(1).ToString();
                user.NumberEmployee = numberEmployee;
                int codeStore = Convert.ToInt32(r.GetValue(2));
                int codeBox = Convert.ToInt32(r.GetValue(3));
                int expirationTime = Convert.ToInt32(r.GetValue(4));
                user.NumeroCaja = codeBox;
                user.Nombre = r.GetValue(5).ToString();
            }
            if (user.CodeEstatus == 101)
                user.NumberAttempts++;
            return user;
        }

        /// <summary>
        /// Validación de usuario
        /// </summary>
        /// <param name="numberEmployee">Numero de empleado</param>
        /// <param name="password">Contraseña</param>
        /// <param name="codigoTienda">Código de la tienda</param>
        //// <param name="codigoCaja">Código de la caja</param>
        /// <returns>Resultado de la operación</returns>
        public OperationResponse LoginValidacion(int numberEmployee, string password, int codigoTienda, int codigoCaja)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoEmpleado", numberEmployee);
            parameters.Add("@Password", password);
            parameters.Add("@CodigoTienda", codigoTienda);
            parameters.Add("@CodigoCaja", codigoCaja);
            foreach (var r in data.GetDataReader("sp_vanti_SegLoginValidar", parameters))
            {
                operationResponse.CodeNumber = r.GetValue(0).ToString();
                operationResponse.CodeDescription = r.GetValue(1).ToString();
            }
            return operationResponse;
        }

        /// <summary>
        /// Obtiene los permisos sobre los recursos del empleado
        /// </summary>
        /// <param name="numberEmployee">Número de empleado</param>
        /// <returns></returns>
        public string GetResources(int numberEmployee)
        {
            string result = "";
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoEmpleado", numberEmployee);
            foreach (var r in data.GetDataReader("sp_vanti_SegRecursosUsuario", parameters))
            {
                result +=
                    string.Format("{0}::C{1}|{0}::R{2}|{0}::U{3}|{0}::D{4}|{0}::E{5}|",
                    r.GetValue(0).ToString(),
                    Convert.ToBoolean(r.GetValue(1)) == true ? "1" : "0",
                    Convert.ToBoolean(r.GetValue(2)) == true ? "1" : "0",
                    Convert.ToBoolean(r.GetValue(3)) == true ? "1" : "0",
                    Convert.ToBoolean(r.GetValue(4)) == true ? "1" : "0",
                    Convert.ToBoolean(r.GetValue(5)) == true ? "1" : "0"
                    );

            }
            return result;
        }

        /// <summary>
        /// Genera el token del usuario validado
        /// </summary>
        /// <param name="numberEmployee">Número de empleado</param>
        /// <param name="codeStore">Número de tiemda</param>
        /// <param name="codeBox">Número de caja</param>
        /// <returns></returns>
        private string GenerateToken(int numberEmployee, int codeStore, int codeBox, int expirationTime)
        {
            DateTime issued = DateTime.Now;
            var tempToken = new Dictionary<string, object>()
                    {
                        {"usuario", numberEmployee},
                        {"nombre", numberEmployee},
                        {"resources",  this.GetResources (numberEmployee )},
                        {"exp", ToUnixTime(expirationTime).ToString()},
                        {"codeStore", codeStore },
                        {"codeBox", codeBox }
                    };
            return Encrypted.Encode(tempToken);
        }

        /// <summary>
        /// Genera el tiempo de vida del token
        /// </summary>
        /// <param name="expirationTime">Minutos de expiración</param>
        /// <returns></returns>
        private string ToUnixTime(int expirationTime)
        {
            DateTime dateTime = DateTime.Now;
            DateTime dt = dateTime.AddMinutes(expirationTime);
            return dt.ToString("dd/MM/yyyy hh:mm:ss tt");
        }

        /// <summary>
        /// Metodo de cambio de contraseña del empleado
        /// </summary>
        /// <param name="numberEmployee">Número de empleado</param>
        /// <param name="password">Contraseña</param>
        /// <returns></returns>
        public OperationResponse ChangePassword(int numberEmployee, string password)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoEmpleado", numberEmployee);
            parameters.Add("@Password", password);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@codigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@mensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("sp_vanti_SegCambiarContrasenia", parameters, parametersOut);
            operationResponse.CodeNumber = result["@codigoResultado"].ToString();
            operationResponse.CodeDescription = result["@mensajeResultado"].ToString();
            return operationResponse;
        }

        /// <summary>
        /// Metodo para realizar el Logout de la aplicación
        /// </summary>
        /// <param name="numberEmployee">Número de empleado</param>    
        /// <param name="codeStore">Codigo de la Tienda</param>
        /// <param name="codeBox">Codigo de la Caja</param>
        /// <returns></returns>
        public OperationResponse Logout(int numberEmployee, int codeStore, int codeBox)
        {
            OperationResponse operationResponse = new OperationResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoCaja", codeBox);
            parameters.Add("@CodigoTienda", codeStore);
            parameters.Add("@CodigoEmpleado", numberEmployee);
            List<System.Data.SqlClient.SqlParameter> parametersOut = new List<System.Data.SqlClient.SqlParameter>();
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CodigoResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            parametersOut.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@MensajeResultado", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.NVarChar, Size = 4000 });
            var result = data.ExecuteProcedure("[dbo].[sp_vanti_SegLogout]", parameters, parametersOut);
            operationResponse.CodeNumber = result["@CodigoResultado"].ToString();
            operationResponse.CodeDescription = result["@MensajeResultado"].ToString();
            return operationResponse;
        }

    }
}
