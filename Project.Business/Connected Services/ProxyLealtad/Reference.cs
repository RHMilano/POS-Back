﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Milano.BackEnd.Business.ProxyLealtad {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="RespuestaRegistrarCliente", Namespace="http://tempuri.org/")]
    [System.SerializableAttribute()]
    public partial class RespuestaRegistrarCliente : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private bool bErrorField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string sMensajeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string sNivelField;
        
        private bool bPrimeraCompraField;
        
        private int iCodigoClienteField;
        
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
        public bool bError {
            get {
                return this.bErrorField;
            }
            set {
                if ((this.bErrorField.Equals(value) != true)) {
                    this.bErrorField = value;
                    this.RaisePropertyChanged("bError");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
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
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
        public string sNivel {
            get {
                return this.sNivelField;
            }
            set {
                if ((object.ReferenceEquals(this.sNivelField, value) != true)) {
                    this.sNivelField = value;
                    this.RaisePropertyChanged("sNivel");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=3)]
        public bool bPrimeraCompra {
            get {
                return this.bPrimeraCompraField;
            }
            set {
                if ((this.bPrimeraCompraField.Equals(value) != true)) {
                    this.bPrimeraCompraField = value;
                    this.RaisePropertyChanged("bPrimeraCompra");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=4)]
        public int iCodigoCliente {
            get {
                return this.iCodigoClienteField;
            }
            set {
                if ((this.iCodigoClienteField.Equals(value) != true)) {
                    this.iCodigoClienteField = value;
                    this.RaisePropertyChanged("iCodigoCliente");
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="RespuestaConsultarCliente", Namespace="http://tempuri.org/")]
    [System.SerializableAttribute()]
    public partial class RespuestaConsultarCliente : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string sMensajeErrorField;
        
        private int iCantidadClientesField;
        
        private bool bCantidadLimitadaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Milano.BackEnd.Business.ProxyLealtad.InfoClientesCRM[] infoClientesCRMsField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
        public string sMensajeError {
            get {
                return this.sMensajeErrorField;
            }
            set {
                if ((object.ReferenceEquals(this.sMensajeErrorField, value) != true)) {
                    this.sMensajeErrorField = value;
                    this.RaisePropertyChanged("sMensajeError");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=1)]
        public int iCantidadClientes {
            get {
                return this.iCantidadClientesField;
            }
            set {
                if ((this.iCantidadClientesField.Equals(value) != true)) {
                    this.iCantidadClientesField = value;
                    this.RaisePropertyChanged("iCantidadClientes");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=2)]
        public bool bCantidadLimitada {
            get {
                return this.bCantidadLimitadaField;
            }
            set {
                if ((this.bCantidadLimitadaField.Equals(value) != true)) {
                    this.bCantidadLimitadaField = value;
                    this.RaisePropertyChanged("bCantidadLimitada");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public Milano.BackEnd.Business.ProxyLealtad.InfoClientesCRM[] infoClientesCRMs {
            get {
                return this.infoClientesCRMsField;
            }
            set {
                if ((object.ReferenceEquals(this.infoClientesCRMsField, value) != true)) {
                    this.infoClientesCRMsField = value;
                    this.RaisePropertyChanged("infoClientesCRMs");
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="InfoClientesCRM", Namespace="http://tempuri.org/")]
    [System.SerializableAttribute()]
    public partial class InfoClientesCRM : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private int iCodigoClienteField;
        
        private int iCodigoClienteSistemaCreditoField;
        
        private int iCodigoEmpleadoField;
        
        private int iCodigoClienteWebField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string sNivelField;
        
        private bool bPrimeraCompraField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string sTelefonoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string sPaternoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string sMaternoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string sNombreField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string sGeneroField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string sFechaNacimientoField;
        
        private double dSaldoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string sFechaRegistroField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string sEmailField;
        
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
        public int iCodigoCliente {
            get {
                return this.iCodigoClienteField;
            }
            set {
                if ((this.iCodigoClienteField.Equals(value) != true)) {
                    this.iCodigoClienteField = value;
                    this.RaisePropertyChanged("iCodigoCliente");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public int iCodigoClienteSistemaCredito {
            get {
                return this.iCodigoClienteSistemaCreditoField;
            }
            set {
                if ((this.iCodigoClienteSistemaCreditoField.Equals(value) != true)) {
                    this.iCodigoClienteSistemaCreditoField = value;
                    this.RaisePropertyChanged("iCodigoClienteSistemaCredito");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public int iCodigoEmpleado {
            get {
                return this.iCodigoEmpleadoField;
            }
            set {
                if ((this.iCodigoEmpleadoField.Equals(value) != true)) {
                    this.iCodigoEmpleadoField = value;
                    this.RaisePropertyChanged("iCodigoEmpleado");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=3)]
        public int iCodigoClienteWeb {
            get {
                return this.iCodigoClienteWebField;
            }
            set {
                if ((this.iCodigoClienteWebField.Equals(value) != true)) {
                    this.iCodigoClienteWebField = value;
                    this.RaisePropertyChanged("iCodigoClienteWeb");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string sNivel {
            get {
                return this.sNivelField;
            }
            set {
                if ((object.ReferenceEquals(this.sNivelField, value) != true)) {
                    this.sNivelField = value;
                    this.RaisePropertyChanged("sNivel");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=5)]
        public bool bPrimeraCompra {
            get {
                return this.bPrimeraCompraField;
            }
            set {
                if ((this.bPrimeraCompraField.Equals(value) != true)) {
                    this.bPrimeraCompraField = value;
                    this.RaisePropertyChanged("bPrimeraCompra");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=6)]
        public string sTelefono {
            get {
                return this.sTelefonoField;
            }
            set {
                if ((object.ReferenceEquals(this.sTelefonoField, value) != true)) {
                    this.sTelefonoField = value;
                    this.RaisePropertyChanged("sTelefono");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=7)]
        public string sPaterno {
            get {
                return this.sPaternoField;
            }
            set {
                if ((object.ReferenceEquals(this.sPaternoField, value) != true)) {
                    this.sPaternoField = value;
                    this.RaisePropertyChanged("sPaterno");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=8)]
        public string sMaterno {
            get {
                return this.sMaternoField;
            }
            set {
                if ((object.ReferenceEquals(this.sMaternoField, value) != true)) {
                    this.sMaternoField = value;
                    this.RaisePropertyChanged("sMaterno");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=9)]
        public string sNombre {
            get {
                return this.sNombreField;
            }
            set {
                if ((object.ReferenceEquals(this.sNombreField, value) != true)) {
                    this.sNombreField = value;
                    this.RaisePropertyChanged("sNombre");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=10)]
        public string sGenero {
            get {
                return this.sGeneroField;
            }
            set {
                if ((object.ReferenceEquals(this.sGeneroField, value) != true)) {
                    this.sGeneroField = value;
                    this.RaisePropertyChanged("sGenero");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=11)]
        public string sFechaNacimiento {
            get {
                return this.sFechaNacimientoField;
            }
            set {
                if ((object.ReferenceEquals(this.sFechaNacimientoField, value) != true)) {
                    this.sFechaNacimientoField = value;
                    this.RaisePropertyChanged("sFechaNacimiento");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=12)]
        public double dSaldo {
            get {
                return this.dSaldoField;
            }
            set {
                if ((this.dSaldoField.Equals(value) != true)) {
                    this.dSaldoField = value;
                    this.RaisePropertyChanged("dSaldo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=13)]
        public string sFechaRegistro {
            get {
                return this.sFechaRegistroField;
            }
            set {
                if ((object.ReferenceEquals(this.sFechaRegistroField, value) != true)) {
                    this.sFechaRegistroField = value;
                    this.RaisePropertyChanged("sFechaRegistro");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=14)]
        public string sEmail {
            get {
                return this.sEmailField;
            }
            set {
                if ((object.ReferenceEquals(this.sEmailField, value) != true)) {
                    this.sEmailField = value;
                    this.RaisePropertyChanged("sEmail");
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ProxyLealtad.wsLealtadSoap")]
    public interface wsLealtadSoap {
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento sTelefono del espacio de nombres http://tempuri.org/ no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/RegistrarCliente", ReplyAction="*")]
        Milano.BackEnd.Business.ProxyLealtad.RegistrarClienteResponse RegistrarCliente(Milano.BackEnd.Business.ProxyLealtad.RegistrarClienteRequest request);
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento sTelefono del espacio de nombres http://tempuri.org/ no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ConsultarCliente", ReplyAction="*")]
        Milano.BackEnd.Business.ProxyLealtad.ConsultarClienteResponse ConsultarCliente(Milano.BackEnd.Business.ProxyLealtad.ConsultarClienteRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class RegistrarClienteRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="RegistrarCliente", Namespace="http://tempuri.org/", Order=0)]
        public Milano.BackEnd.Business.ProxyLealtad.RegistrarClienteRequestBody Body;
        
        public RegistrarClienteRequest() {
        }
        
        public RegistrarClienteRequest(Milano.BackEnd.Business.ProxyLealtad.RegistrarClienteRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class RegistrarClienteRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public int iCodigoClienteSistemaCredito;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=1)]
        public int iCodigoEmpleado;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=2)]
        public int iCodigoClienteWeb;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string sTelefono;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string sPaterno;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=5)]
        public string sMaterno;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=6)]
        public string sNombre;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=7)]
        public string sGenero;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=8)]
        public string sFechaNacimiento;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=9)]
        public string sFolioVenta;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=10)]
        public int iCodigoTiendaRegistra;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=11)]
        public int iCodigoCajaRegistra;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=12)]
        public int iCodigoEmpleadoRegistra;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=13)]
        public string sEmail;
        
        public RegistrarClienteRequestBody() {
        }
        
        public RegistrarClienteRequestBody(int iCodigoClienteSistemaCredito, int iCodigoEmpleado, int iCodigoClienteWeb, string sTelefono, string sPaterno, string sMaterno, string sNombre, string sGenero, string sFechaNacimiento, string sFolioVenta, int iCodigoTiendaRegistra, int iCodigoCajaRegistra, int iCodigoEmpleadoRegistra, string sEmail) {
            this.iCodigoClienteSistemaCredito = iCodigoClienteSistemaCredito;
            this.iCodigoEmpleado = iCodigoEmpleado;
            this.iCodigoClienteWeb = iCodigoClienteWeb;
            this.sTelefono = sTelefono;
            this.sPaterno = sPaterno;
            this.sMaterno = sMaterno;
            this.sNombre = sNombre;
            this.sGenero = sGenero;
            this.sFechaNacimiento = sFechaNacimiento;
            this.sFolioVenta = sFolioVenta;
            this.iCodigoTiendaRegistra = iCodigoTiendaRegistra;
            this.iCodigoCajaRegistra = iCodigoCajaRegistra;
            this.iCodigoEmpleadoRegistra = iCodigoEmpleadoRegistra;
            this.sEmail = sEmail;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class RegistrarClienteResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="RegistrarClienteResponse", Namespace="http://tempuri.org/", Order=0)]
        public Milano.BackEnd.Business.ProxyLealtad.RegistrarClienteResponseBody Body;
        
        public RegistrarClienteResponse() {
        }
        
        public RegistrarClienteResponse(Milano.BackEnd.Business.ProxyLealtad.RegistrarClienteResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class RegistrarClienteResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public Milano.BackEnd.Business.ProxyLealtad.RespuestaRegistrarCliente RegistrarClienteResult;
        
        public RegistrarClienteResponseBody() {
        }
        
        public RegistrarClienteResponseBody(Milano.BackEnd.Business.ProxyLealtad.RespuestaRegistrarCliente RegistrarClienteResult) {
            this.RegistrarClienteResult = RegistrarClienteResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ConsultarClienteRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ConsultarCliente", Namespace="http://tempuri.org/", Order=0)]
        public Milano.BackEnd.Business.ProxyLealtad.ConsultarClienteRequestBody Body;
        
        public ConsultarClienteRequest() {
        }
        
        public ConsultarClienteRequest(Milano.BackEnd.Business.ProxyLealtad.ConsultarClienteRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class ConsultarClienteRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public int iCodigoCliente;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=1)]
        public int iCodigoClienteSistemaCredito;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=2)]
        public int iCodigoEmpleado;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=3)]
        public int iCodigoClienteWeb;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string sTelefono;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=5)]
        public string sPaterno;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=6)]
        public string sMaterno;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=7)]
        public string sNombre;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=8)]
        public string sFechaNacimiento;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=9)]
        public int iCodigoTiendaRegistro;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=10)]
        public string sEmail;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=11)]
        public int iCodigoTienda;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=12)]
        public int iCodigoCaja;
        
        public ConsultarClienteRequestBody() {
        }
        
        public ConsultarClienteRequestBody(int iCodigoCliente, int iCodigoClienteSistemaCredito, int iCodigoEmpleado, int iCodigoClienteWeb, string sTelefono, string sPaterno, string sMaterno, string sNombre, string sFechaNacimiento, int iCodigoTiendaRegistro, string sEmail, int iCodigoTienda, int iCodigoCaja) {
            this.iCodigoCliente = iCodigoCliente;
            this.iCodigoClienteSistemaCredito = iCodigoClienteSistemaCredito;
            this.iCodigoEmpleado = iCodigoEmpleado;
            this.iCodigoClienteWeb = iCodigoClienteWeb;
            this.sTelefono = sTelefono;
            this.sPaterno = sPaterno;
            this.sMaterno = sMaterno;
            this.sNombre = sNombre;
            this.sFechaNacimiento = sFechaNacimiento;
            this.iCodigoTiendaRegistro = iCodigoTiendaRegistro;
            this.sEmail = sEmail;
            this.iCodigoTienda = iCodigoTienda;
            this.iCodigoCaja = iCodigoCaja;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ConsultarClienteResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ConsultarClienteResponse", Namespace="http://tempuri.org/", Order=0)]
        public Milano.BackEnd.Business.ProxyLealtad.ConsultarClienteResponseBody Body;
        
        public ConsultarClienteResponse() {
        }
        
        public ConsultarClienteResponse(Milano.BackEnd.Business.ProxyLealtad.ConsultarClienteResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class ConsultarClienteResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public Milano.BackEnd.Business.ProxyLealtad.RespuestaConsultarCliente ConsultarClienteResult;
        
        public ConsultarClienteResponseBody() {
        }
        
        public ConsultarClienteResponseBody(Milano.BackEnd.Business.ProxyLealtad.RespuestaConsultarCliente ConsultarClienteResult) {
            this.ConsultarClienteResult = ConsultarClienteResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface wsLealtadSoapChannel : Milano.BackEnd.Business.ProxyLealtad.wsLealtadSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class wsLealtadSoapClient : System.ServiceModel.ClientBase<Milano.BackEnd.Business.ProxyLealtad.wsLealtadSoap>, Milano.BackEnd.Business.ProxyLealtad.wsLealtadSoap {
        
        public wsLealtadSoapClient() {
        }
        
        public wsLealtadSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public wsLealtadSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public wsLealtadSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public wsLealtadSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Milano.BackEnd.Business.ProxyLealtad.RegistrarClienteResponse Milano.BackEnd.Business.ProxyLealtad.wsLealtadSoap.RegistrarCliente(Milano.BackEnd.Business.ProxyLealtad.RegistrarClienteRequest request) {
            return base.Channel.RegistrarCliente(request);
        }
        
        public Milano.BackEnd.Business.ProxyLealtad.RespuestaRegistrarCliente RegistrarCliente(int iCodigoClienteSistemaCredito, int iCodigoEmpleado, int iCodigoClienteWeb, string sTelefono, string sPaterno, string sMaterno, string sNombre, string sGenero, string sFechaNacimiento, string sFolioVenta, int iCodigoTiendaRegistra, int iCodigoCajaRegistra, int iCodigoEmpleadoRegistra, string sEmail) {
            Milano.BackEnd.Business.ProxyLealtad.RegistrarClienteRequest inValue = new Milano.BackEnd.Business.ProxyLealtad.RegistrarClienteRequest();
            inValue.Body = new Milano.BackEnd.Business.ProxyLealtad.RegistrarClienteRequestBody();
            inValue.Body.iCodigoClienteSistemaCredito = iCodigoClienteSistemaCredito;
            inValue.Body.iCodigoEmpleado = iCodigoEmpleado;
            inValue.Body.iCodigoClienteWeb = iCodigoClienteWeb;
            inValue.Body.sTelefono = sTelefono;
            inValue.Body.sPaterno = sPaterno;
            inValue.Body.sMaterno = sMaterno;
            inValue.Body.sNombre = sNombre;
            inValue.Body.sGenero = sGenero;
            inValue.Body.sFechaNacimiento = sFechaNacimiento;
            inValue.Body.sFolioVenta = sFolioVenta;
            inValue.Body.iCodigoTiendaRegistra = iCodigoTiendaRegistra;
            inValue.Body.iCodigoCajaRegistra = iCodigoCajaRegistra;
            inValue.Body.iCodigoEmpleadoRegistra = iCodigoEmpleadoRegistra;
            inValue.Body.sEmail = sEmail;
            Milano.BackEnd.Business.ProxyLealtad.RegistrarClienteResponse retVal = ((Milano.BackEnd.Business.ProxyLealtad.wsLealtadSoap)(this)).RegistrarCliente(inValue);
            return retVal.Body.RegistrarClienteResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Milano.BackEnd.Business.ProxyLealtad.ConsultarClienteResponse Milano.BackEnd.Business.ProxyLealtad.wsLealtadSoap.ConsultarCliente(Milano.BackEnd.Business.ProxyLealtad.ConsultarClienteRequest request) {
            return base.Channel.ConsultarCliente(request);
        }
        
        public Milano.BackEnd.Business.ProxyLealtad.RespuestaConsultarCliente ConsultarCliente(int iCodigoCliente, int iCodigoClienteSistemaCredito, int iCodigoEmpleado, int iCodigoClienteWeb, string sTelefono, string sPaterno, string sMaterno, string sNombre, string sFechaNacimiento, int iCodigoTiendaRegistro, string sEmail, int iCodigoTienda, int iCodigoCaja) {
            Milano.BackEnd.Business.ProxyLealtad.ConsultarClienteRequest inValue = new Milano.BackEnd.Business.ProxyLealtad.ConsultarClienteRequest();
            inValue.Body = new Milano.BackEnd.Business.ProxyLealtad.ConsultarClienteRequestBody();
            inValue.Body.iCodigoCliente = iCodigoCliente;
            inValue.Body.iCodigoClienteSistemaCredito = iCodigoClienteSistemaCredito;
            inValue.Body.iCodigoEmpleado = iCodigoEmpleado;
            inValue.Body.iCodigoClienteWeb = iCodigoClienteWeb;
            inValue.Body.sTelefono = sTelefono;
            inValue.Body.sPaterno = sPaterno;
            inValue.Body.sMaterno = sMaterno;
            inValue.Body.sNombre = sNombre;
            inValue.Body.sFechaNacimiento = sFechaNacimiento;
            inValue.Body.iCodigoTiendaRegistro = iCodigoTiendaRegistro;
            inValue.Body.sEmail = sEmail;
            inValue.Body.iCodigoTienda = iCodigoTienda;
            inValue.Body.iCodigoCaja = iCodigoCaja;
            Milano.BackEnd.Business.ProxyLealtad.ConsultarClienteResponse retVal = ((Milano.BackEnd.Business.ProxyLealtad.wsLealtadSoap)(this)).ConsultarCliente(inValue);
            return retVal.Body.ConsultarClienteResult;
        }
    }
}
