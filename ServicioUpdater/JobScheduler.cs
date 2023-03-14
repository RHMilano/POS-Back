using Quartz;
using Quartz.Impl;
using System;

namespace ServicioUpdater
{
    class JobScheduler
    {

        public void Iniciar()
        {
            try
            {
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Start();

                // Proceso para Procesar peticiones de actualización de Software
                IJobDetail updaterJob = JobBuilder.Create<UpdaterJob>().Build();
                ITrigger updaterTrigger = TriggerBuilder.Create().WithSimpleSchedule(a => a.WithIntervalInSeconds(59).RepeatForever()).Build();

                // Lanzamiento               
                scheduler.ScheduleJob(updaterJob, updaterTrigger);
            }
            catch (Exception)
            {
                this.Parar();
                this.Iniciar();
            }
        }

        public void Parar()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Shutdown();
        }

    }
}