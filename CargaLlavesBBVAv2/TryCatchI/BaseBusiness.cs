namespace CargaLlavesBBVAv2.TryCatchI
{
    public class BaseBusiness
    {
        /// <summary>
        /// Propiedad que heredan las otras clases para el manejo de errores
        /// </summary>
        protected TryCatchBusinessExecutor tryCatch;
        /// <summary>
        /// Constructor por default
        /// </summary>
        public BaseBusiness()
        {
            this.tryCatch = new TryCatchBusinessExecutor();
        }
    }
}
