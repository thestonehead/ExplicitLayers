namespace ExplicitLayers.Analyzers.Config
{
    class LayerConfiguration
    {
        public string Name { get; set; }
        public string[] AllowedDependencies { get; set; }
        public string[] Paths { get; set; }
    }

}
