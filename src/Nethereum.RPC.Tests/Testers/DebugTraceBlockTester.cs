using System;
using System.Threading.Tasks;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.DebugGeth;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Nethereum.RPC.Tests.Testers
{
    public class DebugTraceBlockTester : RPCRequestTester<JObject>, IRPCRequestTester
    {

        [Fact]
        public async void ShouldDecodeTheBlockRplAsJObject()
        {
            var result = await ExecuteAsync(ClientFactory.GetClient());
            Assert.NotNull(result);
        }

        public override async Task<JObject> ExecuteAsync(IClient client)
        {
            var debugTraceBlock = new DebugTraceBlock(client);
            var debugGetBlockRlp = new DebugGetBlockRlp(client);
            var rlp = await debugGetBlockRlp.SendRequestAsync(10);
            return await debugTraceBlock.SendRequestAsync("0x" + rlp);
        }

        public override Type GetRequestType()
        {
            return typeof(DebugTraceBlock);
        }
    }
}
        