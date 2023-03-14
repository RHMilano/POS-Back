using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Milano.BackEnd.Business;
using Milano.BackEnd.Dto;
namespace ServicioSincronizacion
{
    class ApartadosJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            ApartadosBusiness apartado = new ApartadosBusiness();
            apartado.CederApartados();
        }
    }
}
