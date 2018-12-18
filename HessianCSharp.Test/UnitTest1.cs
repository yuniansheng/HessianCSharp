using System;
using Xunit;
using Xunit.Abstractions;

namespace HessianCSharp.Test
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper output;

        public UnitTest1(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void TestMethod1()
        {
        }
    }
}
