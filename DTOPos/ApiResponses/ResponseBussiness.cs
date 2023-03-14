using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DTOPos.ApiResponses
{
    /// <summary>
    /// Clase que permite enviar un objeto y su resultado despues de una operacion
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract]
    public class ResponseBussiness<T>
    {
        /// <summary>
        /// Constructor por default
        /// </summary>
        public ResponseBussiness()
        {
            Result = new EstatusRequest();
            Result.Status = true;
        }

        /// <summary>
        /// Resultado de la operacion despues de invocar al Action
        /// </summary>
        [DataMember(Name = "result")]
        public EstatusRequest Result { get; set; }

        /// <summary>
        /// Datos de respuesta
        /// </summary>
        [DataMember(Name = "data")]
        public T Data { get; set; }

        /// <summary>
        /// Metodo estatico para asignar el valor de la entidad
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator T(ResponseBussiness<T> value)
        {
            return value.Data;
        }

        /// <summary>
        /// Metodo para obtener el valor de la entidad
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ResponseBussiness<T>(T value)
        {
            return new ResponseBussiness<T> { Data = value };
        }
    }
}
