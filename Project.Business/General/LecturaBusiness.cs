using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Milano.BackEnd.Repository.General;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Repository;
using System.Transactions;
using Milano.BackEnd.Business.ImpresionMM;
using Milano.BackEnd.Dto.Impresion;
using static Milano.BackEnd.Business.ImpresionMM.ImprimeTicketsMM;
using System.Runtime.InteropServices;
using System.Data;
using System.Drawing.Printing;
using System.Drawing;

namespace Milano.BackEnd.Business.General
{
    /// <summary>
    /// Clase de negocio de Lectura
    /// </summary>
    public class LecturaBusiness : BaseBusiness
    {

        private String PrinterName;
        private PrinterConfigResponse printerConfigResponse;

        /// <summary>
        /// Repositorio para abrir cajon
        /// </summary>
        private PrintTicketRepository printTicketRepository;

        /// <summary>
        /// Atributo de repositorio de egresos
        /// </summary>
        private LecturaRepository repository;

        /// <summary>
        ///Atributo del token usuario
        /// </summary>
        private TokenDto token;

        /// <summary>
        /// Constructor sin parametros
        /// </summary>
        public LecturaBusiness(TokenDto token)
        {
            this.PrinterName = "";
            this.token = token;
            this.printTicketRepository = new PrintTicketRepository();
            this.repository = new LecturaRepository();
            this.SetPrinterConfig();
        }

