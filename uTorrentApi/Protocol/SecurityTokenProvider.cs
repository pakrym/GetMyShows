//-----------------------------------------------------------------------
// <copyright file="SecurityTokenProvider.cs" company="Mike Davis">
//     To the extent possible under law, Mike Davis has waived all copyright and related or neighboring rights to this work.  This work is published from: United States.  See copying.txt for details.  I would appreciate credit when incorporating this work into other works.  However, you are under no legal obligation to do so.
// </copyright>
//-----------------------------------------------------------------------

namespace UTorrentAPI.Protocol
{
    using System;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Internal type")]
    [AttributeUsage(AttributeTargets.Method)]
    internal class SecurityTokenProvider : Attribute, IOperationBehavior, IContractBehavior
    {
        public static readonly string TokenOperationPropertyName = "UTorrentAPI.Protocol.SecurityTokenOperationProperty";

        private string tokenProviderOperation;

        public SecurityTokenProvider()
        {
            this.UseSecurityToken = true;
        }

        public string TokenXPath { get; set; }

        public bool UseSecurityToken { get; set; }

        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
            // No implementation necessary
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            clientOperation.Formatter = new SecurityTokenExtractor(clientOperation.Formatter, this.TokenXPath);
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            // No implementation necessary
        }

        public void Validate(OperationDescription operationDescription)
        {
            this.tokenProviderOperation = operationDescription.Name;
            operationDescription.DeclaringContract.Behaviors.Add(this);
        }

        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            // No implementation necessary
        }

        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new SecurityTokenUrlAugmentor(this.tokenProviderOperation));
        }

        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            // No implementation necessary
        }

        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
            // No implementation necessary
        }
    }
}
