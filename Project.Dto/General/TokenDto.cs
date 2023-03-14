using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// Clase de token
    /// </summary>
    public class TokenDto
    {

        /// <summary>
        /// Constructor por default
        /// </summary>
        public TokenDto(int codeStore, int codeBox)
        {
            this.CodeStore = codeStore;
            this.CodeBox = codeBox;
        }

        /// <summary>
        /// Constructor indicando el código del empleado de la caja
        /// </summary>
        public TokenDto(int codeStore, int codeBox, int CodeEmployee)
        {
            this.CodeStore = codeStore;
            this.CodeBox = codeBox;
            this.CodeEmployee = CodeEmployee;
        }

        /// <summary>
        /// Codigo de la tienda
        /// </summary>
        public int CodeStore { get; set; }

        /// <summary>
        /// Codigo de la caja
        /// </summary>
        public int CodeBox { get; set; }

        /// <summary>
        /// Codigo del empleado de la caja
        /// </summary>
        public int CodeEmployee { get; set; }

    }
}
