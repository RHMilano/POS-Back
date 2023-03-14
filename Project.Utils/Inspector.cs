using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Milano.BackEnd.Utils;

namespace Project.Services.Utils
{
    /// <summary>
    /// Clase para ejecutar acciones sobre los mensajes
    /// </summary>
    public class Inspector
    {

        /// <summary>
        /// Metodo para truncar valores decimales
        /// </summary>
        /// <returns></returns>
        public decimal TruncarValor(decimal valor)
        {
            if (valor != 0)
            {
                String nuevoValorDecimal = (Math.Truncate(100 * valor) / 100).ToString();
                decimal valorFinalDecimal = decimal.Parse(nuevoValorDecimal);
                return valorFinalDecimal;
            }
            else
            {
                return valor;
            }
        }

        /// <summary>
        /// Metodo para truncar valores doubles
        /// </summary>
        /// <returns></returns>
        public double TruncarValor(double valor)
        {
            if (valor != 0)
            {
                String nuevoValorDouble = (Math.Truncate(100 * valor) / 100).ToString();
                double valorFinalDouble = double.Parse(nuevoValorDouble);
                return valorFinalDouble;
            }
            else
            {
                return valor;
            }
        }

    }
}