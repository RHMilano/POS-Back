using Milano.BackEnd.Dto.Impresion;
using System;
using System.Collections.Generic;


namespace Milano.BackEnd.Repository.General
{
    /// <summary>
    /// 
    /// </summary>
    public class PrintReporteRepository : BaseRepository
    {
        /// <summary>
        /// Trae la configuracion de la impresora
        /// </summary>
        /// <param name="printerConfigRequest"></param>
        /// <returns></returns>
        public PrinterConfigResponse getPrinterConfig(PrinterConfigRequest printerConfigRequest)
        {
            PrinterConfigResponse printerConfigResponse = new PrinterConfigResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoCaja", printerConfigRequest.CodigoCaja);
            parameters.Add("@CodigoTienda", printerConfigRequest.CodigoTienda);

            foreach (var c in data.GetDataReader("dbo.sp_vanti_ObtenerConfigImpresoraReportes", parameters))
            {
                printerConfigResponse.NombreImpresora = c.GetValue(0).ToString();
            }

            return printerConfigResponse;
        }
        /// <summary>
        /// Trae la configuracion de la impresora
        /// </summary>
        /// <param name="printerConfigRequest"></param>
        /// <param name="idRelacionCaja"></param>
        /// <returns></returns>
        public RelacionCajaHeaderResponse getHeader(PrinterConfigRequest printerConfigRequest, int idRelacionCaja)
        {
            RelacionCajaHeaderResponse relacionCajaHeader = new RelacionCajaHeaderResponse();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoTienda", printerConfigRequest.CodigoTienda);
            parameters.Add("@IdRelacionCaja", idRelacionCaja);

            foreach (var c in data.GetDataReader("dbo.sp_vanti_ObtenerCabeceraRelacionCaja", parameters))
            {
                relacionCajaHeader.CodigoTienda = Convert.ToInt32(c.GetValue(0));
                relacionCajaHeader.Marca = c.GetValue(1).ToString();
                relacionCajaHeader.Direccion = c.GetValue(2).ToString();
                relacionCajaHeader.Telefono = c.GetValue(3).ToString();
                relacionCajaHeader.Fecha = c.GetValue(4).ToString();
                relacionCajaHeader.DescripcionTienda = c.GetValue(5).ToString();
                relacionCajaHeader.FechaOperacion = c.GetValue(6).ToString();
                relacionCajaHeader.FechaCorte = c.GetValue(7).ToString();
                relacionCajaHeader.FechaHoraInicioDedia = c.GetValue(8).ToString();
            }

            return relacionCajaHeader;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CodigoRelacion"></param>
        ///// <param name="CodigoTienda"></param>
        /// <returns></returns>
        public List<RelacionCajaRespose> getReporteRelacionCaja(int CodigoRelacion)
        {
            List<RelacionCajaRespose> relacionCajas = new List<RelacionCajaRespose>();
            var parameters = new Dictionary<string, object>();
            parameters.Add("@CodigoRelacion", CodigoRelacion);

            foreach (var a in data.GetDataReader("dbo.sp_vanti_bo_ReporteRelacionCajaTotal", parameters))
            {
                RelacionCajaRespose relacionCajaRespose = new RelacionCajaRespose();

                relacionCajaRespose.Id = Convert.ToInt32(a.GetValue(0));
                relacionCajaRespose.Descripcion = a.GetValue(1).ToString();
                relacionCajaRespose.TotalConIva = Convert.ToDecimal(a.GetValue(2));
                relacionCajaRespose.Seccion = new List<RelacionCajaDesgloseRespose>();
                var parametersSeccion = new Dictionary<string, object>();
                parametersSeccion.Add("@CodigoRelacion", relacionCajaRespose.Id);

                foreach (var c in data.GetDataReader("dbo.sp_vanti_bo_ReporteRelacionCajaSeccion", parametersSeccion))
                {
                    RelacionCajaDesgloseRespose relacionCajaDesglose = new RelacionCajaDesgloseRespose();
                    relacionCajaDesglose.Id = Convert.ToInt32(c.GetValue(0));
                    relacionCajaDesglose.Descripcion = c.GetValue(1).ToString();
                    relacionCajaDesglose.TotalConIva = Convert.ToDecimal(c.GetValue(2));
                    relacionCajaDesglose.TotalSinIva = Convert.ToDecimal(c.GetValue(3));
                    relacionCajaDesglose.Iva = Convert.ToDecimal(c.GetValue(4));
                    relacionCajaDesglose.Desglose = new List<RelacionCajaDetalleResponse>();

                    var parametersDesglose = new Dictionary<string, object>();
                    parametersDesglose.Add("@IdSeccionRelacionCaja", relacionCajaDesglose.Id);

                    foreach (var d in data.GetDataReader("dbo.sp_vanti_bo_ReporteRelacionCajaSeccionDesglose", parametersDesglose))
                    {
                        RelacionCajaDetalleResponse relacionCajaDetalleResponse = new RelacionCajaDetalleResponse();
                        relacionCajaDetalleResponse.Id = Convert.ToInt32(d.GetValue(0));
                        relacionCajaDetalleResponse.Descripcion = d.GetValue(1).ToString();
                        relacionCajaDetalleResponse.TotalConIva = Convert.ToDecimal(d.GetValue(2));

                        relacionCajaDesglose.Desglose.Add(relacionCajaDetalleResponse);
                    }
                    relacionCajaRespose.Seccion.Add(relacionCajaDesglose);
                }

                relacionCajas.Add(relacionCajaRespose);
            }
            return relacionCajas;
        }

    }
}
