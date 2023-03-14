using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Milano.BackEnd.Dto;
using Milano.BackEnd.Repository;
namespace Milano.BackEnd.Business
{
	/// <summary>
	/// Clase de negocios para listas 
	/// </summary>
	public class ElementosBusiness:BaseBusiness 
	{
		private ElementosRepository repository;

		/// <summary>
		/// Constructor 
		/// </summary>
		public ElementosBusiness()
		{
			repository = new ElementosRepository();
		}

		/// <summary>
		/// Lista de proveedores
		/// </summary>
		/// <returns>Arreglo de proveedores</returns>
		public ResponseBussiness<Elemento []> Proveedores()
		{
			return tryCatch.SafeExecutor(() =>
			{
				return repository.Proveedores();
			});
		}

		/// <summary>
		/// LIsta de estilos por proveedor
		/// </summary>
		/// <param name="codigoProveedor">codigo del proveedor</param>
		/// <returns>Arreglo de estilos</returns>
		public ResponseBussiness<EstiloDto[]> Estilos(int codigoProveedor)
		{
			return tryCatch.SafeExecutor(() =>
			{
				return repository.Estilos (codigoProveedor);
			});
		}

		/// <summary>
		/// Lista de departamentos
		/// </summary>
		/// <returns>Arreglo de departamentos</returns>
		public ResponseBussiness<Elemento[]> Departamentos()
		{
			return tryCatch.SafeExecutor(() =>
			{
				return repository.Departamentos ();
			});
		}

		/// <summary>
		/// Lista de subdepartamentos
		/// </summary>
		/// <param name="codigoDepartamento">Codigo del departamento</param>
		/// <returns>Arreglo de subdeparamentos</returns>
		public ResponseBussiness<Elemento[]> Subdepartamentos(int codigoDepartamento)
		{
			return tryCatch.SafeExecutor(() =>
			{
				return repository.SubDepartamentos (codigoDepartamento);
			});
		}

		/// <summary>
		/// Lista de clases
		/// </summary>
		/// <param name="codigoDepartamento">Codigo del departamento</param>
		/// <param name="codigoSubDepartamento">Codigo del subdepartamento</param>
		/// <returns>Arreglo de clases</returns>
		public ResponseBussiness<Elemento[]> Clases(int codigoDepartamento,int codigoSubDepartamento)
		{
			return tryCatch.SafeExecutor(() =>
			{
				return repository.Clases(codigoDepartamento,codigoSubDepartamento);
			});
		}

		/// <summary>
		/// LIsta de subclases
		/// </summary>
		/// <param name="codigoDepartamento">Codigo del departamento</param>
		/// <param name="codigoSubDepartamento">Codigo del subdepartamento</param>
		/// <param name="codigoClase">Codigo de la Clase</param>
		/// <returns>Arreglo de subclases</returns>
		public ResponseBussiness<Elemento[]> SubClases(int codigoDepartamento, int codigoSubDepartamento,int codigoClase)
		{
			return tryCatch.SafeExecutor(() =>
			{
				return repository.SubClases (codigoDepartamento ,codigoSubDepartamento ,  codigoClase);
			});
		}



	}
}
