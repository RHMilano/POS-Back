using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Repository.General;

namespace Milano.BackEnd.Business.General
{
    /// <summary>
    /// Clase de negocios de configuración de caja
    /// </summary>
    public class InstallationServiceBusiness : BaseBusiness
    {
        /// <summary>
        /// Repositorio de configuración de caja
        /// </summary>
        protected InstallationServiceRepository repository;

        /// <summary>
        /// Constructor por default
        /// </summary>
        public InstallationServiceBusiness()
        {
            this.repository = new InstallationServiceRepository();
        }
        /// <summary>
        /// Insertar configuracion de caja
        /// </summary>
        /// <param name="configurationService">Objeto de la caja contiene codigo, ip estatica y codigo empleado</param>
        /// <returns></returns>
        public ResponseBussiness <OperationResponse> InsertConfigurationBox(ConfiguracionServiceRequest configurationService)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.InsertConfigurationBox(configurationService.CodigoCaja, configurationService.IpEstaticaCaja, configurationService.CodigoEmpleado);
            });
        }
    }
}
