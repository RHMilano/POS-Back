using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Milano.BackEnd.Dto.Configuracion
{

    /// <summary>
    /// DTO de Configuración de Botón
    /// </summary>
    [DataContract]
    public class ConfiguracionBoton
    {

        /// <summary>
        /// Identificador del botón
        /// </summary>
        [DataMember(Name = "identificador")]
        public string Identificador { get; set; }

        /// <summary>
        /// Orden de aparición
        /// </summary>
        [DataMember(Name = "orden")]
        public int Orden { get; set; }

        /// <summary>
        /// Descripción visible del botón
        /// </summary>
        [DataMember(Name = "textoDescripcion")]
        public string TextoDescripcion { get; set; }

        /// <summary>
        /// Ruta de la imagen ícono del botón
        /// </summary>
        [DataMember(Name = "rutaImagen")]
        public string RutaImagen { get; set; }

        /// <summary>
        /// Tecla de acceso rápido
        /// </summary>
        [DataMember(Name = "teclaAccesoRapido")]
        public string TeclaAccesoRapido { get; set; }

        /// <summary>
        /// Propiedad sobre visibilidad del botón
        /// </summary>
        [DataMember(Name = "visible")]
        public Boolean Visible { get; set; }

        /// <summary>
        /// Propiedad sobre si el botón se encuentra habilitado
        /// </summary>
        [DataMember(Name = "habilitado")]
        public Boolean Habilitado { get; set; }

        /// <summary>
        /// Configuración de SubBotones
        /// </summary>
        [DataMember(Name = "configuracionSubBotones")]
        public ConfiguracionBoton[] ConfiguracionSubBotones { get; set; }

    }
}
