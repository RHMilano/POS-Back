using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milano.BackEnd.Dto;
using System.Collections;
using Milano.BackEnd.Repository;
using Milano.BackEnd.Repository.Security;
using System.Transactions;
using System.ServiceModel.Web;
using System.Net;
using Newtonsoft.Json;
using Milano.BackEnd.Utils;
/// <summary>
/// Espacio de nombre para la seguridad de la aplicacion
/// </summary>
namespace Milano.BackEnd.Business.Security
{
    /// <summary>
    /// Clase de Business , encargada  de la seguridad
    /// </summary>
    public class SecurityBusiness : BaseBusiness
    {
        /// <summary>
        /// Atributo de repositorio de seguridad
        /// </summary>
        protected SecurityRepository repository;
        /// <summary>
        /// Atributo del token usuario
        /// </summary>
        protected TokenDto token;

        /// <summary>
        /// Constructor por default
        /// </summary>
        public SecurityBusiness()
        {
            repository = new SecurityRepository();
        }

        /// <summary>
        /// Constructor con Token
        /// </summary>
        public SecurityBusiness(TokenDto token)
        {
            repository = new SecurityRepository();
            this.token = token;
        }

        /// <summary>
        /// Metodo que valida los permisos de un usuario
        /// </summary>
        /// <param name="recurso">Recurso</param>
        /// <param name="permiso">Permiso necesario</param>        
        public void ValidarPermisos(String recurso, String permiso)
        {
            string permisoNecesario = string.Format("{0}::{1}1", recurso, permiso);
            string permisoConCredencialNecesario = string.Format("#SUDO{0}::{1}1", recurso, permiso);
            if (!((ContieneRol(permisoNecesario)) || (ContieneRol(permisoConCredencialNecesario))))
            {
                //Throw an exception with the associated HTTP status code equivalent to HTTP status 401
                throw new WebFaultException<string>("No tiene los privilegios suficientes", HttpStatusCode.Unauthorized);
            }
        }

