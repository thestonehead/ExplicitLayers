using System;
using System.Collections.Generic;
using System.Text;

namespace ExplicitLayers.Test.TestFiles.Test01
{
    [Layer("Domain")]
    internal class DomainClass1
    {

        public void DoStuff()
        {
            var a = new InfrastructureClass1();
        }
    }
}
