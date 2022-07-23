using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;
using VerifyCS = ExplicitLayers.Test.CSharpAnalyzerVerifier<
    ExplicitLayers.Analyzers.ExplicitLayersAnalyzer>;

namespace ExplicitLayers.Test
{
    [TestClass]
    public class ExplicitLayersUnitTest
    {

        private const string AttributeCodePath = @"..\..\..\..\ExplicitLayers\LayerAttribute.cs";
        private const string AnalyserName = "ExplicitLayers";

        //No diagnostics expected to show up
        [TestMethod]
        public async Task TestMethod1()
        {
            var test = @"";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task TestFindingDependencies_WithOkAssignment()
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
        [Layer(""Domain"")]
        class TypeA
        {   
        }
        [Layer(""Domain"")]
        class TypeB 
        {
            void Method(){
                var a = new TypeA();
            }
        }
    }";
            //var expected = VerifyCS.Diagnostic("TestAnalyserRosyln").WithSpan(19, 25, 19, 36).WithArguments("TypeA", "TypeB");
            await VerifyCS.VerifyAnalyzerAsync(test + File.ReadAllText(AttributeCodePath));


        }


        [TestMethod]
        public async Task TestFindingDependencies_WithAssignment()
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
        [Layer(""Infrastructure"")]
        class TypeA
        {   
        }
        [Layer(""Domain"")]
        class TypeB 
        {
            void Method(){
                var a = new TypeA();
            }
        }
    }";
            var expected = VerifyCS.Diagnostic(AnalyserName).WithSpan(19, 25, 19, 36).WithArguments("TypeA", "Infrastructure", "TypeB", "Domain");
            await VerifyCS.VerifyAnalyzerAsync(test + File.ReadAllText(AttributeCodePath), expected);


        }

        [TestMethod]
        public async Task TestFindingDependencies_WithACall()
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
        [Layer(""Infrastructure"")]
        class TypeA
        {
            public void DoStuff() {}
        }
        [Layer(""Domain"")]
        class TypeB 
        {
            void Method(TypeA a){
                a.DoStuff();
            }
        }
    }";
            var expected = VerifyCS.Diagnostic(AnalyserName).WithSpan(20, 17, 20, 26).WithArguments("TypeA", "Infrastructure", "TypeB", "Domain");
            await VerifyCS.VerifyAnalyzerAsync(test + File.ReadAllText(AttributeCodePath), expected);
        }

        [TestMethod]
        public async Task TestFindingDependencies_WithACallToInterface()
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
        interface ITypeA {
            void DoStuff();
        }
        [Layer(""Web"")]
        class TypeA : ITypeA
        {
            public void DoStuff() {}
        }
        [Layer(""Application"")]
        class TypeB 
        {
            void Method(ITypeA a){
                a.DoStuff();
            }
        }
    }";
            await VerifyCS.VerifyAnalyzerAsync(test + File.ReadAllText(AttributeCodePath));
        }

        [TestMethod]
        public async Task TestFindingDependencies_WithBaseClassInheritance()
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
        [Layer(""Infrastructure"")]
        class TypeA
        {
            public void DoStuff() {}
        }
        [Layer(""Domain"")]
        class TypeB : TypeA
        {
         
        }
    }";
            var expected = VerifyCS.Diagnostic(AnalyserName).WithSpan(16, 9, 20, 10).WithArguments("TypeA", "Infrastructure", "TypeB", "Domain");
            await VerifyCS.VerifyAnalyzerAsync(test + File.ReadAllText(AttributeCodePath), expected);
        }

        [TestMethod]
        public async Task TestFindingDependencies_WithInterfaceInheritance()
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
        [Layer(""Infrastructure"")]
        class ITypeA
        {
            public void DoStuff() {}
        }
        [Layer(""Domain"")]
        class TypeB : ITypeA
        {
         
        }
    }";
            var expected = VerifyCS.Diagnostic(AnalyserName).WithSpan(16, 9, 20, 10).WithArguments("ITypeA", "Infrastructure", "TypeB", "Domain");
            await VerifyCS.VerifyAnalyzerAsync(test + File.ReadAllText(AttributeCodePath), expected);
        }
       
    }
}
