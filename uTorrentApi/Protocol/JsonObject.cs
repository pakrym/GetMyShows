//-----------------------------------------------------------------------
// <copyright file="JsonObject.cs" company="Microsoft Corporation">
//     From Microsoft WCF samples project (WeaklyTypedJson.cs) with edits 
//     by Mike Davis.  Original code license states:
//     You may modify, copy, and distribute the source and object code form of code marked as “sample.”
// </copyright>
//-----------------------------------------------------------------------

namespace UTorrentAPI.Protocol
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization.Json;
    using System.Xml;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "This is mostly external code")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Internal type")]
    [Serializable]
    internal class JsonObject : JsonCollection
    {
        public JsonObject(XmlReader reader)
            : base(reader, new Dictionary<string, JsonBaseType>()) 
        { 
        }

        // In an object (dictionary), the [string] indexer is appropriate, so implement it.
        // The [int] indexer will throw as per JsonBaseType.
        public override JsonBaseType this[string key]
        {
            get
            {
                JsonBaseType val;
                if (((Dictionary<string, JsonBaseType>)InternalValue).TryGetValue(key, out val))
                {
                    return val;
                }

                return null;
            }

            set
            {
                ((Dictionary<string, JsonBaseType>)InternalValue)[key] = (JsonBaseType)value;
            }
        }

        // Implement the correct add method for an object (dictionary)
        internal void Add(string key, JsonBaseType value)
        {
            ((Dictionary<string, JsonBaseType>)InternalValue).Add(key, value);
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "This is mostly external code")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Internal type")]
    [Serializable]
    internal class JsonArray : JsonCollection
    {
        internal JsonArray(XmlReader reader)
            : base(reader, new List<JsonBaseType>())
        {        
        }

        public int Count
        {
            get
            {
                return ((List<JsonBaseType>)this.InternalValue).Count;
            }
        }

        // In an array, the [int] indexer is appropriate, so implement it.
        // The [string] indexer will throw as per JsonBaseType.
        public override JsonBaseType this[int index]
        {
            get
            {
                return ((List<JsonBaseType>)InternalValue)[index];
            }

            set
            {
                ((List<JsonBaseType>)InternalValue)[index] = (JsonBaseType)value;
            }
        }
        
        // Implement the correct add method for an array
        internal void Add(JsonBaseType value)
        {
            ((List<JsonBaseType>)InternalValue).Add(value);
        }
    }

    // JSON contains two collection types: an object and an array. This class abstracts some 
    // common functionality used by both.
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "This is mostly external code")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Internal type")]
    [Serializable]
    internal class JsonCollection : JsonBaseType
    {
        private const string ExceptionString = "You cannot use this Add method on this JSON type";

        internal JsonCollection(XmlReader reader, object value)
            : base(value)
        {
            string nodeName, nodeType;
            string rootName = reader.Name;

            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    nodeName = reader.Name;
                    reader.MoveToAttribute("type");
                    nodeType = reader.Value;
                    reader.MoveToElement();

                    switch (nodeType)
                    {
                        // The object JSON type needs to be handled by us recursively, since 
                        // DataContractJsonSerializer cannot deserialize it without a DataContract, 
                        // which we don't have
                        case "object":
                            this.AddSelector(nodeName, new JsonObject(reader));
                            break;

                        // Normally DataContractJson serializer can deserialize arrays, but we could have
                        // an array with an object as one if its items. In that case the serializer
                        // wouldn't work, so we need to handle the entire array case manually
                        case "array":
                            this.AddSelector(nodeName, new JsonArray(reader));
                            break;

                        // The number, string, and bool JSON types can be deferred to 
                        // DataContractJsonSerializer
                        case "number":
                            this.AddValueType<double>(reader);
                            break;

                        case "string":
                            this.AddValueType<string>(reader);
                            break;

                        case "boolean":
                            this.AddValueType<bool>(reader);
                            break;

                        // For null values, we just use the CLR null type
                        case "null":
                            this.AddSelector(nodeName, null);
                            break;
                    }
                }
                else if (reader.Name == rootName)
                {
                    break;
                }
            }
        }

        // We will be working with one if this class' children - object or array - so we need to 
        // use the add method that's appropriate for that child
        private void AddSelector(string key, JsonBaseType value)
        {
            JsonArray thisArray = this as JsonArray;

            if (thisArray != null)
            {
                thisArray.Add(value);
            }
            else
            {
                ((JsonObject)this).Add(key, value);
            }
        }

        private void AddValueType<T>(XmlReader reader)
        {
            // Use DataContractJsonSerialzier to deserialize JXML into a CLR type
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T), reader.Name);
            JsonBaseType jsonElement = new JsonBaseType((T)serializer.ReadObject(reader.ReadSubtree()));
            this.AddSelector(reader.Name, jsonElement);
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "This is mostly external code")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Internal type")]
    [Serializable]
    internal class JsonBaseType
    {
        private const string ExceptionString = "You cannot use indexers on this JSON type";

        // Constructor
        internal JsonBaseType(object value)
        {
            this.InternalValue = value;
        }

        protected object InternalValue { get; set; }

        // The generic JSON type needs to support indexers of both [string] and [int] (for the JSON
        // object and array) and also cast easily into int, bool, and string (for the JSON number,
        // boolean, and string types)

        // Indexers
        public virtual JsonBaseType this[string key]
        {
            get
            {
                throw new NotSupportedException(ExceptionString);
            }

            set
            {
                throw new NotSupportedException(ExceptionString);
            }
        }

        public virtual JsonBaseType this[int index]
        {
            get
            {
                throw new NotSupportedException(ExceptionString);
            }

            set
            {
                throw new NotSupportedException(ExceptionString);
            }
        }

        // Type cast operator overloads
        public static implicit operator int(JsonBaseType type)
        {
            // Have to do this to unbox correctly
            return Convert.ToInt32(type.InternalValue);
        }

        public static implicit operator double(JsonBaseType type)
        {
            // Have to do this to unbox correctly
            return Convert.ToDouble(type.InternalValue);
        }

        public static implicit operator bool(JsonBaseType type)
        {
            return Convert.ToBoolean(type.InternalValue);
        }

        public static implicit operator string(JsonBaseType type)
        {
            return Convert.ToString(type.InternalValue);
        }

        public static implicit operator long(JsonBaseType type)
        {
            return Convert.ToInt64(type.InternalValue);
        }
    }
}
