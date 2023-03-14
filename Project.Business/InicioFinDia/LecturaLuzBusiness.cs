using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Dto.InicioFinDia;
using Milano.BackEnd.Repository;
using Milano.BackEnd.Repository.InicioFinDia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Milano.BackEnd.Business.InicioFinDia
{
    /// <summary>
    /// Business de Lectura Luz
    /// </summary>
    public class LecturaLuzBusiness : BaseBusiness
    {
        LecturaLuzRepository lecturaLuzRepository;
        InfoService infoService = new InfoService();
        ProxyCapturaLuz.wsCapturaDeLuzSoapClient proxy;
        InformacionServiciosExternosRepository externosRepository;
        TokenDto token;
        /// <summary>
        /// Repositorio de lectura luz
        /// </summary>
        public LecturaLuzBusiness(TokenDto tokenDto)
        {
            lecturaLuzRepository = new LecturaLuzRepository();
            this.token = tokenDto;
            externosRepository = new InformacionServiciosExternosRepository();
            infoService = externosRepository.ObtenerInfoServicioExterno(21);
            proxy = new ProxyCapturaLuz.wsCapturaDeLuzSoapClient();
            proxy.Endpoint.Address = new System.ServiceModel.EndpointAddress(infoService.UrlService);
        }

        /// <summary>
        /// 
        /// </summary>
        public ResponseBussiness<ValidacionOperacionResponse> CapturaLecturaLuzInicioDia(CapturaLuzRequest capturaLuzRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return CapturaLecturaLuzInicioDiaInternal(capturaLuzRequest);
            });
        }

        private ValidacionOperacionResponse CapturaLecturaLuzInicioDiaInternal(CapturaLuzRequest capturaLuzRequest)
        {
            // OCG: Referencia 1
            String respuestaProxy = "Error al ejecutar el proceso";
            FechaOperacionResponse fechaOperacionResponse = new FechaOperacionResponse();
            ValidacionOperacionResponse validacionOperacionResponse = new ValidacionOperacionResponse();
            // Realizar Inicio de día con WS Milano
            // -- Inicia Se valida si no está disponible el WS            
            try
            {
                respuestaProxy = proxy.CapturaApertura(token.CodeStore, token.CodeEmployee, token.CodeBox, fechaOperacionResponse.FechaOperacion, capturaLuzRequest.ValorLectura);
                if (respuestaProxy == "100")
                {
                    // Respuesta Web Service Milano OK
                    fechaOperacionResponse = lecturaLuzRepository.CapturaLecturaLuzInicioDia(token.CodeStore, token.CodeBox, token.CodeEmployee, capturaLuzRequest);
                    validacionOperacionResponse.CodeNumber = fechaOperacionResponse.CodeNumber;
                    validacionOperacionResponse.CodeDescription = fechaOperacionResponse.CodeDescription;
                }
                else
                {
                    // Respuesta Web Service Milano ERROR
                    validacionOperacionResponse.CodeDescription = respuestaProxy;
                }
            }
            catch (Exception exeption)
            {
                // El Web Service no está disponible, mando un error por cualquier razón. Se maneja escenario Offline
                capturaLuzRequest.ValorLectura = 0;
                capturaLuzRequest.ValorLecturaAdicional = 0;
                fechaOperacionResponse = lecturaLuzRepository.CapturaLecturaLuzInicioDia(token.CodeStore, token.CodeBox, token.CodeEmployee, capturaLuzRequest);
                validacionOperacionResponse.CodeNumber = fechaOperacionResponse.CodeNumber;
                validacionOperacionResponse.CodeDescription = fechaOperacionResponse.CodeDescription + ". Registro Offline";
            }
            // -- Termina Se valida si no está disponible el WS            
            return validacionOperacionResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        public ResponseBussiness<ValidacionOperacionResponse> CapturaLecturaLuzFinDia(CapturaLuzRequest capturaLuzRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return CapturaLecturaLuzFinDiaInternal(capturaLuzRequest);
            });
        }

        private ValidacionOperacionResponse CapturaLecturaLuzFinDiaInternal(CapturaLuzRequest capturaLuzRequest)
        {
            // OCG: Referencia 2
            String respuestaProxy = "Error al ejecutar el proceso";
            FechaOperacionResponse fechaOperacionResponse = new FechaOperacionResponse();
            ValidacionOperacionResponse validacionOperacionResponse = new ValidacionOperacionResponse();
            // Realizar Fin de día con WS Milano
            // -- Inicia Se valida si no está disponible el WS            
            try
            {
                respuestaProxy = proxy.CapturaCierre(token.CodeStore, token.CodeEmployee, token.CodeBox, fechaOperacionResponse.FechaOperacion, capturaLuzRequest.ValorLecturaAdicional, capturaLuzRequest.ValorLectura);
                if (respuestaProxy == "100")
                {
                    // Respuesta Web Service Milano OK
                    fechaOperacionResponse = lecturaLuzRepository.CapturaLecturaLuzFinDia(token.CodeStore, token.CodeBox, token.CodeEmployee, capturaLuzRequest);
                    validacionOperacionResponse.CodeNumber = fechaOperacionResponse.CodeNumber;
                    validacionOperacionResponse.CodeDescription = fechaOperacionResponse.CodeDescription;
                }
                else
                {
                    // Respuesta Web Service Milano ERROR
                    validacionOperacionResponse.CodeDescription = respuestaProxy;
                }
            }
            catch (Exception exeption)
            {
                // El Web Service no está disponible, mando un error por cualquier razón. Se ignora la captura de luz al final del día
                fechaOperacionResponse = lecturaLuzRepository.CapturaLecturaLuzFinDia(token.CodeStore, token.CodeBox, token.CodeEmployee, capturaLuzRequest);
                validacionOperacionResponse.CodeNumber = fechaOperacionResponse.CodeNumber;
                validacionOperacionResponse.CodeDescription = fechaOperacionResponse.CodeDescription + ". Registro Offline"; ;
            }
            // -- Termina Se valida si no está disponible el WS                
            return validacionOperacionResponse;
        }

        public ResponseBussiness<String> ConfirmacionFinDia()
        {
            return tryCatch.SafeExecutor(() =>
            {
                return lecturaLuzRepository.ConfirmacionFinDia();
            });
        }
    }
}
