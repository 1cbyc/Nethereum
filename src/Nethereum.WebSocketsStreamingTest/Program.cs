﻿using Nethereum.Contracts;
using Nethereum.JsonRpc.Client.RpcMessages;
using Nethereum.JsonRpc.WebSocketStreamingClient;
using Nethereum.RPC.Eth.DTOs;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using Nethereum.Contracts.Extensions;
using Nethereum.ABI.FunctionEncoding.Attributes;
using System.Numerics;
using Nethereum.JsonRpc.Client.Streaming;
using Nethereum.RPC.Eth.Subscriptions;

namespace Nethereum.WebSocketsStreamingTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new StreamingWebSocketClient("wss://mainnet.infura.io/ws");

            // var client = new StreamingWebSocketClient("ws://127.0.0.1:8546");
            var blockHeaderSubscription = new EthNewBlockHeadersObservableSubscription(client);

            blockHeaderSubscription.GetSubscribeResponseAsObservable().Subscribe(subscriptionId =>
                Console.WriteLine("Block Header subscription Id: " + subscriptionId));

            blockHeaderSubscription.GetSubscriptionDataResponsesAsObservable().Subscribe(block =>
                Console.WriteLine("New Block: " + block.BlockHash));

            blockHeaderSubscription.GetUnsubscribeResponseAsObservable().Subscribe(response =>
                            Console.WriteLine("Block Header unsubscribe result: " + response));


            var blockHeaderSubscription2 = new EthNewBlockHeadersSubscription(client);
            blockHeaderSubscription2.SubscriptionDataResponse += (object sender, StreamingEventArgs<Block> e) =>
            {
                Console.WriteLine("New Block from event: " + e.Response.BlockHash);
            };

            var disposable = Observable.FromEventPattern<StreamingEventArgs<Block>>(h => blockHeaderSubscription2.SubscriptionDataResponse += h,
                h => blockHeaderSubscription2.SubscriptionDataResponse -= h).Subscribe(x =>
                 Console.WriteLine("New Block from observable from event : " + x.EventArgs.Response.BlockHash)
                );

            var pendingTransactionsSubscription = new EthNewPendingTransactionObservableSubscription(client);

            pendingTransactionsSubscription.GetSubscribeResponseAsObservable().Subscribe(subscriptionId =>
                Console.WriteLine("Pending transactions subscription Id: " + subscriptionId));

            pendingTransactionsSubscription.GetSubscriptionDataResponsesAsObservable().Subscribe(transactionHash =>
                Console.WriteLine("New Pending TransactionHash: " + transactionHash));

            pendingTransactionsSubscription.GetUnsubscribeResponseAsObservable().Subscribe(response =>
                            Console.WriteLine("Pending transactions unsubscribe result: " + response));


            var ethGetBalance = new EthGetBalanceObservableHandler(client);
            var subs = ethGetBalance.GetResponseAsObservable().Subscribe(balance =>
                            Console.WriteLine("Balance xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx: " + balance.Value.ToString()));
           
            var ethBlockNumber = new EthBlockNumberObservableHandler(client);
            ethBlockNumber.GetResponseAsObservable().Subscribe(blockNumber =>
                                Console.WriteLine("Block number: bbbbbbbbbbbbbb" + blockNumber.Value.ToString()));


            var ethLogs = new EthLogsObservableSubscription(client);
            ethLogs.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
                Console.WriteLine("Log Address:" + log.Address));

            //no contract address

            var filterTransfers = new TransferEventDTO().GetEventABI().CreateFilterInput((string)null);

            var ethLogsTokenTransfer = new EthLogsObservableSubscription(client);
            ethLogsTokenTransfer.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    var decoded = new TransferEventDTO().DecodeEvent(log);
                    if (decoded != null)
                    {
                        Console.WriteLine("Contract address: " + log.Address +  " Log Transfer from:" + decoded.From);
                    }
                    else
                    {
                        Console.WriteLine("Found not standard transfer log");
                    }
                }
                catch {
                }
            });

            
                


            client.Start().Wait();

            blockHeaderSubscription.SubscribeAsync().Wait();

            blockHeaderSubscription2.SubscribeAsync().Wait();

            pendingTransactionsSubscription.SubscribeAsync().Wait();
            
            ethGetBalance.SendRequestAsync("0x742d35cc6634c0532925a3b844bc454e4438f44e", BlockParameter.CreateLatest()).Wait();

            ethBlockNumber.SendRequestAsync().Wait();

            ethLogs.SubscribeAsync().Wait();

            ethLogsTokenTransfer.SubscribeAsync().Wait();

            Thread.Sleep(30000);
            pendingTransactionsSubscription.UnsubscribeAsync().Wait();

            Thread.Sleep(20000);

            blockHeaderSubscription.UnsubscribeAsync().Wait();

            Thread.Sleep(20000);
        }


        public partial class TransferEventDTO : TransferEventDTOBase { }

        [Event("Transfer")]
        public class TransferEventDTOBase : IEventDTO
        {
            [Parameter("address", "_from", 1, true)]
            public virtual string From { get; set; }
            [Parameter("address", "_to", 2, true)]
            public virtual string To { get; set; }
            [Parameter("uint256", "_value", 3, false)]
            public virtual BigInteger Value { get; set; }
        }


    }
}
