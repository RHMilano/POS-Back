using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto
{
    /// <summary>
    /// DTO que representa un elemento del formulario
    /// </summary>
    [DataContract]
    public class ElementoFormulario
    {

        /// <summary>
        /// Tipo del elemento a renderizar: INPUT, SELECT, SUBMIT
        /// </summary>
        [DataMember(Name = "tipoElementoFormulario")]
        public String TipoElementoFormulario { get; set; }

        /// <summary>
        /// Nombre del elemento
        /// </summary>
        [DataMember(Name = "nombre")]
        public String Nombre { get; set; }

        /// <summary>
        /// Valor del elemento
        /// </summary>
        [DataMember(Name = "valor")]
        public String Valor { get; set; }

        /// <summary>
        /// Indica si elemento de solo lectura
        /// </summary>
        [DataMember(Name = "soloLectura")]
        public Boolean SoloLectura { get; set; }

        /// <summary>
        /// Definición del elemento Input, tiene validez si el elemento de formulario es INPUT
        /// </summary>
        [DataMember(Name = "definicionElementoInput")]
        public InputElement DefinicionElementoInput { get; set; }

        /// <summary>
        /// Definición del elemento Select, tiene validez si el elemento de formulario es SELECT
        /// </summary>
        [DataMember(Name = "definicionElementoSelect")]
        public SelectElement DefinicionElementoSelect { get; set; }

    }
}
