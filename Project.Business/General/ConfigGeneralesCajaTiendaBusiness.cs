using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Milano.BackEnd.Repository.General;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Dto;

namespace Milano.BackEnd.Business.General
{
    /// <summary>
    /// Clase de negocios de las configuraciones generales de las cajas y tiendas.
    /// </summary>
    public class ConfigGeneralesCajaTiendaBusiness : BaseBusiness
    {
        /// <summary>
        /// Repositorio de de configuraciones
        /// </summary>
        protected ConfigGeneralesCajaTiendaRepository repository;
        /// <summary>
		/// Atributo del token usuario
		/// </summary>
		protected TokenDto token;

        /// <summary>
        /// Constructor por default
        /// </summary>
        public ConfigGeneralesCajaTiendaBusiness(TokenDto token)
        {
            this.repository = new ConfigGeneralesCajaTiendaRepository();
            this.token = token;
        }

        /// <summary>
        /// Trae la configuracion de la caja y tienda
        /// </summary>        
        /// <returns></returns>
        public ResponseBussiness<ConfigGeneralesCajaTiendaResponse> getConfigs(string versionPOS = "")
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.GetConfig(token.CodeBox, token.CodeStore, token.CodeEmployee, versionPOS);
            });
        }
    }
}
