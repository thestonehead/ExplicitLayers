using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("stuff")]
namespace TestAnalyserRosyln
{
    [Serializable]
    class TestingClass
    {
        public TestingClass()
        {
            var a = new Class1();

            Class1.DoStatic();
            IClass1 b = a;

            Hello(b);
        }

        void Hello(IClass1 c)
        {
            var x = c.MyProperty;
        }

    }

    class Class1 : IClass1
    {
        public int MyProperty { get; set; } 

        public static void DoStatic() { }
    }

    internal interface IClass1
    {
        int MyProperty { get; }
    }
}
