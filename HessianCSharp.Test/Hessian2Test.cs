using com.nelson;
using hessiancsharp.io;
using System;
using System.IO;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace HessianCSharp.Test
{
    public class Hessian2Test
    {
        private readonly ITestOutputHelper output;

        public Hessian2Test(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void ReferenceTest()
        {
            //no reference is ok
            var input = Util.GetHessian2Input("4313636f6d2e63617563686f2e6d6f656c2e466f6fffffff948696e7456616c756596c6f6e6756616c756566256616c756567356616c756560ffffff914c02bffffffdc5462ffffff91fffffff4ffffffb154b48656c6c6f20576f726c64");
            var obj = input.ReadObject();
        }
    }
}
