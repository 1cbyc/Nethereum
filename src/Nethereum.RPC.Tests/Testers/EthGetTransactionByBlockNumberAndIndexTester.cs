using System;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.Transactions;

namespace Nethereum.RPC.Tests.Testers
{
    public class EthGetTransactionByBlockNumberAndIndexTester : IRPCRequestTester
    {
        public async Task<object> ExecuteTestAsync(IClient client)
        {
            var ethGetTransactionByBlockNumberAndIndex = new EthGetTransactionByBlockNumberAndIndex(client);
            return
                (object)
                    await
                        ethGetTransactionByBlockNumberAndIndex.SendRequestAsync(new HexBigInteger(20),
                            new HexBigInteger(0));
        }

        public Type GetRequestType()
        {
            return typeof (EthGetTransactionByBlockNumberAndIndex);
        }
    }
}