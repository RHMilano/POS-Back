using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.TryCatchI
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
