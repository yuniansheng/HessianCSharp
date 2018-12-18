using com.nelson;
using hessiancsharp.io;
using System;
using System.IO;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace HessianCSharp.Test
{
    public class HessianTest
    {
        private readonly ITestOutputHelper output;

        public HessianTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void ReferenceTest()
        {
            //no reference is ok
            var input = GetHessianInput("4d74000e636f6d2e6e656c736f6e2e466f6f530004646174656400000167c0f0fc91530007636c6173736573567400105b6a6176612e6c616e672e436c6173736c000000034d74000f6a6176612e6c616e672e436c6173735300046e616d655300046c6f6e677a4d74000f6a6176612e6c616e672e436c6173735300046e616d65530003696e747a52000000037a7a");
            var foo = (Foo)input.ReadObject();
        }

        [Fact]
        public void RpcRequestReferenceTest()
        {            
            var input = GetHessianInput("4d740025636f6d2e78786c2e6a6f622e636f72652e7270632e636f6465632e5270635265717565737453000d736572766572416464726573735300133137322e31382e32332e3133323a32383838325300106372656174654d696c6c697354696d654c00000167ba8c08f653000b616363657373546f6b656e5300206364616666383133616266303266666530366265303436396233663365663433530009636c6173734e616d65530020636f6d2e78786c2e6a6f622e636f72652e62697a2e4578656375746f7242697a53000a6d6574686f644e616d655300036c6f6753000e706172616d657465725479706573567400105b6a6176612e6c616e672e436c6173736c000000034d74000f6a6176612e6c616e672e436c6173735300046e616d655300046c6f6e677a4d74000f6a6176612e6c616e672e436c6173735300046e616d65530003696e747a52000000037a53000a706172616d6574657273567400075b6f626a6563746c000000034c00000167ba8bbfa0490000059549000000017a7a");
            var request = (com.xxl.job.core.rpc.codec.RpcRequest)input.ReadObject();            
        }

        private CHessianInput GetHessianInput(string content)
        {
            var buffer = new byte[content.Length / 2];
            for (int i = 0; i < content.Length; i += 2)
            {
                buffer[i / 2] = Convert.ToByte(content.Substring(i, 2), 16);
            }

            output.WriteLine(Encoding.ASCII.GetString(buffer));
            var stream = new MemoryStream(buffer);
            CHessianInput input = new CHessianInput(stream);
            return input;
        }
    }
}
