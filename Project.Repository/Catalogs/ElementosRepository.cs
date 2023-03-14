using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Milano.BackEnd.Dto;
namespace Milano.BackEnd.Repository
{
	/// <summary>
	/// Clase para obtener lista de elementos
	/// </summary>
	public class ElementosRepository:BaseRepository 
	{
		/// <summary>
		/// Lista de proveedores
		/// </summary>
		/// <returns>Arreglo de proveedores</returns>
		public Elemento[] Proveedores()
		{
			List<Elemento> list = new List<Elemento>();
			var parameters = new Dictionary<string, object>();			
			foreach (var item in data.GetDataReader("[dbo].[sp_vanti_ProveedoresObtener]", parameters))
				list.Add(new Elemento() {  Codigo =Convert.ToInt32 ( item.GetValue (0)) , Descripcion =item.GetValue (1).ToString ()});
			return list.ToArray();
		}

		/// <summary>
		/// Lista de Estilos por proveedor
		/// </summary>
		/// <param name="codigoProveedor">Codigo del proveedor</param>
		/// <returns>Arreglo de estilos</returns>
		public EstiloDto [] Estilos(int codigoProveedor)
		{
			List<EstiloDto> list = new List<EstiloDto>();
			var parameters = new Dictionary<string, object>();
			parameters.Add("@CodigoProveedor", codigoProveedor);
			foreach (var item in data.GetDataReader("[dbo].[sp_vanti_EstilosObtener]", parameters))
				list.Add(new EstiloDto() { Codigo =item.GetValue(0).ToString (), Descripcion = item.GetValue(1).ToString().Trim () });
			return list.ToArray();
		}



		/// <summary>
		/// Lista de Departamentos
		/// </summary>
		/// <returns>Arreglo de Departamentos</returns>
		public Elemento[] Departamentos()
		{
			List<Elemento> list = new List<Elemento>();
			var parameters = new Dictionary<string, object>();
			foreach (var item in data.GetDataReader("[dbo].[sp_vanti_DepartamentosObtener]", parameters))
				list.Add(new Elemento() { Codigo = Convert.ToInt32(item.GetValue(0)), Descripcion = item.GetValue(1).ToString() });
			return list.ToArray();
		}

		/// <summary>
		/// Lista de Subdepartamentos
		/// </summary>
		/// <param name="codigoDepartamento">Codigo del departamento</param>
		/// <returns>Arreglo de subdepartamentos</returns>
		public Elemento[] SubDepartamentos(int codigoDepartamento)
		{
			List<Elemento> list = new List<Elemento>();
			var parameters = new Dictionary<string, object>();
			parameters.Add("@CodigoDepartamento", codigoDepartamento);
			foreach (var item in data.GetDataReader("[dbo].[sp_vanti_SubDepartamentosObtener]", parameters))
				list.Add(new Elemento() { Codigo = Convert.ToInt32(item.GetValue(0)), Descripcion = item.GetValue(1).ToString() });
			return list.ToArray();
		}

		/// <summary>
		/// Lista de clases
		/// </summary>
		/// <param name="codigoDepartamento">Codigo del departamento</param>
		/// <param name="codigoSubdepartamento">Codigo del subdepartamento</param>
		/// <returns>Arreglo de clases</returns>
		public Elemento[] Clases(int codigoDepartamento,int codigoSubdepartamento)
		{
			List<Elemento> list = new List<Elemento>();
			var parameters = new Dictionary<string, object>();
			parameters.Add("@CodigoDepartamento", codigoDepartamento);
			parameters.Add("@CodigoSubDepartamento", codigoSubdepartamento);
			foreach (var item in data.GetDataReader("[dbo].[sp_vanti_ClaseObtener]", parameters))
				list.Add(new Elemento() { Codigo = Convert.ToInt32(item.GetValue(0)), Descripcion = item.GetValue(1).ToString() });
			return list.ToArray();
		}

		/// <summary>
		/// LIsta de subclases
		/// </summary>
		/// <param name="codigoDepartamento">Codigo del departamento</param>
		/// <param name="codigoSubdepartamento">Codigo del subdepartamento</param>
		/// <param name="codigoClase">Codigo de claes</param>
		/// <returns>Arrerglo de subclases</returns>
		public Elemento[] SubClases(int codigoDepartamento, int codigoSubdepartamento,int codigoClase)
		{
			List<Elemento> list = new List<Elemento>();
			var parameters = new Dictionary<string, object>();
			parameters.Add("@CodigoDepartamento", codigoDepartamento);
			parameters.Add("@CodigoSubDepartamento", codigoSubdepartamento);
			parameters.Add("@codigoClase", codigoClase);
			foreach (var item in data.GetDataReader("[dbo].[sp_vanti_SubClaseObtener]", parameters))
				list.Add(new Elemento() { Codigo = Convert.ToInt32(item.GetValue(0)), Descripcion = item.GetValue(1).ToString() });
			return list.ToArray();
		}


	}
}
