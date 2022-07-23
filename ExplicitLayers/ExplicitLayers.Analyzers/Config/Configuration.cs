using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ExplicitLayers.Analyzers.Config
{
    class Configuration
    {
        public ICollection<LayerConfiguration> LayerConfigurations { get; private set; }

        public Configuration(ICollection<LayerConfiguration> layerConfigurations)
        {
            LayerConfigurations = layerConfigurations;
        }

        public bool IsDependencyAllowed(string callingLayer, string referencedLayer)
        {
            var config = LayerConfigurations.FirstOrDefault(l => l.Name == callingLayer);
            if (config == null)
                return true;
            return config.AllowedDependencies?.Contains(referencedLayer) == true;
        }

        public string GetLayerNameByPath(string path)
        {
            return LayerConfigurations.FirstOrDefault(l => l.Paths?.Any(p => Regex.IsMatch(path, p, RegexOptions.IgnoreCase)) == true)?.Name;
        }

        public static Configuration ReadLayerConfiguration(SemanticModelAnalysisContext context)
        {
            try
            {
                var config = context.Options.AnalyzerConfigOptionsProvider.GetOptions(context.SemanticModel.SyntaxTree);

                if (!config.TryGetValue("dotnet_diagnostic.ExplicitLayers.comma_separated_layer_names", out var layerNames))
                    return null;

                var result = new List<LayerConfiguration>();
                foreach (var layerName in layerNames.Split(','))
                {
                    if (string.IsNullOrWhiteSpace(layerName))
                        continue;
                    var item = new LayerConfiguration();
                    item.Name = layerName;
                    if (config.TryGetValue($"dotnet_diagnostic.ExplicitLayers.{layerName}.comma_separated_allowed_dependencies", out var allowedDependencies))
                    {
                        item.AllowedDependencies = allowedDependencies.Split(',').Where(d => !string.IsNullOrWhiteSpace(d)).ToArray();
                    }
                    if (config.TryGetValue($"dotnet_diagnostic.ExplicitLayers.{layerName}.comma_separated_regex_paths", out var paths))
                    {
                        item.Paths = paths.Split(',').Where(d => !string.IsNullOrWhiteSpace(d)).ToArray();
                    }
                    result.Add(item);
                }
                return new Configuration(result);
            }
            catch
            {
                return null;
            }
        }

        public static Configuration Default
        {
            get
            {
                return new Configuration(new LayerConfiguration[]
                {
                    new LayerConfiguration()
                    {
                        Name = "Domain"
                    },
                    new LayerConfiguration()
                    {
                        Name = "Infrastructure",
                        AllowedDependencies = new string[] { "Domain" }
                    },
                    new LayerConfiguration()
                    {
                        Name = "Web",
                        AllowedDependencies = new string[] { "Domain", "Infrastructure"},
                        Paths = new string[] { @".*\\Web\\.*" }
                    }
                });
            }
        }
    }

}
