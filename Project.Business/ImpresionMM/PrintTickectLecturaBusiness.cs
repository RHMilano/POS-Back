using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.Impresion;
using Milano.BackEnd.Repository.General;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;

namespace Milano.BackEnd.Business.ImpresionMM
{
    /// <summary>
    /// Imprime los tickets de lectura x o z
    /// </summary>
    public class PrintTickectLecturaBusiness : BaseBusiness
    {
        private TokenDto token;
        private string PrinterName;
        private PrintDocument printDocument;
        private Font drawFont;
        private SolidBrush drawBrush;
        private System.Drawing.Point pos;
        private PrintLecturaRequest printLecturaRequest;
        private int startX = 150;
        private int startY = 0;
        private int Offset = 20;
        private PrinterConfigResponse printerConfigResponse;

        /// <summary>
        /// Repositorio para obtener datos 
        /// </summary>
        protected PrintTicketRepository repository;

        /// <summary>
        /// Constructor por default
        /// </summary>
        public PrintTickectLecturaBusiness(TokenDto token)
        {
            try
            {
                this.PrinterName = "";
                this.repository = new PrintTicketRepository();
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

            PrinterConfigRequest printerConfigRequest = new PrinterConfigRequest();
            printerConfigRequest.CodigoCaja = this.token.CodeBox;
            printerConfigRequest.CodigoTienda = this.token.CodeStore;
            this.printerConfigResponse = repository.getPrinterConfig(printerConfigRequest);
            this.PrinterName = this.printerConfigResponse.NombreImpresora;
            printDocument.PrinterSettings.PrinterName = this.PrinterName;
            this.drawFont = new Font("Courier New", 8);
            this.drawBrush = new SolidBrush(System.Drawing.Color.Black);
            this.pos = new System.Drawing.Point(10, 10);
            printDocument.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
        }

        /// <summary>
        /// Imprime los tickets de lectura X y Z
        /// </summary>
        public OperationResponse PrintNow(PrintLecturaRequest printLecturaRequest)
        {
            OperationResponse operationResponse = new OperationResponse();
            try
            {
                this.printLecturaRequest = printLecturaRequest;
                if (printDocument.PrinterSettings.IsValid)
                {
                    printDocument.Print();
                    operationResponse.CodeNumber = "100";
                    operationResponse.CodeNumber = "Impresion correcta";
                }
              
            }
            catch (Exception exception)
            {
                TryCatchBusinessExecutor tryCatch = new TryCatchBusinessExecutor();
                tryCatch.AddErrorLog<OperationResponse>(exception.Message, exception.StackTrace, "Negocio", exception.ToString(), "Error de impresión");
                operationResponse.CodeNumber = "300";
                operationResponse.CodeNumber = exception.Message;
            }
            return operationResponse;
        }

        private class PrintDirect
        {
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            public class DOCINFOA
            {
                [MarshalAs(UnmanagedType.LPStr)]
                public string pDocName;
                [MarshalAs(UnmanagedType.LPStr)]
                public string pOutputFile;
                [MarshalAs(UnmanagedType.LPStr)]
                public string pDataType;
            }

            [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
            public static extern bool OpenPrinter(String pPrinterName, ref IntPtr phPrinter, int pDefault);

            [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
            public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

            [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
            public static extern bool StartPagePrinter(IntPtr hPrinter);

            [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
            public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

            [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
            public static extern bool EndPagePrinter(IntPtr hPrinter);

            [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
            public static extern long EndPrinter(IntPtr hPrinter);

            [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
            public static extern long EndDocPrinter(IntPtr hPrinter);

            [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
            public static extern long ClosePrinter(IntPtr hPrinter);

            [DllImport("kernel32.dll")]
            public static extern Int32 GetLastError();
        }



        // The PrintPage event is raised for each page to be printed. 
        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            PrintLecturaResponse[] printLecturaResponses = repository.getLecturaDatos(this.printLecturaRequest);
            foreach (var r in printLecturaResponses)
            {
                PrintLecturaResponse item = new PrintLecturaResponse();
                item.Texto = r.Texto;
                item.Formato = r.Formato;
                StringFormat format1 = new StringFormat(StringFormatFlags.NoClip);
                Font tmpFont = new Font(drawFont.Name, drawFont.Size);
                // Por default alineado a la izquierda
                format1.Alignment = StringAlignment.Far;
                string[] formatos = item.Formato.Split('|');
                foreach (var f in formatos)
                {
                    if (f == "center")
                    {
                        format1.Alignment = StringAlignment.Center;
                    }
                    else if (f.IndexOf("font") != -1)
                    {
                        tmpFont = new Font(f.Split(':')[1], Convert.ToInt32(f.Split(':')[2]));
                    }

                }
                pos.X = startX;
                pos.Y = startY + Offset;
                ev.Graphics.DrawString(item.Texto, tmpFont, drawBrush, pos, format1);
                Offset = Offset + 10;
            }
            ev.HasMorePages = false;
        }
        /// <summary>
        /// Manda la instruccion de abrir cajon
        /// </summary>
        /// <returns></returns>
        public bool AbrirCajon()
        {
            string codigoApertura = this.printerConfigResponse.CodigoAperturaCajon;
            string szPrinterName = this.printerConfigResponse.NombreImpresora;
            //27,112,48,55,121
            Int32 dwWritten = 0;
            IntPtr hPrinter = new IntPtr(0);
            PrintDirect.DOCINFOA di = new PrintDirect.DOCINFOA();
            bool bSuccess = false;
            if (codigoApertura == "")
                return false;

            string[] splitCodes = codigoApertura.Split(',');

            byte[] DrawerOpener = new byte[splitCodes.Length];
            for (int i = 0; i < splitCodes.Length; i++)
                DrawerOpener[i] = Byte.Parse(splitCodes[i]);

            di.pDocName = "OpenDrawer";
            di.pDataType = "RAW";

            if (PrintDirect.OpenPrinter(szPrinterName, ref hPrinter, 0)) //Obtiene el número de trabajo de impresión, 0 si error
            {
                if (PrintDirect.StartDocPrinter(hPrinter, 1, di))
                {
                    if (PrintDirect.StartPagePrinter(hPrinter))
                    {
                        IntPtr p = Marshal.AllocCoTaskMem(DrawerOpener.Length);
                        Marshal.Copy(DrawerOpener, 0, p, DrawerOpener.Length);
                        bSuccess = PrintDirect.WritePrinter(hPrinter, p, DrawerOpener.Length, out dwWritten);
                        PrintDirect.EndPagePrinter(hPrinter);
                        Marshal.FreeCoTaskMem(p);
                    }
                    PrintDirect.EndDocPrinter(hPrinter);
                }
                PrintDirect.ClosePrinter(hPrinter);
            }

            return bSuccess;
        }

    }
}
