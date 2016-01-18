using System;
using Ethereum.RPC.ABI;
using Ethereum.RPC.Util;
using Xunit;
using System.Linq;

namespace Ethereum.ABI.Tests.DNX
{
    public class AddressEncodingTests
    {
        [Fact]
        public virtual void ShouldEncodeAddressString()
        {
            AddressType addressType = new AddressType();
            var result2 = addressType.Encode("1234567890abcdef1234567890abcdef12345678").ToHex();
            Assert.Equal("0000000000000000000000001234567890abcdef1234567890abcdef12345678", result2);
        }
    }
}