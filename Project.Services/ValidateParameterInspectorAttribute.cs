using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Text;

namespace Milano.BackEnd.Dto
{
    public class ValidateParameterInspectorAttribute : Attribute, IParameterInspector, IOperationBehavior
    {

        string tipoObjeto;

        public ValidateParameterInspectorAttribute(string tipoObjeto)
        {
            this.tipoObjeto = tipoObjeto;
        }

        public object BeforeCall(string operationName, object[] inputs)
        {
            switch (tipoObjeto)
            {
                case "ProductsRequest":
                    InputParams.ValidandoParametrosProductsRequest(inputs);
                    break;
            }

            return null;
        }

        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {

        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            dispatchOperation.ParameterInspectors.Add(this);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationDescription"></param>
        public void Validate(OperationDescription operationDescription)
        {
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

    }

    public class InputParams
    {
        internal static void ValidandoParametrosProductsRequest(object[] inputs)
        {

        }



        private static Exception ThrowFaultException(string mensaje)
        {

            throw new Exception(mensaje);
        }


    }
}