        /// <summary>
        /// Metodo que valida si un usuario tiene un permiso asignado
        /// </summary>        
        /// <param name="permisoNecesario">Permiso que desea verificarse</param> 
        public Boolean ContieneRol(String permisoNecesario)
        {
            var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
            var SUDOHeader = WebOperationContext.Current.IncomingRequest.Headers["SUDO"];
            if ((authHeader != null) && (authHeader != string.Empty))
            {
                try
                {
                    var access_token = Encrypted.Decode(authHeader);
                    var token = JsonConvert.DeserializeObject<Dictionary<string, object>>(access_token);
                    var usuario = token["usuario"].ToString();
                    var fechaExpiracion = DateTime.Parse(token["exp"].ToString());
                    string recursos = token["resources"].ToString();
                    if (DateTime.Now > fechaExpiracion)
                    {
                        throw new WebFaultException<string>("El Token de Acceso Expiró", HttpStatusCode.Unauthorized);
                    }
                    List<string> listaRecursos = new List<string>();
                    if ((SUDOHeader != null) && (SUDOHeader != string.Empty))
                    {
                        SecurityBusiness securityBusiness = new SecurityBusiness();
                        var informacionDecodificadaSUDO = new Encrypted().Base64DecodeToString(SUDOHeader);
                        var credenciales = informacionDecodificadaSUDO.Split(':');
                        var userResponse = new SecurityBusiness().LoginSUDO(Int32.Parse(credenciales[0]), credenciales[1], 1, GetIp());
                        if (userResponse.Data.CodeEstatus == 100)
                        {
                            var recursosSUDO = new SecurityBusiness().ObtenerRecursos(Int32.Parse(credenciales[0]));
                            foreach (string recurso in recursosSUDO.Data.Split('|'))
                            {
                                listaRecursos.Add(recurso);
                            }
                        }
                        else
                        {
                            throw new WebFaultException<string>(userResponse.Data.Estatus, HttpStatusCode.Unauthorized);
                        }
                    }
                    else
                    {
                        foreach (var resource in recursos.Split('|'))
                        {
                            listaRecursos.Add(resource);
                        }
                    }
                    // Validar permisos
                    foreach (String permiso in listaRecursos)
                    {
                        if (permisoNecesario.Equals(permiso))
                        {
                            return true;
                        }
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    //Throw an exception with the associated HTTP status code equivalent to HTTP status 401  
                    throw new WebFaultException<string>("El Token de acceso no es válido: " + ex.ToString(), HttpStatusCode.Unauthorized);
                }
            }
            else
            {
                //Throw an exception with the associated HTTP status code equivalent to HTTP status 401  
                throw new WebFaultException<string>("No existe cabecera de autorización", HttpStatusCode.Unauthorized);
            }
        }

        /// <summary>
        /// Metodo que obtiene la IP
        /// </summary>
        /// <returns></returns>
        private string GetIp()
        {
            return "::1";
            /*
			OperationContext context = OperationContext.Current;
			MessageProperties prop = context.IncomingMessageProperties;
			RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
			return endpoint.Address;
			*/
        }

        /// <summary>
        /// Metodo de autorizacion y autentificacion
        /// </summary>
        /// <param name="numberEmployee">numero de empleado</param>
        /// <param name="password">contraseña</param>
        /// <param name="numberAttempts">numero de intentos</param>
        /// <param name="ip">Ip del cliente</param>
        /// <param name="tokenDevice">token del dispositivo</param>
        /// <param name="esLoginInicial">Es Login inicial</param>
        /// <returns></returns>
        public ResponseBussiness<UserResponse> Login(int numberEmployee, string password, int numberAttempts, string ip, string tokenDevice, int esLoginInicial)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.Login(numberEmployee, password, numberAttempts, ip, tokenDevice, esLoginInicial);
            });
        }

        /// <summary>
        /// Login sudo
        /// </summary>
        /// <param name="numberEmployee">numero de empleado</param>
        /// <param name="password">contraseña</param>
        /// <param name="numberAttempts">numero de intentos</param>
        /// /// <param name="ip">Ip del cliente</param>
        /// <returns></returns>
        public ResponseBussiness<UserResponse> LoginSUDO(int numberEmployee, string password, int numberAttempts, string ip)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.LoginSUDO(numberEmployee, password, numberAttempts, ip);
            });
        }

        /// <summary>
        /// Validacionde usuario
        /// </summary>
        /// <param name="numberEmployee">Numero de empleado</param>
        /// <param name="password">Contraseña</param>		
        /// <param name="codigoTienda">Código de la tienda</param>
        /// <param name="codigoCaja">Código de la caja</param>
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> LoginValidacion(int numberEmployee, string password, int codigoTienda, int codigoCaja)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.LoginValidacion(numberEmployee, password, codigoTienda, codigoCaja);
            });
        }

        /// <summary>
        /// Obtener los recursos del empleado
        /// </summary>
        /// <param name="numeroEmpleado">Numero de empleado</param>
        /// <returns>Resultado de la operación</returns>
        public ResponseBussiness<string> ObtenerRecursos(int numeroEmpleado)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.GetResources(numeroEmpleado);
            });
        }

        /// <summary>
        /// Metodo para cambio de contraseña
        /// </summary>
        /// <param name="numberEmployee">numero de empleado</param>
        /// <param name="password">contraseña</param>
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> ChangePassword(int numberEmployee, string password)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return this.ChangePasswordInternal(numberEmployee, password);
            });
        }

        private OperationResponse ChangePasswordInternal(int numberEmployee, string password)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                OperationResponse operationResponseLocal = repository.ChangePassword(numberEmployee, password);
                OperationResponse operationResponseRemoto = null;
                if (operationResponseLocal.CodeNumber == "106")
                {
                    operationResponseRemoto = new AdministracionContraseniaBusiness(this.token).CambiarPassword(numberEmployee, password);
                }
                else
                {
                    // No fue posible actualizar password
                    return operationResponseLocal;
                }
                if (operationResponseRemoto.CodeNumber == "")
                {
                    scope.Complete();
                    return operationResponseLocal;
                }
                else
                {
                    // Ocurrió un error al actualizar password remoto
                    return operationResponseRemoto;
                }
            }
        }

        /// <summary>
        /// Metodo para realizar Logout de la aplicación
        /// </summary>        
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> Logout()
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.Logout(token.CodeEmployee, token.CodeStore, token.CodeBox);
            });
        }

        /// <summary>
        /// Método para validar permisos sobre un recurso a través de credenciales
        /// </summary>
        /// <param name="numberEmployee">Número de empleado</param>
        /// <param name="password">Contraseña de empleado</param>
        /// <param name="resourceName">Nombre del recurso sobre el cual se requiere el permiso</param>
        /// <param name="permissionType">Tipo de permiso sobre el recurso CRUDE</param>
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> ValidatePermissionWithCredentials(int numberEmployee, string password, string resourceName, string permissionType)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.ValidatePermissionWithCredentials(numberEmployee, password, resourceName, permissionType);
            });
        }

    }
}
