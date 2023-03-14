using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaLlavesBBVAv2
{
    class Program
    {
        static void Main(string[] args)
        {
            Llaves llaves = new Llaves();


            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("APP CARGA LLAVES BBVA V2 ");
            Console.WriteLine("MILANO OPERADORA S.A. DE C.V.");
            Console.WriteLine("");

            llaves.CargaManual();





         }
    }
}
