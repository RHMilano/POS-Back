using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Threading;
using System.Web;

namespace Project.Services
{
    /// <summary>
    /// Clase de validacion de atributo de seguridad para los servicios
    /// </summary>
    public class CustomMembershipAuthorization : Attribute, IOperationBehavior, IParameterInspector
    {
        /// <summary>
        /// Recurso
        /// </summary>
        public string Resource { get; set; }
        /// <summary>
        /// Permico CRUD
        /// </summary>
        public string Permission { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public CustomMembershipAuthorization()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="resource">Nombre del recurso</param>
        /// <param name="permission">Permiso requerido sobre el recurso (C,R,U,D,E)</param>
        public CustomMembershipAuthorization(string resource, string permission)
        {
            Permission = permission;
            Resource = resource;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationDescription"></param>
        /// <param name="dispatchOperation"></param>
        public void ApplyDispatchBehavior
        (OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            dispatchOperation.ParameterInspectors.Add(this);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationName"></param>
        /// <param name="outputs"></param>
        /// <param name="returnValue"></param>
        /// <param name="correlationState"></param>
        public void AfterCall(string operationName, object[] outputs,
                              object returnValue, object correlationState)
        {
        }

        /// <summary>
        /// Método que valida antes de invocar a un método de negocio si el usuario tiene privilegios suficientes
        /// </summary>
        /// <param name="operationName"></param>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public object BeforeCall(string operationName, object[] inputs)
        {
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationDescription"></param>
        /// <param name="bindingParameters"></param>
        public void AddBindingParameters(OperationDescription operationDescription,
        System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationDescription"></param>
        /// <param name="clientOperation"></param>
        public void ApplyClientBehavior
        (OperationDescription operationDescription, ClientOperation clientOperation)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationDescription"></param>
        public void Validate(OperationDescription operationDescription)
        {
        }

    }

}