using System;
namespace ExplicitLayers
{
    /// <summary>
    /// Use to specify explicit layer this type belongs to
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
    public class LayerAttribute : Attribute
    {
        readonly string layerName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="layerName">Use only layer names configured in your .editorconfig</param>
        public LayerAttribute(string layerName)
        {
            this.layerName = layerName;
        }

        public string PositionalString
        {
            get { return layerName; }
        }
    }

}