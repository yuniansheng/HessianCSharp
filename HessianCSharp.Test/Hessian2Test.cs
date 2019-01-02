using com.caucho.model;
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
            var input = Util.GetHessian2Input("4314636f6d2e63617563686f2e6d6f64656c2e466f6f9408696e7456616c7565096c6f6e6756616c7565066256616c7565067356616c756560914c002bdc546291f4b1540b48656c6c6f20576f726c64");
            var foo = (Foo)input.ReadObject();
            Assert.Equal(1, foo.intValue);
            Assert.Equal(12345678987654321L, foo.longValue);
            Assert.True(foo.bValue);
            Assert.Equal("Hello World", foo.sValue);
        }
    }
}
