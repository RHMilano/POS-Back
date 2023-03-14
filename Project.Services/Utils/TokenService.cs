using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;
using Newtonsoft.Json;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Utils;

namespace Project.Services
{
	/// <summary>
	/// Clase para leer el token
	/// </summary>
	public class TokenService
	{
		/// <summary>
		/// Metodo de lectura del token
		/// </summary>
		/// <returns></returns>
		public TokenDto Get()
		{
			var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
			var access_token = Encrypted.Decode(authHeader);
			var token = JsonConvert.DeserializeObject<Dictionary<string, object>>(access_token);
			TokenDto tokenDto = new TokenDto(int.Parse(token["codeStore"].ToString()), int.Parse(token["codeBox"].ToString()), int.Parse(token["usuario"].ToString()));			
			return tokenDto;
		}
	}
}