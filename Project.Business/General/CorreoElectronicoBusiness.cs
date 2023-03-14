using Milano.BackEnd.Repository.General;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Dto;

using System.Net;
using System.Net.Mail;

namespace Milano.BackEnd.Business.General
{

    /// <summary>
    /// Clase business Correo Electronico
    /// </summary>

    public class CorreoElectronicoBusiness : BaseBusiness
    {

        /// <summary>
        /// Repositorio de Correo Electronico
        /// </summary>
        protected CorreoElectronicoRepository repository;

        /// <summary>
        /// Constructor por default
        /// </summary>
        public CorreoElectronicoBusiness()
        {
            this.repository = new CorreoElectronicoRepository();
        }

        /// <summary>
        /// Búsqueda de información para correo electrónico
        /// </summary>
        /// <param name="correoElectronico"> Objeto de peticion del tipo de correo electrónico</param>
        /// <returns></returns>
        public ResponseBussiness<CorreoElectronicoResponse> GetCorreoInfo(CorreoElectronicoRequest correoElectronico)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.getInformacionCorreo(correoElectronico.TipoCorreo);
            });
        }


        /// <summary>
        /// Método de envío de correo electrónico
        /// </summary>
        /// <param name="correoElectronico"> Objeto de peticion del tipo de correo electrónico</param>
        /// <returns></returns>
        public ResponseBussiness<CorreoElectronicoResponse> EnviarCorreoElectronico(CorreoElectronicoRequest correoElectronico)
        {
            return tryCatch.SafeExecutor(() =>
            {
                CorreoElectronicoResponse infoCorreoElectronico = this.GetCorreoInfo(correoElectronico);
                /*
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.EnableSsl = true;
                smtp.Port = 587;
                smtp.Credentials = new NetworkCredential("2nd.ogt@gmail.com", "flgaalwxfthxkwnx");
                smtp.Send("2nd.ogt@gmail.com", infoCorreoElectronico.Destinatario, infoCorreoElectronico.Cabecera, infoCorreoElectronico.Content);
                */
                return infoCorreoElectronico;
            });
        }

    }
}
