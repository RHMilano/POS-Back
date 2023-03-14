using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.Catalogs;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Dto.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Milano.BackEnd.Repository
{

    /// <summary>
    /// Clase que gestiona las operaciones referentes a las formas de pago
    /// </summary>
    public class FormasPagoRepository : BaseRepository
    {
        /// <summary>
        /// Método para asociar las formas de pago a una transacción
        /// </summary>
        /// <param name="codeStore">Código de tienda</param>      
        /// <param name="codeBox">Código de caja</param>      
        /// <param name="codeEmployee">Código de empleado cajero</param>     
        /// <param name="folioOperacion">Folio de la operación</param>
        /// <param name="formasPagoUtilizadas">Formas de pago utilizadas</param>        
        /// <param name="clasificacionVenta">Clasificación de la venta</param>     
        public void AsociarFormasPago(int codeStore, int codeBox, int codeEmployee, String folioOperacion, FormaPagoUtilizado[] formasPagoUtilizadas, string clasificacionVenta)
        {
            Divisa[] divisas = this.ObtenerFormasPagoEfectivoMonedaExtranjera(codeStore);
            List<string> listaDivisas = new List<string>();
            
            foreach (Divisa divisa in divisas)
            {
                listaDivisas.Add(divisa.Codigo);
            }

            FormaPagoResponse[] formasPagoVales = this.getFormasPagoVales(codeStore);
            List<string> listaFormasPagoVales = new List<string>();

            foreach (FormaPagoResponse formaPagoVale in formasPagoVales)
            {
                listaFormasPagoVales.Add(formaPagoVale.CodigoFormaPago);
            }

            PaymentProcessingRepository paymentProcessingRepository = new PaymentProcessingRepository();
            foreach (var formaPagoUtilizada in formasPagoUtilizadas)
            {
                // Pagos que deben registrarse exitosamente al momento de finalizar
                if (formaPagoUtilizada.Estatus == "PP")
                {
                    formaPagoUtilizada.Estatus = "PE";
                    // Pagos con Vales
                    if (validarCodigoFormaPago(formaPagoUtilizada.CodigoFormaPagoImporte, listaFormasPagoVales.ToArray()))
                    {
                        paymentProcessingRepository.ProcesarMovimientoVales(codeStore, codeBox, codeEmployee, folioOperacion, formaPagoUtilizada, clasificacionVenta);
                        // Se procesan las promociones por venta
                        foreach (var item in formaPagoUtilizada.DescuentosPromocionalesPorVentaAplicados.DescuentoPromocionesAplicados)
                        {
                            OperationResponse response = new OperationResponse();
                            response = paymentProcessingRepository.PersistirPromocionesVenta(folioOperacion, codeStore, codeBox, item.ImporteDescuento
                                                                    , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);
                        }
                        // Se procesan las promociones por línea de venta
                        foreach (var item in formaPagoUtilizada.DescuentosPromocionalesPorLineaAplicados.DescuentoPromocionesAplicados)
                        {
                            OperationResponse response = new OperationResponse();
                            response = paymentProcessingRepository.PersistirPromocionesLineaVenta(folioOperacion, codeStore, codeBox, item.Secuencia, item.ImporteDescuento
                                                                    , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);
                        }
                    }
                    // Pagos con Efectivo y con Moneda Extranjera
                    if (validarCodigoFormaPago(formaPagoUtilizada.CodigoFormaPagoImporte, listaDivisas.ToArray()))
                    {
                        paymentProcessingRepository.ProcesarMovimientoEfectivo(codeStore, codeBox, codeEmployee, folioOperacion, formaPagoUtilizada);
                        // Se procesan las promociones por venta
                        foreach (var item in formaPagoUtilizada.DescuentosPromocionalesPorVentaAplicados.DescuentoPromocionesAplicados)
                        {
                            OperationResponse response = new OperationResponse();
                            response = paymentProcessingRepository.PersistirPromocionesVenta(folioOperacion, codeStore, codeBox, item.ImporteDescuento
                                                                    , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);
                        }
                        // Se procesan las promociones por línea de venta
                        foreach (var item in formaPagoUtilizada.DescuentosPromocionalesPorLineaAplicados.DescuentoPromocionesAplicados)
                        {
                            OperationResponse response = new OperationResponse();
                            response = paymentProcessingRepository.PersistirPromocionesLineaVenta(folioOperacion, codeStore, codeBox, item.Secuencia, item.ImporteDescuento
                                                                    , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);
                        }
                    }

                    //// Transacciones
                    //if (formaPagoUtilizada.CodigoFormaPagoImporte == "TR"  )
                    //{
                    //    paymentProcessingRepository.ProcesarMovimientoEfectivo(codeStore, codeBox, codeEmployee, folioOperacion, formaPagoUtilizada);
                        
                    //    //// Se procesan las promociones por venta
                    //    //foreach (var item in formaPagoUtilizada.DescuentosPromocionalesPorVentaAplicados.DescuentoPromocionesAplicados)
                    //    //{
                    //    //    OperationResponse response = new OperationResponse();
                    //    //    response = paymentProcessingRepository.PersistirPromocionesVenta(folioOperacion, codeStore, codeBox, item.ImporteDescuento
                    //    //                                            , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);
                    //    //}
                        
                    //    //// Se procesan las promociones por línea de venta
                    //    //foreach (var item in formaPagoUtilizada.DescuentosPromocionalesPorLineaAplicados.DescuentoPromocionesAplicados)
                    //    //{
                    //    //    OperationResponse response = new OperationResponse();
                    //    //    response = paymentProcessingRepository.PersistirPromocionesLineaVenta(folioOperacion, codeStore, codeBox, item.Secuencia, item.ImporteDescuento
                    //    //                                            , item.CodigoPromocionAplicado, item.DescripcionCodigoPromocionAplicado, item.PorcentajeDescuento, item.CodigoRazonDescuento, item.FormaPagoCodigoPromocionAplicado);
                    //    //}
                    //}

                }

                // Pagos que ya estan registrados y solo deben asociarse al momento de finalizar
                if (formaPagoUtilizada.Estatus == "PR")
                {
                    paymentProcessingRepository.AsociarMovimientoRegistrado(codeStore, codeBox, folioOperacion, formaPagoUtilizada);
                }
            }
        }

        private Boolean validarCodigoFormaPago(string codigoFormaPago, string[] codigosFormasPago)
        {
            foreach (string item in codigosFormasPago)
            {
                if (codigoFormaPago.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }


        private Divisa[] ObtenerFormasPagoEfectivoMonedaExtranjera(int codeStore)
        {
            List<Divisa> list = new List<Divisa>();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", codeStore);

            foreach (var r in data.GetDataReader("[dbo].[sp_vanti_ObtenerFormasPagoMonedaExtEfectivo]", parameters))
            {
                Divisa elemento = new Divisa();
                elemento.Codigo = r.GetValue(0).ToString();
                elemento.Descripcion = r.GetValue(1).ToString();

                list.Add(elemento);
            }
            return list.ToArray();
        }

        /// <summary>
        /// Obtener las diferentes formas de pago
        /// </summary>        
        /// <returns>Lista de formas de pago</returns>
        public FormaPagoResponse[] getFormasPagoVales(int CodeStore)
        {
            List<FormaPagoResponse> list = new List<FormaPagoResponse>();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", CodeStore);
            foreach (var r in data.GetDataReader("[dbo].[sp_vanti_ObtenFormasPagoVales]", parameters))
            {
                FormaPagoResponse FormaPago = new FormaPagoResponse();
                FormaPago.CodigoFormaPago = r.GetValue(0).ToString();
                FormaPago.DescripcionFormaPago = r.GetValue(1).ToString();
                FormaPago.DescripcionCorta = r.GetValue(2).ToString();
                FormaPago.RequiereDocumento = Convert.ToInt32(r.GetValue(3));
                FormaPago.CodigoMoneda = r.GetValue(4).ToString();
                FormaPago.EsTarjetaDeRegalo = Convert.ToByte(r.GetValue(5));
                FormaPago.EsCupon = Convert.ToByte(r.GetValue(6));
                list.Add(FormaPago);
            }
            return list.ToArray();
        }


        /// <summary>
        /// Regresa formas de pago en moneda extranjera 
        /// </summary>
        /// <param name="codigoCaja">Número de la caja</param>
        /// <param name="codigoTienda">Codigo de la tienda a buscar</param>
        /// <param name="folioOperacion">Folio de la operación asociada</param>
        /// <param name="folioDevolucion">Folio de la devolución asociada</param>
        /// <param name="tipoCabeceraVenta">Tipo cabecera venta asociada</param>
        /// <returns></returns>
        public ConfigGeneralesCajaTiendaFormaPago[] GetConfigFormasPagoExt(int codigoCaja, int codigoTienda, String folioOperacion, String folioDevolucion, String tipoCabeceraVenta)
        {
            ConfigGeneralesCajaTiendaFormaPago[] configGeneralesFp = new ConfigGeneralesCajaTiendaFormaPago[0];
            List<ConfigGeneralesCajaTiendaFormaPago> confGralFormaPagoList = new List<ConfigGeneralesCajaTiendaFormaPago>();
            ConfigGeneralesCajaTiendaFormaPago confGralFormaPagoItem;

            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioOperacion", folioOperacion);
            parameters.Add("@FolioDevolucion", folioDevolucion);
            parameters.Add("@TipoCabeceraVenta", tipoCabeceraVenta);
            parameters.Add("@CodigoTienda", codigoTienda);
            parameters.Add("@CodigoCaja", codigoCaja);

            foreach (var result in data.GetDataReader("[dbo].[sp_vanti_ConfiguracionFormasPagoExtranjera]", parameters))
            {
                confGralFormaPagoItem = new ConfigGeneralesCajaTiendaFormaPago();
                confGralFormaPagoItem.IdentificadorFormaPago = result.GetValue(0).ToString();
                confGralFormaPagoItem.CodigoFormaPago = result.GetValue(0).ToString();
                confGralFormaPagoItem.DescripcionFormaPago = result.GetValue(1).ToString();
                confGralFormaPagoList.Add(confGralFormaPagoItem);
            }

            return confGralFormaPagoList.ToArray();
        }

        /// <summary>
        /// Busca y regresa las formas de pago que no son moneda extranjera
        /// </summary>
        /// <param name="codigoCaja">Número de la caja</param>
        /// <param name="codigoTienda">Codigo de la tienda a buscar</param>
        /// <param name="folioOperacion">Folio de la operación asociada</param>
        /// <param name="folioDevolucion">Folio de la devolución asociada</param>
        /// <param name="tipoCabeceraVenta">Tipo cabecera venta asociada</param>
        /// <returns></returns>
        public ConfigGeneralesCajaTiendaFormaPago[] GetConfigFormasPago(int codigoCaja, int codigoTienda, String folioOperacion, String folioDevolucion, String tipoCabeceraVenta)
        {
            ConfigGeneralesCajaTiendaFormaPago[] configGeneralesFp = new ConfigGeneralesCajaTiendaFormaPago[0];
            List<ConfigGeneralesCajaTiendaFormaPago> confGralFormaPagoList = new List<ConfigGeneralesCajaTiendaFormaPago>();
            ConfigGeneralesCajaTiendaFormaPago confGralFormaPagoItem;
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioOperacion", folioOperacion);
            parameters.Add("@TipoCabeceraVenta", tipoCabeceraVenta);
            parameters.Add("@FolioDevolucion", folioDevolucion);
            parameters.Add("@CodigoTienda", codigoTienda);
            parameters.Add("@CodigoCaja", codigoCaja);
            foreach (var result in data.GetDataReader("[dbo].[sp_vanti_ConfiguracionFormasPago]", parameters))
            {
                confGralFormaPagoItem = new ConfigGeneralesCajaTiendaFormaPago();
                confGralFormaPagoItem.IdentificadorFormaPago = result.GetValue(0).ToString();
                confGralFormaPagoItem.CodigoFormaPago = result.GetValue(0).ToString();
                confGralFormaPagoItem.DescripcionFormaPago = result.GetValue(1).ToString();
                confGralFormaPagoList.Add(confGralFormaPagoItem);
            }
            return confGralFormaPagoList.ToArray();
        }

    }
}
