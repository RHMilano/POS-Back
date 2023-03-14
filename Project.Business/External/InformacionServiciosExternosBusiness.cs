using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Repository;
namespace Milano.BackEnd.Business
{

    /// <summary>
    /// Clase para obtener las credenciales de los servicios
    /// </summary>
    public class InformacionServiciosExternosBusiness : BaseBusiness
    {
        InformacionServiciosExternosRepository repositorio;

        /// <summary>
        /// Constructor
        /// </summary>
        public InformacionServiciosExternosBusiness()
        {
            this.repositorio = new InformacionServiciosExternosRepository();
        }

        /// <summary>
        /// Obtiene las credenciales de tiempo aire
        /// </summary>
        /// <returns>Credenciales tiempo aire</returns>
        public ResponseBussiness<CredencialesServicioExterno> ObtenerCredencialesTiempoAire()
        {
            return tryCatch.SafeExecutor(() =>
            {
                return this.repositorio.ObtenerCredenciales(11);
            });
        }

        /// <summary>
        /// Obtiene las credenciales de tiempo aire
        /// </summary>
        /// <returns>Credenciales tiempo aire</returns>
        public ResponseBussiness<CredencialesServicioExterno> ObtenerCredencialesConsultaTCMM()
        {
            return tryCatch.SafeExecutor(() =>
            {
                return this.repositorio.ObtenerCredenciales(6);
            });
        }

        /// <summary>
        /// Obtiene las credenciales de pago de servicios
        /// </summary>
        /// <returns>Credenciales de pago de servicios</returns>
        public ResponseBussiness<CredencialesServicioExterno> ObtenerCredencialesPagoServicios()
        {
            return tryCatch.SafeExecutor(() =>
            {
                return this.repositorio.ObtenerCredenciales(12);
            });
        }

        /// <summary>
        /// Obtiene las credenciales de cambio de divisa
        /// </summary>
        /// <returns>Credenciales de cambio de divisa</returns>
        public ResponseBussiness<CredencialesServicioExterno> ObtenerCredencialesCambioDivisa()
        {
            return tryCatch.SafeExecutor(() =>
            {
                return this.repositorio.ObtenerCredenciales(14);
            });
        }

        /// <summary>
        /// Obtiene las credenciales de Finlag
        /// </summary>
        /// <returns>Credenciales de cambio de divisa</returns>
        public ResponseBussiness<CredencialesServicioExterno> ObtenerCredencialesFinlag()
        {
            return tryCatch.SafeExecutor(() =>
            {
                return this.repositorio.ObtenerCredenciales(22);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public InfoService ObtenerCadenaServicioExterno(int idServicio)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return this.repositorio.ObtenerInfoServicioExterno(idServicio);
            });
        }

    }
}
