﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace M_Platform.ServiceIportUser {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceIportUser.WebServiceUserSoap")]
    public interface WebServiceUserSoap {
        
        // CODEGEN: 命名空间 http://tempuri.org/ 的元素名称 xmlParams 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Add", ReplyAction="*")]
        M_Platform.ServiceIportUser.AddResponse Add(M_Platform.ServiceIportUser.AddRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Add", ReplyAction="*")]
        System.Threading.Tasks.Task<M_Platform.ServiceIportUser.AddResponse> AddAsync(M_Platform.ServiceIportUser.AddRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class AddRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Add", Namespace="http://tempuri.org/", Order=0)]
        public M_Platform.ServiceIportUser.AddRequestBody Body;
        
        public AddRequest() {
        }
        
        public AddRequest(M_Platform.ServiceIportUser.AddRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class AddRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string xmlParams;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=1)]
        public int retType;
        
        public AddRequestBody() {
        }
        
        public AddRequestBody(string xmlParams, int retType) {
            this.xmlParams = xmlParams;
            this.retType = retType;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class AddResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="AddResponse", Namespace="http://tempuri.org/", Order=0)]
        public M_Platform.ServiceIportUser.AddResponseBody Body;
        
        public AddResponse() {
        }
        
        public AddResponse(M_Platform.ServiceIportUser.AddResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class AddResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string AddResult;
        
        public AddResponseBody() {
        }
        
        public AddResponseBody(string AddResult) {
            this.AddResult = AddResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface WebServiceUserSoapChannel : M_Platform.ServiceIportUser.WebServiceUserSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WebServiceUserSoapClient : System.ServiceModel.ClientBase<M_Platform.ServiceIportUser.WebServiceUserSoap>, M_Platform.ServiceIportUser.WebServiceUserSoap {
        
        public WebServiceUserSoapClient() {
        }
        
        public WebServiceUserSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WebServiceUserSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WebServiceUserSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WebServiceUserSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        M_Platform.ServiceIportUser.AddResponse M_Platform.ServiceIportUser.WebServiceUserSoap.Add(M_Platform.ServiceIportUser.AddRequest request) {
            return base.Channel.Add(request);
        }
        
        public string Add(string xmlParams, int retType) {
            M_Platform.ServiceIportUser.AddRequest inValue = new M_Platform.ServiceIportUser.AddRequest();
            inValue.Body = new M_Platform.ServiceIportUser.AddRequestBody();
            inValue.Body.xmlParams = xmlParams;
            inValue.Body.retType = retType;
            M_Platform.ServiceIportUser.AddResponse retVal = ((M_Platform.ServiceIportUser.WebServiceUserSoap)(this)).Add(inValue);
            return retVal.Body.AddResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<M_Platform.ServiceIportUser.AddResponse> M_Platform.ServiceIportUser.WebServiceUserSoap.AddAsync(M_Platform.ServiceIportUser.AddRequest request) {
            return base.Channel.AddAsync(request);
        }
        
        public System.Threading.Tasks.Task<M_Platform.ServiceIportUser.AddResponse> AddAsync(string xmlParams, int retType) {
            M_Platform.ServiceIportUser.AddRequest inValue = new M_Platform.ServiceIportUser.AddRequest();
            inValue.Body = new M_Platform.ServiceIportUser.AddRequestBody();
            inValue.Body.xmlParams = xmlParams;
            inValue.Body.retType = retType;
            return ((M_Platform.ServiceIportUser.WebServiceUserSoap)(this)).AddAsync(inValue);
        }
    }
}