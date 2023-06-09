﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Milano.BackEnd.Business.ProxyFinlag {
    using System.Data;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ProxyFinlag.VentasFinlagSoap")]
    public interface VentasFinlagSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ValidaVale", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable ValidaVale(int idDistribuidora, string FolioVale, double MontoVale, string usuario, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ConsultaVale", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable ConsultaVale(string FolioVale, string usuario, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/AplicaVale", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable AplicaVale(
                    string FolioVale, 
                    int idDistribuidora, 
                    decimal MontoVale, 
                    int Quincenas, 
                    string TipoPago, 
                    int IdCajaAplica, 
                    int TiendaAplica, 
                    System.DateTime FechaNacimiento, 
                    string INE, 
                    string Nombre, 
                    string Apaterno, 
                    string Amaterno, 
                    string Calle, 
                    string NumExt, 
                    string Colonia, 
                    string Estado, 
                    string Municipio, 
                    string CP, 
                    string Sexo, 
                    string Descripcion, 
                    string usuario, 
                    string password, 
                    string puntosUtilizados, 
                    string efectivoPuntos);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CancelaAplicaVale", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string CancelaAplicaVale(string foliovale, string FolioVenta, string motivo, string usuario, string password, string passCancelacion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/TablaAmortizacion", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable TablaAmortizacion(int idDistribuidora, string folioVale, double montoVenta, string Nombre, string Apaterno, string Amaterno, System.DateTime FechaNacimiento, string Calle, string NumExt, string Colonia, string CP, string Municipio, string Estado, string usuario, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/TramaImpresion", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable TramaImpresion(string FolioVale, string usuario, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ConsultaCliente", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable ConsultaCliente(string Nombre, string Apaterno, string Amaterno, string FechaNacimiento, string usuario, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ConsultaMovimientos", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable ConsultaMovimientos(int IdTienda, System.DateTime FechaInicio, System.DateTime FechaFinal, string usuario, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ConsultaMovimientosPuntosDV", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable ConsultaMovimientosPuntosDV(System.DateTime FechaInicio, System.DateTime FechaFinal, string usuario, string password);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface VentasFinlagSoapChannel : Milano.BackEnd.Business.ProxyFinlag.VentasFinlagSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class VentasFinlagSoapClient : System.ServiceModel.ClientBase<Milano.BackEnd.Business.ProxyFinlag.VentasFinlagSoap>, Milano.BackEnd.Business.ProxyFinlag.VentasFinlagSoap {
        
        public VentasFinlagSoapClient() {
        }
        
        public VentasFinlagSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public VentasFinlagSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public VentasFinlagSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public VentasFinlagSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Data.DataTable ValidaVale(int idDistribuidora, string FolioVale, double MontoVale, string usuario, string password) {
            return base.Channel.ValidaVale(idDistribuidora, FolioVale, MontoVale, usuario, password);
        }
        
        public System.Data.DataTable ConsultaVale(string FolioVale, string usuario, string password) {
            return base.Channel.ConsultaVale(FolioVale, usuario, password);
        }
        
        public System.Data.DataTable AplicaVale(
                    string FolioVale, 
                    int idDistribuidora, 
                    decimal MontoVale, 
                    int Quincenas, 
                    string TipoPago, 
                    int IdCajaAplica, 
                    int TiendaAplica, 
                    System.DateTime FechaNacimiento, 
                    string INE, 
                    string Nombre, 
                    string Apaterno, 
                    string Amaterno, 
                    string Calle, 
                    string NumExt, 
                    string Colonia, 
                    string Estado, 
                    string Municipio, 
                    string CP, 
                    string Sexo, 
                    string Descripcion, 
                    string usuario, 
                    string password, 
                    string puntosUtilizados, 
                    string efectivoPuntos) {
            return base.Channel.AplicaVale(FolioVale, idDistribuidora, MontoVale, Quincenas, TipoPago, IdCajaAplica, TiendaAplica, FechaNacimiento, INE, Nombre, Apaterno, Amaterno, Calle, NumExt, Colonia, Estado, Municipio, CP, Sexo, Descripcion, usuario, password, puntosUtilizados, efectivoPuntos);
        }
        
        public string CancelaAplicaVale(string foliovale, string FolioVenta, string motivo, string usuario, string password, string passCancelacion) {
            return base.Channel.CancelaAplicaVale(foliovale, FolioVenta, motivo, usuario, password, passCancelacion);
        }
        
        public System.Data.DataTable TablaAmortizacion(int idDistribuidora, string folioVale, double montoVenta, string Nombre, string Apaterno, string Amaterno, System.DateTime FechaNacimiento, string Calle, string NumExt, string Colonia, string CP, string Municipio, string Estado, string usuario, string password) {
            return base.Channel.TablaAmortizacion(idDistribuidora, folioVale, montoVenta, Nombre, Apaterno, Amaterno, FechaNacimiento, Calle, NumExt, Colonia, CP, Municipio, Estado, usuario, password);
        }
        
        public System.Data.DataTable TramaImpresion(string FolioVale, string usuario, string password) {
            return base.Channel.TramaImpresion(FolioVale, usuario, password);
        }
        
        public System.Data.DataTable ConsultaCliente(string Nombre, string Apaterno, string Amaterno, string FechaNacimiento, string usuario, string password) {
            return base.Channel.ConsultaCliente(Nombre, Apaterno, Amaterno, FechaNacimiento, usuario, password);
        }
        
        public System.Data.DataTable ConsultaMovimientos(int IdTienda, System.DateTime FechaInicio, System.DateTime FechaFinal, string usuario, string password) {
            return base.Channel.ConsultaMovimientos(IdTienda, FechaInicio, FechaFinal, usuario, password);
        }
        
        public System.Data.DataTable ConsultaMovimientosPuntosDV(System.DateTime FechaInicio, System.DateTime FechaFinal, string usuario, string password) {
            return base.Channel.ConsultaMovimientosPuntosDV(FechaInicio, FechaFinal, usuario, password);
        }
    }
}
