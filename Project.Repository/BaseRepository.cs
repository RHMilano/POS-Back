using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Milano.BackEnd.Dto;

using Milano.BackEnd.Repository;
using System.Data;
namespace Milano.BackEnd.Repository
{
    /// <summary>
    /// Clase base de los repositorios
    /// </summary>
    public class BaseRepository
    {

        protected DataAccess.Data data;

        /// <summary>
        /// Constructor por default
        /// </summary>
        public BaseRepository()
        {
            data = new DataAccess.Data();
        }

    }
}
