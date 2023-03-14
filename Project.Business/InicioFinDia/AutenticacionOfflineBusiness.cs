using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.InicioFinDia;
using Milano.BackEnd.Repository.InicioFinDia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Milano.BackEnd.Business.InicioFinDia
{

    /// <summary>
    /// Business de autenticacion sin conexion
    /// </summary>
    public class AutenticacionOfflineBusiness : BaseBusiness
    {

        AutenticacionOfflineRepository autenticacionOfflineRepository;
        TokenDto token;
        /// <summary>
        /// Se crea el repositorio de autenticacion sin conexion
        /// </summary>
        public AutenticacionOfflineBusiness(TokenDto tokenDto)
        {
            autenticacionOfflineRepository = new AutenticacionOfflineRepository();
            this.token = tokenDto;
        }

        /// <summary>
        /// 
        /// </summary>
        public ResponseBussiness<ValidacionOperacionResponse> LoginOffline(AutenticacionOfflineRequest autenticacionOfflineRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return autenticacionOfflineRepository.LoginOffline(token, autenticacionOfflineRequest);
            });
        }

    }
}
