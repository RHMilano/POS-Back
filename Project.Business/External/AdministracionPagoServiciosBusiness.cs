using Milano.BackEnd.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Milano.BackEnd.Repository;
using SecurityCCK;
using System.Web.Script.Serialization;
using System.Threading;
using Milano.BackEnd.Dto.General;

namespace Milano.BackEnd.Business
{
    /// <summary>
    /// Clase administrar las opciones de tiempo aire
    /// </summary>
    public class AdministracionPagoServiciosBusiness : BaseBusiness
    {

        /// <summary>
        /// Variables de clase
        /// </summary>
        PagoServiciosRepository repository;
        CredencialesServicioExterno credenciales;
        ProxyPagoServicios.transactSoapClient transact;
        InformacionServiciosExternosRepository externosRepository;
        InfoService inforService;
        TokenDto token;

        /// <summary>
        /// Constructor de tiempo aire
        /// </summary>
        public AdministracionPagoServiciosBusiness(TokenDto token)
        {
            this.token = token;
            repository = new PagoServiciosRepository(token);
            inforService = new InfoService();
            externosRepository = new InformacionServiciosExternosRepository();
            inforService = externosRepository.ObtenerInfoServicioExterno(12);
            transact = new ProxyPagoServicios.transactSoapClient();
            transact.Endpoint.Address = new System.ServiceModel.EndpointAddress(inforService.UrlService);
            credenciales = new CredencialesServicioExterno();
            credenciales = new InformacionServiciosExternosBusiness().ObtenerCredencialesPagoServicios();
            SecurityCCK.encripta encripta = new encripta();
            object userName = credenciales.UserName;
            object pwd = credenciales.Password;
            var obj = encripta.Encrypt(ref userName);
            var obje2 = encripta.Encrypt(ref pwd);
            this.credenciales.UserName = obj.ToString();
            this.credenciales.Password = obje2.ToString();
        }

