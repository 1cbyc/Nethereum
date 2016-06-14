
using System;
using System.Threading.Tasks;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth;
using Nethereum.RPC.Tests;
using Xunit;
using Nethereum.RPC.DebugGeth;

namespace Nethereum.RPC.Sample.Testers
{
    public class DebugGoTraceTester : RPCRequestTester<object>, IRPCRequestTester
    {
        [Fact]
        public async void ShouldAlwaysReturnNull()
        {
            var result = await ExecuteAsync(ClientFactory.GetClient());
            Assert.Null(result);
        }

        public override async Task<object> ExecuteAsync(IClient client)
        {
            var debugGoTrace = new DebugGoTrace(client);
            return await debugGoTrace.SendRequestAsync(@"C:\ProgramData\chocolatey\lib\geth-stable\tools\log.txt", 10);
        }

        public override Type GetRequestType()
        {
            return typeof(DebugGoTrace);
        }
    }
}
        