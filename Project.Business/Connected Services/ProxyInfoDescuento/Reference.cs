﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Milano.BackEnd.Business.ProxyInfoDescuento {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="WsPosResponseModel", Namespace="http://tempuri.org/")]
    [System.SerializableAttribute()]
    public partial class WsPosResponseModel : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private int statusField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string codeNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string mensajeField;
        
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
        public int status {
            get {
                return this.statusField;
            }
            set {
                if ((this.statusField.Equals(value) != true)) {
                    this.statusField = value;
                    this.RaisePropertyChanged("status");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string codeNumber {
            get {
                return this.codeNumberField;
            }
            set {
                if ((object.ReferenceEquals(this.codeNumberField, value) != true)) {
                    this.codeNumberField = value;
                    this.RaisePropertyChanged("codeNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string mensaje {
            get {
                return this.mensajeField;
            }
            set {
                if ((object.ReferenceEquals(this.mensajeField, value) != true)) {
                    this.mensajeField = value;
                    this.RaisePropertyChanged("mensaje");
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ProxyInfoDescuento.InfoDescuentoSoap")]
    public interface InfoDescuentoSoap {
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento ConsecutivoVenta del espacio de nombres http://tempuri.org/ no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getAutorization", ReplyAction="*")]
        Milano.BackEnd.Business.ProxyInfoDescuento.getAutorizationResponse getAutorization(Milano.BackEnd.Business.ProxyInfoDescuento.getAutorizationRequest request);
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento ConsecutivoVenta del espacio de nombres http://tempuri.org/ no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getAutorizationV2", ReplyAction="*")]
        Milano.BackEnd.Business.ProxyInfoDescuento.getAutorizationV2Response getAutorizationV2(Milano.BackEnd.Business.ProxyInfoDescuento.getAutorizationV2Request request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getAutorizationRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getAutorization", Namespace="http://tempuri.org/", Order=0)]
        public Milano.BackEnd.Business.ProxyInfoDescuento.getAutorizationRequestBody Body;
        
        public getAutorizationRequest() {
        }
        
        public getAutorizationRequest(Milano.BackEnd.Business.ProxyInfoDescuento.getAutorizationRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getAutorizationRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public int Tienda;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string ConsecutivoVenta;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=2)]
        public decimal MontoVenta;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=3)]
        public int Caja;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=4)]
        public System.DateTime Fecha;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=5)]
        public int RazonDescuento;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=6)]
        public int OpcionDescuento;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=7)]
        public string TipoDescuento;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=8)]
        public decimal MontoDescuento;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=9)]
        public int Linea;
        
        public getAutorizationRequestBody() {
        }
        
        public getAutorizationRequestBody(int Tienda, string ConsecutivoVenta, decimal MontoVenta, int Caja, System.DateTime Fecha, int RazonDescuento, int OpcionDescuento, string TipoDescuento, decimal MontoDescuento, int Linea) {
            this.Tienda = Tienda;
            this.ConsecutivoVenta = ConsecutivoVenta;
            this.MontoVenta = MontoVenta;
            this.Caja = Caja;
            this.Fecha = Fecha;
            this.RazonDescuento = RazonDescuento;
            this.OpcionDescuento = OpcionDescuento;
            this.TipoDescuento = TipoDescuento;
            this.MontoDescuento = MontoDescuento;
            this.Linea = Linea;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getAutorizationResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getAutorizationResponse", Namespace="http://tempuri.org/", Order=0)]
        public Milano.BackEnd.Business.ProxyInfoDescuento.getAutorizationResponseBody Body;
        
        public getAutorizationResponse() {
        }
        
        public getAutorizationResponse(Milano.BackEnd.Business.ProxyInfoDescuento.getAutorizationResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getAutorizationResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public Milano.BackEnd.Business.ProxyInfoDescuento.WsPosResponseModel getAutorizationResult;
        
        public getAutorizationResponseBody() {
        }
        
        public getAutorizationResponseBody(Milano.BackEnd.Business.ProxyInfoDescuento.WsPosResponseModel getAutorizationResult) {
            this.getAutorizationResult = getAutorizationResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getAutorizationV2Request {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getAutorizationV2", Namespace="http://tempuri.org/", Order=0)]
        public Milano.BackEnd.Business.ProxyInfoDescuento.getAutorizationV2RequestBody Body;
        
        public getAutorizationV2Request() {
        }
        
        public getAutorizationV2Request(Milano.BackEnd.Business.ProxyInfoDescuento.getAutorizationV2RequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getAutorizationV2RequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public int Tienda;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string ConsecutivoVenta;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=2)]
        public decimal MontoVenta;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=3)]
        public int Caja;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=4)]
        public System.DateTime Fecha;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=5)]
        public int RazonDescuento;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=6)]
        public int OpcionDescuento;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=7)]
        public string TipoDescuento;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=8)]
        public decimal MontoDescuento;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=9)]
        public int Linea;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=10)]
        public string folioCupon;
        
        public getAutorizationV2RequestBody() {
        }
        
        public getAutorizationV2RequestBody(int Tienda, string ConsecutivoVenta, decimal MontoVenta, int Caja, System.DateTime Fecha, int RazonDescuento, int OpcionDescuento, string TipoDescuento, decimal MontoDescuento, int Linea, string folioCupon) {
            this.Tienda = Tienda;
            this.ConsecutivoVenta = ConsecutivoVenta;
            this.MontoVenta = MontoVenta;
            this.Caja = Caja;
            this.Fecha = Fecha;
            this.RazonDescuento = RazonDescuento;
            this.OpcionDescuento = OpcionDescuento;
            this.TipoDescuento = TipoDescuento;
            this.MontoDescuento = MontoDescuento;
            this.Linea = Linea;
            this.folioCupon = folioCupon;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getAutorizationV2Response {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getAutorizationV2Response", Namespace="http://tempuri.org/", Order=0)]
        public Milano.BackEnd.Business.ProxyInfoDescuento.getAutorizationV2ResponseBody Body;
        
        public getAutorizationV2Response() {
        }
        
        public getAutorizationV2Response(Milano.BackEnd.Business.ProxyInfoDescuento.getAutorizationV2ResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getAutorizationV2ResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public Milano.BackEnd.Business.ProxyInfoDescuento.WsPosResponseModel getAutorizationV2Result;
        
        public getAutorizationV2ResponseBody() {
        }
        
        public getAutorizationV2ResponseBody(Milano.BackEnd.Business.ProxyInfoDescuento.WsPosResponseModel getAutorizationV2Result) {
            this.getAutorizationV2Result = getAutorizationV2Result;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface InfoDescuentoSoapChannel : Milano.BackEnd.Business.ProxyInfoDescuento.InfoDescuentoSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class InfoDescuentoSoapClient : System.ServiceModel.ClientBase<Milano.BackEnd.Business.ProxyInfoDescuento.InfoDescuentoSoap>, Milano.BackEnd.Business.ProxyInfoDescuento.InfoDescuentoSoap {
        
        public InfoDescuentoSoapClient() {
        }
        
        public InfoDescuentoSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public InfoDescuentoSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public InfoDescuentoSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public InfoDescuentoSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Milano.BackEnd.Business.ProxyInfoDescuento.getAutorizationResponse Milano.BackEnd.Business.ProxyInfoDescuento.InfoDescuentoSoap.getAutorization(Milano.BackEnd.Business.ProxyInfoDescuento.getAutorizationRequest request) {
            return base.Channel.getAutorization(request);
        }
        
        public Milano.BackEnd.Business.ProxyInfoDescuento.WsPosResponseModel getAutorization(int Tienda, string ConsecutivoVenta, decimal MontoVenta, int Caja, System.DateTime Fecha, int RazonDescuento, int OpcionDescuento, string TipoDescuento, decimal MontoDescuento, int Linea) {
            Milano.BackEnd.Business.ProxyInfoDescuento.getAutorizationRequest inValue = new Milano.BackEnd.Business.ProxyInfoDescuento.getAutorizationRequest();
            inValue.Body = new Milano.BackEnd.Business.ProxyInfoDescuento.getAutorizationRequestBody();
            inValue.Body.Tienda = Tienda;
            inValue.Body.ConsecutivoVenta = ConsecutivoVenta;
            inValue.Body.MontoVenta = MontoVenta;
            inValue.Body.Caja = Caja;
            inValue.Body.Fecha = Fecha;
            inValue.Body.RazonDescuento = RazonDescuento;
            inValue.Body.OpcionDescuento = OpcionDescuento;
            inValue.Body.TipoDescuento = TipoDescuento;
            inValue.Body.MontoDescuento = MontoDescuento;
            inValue.Body.Linea = Linea;
            Milano.BackEnd.Business.ProxyInfoDescuento.getAutorizationResponse retVal = ((Milano.BackEnd.Business.ProxyInfoDescuento.InfoDescuentoSoap)(this)).getAutorization(inValue);
            return retVal.Body.getAutorizationResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Milano.BackEnd.Business.ProxyInfoDescuento.getAutorizationV2Response Milano.BackEnd.Business.ProxyInfoDescuento.InfoDescuentoSoap.getAutorizationV2(Milano.BackEnd.Business.ProxyInfoDescuento.getAutorizationV2Request request) {
            return base.Channel.getAutorizationV2(request);
        }
        
        public Milano.BackEnd.Business.ProxyInfoDescuento.WsPosResponseModel getAutorizationV2(int Tienda, string ConsecutivoVenta, decimal MontoVenta, int Caja, System.DateTime Fecha, int RazonDescuento, int OpcionDescuento, string TipoDescuento, decimal MontoDescuento, int Linea, string folioCupon) {
            Milano.BackEnd.Business.ProxyInfoDescuento.getAutorizationV2Request inValue = new Milano.BackEnd.Business.ProxyInfoDescuento.getAutorizationV2Request();
            inValue.Body = new Milano.BackEnd.Business.ProxyInfoDescuento.getAutorizationV2RequestBody();
            inValue.Body.Tienda = Tienda;
            inValue.Body.ConsecutivoVenta = ConsecutivoVenta;
            inValue.Body.MontoVenta = MontoVenta;
            inValue.Body.Caja = Caja;
            inValue.Body.Fecha = Fecha;
            inValue.Body.RazonDescuento = RazonDescuento;
            inValue.Body.OpcionDescuento = OpcionDescuento;
            inValue.Body.TipoDescuento = TipoDescuento;
            inValue.Body.MontoDescuento = MontoDescuento;
            inValue.Body.Linea = Linea;
            inValue.Body.folioCupon = folioCupon;
            Milano.BackEnd.Business.ProxyInfoDescuento.getAutorizationV2Response retVal = ((Milano.BackEnd.Business.ProxyInfoDescuento.InfoDescuentoSoap)(this)).getAutorizationV2(inValue);
            return retVal.Body.getAutorizationV2Result;
        }
    }
}