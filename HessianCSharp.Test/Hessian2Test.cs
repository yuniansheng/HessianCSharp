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
            var content = "4314636f6d2e63617563686f2e6d6f64656c2e466f6f9e09706c7573496e7436340a706c7573496e743132380b706c7573496e74323034370a6d696e7573496e7436340b6d696e7573496e743132380c6d696e7573496e74323034370f646f75626c6531706f696e743233340a7a65726f446f75626c650d6d696e7573446f75626c6536340c706c7573446f75626c653634096c6f6e6756616c7565066256616c7565067356616c75650662756666657260c840c880cfffc7c0c780c0015f000004d25b5dc05d404c002bdc546291f4b1540b48656c6c6f20576f726c6422c040";

            content = "4d14636f6d2e63617563686f2e6d6f64656c2e466f6f09706c7573496e743634c8400a706c7573496e74313238c8800b706c7573496e7432303437cfff0a6d696e7573496e743634c7c00b6d696e7573496e74313238c7800c6d696e7573496e7432303437c0010f646f75626c6531706f696e743233345f000004d20a7a65726f446f75626c655b0d6d696e7573446f75626c6536345dc00c706c7573446f75626c6536345d400662756666657272065b7362797465c7c0c8405a096c6f6e6756616c75654c002bdc546291f4b1066256616c756554067356616c75650b48656c6c6f20576f726c645a";
            var foo = (Foo)Util.Deserialize2(content.ToBuffer());
            Assert.Equal(64, foo.plusInt64);
            Assert.Equal(128, foo.plusInt128);
            Assert.Equal(2047, foo.plusInt2047);
            Assert.Equal(-64, foo.minusInt64);
            Assert.Equal(-128, foo.minusInt128);
            Assert.Equal(-2047, foo.minusInt2047);

            Assert.Equal(1.234, foo.double1point234);
            Assert.Equal(0, foo.zeroDouble);
            Assert.Equal(-64, foo.minusDouble64);
            Assert.Equal(64, foo.plusDouble64);

            Assert.Equal(-64, foo.buffer[0]);
            Assert.Equal(64, foo.buffer[1]);

            Assert.Equal(12345678987654321L, foo.longValue);
            Assert.True(foo.bValue);
            Assert.Equal("Hello World", foo.sValue);

            //var content1 = Util.Serialize2(foo).ToHexString();
            //output.WriteLine(content1);
            //Assert.Equal(content.Length, content1.Length);
            //Assert.Equal(content, content1);

        }
    }
}
