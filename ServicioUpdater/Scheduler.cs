using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace ServicioUpdater
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
                scheduler.Parar();
            }
        }
    }
}
