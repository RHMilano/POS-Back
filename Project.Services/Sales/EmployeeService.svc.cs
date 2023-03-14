using Milano.BackEnd.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using Milano.BackEnd.Business;
namespace Project.Services.Sales
{
    /// <summary>
    /// Servicio que gestiona las operaciones referentes a la entidad empleados
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class EmployeeService
    {
        /// <summary>
        ///Servicio de búsqueda de empleados
        /// </summary>
        /// <param name="employeeRequest">Objeto que representa el parámetro de búsqeuda, codigo ó nombre del empleado</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/getEmployee")]
        public ResponseBussiness<EmployeeResponse[]> SearchEmployee(EmployeeRequest employeeRequest)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<EmployeeResponse[]> response = new EmployeeBusiness(token).SearchEmployee(employeeRequest);
            return response;
        }

        /// <summary>
        /// Busqueda de empleados milano
        /// </summary>
        /// <param name="codigoEmpleado">Código empleado</param>
        /// <param name="codigoTienda">Código de caja</param>
        /// <param name="codigoCaja">Código de caja</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/getEmployeeMilano/{codigoEmpleado}/{codigoTienda}/{codigoCaja}")]
        public ResponseBussiness<EmpleadoMilanoResponse> SearchSalesEmployee(string codigoEmpleado, string codigoTienda, string codigoCaja)
        {
            TokenDto token = new TokenService().Get();
            ResponseBussiness<EmpleadoMilanoResponse> response = new AdministracionVentaEmpleadoBusiness().Buscar(codigoEmpleado, token.CodeStore.ToString(), token.CodeBox.ToString());
            return response;
        }
    }
}
