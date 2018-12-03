﻿using System;
using System.Collections.Generic;
using System.Text;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.Subscriptions;

namespace Nethereum.RPC.Eth.Services
{
    public class EthSubscriptionService : RpcClientWrapper
    {
        public EthSubscriptionService(IStreamingClient client) : base(client)
        {
            NewPendingTransactionSubscription = new EthNewPendingTransactionSubscription(client);
        }

        public EthNewPendingTransactionSubscription NewPendingTransactionSubscription { get; private set; }
    }
}
