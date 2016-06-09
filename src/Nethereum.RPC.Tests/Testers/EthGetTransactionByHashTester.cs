using System;
using System.Threading.Tasks;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.Transactions;

namespace Nethereum.RPC.Sample.Testers
{
    public class EthGetTransactionByHashTester : IRPCRequestTester
    {
        public async Task<object> ExecuteTestAsync(IClient client)
        {
            var ethGetTransactionByHash = new EthGetTransactionByHash(client);
            return
                await
                    ethGetTransactionByHash.SendRequestAsync(
                        "0xb903239f8543d04b5dc1ba6579132b143087c68db1b2168786408fcbce568238");
        }

        public Type GetRequestType()
        {
            return typeof (EthGetTransactionByHash);
        }
    }
}