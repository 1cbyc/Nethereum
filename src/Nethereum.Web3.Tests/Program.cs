﻿using SimpleTests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nethereum.Web3.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {

            //var contractDeploymentAndCallTest = new ContractDeploymentAndCall();
            //Console.WriteLine(contractDeploymentAndCallTest.Test().Result);

            //var intTests = new IntTypeIntegrationTests();
            //Console.WriteLine(intTests.Test().Result);

            // var personalTest = new PersonalTest();
            // Console.WriteLine(personalTest.Test().Result);

            // var contractConstructorDeploymentAndCall = new ContractConstructorDeploymentAndCall();
            //Console.WriteLine(contractConstructorDeploymentAndCall.Test().Result);

            //var eventFilterTopic = new EventFilterTopic();
            //Console.WriteLine(eventFilterTopic.Test().Result);

            var eventFilterTopic2 = new EventFilterWith2Topics();
            eventFilterTopic2.Test().Wait();
  
            var passed = new TransactionSigningTests().ShouldSignAndSendRawTransaction().Result;
            new TransactionTests().TestTransactionFromSignedRLP();
            new TransactionTests().ShouldEncodeATransactionUsingKeccak256();
            new TransactionTests().ShouldResolveAddress();
            new TransactionTests().ShouldSignEncodeTransactionAndRecoverPublicAddress();
            new TransactionTests().TestTransactionFromUnSignedRLP();

            Console.ReadLine();
        }
    }
}
