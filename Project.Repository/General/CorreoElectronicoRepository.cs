using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Milano.BackEnd.Dto.General;

namespace Milano.BackEnd.Repository.General
{
    /// <summary>
    /// Repository Envio Correo Electronico
    /// </summary>
    public class CorreoElectronicoRepository : BaseRepository
    {
        /// <summary>
        /// Obtener los datos del correo Electronico a Enviar
        /// </summary>
        /// <param name="tipoEmail">Tipo de correo electrónico</param>
        /// <returns>Datos Correo</returns>

        public CorreoElectronicoResponse getInformacionCorreo(int tipoEmail)
        {
            CorreoElectronicoResponse correoElectronicoResponse = new CorreoElectronicoResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@TipoEmail", tipoEmail);
            foreach (var r in data.GetDataReader("[dbo].[sp_vanti_ObtenInformacionEmail]", parameters))
            {
                correoElectronicoResponse.Cabecera = r.GetValue (1).ToString ();
                correoElectronicoResponse.Destinatario = r.GetValue(0).ToString ();
                correoElectronicoResponse.Content = r.GetValue(2).ToString ();
            }
            return correoElectronicoResponse;
        }
    }
}
