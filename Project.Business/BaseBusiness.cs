using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milano.BackEnd.Dto;
using System.Collections;
using Milano.BackEnd.Repository;
namespace Milano.BackEnd.Business
{
    /// <summary>
    /// Clase de base de Business
    /// </summary>
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