        /// <summary>
        /// Obtener totales por forma de pago
        /// </summary>
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<LecturaTotalDetalleFormaPago[]> ObtenerTotalesPorFormaPago()
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.ObtenerTotalesPorFormaPago(token.CodeStore, token.CodeBox);
            });
        }

        /// <summary>
        /// Obtener totales por forma de pago
        /// </summary>
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<LecturaTotalDetalleFormaPago[]> ObtenerTotalesPorFormaPago(int caja)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.ObtenerTotalesPorFormaPago(token.CodeStore, token.CodeBox, caja);
            });
        }

        /// <summary>
        /// Establece la configuracion de la impresora 
        /// </summary>
        private void SetPrinterConfig()
        {
            PrinterConfigRequest printerConfigRequest = new PrinterConfigRequest();
            printerConfigRequest.CodigoCaja = this.token.CodeBox;
            printerConfigRequest.CodigoTienda = this.token.CodeStore;
            this.printerConfigResponse = printTicketRepository.getPrinterConfig(printerConfigRequest);
            this.PrinterName = this.printerConfigResponse.NombreImpresora;
        }

        /// <summary>
        /// Manda la instruccion para abrir cajon
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

        /// <summary>
        /// Lectura X
        /// </summary>
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<OperationResponse> LecturaX(LecturaCaja lecturaCaja)
        {
            return tryCatch.SafeExecutor(() =>
            {
                OperationResponse resultado = new OperationResponse();
                LecturaTotalDetalleFormaPago lecturaTotalDetalleFormaPagoCA = null;
                LecturaX lecturaX;
                using (TransactionScope scope = new TransactionScope())
                {
                    int secuencia = 1;
                    lecturaX = repository.ObtenerFoliosLecturaX(token.CodeStore, token.CodeBox, token.CodeEmployee, 0);
                    foreach (var lecturaTotalDetalleFormaPago in lecturaCaja.LecturasTotales)
                    {
                        resultado = repository.LecturaX(token.CodeStore, token.CodeBox, token.CodeEmployee, lecturaTotalDetalleFormaPago.InformacionAsociadaFormasPago.CodigoFormaPago, secuencia, lecturaTotalDetalleFormaPago.ImporteFisico, lecturaTotalDetalleFormaPago.ImporteTeorico, 0.00M, lecturaX);
                        if (lecturaTotalDetalleFormaPago.InformacionAsociadaFormasPago.CodigoFormaPago == "CA")
                        {
                            lecturaTotalDetalleFormaPagoCA = lecturaTotalDetalleFormaPago;
                        }
                        secuencia = secuencia + 1;
                    }
                    if (lecturaTotalDetalleFormaPagoCA != null)
                    {
                        // Se persisten las denominaciones
                        foreach (var item in lecturaTotalDetalleFormaPagoCA.InformacionAsociadaDenominaciones)
                        {
                            repository.PersistirDenominacionesRetiro(lecturaX.FolioCorteParcial, item.CodigoFormaPago, item.TextoDenominacion, item.Cantidad);
                        }
                    }
                    scope.Complete();
                }
                // Imprimir Ticket
                PrintTickectLecturaBusiness printTickectLectura = new PrintTickectLecturaBusiness(token);
                PrintLecturaRequest printLecturaRequest = new PrintLecturaRequest();
                printLecturaRequest.FolioCorte = lecturaX.FolioCorteParcial;
                printLecturaRequest.TipoLectura = "x";
                printTickectLectura.PrintNow(printLecturaRequest);
                return resultado;
            });
        }

        /// <summary>
        /// Lectura Z
        /// </summary>
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<OperationResponse> LecturaZ(LecturaCaja lecturaCaja)
        {
            return tryCatch.SafeExecutor(() =>
            {
                OperationResponse resultado = new OperationResponse();
                LecturaZGuardarResponse lecturaZGuardarResponse = new LecturaZGuardarResponse();
                LecturaTotalDetalleFormaPago lecturaTotalDetalleFormaPagoCA = null;
                using (TransactionScope scope = new TransactionScope())
                {
                    int secuencia = 1;
                    // Se ejecuta la lectura X
                    LecturaX lecturaX = repository.ObtenerFoliosLecturaX(token.CodeStore, token.CodeBox, token.CodeEmployee, 0);
                    foreach (var lecturaTotalDetalleFormaPago in lecturaCaja.LecturasTotales)
                    {
                        resultado = repository.LecturaX(token.CodeStore, token.CodeBox, token.CodeEmployee, lecturaTotalDetalleFormaPago.InformacionAsociadaFormasPago.CodigoFormaPago, secuencia, lecturaTotalDetalleFormaPago.ImporteFisico, lecturaTotalDetalleFormaPago.ImporteTeorico, lecturaTotalDetalleFormaPago.ImporteFisico, lecturaX);
                        if (lecturaTotalDetalleFormaPago.InformacionAsociadaFormasPago.CodigoFormaPago == "CA")
                        {
                            lecturaTotalDetalleFormaPagoCA = lecturaTotalDetalleFormaPago;
                        }
                        secuencia = secuencia + 1;
                    }
                    // Se ejecutar la lectura Z
                    if (lecturaTotalDetalleFormaPagoCA != null)
                    {
                        lecturaZGuardarResponse = repository.LecturaZ(token.CodeStore, token.CodeBox, token.CodeEmployee,
                            lecturaX.FolioCorteParcial, lecturaTotalDetalleFormaPagoCA.ImporteFisico,
                            lecturaTotalDetalleFormaPagoCA.ImporteTeorico, lecturaTotalDetalleFormaPagoCA.ImporteFisico, 0);
                        resultado.CodeNumber = lecturaZGuardarResponse.CodeNumber;
                        resultado.CodeDescription = lecturaZGuardarResponse.CodeDescription;
                        // Se persisten las denominaciones
                        foreach (var item in lecturaTotalDetalleFormaPagoCA.InformacionAsociadaDenominaciones)
                        {
                            repository.PersistirDenominacionesRetiro(lecturaX.FolioCorteParcial, item.CodigoFormaPago, item.TextoDenominacion, item.Cantidad);
                        }
                    }
                    scope.Complete();
                }

                // Imprimir Ticket
                PrintTickectLecturaBusiness printTickectLectura = new PrintTickectLecturaBusiness(token);
                PrintLecturaRequest printLecturaRequest = new PrintLecturaRequest();
                printLecturaRequest.FolioCorte = lecturaZGuardarResponse.FolioCorte;
                printLecturaRequest.TipoLectura = "z";
                printTickectLectura.PrintNow(printLecturaRequest);

                PrintTicketEgresosBusiness printTicketEgresos = new PrintTicketEgresosBusiness(token);
                PrintTicketEgresosRequest printTicketEgresosRequest = new PrintTicketEgresosRequest();
                printTicketEgresosRequest.FolioCorteZ = lecturaZGuardarResponse.FolioCorte;
                printTicketEgresos.PrintNow(printTicketEgresosRequest);

                return resultado;
            });
        }

        /// <summary>
        /// ObtenerDenominaciones
        /// </summary>
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<Denominacion[]> ObtenerDenominaciones()
        {
            return tryCatch.SafeExecutor(() =>
            {
                //Abrir Cajón
                AbrirCajon();
                //Mandamos a llamar al objeto para obtener denominaciones desde business
                return repository.ObtenerDenominacionesPorMarca(token.CodeStore);

            });
        }

        /// <summary>
        /// Lectura Z Offline
        /// </summary>
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<LecturaZGuardarResponse> LecturaZOffline(LecturaCaja lecturaCaja)
        {
            return tryCatch.SafeExecutor(() =>
            {
                OperationResponse resultado = new OperationResponse();
                LecturaZGuardarResponse lecturaZGuardarResponse = new LecturaZGuardarResponse();
                LecturaTotalDetalleFormaPago lecturaTotalDetalleFormaPagoCA = null;
                using (TransactionScope scope = new TransactionScope())
                {
                    int secuencia = 1;
                    // Se ejecuta la lectura X
                    LecturaX lecturaX = repository.ObtenerFoliosLecturaX(token.CodeStore, token.CodeBox, token.CodeEmployee, 1);
                    foreach (var lecturaTotalDetalleFormaPago in lecturaCaja.LecturasTotales)
                    {
                        // Se pone el parámetro de Caja = 0 porque es BackOffice
                        resultado = repository.LecturaX(token.CodeStore, 0, token.CodeEmployee,
                            lecturaTotalDetalleFormaPago.InformacionAsociadaFormasPago.CodigoFormaPago, secuencia,
                            lecturaTotalDetalleFormaPago.ImporteFisico, lecturaTotalDetalleFormaPago.ImporteTeorico,
                            lecturaTotalDetalleFormaPago.ImporteFisico, lecturaX);
                        if (lecturaTotalDetalleFormaPago.InformacionAsociadaFormasPago.CodigoFormaPago == "CA")
                        {
                            lecturaTotalDetalleFormaPagoCA = lecturaTotalDetalleFormaPago;
                        }
                        secuencia = secuencia + 1;
                    }
                    // Se ejecutar la lectura Z
                    if (lecturaTotalDetalleFormaPagoCA != null)
                    {
                        lecturaZGuardarResponse = repository.LecturaZ(token.CodeStore, token.CodeBox, token.CodeEmployee,
                            lecturaX.FolioCorteParcial, lecturaTotalDetalleFormaPagoCA.ImporteFisico,
                            lecturaTotalDetalleFormaPagoCA.ImporteTeorico, lecturaTotalDetalleFormaPagoCA.ImporteFisico, 1);
                        resultado.CodeNumber = lecturaZGuardarResponse.CodeNumber;
                        resultado.CodeDescription = lecturaZGuardarResponse.CodeDescription;

                        // Se persisten las denominaciones
                        foreach (var item in lecturaTotalDetalleFormaPagoCA.InformacionAsociadaDenominaciones)
                        {
                            repository.PersistirDenominacionesRetiro(lecturaX.FolioCorteParcial, item.CodigoFormaPago, item.TextoDenominacion, item.Cantidad);
                        }
                    }
                    scope.Complete();
                }
                return lecturaZGuardarResponse;
            });
        }

    }
}
