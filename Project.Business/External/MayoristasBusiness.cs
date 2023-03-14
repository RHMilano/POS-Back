using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Repository;
using Project.Services.Utils;

namespace Milano.BackEnd.Business
{
    /// <summary>
    /// Clase de negocio de mayoristas
    /// </summary>
    public class MayoristasBusiness : BaseBusiness
    {

        ProxyMayoristas.wsVentaMayoristaSoapClient wsVentaMayoristaSoapClient;
        ProxyNotaCredito.wsNotaCreditoSoapClient wsNotaCreditoSoapClient; // OCG Integracion de transferencias

        InformacionServiciosExternosRepository externosRepository;
        MayoristaRepository repository;
        InfoService inforService;
        TokenDto token;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token"></param>
        public MayoristasBusiness(TokenDto token)
        {
            this.token = token;
            repository = new MayoristaRepository();
            inforService = new InfoService();
            externosRepository = new InformacionServiciosExternosRepository();
            inforService = externosRepository.ObtenerInfoServicioExterno(17);
            wsVentaMayoristaSoapClient = new ProxyMayoristas.wsVentaMayoristaSoapClient();
            wsVentaMayoristaSoapClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(inforService.UrlService);
        }

        /// <summary>
        ///Busqueda de mayorista
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseBussiness<BusquedaMayoristaResponse> BusquedaMayorista(BusquedaMayoristaRequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                BusquedaMayoristaResponse respuesta = new BusquedaMayoristaResponse();
                Inspector inspector = new Inspector();
                ProxyMayoristas.InfoMayorista info = wsVentaMayoristaSoapClient.ConsultaMayorista(token.CodeStore, token.CodeBox, token.CodeEmployee, request.CodigoMayorista, request.Nombre, request.SoloActivos);
                respuesta.Calle = info.Calle;
                respuesta.CodigoMayorista = info.NumeroMayorista;
                respuesta.Ciudad = info.Ciudad;
                respuesta.CodigoPostal = info.CodigoPostal;
                respuesta.CodigoTienda = info.codigoTienda;
                respuesta.Colonia = info.Colonia;
                respuesta.CreditoDisponible = inspector.TruncarValor(info.CreditoDisponible);
                respuesta.Error = info.sError;
                respuesta.Estado = info.Estado;
                respuesta.Estatus = info.Estatus;
                respuesta.FechaUltimoPago = info.FechaUltimoPago.ToString();
                respuesta.LimiteCredito = inspector.TruncarValor(info.LimiteCredito);
                respuesta.Mensaje = info.sMensaje;
                respuesta.Municipio = info.Municipio;
                respuesta.Nombre = info.Nombre;
                respuesta.NumeroExterior = info.NumeroExterior;
                respuesta.NumeroInterior = info.NumeroInterior;
                respuesta.PagosPeriodoActual = inspector.TruncarValor(info.PagosPeriodoActual);
                respuesta.PorcentajeComision = inspector.TruncarValor(info.PorcentajeComision);
                respuesta.RFC = info.RFC;
                respuesta.Saldo = inspector.TruncarValor(info.Saldo);
                respuesta.Telefono = info.Telefono;
                respuesta.EstadoCuentaMayorista = new EstadoCuentaMayorista();
                respuesta.EstadoCuentaMayorista.Existe = info.estadoCuenta.bExiste;
                respuesta.EstadoCuentaMayorista.FechaCorte = info.estadoCuenta.FechaCorte.ToString();
                respuesta.EstadoCuentaMayorista.Anio = info.estadoCuenta.Año;
                respuesta.EstadoCuentaMayorista.Periodo = info.estadoCuenta.Periodo;
                respuesta.EstadoCuentaMayorista.FechaInicial = info.estadoCuenta.FechaInicial.ToString();
                respuesta.EstadoCuentaMayorista.FechaFinal = info.estadoCuenta.FechaFinal.ToString();
                respuesta.EstadoCuentaMayorista.FechaLimitePago = info.estadoCuenta.FechaLimitePago.ToString();
                respuesta.EstadoCuentaMayorista.LimiteCredito = info.estadoCuenta.LimiteCredito;
                respuesta.EstadoCuentaMayorista.SaldoAnterior = info.estadoCuenta.SaldoAnterior;
                respuesta.EstadoCuentaMayorista.Compras = info.estadoCuenta.Compras;
                respuesta.EstadoCuentaMayorista.Pagos = info.estadoCuenta.Pagos;
                respuesta.EstadoCuentaMayorista.NotasDeCredito = info.estadoCuenta.NotasDeCredito;
                respuesta.EstadoCuentaMayorista.NotasDeCargo = info.estadoCuenta.NotasDeCargo;
                respuesta.EstadoCuentaMayorista.SaldoActual = info.estadoCuenta.SaldoActual;
                respuesta.EstadoCuentaMayorista.PagoQuincenal = info.estadoCuenta.PagoQuincenal;
                respuesta.EstadoCuentaMayorista.PagoMinimo = info.estadoCuenta.PagoMinimo;
                respuesta.EstadoCuentaMayorista.PagoVencido = info.estadoCuenta.PagoVencido;
                respuesta.EstadoCuentaMayorista.CreditoDisponible = info.estadoCuenta.CreditoDisponible;
                respuesta.EstadoCuentaMayorista.NumeroAtrasos = info.estadoCuenta.NumeroAtrasos;
                return respuesta;
            });
        }

        /// <summary>
        ///Busqueda de mayoristas
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseBussiness<BusquedaMayoristasResponse[]> BusquedaMayoristas(BusquedaMayoristasRequest request)
        {
            return tryCatch.SafeExecutor(() =>
            {
                List<BusquedaMayoristasResponse> list = new List<BusquedaMayoristasResponse>();
                BusquedaMayoristasResponse respuestaGral = new BusquedaMayoristasResponse();
                Inspector inspector = new Inspector();
                ProxyMayoristas.InfoMayoristas info = wsVentaMayoristaSoapClient.ConsultaMayoristas(token.CodeStore, token.CodeBox, token.CodeEmployee, request.CodigoMayorista, request.Nombre, request.SoloActivos, request.SoloTiendaActual);
                respuestaGral.Error = info.sError;
                respuestaGral.Mensaje = info.sMensaje;
                if (info.infoMayorista != null)
                {
                    foreach (var r in info.infoMayorista)
                    {
                        BusquedaMayoristasResponse respuesta = new BusquedaMayoristasResponse();
                        respuesta.Calle = r.Calle;
                        respuesta.CodigoMayorista = r.NumeroMayorista;
                        respuesta.Ciudad = r.Ciudad;
                        respuesta.CodigoPostal = r.CodigoPostal;
                        respuesta.CodigoTienda = r.codigoTienda;
                        respuesta.Colonia = r.Colonia;
                        respuesta.CreditoDisponible = inspector.TruncarValor(r.CreditoDisponible);
                        respuesta.Estado = r.Estado;
                        respuesta.Estatus = r.Estatus;
                        respuesta.FechaUltimoPago = r.FechaUltimoPago.ToString();
                        respuesta.LimiteCredito = inspector.TruncarValor(r.LimiteCredito);
                        /*respuesta.Error = r.sError;
                        respuesta.Mensaje = r.sMensaje;*/
                        respuesta.Error = respuestaGral.Error;
                        respuesta.Mensaje = respuestaGral.Mensaje;
                        respuesta.Municipio = r.Municipio;
                        respuesta.Nombre = r.Nombre;
                        respuesta.NumeroExterior = r.NumeroExterior;
                        respuesta.NumeroInterior = r.NumeroInterior;
                        respuesta.PagosPeriodoActual = inspector.TruncarValor(r.PagosPeriodoActual);
                        respuesta.PorcentajeComision = inspector.TruncarValor(r.PorcentajeComision);
                        respuesta.RFC = r.RFC;
                        respuesta.Saldo = inspector.TruncarValor(r.Saldo);
                        respuesta.Telefono = r.Telefono;
                        list.Add(respuesta);
                    }
                }
                else
                {
                    list.Add(respuestaGral);
                }
                return list.ToArray();
            });
        }


        /// <summary>
        /// Buscar cliente final
        /// </summary>
        /// <param name="busquedaClienteFinalRequest">Dto con parametros de busqueda del cliente final</param>
        /// <returns>Resultado de la operación</returns>
        public ResponseBussiness<BusquedaClienteFinalResponse[]> BuscarClienteFinal(BusquedaClienteFinalRequest busquedaClienteFinalRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                List<BusquedaClienteFinalResponse> listaClientes = new List<BusquedaClienteFinalResponse>();
                ProxyMayoristas.InfoClientesFinales infoClientesFinales = wsVentaMayoristaSoapClient.BuscarClienteFinal(this.token.CodeStore, this.token.CodeBox, this.token.CodeEmployee,
                    busquedaClienteFinalRequest.CodigoMayorista, busquedaClienteFinalRequest.CodigoClienteFinal, busquedaClienteFinalRequest.Nombres,
                    busquedaClienteFinalRequest.Apellidos, busquedaClienteFinalRequest.Ine, busquedaClienteFinalRequest.Rfc);
                if (infoClientesFinales.sError == "")
                {
                    foreach (ProxyMayoristas.ClientesFinales cliente in infoClientesFinales.clientesFinales)
                    {
                        BusquedaClienteFinalResponse clienteEncontrado = new BusquedaClienteFinalResponse();
                        clienteEncontrado.Apellidos = cliente.sApellidos;
                        clienteEncontrado.CodigoClienteFinal = cliente.codigoClienteFinal;
                        clienteEncontrado.CodigoMayorista = cliente.codigoMayorista;
                        clienteEncontrado.Error = cliente.sError;
                        clienteEncontrado.FechaNacimiento = cliente.dtFechaNatimiento.ToShortDateString();
                        clienteEncontrado.Ine = cliente.sIFE;
                        clienteEncontrado.Mensaje = cliente.sMensaje;
                        clienteEncontrado.Nombres = cliente.sNombre;
                        clienteEncontrado.Rfc = cliente.sRFC;
                        clienteEncontrado.Sexo = cliente.sSexo;
                        clienteEncontrado.Telefono = cliente.sSexo;
                        listaClientes.Add(clienteEncontrado);
                    }
                }
                return listaClientes.ToArray();
            });
        }

        /// <summary>
        /// Agregar cliente final
        /// </summary>
        /// <param name="altaClienteFinalRequest">Dto con los datos del cliente</param>
        /// <returns>Resultado de la operacion</returns>
        public ResponseBussiness<BusquedaClienteFinalResponse> AgregarCliente(AltaClienteFinalRequest altaClienteFinalRequest)
        {
            return tryCatch.SafeExecutor(() =>
            {
                BusquedaClienteFinalResponse clienteGenerado = new BusquedaClienteFinalResponse();
                if (altaClienteFinalRequest.NumeroInterior == string.Empty)
                    altaClienteFinalRequest.NumeroInterior = "NA";
                int anio = int.Parse(altaClienteFinalRequest.Rfc.Substring(4, 2));
                int mes = int.Parse(altaClienteFinalRequest.Rfc.Substring(6, 2));
                int dia = int.Parse(altaClienteFinalRequest.Rfc.Substring(8, 2));
                DateTime fechaRfc = new DateTime();
                if (anio >= 0 && anio <= DateTime.Now.Year - 2000)
                {
                    fechaRfc = new DateTime(2000 + anio, mes, dia);
                }
                else
                {
                    fechaRfc = new DateTime(1900 + anio, mes, dia);
                }
                DateTime nacimiento = DateTime.Parse(altaClienteFinalRequest.FechaNacimiento);
                int edad = DateTime.Today.AddTicks(-nacimiento.Ticks).Year - 1;
                altaClienteFinalRequest.Municipio = altaClienteFinalRequest.Ciudad;
                if (fechaRfc == DateTime.Parse(altaClienteFinalRequest.FechaNacimiento))
                {
                    if (edad >= 18)
                    {
                        ProxyMayoristas.ClientesFinales cliente = wsVentaMayoristaSoapClient.CrearClienteFinal(this.token.CodeStore, this.token.CodeBox, this.token.CodeEmployee, altaClienteFinalRequest.Ine, altaClienteFinalRequest.Rfc, altaClienteFinalRequest.Nombre,
                            altaClienteFinalRequest.Apellidos, DateTime.Parse(altaClienteFinalRequest.FechaNacimiento), altaClienteFinalRequest.Sexo, altaClienteFinalRequest.Calle, altaClienteFinalRequest.NumeroExterior, altaClienteFinalRequest.NumeroInterior,
                            altaClienteFinalRequest.Colonia, altaClienteFinalRequest.Municipio, altaClienteFinalRequest.Cp, altaClienteFinalRequest.Ciudad, altaClienteFinalRequest.Estado, altaClienteFinalRequest.CodigoMayorista, altaClienteFinalRequest.Telefono);
                        clienteGenerado.Apellidos = cliente.sApellidos;
                        clienteGenerado.CodigoClienteFinal = cliente.codigoClienteFinal;
                        clienteGenerado.CodigoMayorista = cliente.codigoMayorista;
                        clienteGenerado.Error = cliente.sError;
                        clienteGenerado.FechaNacimiento = cliente.dtFechaNatimiento.ToShortDateString();
                        clienteGenerado.Ine = cliente.sIFE;
                        clienteGenerado.Mensaje = cliente.sMensaje;
                        clienteGenerado.Nombres = cliente.sNombre;
                        clienteGenerado.Rfc = cliente.sRFC;
                        clienteGenerado.Sexo = cliente.sSexo;
                        clienteGenerado.Telefono = cliente.sSexo;
                    }
                    else
                    {
                        clienteGenerado.CodigoClienteFinal = 0;
                        clienteGenerado.Error = this.repository.ObtenerMensajeMenorEdad().CodeDescription;
                    }
                }
                else
                {
                    clienteGenerado.CodigoClienteFinal = 0;
                    clienteGenerado.Error = this.repository.ObtenerMensajeFechasInvalidaRFC().CodeDescription;
                }
                return clienteGenerado;
            });
        }

        /// <summary>
        /// Validar vale de cliente final
        /// </summary>
        /// <param name="codigoVale"></param>
        /// <returns></returns>
        public InfoValeResponse ValidarVale(int codigoVale)
        {
            ProxyMayoristas.InfoVale infoVale = wsVentaMayoristaSoapClient.ConsultarVale(this.token.CodeStore, this.token.CodeBox, this.token.CodeEmployee, codigoVale);
            InfoValeResponse infoValeResponse = new InfoValeResponse();
            infoValeResponse.Error = infoVale.sError;
            infoValeResponse.Mensaje = infoVale.sMensaje;
            if (infoVale.sError == "")
            {
                infoValeResponse.CodigoMayorista = infoVale.codigoMayorista;
                infoValeResponse.CodigoTiendaPago = infoVale.codigoTiendaPago;
                infoValeResponse.Estatus = infoVale.Estatus;
                infoValeResponse.FechaCanje = infoVale.FechaCanje;
                infoValeResponse.FechaCreacion = infoVale.FechaCreacion;
                infoValeResponse.FechaModificacion = infoVale.UltimaModificacion;
                infoValeResponse.NoAutorizacion = infoVale.NoAutorizacion;
                infoValeResponse.NoRevision = infoVale.NoRevision;
                infoValeResponse.Referencia = infoVale.referencia;
            }
            return infoValeResponse;
        }






        /// <summary>
        /// Pago de crédito de parte de un Mayorista
        /// </summary>
        /// <param name="pagoMayoristaRequest">DTO con propiedades del Pago del Mayorista</param>
        /// <param name="codigoTransaccion">Código de la transacción</param>
        /// <returns>Resultado de la operación </returns>
        public OperationResponse PagoCreditoMayorista(PagoCreditoMayoristaRequest pagoMayoristaRequest, int codigoTransaccion)
        {
            OperationResponse operation = new OperationResponse();
            DateTime fechaVenta = DateTime.Now;
            ProxyMayoristas.InfoVenta respuesta = wsVentaMayoristaSoapClient.RealizarPago(this.token.CodeStore, this.token.CodeBox, this.token.CodeEmployee, pagoMayoristaRequest.CodigoMayorista
                , double.Parse(pagoMayoristaRequest.ImportePago.ToString()), pagoMayoristaRequest.FolioOperacionAsociada, codigoTransaccion, fechaVenta);
            if (respuesta.sError == "")
            {
                operation.CodeNumber = "1";
                operation.CodeDescription = respuesta.sMensaje;
            }
            else
            {
                operation.CodeNumber = "0";
                operation.CodeDescription = respuesta.sError;
            }
            return operation;
        }

        /// <summary>
        /// Pago de mayorista
        /// </summary>
        /// <param name="procesarMovimientoMayorista">Dto con propiedades del pago</param>
        /// <param name="codigoTransaccion">Código de la transacción</param>
        /// <returns>Resultado de la operación </returns>
        public OperationResponse PagoVentaMayorista(ProcesarMovimientoMayorista procesarMovimientoMayorista, int codigoTransaccion)
        {
            OperationResponse operation = new OperationResponse();
            try
            {
                DateTime fechaVenta = DateTime.Now;
                ProxyMayoristas.InfoVenta respuesta = wsVentaMayoristaSoapClient.RealizarVenta(this.token.CodeStore, this.token.CodeBox, this.token.CodeEmployee, procesarMovimientoMayorista.CodigoMayorista, procesarMovimientoMayorista.CodigoClienteFinal,
                     procesarMovimientoMayorista.NumeroVale, double.Parse(procesarMovimientoMayorista.ImporteVentaTotal.ToString()), procesarMovimientoMayorista.MontoFinanciado, procesarMovimientoMayorista.FolioOperacionAsociada, codigoTransaccion, fechaVenta);
                if (respuesta.sError == "")
                {
                    operation.CodeNumber = "1";
                    operation.CodeDescription = respuesta.sMensaje;
                }
                else
                {
                    operation.CodeNumber = "0";
                    operation.CodeDescription = respuesta.sError;
                }
            }
            catch (Exception ex)
            {
                operation.CodeNumber = "0";
                operation.CodeDescription = ex.Message;
            }
            return operation;
        }

    }
}
