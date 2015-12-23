using System;
using System.Threading.Tasks;
using edjCase.JsonRpc.Client;
using Ethereum.RPC.ABI;
using Ethereum.RPC.Eth;

namespace Ethereum.RPC.Sample
{
    public class DeployContractExecuteTransactionFilterEventTester : IRPCRequestTester
    {
        public async Task<dynamic> ExecuteTestAsync(RpcClient client)
        {
            /*
            contract test { 
    
                event Multiplied(uint indexed a, address sender);
    
                function multiply(uint a) returns(uint d) 
                { 
                    return a * 7; 
                    Multiplied(a, msg.sender);
                } 
    
            }*/

            //deploy contract with event
            var contractByteCode =  "6060604052602a8060106000396000f3606060405260e060020a6000350463c6888fa18114601a575b005b6007600435026060908152602090f3";
            
            //Create a new Eth Send Transanction RPC Handler
            var ethSendTransation = new EthSendTransaction();
            //As the input the compiled contract is the Data, together with our address
            var transactionInput = new EthSendTransactionInput();
            transactionInput.Data = contractByteCode;
            transactionInput.From = "0x12890d2cce102216644c59dae5baed380d84830c";
            // retrieve the hash
            var transactionHash = await ethSendTransation.SendRequestAsync(client, transactionInput);

            //the contract should be mining now

            //get contract address 
            var ethGetTransactionReceipt = new EthGetTransactionReceipt();
            EthTransactionReceipt receipt = null;
            //wait for the contract to be mined to the address
            while (receipt == null)
            {
                receipt = await ethGetTransactionReceipt.SendRequestAsync(client, transactionHash);
            }

            //Encode and build function parameters 

            //create a transaction which will raise the event

            //Encode and build function parameters 
            var function = new ABI.FunctionCallEncoder();

            //Input the function method Sha3Encoded (4 bytes) 
            function.FunctionSha3Encoded = "c6888fa1";
            //Define input and output parameters
            function.InputsParameters = new[] { new Parameter() { Name = "a", Type = ABIType.CreateABIType("uint") } };
            function.OutputParameters = new[] { new Parameter() { Type = ABIType.CreateABIType("uint") } };
            //encode the function call (function + parameter input)
            //using 69 as the input
            var functionCall = function.EncodeRequest(69);
            //reuse the transaction input, (just the address) 
            //the destination address is the contract address
            transactionInput.To = receipt.ContractAddress;
            //use as data the function call
            transactionInput.Data = functionCall;

            var transactionHashFunction = await  ethSendTransation.SendRequestAsync(client, transactionInput);


            //create a filter (could be done before of the transaction)
            //sh3 the event call 
            var eventCall = "Multiplied(uint256, address)";
            var eventCallSh3 = await new Web3.Web3Sha3().SendRequestAsync(client, eventCall);

            //just listen to anything no more filter topics (ie int indexed number)

            //get filter changes

            //decode result

            throw new NotImplementedException();
        }

        public Type GetRequestType()
        {
            return typeof(DeployContractCallFunctionTester);
        }
    }
}