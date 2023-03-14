using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Milano.BackEnd.Dto.General
{
    /// <summary>
    /// Clase para la configuración de caja local
    /// </summary>
    [DataContract]
    public class ConfiguracionServiceRequest
    {
        ///<summary>
		/// código de caja
		/// </summary>
        [DataMember(Name = "codigoCaja")]
        public int CodigoCaja { get; set; }

        ///<summary>
		/// Ip estática caja
		/// </summary>
        [DataMember(Name = "ipEstaticaCaja")]
        public string IpEstaticaCaja { get; set; }

        ///<summary>
		/// Código de empleado
		/// </summary>
        [DataMember(Name = "codigoEmpleado")]
        public int CodigoEmpleado { get; set; }
    }
}
