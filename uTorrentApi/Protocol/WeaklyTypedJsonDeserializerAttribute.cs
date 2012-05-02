//-----------------------------------------------------------------------
// <copyright file="WeaklyTypedJsonDeserializerAttribute.cs" company="Mike Davis">
//     To the extent possible under law, Mike Davis has waived all copyright and related or neighboring rights to this work.  This work is published from: United States.  See copying.txt for details.  I would appreciate credit when incorporating this work into other works.  However, you are under no legal obligation to do so.
// </copyright>
//-----------------------------------------------------------------------

namespace UTorrentAPI.Protocol
{
    using System;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    /// <summary>
    /// Used to specify that the return type on the annotated operation
    /// should be deserialized by calling the LoadFromJson method.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Internal type")]
    [AttributeUsage(AttributeTargets.Method)]
    internal class WeaklyTypedJsonDeserializerAttribute : Attribute, IOperationBehavior
    {
        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
            // No implementation necessary
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            clientOperation.Formatter = new WeaklyTypedJsonDeserializer(clientOperation.Formatter, clientOperation.SyncMethod.ReturnType);
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            // No implementation necessary
        }

        public void Validate(OperationDescription operationDescription)
        {
            // No implementation necessary
        }
    }
}
