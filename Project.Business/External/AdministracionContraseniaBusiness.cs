using Milano.BackEnd.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Milano.BackEnd.Repository;
using Milano.BackEnd.Dto.General;

namespace Milano.BackEnd.Business
{
    /// <summary>
    /// Administración cambio de contraseña
    /// </summary>
    public class AdministracionContraseniaBusiness : BaseBusiness
    {
        ProxyCambioContrasenia.wsCambioPasswordSoapClient proxy;
        TokenDto token;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token"></param>
        public AdministracionContraseniaBusiness(TokenDto token)
        {
            InformacionServiciosExternosRepository externosRepository = new InformacionServiciosExternosRepository();
            InfoService inforService = new InfoService();
            inforService = externosRepository.ObtenerInfoServicioExterno(13);
            proxy = new ProxyCambioContrasenia.wsCambioPasswordSoapClient();
            proxy.Endpoint.Address = new System.ServiceModel.EndpointAddress(inforService.UrlService);
            this.token = token;
        }

        /// <summary>
        /// Metodo para cambiar password
        /// </summary>
        /// <param name="codigoEmpleado">Codigo de empleado</param>
        /// <param name="password">password</param>
        /// <returns>respuesta OperationResponse</returns>
        public ResponseBussiness<OperationResponse> CambiarPassword(int codigoEmpleado, string password)
        {
            return tryCatch.SafeExecutor(() =>
            {
                string result = proxy.CambiarPassword(token.CodeStore, token.CodeBox, codigoEmpleado, password);
                OperationResponse operationResponseMilano = new OperationResponse();
                operationResponseMilano.CodeDescription = result;
                operationResponseMilano.CodeNumber = result;
                return operationResponseMilano;
            });
        }

    }
}
