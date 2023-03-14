using Milano.BackEnd.Business.Sincronizacion;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicioSincronizacion
{
    class JobRespaldo : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            SincronizacionBusiness ms = new SincronizacionBusiness();
            ms.ProcesoDeRespaldo();
        }
    }
}
