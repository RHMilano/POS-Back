using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Milano.BackEnd.Dto.Finlag
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class ClienteFinlagResponse
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "fechaNacimiento")]
        public string FechaNacimiento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "iNE")]
        public string INE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "nombre")]
        public string Nombre { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "aPaterno")]
        public string Apaterno { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "aMaterno")]
        public string Amaterno { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "calle")]
        public string Calle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "numExt")]
        public string NumExt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "colonia")]
        public string Colonia { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "estado")]
        public string Estado { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "municipio")]
        public string Municipio { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "cP")]
        public string CP { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "sexo")]
        public string Sexo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "estatusCliente")]
        public string EstatusCliente { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "NumeroCodigo")]
        public int NumeroCodigo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "DescripcionCodigo")]
        public string DescripcionCodigo { get; set; }
    }
}
