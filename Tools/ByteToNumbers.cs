using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public enum NumbersInBytes
    {
        Uno = 1,
        Dos = 4,
        Tres = 8,
        Cuatro = 16,
        Cinco = 32,
        Seis = 64,
        Siete = 128,
        Ocho = 256,
        Nueve = 512,
        Diez = 1024,
        Once = 2048,
        Doce = 4096,
        Trece = 8192,
        Catorce = 16384,
        Quince = 32768,
        Dieciséis = 65537,
        Diecisiéte = 131072,
        Dieciocho = 262144
    }




    public class ByteToNumbers
    {
        public string SelectNumbers(int ByteValue)
        {
            Dictionary<string, string> openWith = new Dictionary<string, string>();

            string x = "";

            //var _bytes = new int[] { 65537, 32768, 16384, 8192, 4096, 2048, 1024, 512, 256, 128, 64, 32, 16, 8, 4, 1 };
            


            string[] _bytes = new string[] { 
                    "262144,18,DieciOcho",
                    "131072,17,DieciSiéte",
                    "65536,16,DieciSéis",
                    "32768,15,Quince",
                    "16384,14,Catorce",
                    "8192,13,Trece",
                    "4096,12,Doce",
                    "2048,11,Once",
                    "1024,10,Diez",
                    "512,9,Nueve",
                    "256,8,Ocho",
                    "128,7,Siete",
                    "64,6,Seis",
                    "32,5,Cinco",
                    "16,4,Cuatro",
                    "8,3,Tres",
                    "4,2,Dos",
                    "2,1,Uno"

                } ;

            foreach (string _byte in _bytes)
            {
                string[] _byteval = _byte.Split(',');

                if (ByteValue == Convert.ToInt32(_byteval[0]))
                {
                    openWith.Add(_byteval[2], _byteval[1]);
                    break;
                   
                }

                if (ByteValue > Convert.ToInt32(_byteval[0]))
                {
                    openWith.Add(_byteval[2], _byteval[1]);
                    ByteValue -= Convert.ToInt32(_byteval[0]);
                }
            }
            return x;




            //    // Add some elements to the dictionary. There are no
            //    // duplicate keys, but some of the values are duplicates.
            //   
            //    openWith.Add("bmp", "paint.exe");
            //    openWith.Add("dib", "paint.exe");
            //    openWith.Add("rtf", "wordpad.exe");


        }

    }
}
