using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Milano.BackEnd.Dto;
using System.Configuration;
using Milano.BackEnd.Repository;
using Milano.BackEnd.Dto.General;

namespace Milano.BackEnd.Business
{
    /// <summary>
    /// Clase tiempo aire
    /// </summary>
    public class TiempoAireBusiness : BaseBusiness
    {

        CredencialesServicioExterno credenciales;
        ProxyTiempoAire.transactSoapClient transact;
        PagoServiciosRepository repository;
        private InformacionServiciosExternosRepository externosRepository;
        InfoService inforService;
        TokenDto token;

        /// <summary>
        /// Constructor de tiempo aire
        /// </summary>
        public TiempoAireBusiness(TokenDto token)
        {
            this.token = token;
            credenciales = new CredencialesServicioExterno();
            credenciales = new InformacionServiciosExternosBusiness().ObtenerCredencialesTiempoAire();
            inforService = new InfoService();
            externosRepository = new InformacionServiciosExternosRepository();
            inforService = externosRepository.ObtenerInfoServicioExterno(11);
            transact = new ProxyTiempoAire.transactSoapClient();
            transact.Endpoint.Address = new System.ServiceModel.EndpointAddress(inforService.UrlService);
            repository = new PagoServiciosRepository(this.token);
        }

        /// <summary>
        /// Metodo para agregar tiempo aire
        /// </summary>
        /// <param name="tiempoAireRequest">Petición de Tiempo Aire</param>
        /// <param name="sku">SKU</param>
        /// <param name="folio">Folio de la Venta</param>
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> AddTiempoAire(TiempoAireRequest tiempoAireRequest, int sku, string folio)
        {
            return tryCatch.SafeExecutor(() =>
            {
                OperationResponse operation = new OperationResponse();
                ProxyTiempoAire.TResponse tResponse = new ProxyTiempoAire.TResponse();
                string requestId = string.Empty;
                requestId = GetRequestId(0);
                if (requestId != "")
                {
                    tResponse = GetDto(requestId, tiempoAireRequest.SkuCode, tiempoAireRequest.Telefono, tiempoAireRequest.Monto);
                    string result = "";
                    if (tResponse != null)
                    {
                        if (tResponse.rcode < 3)
                        {
                            result = this.Doa(requestId, tResponse.op_authorization);
                        }
                        else
                        {
                            result = tResponse.rcode_description;
                        }
                    }
                    else
                    {
                        result = "Transacción Fallida por timeout";
                    }
                    operation.CodeDescription = result;
                }
                else
                {
                    operation.CodeDescription = "Error del Web Service, contactar a Administrador del Sistema";
                }
                if (operation.CodeDescription.ToUpper() == "<DoAResponse>OK</DoAResponse>".ToUpper())
                {
                    repository.RegistrarAutorizacionPagoTiempoAire(tiempoAireRequest.SkuCode, sku, folio, tResponse.op_authorization);
                    operation.CodeNumber = "1";
                    operation.CodeDescription = requestId;
                }
                else
                {
                    operation.CodeNumber = "0";
                }
                return operation;
            });
        }

        private string GetRequestId(int numeroIntentos)
        {
            string id = string.Empty;
            try
            {
                id = transact.GetTRequestID(this.credenciales.UserName, this.credenciales.Password, this.credenciales.Licence);
            }
            catch
            { }
            if ((id == "" || id == null) && numeroIntentos < 3)
                GetRequestId(numeroIntentos + 1);
            return id;
        }

        private ProxyTiempoAire.TResponse GetDto(string requestId, string skuCode, string oppAcount, float monto)
        {
            ProxyTiempoAire.TResponse tResponse = null;
            try
            {
                tResponse = transact.DoT(requestId, this.credenciales.UserName, skuCode, oppAcount, monto, null);
                if (tResponse.rcode == 2)
                    tResponse = this.CheckTransaction(requestId);
            }
            catch (Exception)
            {
                return this.CheckTransaction(requestId);
            }
            return tResponse;
        }

        private ProxyTiempoAire.TResponse CheckTransaction(string requestId)
        {
            ProxyTiempoAire.TResponse tResponse = null;
            int cont = 0;
            bool superamosLosIntentos = false;
            while (cont <= this.credenciales.NumeroIntentos)
            {
                try
                {
                    tResponse = transact.CheckTransaction(requestId, this.credenciales.UserName);
                }
                catch (Exception)
                {
                }
                if (tResponse.rcode != 2)
                    cont = this.credenciales.NumeroIntentos;
                else
                    System.Threading.Thread.Sleep(5000);
                cont++;
                if (cont >= this.credenciales.NumeroIntentos)
                    superamosLosIntentos = true;
            }
            if (superamosLosIntentos)
                return null;
            return tResponse;
        }

        private string Doa(string requestId, string op_authorization)
        {
            return transact.DoA(requestId, this.credenciales.UserName, op_authorization);
        }

    }
}
