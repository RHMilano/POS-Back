using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.PointOfService;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Repository.General;
using Project.Dto.General;

namespace Milano.BackEnd.Business.General
{

    /// <summary>
    /// Clase de negocios para controlar la impresión de los tickets
    /// </summary>    
    public class PrintTicketBusiness : BaseBusiness
    {

        /// <summary>
        /// Explorador de dispositivos
        /// </summary>
        private PosExplorer _explorer;
        /// <summary>
        /// Nombre logico de la impresora el cual se registro en el driver de OPOS
        /// </summary>
        public string PrinterName { get; private set; }
        private PosPrinter _printer;
        protected PrintTicketRepository repository;

        /// <summary>
        /// Constructor por default
        /// </summary>
        public PrintTicketBusiness()
        {
            this.PrinterName = "";
            this.repository = new PrintTicketRepository();

        }

        /// <summary>
        /// Crea la instancia del explorador 
        /// </summary>
        private PosExplorer Explorer
        {
            get { return _explorer ?? (_explorer = new PosExplorer()); }
        }

        /// <summary>
        /// Crea la instancia de la impresora y se conecta a la misma
        /// </summary>
        private PosPrinter Printer
        {
           
            get
            {
                if (_printer != null)
                {
                    InitPrinter();
                    return _printer;
                }

                if (String.IsNullOrEmpty(PrinterName))
                {
                    throw new ArgumentNullException("El nombre lógico de la impresora es nulo.");
                }
                var device = Explorer.GetDevice(DeviceType.PosPrinter, PrinterName);
                if (device == null)
                {
                    throw new NullReferenceException(
                        "No se encontró el nombre lógico de la impresora : {0}." + PrinterName.ToString());
                }
                _printer = Explorer.CreateInstance(device) as PosPrinter;

                if (_printer == null)
                {
                    throw new NullReferenceException(
                        "Error al crear una instancia de Microsoft.PointOfService.PosPrinter.");
                }
                InitPrinter();
                return _printer;
            }
        }

        /// <summary>
        /// Inicializa la intancia de la impresora
        /// </summary>
        private void InitPrinter()
        {
            if (_printer.State == ControlState.Closed)
            {
                try { 
                _printer.Open();
                }catch(SystemException e)
                {
                    throw new NullReferenceException(
                        "Impresora no conectada o no encontrada");
                }
            }
            if (!_printer.Claimed)
            {
                _printer.Claim(0);
            }
            if (!_printer.DeviceEnabled)
            {
                _printer.DeviceEnabled = true;
            }
            if (!_printer.RecLetterQuality)
            {
                //Si es true imprime en modo de alta resolucion, en false imprime en alta velocidad
                _printer.RecLetterQuality = true;
            }
        }

        /// <summary>
        /// Manda a imprimer el ticket 
        /// </summary>
        /// <param name="folio">Cadena del ticket que va a recibir</param>
        public void Print(string folio)
        {
           // PrintTicketResponse printTicketResponse = repository.getTicket(folio);
            PrintTicketCabecerasResponse printTicketCabecerasResponse = repository.GetHeaders(folio);
            PrintTicketItem[] detalle = repository.GetDetalleVenta(folio);
            if (printTicketCabecerasResponse.Cabecera != null  && printTicketCabecerasResponse.Footer != null && printTicketCabecerasResponse.NombreImpresora != null)
            {
                this.PrinterName = printTicketCabecerasResponse.NombreImpresora;
                Printer.AsyncMode = false;

               
                InternalPrint(printTicketCabecerasResponse, detalle, folio);
            }
            else {
                Close();
                return;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="printTicketItems"></param>
        /// <returns></returns>
        private string GenerateBody(PrintTicketItem[] printTicketItems)
        {
            string body = "";
         
            foreach (var item in printTicketItems) {
                body += "\x1B|rA" + item.Sku  +"  "+ item.Descripcion.PadRight(30) + "\n    \x1B|lA\x1B|bC$" + item.CostoUnitario + "\x1B|!bC(" + item.Cantidad + ")\x1B|2lF" + item.Total; 
            }
            return body;
        }


        /// <summary>
        /// Imprime el ticket 
        /// </summary>
        /// <param name="printTicketCabecerasResponse"></param>
        /// <param name="detalle"></param>
        /// <param name="folio"></param>
        private void InternalPrint(PrintTicketCabecerasResponse printTicketCabecerasResponse, PrintTicketItem[] detalle, string folio)
        {
            string body = GenerateBody(detalle);
            
            PrintTicketSaveRequest printTicketSaveRequest = new PrintTicketSaveRequest();
            printTicketSaveRequest.FolioOperacion = folio;
            printTicketSaveRequest.CodigoTienda = detalle.First().CodigoTienda;
            printTicketSaveRequest.CodigoCaja = detalle.First().CodigoCaja;
            printTicketSaveRequest.TipoTicket = 1;
            printTicketSaveRequest.Cuerpo = body;

            // OperationResponse saveResponse = repository.SaveTicket(printTicketSaveRequest);

            Printer.PrintBitmap(PrinterStation.Receipt, @"C:\Users\darth\Downloads\Logo_808042.jpg", PosPrinter.PrinterBitmapAsIs,PosPrinter.PrinterBitmapCenter );

            Printer.PrintNormal(PrinterStation.Receipt, "\x1B|cA" + printTicketCabecerasResponse.Cabecera.Replace("\\n", "\n"));
            Printer.PrintNormal(PrinterStation.Receipt, "__________________________________________\x1B|3lF");
            Printer.PrintNormal(PrinterStation.Receipt, body);
            Printer.PrintNormal(PrinterStation.Receipt, "\x1B|3lF__________________________________________");
            Printer.PrintNormal(PrinterStation.Receipt, "\x1B|cA" + printTicketCabecerasResponse.Footer.Replace("\\n", "\n"));
            PrintBarCode(printTicketCabecerasResponse.FolioVenta);
            Printer.PrintNormal(PrinterStation.Receipt, "\x1B|5lF");
            
            Close();
        }

        /// <summary>
        /// Cierra la conexion a la impresora y corta el papel
        /// </summary>
        public void Close()
        {
            if (_printer != null)
            {
                _printer.CutPaper(100);
                _printer.Release();
                _printer.Close();
            }
        }

        /// <summary>
        /// Imprime el codigo de barras
        /// </summary>
        public void PrintBarCode(string FolioOperacion)
        {
            Printer.PrintBarCode(PrinterStation.Receipt, FolioOperacion.ToString(), BarCodeSymbology.Code128, Printer.RecLineHeight * 5, Printer.RecLineWidth, PosPrinter.PrinterBitmapCenter, BarCodeTextPosition.Below);
        }

    }
}
