using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milano.BackEnd.Dto;
using System.Collections;
using Milano.BackEnd.Repository;

/// <summary>
/// Espacio de nombres de la logica de negocio
/// </summary>
namespace Milano.BackEnd.Business
{
    /// <summary>
    /// Clase de negocios de empleados.
    /// </summary>
    public class EmployeeBusiness : BaseBusiness
    {
        /// <summary>
        /// Repositorio de empleados
        /// </summary>
        protected EmployeeRepository repository;

		TokenDto token;

        /// <summary>
        /// Constructor por default
        /// </summary>
        public EmployeeBusiness(TokenDto token)
        {
            this.repository = new EmployeeRepository();
			this.token = token;
        }


        /// <summary>
        /// Búsqueda de empleado
        /// </summary>
        /// <param name="employee"> Objeto de peticion del empleado a buscar, contiene código y nombre del empleado</param>
        /// <returns>Arreglo de empleados encontrados</returns>
        public ResponseBussiness<EmployeeResponse[]> SearchEmployee(EmployeeRequest employee)
        {
            return tryCatch.SafeExecutor(() =>
            {
                return repository.SearchEmployee(employee.Code, employee.Name,token.CodeStore );
            });
        }
    }
}
