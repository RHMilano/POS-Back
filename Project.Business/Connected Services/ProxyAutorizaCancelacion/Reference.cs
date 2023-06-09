﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Milano.BackEnd.Business.ProxyAutorizaCancelacion {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Respuesta", Namespace="http://tempuri.org/")]
    [System.SerializableAttribute()]
    public partial class Respuesta : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private int hayErrorField;
        
        private int autorizadoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string sMensajeField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public int hayError {
            get {
                return this.hayErrorField;
            }
            set {
                if ((this.hayErrorField.Equals(value) != true)) {
                    this.hayErrorField = value;
                    this.RaisePropertyChanged("hayError");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=1)]
        public int autorizado {
            get {
                return this.autorizadoField;
            }
            set {
                if ((this.autorizadoField.Equals(value) != true)) {
                    this.autorizadoField = value;
                    this.RaisePropertyChanged("autorizado");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string sMensaje {
            get {
                return this.sMensajeField;
            }
            set {
                if ((object.ReferenceEquals(this.sMensajeField, value) != true)) {
                    this.sMensajeField = value;
                    this.RaisePropertyChanged("sMensaje");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ProxyAutorizaCancelacion.wsAutorizacionCancelacionTrxSoap")]
    public interface wsAutorizacionCancelacionTrxSoap {
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento folioVenta del espacio de nombres http://tempuri.org/ no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SolicitarCancelacionTrx", ReplyAction="*")]
        Milano.BackEnd.Business.ProxyAutorizaCancelacion.SolicitarCancelacionTrxResponse SolicitarCancelacionTrx(Milano.BackEnd.Business.ProxyAutorizaCancelacion.SolicitarCancelacionTrxRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SolicitarCancelacionTrxRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SolicitarCancelacionTrx", Namespace="http://tempuri.org/", Order=0)]
        public Milano.BackEnd.Business.ProxyAutorizaCancelacion.SolicitarCancelacionTrxRequestBody Body;
        
        public SolicitarCancelacionTrxRequest() {
        }
        
        public SolicitarCancelacionTrxRequest(Milano.BackEnd.Business.ProxyAutorizaCancelacion.SolicitarCancelacionTrxRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class SolicitarCancelacionTrxRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public int codigoTienda;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=1)]
        public int codigoCaja;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=2)]
        public int codigoTipoTrxCab;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string folioVenta;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=4)]
        public int transaccion;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=5)]
        public string nombreCajero;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=6)]
        public int codigoCajeroAutorizo;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=7)]
        public decimal totalTransaccion;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=8)]
        public decimal totalTransaccionPositivo;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=9)]
        public int totalPiezas;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=10)]
        public int totalPiezasPositivas;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=11)]
        public string codigoRazonMMS;
        
        public SolicitarCancelacionTrxRequestBody() {
        }
        
        public SolicitarCancelacionTrxRequestBody(int codigoTienda, int codigoCaja, int codigoTipoTrxCab, string folioVenta, int transaccion, string nombreCajero, int codigoCajeroAutorizo, decimal totalTransaccion, decimal totalTransaccionPositivo, int totalPiezas, int totalPiezasPositivas, string codigoRazonMMS) {
            this.codigoTienda = codigoTienda;
            this.codigoCaja = codigoCaja;
            this.codigoTipoTrxCab = codigoTipoTrxCab;
            this.folioVenta = folioVenta;
            this.transaccion = transaccion;
            this.nombreCajero = nombreCajero;
            this.codigoCajeroAutorizo = codigoCajeroAutorizo;
            this.totalTransaccion = totalTransaccion;
            this.totalTransaccionPositivo = totalTransaccionPositivo;
            this.totalPiezas = totalPiezas;
            this.totalPiezasPositivas = totalPiezasPositivas;
            this.codigoRazonMMS = codigoRazonMMS;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SolicitarCancelacionTrxResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SolicitarCancelacionTrxResponse", Namespace="http://tempuri.org/", Order=0)]
        public Milano.BackEnd.Business.ProxyAutorizaCancelacion.SolicitarCancelacionTrxResponseBody Body;
        
        public SolicitarCancelacionTrxResponse() {
        }
        
        public SolicitarCancelacionTrxResponse(Milano.BackEnd.Business.ProxyAutorizaCancelacion.SolicitarCancelacionTrxResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class SolicitarCancelacionTrxResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public Milano.BackEnd.Business.ProxyAutorizaCancelacion.Respuesta SolicitarCancelacionTrxResult;
        
        public SolicitarCancelacionTrxResponseBody() {
        }
        
        public SolicitarCancelacionTrxResponseBody(Milano.BackEnd.Business.ProxyAutorizaCancelacion.Respuesta SolicitarCancelacionTrxResult) {
            this.SolicitarCancelacionTrxResult = SolicitarCancelacionTrxResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface wsAutorizacionCancelacionTrxSoapChannel : Milano.BackEnd.Business.ProxyAutorizaCancelacion.wsAutorizacionCancelacionTrxSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class wsAutorizacionCancelacionTrxSoapClient : System.ServiceModel.ClientBase<Milano.BackEnd.Business.ProxyAutorizaCancelacion.wsAutorizacionCancelacionTrxSoap>, Milano.BackEnd.Business.ProxyAutorizaCancelacion.wsAutorizacionCancelacionTrxSoap {
        
        public wsAutorizacionCancelacionTrxSoapClient() {
        }
        
        public wsAutorizacionCancelacionTrxSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public wsAutorizacionCancelacionTrxSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public wsAutorizacionCancelacionTrxSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public wsAutorizacionCancelacionTrxSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Milano.BackEnd.Business.ProxyAutorizaCancelacion.SolicitarCancelacionTrxResponse Milano.BackEnd.Business.ProxyAutorizaCancelacion.wsAutorizacionCancelacionTrxSoap.SolicitarCancelacionTrx(Milano.BackEnd.Business.ProxyAutorizaCancelacion.SolicitarCancelacionTrxRequest request) {
            return base.Channel.SolicitarCancelacionTrx(request);
        }
        
        public Milano.BackEnd.Business.ProxyAutorizaCancelacion.Respuesta SolicitarCancelacionTrx(int codigoTienda, int codigoCaja, int codigoTipoTrxCab, string folioVenta, int transaccion, string nombreCajero, int codigoCajeroAutorizo, decimal totalTransaccion, decimal totalTransaccionPositivo, int totalPiezas, int totalPiezasPositivas, string codigoRazonMMS) {
            Milano.BackEnd.Business.ProxyAutorizaCancelacion.SolicitarCancelacionTrxRequest inValue = new Milano.BackEnd.Business.ProxyAutorizaCancelacion.SolicitarCancelacionTrxRequest();
            inValue.Body = new Milano.BackEnd.Business.ProxyAutorizaCancelacion.SolicitarCancelacionTrxRequestBody();
            inValue.Body.codigoTienda = codigoTienda;
            inValue.Body.codigoCaja = codigoCaja;
            inValue.Body.codigoTipoTrxCab = codigoTipoTrxCab;
            inValue.Body.folioVenta = folioVenta;
            inValue.Body.transaccion = transaccion;
            inValue.Body.nombreCajero = nombreCajero;
            inValue.Body.codigoCajeroAutorizo = codigoCajeroAutorizo;
            inValue.Body.totalTransaccion = totalTransaccion;
            inValue.Body.totalTransaccionPositivo = totalTransaccionPositivo;
            inValue.Body.totalPiezas = totalPiezas;
            inValue.Body.totalPiezasPositivas = totalPiezasPositivas;
            inValue.Body.codigoRazonMMS = codigoRazonMMS;
            Milano.BackEnd.Business.ProxyAutorizaCancelacion.SolicitarCancelacionTrxResponse retVal = ((Milano.BackEnd.Business.ProxyAutorizaCancelacion.wsAutorizacionCancelacionTrxSoap)(this)).SolicitarCancelacionTrx(inValue);
            return retVal.Body.SolicitarCancelacionTrxResult;
        }
    }
}
