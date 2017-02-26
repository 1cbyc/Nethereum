﻿using System;
using Nethereum.JsonRpc.Client;

namespace Nethereum.Web3.Tests
{
    public class ClientFactory
    {
        public static IClient GetClient()
        {
            //var client = new IpcClient("./geth.ipc");
            // return client;
            //
            return new RpcClient(new Uri("http://localhost:8545/"));
           
        }
    }
}
