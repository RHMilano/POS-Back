using DTOPos.ApiResponses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Tools
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
		public Token Get()
		{
			var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
			var access_token = Encrypted.Decode(authHeader);
			var token = JsonConvert.DeserializeObject<Dictionary<string, object>>(access_token);
			Token _token = new Token(int.Parse(token["codeStore"].ToString()), int.Parse(token["codeBox"].ToString()), int.Parse(token["usuario"].ToString()));
			return _token;
		}
	}
}
