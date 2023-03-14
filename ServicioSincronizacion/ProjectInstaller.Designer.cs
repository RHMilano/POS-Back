namespace ServicioSincronizacion
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
            this.SincronizadorMilano = new System.ServiceProcess.ServiceInstaller();
            // 
            // ProcessInstaller
            // 
            this.ProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.ProcessInstaller.Password = null;
            this.ProcessInstaller.Username = null;
            // 
            // SincronizadorMilano
            // 
            this.SincronizadorMilano.ServiceName = "SincronizadorMilano";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ProcessInstaller,
            this.SincronizadorMilano});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller ProcessInstaller;
        private System.ServiceProcess.ServiceInstaller SincronizadorMilano;
    }
}