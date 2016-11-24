using System;
using System.Threading.Tasks;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.DebugGeth;
using Xunit;

namespace Nethereum.RPC.Tests.Testers
{
    public class DebugSeedHashTester : RPCRequestTester<string>, IRPCRequestTester
    {
        
        [Fact]
        public async void ShouldReturnTheHash()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }

        public override async Task<string> ExecuteAsync(IClient client)
        {
            var debugSeedHash = new DebugSeedHash(client);
            return await debugSeedHash.SendRequestAsync(Settings.GetBlockNumber());
        }

        public override Type GetRequestType()
        {
            return typeof(DebugSeedHash);
        }
    }
}
        