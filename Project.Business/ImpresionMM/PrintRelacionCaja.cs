using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.Impresion;
using Milano.BackEnd.Repository.General;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace Milano.BackEnd.Business.ImpresionMM
{
    /// <summary>
    /// Imprime el reporte de relacion de caja
    /// </summary>
    public class PrintRelacionCaja : BaseBusiness
    {
        int nextPageToPrint = -1;
        private PrintDocument printDocument;
        private TokenDto token;
        private List<RelacionCajaRespose> CurrentData;

        // we need to keep track of the horizontal position
        // this is also the top margin:
        float y = 10f;
        // we will need to measure the texts we print:
        SizeF size = Size.Empty;
        Font font = new Font("Arial", 6);
        Font fontEnfasis = new Font("Arial Black", 6);
        Font fontTitulo = new Font("Arial Black", 10);
        string PrinterName;
        int margin = 20;
        RelacionCajaHeaderResponse rch;

        /// <summary>
        /// Repositorio para obtener datos 
        /// </summary>
        protected PrintReporteRepository repository;
        /// <summary>
        ///  Constructor
        /// </summary>
        public PrintRelacionCaja(TokenDto token)
        {
            try
            {
                this.repository = new PrintReporteRepository();
                this.token = token;
                this.printDocument = new PrintDocument();
                this.SetPrinterConfig();
            }
            catch (Exception exception)
            {
                TryCatchBusinessExecutor tryCatch = new TryCatchBusinessExecutor();
                tryCatch.AddErrorLog<OperationResponse>(exception.Message, exception.StackTrace, "Negocio", exception.ToString(), "Error de impresión");
            }


        }


        /// <summary>
        /// Establece la configuracion de la impresora 
        /// </summary>
        private void SetPrinterConfig()
        {
            try
            {
                PrinterConfigRequest printerConfigRequest = new PrinterConfigRequest();
                printerConfigRequest.CodigoCaja = this.token.CodeBox;
                printerConfigRequest.CodigoTienda = this.token.CodeStore;
                //Solo se recupera el nombre de la impresora
                PrinterConfigResponse printerConfigResponse = repository.getPrinterConfig(printerConfigRequest);
                //Configuracion de la impresora
                PrinterSettings ps = new PrinterSettings();
                PrinterName = printerConfigResponse.NombreImpresora;
                ps.PrinterName = printerConfigResponse.NombreImpresora;
                //    IEnumerable<PaperSize> paperSizes = ps.PaperSizes.Cast<PaperSize>();
                //    PaperSize sizeA4 = paperSizes.First<PaperSize>(size => size.Kind == PaperKind.Letter);
                //    this.printDocument.DefaultPageSettings.PaperSize = sizeA4;


                this.printDocument.PrinterSettings = ps;
                this.printDocument.PrintPage += new PrintPageEventHandler(this.printDocument1_PrintPage);

            }
            catch (Exception exception)
            {
                TryCatchBusinessExecutor tryCatch = new TryCatchBusinessExecutor();
                tryCatch.AddErrorLog<OperationResponse>(exception.Message, exception.StackTrace, "Negocio", exception.ToString(), "Error de impresión");
            }
        }
        /// <summary>
        /// Imprime el reporte de la relacion de caja del id dado
        /// </summary>
        /// <param name="idReporte"></param>
        /// <returns></returns>
        public OperationResponse printReporte(int idReporte)
        {
            OperationResponse operationResponse = new OperationResponse();
            //try
            //{
                Boolean isValid = printDocument.PrinterSettings.IsValid;
                Boolean isDefault = printDocument.PrinterSettings.IsDefaultPrinter;

                if (isValid)
                {
                    PrinterConfigRequest printerConfigRequest = new PrinterConfigRequest();
                    printerConfigRequest.CodigoTienda = this.token.CodeStore;

                    printDocument.PrinterSettings.PrintFileName = "RelacionCaja";
                    this.CurrentData = this.repository.getReporteRelacionCaja(idReporte);
                    //Trayendo la info de la tienda
                    this.rch = this.repository.getHeader(printerConfigRequest, idReporte);

                    printDocument.Print();
                    operationResponse.CodeNumber = "100";
                    operationResponse.CodeDescription = "ok";
                }
                else
                {
                    string err = "Error de configuracion Impresora:" + PrinterName + " Tienda:" + token.CodeStore + " Caja: " + token.CodeBox;
                    TryCatchBusinessExecutor tryCatch = new TryCatchBusinessExecutor();
                    tryCatch.AddErrorLog<OperationResponse>(err, "", "Negocio", "", "Error de impresión");
                    operationResponse.CodeNumber = "200";
                    operationResponse.CodeDescription = err;
                }

            //}
            //catch (Exception exception)
            //{
            //    TryCatchBusinessExecutor tryCatch = new TryCatchBusinessExecutor();
            //    tryCatch.AddErrorLog<OperationResponse>(exception.Message, exception.StackTrace, "Negocio", exception.ToString(), "Error de impresión");
            //    operationResponse.CodeNumber = "200";
            //    operationResponse.CodeDescription = exception.ToString();
            //}
            // operationResponse.CodeDescription = JsonConvert.SerializeObject(this.CurrentData);
            return operationResponse;
        }

        /// <summary>
        /// Crea el encabezado del reporte
        /// </summary>
        private void setHeader(object sender, PrintPageEventArgs e)
        {

            Font fontHeader = new Font("Arial", 8);

            string header = "RELACIÓN DE CAJA DE " + rch.DescripcionTienda;
            size = e.Graphics.MeasureString(header, fontTitulo, 9999);
            // rectangulo
            e.Graphics.FillRectangle(Brushes.Gainsboro, margin, this.y, 180, size.Height);
            // a simple header:
            e.Graphics.DrawString(header, fontTitulo, Brushes.Black, this.margin + (180 - size.Width) / 2, this.y);
            this.y += size.Height;
            // Resto del header
            size = e.Graphics.MeasureString(header, fontHeader, 9999);
            e.Graphics.DrawString("Codigo Tienda: " + rch.CodigoTienda.ToString(), fontHeader, Brushes.Black, margin + 2, this.y);
            e.Graphics.DrawString("Marca: " + rch.Marca, fontHeader, Brushes.Black, margin + 90, this.y);
            this.y += size.Height;
            e.Graphics.DrawString("Dirección Tienda: " + rch.Direccion, fontHeader, Brushes.Black, margin + 2, this.y);
            this.y += size.Height;
            // linea 2
            e.Graphics.DrawString("Fecha de Operación: " + rch.FechaOperacion, fontHeader, Brushes.Black, margin + 2, this.y);
            e.Graphics.DrawString("Teléfono Tienda: " + rch.Telefono, fontHeader, Brushes.Black, margin + 100, this.y);
            this.y += size.Height;
            e.Graphics.DrawString("Fecha y Hora de Inicio de Dia: " + rch.FechaHoraInicioDedia, fontHeader, Brushes.Black, margin + 2, this.y);
            this.y += size.Height;
            e.Graphics.DrawString("Fecha y Hora de Corte: " + rch.FechaCorte, fontHeader, Brushes.Black, margin + 2, this.y);
            this.y += size.Height + 5;
            //e.Graphics.DrawString("Fecha Corte: " + rch.FechaCorte, fontHeader, Brushes.Black, margin + 2, this.y);
            //this.y += size.Height + 5;

        }


        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            nextPageToPrint++;
            //Se mide la pagina en milimetros
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;
            setHeader(sender, e);
            float maxY = y;

            foreach (RelacionCajaRespose r in this.CurrentData)
            {
                float yControl = y;
                SizeF s = Size.Empty;
                if (r.Descripcion.ToUpper() == "INGRESOS")
                {
                    yControl = printTable(r.Seccion, this.margin, 105, this.y, e);
                    yControl += 3;
                    string t = "TOTAL INGRESOS: " + r.TotalConIva.ToString("C");
                    s = e.Graphics.MeasureString(t, fontEnfasis, 9999);
                    float middle = margin + (105 - margin - s.Width) / 2;
                    e.Graphics.DrawString(t, fontEnfasis, Brushes.Black, middle, yControl);
                    yControl += s.Height;
                    if (yControl > maxY)
                    {
                        maxY = yControl;
                        maxY = yControl;
                    }
                }
                else if (r.Descripcion.ToUpper() == "EGRESOS")
                {
                    yControl = printTable(r.Seccion, 110, 200, this.y, e);
                    yControl += 3;
                    string t = "TOTAL EGRESOS: " + r.TotalConIva.ToString("C"); ;
                    s = e.Graphics.MeasureString(t, fontEnfasis, 9999);
                    float middle = 110 + (90 - s.Width) / 2;
                    e.Graphics.DrawString(t, fontEnfasis, Brushes.Black, middle, yControl);
                    yControl += s.Height;
                    if (yControl > maxY)
                    {
                        maxY = yControl;
                    }
                }
                else
                {
                    yControl = maxY;
                    s = e.Graphics.MeasureString(r.Descripcion, font, 9999);
                    // rectangulo
                    e.Graphics.FillRectangle(Brushes.Gainsboro, margin, yControl, 180, s.Height);
                    //Titulo
                    float middle = margin + ((180 - s.Width) / 2);

                    e.Graphics.DrawString(r.Descripcion, font, Brushes.Black, middle, yControl);
                    yControl += s.Height + 3;
                    yControl = this.printTable(r.Seccion, this.margin, 105, yControl, e);
                }

            }
        }

        /// <summary>
        /// Imprime la tabla
        /// </summary>
        /// <param name="r"></param>
        /// <param name="startX"></param>
        /// <param name="endX"></param>
        /// <param name="yStart"></param>
        /// <param name="e"></param>
        private float printTable(List<RelacionCajaDesgloseRespose> r, float startX, float endX, float yStart, PrintPageEventArgs e)
        {
            float yControl = yStart;
            foreach (RelacionCajaDesgloseRespose rc in r)
            {
                SizeF s = Size.Empty;
                s = e.Graphics.MeasureString(rc.Descripcion, font, 9999);
                // rectangulo
                e.Graphics.FillRectangle(Brushes.Gainsboro, startX, yControl, endX - startX, s.Height);
                //Titulo
                float middle = startX + ((endX - startX - s.Width) / 2);

                e.Graphics.DrawString(rc.Descripcion, font, Brushes.Black, middle, yControl);
                yControl += s.Height;

                foreach (RelacionCajaDetalleResponse detalle in rc.Desglose)
                {
                    e.Graphics.DrawString(detalle.Descripcion, font, Brushes.Black, startX, yControl);
                    string cantidad = detalle.TotalConIva.ToString("C");
                    s = e.Graphics.MeasureString(cantidad, font, 9999);
                    e.Graphics.DrawString(cantidad, font, Brushes.Black, endX - s.Width, yControl);

                    yControl += s.Height;
                }
                //Separacion entre los totales
                yControl += 2;
                // Totales
                e.Graphics.DrawString("Total sin IVA", font, Brushes.Black, startX, yControl);
                string TotalSinIva = rc.TotalSinIva.ToString("C"); ;
                s = e.Graphics.MeasureString(TotalSinIva, font, 9999);
                e.Graphics.DrawString(TotalSinIva, font, Brushes.Black, endX - s.Width, yControl);
                yControl += s.Height;

                e.Graphics.DrawString("IVA", font, Brushes.Black, startX, yControl);
                string Iva = rc.Iva.ToString("C");
                s = e.Graphics.MeasureString(Iva, font, 9999);
                e.Graphics.DrawString(Iva, font, Brushes.Black, endX - s.Width, yControl);
                yControl += s.Height;

                e.Graphics.DrawString("Total con IVA", fontEnfasis, Brushes.Black, startX, yControl);
                string TotalConIva = rc.TotalConIva.ToString("C");
                s = e.Graphics.MeasureString(TotalConIva, fontEnfasis, 9999);
                e.Graphics.DrawString(TotalConIva, fontEnfasis, Brushes.Black, endX - s.Width, yControl);
                yControl += s.Height;
            }
            return yControl;
        }


        private void printDocument1_BeginPrint(object sender, PrintEventArgs e)
        {
            nextPageToPrint = 0;

        }
    }
}
