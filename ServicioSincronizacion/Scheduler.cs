using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace ServicioSincronizacion
{
    public partial class Scheduler : ServiceBase
    {
        JobScheduler scheduler;

        public Scheduler()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            scheduler = new JobScheduler();
            scheduler.Iniciar();
        }

        protected override void OnStop()
        {
            if (scheduler != null)
            {
                scheduler.Parar(true);
            }
        }

        public void OnStartTest()
        {
            scheduler = new JobScheduler();
            scheduler.Iniciar();
        }

        public void OnStopTest()
        {
            if (scheduler != null)
            {
                scheduler.Parar(true);
            }

            scheduler = new JobScheduler();
            scheduler.Parar(true);
        }
    }
}
