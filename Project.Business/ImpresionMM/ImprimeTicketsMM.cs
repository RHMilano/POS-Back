using Milano.BackEnd.DataAccess;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Dto.Impresion;
using Milano.BackEnd.Repository.General;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Milano.BackEnd.Business.ImpresionMM
{
    /// <summary>
    ///  Clase de impresion para los tickets X y Z
    /// </summary>
    public class ImprimeTicketsMM : BaseBusiness
    {
        public class PrintDirect
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

        string[] Cadenas = new string[300];

        System.Drawing.Printing.PrintDocument oImpresion;
        string Parseado;                    //Se guarda la última función parseada en esta variable
        bool ParsearError;                  //Marca si al evaluar hubo error

        //Variables del programa
        int AnchoImpresoraHere;             //AnchoImpresora
        int AltoImpresoraHere;              //AltoImpresora
        decimal MargenX;                    //Para la función SetMargenX
        decimal MargenY;                    //Para la función SetMargenY
        string CadenaFormateada;            //Para la función DarFormato Almacena la cadena formateada
        string FormatoExpresion;            //Para la función EstablecerFormato almacena el Formato Ej "###,##0.00"
        int[] Enteros = new int[50];        //50 variables de enteros para hacer lo que el usuario quiera PonEntero1-50
        string[] Strings = new string[50];  //50 variables de enteros para hacer lo que el usuario quiera PonCadena1-50
        double[] Dobles = new double[50]; //50 variables de enteros para hacer lo que el usuario quiera PonDoble1-50
        bool[] Boleanos = new bool[50];     //50 variables de enteros para hacer lo que el usuario quiera PonBoleano1-50    

        string[] Expresiones = new string[1000];  //Son todas las expresiones del formato de ticket nota y/o factura
        int[] SiguienteEnIf = new int[1000];     //Este aun se puede mejorar, cuando haya un if en la expresión actual, indicará (en caso de que de falso la evaluación false) el número de la siguiente expresión a evaluar
        int[] IfsEncontrados = new int[10];      //Este se usa de manera temporal para analizar la consulta, indica el número de ifs anidados en un mismo momento
        int IniciaPartidasEn;
        int FinPartidasEn;
        int IniciaCuponesEn;
        int FinCuponesEn;
        int IniciaNotasDeCreditoGeneradasEn;
        int FinNotasDeCreditoGeneradasEn;
        int IniciaDescuentosGlobalesEn;
        int FinDescuentosGlobalesEn;
        int Conteo;
        int ConteoGeneral;


        int iPaginaActual = 0;
        bool bEstoyEnPartidas = false;
        bool bEstoyEnCupones = false;
        bool bEstoyEnNotasDecredito = false;
        bool bEstoyEnDescuentosGenerales = false;
        int iPosicionAnterior = 0;

        string[] FuncionesArray;
        int TotalFunciones;

        string Funciones;

        string[] ExpresionesAParsear = new string[20];      //Hasta 10 posibles variables o expresiones
        string[] ExpresionesParseadas = new string[20];     //Hasta 10 posibles variables ya parseadas
        int CantidadExpresionesAParsear;                    //Cuántas variables tiene la expresión actual
        string FuncionParseada;                             //La función de la expresión
        string CadenaParseadaCAC;                           //Se guarda la última cadena parseada llamada por ParseaExpresionUsuario

        Data cnxVanti;
        DataTable dtCabecero;               //Guarda cabecero de venta
        DataTable dtDetalle;                //Guarda detalle de venta
        DataTable dtFormasPago;             //Guarda las Formas de pago
        DataTable dtInfoSucursal;           //Guarda la información de sucursal (tabla gensucursalescat)
        DataTable dtInfoEmpresa;            //Guarda la información de la empresa (tabla genempresascat)
        DataTable dtVariables;              //Tabla de variables es el que indica
        DataTable dtVoucherCliente;         //Tabla donde vienen los comandos para imprimir el voucher
        DataTable dtCuponesGenerados;       //Tabla donde vienen los cupones generados.
        DataTable dtNotasDeCreditoGeneradas;//Tabla donde vienen las notas de crédito generadas.
        DataTable dtDescuentosGlobales;     //Guarda los descuentos a nivel global en mas de una partida
        DataTable dtFinLag;                 //Si nos pagan con finlag, se persisten los datos del cliente y el voucher


        bool bCopiaComercio = false;

        double TotalFormasPago;
        string FolioVenta;

        string sTipoVenta;
        double CantidadPiezas;
        int TotalPartidas;
        int TotalDescuentosGlobales;
        int TotalCuponesGenerados;
        int TotalNotasDeCreditoGeneradas;
        int partidaActual;
        int descuentoActual;
        int cuponActual;
        int notaDeCreditoActual;
        int voucherActual;

        int TipoImpresion;                  //indica qué se va a imprimir 1 = Voucher, 2 = Voucher Comercio, 3 = Voucher cliente
        bool bAbrirCajon;
        int iNumeroCopias;
        bool bTieneFinLag = false;
        bool bCopiaComercioFinlag = false;

        //int AltoImpresoraHere;              //AltoImpresora v.2.8 CGC Integración de ticketsPv

        private TokenDto token;
        //'Private funcionesCaja As FuncionesCajaCab

        /// <summary>
        /// Repositorio para obtener datos 
        /// </summary>
        protected PrintTicketRepository repository;
        /// <summary>
        /// Constructor por default
        /// </summary>
        /// <param name="token"></param>
        public ImprimeTicketsMM(TokenDto token)
        {

            try
            {
                this.token = token;
                this.iCaja = token.CodeBox;
                this.repository = new PrintTicketRepository();
            }
            catch (Exception exception)
            {
                TryCatchBusinessExecutor tryCatch = new TryCatchBusinessExecutor();
                tryCatch.AddErrorLog<OperationResponse>(exception.Message, exception.StackTrace, "Negocio", exception.ToString(), "Error de impresión");

            }

        }

        public struct fuenteProp
        {
            public bool Bold;
            public bool Italic;
            public bool Underline;
            public bool StrikeOut;
            public bool Regular;
            public string NombreFuente;
            public Single Tamaño;
        }

        System.Drawing.Printing.PrintPageEventArgs Printer;
        Font prFont;
        bool bReimpresion;
        string sImpresora;
        string sImpresoraOriginal;
        int iCaja;
        int iLargoTicket;

        Point currentXY;
        Size ultimoBox;
        public fuenteProp TipoFuente;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sFolioventa"></param>
        /// <param name="Reimpresion"></param>
        /// <returns></returns>
        public OperationResponse PrintTicket(string sFolioventa, bool Reimpresion)
        {
            OperationResponse operationResponse = new OperationResponse();
            try
            {
                TicketsMM(sFolioventa, Reimpresion);
                operationResponse.CodeNumber = "100";
                operationResponse.CodeDescription = "Reimpresión correcta";
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sFolioventa"></param>
        /// <param name="Reimpresion"></param>

        private void TicketsMM(string sFolioventa, bool Reimpresion)
        {
            //OCG|27042022 Imprime vertical
            Funciones = "Imprime IMPRIMECONLATERAL ImprimirImagen ImprimeCentrado ImprimeCodigoDeBarras ImprimeCompleto ImprimeImagen PonFuente PonNegrita QuitaNegrita PonCursiva QuitaCursiva PonSubrayado ";
            Funciones += "QuitaSubrayado PonTachado ";
            Funciones += "RegresarDetalle RegresarDescuentosGlobales RegresarCupones RegresarNotasDeCredito QuitaTachado PonNormal PonFuente PonTamaño PonColor MueveCursorX MueveCursorY MueveCursorAbsolutoX ImprimeNumeroATexto ";
            Funciones += "MueveCursorAbsolutoY ImprimeDerecha ImprimeCentrado TruncarEnX AbrirCajon ";
            Funciones += "PonMargenX PonMargenY NuevaHoja PonAnchoImpresora CaracteresExpresion AnchoExpresion AltoExpresion AnchoImpresora DarFormato ";
            Funciones += "AbrirCajon AvanzarNCaracteres AvanzarNLineas RellenarNConX SubCadena Concatenar IMPRIMIRENANGULO";

            PrinterConfigRequest printerConfigRequest = new PrinterConfigRequest();

            printerConfigRequest.CodigoCaja = this.token.CodeBox;
            printerConfigRequest.CodigoTienda = this.token.CodeStore;
            PrinterConfigResponse printerConfigResponse = repository.getPrinterConfig(printerConfigRequest);

            this.sImpresora = printerConfigResponse.NombreImpresora;
            this.sImpresoraOriginal = printerConfigResponse.NombreImpresora;
            bReimpresion = Reimpresion;
            iCaja = this.token.CodeBox;

            TipoFuente = new fuenteProp();

            currentXY = new Point();
            ultimoBox = new Size();

            cnxVanti = new Data();

            //string[] PortNames = SerialPort.GetPortNames();

            FolioVenta = sFolioventa;

            #region v.2.8 CGC Integración de ticketsPv
            bReimpresion = Reimpresion;
            //OCG: Estos datos los recupera de forma interna
            //sImpresora = Impresora;
            //sImpresoraOriginal = Impresora;
            //iCaja = Caja;
            iLargoTicket = 0; // Esta dato no lo va a enviar el POS
            #endregion

            Funciones = Funciones.ToUpper();

            FuncionesArray = Funciones.Split(' ');
            TotalFunciones = FuncionesArray.Length;

            //De donde a donde están las partidas
            IniciaPartidasEn = -1;
            FinPartidasEn = -1;
            IniciaDescuentosGlobalesEn = -1;
            FinDescuentosGlobalesEn = -1;
            IniciaCuponesEn = -1;
            FinCuponesEn = -1;
            IniciaNotasDeCreditoGeneradasEn = -1;
            FinNotasDeCreditoGeneradasEn = -1;

            //Limpiamos el conteo de las partidas
            Conteo = 0;
            ConteoGeneral = 0;

            ImprimirTicket();
        }

        private void ImprimirTicket()
        {

            string StrString = "";
            string Str;
            int i;
            int ConteoIfAbiertos;

            var parameters = new Dictionary<string, object>();
            parameters.Add("@FolioVenta", FolioVenta);
            parameters.Add("@Tipo", 0);

            foreach (SqlDataReader dr in cnxVanti.GetDataReader("SP_ObtieneDatosVenta", parameters))
            {
                ConteoIfAbiertos = 0;

                while (dr.Read())
                {
                    StrString = dr.GetString(0);
                    ConteoGeneral = ConteoGeneral + 1;
                    StrString = StrString.Trim();

                    do
                    {
                        if (StrString == "")
                            break;
                        Str = GetFirstFromString(StrString);
                        if (Str == "")
                            break;

                        if (Str == " " || Encoding.ASCII.GetBytes(Str)[0] == 9)
                            StrString = RemoveFirstFromString(StrString);

                    } while (Str == " " || Encoding.ASCII.GetBytes(Str)[0] == 9);
                    //Loop Until (Str <> " " And Asc(Str) <> 9)


                    if (StrString != "")
                    {
                        if (StrString.Length < 6) //mínimo requerido
                        {
                            if (StrString.Length >= 2)
                            {
                                if (StrString.Substring(0, 2) != "//")
                                {
                                    //MsgBox("La expresión " & StrString & " no es un estandar del reporteador de Melody-Milano, verifique" & vbCrLf & vbCrLf & "Encontrado en línea " & ConteoGeneral)
                                    dr.Close();
                                    return;
                                }
                            }
                            else
                            {
                                //MsgBox("La expresión " & StrString & " no es un estandar del reporteador de Melody-Milano, verifique" & vbCrLf & vbCrLf & "Encontrado en línea " & ConteoGeneral)
                                dr.Close();
                                return;
                            }
                        }

                        if (StrString.Substring(0, 2) != "//")
                        {
                            Expresiones[Conteo] = StrString;

                            if (Expresiones[Conteo].Substring(0, 3).ToUpper() == "SI ")
                            {
                                ConteoIfAbiertos = ConteoIfAbiertos + 1;
                                if (ConteoIfAbiertos > 10)
                                {
                                    //MsgBox("Demasiados SI´s anidados (Máximo 10 Si´s anidados)" & vbCrLf & "Error en renglón " & ConteoGeneral)
                                    dr.Close();
                                    return;
                                }
                                IfsEncontrados[ConteoIfAbiertos] = Conteo;
                            }

                            if (Expresiones[Conteo].Substring(0, 6).ToUpper() == "FIN SI")
                            {
                                if (ConteoIfAbiertos > 0)
                                {
                                    SiguienteEnIf[IfsEncontrados[ConteoIfAbiertos]] = Conteo;
                                    ConteoIfAbiertos = ConteoIfAbiertos - 1;
                                }
                                else
                                {
                                    //MsgBox("Parse error. Encontrada una expresión FIN SI sin declaración previa SI en renglon " & ConteoGeneral)
                                    dr.Close();
                                    return;
                                }
                            }

                            if (Expresiones[Conteo].ToUpper() == "INICIAPARTIDAS")
                            {
                                if (IniciaPartidasEn != -1)
                                {
                                    //MsgBox("Error al procesar el archivo de impresión del ticket; Encontrados más de 1 expresiones del tipo INICIAPARTIDAS y solo puede haber una en todo el archivo")
                                    dr.Close();
                                    return;
                                }
                                IniciaPartidasEn = Conteo;
                                Conteo = Conteo - 1;
                            }

                            if (Expresiones[Conteo].ToUpper() == "FINPARTIDAS")
                            {
                                if (IniciaPartidasEn == -1)
                                {
                                    //MsgBox("Error al procesar el archivo de impresión del ticket; Encontrados 1 expresión del tipo FINPARTIDAS sin inicio de partidas INICIAPARTIDAS; Verifica")
                                    dr.Close();
                                    return;
                                }

                                if (FinPartidasEn != -1)
                                {
                                    //MsgBox("Error al procesar el archivo de impresión del ticket; Encontrados más de 1 expresiones del tipo FINPARTIDAS y solo puede haber una en todo el archivo")
                                    dr.Close();
                                    return;
                                }
                                FinPartidasEn = Conteo;
                            }

                            if (Expresiones[Conteo].ToUpper() == "INICIADESCUENTOSGLOBALES")
                            {
                                if (IniciaDescuentosGlobalesEn != -1)
                                {
                                    //MsgBox("Error al procesar el archivo de impresión del ticket; Encontrados más de 1 expresiones del tipo INICIAPARTIDAS y solo puede haber una en todo el archivo")
                                    dr.Close();
                                    return;
                                }
                                IniciaDescuentosGlobalesEn = Conteo;
                                Conteo = Conteo - 1;
                            }

                            if (Expresiones[Conteo].ToUpper() == "FINDESCUENTOSGLOBALES")
                            {
                                if (IniciaDescuentosGlobalesEn == -1)
                                {
                                    //MsgBox("Error al procesar el archivo de impresión del ticket; Encontrados 1 expresión del tipo FINPARTIDAS sin inicio de partidas INICIAPARTIDAS; Verifica")
                                    dr.Close();
                                    return;
                                }

                                if (FinDescuentosGlobalesEn != -1)
                                {
                                    //MsgBox("Error al procesar el archivo de impresión del ticket; Encontrados más de 1 expresiones del tipo FINPARTIDAS y solo puede haber una en todo el archivo")
                                    dr.Close();
                                    return;
                                }
                                FinDescuentosGlobalesEn = Conteo;
                            }

                            if (Expresiones[Conteo].ToUpper() == "INICIACUPONES")
                            {
                                if (IniciaCuponesEn != -1)
                                {
                                    //MsgBox("Error al procesar el archivo de impresión del ticket; Encontrados más de 1 expresiones del tipo INICIAPARTIDAS y solo puede haber una en todo el archivo")
                                    dr.Close();
                                    return;
                                }
                                IniciaCuponesEn = Conteo;
                                Conteo = Conteo - 1;
                            }

                            if (Expresiones[Conteo].ToUpper() == "FINCUPONES")
                            {
                                if (IniciaCuponesEn == -1)
                                {
                                    //MsgBox("Error al procesar el archivo de impresión del ticket; Encontrados 1 expresión del tipo FINPARTIDAS sin inicio de partidas INICIAPARTIDAS; Verifica")
                                    dr.Close();
                                    return;
                                }

                                if (FinCuponesEn != -1)
                                {
                                    //MsgBox("Error al procesar el archivo de impresión del ticket; Encontrados más de 1 expresiones del tipo FINPARTIDAS y solo puede haber una en todo el archivo")
                                    dr.Close();
                                    return;
                                }
                                FinCuponesEn = Conteo;
                            }

                            if (Expresiones[Conteo].ToUpper() == "INICIANOTASDECREDITO")
                            {
                                if (IniciaNotasDeCreditoGeneradasEn != -1)
                                {
                                    //MsgBox("Error al procesar el archivo de impresión del ticket; Encontrados más de 1 expresiones del tipo INICIAPARTIDAS y solo puede haber una en todo el archivo")
                                    dr.Close();
                                    return;
                                }
                                IniciaNotasDeCreditoGeneradasEn = Conteo;
                                Conteo = Conteo - 1;
                            }

                            if (Expresiones[Conteo].ToUpper() == "FINNOTASDECREDITO")
                            {
                                if (IniciaNotasDeCreditoGeneradasEn == -1)
                                {
                                    //MsgBox("Error al procesar el archivo de impresión del ticket; Encontrados 1 expresión del tipo FINPARTIDAS sin inicio de partidas INICIAPARTIDAS; Verifica")
                                    dr.Close();
                                    return;
                                }

                                if (FinNotasDeCreditoGeneradasEn != -1)
                                {
                                    //MsgBox("Error al procesar el archivo de impresión del ticket; Encontrados más de 1 expresiones del tipo FINPARTIDAS y solo puede haber una en todo el archivo")
                                    dr.Close();
                                    return;
                                }
                                FinNotasDeCreditoGeneradasEn = Conteo;
                            }

                            Conteo = Conteo + 1;
                        }
                    }
                }
            }

            //Fin de la validación sintáctica de IF y END IF
            CargarDatosClienteYVenta();

            this.oImpresion = new System.Drawing.Printing.PrintDocument();
            oImpresion.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(ImprimeDocumento);

            //if(!bReimpresion && bAbrirCajon == true) //este si va
            //    sbAbrirCajon();


            System.Drawing.Printing.PrinterSettings settings = new System.Drawing.Printing.PrinterSettings();
            settings.PrinterName = sImpresora;

            System.Drawing.Printing.PrintController standardPrintController = new System.Drawing.Printing.StandardPrintController();

            oImpresion.DocumentName = "Folio " + FolioVenta;
            oImpresion.PrintController = standardPrintController;
            oImpresion.PrinterSettings = settings;


            #region #region v.2.8 CGC Integración de ticketsPv

            settings.PrinterName = sImpresora;

            if (iLargoTicket != 0)
            {
                System.Drawing.Printing.PaperSize paperSize = new System.Drawing.Printing.PaperSize("Custom", 315, (int)(iLargoTicket * 3.9375));
                paperSize.RawKind = (int)System.Drawing.Printing.PaperKind.Custom;
                oImpresion.DefaultPageSettings.PaperSize = paperSize;
            }

            #endregion


            TipoImpresion = 1;
            oImpresion.Print();
            //sbCortarPapel()

            if (!bReimpresion)
            {
                for (i = 1; i < iNumeroCopias; i++)
                {
                    oImpresion.DocumentName = FolioVenta + " Copia";
                    oImpresion.Print();
                    //sbCortarPapel()                    
                }
            }


            if (dtVoucherCliente.Rows.Count != 0)
            {
                TipoImpresion = 2;

                for (i = 0; i < dtVoucherCliente.Rows.Count; i++)
                {
                    voucherActual = i;

                    if (bReimpresion)
                    {
                        oImpresion.DocumentName = FolioVenta + " - Reimpresión " + (i + 1).ToString();
                        bCopiaComercio = false;
                        oImpresion.Print();
                    }
                    else
                    {
                        bCopiaComercio = true;
                        oImpresion.DocumentName = FolioVenta + " - Tarjeta Comercio " + (i + 1).ToString();
                        oImpresion.Print();
                        bCopiaComercio = false;
                        oImpresion.DocumentName = FolioVenta + " - Tarjeta Cliente " + (i + 1).ToString();
                        oImpresion.Print();
                    }
                }
            }

            #region v.2.8 CGC Integración de ticketsPv
            if (bTieneFinLag)
            {
                TipoImpresion = 3;

                oImpresion.DocumentName = FolioVenta + " - FinlagMilano";
                oImpresion.Print();

                oImpresion.DocumentName = FolioVenta + " - FinlagCliente";
                oImpresion.Print();
            }
            #endregion

        }

        //Una vez que tengo todo cargado, ahora si imprimo
        private void ImprimeDocumento(System.Object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int i, j;
            string StrString;
            //'ImprimeDocumento = True

            Printer = e;

            //Printer.PageSettings.PrintableArea.Width = Printer.PageSettings.PrintableArea.Width + 10;
            if (iPaginaActual == 0)//v.2.8 CGC Integración de ticketsPv
            {
                AnchoImpresoraHere = (Int32)Printer.PageSettings.PrintableArea.Width + 10;
                AltoImpresoraHere = (Int32)Printer.PageSettings.PrintableArea.Height + 10;
                //AltoImpresoraHere =280;
                currentXY.Y = 0;
                currentXY.X = 0;
                MargenX = 0;
                TipoFuente.Regular = true;
                // OCG: Se reduce el tamanio de la fuente
                TipoFuente.NombreFuente = "Arial";
                TipoFuente.Tamaño = 7; //OCG|27042022 Imprime vertical
                //--
                CambiarEstiloFuente();
                partidaActual = 0;
                descuentoActual = 0;
                cuponActual = 0;
                notaDeCreditoActual = 0;
            }
            else//v.2.8 CGC Integración de ticketsPv
                currentXY.Y = 0;

            //if (bReimpresion)
            if (bReimpresion == true && iPaginaActual == 0) //v.2.8 CGC Integración de ticketsPv
            {
                currentXY.X = (Int32)((AnchoImpresoraHere / 2) - (getTextWidth(prFont, "REIMPRESION") / 2));
                ImprimirTexto("REIMPRESION");//OCG|27042022 Imprime vertical
            }

            //if (TipoImpresion == 1)
            //{
            //    //La función se establece en FuncionParseada
            //    //for (i = 0; i <= Conteo - 1; i++)
            //    {
            //        //Checo que no sea la parte de partidas
            //        if (i != 0 && (i == IniciaPartidasEn || i == IniciaDescuentosGlobalesEn || i == IniciaCuponesEn || i == IniciaNotasDeCreditoGeneradasEn))
            //        {
            //            if (i == IniciaPartidasEn)
            //            {
            //                if (!ProcesarPartidas(ref Expresiones, ref SiguienteEnIf, IniciaPartidasEn, FinPartidasEn - 1, Conteo))
            //                {
            //                    Printer.HasMorePages = false;
            //                    throw new Exception("No se pudieron procesar las partidas");
            //                }
            //                i = FinPartidasEn;
            //            }
            //            else
            //            {
            //                if (i == IniciaDescuentosGlobalesEn)
            //                {
            //                    if (!ProcesarDescuentosGlobales(ref Expresiones, ref SiguienteEnIf, IniciaDescuentosGlobalesEn, FinDescuentosGlobalesEn - 1, Conteo))
            //                    {
            //                        Printer.HasMorePages = false;
            //                        throw new Exception("No se pudieron procesar los descuentos globales");
            //                    }
            //                    i = FinDescuentosGlobalesEn;
            //                }
            //                else
            //                {
            //                    if (i == IniciaCuponesEn)
            //                    {
            //                        if (!ProcesarCupones(ref Expresiones, ref SiguienteEnIf, IniciaCuponesEn, FinCuponesEn - 1, Conteo))
            //                        {
            //                            Printer.HasMorePages = false;
            //                            throw new Exception("No se pudieron generar los cupones");
            //                        }
            //                        i = FinCuponesEn;
            //                    }
            //                    else
            //                    {
            //                        if (i == IniciaNotasDeCreditoGeneradasEn)
            //                        {
            //                            if (!ProcesarNotasDeCredito(ref Expresiones, ref SiguienteEnIf, IniciaNotasDeCreditoGeneradasEn, FinNotasDeCreditoGeneradasEn - 1, Conteo))
            //                            {
            //                                Printer.HasMorePages = false;
            //                                throw new Exception("No se pudieron generar las notas de crédito");
            //                            }
            //                            i = FinNotasDeCreditoGeneradasEn;
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        else
            //        {
            //            //primero verifico que no sea una condicionante SI FIN SI
            //            if (SiguienteEnIf[i] != 0 || Expresiones[i].ToUpper() == "FIN SI")
            //            {
            //                if (Expresiones[i].ToUpper() != "FIN SI")
            //                {
            //                    if (!EvaluaExpresionSI(Expresiones[i]))
            //                    {
            //                        i = SiguienteEnIf[i];
            //                        if (i > Conteo - 1)
            //                        {
            //                            break;
            //                        }
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                if (Expresiones[i].Contains("IMPRIME DOBLE4"))
            //                    i = i;
            //                if (!EsSetVariable(Expresiones[i]))
            //                {
            //                    if (!GetVariables(Expresiones[i]))
            //                    {
            //                        if (FuncionParseada == "")
            //                        {
            //                            Printer.HasMorePages = false;
            //                            throw new Exception("Se mandó a llamar una función que no existe en nuestro diccionario en posición " + i + "; Expresión: " + Expresiones[i]);
            //                        }
            //                    }
            //                    else
            //                    {
            //                        for (j = 0; j <= CantidadExpresionesAParsear - 1; j++)
            //                        {
            //                            if (!GetCadenaAImprimir(j)) //Me pone el resultado en ExpresionesParseadas(i)
            //                            {
            //                                Printer.HasMorePages = false;
            //                                throw new Exception("Error al imprimir; Error al parsear la operación " + ExpresionesAParsear[i] + " en la expresion " + Expresiones[i]);
            //                            }
            //                        }

            //                        StrString = RealizarAccion(Expresiones[i]);
            //                        if (StrString != "")
            //                        {
            //                            Printer.HasMorePages = false;
            //                            throw new Exception("Ocurrió un error al procesar la expresión " + Expresiones[i] + "; Función: " + FuncionParseada + "Error en detalle: " + StrString);
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            //if (TipoImpresion == 2)
            //    ProcesaVoucher(dtVoucherCliente);


            #region v.2.8 CGC Integración de ticketsPv
            if (TipoImpresion == 1)
            {

                //La función se establece en FuncionParseada
                for (i = iPosicionAnterior; i <= Conteo - 1; i++)
                {
                    //Checo que no sea la parte de partidas
                    if (i != 0 && (i == IniciaPartidasEn || i == IniciaDescuentosGlobalesEn || i == IniciaCuponesEn || i == IniciaNotasDeCreditoGeneradasEn) || bEstoyEnPartidas == true || bEstoyEnCupones == true || bEstoyEnDescuentosGenerales == true)
                    {
                        if (i == IniciaPartidasEn || bEstoyEnPartidas == true)
                        {
                            bEstoyEnPartidas = true;
                            if (!ProcesarPartidas(ref Expresiones, ref SiguienteEnIf, IniciaPartidasEn, FinPartidasEn - 1, Conteo))
                            {
                                Printer.HasMorePages = false;
                                throw new Exception("No se pudieron procesar las partidas");
                            }
                            if (Printer.HasMorePages == true)
                            {
                                iPosicionAnterior = i;
                                iPaginaActual++;
                                return;
                            }

                            i = FinPartidasEn;
                            bEstoyEnPartidas = false;
                        }
                        else
                        {
                            if (i == IniciaDescuentosGlobalesEn)
                            {
                                bEstoyEnDescuentosGenerales = true;
                                if (!ProcesarDescuentosGlobales(ref Expresiones, ref SiguienteEnIf, IniciaDescuentosGlobalesEn, FinDescuentosGlobalesEn - 1, Conteo))
                                {
                                    Printer.HasMorePages = false;
                                    throw new Exception("No se pudieron procesar los descuentos globales");
                                }
                                if (Printer.HasMorePages == true)
                                {
                                    iPosicionAnterior = i;
                                    iPaginaActual++;
                                    return;
                                }
                                i = FinDescuentosGlobalesEn;
                                bEstoyEnDescuentosGenerales = false;
                            }
                            else
                            {
                                if (i == IniciaCuponesEn)
                                {
                                    if (!bEstoyEnCupones)
                                    {
                                        bEstoyEnCupones = true;
                                        Printer.HasMorePages = true;
                                        iPosicionAnterior = i;
                                        iPaginaActual++;
                                        return;
                                    }

                                    if (!ProcesarCupones(ref Expresiones, ref SiguienteEnIf, IniciaCuponesEn, FinCuponesEn - 1, Conteo))
                                    {
                                        Printer.HasMorePages = false;
                                        throw new Exception("No se pudieron generar los cupones");
                                    }
                                    i = FinCuponesEn;
                                    bEstoyEnCupones = false;
                                }
                                else
                                {
                                    if (i == IniciaNotasDeCreditoGeneradasEn)
                                    {
                                        if (!bEstoyEnNotasDecredito)
                                        {
                                            bEstoyEnNotasDecredito = true;
                                            Printer.HasMorePages = true;
                                            iPosicionAnterior = i;
                                            iPaginaActual++;
                                            return;
                                        }
                                        if (!ProcesarNotasDeCredito(ref Expresiones, ref SiguienteEnIf, IniciaNotasDeCreditoGeneradasEn, FinNotasDeCreditoGeneradasEn - 1, Conteo))
                                        {
                                            Printer.HasMorePages = false;
                                            throw new Exception("No se pudieron generar las notas de crédito");
                                        }
                                        i = FinNotasDeCreditoGeneradasEn;
                                        bEstoyEnNotasDecredito = false;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //primero verifico que no sea una condicionante SI FIN SI
                        if (SiguienteEnIf[i] != 0 || Expresiones[i].ToUpper() == "FIN SI")
                        {
                            if (Expresiones[i].ToUpper() != "FIN SI")
                            {
                                if (!EvaluaExpresionSI(Expresiones[i]))
                                {
                                    i = SiguienteEnIf[i];
                                    if (i > Conteo - 1)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (Expresiones[i].Contains("IMPRIME DOBLE4"))
                                i = i;
                            if (!EsSetVariable(Expresiones[i]))
                            {
                                if (!GetVariables(Expresiones[i]))
                                {
                                    if (FuncionParseada == "")
                                    {
                                        Printer.HasMorePages = false;
                                        throw new Exception("Se mandó a llamar una función que no existe en nuestro diccionario en posición " + i + "; Expresión: " + Expresiones[i]);
                                    }
                                }
                                else
                                {
                                    for (j = 0; j <= CantidadExpresionesAParsear - 1; j++)
                                    {
                                        if (!GetCadenaAImprimir(j)) //Me pone el resultado en ExpresionesParseadas(i)
                                        {
                                            Printer.HasMorePages = false;
                                            throw new Exception("Error al imprimir; Error al parsear la operación " + ExpresionesAParsear[i] + " en la expresion " + Expresiones[i]);
                                        }
                                    }

                                    StrString = RealizarAccion(Expresiones[i]);
                                    if (currentXY.Y + 10 >= AltoImpresoraHere)
                                    {
                                        Printer.HasMorePages = true;
                                        iPosicionAnterior = i;
                                        iPaginaActual++;
                                        return;
                                    }
                                    if (StrString != "")
                                    {
                                        Printer.HasMorePages = false;
                                        throw new Exception("Ocurrió un error al procesar la expresión " + Expresiones[i] + "; Función: " + FuncionParseada + "Error en detalle: " + StrString);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            iPaginaActual = 0;

            if (TipoImpresion == 2)
                ProcesaVoucher(dtVoucherCliente);

            if (TipoImpresion == 3)
                ProcesaFinlag();
            #endregion

            Printer.HasMorePages = false;
        }

        // v.2.8 CGC Integración de ticketsPv
        private void ProcesaFinlag()
        {
            //int i;
            //bool bBandera = false;
            Printer.HasMorePages = true;
            //currentXY.Y = 0;
            //currentXY.X = 0;
            FuncionParseada = "PONFUENTE";
            ExpresionesParseadas[0] = "Courier Condensed";
            ExpresionesParseadas[1] = "7";
            CantidadExpresionesAParsear = 2;
            RealizarAccion(FuncionParseada);
            FuncionParseada = "IMPRIMECENTRADO";

            CantidadExpresionesAParsear = 1;
            ExpresionesParseadas[0] = dtInfoEmpresa.Rows[0]["RAZONSOCIALMARCA"].ToString();
            RealizarAccion("");
            ExpresionesParseadas[0] = "Puebla No. 329 Col. Roma";
            RealizarAccion("");
            ExpresionesParseadas[0] = "Del. Cuauhtemoc, CDMX, CP 06700";
            RealizarAccion("");
            ExpresionesParseadas[0] = "RFC: DIS-880803-JW8";
            RealizarAccion("");
            ExpresionesParseadas[0] = "SUC. " + dtInfoSucursal.Rows[0]["CODIGOTIENDASUCURSAL"] + " - " + dtInfoSucursal.Rows[0]["DESCRIPCIONTIENDASUCURSAL"];
            RealizarAccion("");
            ExpresionesParseadas[0] = "VENTA FINLAG " + dtCabecero.Rows[0]["FECHACABECERO"];
            RealizarAccion("");
            FuncionParseada = "IMPRIME";
            ImprimirTexto("-------------------");
            ImprimirTexto("Punto de venta: MILANO");
            ExpresionesParseadas[0] = "Referencia: " + dtCabecero.Rows[0]["FOLIOVENTACABECERO"];
            RealizarAccion("");
            ExpresionesParseadas[0] = "ID Promotor: " + dtFinLag.Rows[0]["IDDISTRIBUIDORAFINLAG"];
            RealizarAccion("");
            ExpresionesParseadas[0] = "Folio Vale: " + dtFinLag.Rows[0]["FOLIOVALEFINLAG"];
            RealizarAccion("");
            ImprimirTexto("Datos del Cliente:");
            ExpresionesParseadas[0] = dtFinLag.Rows[0]["NOMBRECOMPLETOFINLAG"].ToString();
            RealizarAccion("");
            ExpresionesParseadas[0] = dtFinLag.Rows[0]["CALLEFINLAG"].ToString();
            RealizarAccion("");
            ExpresionesParseadas[0] = "Col " + dtFinLag.Rows[0]["COLONIAFINLAG"];
            RealizarAccion("");
            ExpresionesParseadas[0] = "C.P. " + dtFinLag.Rows[0]["CPFINLAG"];
            RealizarAccion("");
            ExpresionesParseadas[0] = dtFinLag.Rows[0]["MUNICIPIOFINLAG"] + ", " + dtFinLag.Rows[0]["ESTADOFINLAG"];
            RealizarAccion("");
            ImprimirTexto("");
            ExpresionesParseadas[0] = "Cantidad Vale: " + dtFinLag.Rows[0]["MONTOAPLICADOFINLAG"];
            RealizarAccion("");
            if (Double.Parse(dtFinLag.Rows[0]["TOTALPAGARFINLAG"].ToString().Replace("$", "")) != 0)
            {
                ExpresionesParseadas[0] = "Total a pagar:" + dtFinLag.Rows[0]["TOTALPAGARFINLAG"];
                RealizarAccion("");
                ExpresionesParseadas[0] = "Pago Quincenal: " + dtFinLag.Rows[0]["PAGOQUINCENALFINLAG"];
                RealizarAccion("");
                ExpresionesParseadas[0] = "Número de pagos: " + dtFinLag.Rows[0]["QUINCENASFINLAG"];
                RealizarAccion("");
                ExpresionesParseadas[0] = "Fecha del primer pago: " + dtFinLag.Rows[0]["FECHAPRIMERPAGOFINLAG"];
                RealizarAccion("");
                ImprimirTexto("Dias de pago: Día 15 y último de cada mes");
                ImprimirTexto("---------------------------------------------------------------------");
                ExpresionesParseadas[0] = dtFinLag.Rows[0]["PAGAREFINLAG"].ToString();
                FuncionParseada = "IMPRIMECOMPLETO";
                RealizarAccion("");
                ImprimirTexto("---------------------------------------------------------------------");
            }
            if (Int32.Parse(dtFinLag.Rows[0]["PUNTOSUTILIZADOSFINLAG"].ToString()) != 0)
            {
                ExpresionesParseadas[0] = "Efectivo Puntos:" + dtFinLag.Rows[0]["EFECTIVOPUNTOSFINLAG"];
                RealizarAccion("");
                ExpresionesParseadas[0] = "Puntos Utilizados: " + dtFinLag.Rows[0]["PUNTOSUTILIZADOSFINLAG"];
                RealizarAccion("");
            }
            ImprimirTexto("");
            ImprimirTexto("");
            FuncionParseada = "IMPRIMECENTRADO";
            if (bCopiaComercioFinlag)
            {
                ExpresionesParseadas[0] = dtFinLag.Rows[0]["NOMBRECOMPLETOFINLAG"].ToString();
                RealizarAccion("");
                ExpresionesParseadas[0] = "FIRMA DEL CLIENTE";
                RealizarAccion("");
                ExpresionesParseadas[0] = "____________________________";
                RealizarAccion("");
                ExpresionesParseadas[0] = "COPIA COMERCIO";
                RealizarAccion("");
            }
            else
            {
                ExpresionesParseadas[0] = "COPIA CLIENTE";
                RealizarAccion("");
            }

            ImprimirTexto("");
            ImprimirTexto("");
            bCopiaComercioFinlag = true;
        }

        private void ProcesaVoucher(DataTable voucher)
        {
            int i;
            bool bBandera = false;
            Printer.HasMorePages = true;
            //currentXY.Y = 0;
            //currentXY.X = 0;
            FuncionParseada = "PONFUENTE";
            ExpresionesParseadas[0] = "Courier Condensed";
            ExpresionesParseadas[1] = "7";
            CantidadExpresionesAParsear = 2;
            RealizarAccion(FuncionParseada);
            FuncionParseada = "IMPRIMECENTRADO";
            i = voucherActual;

            CantidadExpresionesAParsear = 1;
            ExpresionesParseadas[0] = "BBVA";
            RealizarAccion("");
            ExpresionesParseadas[0] = dtInfoEmpresa.Rows[0]["RAZONSOCIALMARCA"].ToString();
            RealizarAccion("");
            ExpresionesParseadas[0] = "Puebla No. 329 Col. Roma";
            RealizarAccion("");
            ExpresionesParseadas[0] = "Del. Cuauhtemoc, CDMX, CP 06700";
            RealizarAccion("");
            ExpresionesParseadas[0] = "RFC: DIS-880803-JW8";
            RealizarAccion("");
            ExpresionesParseadas[0] = "SUC. " + dtInfoSucursal.Rows[0]["CODIGOTIENDASUCURSAL"] + " - " + dtInfoSucursal.Rows[0]["DESCRIPCIONTIENDASUCURSAL"];
            RealizarAccion("");
            ExpresionesParseadas[0] = "AFILIACIÓN: " + voucher.Rows[i]["afiliacionVB"];
            RealizarAccion("");
            ImprimirTexto("");
            if (dtCabecero.Rows[0]["TIPOTRANSACCIONCABECERO"].ToString() == "EG")
                ExpresionesParseadas[0] = "RETIRO DE EFECTIVO SIN COMPRA";
            else
                ExpresionesParseadas[0] = "VENTA";
            RealizarAccion("");
            ImprimirTexto("");

            if (voucher.Rows[i]["EsBancomerVB"].ToString() == "1")
            {
                ExpresionesParseadas[0] = "TARJETA: ************" + voucher.Rows[i]["numeroTarjetaVB"];
                RealizarAccion("");
                ImprimirTexto("");

                ExpresionesParseadas[0] = voucher.Rows[i]["nombreAplicacionVB"].ToString();
                RealizarAccion("");
                ImprimirTexto("");
                ExpresionesParseadas[0] = voucher.Rows[i]["leyendaRespuestaVB"].ToString();
                RealizarAccion("");

                ExpresionesParseadas[0] = voucher.Rows[i]["CriptogramaVB"].ToString();
                RealizarAccion("");

                ExpresionesParseadas[0] = voucher.Rows[i]["AIDVB"].ToString();
                RealizarAccion("");

                if (voucher.Rows[i]["PagoConPuntosVB"].ToString() == "1")
                {
                    double dImporte = Double.Parse(voucher.Rows[i]["importeTransaccionVB"].ToString());
                    double dImportePagadoPuntosPesos = Double.Parse(voucher.Rows[i]["importeRedimidoPesosVB"].ToString());
                    double dDiferencia = dImporte - dImportePagadoPuntosPesos;
                    ExpresionesParseadas[0] = "";
                    RealizarAccion("");

                    ExpresionesParseadas[0] = "Importe Venta: $" + Double.Parse(voucher.Rows[i]["importeTransaccionVB"].ToString()).ToString("###,##0.00") + " MXN";
                    RealizarAccion("");

                    ExpresionesParseadas[0] = "Pagado con Puntos: $" + Double.Parse(voucher.Rows[i]["ImporteRedimidoPesosVB"].ToString()).ToString("###,##0.00") + " MXN";
                    //ExpresionesParseadas[0] = "Pagado con Ptos: " + Double.Parse(voucher.Rows[i]["SaldoRedimidoPuntosVB"].ToString()).ToString("###,##0") + " puntos";
                    RealizarAccion("");
                    ExpresionesParseadas[0] = "Total a pagar: $" + dDiferencia.ToString("###,##0.00") + " MXN";
                    RealizarAccion("");
                    ExpresionesParseadas[0] = "";
                    RealizarAccion("");
                }

                if (voucher.Rows[i]["PagoConPuntosVB"].ToString() != "1")
                {
                    ExpresionesParseadas[0] = "Importe Venta: $" + Double.Parse(voucher.Rows[i]["ImporteTransaccionVB"].ToString()).ToString("###,##0.00");
                }

                RealizarAccion("");
                if (voucher.Rows[i]["EsCashBackVB"].ToString() == "True")
                {
                    bBandera = true;
                    ExpresionesParseadas[0] = "Disp. Efectivo: $" + Double.Parse(voucher.Rows[i]["DisponibleEfectivoVB"].ToString()).ToString("###,##0.00");
                    RealizarAccion("");
                }
                if (voucher.Rows[i]["PagoConPuntosVB"].ToString() == "True")
                {
                    bBandera = true;
                    ExpresionesParseadas[0] = "Pagado con Puntos BBVA: " + Double.Parse(voucher.Rows[i]["MontoPagadoConPuntosVB"].ToString()).ToString("###,##0") + " puntos";
                    RealizarAccion("");
                }
                if (bBandera)
                {
                    ExpresionesParseadas[0] = "Total a pagar: $" + Double.Parse(voucher.Rows[i]["MontoVentaTotalVB"].ToString()).ToString("###,##0.00");
                    RealizarAccion("");
                }

                ExpresionesParseadas[0] = "Operador: " + dtCabecero.Rows[0]["CODIGOEMPLEADOCABECERO"] + " " + dtCabecero.Rows[0]["NOMBRECAJEROCABECERO"] + " " + dtCabecero.Rows[0]["PATERNOCAJEROCABECERO"];
                RealizarAccion("");


                ExpresionesParseadas[0] = "REF: " + voucher.Rows[i]["ReferenciaFinancieraVB"];
                RealizarAccion("");
                ExpresionesParseadas[0] = "REF Comercio: " + FolioVenta;
                RealizarAccion("");
                ExpresionesParseadas[0] = "CAJA: " + voucher.Rows[i]["NumeroTerminalVB"];
                RealizarAccion("");
                ExpresionesParseadas[0] = "SEC TNX: " + voucher.Rows[i]["SecuenciaTransaccionVB"];
                RealizarAccion("");

                if (voucher.Rows[i]["PagoConPuntosVB"].ToString() == "1" && !bCopiaComercio)
                {
                    ExpresionesParseadas[0] = "PUNTOS BBVA";
                    RealizarAccion("");
                    ExpresionesParseadas[0] = "Saldo Anterior Puntos: " + Double.Parse(voucher.Rows[i]["SaldoAnteriorPuntosVB"].ToString()).ToString("###,##0");
                    RealizarAccion("");
                    ExpresionesParseadas[0] = "Saldo Anterior Pesos: $" + Double.Parse(voucher.Rows[i]["ImportePesosAnteriorVB"].ToString()).ToString("###,##0.00");
                    RealizarAccion("");
                    ExpresionesParseadas[0] = "Puntos Redimidos : " + Double.Parse(voucher.Rows[i]["SaldoRedimidoPuntosVB"].ToString()).ToString("###,##0");
                    RealizarAccion("");
                    ExpresionesParseadas[0] = "Pesos Redimidos: $" + Double.Parse(voucher.Rows[i]["ImporteRedimidoPesosVB"].ToString()).ToString("###,##0.00");
                    RealizarAccion("");
                    ExpresionesParseadas[0] = "Saldo Disponible Puntos: " + Double.Parse(voucher.Rows[i]["SaldoActualPuntosVB"].ToString()).ToString("###,##0");
                    RealizarAccion("");
                    ExpresionesParseadas[0] = "Saldo Disponible Pesos: $" + Double.Parse(voucher.Rows[i]["SaldoPuntosImportePesosVB"].ToString()).ToString("###,##0.00");
                    RealizarAccion("");
                    ImprimirTexto("");
                }

                if (voucher.Rows[i]["Leyenda1VB"].ToString() != "")
                {
                    ImprimirTexto("");
                    FuncionParseada = "IMPRIMECENTRADO";
                    ExpresionesParseadas[0] = voucher.Rows[i]["Leyenda1VB"].ToString();
                    RealizarAccion("");
                    FuncionParseada = "IMPRIMECENTRADO";
                    ImprimirTexto("");
                }


                FuncionParseada = "IMPRIMECOMPLETO";
                ExpresionesParseadas[0] = "Por este pagaré me obligo incondicionalmente a pagar a la orden del banco acreditante el importe de este titulo. Este pagaré procede del contrato de apertura de crédito que el banco acreditante y el tarjetahabiente tienen celebrado";
                RealizarAccion("");
                FuncionParseada = "IMPRIMECENTRADO";



                //if (bCopiaComercio)
                //{
                ImprimirTexto("");
                if (voucher.Rows[i]["FirmaElectronicaVB"].ToString() == "1")
                    ExpresionesParseadas[0] = "AUTORIZADO MEDIANTE FIRMA ELECTRÓNICA";
                else
                if (voucher.Rows[i]["FirmaElectronicaVB"].ToString() == "2")
                    ExpresionesParseadas[0] = "AUTORIZADO SIN FIRMA";
                if (voucher.Rows[i]["FirmaElectronicaVB"].ToString() == "0")
                {
                    ExpresionesParseadas[0] = "";
                    RealizarAccion("");
                    ExpresionesParseadas[0] = "____________________________________";

                    RealizarAccion("");
                    ExpresionesParseadas[0] = "FIRMA";

                }
                RealizarAccion("");
                //}

                ExpresionesParseadas[0] = voucher.Rows[i]["NombreClienteVB"].ToString();
                RealizarAccion("");
                ExpresionesParseadas[0] = voucher.Rows[i]["FechaHoraComercioVB"].ToString();
                RealizarAccion("");
                FuncionParseada = "IMPRIMECOMPLETO";
                ExpresionesParseadas[0] = "Pagaré negociable únicamente en instituciones de crédito";
                RealizarAccion("");
            }
            else  //Tarjeta de crédito Melody-Milano
            {
                ImprimirTexto("Fecha: " + dtCabecero.Rows[0]["FECHACABECERO"] + "HORA: " + dtCabecero.Rows[0]["HORACABECERO"]);
                ImprimirTexto("Transacción: " + dtCabecero.Rows[0]["TRANSACCIONCABECERO"] + " CAJA " + dtCabecero.Rows[0]["CODIGOCAJACABECERO"]);
                ImprimirTexto("Cajero: " + dtCabecero.Rows[0]["CODIGOEMPLEADOCABECERO"]);
                ImprimirTexto("TARJETA: ************" + voucher.Rows[i]["numeroTarjetaVB"].ToString().Substring(12));
                ImprimirTexto("Monto:          $" + Double.Parse(voucher.Rows[i]["importeTransaccionVB"].ToString()).ToString("###,##0.00"));
                ImprimirTexto("Autorizacion: " + voucher.Rows[i]["autorizacionVB"] + " / En línea");
                ImprimirTexto(voucher.Rows[i]["secuenciaPOSVB"] + " MESES CUOTA FIJA");
                ImprimirTexto("Si usted paga puntualmente, pagará $" + Double.Parse(voucher.Rows[i]["CodigoRespuestaVB"].ToString()).ToString("###,##0.00"));
                ImprimirTexto("Para no generar intereses, deberá cubrir el total");
                ImprimirTexto("de su cuenta antes de su fecha de corte");
                ImprimirTexto("");

                if (voucher.Rows[i]["leyenda1VB"].ToString() != "")
                {
                    FuncionParseada = "IMPRIMECOMPLETO";
                    ExpresionesParseadas[0] = voucher.Rows[i]["leyenda1VB"].ToString();
                    RealizarAccion("");
                    ImprimirTexto("");
                }

                if (voucher.Rows[i]["leyenda2VB"].ToString() != "")
                {
                    FuncionParseada = "IMPRIMECOMPLETO";
                    ExpresionesParseadas[0] = voucher.Rows[i]["leyenda2VB"].ToString();
                    RealizarAccion("");
                    ImprimirTexto("");
                }

                FuncionParseada = "IMPRIMECENTRADO";
                ExpresionesParseadas[0] = "______________________________";
                RealizarAccion("");
                FuncionParseada = "IMPRIMECENTRADO";
                ExpresionesParseadas[0] = "Firma";
                RealizarAccion("");

                if (voucher.Rows[i]["leyenda3VB"].ToString() != "")
                {
                    FuncionParseada = "IMPRIMECOMPLETO";
                    ExpresionesParseadas[0] = voucher.Rows[i]["leyenda3VB"].ToString();
                    RealizarAccion("");
                    ImprimirTexto("");
                }
            }

            FuncionParseada = "IMPRIMECENTRADO";
            if (!bCopiaComercio)
                ExpresionesParseadas[0] = "COPIA CLIENTE";
            else
                ExpresionesParseadas[0] = "COPIA COMERCIO";
            RealizarAccion("");
            ImprimirTexto("");
            ImprimirTexto("");
        }

        private bool ProcesarPartidas(ref string[] Expresiones, ref int[] SiguienteEnIf, int Inicio, int Fin, int Conteo)
        {
            int i = 0, j = 0, m = 0;
            string StrString;
            string sExpresion = "";

            //MaximoY = Printer.CurrentY
            for (m = partidaActual; m <= TotalPartidas - 1; m++)
            {
                //Printer.CurrentY = MaximoY
                for (i = Inicio; i <= Fin; i++)
                {
                    sExpresion = Expresiones[i];
                    //primero verifico que no sea una condicionante SI FIN SI
                    if (SiguienteEnIf[i] != 0 || sExpresion.ToUpper() == "FIN SI")
                    {
                        if (sExpresion.ToUpper() != "FIN SI")
                        {
                            if (!EvaluaExpresionSI(sExpresion))
                            {
                                i = SiguienteEnIf[i];
                                if (i > Conteo - 1)
                                    break;

                                //Si no, automáticamente se sigue con la siguiente línea
                            }
                        }
                    }
                    else
                    {
                        //Primero verifico que no sea un set de variables ej ENTERO1 = 45*3/2
                        if (!EsSetVariable(sExpresion))
                        {
                            if (!GetVariables(sExpresion))
                            {
                                if (FuncionParseada == "")
                                {   //'Printer.EndDoc()
                                    //MsgBox("Error al imprimir; No se encontró la función en la expresión " & Expresiones(i))
                                    return false;
                                }
                            }
                            else
                            {
                                for (j = 0; j <= CantidadExpresionesAParsear - 1; j++)
                                {
                                    if (!GetCadenaAImprimir(j)) //'Me pone el resultado en ExpresionesParseadas(i)
                                    {
                                        //' Printer.EndDoc()
                                        //MsgBox("Error al imprimir; Error al parsear la operación " & ExpresionesAParsear(i) & " en la expresion " & Expresiones(i))
                                        return false;
                                    }
                                }

                                StrString = RealizarAccion(sExpresion);
                                if (StrString != "")
                                {
                                    //Printer.EndDoc()
                                    //MsgBox("Ocurrió un error al procesar la expresión " & Expresiones(i) & "; Función: " & FuncionParseada & vbCrLf & vbCrLf & "Error en detalle: " & StrString)
                                    return false;
                                }

                                //v.2.8 CGC Integración de ticketsPv
                                if (currentXY.Y + 10 >= AltoImpresoraHere)
                                {
                                    Printer.HasMorePages = true;
                                    return true;
                                }

                            }  //If GetVariables
                        }
                    }
                    //MaximoY = Printer.CurrentY
                }
                //SumaCantidadPartidas = SumaCantidadPartidas + CantidadPartida(partidaActual)
                //SumaImpuestoPartidas = SumaImpuestoPartidas + (CantidadPartida(partidaActual) * ImpuestoPartida(partidaActual))
                //SumaTotalPartidas = SumaTotalPartidas + TotalPartida(partidaActual)
                partidaActual++;

            }
            return true;
        }

        private bool ProcesarDescuentosGlobales(ref string[] Expresiones, ref int[] SiguienteEnIf, int Inicio, int Fin, int Conteo)
        {
            int i = 0, j = 0, m = 0;
            string StrString;
            string sExpresion = "";

            //MaximoY = Printer.CurrentY
            for (m = descuentoActual; m <= TotalDescuentosGlobales - 1; m++)
            {
                //Printer.CurrentY = MaximoY
                for (i = Inicio; i <= Fin; i++)
                {
                    sExpresion = Expresiones[i];
                    //primero verifico que no sea una condicionante SI FIN SI
                    if (SiguienteEnIf[i] != 0 || sExpresion.ToUpper() == "FIN SI")
                    {
                        if (sExpresion.ToUpper() != "FIN SI")
                        {
                            if (!EvaluaExpresionSI(sExpresion))
                            {
                                i = SiguienteEnIf[i];
                                if (i > Conteo - 1)
                                    break;

                                //Si no, automáticamente se sigue con la siguiente línea
                            }
                        }
                    }
                    else
                    {
                        //Primero verifico que no sea un set de variables ej ENTERO1 = 45*3/2
                        if (!EsSetVariable(sExpresion))
                        {
                            if (!GetVariables(sExpresion))
                            {
                                if (FuncionParseada == "")
                                {   //'Printer.EndDoc()
                                    //MsgBox("Error al imprimir; No se encontró la función en la expresión " & Expresiones(i))
                                    return false;
                                }
                            }
                            else
                            {
                                for (j = 0; j <= CantidadExpresionesAParsear - 1; j++)
                                {
                                    if (!GetCadenaAImprimir(j)) //'Me pone el resultado en ExpresionesParseadas(i)
                                    {
                                        //' Printer.EndDoc()
                                        //MsgBox("Error al imprimir; Error al parsear la operación " & ExpresionesAParsear(i) & " en la expresion " & Expresiones(i))
                                        return false;
                                    }
                                }

                                StrString = RealizarAccion(sExpresion);
                                if (StrString != "")
                                {
                                    //Printer.EndDoc()
                                    //MsgBox("Ocurrió un error al procesar la expresión " & Expresiones(i) & "; Función: " & FuncionParseada & vbCrLf & vbCrLf & "Error en detalle: " & StrString)
                                    return false;
                                }

                                //v.2.8 CGC Integración de ticketsPv
                                if (currentXY.Y + 10 >= AltoImpresoraHere)
                                {
                                    Printer.HasMorePages = true;
                                    return true;
                                }

                            }  //If GetVariables
                        }
                    }
                }
                descuentoActual++;
            }
            return true;
        }

        private bool ProcesarCupones(ref string[] Expresiones, ref int[] SiguienteEnIf, int Inicio, int Fin, int Conteo)
        {
            int i = 0, j = 0, m = 0;
            string StrString;
            string sExpresion = "";

            //MaximoY = Printer.CurrentY
            for (m = cuponActual; m <= TotalCuponesGenerados - 1; m++)
            {
                //Printer.CurrentY = MaximoY
                for (i = Inicio; i <= Fin; i++)
                {
                    sExpresion = Expresiones[i];
                    //primero verifico que no sea una condicionante SI FIN SI
                    if (SiguienteEnIf[i] != 0 || sExpresion.ToUpper() == "FIN SI")
                    {
                        if (sExpresion.ToUpper() != "FIN SI")
                        {
                            if (!EvaluaExpresionSI(sExpresion))
                            {
                                i = SiguienteEnIf[i];
                                if (i > Conteo - 1)
                                    break;
                            }
                        }
                    }
                    else
                    {
                        //Primero verifico que no sea un set de variables ej ENTERO1 = 45*3/2
                        if (!EsSetVariable(sExpresion))
                        {
                            if (!GetVariables(sExpresion))
                            {
                                if (FuncionParseada == "")
                                {   //'Printer.EndDoc()
                                    //MsgBox("Error al imprimir; No se encontró la función en la expresión " & Expresiones(i))
                                    return false;
                                }
                            }
                            else
                            {
                                for (j = 0; j <= CantidadExpresionesAParsear - 1; j++)
                                {
                                    if (!GetCadenaAImprimir(j)) //'Me pone el resultado en ExpresionesParseadas(i)
                                    {
                                        //' Printer.EndDoc()
                                        //MsgBox("Error al imprimir; Error al parsear la operación " & ExpresionesAParsear(i) & " en la expresion " & Expresiones(i))
                                        return false;
                                    }
                                }

                                StrString = RealizarAccion(sExpresion);
                                if (StrString != "")
                                {
                                    //Printer.EndDoc()
                                    //MsgBox("Ocurrió un error al procesar la expresión " & Expresiones(i) & "; Función: " & FuncionParseada & vbCrLf & vbCrLf & "Error en detalle: " & StrString)
                                    return false;
                                }

                            }  //If GetVariables
                        }
                    }
                }
                cuponActual++;
            }
            return true;
        }

        private bool ProcesarNotasDeCredito(ref string[] Expresiones, ref int[] SiguienteEnIf, int Inicio, int Fin, int Conteo)
        {
            int i = 0, j = 0, m = 0;
            string StrString;
            string sExpresion = "";

            //MaximoY = Printer.CurrentY
            for (m = notaDeCreditoActual; m <= TotalNotasDeCreditoGeneradas - 1; m++)
            {
                //Printer.CurrentY = MaximoY
                for (i = Inicio; i <= Fin; i++)
                {
                    sExpresion = Expresiones[i];
                    //primero verifico que no sea una condicionante SI FIN SI
                    if (SiguienteEnIf[i] != 0 || sExpresion.ToUpper() == "FIN SI")
                    {
                        if (sExpresion.ToUpper() != "FIN SI")
                        {
                            if (!EvaluaExpresionSI(sExpresion))
                            {
                                i = SiguienteEnIf[i];
                                if (i > Conteo - 1)
                                    break;
                            }
                        }
                    }
                    else
                    {
                        //Primero verifico que no sea un set de variables ej ENTERO1 = 45*3/2
                        if (!EsSetVariable(sExpresion))
                        {
                            if (!GetVariables(sExpresion))
                            {
                                if (FuncionParseada == "")
                                {   //'Printer.EndDoc()
                                    //MsgBox("Error al imprimir; No se encontró la función en la expresión " & Expresiones(i))
                                    return false;
                                }
                            }
                            else
                            {
                                for (j = 0; j <= CantidadExpresionesAParsear - 1; j++)
                                {
                                    if (!GetCadenaAImprimir(j)) //'Me pone el resultado en ExpresionesParseadas(i)
                                    {
                                        //' Printer.EndDoc()
                                        //MsgBox("Error al imprimir; Error al parsear la operación " & ExpresionesAParsear(i) & " en la expresion " & Expresiones(i))
                                        return false;
                                    }
                                }

                                StrString = RealizarAccion(sExpresion);
                                if (StrString != "")
                                {
                                    //Printer.EndDoc()
                                    //MsgBox("Ocurrió un error al procesar la expresión " & Expresiones(i) & "; Función: " & FuncionParseada & vbCrLf & vbCrLf & "Error en detalle: " & StrString)
                                    return false;
                                }

                            }  //If GetVariables
                        }
                    }
                }
                notaDeCreditoActual++;
            }
            return true;
        }

        private string RealizarAccion(string Expresion)
        {
            string StrCadena;
            int i;
            int LongHere;
            int outInt;
            int outInt2;
            int outInt3;
            double outDouble;

            switch (FuncionParseada)
            {
                case "IMPRIME":  //'Expresion, Formato, CaracteresAImprimir, EmpezarEn (Positivo agregar espacios, negativo alinear derecha)
                    if (CantidadExpresionesAParsear >= 1 && CantidadExpresionesAParsear <= 4)
                    {
                        StrCadena = ExpresionesParseadas[0];
                        if (CantidadExpresionesAParsear >= 2)
                        {
                            if (ExpresionesParseadas[1] != "") //Validamos el formato
                            {
                                double dDobleFormato;
                                if (Double.TryParse(ExpresionesParseadas[0], out dDobleFormato))
                                    StrCadena = dDobleFormato.ToString(ExpresionesParseadas[1]);
                                else
                                    StrCadena = String.Format(ExpresionesParseadas[0], ExpresionesParseadas[1]);
                            }
                        }

                        if (CantidadExpresionesAParsear >= 3)
                        {
                            if (Int32.TryParse(ExpresionesParseadas[2], out outInt))   //Validamos los caracteres a imprimir
                            {
                                LongHere = outInt;
                                if (LongHere > 0)
                                {
                                    if (StrCadena.Length > LongHere)
                                        StrCadena = StrCadena.Substring(0, LongHere);
                                }
                            }
                        }

                        if (CantidadExpresionesAParsear == 4)
                        {
                            if (Int32.TryParse(ExpresionesParseadas[3], out LongHere)) //Validamos los números a rellenar con 0´s
                            {
                                if (LongHere > 0)
                                    StrCadena = new String(' ', Int32.Parse(ExpresionesParseadas[3])) + StrCadena;
                                else
                                {
                                    //Aquí va alineado a la derecha, por lo que rellenamos con espacios a la izquierda
                                    //a manera que se alinee exactamente a la derecha que nos dice
                                    if (LongHere <= 0)
                                    {
                                        LongHere = LongHere * (-1);
                                        if (StrCadena.Length < LongHere)
                                            StrCadena = new String(' ', LongHere - StrCadena.Length) + StrCadena;

                                    }
                                }
                            }
                        }
                        //AñadirLogFile("Imprimiendo " & StrCadena)
                        //On Error Resume Next
                        //Printer.Print(StrCadena)
                        //Printer.CurrentX = MargenX
                        //On Error GoTo 0
                        ImprimirTexto(StrCadena);
                    }
                    break;
                case "IMPRIMECOMPLETO":
                    if (CantidadExpresionesAParsear == 1)
                    {
                        double dAnchoTexto;
                        double dPosXHere;
                        string dTextoHere;
                        dPosXHere = currentXY.X;
                        dTextoHere = ExpresionesParseadas[0];
                        dAnchoTexto = getTextWidth(prFont, dTextoHere);
                        while (dAnchoTexto > AnchoImpresoraHere - currentXY.X)
                        {
                            for (i = dTextoHere.Length - 1; i >= 3; i--)
                            {
                                if (getTextWidth(prFont, dTextoHere.Substring(0, i)) <= AnchoImpresoraHere - dPosXHere - getTextWidth(prFont, "MM"))
                                {
                                    //ImprimirTexto(dTextoHere.Substring(0, i),false);//OCG|27042022
                                    ImprimirTexto(dTextoHere.Substring(0, i));//OCG|27042022
                                    dTextoHere = dTextoHere.Substring(i);
                                    dAnchoTexto = getTextWidth(prFont, dTextoHere);
                                    currentXY.X = (Int32)dPosXHere;
                                    break;
                                }
                            }
                        }
                        ImprimirTexto(dTextoHere);//OCG|27042022 Imprime vertical
                    }
                    break;
                case "IMPRIMECENTRADO":
                    if (CantidadExpresionesAParsear == 1)
                    {
                        StrCadena = ExpresionesParseadas[0];
                        currentXY.X = (Int32)((AnchoImpresoraHere / 2) - (getTextWidth(prFont, StrCadena) / 2));
                        ImprimirTexto(StrCadena); //OCG|27042022
                    }
                    break;
                case "IMPRIMECONLATERAL":
                    if (CantidadExpresionesAParsear == 2)
                    {
                        StrCadena = ExpresionesParseadas[0];
                        //currentXY.X = (Int32)((AnchoImpresoraHere / 2) - (getTextWidth(prFont, StrCadena) / 2));
                        ImprimirTextoConLateral(StrCadena, Convert.ToInt32(ExpresionesParseadas[1])); //OCG|27042022
                    }
                    break;
                case "IMPRIMECODIGODEBARRAS":
                    if (CantidadExpresionesAParsear == 1)
                    {
                        StrCadena = "*" + ExpresionesParseadas[0] + "*";
                        TipoFuente.NombreFuente = "C39HrP24DhTt";
                        TipoFuente.Tamaño = 34;
                        TipoFuente.Regular = true;
                        CambiarEstiloFuente();
                        currentXY.X = (Int32)((AnchoImpresoraHere / 2) - (getTextWidth(prFont, StrCadena) / 2));
                        ImprimirTexto(StrCadena);
                    }
                    break;
                case "IMPRIMIRENANGULO"://OCG|27042022 Imprime vertical
                    if (CantidadExpresionesAParsear == 4)
                    {
                        StrCadena = ExpresionesParseadas[0];

                        // Parametros: Texto, Angulo, x, y
                        ImprimirEnAngulo(StrCadena, Convert.ToInt32(ExpresionesParseadas[1]), Convert.ToInt32(ExpresionesParseadas[2]), Convert.ToInt32(ExpresionesParseadas[3]));

                    }
                    break;
                case "IMPRIMIRIMAGEN"://OCG|27042022 Imprime vertical
                    if (CantidadExpresionesAParsear == 6)
                    {
                        ImprimirImagen(ExpresionesParseadas[0], Convert.ToInt32(ExpresionesParseadas[1]), Convert.ToInt32(ExpresionesParseadas[2]), ExpresionesParseadas[3], Convert.ToInt32(ExpresionesParseadas[4]), Convert.ToInt32(ExpresionesParseadas[5]));
                    }
                    break;
                case "IMPRIMIRVOUCHERSANTANDER":
                    StrCadena = ExpresionesParseadas[0];
                    i = StrCadena.IndexOf("@");
                    break;
                case "IMPRIMENUMEROATEXTO":
                    if (CantidadExpresionesAParsear == 1)
                    {
                        StrCadena = ExpresionesParseadas[0];
                        if (double.TryParse(StrCadena, out outDouble))
                        {
                            double dAnchoTexto;
                            double dPosXHere;
                            string dTextoHere;

                            StrCadena = Letras(outDouble);

                            dPosXHere = currentXY.X;
                            dTextoHere = StrCadena;
                            dAnchoTexto = getTextWidth(prFont, dTextoHere);
                            while (dAnchoTexto > AnchoImpresoraHere - currentXY.X)
                            {
                                for (i = dTextoHere.Length - 1; i >= 3; i--)
                                {
                                    if (getTextWidth(prFont, dTextoHere.Substring(0, i)) <= AnchoImpresoraHere - dPosXHere - getTextWidth(prFont, "MM"))
                                    {
                                        ImprimirTexto(dTextoHere.Substring(0, i));
                                        dTextoHere = dTextoHere.Substring(i);
                                        dAnchoTexto = getTextWidth(prFont, dTextoHere);
                                        currentXY.X = (Int32)dPosXHere;
                                        break;
                                    }
                                }
                            }
                            ImprimirTexto(dTextoHere);
                        }
                    }
                    break;
                case "IMPRIMEIMAGEN":
                    if (CantidadExpresionesAParsear == 3)
                    {
                        Image img = Image.FromFile(ExpresionesParseadas[0]);
                        Printer.Graphics.DrawImage(img, (Single)(AnchoImpresoraHere / 2 - Int32.Parse(ExpresionesParseadas[1]) / 2), currentXY.Y, Int32.Parse(ExpresionesParseadas[1]), Int32.Parse(ExpresionesParseadas[2]));
                        //Calculate the location of the next image.
                        currentXY.Y += Int32.Parse(ExpresionesParseadas[2]) + 1;
                    }
                    break;
                case "SUBCADENA":
                    if (CantidadExpresionesAParsear == 4)     //'Validamos parámetros    (Variable Destino, Cadena Original, Posicion Inicial, Posición final                        
                    {
                        if (ExpresionesAParsear[0].ToUpper().Contains("CADENA")) //  'Validamos  que exista "CADENA"
                        {
                            if (ExpresionesAParsear[0].Length <= 8)    //Validamos la variable destino
                            {
                                if (Int32.TryParse(ExpresionesParseadas[2], out outInt) && Int32.TryParse(ExpresionesParseadas[3], out outInt2))
                                {
                                    LongHere = ExpresionesParseadas[1].Length;
                                    if (LongHere > outInt && outInt <= outInt2 && LongHere != 0)
                                        EsSetVariable(ExpresionesAParsear[0] + " = \"" + ExpresionesParseadas[1].Substring(outInt, (outInt2 > LongHere) ? LongHere - 1 : outInt2 - 1) + "\"");
                                    else
                                        EsSetVariable(ExpresionesAParsear[0] + " = \"");
                                }
                            }

                        }
                    }
                    break;
                case "CONCATENAR":   //Cadena destino, Cadena, Tamaño, Alineación 0 = Izq, 1 = Der
                    if ((CantidadExpresionesAParsear - 1) % 3 == 0 && CantidadExpresionesAParsear > 1)
                    {
                        LongHere = (CantidadExpresionesAParsear - 1) / 3;
                        StrCadena = "";
                        for (i = 1; i <= LongHere; i++)
                        {
                            if (!Int32.TryParse(ExpresionesParseadas[((i - 1) * 3) + 2], out outInt))
                            {
                                return "Al procesar la cadena " + i + " de la función CONCATENAR se recibió un tamaño no numérico";
                            }
                            if (!Int32.TryParse(ExpresionesParseadas[((i - 1) * 3) + 3], out outInt2))
                            {
                                return "Al procesar la cadena " + i + " de la función CONCATENAR se recibió una alineación no numérica";
                            }

                            StrCadena = StrCadena + FuncionMediaConcatenar(ExpresionesParseadas[((i - 1) * 3) + 1], outInt, outInt2);
                        }
                    }
                    else
                    {
                        if (CantidadExpresionesAParsear == 0)
                            return "CONCATENAR llegó con 1 parámetro";
                        else
                            return "CONCATENAR llegó con un número de parámetros no divisible entre 3, llegó con " + (CantidadExpresionesAParsear - 1).ToString();

                    }
                    EsSetVariable(ExpresionesAParsear[0] + " = \"\"" + StrCadena + "\"\"");
                    //'On Error Resume Next
                    ImprimirTexto(StrCadena);
                    //'Printer.Print StrCadena
                    //'Printer.CurrentX = MargenX
                    //'On Error GoTo 0
                    break;
                case "AVANZARNCARACTERES":
                    if (Int32.TryParse(ExpresionesParseadas[0], out LongHere))
                        if (LongHere != 0)
                        {
                            currentXY.X += (Int32)(getTextWidth(prFont, " ") * LongHere);
                        }
                    break;
                case "RELLENARNCONX":
                    if (CantidadExpresionesAParsear == 3)
                    {
                        if (Int32.TryParse(ExpresionesAParsear[0].ToUpper().Replace("CADENA", ""), out outInt))
                        {
                            if (Int32.TryParse(ExpresionesParseadas[1], out outInt))
                            {
                                if (ExpresionesParseadas[2].Length == 1)
                                {
                                    string sCompleto = new String(ExpresionesParseadas[2].ToCharArray()[0], Int32.Parse(ExpresionesParseadas[1]));
                                    if (!EsSetVariable(ExpresionesAParsear[0] + " = \"" + sCompleto + "\""))
                                        RealizarAccion("No se pudo aplicar la función RELLENARCONX");
                                }
                            }
                        }
                    }
                    break;
                case "AVANZARNLINEAS":
                    if (Int32.TryParse(ExpresionesParseadas[0], out LongHere))
                    {
                        if (LongHere != 0)
                        {
                            if (LongHere > 0)
                                for (i = 1; i <= LongHere; i++)
                                    ImprimirTexto(" ");
                            else
                            {
                                LongHere = LongHere * (-1);
                                for (i = 1; i <= LongHere; i++)
                                    currentXY.Y -= (Int32)getTextHeight(prFont, "0");
                            }
                        }
                    }
                    break;
                case "ABRIRCAJON":
                    //'AbrirCajon(HandleLocal)                    
                    break;
                case "NUEVAHOJA":
                    Printer.HasMorePages = true;
                    //currentXY.X = 0;
                    //currentXY.Y = 0;
                    break;
                case "PONFUENTE":
                    TipoFuente.NombreFuente = ExpresionesParseadas[0];
                    TipoFuente.Tamaño = Single.Parse(ExpresionesParseadas[1]);
                    CambiarEstiloFuente();
                    break;
                case "PONMARGENX":
                    MargenX = Int32.Parse(ExpresionesParseadas[0]);
                    break;
                case "PONMARGENY":
                    MargenY = Int32.Parse(ExpresionesParseadas[0]);
                    break;
                case "PONNEGRITA":
                    TipoFuente.Bold = true;
                    CambiarEstiloFuente();
                    break;
                case "QUITANEGRITA":
                    TipoFuente.Bold = false;
                    CambiarEstiloFuente();
                    break;
                case "PONCURSIVA":
                    TipoFuente.Italic = true;
                    CambiarEstiloFuente();
                    break;
                case "QUITACURSIVA":
                    TipoFuente.Italic = false;
                    CambiarEstiloFuente();
                    break;
                case "PONSUBRAYADO":
                    TipoFuente.Underline = true;
                    CambiarEstiloFuente();
                    break;
                case "QUITASUBRAYADO":
                    TipoFuente.Underline = false;
                    CambiarEstiloFuente();
                    break;
                case "PONTACHADO":
                    TipoFuente.StrikeOut = true;
                    CambiarEstiloFuente();
                    break;
                case "QUITATACHADO":
                    TipoFuente.StrikeOut = false;
                    CambiarEstiloFuente();
                    break;
                case "PONNORMAL":
                    TipoFuente.StrikeOut = false;
                    TipoFuente.Underline = false;
                    TipoFuente.Italic = false;
                    TipoFuente.Bold = false;
                    TipoFuente.Regular = true;

                    CambiarEstiloFuente();
                    break;
                case "PONTAMAÑO":
                    TipoFuente.Tamaño = Single.Parse(ExpresionesParseadas[0]);
                    CambiarEstiloFuente();
                    break;
                case "MUEVECURSORX":
                    currentXY.X = Int32.Parse(ExpresionesParseadas[0]);
                    break;
                case "MUEVECURSORY":
                    currentXY.Y = Int32.Parse(ExpresionesParseadas[0]);
                    break;
                case "REGRESARDETALLE":
                    partidaActual = 0;
                    break;
                case "REGRESARDESCUENTOSGLOBALES":
                    descuentoActual = 0;
                    break;
                case "REGRESARCUPONES":
                    cuponActual = 0;
                    break;
                case "REGRESARNOTASDECREDITO":
                    notaDeCreditoActual = 0;
                    break;
                case "PONCOLOR":
                    if (CantidadExpresionesAParsear == 3)
                    {
                        if (Int32.TryParse(ExpresionesParseadas[0], out outInt) && Int32.TryParse(ExpresionesParseadas[1], out outInt2) && Int32.TryParse(ExpresionesParseadas[2], out outInt3))
                            //'Printer.ForeColor = RGB(CLng(ExpresionesParseadas(0)), CLng(ExpresionesParseadas(1)), CLng(ExpresionesParseadas(2)))                            
                            i = 0;
                        else
                            return "Uno de los elementos de PONCOLOR no es numérico, Verifica; Expresion: " + ExpresionesParseadas[0] + " " + ExpresionesParseadas[1] + " " + ExpresionesParseadas[2];
                    }
                    else
                        return "La función PONCOLOR lleva 3 elementos y se recibieron " + CantidadExpresionesAParsear;
                    break;
                case "CARACTERESEXPRESION":
                    if (CantidadExpresionesAParsear == 2)
                        if (!EsSetVariable(ExpresionesAParsear[0] + " = " + ExpresionesParseadas[1].Length))
                            return "Error al establecer la variable en expresión " + ExpresionesParseadas[0] + " " + ExpresionesParseadas[1];
                        else
                            return "La función CARACTERESEXPRESION toma 2 parámetros y se recibieron " + CantidadExpresionesAParsear + " en expresión " + ExpresionesParseadas[0] + " " + ExpresionesParseadas[1];
                    break;
                case "ANCHOEXPRESION":
                    if (CantidadExpresionesAParsear == 2)
                        if (!EsSetVariable(ExpresionesAParsear[0] + " = " + getTextWidth(prFont, ExpresionesParseadas[1]).ToString()))
                            return "Error al establecer la variable en expresión " + Expresion;
                        else
                            return "La función ANCHOEXPRESION toma 2 parámetros y se recibieron " + CantidadExpresionesAParsear + " en expresión " + Expresion;
                    break;
                case "ALTOEXPRESION":
                    if (CantidadExpresionesAParsear == 2)
                    {
                        if (!EsSetVariable(ExpresionesAParsear[0] + " = " + getTextHeight(prFont, ExpresionesParseadas[1])))
                            return "Error al establecer la variable en expresión " + Expresion;
                    }
                    else
                        return "La función ALTOEXPRESION toma 2 parámetros y se recibieron " + CantidadExpresionesAParsear + " en expresión " + Expresion;
                    break;
                case "ANCHOIMPRESORA":
                    if (CantidadExpresionesAParsear == 1)
                    {
                        if (EsSetVariable(ExpresionesAParsear[0] + " = " + Printer.PageSettings.PaperSize.Width))
                            return "Error al establecer la variable en expresión " + Expresion;
                    }
                    else
                        return "La función ANCHOIMPRESORA toma 1 parámetros y se recibieron " + CantidadExpresionesAParsear + " en expresión " + Expresion;
                    break;
                case "DARFORMATO":
                    if (CantidadExpresionesAParsear == 3)
                    {
                        double dParse;
                        if (Double.TryParse(ExpresionesParseadas[1], out dParse))
                            StrCadena = dParse.ToString(ExpresionesParseadas[2]);
                        else
                            StrCadena = String.Format(ExpresionesParseadas[1], ExpresionesParseadas[2]);

                        if (!EsSetVariable(ExpresionesAParsear[0] + " = \"" + StrCadena + "\""))
                            return "Error al imprimir; Error al aplicar el formato en la expresión " + Expresion;
                    }
                    else
                        return "Error al imprimir; La función DARFORMATO toma 3 variables y se recibieron " + CantidadExpresionesAParsear + "; Expresión: " + Expresion;
                    break;
            }
            return "";
        }

        private string FuncionMediaConcatenar(string Cadena, int Tamaño, int Alineacion)
        {
            if (Cadena.Length > Tamaño)
                Cadena = Cadena.Substring(0, Tamaño);

            if (Alineacion == 0)  //Alineación a la izquierda
                if (Cadena.Length < Tamaño)
                    Cadena = Cadena + new String(' ', Tamaño - Cadena.Length);
                else                 //Alineación a la derecha
                if (Cadena.Length < Tamaño)
                    Cadena = new String(' ', Tamaño - Cadena.Length) + Cadena;

            return Cadena;
        }

        private bool EvaluaExpresionSI(string Expresion)
        {
            int PosSigno;
            int i;
            int longitudSigno;
            string StrString;
            string SignosIgualdad;
            string Expresion1, Expresion2, Expresion1Result, Expresion2Result;
            double outDouble = 0, outDouble2 = 0;
            bool bNumerico1y2;

            StrString = Expresion.Substring(3).Trim(); //Le quitamos el si inicial y su primer espacio

            longitudSigno = 3;
            PosSigno = StrString.IndexOf("!IN");
            if (PosSigno == -1)
            {
                longitudSigno = 2;
                PosSigno = StrString.IndexOf("!=");
                if (PosSigno == -1)
                {
                    PosSigno = StrString.IndexOf(">=");
                    if (PosSigno == -1)
                    {
                        PosSigno = StrString.IndexOf("<=");
                        if (PosSigno == -1)
                        {
                            longitudSigno = 1;
                            PosSigno = StrString.IndexOf("=");
                            if (PosSigno == -1)
                            {
                                PosSigno = StrString.IndexOf(">");
                                if (PosSigno == -1)
                                {
                                    PosSigno = StrString.IndexOf("IN");
                                    if (PosSigno == -1)
                                    {
                                        PosSigno = StrString.IndexOf("<");
                                        if (PosSigno == -1)
                                        {
                                            //MsgBox("Error, se intenta evaluar una expresión SI, FIN SI sin con un evaluador incorrecto, parámetros recibidos: " & SignosIgualdad & "; Los posibles evaluadores de igualdad son Mayor o igual Que (>=); Menor o igual Que (<=); Menor que (<); Mayor que (>); Igualdad (=) y diferente (!=); Verifique")
                                            return false;
                                        }
                                        else
                                            SignosIgualdad = "<";
                                    }
                                    else
                                    {
                                        longitudSigno = 2;
                                        SignosIgualdad = "IN";
                                    }
                                }
                                else
                                    SignosIgualdad = ">";
                            }
                            else
                                SignosIgualdad = "=";
                        }
                        else
                            SignosIgualdad = "<=";
                    }
                    else
                        SignosIgualdad = ">=";
                }
                else
                    SignosIgualdad = "!=";
            }
            else
                SignosIgualdad = "!IN";


            Expresion1 = StrString.Substring(0, PosSigno - 1).Trim();
            Expresion2 = StrString.Substring(PosSigno + longitudSigno).Trim();

            ExpresionesAParsear[0] = Expresion1;
            ExpresionesAParsear[1] = Expresion2;
            if (!GetCadenaAImprimir(0))
            {
                //MsgBox("Error al imprimir; No es una expresión válida al evaluar " + Expresion);
                return false;
            }
            if (!GetCadenaAImprimir(1))
            {
                //MsgBox("Error al imprimir; No es una expresión válida al evaluar " + Expresion);
                return false;
            }
            Expresion1Result = ExpresionesParseadas[0];
            Expresion2Result = ExpresionesParseadas[1];

            if (SignosIgualdad == "IN" || SignosIgualdad == "!IN")
            {
                string[] valorSplit = Expresion2.Split(',');

                for (i = 0; i < valorSplit.Length; i++)
                {
                    if (Double.TryParse(Expresion1Result, out outDouble) && Double.TryParse(valorSplit[i], out outDouble2))
                        bNumerico1y2 = true;
                    else
                        bNumerico1y2 = false;

                    if (bNumerico1y2)
                    {
                        if (outDouble == outDouble2)
                            if (SignosIgualdad == "IN")
                                return true;
                            else
                                return false;
                    }
                    else
                    {
                        if (Expresion1Result == valorSplit[i])
                            if (SignosIgualdad == "IN")
                                return true;
                            else
                                return false;
                    }
                }
                if (SignosIgualdad == "IN")
                    return false;
                else
                    return true;
            }
            else
            {
                if (Double.TryParse(Expresion1Result, out outDouble) && Double.TryParse(Expresion2Result, out outDouble2))
                    bNumerico1y2 = true;
                else
                    bNumerico1y2 = false;

                switch (SignosIgualdad)
                {
                    case "=":
                        if (bNumerico1y2)
                        {
                            if (outDouble == outDouble2)
                                return true;
                            return false;
                        }
                        else
                        {
                            if (Expresion1Result == Expresion2Result)
                                return true;
                            else
                                return false;
                        }
                    case "!=":
                        if (bNumerico1y2)
                        {
                            if (outDouble != outDouble2)
                                return true;
                            return false;
                        }
                        else
                        {
                            if (Expresion1Result != Expresion2Result)
                                return true;
                            else
                                return false;
                        }
                    case ">=":
                        if (bNumerico1y2)
                        {
                            if (outDouble >= outDouble2)
                                return true;
                            return false;
                        }
                        else
                            return false;
                    case "<=":
                        if (bNumerico1y2)
                        {
                            if (outDouble <= outDouble2)
                                return true;
                            else
                                return false;
                        }
                        else
                            return false;
                    case ">":
                        if (bNumerico1y2)
                        {
                            if (outDouble > outDouble2)
                                return true;
                            else
                                return false;
                        }
                        else
                            return false;
                    case "<":
                        if (bNumerico1y2)
                        {
                            if (outDouble < outDouble2)
                                return true;
                            else
                                return false;
                        }
                        else
                            return false;
                    default:
                        throw new Exception("Error, se intenta evaluar una expresión SI, FIN SI sin con un evaluador incorrecto, parámetros recibidos: " + SignosIgualdad + "; Los posibles evaluadores de igualdad son Mayor o igual Que (>=); Menor o igual Que (<=); Menor que (<); Mayor que (>); Igualdad (=) y diferente (!=); Verifique");
                }
            }
        }

        private bool EsSetVariable(string Cadena)
        {
            string VARIABLE;
            int i, j, k;
            string Expresion;
            string StrString;
            int IndiceVariable;
            int outInt, outInt2;
            bool bExpresion0;

            StrString = Cadena.ToUpper();

            if (StrString.Length > 3)
                if (StrString.Substring(0, 3) == "INI" || StrString.Substring(0, 3) == "FIN")
                    return false;

            i = StrString.IndexOf("=");

            if (i == -1)
                return false;

            j = StrString.IndexOf("\"");

            if (j != -1 && j < i)
                //Significa que el igual que encontramos está dentro de una expresión de texto por lo que es inválido
                return false;

            //Sacamos la variable a establecer, a la izquierda del igual
            VARIABLE = StrString.Substring(0, i - 1).Trim();
            Expresion = StrString.Substring(i + 1).Trim();

            ExpresionesAParsear[0] = Expresion;
            if (!GetCadenaAImprimir(0))
            {
                return false;
            }
            Expresion = ExpresionesParseadas[0];

            bExpresion0 = Int32.TryParse(Expresion, out outInt);

            if (bExpresion0)
            {
                if (VARIABLE == "POSICIONX")
                    currentXY.X = outInt;

                if (VARIABLE == "POSICIONY")
                    currentXY.Y = outInt;

                if (VARIABLE == "ANCHOIMPRESORA")
                    AnchoImpresoraHere = outInt;
            }

            if (VARIABLE.IndexOf("ENTERO") == 0)
            {
                StrString = VARIABLE.Replace("ENTERO", "");
                if (Int32.TryParse(StrString, out outInt2))
                {
                    if (outInt2 <= 50)
                    {
                        IndiceVariable = outInt2;
                        if (bExpresion0)
                            Enteros[IndiceVariable] = outInt;
                        else
                            //MsgBox("Intentando establecer entero " & IndiceVariable & " con valor no numérico """ & Expresion & """")
                            Enteros[IndiceVariable] = 0;
                    }
                }
                return true;
            }

            if (VARIABLE.IndexOf("DOBLE") == 0)
            {
                StrString = VARIABLE.Replace("DOBLE", "");
                if (Int32.TryParse(StrString, out outInt2))
                {
                    if (outInt2 <= 50)
                    {
                        IndiceVariable = outInt2;
                        if (!Double.TryParse(Expresion, out Dobles[IndiceVariable]))
                            //MsgBox("Intentando establecer doble " & IndiceVariable & " con valor no numérico """ & Expresion & """")
                            Dobles[IndiceVariable] = 0;

                    }
                }
                return true;
            }

            if (VARIABLE.IndexOf("CADENA") == 0)
            {
                StrString = VARIABLE.Replace("CADENA", "");
                if (Int32.TryParse(StrString, out outInt2))
                {
                    if (outInt2 <= 50)
                    {
                        IndiceVariable = outInt2;
                        Strings[IndiceVariable] = Expresion;
                    }
                }
                return true;
            }
            return false;
        }

        //Aquí me pueden llegar las expresiones concatenadas por el caracter '&', por lo que tengo
        //que separarla en múltiples cadenas y evaluarlas individualmente
        private bool GetCadenaAImprimir(int Indice)
        {
            string StrString;
            int TotalExpresiones;
            int i;

            StrString = ExpresionesAParsear[Indice] + "&";
            TotalExpresiones = GetSeparadores(StrString, "&");
            if (TotalExpresiones > 1)
                SepararLinea(StrString, TotalExpresiones, "&");
            else
                Cadenas[0] = StrString.Substring(0, StrString.Length - 1);

            for (i = 0; i <= TotalExpresiones - 1; i++)
            {
                Cadenas[i] = Cadenas[i].Trim();
                if (Cadenas[i] == "")
                    return false;
            }

            ExpresionesParseadas[Indice] = "";
            for (i = 0; i <= TotalExpresiones - 1; i++)
            {
                ExpresionesParseadas[Indice] += GetCadenaAImprimirSingle(Cadenas[i]);
            }
            return true;
        }

        //Esta es la verdadera función que evaluará las expresiones individuales
        private string GetCadenaAImprimirSingle(string Expresion)
        {
            string StrString;
            //Dim Resultado As Double

            StrString = Expresion.Trim();
            //'Aquí evaluamos que la expresión a impimir sea una cadena (delimitada siempre por "" ya que si es asi, se utiliza como está pero sin las comillas)
            if (StrString.Substring(0, 1) == "\"" && StrString.Substring(StrString.Length - 1) == "\"")
            {
                StrString = StrString.Substring(1, StrString.Length - 2);

                return StrString.Replace("\"\"", "\"");
            }
            else
            {
                //Primero verificamos que la expresión dada no sea una variable a reemplazar solo la cadena
                //si este es el caso, solo reemplazamos la variable por la cadena y listo, si no, evaluamos la expresión
                StrString = GetVariablesTexto(StrString.ToUpper()).Trim();
                if (StrString != "")
                    return StrString;
                else
                {
                    //Si no.... entonces por fin..... si son puras variables numéricas, reemplazamos las variables
                    //numéricas y evaluamos la expresión
                    StrString = Expresion.ToUpper().Trim();
                    StrString = ReemplazaVariablesEnExpresion(StrString);
                    StrString = Parsear(StrString);
                    //If StrString = "" Or StrString = "Error" Then
                    //    Return ""
                    //End If
                    return StrString;
                }
            }
        }

        private string GetVariablesTexto(string Expresion)
        {
            int IndiceVariable;
            string Tipo = "";
            bool bEncontrado = false;
            string sExpresionAReemplazar = "";
            int outInt;

            foreach (DataRow row in dtVariables.Rows)
            {
                //If InStr(UCase(Expresion), UCase(row("Nombre").ToString())) > 0 Then
                if (Expresion.ToUpper() == row["Nombre"].ToString().ToUpper())
                {
                    Tipo = row["Tipo"].ToString().ToUpper();
                    sExpresionAReemplazar = row["Nombre"].ToString().ToUpper();
                    bEncontrado = true;
                    break;
                }
            }

            if (bEncontrado)
            {
                switch (Tipo)
                {
                    case "CABECERO":
                        return dtCabecero.Rows[0][sExpresionAReemplazar].ToString();
                    case "DETALLE":
                        if (partidaActual >= dtDetalle.Rows.Count)
                            partidaActual = 0;
                        return dtDetalle.Rows[partidaActual][sExpresionAReemplazar].ToString();
                    case "DESCUENTOGLOBAL":
                        if (descuentoActual >= dtDescuentosGlobales.Rows.Count)
                            descuentoActual = 0;
                        return dtDescuentosGlobales.Rows[descuentoActual][sExpresionAReemplazar].ToString();
                    case "CUPONES":
                        if (cuponActual >= dtCuponesGenerados.Rows.Count)
                            cuponActual = 0;
                        return dtCuponesGenerados.Rows[cuponActual][sExpresionAReemplazar].ToString();
                    case "NOTASDECREDITO":
                        if (notaDeCreditoActual >= dtNotasDeCreditoGeneradas.Rows.Count)
                            notaDeCreditoActual = 0;
                        return dtNotasDeCreditoGeneradas.Rows[notaDeCreditoActual][sExpresionAReemplazar].ToString();
                    case "SUCURSAL":
                        return dtInfoSucursal.Rows[0][sExpresionAReemplazar].ToString();
                    case "MARCA":
                        return dtInfoEmpresa.Rows[0][sExpresionAReemplazar].ToString();
                    case "FORMAPAGO":
                        return dtFormasPago.Rows[0][sExpresionAReemplazar].ToString();
                    case "FINLAG":
                        return dtFinLag.Rows[0][sExpresionAReemplazar].ToString();
                }
            }
            else
            {
                if (Expresion.IndexOf("ENTERO") == 0)
                {
                    Expresion = Expresion.Replace("ENTERO", "");
                    if (Int32.TryParse(Expresion, out outInt))
                    {
                        if (outInt <= 50)
                        {
                            IndiceVariable = outInt;
                            return Enteros[IndiceVariable].ToString();
                        }
                    }
                }

                if (Expresion.IndexOf("DOBLE") == 0)
                {
                    Expresion = Expresion.Replace("DOBLE", "");
                    if (Int32.TryParse(Expresion, out outInt))
                    {
                        if (outInt <= 50)
                        {
                            IndiceVariable = outInt;
                            return Dobles[IndiceVariable].ToString();
                        }
                    }
                }

                if (Expresion.IndexOf("CADENA") == 0)
                {
                    Expresion = Expresion.Replace("CADENA", "");
                    if (Int32.TryParse(Expresion, out outInt))
                    {
                        if (outInt <= 50)
                        {
                            IndiceVariable = outInt;
                            return Strings[IndiceVariable];
                        }
                    }
                }
            }
            return "";
        }

        //Función que expresiones totalmente matemáticas, reemplazará las variables por su valor correspondiente
        private string ReemplazaVariablesEnExpresion(string Expresion)
        {
            int i;
            string Tipo = "";
            string sExpresionAReemplazar = "";
            string sExpresionMayuscula = Expresion.ToUpper();
            double outDouble;

            //Si lo que me están pasando es un número fijo entonces lo regreso tal cual.
            if (Double.TryParse(Expresion, out outDouble))
                return Expresion;

            foreach (DataRow row in dtVariables.Rows)
            {

                if (sExpresionMayuscula.Contains(row["Nombre"].ToString()))
                {
                    Tipo = row["Tipo"].ToString().ToUpper();
                    sExpresionAReemplazar = row["Nombre"].ToString().ToUpper();
                    switch (Tipo)
                    {
                        case "CABECERO":
                            Expresion = Expresion.Replace(sExpresionAReemplazar, dtCabecero.Rows[0][sExpresionAReemplazar].ToString());
                            break;
                        case "DETALLE":
                            Expresion = Expresion.Replace(sExpresionAReemplazar, dtDetalle.Rows[partidaActual][sExpresionAReemplazar].ToString());
                            break;
                        case "DESCUENTOGLOBAL":
                            Expresion = Expresion.Replace(sExpresionAReemplazar, dtDescuentosGlobales.Rows[descuentoActual][sExpresionAReemplazar].ToString());
                            break;
                        case "CUPONES":
                            Expresion = Expresion.Replace(sExpresionAReemplazar, dtCuponesGenerados.Rows[cuponActual][sExpresionAReemplazar].ToString());
                            break;
                        case "NOTASDECREDITO":
                            Expresion = Expresion.Replace(sExpresionAReemplazar, dtNotasDeCreditoGeneradas.Rows[notaDeCreditoActual][sExpresionAReemplazar].ToString());
                            break;
                        case "SUCURSAL":
                            Expresion = Expresion.Replace(sExpresionAReemplazar, dtInfoSucursal.Rows[0][sExpresionAReemplazar].ToString());
                            break;
                        case "FORMAPAGO":
                            Expresion = Expresion.Replace(sExpresionAReemplazar, dtFormasPago.Rows[0][sExpresionAReemplazar].ToString());
                            break;
                        case "MARCA":
                            Expresion = Expresion.Replace(sExpresionAReemplazar, dtInfoEmpresa.Rows[0][sExpresionAReemplazar].ToString());
                            break;
                        case "FINLAG": // v.2.8 CGC Integración de ticketsPv
                            Expresion = Expresion.Replace(sExpresionAReemplazar, dtFinLag.Rows[cuponActual][sExpresionAReemplazar].ToString());
                            break;
                    }
                }
            }

            //Variables de la impresora
            Expresion = Expresion.Replace("ANCHOIMPRESORA", AnchoImpresoraHere.ToString());
            Expresion = Expresion.Replace("MARGENX", MargenX.ToString());
            Expresion = Expresion.Replace("MARGENY", MargenY.ToString());
            Expresion = Expresion.Replace("POSICIONX", currentXY.X.ToString()); // 'Printer.CurrentX);
            Expresion = Expresion.Replace("POSICIONY", currentXY.Y.ToString()); // 'Printer.CurrentY)
            Expresion = Expresion.Replace("NUMERODEPARTIDAS", TotalPartidas.ToString());
            Expresion = Expresion.Replace("NUMERODEDESCUENTOSGLOBALES", TotalDescuentosGlobales.ToString());
            Expresion = Expresion.Replace("NUMERODECUPONES", TotalCuponesGenerados.ToString());
            Expresion = Expresion.Replace("NUMERODENOTASDECREDITO", TotalNotasDeCreditoGeneradas.ToString());
            Expresion = Expresion.Replace("NUMERODEPIEZAS", CantidadPiezas.ToString());

            //Empezamos desde el 50, para no comernos por ejemplo ENTERO1 reemplaza ENTERO10 y sería fatal
            for (i = 49; i >= 0; i--)
            {
                Expresion = Expresion.Replace("ENTERO" + i, Enteros[i].ToString());
                Expresion = Expresion.Replace("DOBLE" + i, Dobles[i].ToString());
                Expresion = Expresion.Replace("CADENA" + i, Strings[i]);
            }
            return Expresion;
        }

        //Obtiene la función de la expresión, esto es, busca la primera ocurrencia de un espacio en la expresión y esa la
        //compara con una base de datos de funciones, si no encuentra una función válida regresa "" que será error
        private string GetFuncionParser(string Expresion)
        {
            int i;
            string sFuncion;
            Expresion = Expresion.Trim();

            i = Expresion.IndexOf(" ");
            if (i == -1)
                return "";

            sFuncion = Expresion.Substring(0, i).ToUpper();

            for (i = 0; i <= TotalFunciones - 1; i++)
            {
                if (sFuncion == FuncionesArray[i])
                    return sFuncion;
            }
            return "";
        }

        private bool GetVariables(string Expresion)
        {
            //Restablezco mi variable global del número de Variables a parsear
            CantidadExpresionesAParsear = 0;

            string Letra, LetraSiguiente, ExpresionActual = "", StrString, Variables = "";
            bool EstoyEnComilla = false;
            int i = 0;

            //Obtiene si la primera palabra es una función
            FuncionParseada = GetFuncionParser(Expresion);
            if (FuncionParseada == "")
                return false;

            //Obtengo las variables, es decir, le quito la función
            Variables = Expresion.Substring(FuncionParseada.Length + 1);

            StrString = Variables;
            do
            {
                Letra = GetFirstFromString(StrString);
                StrString = RemoveFirstFromString(StrString);
                LetraSiguiente = GetFirstFromString(StrString);
                switch (Letra)
                {
                    case "\"":
                        ExpresionActual += "\""; //Es necesario dejarla como comillas para que el parser no la interprte como una variable o datos numéricos
                        if (!EstoyEnComilla)
                            EstoyEnComilla = true;
                        else    //Estoy dentro de una expresión de comilla
                        {
                            if (LetraSiguiente == "\"")
                                StrString = RemoveFirstFromString(StrString);
                            else
                            {
                                EstoyEnComilla = false;
                                ExpresionActual = ExpresionActual + " ";
                            }
                        }
                        break;
                    case ",":
                        if (EstoyEnComilla)
                            ExpresionActual = ExpresionActual + ",";
                        else
                        {
                            if (ExpresionActual == "")
                            {
                                //MsgBox("Error al imprimir; Error al parsear la expresion " & Expresion & "; Encontrada una coma (,) antes de finalizar parseo de expresión")
                                CantidadExpresionesAParsear = 0;
                                return false;
                            }

                            ExpresionesAParsear[CantidadExpresionesAParsear] = ExpresionActual.Trim();
                            CantidadExpresionesAParsear++;
                            ExpresionActual = "";
                        }
                        break;
                    case " ":
                        if (EstoyEnComilla)
                            ExpresionActual = ExpresionActual + " ";
                        break;
                    case "&":
                        if (EstoyEnComilla)
                            ExpresionActual = ExpresionActual + "&";
                        else
                            ExpresionActual = ExpresionActual + "& ";
                        break;
                    case "\t":
                        if (EstoyEnComilla)
                            ExpresionActual = ExpresionActual + "\t";
                        break;
                    default:
                        ExpresionActual = ExpresionActual + Letra;
                        break;
                }
            } while (Letra != "" && LetraSiguiente != "");

            if (EstoyEnComilla)
            {
                //MsgBox("No se cerró la expresión; Verifique expresión " & Expresion)                
                CantidadExpresionesAParsear = 0;
                return false;
            }

            ExpresionActual = ExpresionActual.Trim();

            if (ExpresionActual != "")
            {
                ExpresionesAParsear[CantidadExpresionesAParsear] = ExpresionActual;
                CantidadExpresionesAParsear++;
            }
            return true;
        }

        //Función que parsea la función dada
        private string Parsear(string FuncionDada)
        {
            DataTable dt;
            string dRetorno;
            dt = new DataTable();
            try
            {
                dRetorno = dt.Compute(FuncionDada, "").ToString();
            }
            catch
            {
                return "ERROR ERROR";
            }

            return dRetorno;
        }

        private string GetFirstFromString(string Cadena)
        {
            if (Cadena.Length >= 1)
                return Cadena.Substring(0, 1);
            else
                return "";
        }

        private string RemoveFirstFromString(string Cadena)
        {
            if (Cadena == "")
                return "";
            else
                return Cadena.Substring(1);
        }

        private int GetSeparadores(string StrString, string Separador)
        {
            int Ocurrencias, j;
            string StrActual;
            StrActual = StrString;

            Ocurrencias = 0;
            j = StrActual.IndexOf(Separador);
            while (j > 0)
            {
                Ocurrencias++;
                StrActual = StrActual.Substring(j + Separador.Length);
                j = StrActual.IndexOf(Separador);
            }
            return Ocurrencias;
        }

        //Toma cualquier cadena y la separa en pedacitos dependiendo de el separados, todo lo convierte a cadena y lo guarda en un arreglo de cadenas
        private bool SepararLinea(string StrString, int NumeroSeparadores, string Separador)
        {
            int i, j;
            string StrActual;
            StrActual = StrString;

            if (Separador == "")
                Separador = "|";

            for (i = 0; i <= NumeroSeparadores - 1; i++)
                Cadenas[i] = "";

            for (i = 0; i <= NumeroSeparadores - 1; i++)
            {
                j = StrActual.IndexOf(Separador);
                if (j != -1)
                {
                    Cadenas[i] = StrActual.Substring(0, j).Trim();
                    StrActual = StrActual.Substring(j + 1).Trim();
                    if (i == NumeroSeparadores - 1)
                        return true;
                }
                else
                    return false;
            }
            return true;
        }

        private Single getTextHeight(Font prFont, string texto)
        {
            return TextRenderer.MeasureText(texto, prFont).Height;
        }

        private Single getTextWidth(Font prFont, string texto)
        {
            return TextRenderer.MeasureText(texto, prFont).Width;

        }

        //OCG|27042022
        private void ImprimirTexto(string texto)
        {
            //if (espaciar)
            //{
            //    currentXY.X = margen;
            //}

            Printer.Graphics.DrawString(texto, prFont, Brushes.Black, currentXY);
            if (texto == "")
                texto = ".";
            ultimoBox = TextRenderer.MeasureText(texto, prFont);
            currentXY.X = (Int32)MargenX;
            currentXY.Y += ultimoBox.Height;
        }
        //OCG|
        private void ImprimirTextoConLateral(string texto, int margen)
        {

            currentXY.X = margen;

            Printer.Graphics.DrawString(texto, prFont, Brushes.Black, currentXY);
            if (texto == "")
                texto = ".";
            ultimoBox = TextRenderer.MeasureText(texto, prFont);
            currentXY.X = (Int32)MargenX;
            currentXY.Y += ultimoBox.Height;
        }
        //OCG|27042022
        private void ImprimirEnAngulo(string texto, int angulo, int x, int y)
        {

            Printer.Graphics.TranslateTransform(x, y);
            Printer.Graphics.RotateTransform(angulo);

            Printer.Graphics.DrawString(texto, prFont, Brushes.Black, 0, 0);

            Printer.Graphics.RotateTransform(-angulo);
        }


        //OCG|27042022
        private void ImprimirImagen(string ruta, int ancho, int alto, string altText, int x = 0, int y = 0)
        {
            try
            {
                Image img = Image.FromFile(@ruta);

                if ((x == 0 && y == 0))
                {
                    x = currentXY.X;
                    y = ((currentXY.Y / 2) - 65);
                }

                Printer.Graphics.DrawImage(img,
                             new Rectangle(x, y, ancho, alto),
                           0, 0, ancho, alto,
                             GraphicsUnit.Pixel);

            }
            catch (Exception ex)
            {
                Printer.Graphics.DrawString(altText, prFont, Brushes.Black, 0, 104);
            }
        }

        private void CambiarEstiloFuente()
        {
            int EstatusAnterior = 0;


            if (TipoFuente.Bold)
                EstatusAnterior += (Int32)FontStyle.Bold;

            if (TipoFuente.Italic)
                EstatusAnterior += (Int32)FontStyle.Italic;

            if (TipoFuente.Underline)
                EstatusAnterior += (Int32)FontStyle.Underline;

            if (TipoFuente.StrikeOut)
                EstatusAnterior += (Int32)FontStyle.Strikeout;


            if (EstatusAnterior == 0)
                EstatusAnterior = (Int32)FontStyle.Regular;


            try
            {
                prFont = new Font(TipoFuente.NombreFuente, TipoFuente.Tamaño, (System.Drawing.FontStyle)EstatusAnterior);
            }
            catch (Exception)
            {
                try
                {
                    prFont = new Font(TipoFuente.NombreFuente, TipoFuente.Tamaño);
                }
                catch (Exception)
                {
                    prFont = new Font("Courier New", TipoFuente.Tamaño);
                }
            }
        }

        private string Letras(double value)
        {
            string Num2Text = "";
            value = Math.Truncate(value);
            if (value == 0) Num2Text = "CERO";
            else if (value == 1) Num2Text = "UNO";
            else if (value == 2) Num2Text = "DOS";
            else if (value == 3) Num2Text = "TRES";
            else if (value == 4) Num2Text = "CUATRO";
            else if (value == 5) Num2Text = "CINCO";
            else if (value == 6) Num2Text = "SEIS";
            else if (value == 7) Num2Text = "SIETE";
            else if (value == 8) Num2Text = "OCHO";
            else if (value == 9) Num2Text = "NUEVE";
            else if (value == 10) Num2Text = "DIEZ";
            else if (value == 11) Num2Text = "ONCE";
            else if (value == 12) Num2Text = "DOCE";
            else if (value == 13) Num2Text = "TRECE";
            else if (value == 14) Num2Text = "CATORCE";
            else if (value == 15) Num2Text = "QUINCE";
            else if (value < 20) Num2Text = "DIECI" + Letras(value - 10);
            else if (value == 20) Num2Text = "VEINTE";
            else if (value < 30) Num2Text = "VEINTI" + Letras(value - 20);
            else if (value == 30) Num2Text = "TREINTA";
            else if (value == 40) Num2Text = "CUARENTA";
            else if (value == 50) Num2Text = "CINCUENTA";
            else if (value == 60) Num2Text = "SESENTA";
            else if (value == 70) Num2Text = "SETENTA";
            else if (value == 80) Num2Text = "OCHENTA";
            else if (value == 90) Num2Text = "NOVENTA";
            else if (value < 100) Num2Text = Letras(Math.Truncate(value / 10) * 10) + " Y " + Letras(value % 10);
            else if (value == 100) Num2Text = "CIEN";
            else if (value < 200) Num2Text = "CIENTO " + Letras(value - 100);
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = Letras(Math.Truncate(value / 100)) + "CIENTOS";
            else if (value == 500) Num2Text = "QUINIENTOS";
            else if (value == 700) Num2Text = "SETECIENTOS";
            else if (value == 900) Num2Text = "NOVECIENTOS";
            else if (value < 1000) Num2Text = Letras(Math.Truncate(value / 100) * 100) + " " + Letras(value % 100);
            else if (value == 1000) Num2Text = "MIL";
            else if (value < 2000) Num2Text = "MIL " + Letras(value % 1000);
            else if (value < 1000000)
            {
                Num2Text = Letras(Math.Truncate(value / 1000)) + " MIL";
                if ((value % 1000) > 0) Num2Text = Num2Text + " " + Letras(value % 1000);
            }
            else if (value == 1000000) Num2Text = "UN MILLON";
            else if (value < 2000000) Num2Text = "UN MILLON " + Letras(value % 1000000);
            else if (value < 1000000000000)
            {
                Num2Text = Letras(Math.Truncate(value / 1000000)) + " MILLONES ";
                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0)
                    Num2Text = Num2Text + " " + Letras(value - Math.Truncate(value / 1000000) * 1000000);
            }
            else if (value == 1000000000000) Num2Text = "UN BILLON";
            else if (value < 2000000000000) Num2Text = "UN BILLON " + Letras(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            else
            {
                Num2Text = Letras(Math.Truncate(value / 1000000000000)) + " BILLONES";
                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + Letras(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            }
            return Num2Text;
        }



        private void CargarDatosClienteYVenta()
        {
            //Consultamos las ventas
            int i;

            SqlDataAdapter da;

            dtVariables = new DataTable();
            dtCabecero = new DataTable();
            dtDetalle = new DataTable();
            dtFormasPago = new DataTable();
            dtInfoSucursal = new DataTable();
            dtInfoEmpresa = new DataTable();
            dtDescuentosGlobales = new DataTable();
            dtCuponesGenerados = new DataTable();
            dtNotasDeCreditoGeneradas = new DataTable();
            dtVoucherCliente = new DataTable();
            dtFinLag = new DataTable();


            int iTabla;

            try
            {
                //Primero obtenemos todas las variables a reemplazar dentro del ticket.

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                iTabla = 1;
                parameters.Add("@FolioVenta", FolioVenta);
                parameters.Add("@Tipo", iTabla);
                dtVariables = cnxVanti.GetDataTable("SP_ObtieneDatosVenta", parameters);

                if (dtVariables.Rows.Count == 0)
                {
                    //MsgBox("Error, no se obtuvieron datos para imprimir el folio " + FolioVenta)
                    throw new System.Exception("Error, no se obtuvieron datos para imprimir el folio " + FolioVenta);
                }

                iTabla = 2;
                parameters.Remove("@Tipo");
                parameters.Add("@Tipo", iTabla);
                dtCabecero = cnxVanti.GetDataTable("SP_ObtieneDatosVenta", parameters);
                if (dtCabecero.Rows.Count == 0)
                    //MsgBox("Error, no se obtuvieron datos del cabecero para imprimir el folio " & FolioVenta)
                    throw new System.Exception("Error, no se obtuvieron datos del cabecero para imprimir el folio " + FolioVenta);

                if (dtCabecero.Rows[0]["AbrirCajonCabecero"].ToString() == "1")
                    bAbrirCajon = true;
                else
                    bAbrirCajon = false;

                iTabla = 3;
                parameters.Remove("@Tipo");
                parameters.Add("@Tipo", iTabla);
                dtDetalle = cnxVanti.GetDataTable("SP_ObtieneDatosVenta", parameters);
                if (dtDetalle.Rows.Count == 0)
                    //MsgBox("Error, no se obtuvieron datos del detalle para imprimir el folio " & FolioVenta)
                    throw new System.Exception("Error, no se obtuvieron datos del detalle para imprimir el folio " + FolioVenta);


                iTabla = 4;
                TotalFormasPago = 0;
                parameters.Remove("@Tipo");
                parameters.Add("@Tipo", iTabla);
                dtFormasPago = cnxVanti.GetDataTable("SP_ObtieneDatosVenta", parameters);
                if (dtFormasPago.Rows.Count == 0)
                {
                    //MsgBox("Error, no se obtuvieron datos de formas de pago para imprimir el folio " & FolioVenta)

                    TotalFormasPago = 0;
                    //throw new System.Exception("Error, no se obtuvieron datos de formas de pago para imprimir el folio " + FolioVenta);
                }
                else
                {
                    iTabla = 11;
                    //if (dtFormasPago.Rows[0]["importe1FormaPago"].ToString() != "0")
                    //{
                    //    TotalFormasPago += Double.Parse(dtFormasPago.Rows[0]["importe1FormaPago"].ToString());
                    //    //if (dtFormasPago.Rows[0]["VOUCHERPAGO1FORMAPAGO"].ToString() != "0")
                    //    //    bImprimeVoucher = true;
                    //}
                    //if (dtFormasPago.Rows[0]["importe2FormaPago"].ToString() != "0")
                    //{
                    //    TotalFormasPago += Double.Parse(dtFormasPago.Rows[0]["importe2FormaPago"].ToString());
                    //    //if (dtFormasPago.Rows[0]["VOUCHERPAGO2FORMAPAGO"].ToString() != "0")
                    //    //    bImprimeVoucher = true;
                    //}
                    //if (dtFormasPago.Rows[0]["importe3FormaPago"].ToString() != "0")
                    //{
                    //    TotalFormasPago += Double.Parse(dtFormasPago.Rows[0]["importe3FormaPago"].ToString());
                    //    //if (dtFormasPago.Rows[0]["VOUCHERPAGO3FORMAPAGO"].ToString() != "0")
                    //    //    bImprimeVoucher = true;
                    //}
                    //if (dtFormasPago.Rows[0]["importe4FormaPago"].ToString() != "0")
                    //{
                    //    TotalFormasPago += Double.Parse(dtFormasPago.Rows[0]["importe4FormaPago"].ToString());
                    //    //if (dtFormasPago.Rows[0]["VOUCHERPAGO4FORMAPAGO"].ToString() != "0")
                    //    //    bImprimeVoucher = true;
                    //}
                    if (dtFormasPago.Rows[0]["importe1FormaPago"].ToString() != "0")
                    {
                        TotalFormasPago += Double.Parse(dtFormasPago.Rows[0]["importe1FormaPago"].ToString());
                        if (dtFormasPago.Rows[0]["CODIGO1FORMAPAGO"].ToString() == "FL")
                            bTieneFinLag = true;
                    }
                    if (dtFormasPago.Rows[0]["importe2FormaPago"].ToString() != "0")
                    {
                        TotalFormasPago += Double.Parse(dtFormasPago.Rows[0]["importe2FormaPago"].ToString());
                        if (dtFormasPago.Rows[0]["CODIGO2FORMAPAGO"].ToString() == "FL")
                            bTieneFinLag = true;
                    }
                    if (dtFormasPago.Rows[0]["importe3FormaPago"].ToString() != "0")
                    {
                        TotalFormasPago += Double.Parse(dtFormasPago.Rows[0]["importe3FormaPago"].ToString());
                        if (dtFormasPago.Rows[0]["CODIGO3FORMAPAGO"].ToString() == "FL")
                            bTieneFinLag = true;
                    }
                    if (dtFormasPago.Rows[0]["importe4FormaPago"].ToString() != "0")
                    {
                        TotalFormasPago += Double.Parse(dtFormasPago.Rows[0]["importe4FormaPago"].ToString());
                        if (dtFormasPago.Rows[0]["CODIGO4FORMAPAGO"].ToString() == "FL")
                            bTieneFinLag = true;
                    }
                    if (dtFormasPago.Rows[0]["importe5FormaPago"].ToString() != "0")
                    {
                        TotalFormasPago += Double.Parse(dtFormasPago.Rows[0]["importe5FormaPago"].ToString());
                        //if (dtFormasPago.Rows[0]["VOUCHERPAGO5FORMAPAGO"].ToString() != "0")
                        //    bImprimeVoucher = true;
                    }
                    if (dtFormasPago.Rows[0]["importe6FormaPago"].ToString() != "0")
                    {
                        TotalFormasPago += Double.Parse(dtFormasPago.Rows[0]["importe6FormaPago"].ToString());
                        //if (dtFormasPago.Rows[0]["VOUCHERPAGO6FORMAPAGO"].ToString() != "0")
                        //    bImprimeVoucher = true;
                    }
                    if (dtFormasPago.Rows[0]["importe7FormaPago"].ToString() != "0")
                    {
                        TotalFormasPago += Double.Parse(dtFormasPago.Rows[0]["importe7FormaPago"].ToString());
                        //if (dtFormasPago.Rows[0]["VOUCHERPAGO7FORMAPAGO"].ToString() != "0")
                        //    bImprimeVoucher = true;
                    }
                    if (dtFormasPago.Rows[0]["importe8FormaPago"].ToString() != "0")
                    {
                        TotalFormasPago += Double.Parse(dtFormasPago.Rows[0]["importe8FormaPago"].ToString());
                        //if (dtFormasPago.Rows[0]["VOUCHERPAGO8FORMAPAGO"].ToString() != "0")
                        //    bImprimeVoucher = true;
                    }
                }
                iTabla = 5;
                parameters.Remove("@Tipo");
                parameters.Add("@Tipo", iTabla);
                dtInfoSucursal = cnxVanti.GetDataTable("SP_ObtieneDatosVenta", parameters);

                iTabla = 6;
                parameters.Remove("@Tipo");
                parameters.Add("@Tipo", iTabla);
                dtInfoEmpresa = cnxVanti.GetDataTable("SP_ObtieneDatosVenta", parameters);

                iTabla = 7;
                parameters.Remove("@Tipo");
                parameters.Add("@Tipo", iTabla);
                try
                {
                    dtDescuentosGlobales = cnxVanti.GetDataTable("SP_ObtieneDatosVenta", parameters);
                    TotalDescuentosGlobales = dtDescuentosGlobales.Rows.Count;
                }
                catch (Exception)
                {
                    TotalDescuentosGlobales = 0;
                }


                iTabla = 8;
                parameters.Remove("@Tipo");
                parameters.Add("@Tipo", iTabla);
                dtVoucherCliente = cnxVanti.GetDataTable("SP_ObtieneDatosVenta", parameters);

                iTabla = 9;
                parameters.Remove("@Tipo");
                parameters.Add("@Tipo", iTabla);
                dtCuponesGenerados = cnxVanti.GetDataTable("SP_ObtieneDatosVenta", parameters);
                TotalCuponesGenerados = dtCuponesGenerados.Rows.Count;

                iTabla = 10;
                parameters.Remove("@Tipo");
                parameters.Add("@Tipo", iTabla);
                try
                {
                    dtCuponesGenerados = cnxVanti.GetDataTable("SP_ObtieneDatosVenta", parameters);
                    TotalNotasDeCreditoGeneradas = dtNotasDeCreditoGeneradas.Rows.Count;
                }
                catch (Exception)
                {
                    TotalNotasDeCreditoGeneradas = 0;
                }

                //v.2.8 CGC Integración de ticketsPv
                if (bTieneFinLag)
                {

                    try
                    {

                        iTabla = 11;
                        parameters.Remove("@Tipo");
                        parameters.Add("@Tipo", iTabla);
                        //dtFormasPago = cnxVanti.GetDataTable("SP_ObtieneDatosVenta", parameters);
                        dtFinLag = cnxVanti.GetDataTable("SP_ObtieneDatosVenta", parameters);
                        //Este coddigo se quita por que no recibo conecciones como parametros
                        //dtFinLag = cnxVanti.GetDataTable(connection, "SP_ObtieneDatosVenta", parameters);
                    }
                    catch (Exception)
                    {
                        iTabla = iTabla;
                    }
                }



                TotalPartidas = dtDetalle.Rows.Count;

                iTabla = 12;
                CantidadPiezas = 0;
                for (i = 0; i <= TotalPartidas - 1; i++)
                    CantidadPiezas += Double.Parse(dtDetalle.Rows[i]["cantidadvendidadetalle"].ToString());


                iTabla = 15;
                iNumeroCopias = Int32.Parse(dtCabecero.Rows[0]["numerocopiasCabecero"].ToString());

                if (sImpresora == "")
                {
                    if (iCaja == 0)
                        //MsgBox("Error al momento de imprimir ticket. La caja 0 no puede combinarse con impresora sin nombre para el ticket, verifica")
                        return;

                    //cmd.CommandText = "select rtrim(ltrim(rutaimpresora)) from cnfFuncionesCajaCab where codigoSucursal = (select codigoSucursal from cnfSucursalLocalCnf) AND numerocaja = " + iCaja;
                    //sImpresora = cmd.ExecuteScalar().ToString();
                    //if (System.Net.Dns.GetHostName().ToUpper() == "CGUZMANC1")
                    //    //sImpresora = "EPSON TM-T88V Receipt";
                    //    sImpresora = "Bolt PDF";

                    ConfigurationManager.RefreshSection("appSettings");
                    sImpresora = ConfigurationManager.AppSettings["impresora"];

                    //sImpresora = "PDF24 PDF"; 
                    //sImpresora = "Win2PDF";

                    //sImpresora = "Tickets";
                }

                if (dtCabecero.Rows[0]["AbrirCajonCabecero"].ToString() == "1")
                    AbrirCajon(sImpresora);
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.ToString());
            }

        }

        public bool AbrirCajon(string szPrinterName)
        {
            //27,112,48,55,121
            Int32 dwWritten = 0;
            IntPtr hPrinter = new IntPtr(0);
            PrintDirect.DOCINFOA di = new PrintDirect.DOCINFOA();
            bool bSuccess = false;
            if (dtCabecero.Rows[0]["CodigoAperturaCajonCabecero"].ToString() == "")
                return false;

            string[] splitCodes = dtCabecero.Rows[0]["CodigoAperturaCajonCabecero"].ToString().Split(',');

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