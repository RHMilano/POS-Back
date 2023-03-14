using Milano.BackEnd.Business.Actualizador;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ServicioUpdater
{
    class UpdaterJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            ActualizadorBusiness actualizadorBusiness = new ActualizadorBusiness();
            actualizadorBusiness.ProcesarPeticionesPendientesActualizacionSoftware();
        }
    }
}
