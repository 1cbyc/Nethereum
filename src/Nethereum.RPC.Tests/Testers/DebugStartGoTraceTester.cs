
using System;
using System.Threading.Tasks;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth;
using Nethereum.RPC.Tests;
using Xunit;
using Nethereum.RPC.DebugGeth;

namespace Nethereum.RPC.Sample.Testers
{
    public class DebugStartGoTraceTester : RPCRequestTester<object>, IRPCRequestTester
    {

        [Fact]
        public async void ShouldAlwaysReturnNull()
        {
            var result = await ExecuteAsync(ClientFactory.GetClient());
            Assert.Null(result);
        }

        public override async Task<object> ExecuteAsync(IClient client)
        {
            var debugStartGoTrace = new DebugStartGoTrace(client);
            return await debugStartGoTrace.SendRequestAsync(@"C:\ProgramData\chocolatey\lib\geth-stable\tools\log.txt", 30);
        }

        public override Type GetRequestType()
        {
            return typeof(DebugStartGoTrace);
        }
    }
}
        