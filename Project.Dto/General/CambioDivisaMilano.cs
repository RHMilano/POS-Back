using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Milano.BackEnd.Dto
{
	/// <summary>
	/// Dto de configuracion local milano, para cambio de divida
	/// </summary>
	public class CambioDivisaMilano
	{
		/// <summary>
		/// Valor que se usara para el cambio
		/// </summary>
		public decimal ValorCambio { get; set; }
		/// <summary>
		/// Bandera para indicar que rango usar: maximo ó minimo
		/// </summary>
		public bool UsarMaximoValor { get; set; }

		/// <summary>
		/// Código externo del cambio de divisa
		/// </summary>
		public string CodigoExterno { get; set; }
	}
}
