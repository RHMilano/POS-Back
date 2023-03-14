using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using System.Security.Principal;
using System.Security;
using Milano.BackEnd.Utils;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Business.Security;
using System.Globalization;
using Milano.BackEnd.Business;
using System.Web;
using Project.Services.LogMonitor;

namespace Project.Services
{
    /// <summary>
    /// Clase para validacion del token
    /// </summary>
    public class DistributorValidator : ServiceAuthorizationManager
    {
        /// <summary>
        /// Metodo que valida el token
        /// </summary>
        /// <param name="operationContext">metodo que se ejecutara</param>
        /// <returns></returns>
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            LogRegister s = new LogRegister();
            //s.LogEntry(" CheckAccessCore: Inicio (Validando Token) ", 2);

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-MX");

            if (WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri.ToString().Contains("LoginService.svc") || WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri.ToString().Contains("LogoutService.svc"))
                return true;

            var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];

            // OCG: SI MARCA ERROR NULL O DE INSTANCIA NO ESTABLECIDA, ES POR QUE LA URL TIENE MAL EL VERBO HTTP O ESTAMOS
            // PASANDO GET CON PARAMETROS
            String webServiceRequestedPath = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.OriginalString;

            if (!webServiceRequestedPath.Contains("Sincronizacion/Sincronizacionservice.svc/ejecutarProcesoSincronizacion"))
            {
                if ((authHeader != null) && (authHeader != string.Empty))
                {
                    try
                    {

                        if (authHeader.Contains("Bearer "))
                        {
                            authHeader = authHeader.Replace("Bearer ", "");
                        }

                        var access_token = Encrypted.Decode(authHeader);
                        var token = JsonConvert.DeserializeObject<Dictionary<string, object>>(access_token);
                        var usuario = token["usuario"].ToString();
                        var fechaExpiracion = DateTime.Now.AddYears(100);  //OCG DateTime.Parse(token["exp"].ToString());
                        string recursos = token["resources"].ToString();
                        if (DateTime.Now > fechaExpiracion)
                        {
                            throw new WebFaultException<string>("El Token de Acceso Expiró", HttpStatusCode.Unauthorized);
                        }
                        List<string> listaRecursos = new List<string>();
                        foreach (var resource in recursos.Split('|'))
                        {
                            listaRecursos.Add(resource);
                        }
                        try
                        {
                            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(usuario), listaRecursos.ToArray());
                        }
                        catch (SecurityException ex)
                        {
                            throw new WebFaultException<string>(ex.ToString(), HttpStatusCode.Unauthorized);
                        }
                        //s.LogEntry(" CheckAccessCore: FIN (Token valido) ", 2);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        _ = ex.Message;
                        //Throw an exception with the associated HTTP status code equivalent to HTTP status 401  
                        throw new WebFaultException<string>("El Token de acceso no es válido", HttpStatusCode.Unauthorized);
                    }
                } 
                else
                {
                    //Throw an exception with the associated HTTP status code equivalent to HTTP status 401  
                    throw new WebFaultException<string>("No existe cabecera de autorización", HttpStatusCode.Unauthorized);
                }
            }
            else
            {
                //s.LogEntry(" CheckAccessCore: FIN (Token valido) ", 2);
                return true;
            }
           
        }
    }
}