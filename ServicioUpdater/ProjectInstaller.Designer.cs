namespace ServicioUpdater
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.ProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.UpdaterMilano = new System.ServiceProcess.ServiceInstaller();
            // 
            // ProcessInstaller
            // 
            this.ProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.ProcessInstaller.Password = null;
            this.ProcessInstaller.Username = null;
            // 
            // UpdaterMilano
            // 
            this.UpdaterMilano.ServiceName = "UpdaterMilano";
            this.UpdaterMilano.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceInstaller1_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ProcessInstaller,
            this.UpdaterMilano});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller ProcessInstaller;
        private System.ServiceProcess.ServiceInstaller UpdaterMilano;
    }
}