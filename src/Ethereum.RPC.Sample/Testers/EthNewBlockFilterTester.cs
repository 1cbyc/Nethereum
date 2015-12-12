
using edjCase.JsonRpc.Client;
using System;
using Ethereum.RPC.Eth;

namespace Ethereum.RPC.Sample.Testers
{
    public class EthNewBlockFilterTester : IRPCRequestTester
    {
        public dynamic ExecuteTest(RpcClient client)
        {
            var ethNewBlockFilter = new EthNewBlockFilter();
            return ethNewBlockFilter.SendRequestAsync(client).Result;
        }

        public Type GetRequestType()
        {
            return typeof(EthNewBlockFilter);
        }
    }
}
        