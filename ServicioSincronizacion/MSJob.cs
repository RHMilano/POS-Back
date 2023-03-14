using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Milano.BackEnd.Business;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Business.Sincronizacion;

namespace ServicioSincronizacion
{
    class MSJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            SincronizacionBusiness ms = new SincronizacionBusiness();
            ms.SincronizarInformacion();
        }
    }
}
