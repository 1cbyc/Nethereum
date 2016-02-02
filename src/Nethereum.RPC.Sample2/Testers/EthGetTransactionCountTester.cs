
using edjCase.JsonRpc.Client;
using System;
using System.Threading.Tasks;

namespace Ethereum.RPC.Sample.Testers
{
    public class EthGetTransactionCountTester : IRPCRequestTester
    {
        public async Task<dynamic> ExecuteTestAsync(RpcClient client)
        {
            var ethGetTransactionCount = new EthGetTransactionCount();
            return await ethGetTransactionCount.SendRequestAsync(client, "0x12890d2cce102216644c59dae5baed380d84830c");
        }

        public Type GetRequestType()
        {
            return typeof(EthGetTransactionCount);
        }
    }
}
        