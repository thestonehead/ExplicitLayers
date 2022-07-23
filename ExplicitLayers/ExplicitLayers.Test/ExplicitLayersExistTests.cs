using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ExplicitLayers.Analyzers;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VerifyCS = ExplicitLayers.Test.CSharpAnalyzerVerifier<
    ExplicitLayers.Analyzers.ExplicitLayerExistAnalyzer>;

namespace ExplicitLayers.Test
{
    [TestClass]
    public class ExplicitLayersExistTests
    {
        private const string AttributeCodePath = @"..\..\..\..\ExplicitLayers\LayerAttribute.cs";
        private const string AnalyserName = ExplicitLayerExistAnalyzer.DiagnosticId;

        [TestMethod]
        public async Task ExplicitLayersExists_LayerDoesntExist()
        {

            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        [Layer(""Unknown"")]
        class TypeA
        {   
        }
    }";
            var expected = VerifyCS.Diagnostic(AnalyserName).WithSpan(11, 10, 11, 26).WithArguments("Unknown");
            await VerifyCS.VerifyAnalyzerAsync(test + File.ReadAllText(AttributeCodePath), expected);

        }

        [TestMethod]
        public async Task ExplicitLayersExists_LayerIsEmpty()
        {

            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        [Layer("""")]
        class TypeA
        {   
        }
    }";
            var expected = VerifyCS.Diagnostic(AnalyserName).WithSpan(11, 10, 11, 19).WithArguments("");
            await VerifyCS.VerifyAnalyzerAsync(test + File.ReadAllText(AttributeCodePath), expected);

        }

        [TestMethod]
        public async Task ExplicitLayersExists_LayerIsNotSpecified()
        {

            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TypeA
        {   
        }
    }";
            await VerifyCS.VerifyAnalyzerAsync(test + File.ReadAllText(AttributeCodePath));

        }
    }
}
