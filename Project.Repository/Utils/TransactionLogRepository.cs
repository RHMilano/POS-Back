using Milano.BackEnd.Repository.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Milano.BackEnd.Dto;
using System.IO;
using System.Reflection;

namespace Milano.BackEnd.Repository
{
    /// <summary>
    /// Clase para serializar los Dto
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TransactionLogRepository<T>
    {
        private TokenDto token;
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="token">parametro de token</param>
        public TransactionLogRepository(TokenDto token)
        {
            this.token = token;
        }
        /// <summary>
        /// Metodo para agregar el log
        /// </summary>
        /// <param name="item">dto a guardar en xml</param>
        /// <param name="fileName">nombre del archivo</param>
        public int Add(T item, string fileName)
        {
            try
            {
                string path = new ConfigGeneralesCajaTiendaRepository().GetConfig(token.CodeBox, token.CodeStore, token.CodeEmployee).RutaLogTransacciones;
                System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(T));
                string dir = DateTime.Now.ToString("dd_MM_yyyy");
                path = string.Format("{0}//{1}", path, dir);
                System.IO.DirectoryInfo infoDirectory = new System.IO.DirectoryInfo(path);
                if (!infoDirectory.Exists)
                    infoDirectory.Create();
                path = string.Format("{0}//{1}.xml", path, fileName);
                System.IO.FileStream file = System.IO.File.Create(path);
                writer.Serialize(file, item);
                file.Close();
                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
