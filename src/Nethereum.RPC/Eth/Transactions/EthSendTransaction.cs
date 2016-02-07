﻿using System;
using System.Threading.Tasks;
using edjCase.JsonRpc.Client;
using edjCase.JsonRpc.Core;
using Nethereum.RPC.Eth.DTOs;
using RPCRequestResponseHandlers;

namespace Nethereum.RPC.Eth.Transactions
{

    public class EthSendTransaction : RpcRequestResponseHandler<String>
    {
        public EthSendTransaction(RpcClient client) : base(client, ApiMethods.eth_sendTransaction.ToString())
        {

        }

        public async Task<String> SendRequestAsync( TransactionInput input, string id = Constants.DEFAULT_REQUEST_ID)
        {
            return await base.SendRequestAsync( id, input);
        }
        public RpcRequest BuildRequest(TransactionInput input, string id = Constants.DEFAULT_REQUEST_ID)
        {
            return base.BuildRequest(id, input);
        }


    }
}
