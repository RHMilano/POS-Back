using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Dto.Recovery;
using Milano.BackEnd.Dto.Sales;
using Milano.BackEnd.Repository.General;
using Project.Services.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;
namespace Milano.BackEnd.Repository
{

    /// <summary>
    /// Repositorio de Devoluciones y Descuentos
    /// </summary>
    public class DescuentosPromocionesRepository : BaseRepository
    {

        /// <summary>
        /// Obtenemos el porcentaje de descuento de primera compra
        /// </summary>
        /// <param name="folioVenta"></param>
        /// <param name="codigoTienda"></param>
        /// <param name="codigoCaja"></param>
        /// <returns></returns>
        public DescuentoPromocionalVenta[] ObtenerDescuentoMMPrimeraCompra(string folioVenta, int codigoTienda, int codigoCaja)
        {
            DescuentoTCMMPrimeraCompra descuento = new DescuentoTCMMPrimeraCompra();
            Inspector inspector = new Inspector();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioVenta", folioVenta);
            parameters.Add("@CodigoTienda", codigoTienda);
            parameters.Add("@CodigoCaja", codigoCaja);
            List<DescuentoPromocionalVenta> list = new List<DescuentoPromocionalVenta>();
            foreach (var item in data.GetDataReader("sp_vanti_prmChecarPromocionesMMPrimeraCompra", parameters))
            {
                int codigoPromocionAplicado = Convert.ToInt32(item.GetValue(0));
                DescuentoPromocionalVenta descuentoPromocional = list.FirstOrDefault(x => x.CodigoPromocionAplicado == codigoPromocionAplicado);
                if (descuentoPromocional == null)
                {
                    descuentoPromocional = new DescuentoPromocionalVenta();
                    descuentoPromocional.CodigoPromocionAplicado = Convert.ToInt32(item.GetValue(0));
                    descuentoPromocional.DescripcionCodigoPromocionAplicado = Convert.ToString(item.GetValue(1));
                    descuentoPromocional.ImporteDescuento = inspector.TruncarValor(Convert.ToDecimal(item.GetValue(2)));
                    descuentoPromocional.CodigoPromocionOrden = Convert.ToInt32(item.GetValue(3));
                    descuentoPromocional.PorcentajeDescuento = inspector.TruncarValor(Convert.ToDecimal(item.GetValue(6)));
                    descuentoPromocional.CodigoRazonDescuento = Convert.ToInt32(item.GetValue(7));
                    descuentoPromocional.DescuentosPromocionalesFormaPago = new DescuentoPromocionalFormaPago[] { };
                    if (!item.IsDBNull(4))
                    {
                        DescuentoPromocionalFormaPago decuentoPromocionalFormaPago = new DescuentoPromocionalFormaPago();
                        decuentoPromocionalFormaPago.codigoFormaPago = Convert.ToString(item.GetValue(4));
                        descuentoPromocional.DescuentosPromocionalesFormaPago.ToList().Add(decuentoPromocionalFormaPago);
                    }
                    list.Add(descuentoPromocional);
                }
                else
                {
                    if (!item.IsDBNull(4))
                    {
                        DescuentoPromocionalFormaPago decuentoPromocionalFormaPago = new DescuentoPromocionalFormaPago();
                        decuentoPromocionalFormaPago.codigoFormaPago = Convert.ToString(item.GetValue(4));
                        descuentoPromocional.DescuentosPromocionalesFormaPago.ToList().Add(decuentoPromocionalFormaPago);
                    }
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// Obtiene las promociones que son aplicables a la venta
        /// </summary>
        /// <param name="folioVenta">Folio de la venta</param>
        /// <param name="codigoTienda">Código de tienda</param>      
        /// <param name="codigoCaja">Código de caja</param>           
        /// <returns>Resultado de la operación</returns>
        public DescuentoPromocionalVenta[] ObtenerPromocionesVenta(string folioVenta, int codigoTienda, int codigoCaja)
        {
            Inspector inspector = new Inspector();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioVenta", folioVenta);
            parameters.Add("@CodigoTienda", codigoTienda);
            parameters.Add("@CodigoCaja", codigoCaja);
            List<DescuentoPromocionalVenta> lista = new List<DescuentoPromocionalVenta>();
            var prueba = data.GetDataReader("[dbo].[sp_vanti_prmChecarPromocionesVenta]", parameters);
            foreach (var item in prueba)
            {
                int grupoFormaPagoAsociada = 0;
                DescuentoPromocionalVenta descuentoPromocional = new DescuentoPromocionalVenta();
                descuentoPromocional.CodigoPromocionAplicado = Convert.ToInt32(item.GetValue(0));
                descuentoPromocional.DescripcionCodigoPromocionAplicado = Convert.ToString(item.GetValue(1));
                descuentoPromocional.ImporteDescuento = inspector.TruncarValor(Convert.ToDecimal(item.GetValue(2)));
                descuentoPromocional.CodigoPromocionOrden = Convert.ToInt32(item.GetValue(3));
                descuentoPromocional.PorcentajeDescuento = inspector.TruncarValor(Convert.ToDecimal(item.GetValue(6)));
                descuentoPromocional.CodigoRazonDescuento = Convert.ToInt32(item.GetValue(7));
                descuentoPromocional.Secuencia = 0;
                if (!item.IsDBNull(5))
                {
                    descuentoPromocional.Secuencia = Convert.ToInt32(item.GetValue(5));
                }
                if (!item.IsDBNull(4))
                {
                    grupoFormaPagoAsociada = Convert.ToInt32(item.GetValue(4));
                }
                if (grupoFormaPagoAsociada > 0)
                {
                    // Obtener las formas de pago asociadas
                    // TODO: Invocar adeacuadamente al SP
                    List<DescuentoPromocionalFormaPago> descuentosPromocionalesFormasPago = new List<DescuentoPromocionalFormaPago>();
                    var parametetrosFormasPago = new Dictionary<string, object>();
                    parametetrosFormasPago.Add("@CodigoPromoFormaPago", grupoFormaPagoAsociada);
                    foreach (var formaPago in data.GetDataReader("[dbo].[sp_vanti_prmGetFormasDePago]", parametetrosFormasPago))
                    {
                        DescuentoPromocionalFormaPago descuentoPromocionalFormaPago = new DescuentoPromocionalFormaPago();
                        descuentoPromocionalFormaPago.codigoFormaPago = Convert.ToString(formaPago.GetValue(0));
                        descuentosPromocionalesFormasPago.Add(descuentoPromocionalFormaPago);
                    }
                }
                lista.Add(descuentoPromocional);
            }
            return lista.ToArray();
        }

        /// <summary>
        /// Procesa las promociones que son aplicables a la venta por concepto de cupones
        /// </summary>
        /// <param name="folioVenta">Folio de la venta</param>
        /// <param name="codigoTienda">Código de tienda</param>      
        /// <param name="codigoCaja">Código de caja</param>           
        /// <returns>Resultado de la operación</returns>
        public CuponPromocionalVenta[] ProcesarPromocionesCupones(string folioVenta, int codigoTienda, int codigoCaja)
        {
            Inspector inspector = new Inspector();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioVenta", folioVenta);
            parameters.Add("@CodigoTienda", codigoTienda);
            parameters.Add("@CodigoCaja", codigoCaja);
            List<CuponPromocionalVenta> listaCupones = new List<CuponPromocionalVenta>();
            foreach (var item in data.GetDataReader("[dbo].[sp_vanti_prmChecarPromocionesCupones]", parameters))
            {
                CuponPromocionalVenta descuentoPromocional = new CuponPromocionalVenta();
                descuentoPromocional.FechaCreacion = Convert.ToDateTime(item.GetValue(0));
                descuentoPromocional.CodigoTienda = Convert.ToInt32(item.GetValue(1));
                descuentoPromocional.CodigoCaja = Convert.ToInt32(item.GetValue(2));
                descuentoPromocional.Transaccion = Convert.ToInt32(item.GetValue(3));
                descuentoPromocional.CodigoPromocionAplicado = Convert.ToInt32(item.GetValue(4));
                descuentoPromocional.FolioOperacion = Convert.ToString(item.GetValue(5));
                descuentoPromocional.Status = Convert.ToString(item.GetValue(6));
                descuentoPromocional.FechaCancelacion = Convert.ToDateTime(item.GetValue(7));
                descuentoPromocional.ImporteDescuento = inspector.TruncarValor(Convert.ToDecimal(item.GetValue(8)));
                descuentoPromocional.Saldo = inspector.TruncarValor(Convert.ToDecimal(item.GetValue(9)));
                descuentoPromocional.MensajeCupon = Convert.ToString(item.GetValue(10));
                listaCupones.Add(descuentoPromocional);
            }
            return listaCupones.ToArray();
        }

    }
}