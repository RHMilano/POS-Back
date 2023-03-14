using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Milano.BackEnd.Repository.General;
using Milano.BackEnd.Dto.General;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Repository;
using Milano.BackEnd.Dto.Impresion;
using Milano.BackEnd.Business.ImpresionMM;
using static Milano.BackEnd.Business.ImpresionMM.ImprimeTicketsMM;
using System.Runtime.InteropServices;

namespace Milano.BackEnd.Business.General
{

    /// <summary>
    /// Clase de negocio de Egresos
    /// </summary>
    public class EgresosBusiness : BaseBusiness
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
        protected EgresosRepository repository;

        /// <summary>
        ///Atributo del token usuario
        /// </summary>
        protected TokenDto token;

        /// <summary>
        /// Constructor sin parametros
        /// </summary>
        public EgresosBusiness(TokenDto token)
        {
            this.PrinterName = "";
            this.token = token;
            this.printTicketRepository = new PrintTicketRepository();
            this.repository = new EgresosRepository();
            this.SetPrinterConfig();
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
        /// Retiro Parcial de Efectivo
        /// </summary>
        /// <param name="retiroParcialEfectivo">Objeto que representa el retiro parcial de efectivo</param>
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<OperationResponse> RetiroParcialEfectivo(RetiroParcialEfectivo retiroParcialEfectivo)
        {
            return tryCatch.SafeExecutor(() =>

            {
                OperationResponse operationResponse = new OperationResponse();
                RetiroParcialEfectivoResponse retiroParcialEfectivoResponse = repository.RetiroParcialEfectivo(token.CodeStore, token.CodeBox, token.CodeEmployee, retiroParcialEfectivo);
                operationResponse.CodeDescription = retiroParcialEfectivoResponse.CodeDescription;
                operationResponse.CodeNumber = retiroParcialEfectivoResponse.CodeNumber;

                AbrirCajon();
                PrintTicketEgresosBusiness printTicketEgresos = new PrintTicketEgresosBusiness(token);
                PrintTicketEgresosRequest printTicketEgresosRequest = new PrintTicketEgresosRequest();
                printTicketEgresosRequest.FolioRetiro = retiroParcialEfectivoResponse.FolioRetiro;
                printTicketEgresos.PrintNow(printTicketEgresosRequest);

                return operationResponse;

            });
        }

        /// <summary>
        /// Ignorar Retiro en efectivo
        /// </summary>
        /// <returns></returns>
        public ResponseBussiness<OperationResponse> IgnorarRetiroEfectivo()
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.IgnorarRetiro(token.CodeBox);
            });
        }


        /// <summary>
        /// Procesar Egreso
        /// </summary>
        /// <param name="egreso">Objeto que representa el egreso de efectivo</param>
        /// <returns>Respuesta de la operación</returns>
        public ResponseBussiness<OperationResponse> ProcesarEgreso(Egreso egreso)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.ProcesarEgreso(token.CodeStore, token.CodeBox, token.CodeEmployee, egreso);
            });
        }

    }
}
