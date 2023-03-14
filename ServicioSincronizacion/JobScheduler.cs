using Milano.BackEnd.Business.Sincronizacion;
using Milano.BackEnd.Dto.Sincronizacion;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicioSincronizacion
{
    class JobScheduler
    {

        public void Iniciar()
        {
            try
            {
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Start();

                // Obtener la Periodicidad de ejecuciones
                SincronizacionBusiness ms = new SincronizacionBusiness();
                Periodicidad periodicidad = ms.ObtenerPeriodicidad();
                int periodicidadRespaldo = ms.ObtenerPeriodicidadRespaldo();

                if (periodicidad == null)
                    throw new Exception("Espera");

                int version = ms.setVersion(2.0);
                int set = ms.setBandera();

                // Proceso de Ceder Apartados
                IJobDetail apartadosJob = JobBuilder.Create<ApartadosJob>().Build();
                ITrigger apartadosTrigger = TriggerBuilder.Create().WithSimpleSchedule(a => a.WithIntervalInSeconds(periodicidad.PeriodicidadProcesoCederApartados).RepeatForever()).Build();

                // Proceso de Motor de Sincronización           
                IJobDetail msJob = JobBuilder.Create<MSJob>().Build();
                ITrigger msTrigger = TriggerBuilder.Create().WithSimpleSchedule(a => a.WithIntervalInSeconds(periodicidad.PeriodicidadProcesoMotorSincronizacion).RepeatForever()).Build();

                // Proceso para Purgar Registros de Auditoría
                IJobDetail purgaJob = JobBuilder.Create<PurgaJob>().Build();
                ITrigger purgaTrigger = TriggerBuilder.Create().WithSimpleSchedule(a => a.WithIntervalInHours(4).RepeatForever()).Build();

                //Proceso para generar el respaldo de auditoria_ddl_dml
                IJobDetail respaldo = JobBuilder.Create<JobRespaldo>().Build();
                ITrigger respaldoTrigger = TriggerBuilder.Create().WithSimpleSchedule(a => a.WithIntervalInMinutes(periodicidadRespaldo).RepeatForever()).Build();

                // Lanzamiento
                scheduler.ScheduleJob(apartadosJob, apartadosTrigger);
                scheduler.ScheduleJob(msJob, msTrigger);
                scheduler.ScheduleJob(purgaJob, purgaTrigger);
                scheduler.ScheduleJob(respaldo, respaldoTrigger);
            }
            catch (Exception e)
            {
                this.Parar();

                //if (e.Message == "Espera")                
                //    System.Threading.Thread.Sleep(30000);                
                //else                
                    System.Threading.Thread.Sleep(2000);
                
                this.Iniciar();
            }
        }

        public void Parar(bool detencion = false)
        {
            if (detencion)
            {
                SincronizacionBusiness ms = new SincronizacionBusiness();
                ms.NotificarDetencionServicio();
                //ms.PruebasAaron("Parando");
            }

            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Shutdown();
        }

    }
}