        /// <summary>
        /// Obtenemos la lista de Empresas
        /// </summary>
        /// <returns>lista de empresas</returns>
        public ResponseBussiness<CompaniasPagoServiciosResponse[]> ObtenerListaEmpresas()
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.ObtenerListaEmpresas();
            });
        }

        /// <summary>
        /// Metodo para obtener los montos de recarga por compañia
        /// </summary>
        /// <param name="codigoEmpresa">codigo de la empresa</param>
        /// <returns>lista de montos</returns>
        public ResponseBussiness<ProductsResponse[]> ObtenerProductos(string codigoEmpresa)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.ObtenerProductos(codigoEmpresa);
            });
        }

        private List<ElementoFormulario> ObtenerElementos(ProxyPagoServicios.ArrayOfElement elementos)
        {
            List<ElementoFormulario> listaElemento = new List<ElementoFormulario>();
            foreach (ProxyPagoServicios.element info in elementos)
            {
                ElementoFormulario elemento = new ElementoFormulario() { Valor = info.m_value, Nombre = info.m_name, SoloLectura = info.m_readonly, TipoElementoFormulario = info.m_eType.ToString() };

                if (info.m_input != null)
                {
                    elemento.DefinicionElementoInput = new InputElement();
                    elemento.DefinicionElementoInput.TipoInput = info.m_input.m_iType.ToString();
                    elemento.DefinicionElementoInput.ValorMaximo = info.m_input.m_iMaxValue;
                    elemento.DefinicionElementoInput.ValorMinimo = info.m_input.m_iMinValue;
                }
                if (info.m_select != null)
                {
                    elemento.DefinicionElementoSelect = new SelectElement();
                    List<OpcionSelect> listaOpcion = new List<OpcionSelect>();
                    foreach (ProxyPagoServicios.option option in info.m_select.m_options)
                    {
                        OpcionSelect nuevaOpcion = new OpcionSelect();
                        nuevaOpcion.Texto = option.m_text;
                        nuevaOpcion.Valor = option.m_value;
                        nuevaOpcion.Cantidad = option.m_amount;
                        listaOpcion.Add(nuevaOpcion);
                    }
                    elemento.DefinicionElementoSelect.Opciones = listaOpcion.ToArray();
                }
                listaElemento.Add(elemento);
            }
            return listaElemento;
        }

        public ResponseBussiness<PagoServiciosResponse> OpcionesAdicionales(InfoElementosRequest pagoServiciosRequest)
        {
            SecurityCCK.encripta encripta = new encripta();
            return tryCatch.SafeExecutor(() =>
            {
                object cuenta = pagoServiciosRequest.Cuenta;
                var cuanteEncripatada = encripta.Encrypt(ref cuenta);
                string xmlDevDat = "";
                if (pagoServiciosRequest.InfoAdicional != null)
                {
                    xmlDevDat = this.Form2_xmlDevData(this.ObtenerElementosAdicionales(pagoServiciosRequest.InfoAdicional, pagoServiciosRequest.InfoAdicional.ModuloId));
                }
                ProxyPagoServicios.InfoResponse infoResponse = GetInfo(pagoServiciosRequest.SkuCode, cuanteEncripatada.ToString(), xmlDevDat);
                PagoServiciosResponse pagos = new PagoServiciosResponse();
                pagos.ModuloId = infoResponse.m_moduleId;
                pagos.ElementosFormulario = this.ObtenerElementos(infoResponse.m_form).ToArray();
                return pagos;
            });
        }

        /// <summary>
        /// Pago de servicio
        /// </summary>
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> PagoServicio(PagoServiciosRequest pagoServiciosRequest, float monto, string folio)
        {
            SecurityCCK.encripta encripta = new encripta();
            int intento = 1;
            string resultadoConfirmarEnvioRecibido = "";
            return tryCatch.SafeExecutor(() =>
            {
                OperationResponse operation = new OperationResponse();
                string requestId = string.Empty;
                requestId = GetRequestId(0);
                if (requestId != "")
                {
                    object cuenta = pagoServiciosRequest.Cuenta;
                    var cuanteEncripatada = encripta.Encrypt(ref cuenta);
                    ProxyPagoServicios.Form formulario = this.ObtenerElementosAdicionales(pagoServiciosRequest.InfoAdicional, pagoServiciosRequest.InfoAdicional.ModuloId);
                    ProxyPagoServicios.TResponse respuestaDot = GetDot(requestId, this.credenciales.UserName, pagoServiciosRequest.SkuCodePagoServicio, cuanteEncripatada.ToString(), monto, this.token.CodeBox, formulario);

                    if (respuestaDot.rcode == 2)//En proceso
                    {
                        while (intento <= this.credenciales.NumeroIntentos)
                        {
                            Thread.Sleep(5000 * 1);
                            ProxyPagoServicios.TResponse respuestaVerificarTransaccion = this.ChecarTransaccion(requestId, this.credenciales.UserName);
                            //si la respuesta es qeu no esta en proceso
                            if (respuestaVerificarTransaccion.rcode != 2)
                            {
                                resultadoConfirmarEnvioRecibido = this.TerminarTransaccionCorrecta(respuestaDot.rcode, requestId, respuestaVerificarTransaccion.op_authorization);
                                intento = 1000;
                            }
                            intento++;
                        }
                        if (resultadoConfirmarEnvioRecibido == "")
                        {
                            ProxyPagoServicios.ReverseResponse respuestaDor = this.DoR(requestId, this.credenciales.UserName);

                            operation.CodeNumber = respuestaDor.rcode.ToString();
                            operation.CodeDescription = respuestaDor.rcode_description;
                        }
                        else
                        {
                            operation.CodeNumber = "1";
                            operation.CodeDescription = resultadoConfirmarEnvioRecibido;
                        }
                    }
                    else if (respuestaDot.rcode == 0 || respuestaDot.rcode == 1) //termino correctamente con ó sin autorizacion
                    {
                        resultadoConfirmarEnvioRecibido = this.TerminarTransaccionCorrecta(respuestaDot.rcode, requestId, respuestaDot.op_authorization);
                        operation.CodeNumber = "1";
                        operation.CodeDescription = resultadoConfirmarEnvioRecibido;

                        this.repository.RegistrarAutorizacionPago(pagoServiciosRequest.SkuCodePagoServicio, pagoServiciosRequest.SkuCode, folio, respuestaDot.op_authorization);
                    }
                    else
                    {
                        operation.CodeNumber = "0";
                        operation.CodeDescription = respuestaDot.rcode_description;
                    }
                }
                else
                {
                    operation.CodeDescription = "Error del Web Service, contactar a Administrador del Sistema";
                    operation.CodeNumber = "0";
                }
                return operation;
            });
        }

        private string TerminarTransaccionCorrecta(int codigoRequireComprobar, string requestId, string autorizacion)
        {
            string resultadoConfirmarEnvioRecibido = "";
            if (codigoRequireComprobar == 10) // requiere comprobación
            {
                ProxyPagoServicios.TResponse infoResponseDoc = this.DoC(requestId, this.credenciales.UserName);
                if (infoResponseDoc.rcode == 0) // comprobación exitosa
                {
                    resultadoConfirmarEnvioRecibido = this.DoA(requestId, this.credenciales.UserName, autorizacion);
                }
            }
            else // no require comprobacion
            {
                if (codigoRequireComprobar == 0 || codigoRequireComprobar == 1) // comprobación exitosa
                {
                    resultadoConfirmarEnvioRecibido = this.DoA(requestId, this.credenciales.UserName, autorizacion);
                }
            }
            return resultadoConfirmarEnvioRecibido;
        }


        private string Form2_xmlDevData(ProxyPagoServicios.Form formOpciones)
        {
            return transact.form2_xmlDevData(formOpciones);
        }


        private ProxyPagoServicios.ReverseResponse DoR(string requestId, string userName)
        {
            ProxyPagoServicios.ReverseResponse response = transact.DoR(requestId, userName);
            return response;
        }

        private ProxyPagoServicios.TResponse DoC(string requestId, string userName)
        {
            ProxyPagoServicios.TResponse response = transact.DoC(requestId, userName);
            return response;
        }

        private string DoA(string requestId, string userName, string autorizacion)
        {
            string respuesta = transact.DoA(requestId, userName, autorizacion);
            return respuesta;
        }

        private ProxyPagoServicios.TResponse ChecarTransaccion(string requestId, string userName)
        {
            ProxyPagoServicios.TResponse response = transact.CheckTransaction(requestId, userName);
            return response;
        }

        private ProxyPagoServicios.TResponse GetDot(string requestId, string userName, string skuCode, string numeroCuenta, float monto, int numeroCaja, ProxyPagoServicios.Form form)
        {

            ProxyPagoServicios.TResponse respuesta = transact.DoT(requestId, userName, skuCode, numeroCuenta, monto, numeroCaja, form);
            return respuesta;

        }

        private ProxyPagoServicios.InfoResponse GetInfo(string sku, string cuenta, string xmlDev)
        {
            return transact.GetInfo(this.credenciales.UserName, this.credenciales.Password, this.credenciales.Licence, sku, cuenta, xmlDev);
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

        private ProxyPagoServicios.Form ObtenerElementosAdicionales(PagoServiciosInfoAdicional infoAdicional, int moduleId)
        {
            ProxyPagoServicios.Form formulario = new ProxyPagoServicios.Form();
            ProxyPagoServicios.ArrayOfElement lista = new ProxyPagoServicios.ArrayOfElement();
            foreach (ElementoFormulario elemento in infoAdicional.ElementosFormulario)
            {
                ProxyPagoServicios.element nuevoElemento = new ProxyPagoServicios.element();
                nuevoElemento.m_value = elemento.Valor;
                nuevoElemento.m_name = elemento.Nombre;
                nuevoElemento.m_readonly = elemento.SoloLectura;
                if (elemento.TipoElementoFormulario == "input")
                {
                    nuevoElemento.m_eType = ProxyPagoServicios.elementType.input;
                    nuevoElemento.m_input = new ProxyPagoServicios.inputElement();
                    if (elemento.DefinicionElementoInput.TipoInput == "dinero")
                    {
                        nuevoElemento.m_input.m_iType = ProxyPagoServicios.iType.dinero;
                    }
                    else if (elemento.DefinicionElementoInput.TipoInput == "fecha")
                    {
                        nuevoElemento.m_input.m_iType = ProxyPagoServicios.iType.fecha;
                    }
                    else if (elemento.DefinicionElementoInput.TipoInput == "numero")
                    {
                        nuevoElemento.m_input.m_iType = ProxyPagoServicios.iType.numero;
                    }
                    else if (elemento.DefinicionElementoInput.TipoInput == "password")
                    {
                        nuevoElemento.m_input.m_iType = ProxyPagoServicios.iType.password;
                    }
                    else if (elemento.DefinicionElementoInput.TipoInput == "submit")
                    {
                        nuevoElemento.m_input.m_iType = ProxyPagoServicios.iType.submit;
                    }
                    else if (elemento.DefinicionElementoInput.TipoInput == "texto")
                    {
                        nuevoElemento.m_input.m_iType = ProxyPagoServicios.iType.texto;
                    }
                    nuevoElemento.m_input.m_iMaxValue = elemento.DefinicionElementoInput.ValorMaximo;
                    nuevoElemento.m_input.m_iMinValue = elemento.DefinicionElementoInput.ValorMinimo;
                }
                else if (elemento.TipoElementoFormulario == "select")
                {
                    nuevoElemento.m_eType = ProxyPagoServicios.elementType.select;
                    nuevoElemento.m_select = new ProxyPagoServicios.selectElement();
                    nuevoElemento.m_select.m_options = new ProxyPagoServicios.ArrayOfOption();
                    foreach (OpcionSelect opcionSelect in elemento.DefinicionElementoSelect.Opciones)
                    {
                        ProxyPagoServicios.option opcion = new ProxyPagoServicios.option();
                        opcion.m_amount = opcionSelect.Cantidad;
                        opcion.m_text = opcionSelect.Texto;
                        opcion.m_value = opcionSelect.Valor;
                        nuevoElemento.m_select.m_options.Add(opcion);
                    }
                }
                lista.Add(nuevoElemento);
            }
            formulario.m_form = lista;
            formulario.m_moduleId = moduleId;
            return formulario;
        }


    }
}
