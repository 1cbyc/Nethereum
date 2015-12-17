using System;
using edjCase.JsonRpc.Client;
using Ethereum.RPC.Eth;

namespace Ethereum.RPC.Sample
{
    public class EthSendTransactionTester : IRPCRequestTester
    {
        /*

         curl -X POST --data '{"jsonrpc":"2.0","method":"eth_sendTransaction","params":[{"from":"0x65180b8c813457b21dad6cc6363d195231b4d2e9","data":"0x606060405260728060106000396000f360606040526000357c010000000000000000000000000000000000000000000000000000000090048063c6888fa1146037576035565b005b604b60048080359060200190919050506061565b6040518082815260200191505060405180910390f35b6000600782029050606d565b91905056"}],"id":1}' http://localhost:8545

         */
        public dynamic ExecuteTest(RpcClient client)
        {
            var contractByteCode = "0x606060405260728060106000396000f360606040526000357c010000000000000000000000000000000000000000000000000000000090048063c6888fa1146037576035565b005b604b60048080359060200190919050506061565b6040518082815260200191505060405180910390f35b6000600782029050606d565b91905056";
            var ethSendTransation = new EthSendTransaction();
            var transactionInput = new EthSendTransactionInput();
            transactionInput.Data = contractByteCode;
            transactionInput.From = "0x12890d2cce102216644c59dae5baed380d84830c";
            return ethSendTransation.SendRequestAsync(client, transactionInput).Result;

        }
        public Type GetRequestType()
        {
            return typeof(EthSendTransaction);
        }
    }
}