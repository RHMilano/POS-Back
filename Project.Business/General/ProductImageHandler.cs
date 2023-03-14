using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Milano.BackEnd.Repository.General;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Repository;
using System.Net;
using System.IO;
using System.Net.Mime;

namespace Milano.BackEnd.Business.General
{

    /// <summary>
    /// Clase de negocio para la manipulación de las imágenes de los productos
    /// </summary>
    public class ProductImageHandler : BaseBusiness
    {

        /// <summary>
        ///Atributo del token usuario
        /// </summary>
        private TokenDto token;
        private ProductImageHandlerRepository repository;


        /// <summary>
        /// Constructor sin parametros
        /// </summary>
        public ProductImageHandler(TokenDto token)
        {
            this.token = token;
            repository = new ProductImageHandlerRepository();
        }

        /// <summary>
        /// Almacenamiento local de imagenes remotas
        /// </summary>
        /// <param name="almacenarImagenArticuloRequest">Información sobre la URL de la imagen remota</param>
        /// <returns>Resultado de la operación</returns>
        public ResponseBussiness<OperationResponse> AlmacenarImagenArticulo(AlmacenarImagenArticuloRequest almacenarImagenArticuloRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                downloadFile(almacenarImagenArticuloRequest.Url);
                OperationResponse operationResponse = new OperationResponse();
                operationResponse.CodeNumber = "100";
                operationResponse.CodeDescription = "OK";
                return operationResponse;
            });
        }


        private void downloadFile(String urlImagen)
        {
            Uri uriImagen = new Uri(urlImagen);
            String file = System.IO.Path.GetFileName(uriImagen.LocalPath);
            WebClient cln = new WebClient();
            String urlLocal = repository.ObtenerRutaImagenes() + file;
            cln.DownloadFileAsync(uriImagen, urlLocal);
        }

    }
}
