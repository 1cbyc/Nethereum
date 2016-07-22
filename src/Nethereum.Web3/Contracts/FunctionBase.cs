using System.Collections.Generic;
using System.Threading.Tasks;
using Nethereum.JsonRpc.Client;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Transactions;

namespace Nethereum.Web3
{
    public abstract class FunctionBase
    {
        private IClient rpcClient;

        private readonly Contract contract;

        public BlockParameter DefaultBlock => contract.DefaultBlock;

        public string ContractAddress => contract.Address;

        protected FunctionCallDecoder FunctionCallDecoder { get; set; }

        protected FunctionCallEncoder FunctionCallEncoder { get; set; }

        private EthCall ethCall;
        private EthSendTransaction ethSendTransaction;
        protected FunctionABI FunctionABI { get; set; }

        protected FunctionBase(IClient rpcClient, Contract contract, FunctionABI functionABI )
        {
            FunctionABI = functionABI;
            this.rpcClient = rpcClient;
            this.contract = contract;
            this.ethCall = new EthCall(rpcClient);
            this.ethSendTransaction = new EthSendTransaction(rpcClient);
               
            this.FunctionCallDecoder = new FunctionCallDecoder();
            this.FunctionCallEncoder = new FunctionCallEncoder();

        }

        public List<ParameterOutput> DecodeInput(string data)
        {
            return FunctionCallDecoder.DecodeFunctionInput(FunctionABI.Sha3Signature, data, FunctionABI.InputParameters);
        } 

        private Parameter GetFirstParameterOrNull(Parameter[] parameters)
        {
            if (parameters == null) return null;
            if (parameters.Length == 0) return null;
            return parameters[0];
        }
         
        protected async Task<TReturn> CallAsync<TReturn>(string encodedFunctionCall)
        {
            var result =  await ethCall.SendRequestAsync(new CallInput(encodedFunctionCall, ContractAddress), DefaultBlock).ConfigureAwait(false);
            
            return FunctionCallDecoder.DecodeSimpleTypeOutput<TReturn>(GetFirstParameterOrNull(FunctionABI.OutputParameters), result);
        }

        protected async Task<TReturn> CallAsync<TReturn>(string encodedFunctionCall, string from, HexBigInteger gas, HexBigInteger value ) 
        {
            var result = await ethCall.SendRequestAsync(new CallInput(encodedFunctionCall, ContractAddress, @from, gas, value), DefaultBlock).ConfigureAwait(false);
            return FunctionCallDecoder.DecodeSimpleTypeOutput<TReturn>(GetFirstParameterOrNull(FunctionABI.OutputParameters), result);
        }

        protected async Task<TReturn> CallAsync<TReturn>(string encodedFunctionCall, CallInput callInput)
        {
            callInput.Data = encodedFunctionCall; 
            var result = await ethCall.SendRequestAsync(callInput, DefaultBlock).ConfigureAwait(false);
            return FunctionCallDecoder.DecodeSimpleTypeOutput<TReturn>(GetFirstParameterOrNull(FunctionABI.OutputParameters), result);
        }

        protected async Task<TReturn> CallAsync<TReturn>(string encodedFunctionCall, CallInput callInput, BlockParameter block) 
        {
            callInput.Data = encodedFunctionCall;
            var result = await ethCall.SendRequestAsync(callInput, block).ConfigureAwait(false);
            return FunctionCallDecoder.DecodeSimpleTypeOutput<TReturn>(GetFirstParameterOrNull(FunctionABI.OutputParameters), result);
        }


        protected async Task<TReturn> CallAsync<TReturn>(TReturn functionOuput, string encodedFunctionCall)
        {
            var result = await ethCall.SendRequestAsync(new CallInput(encodedFunctionCall, ContractAddress), DefaultBlock).ConfigureAwait(false);

            return FunctionCallDecoder.DecodeFunctionOutput(functionOuput, result);
        }

        protected async Task<TReturn> CallAsync<TReturn>(TReturn functionOuput, string encodedFunctionCall, string from, HexBigInteger gas, HexBigInteger value)
        {
            var result = await ethCall.SendRequestAsync(new CallInput(encodedFunctionCall, ContractAddress, @from, gas, value), DefaultBlock).ConfigureAwait(false);
            return FunctionCallDecoder.DecodeFunctionOutput(functionOuput, result);
        }

        protected async Task<TReturn> CallAsync<TReturn>(TReturn functionOuput, string encodedFunctionCall, CallInput callInput)
        {
            callInput.Data = encodedFunctionCall;
            var result = await ethCall.SendRequestAsync(callInput, DefaultBlock).ConfigureAwait(false);
            return FunctionCallDecoder.DecodeFunctionOutput(functionOuput, result);
        }

        protected async Task<TReturn> CallAsync<TReturn>(TReturn functionOuput, string encodedFunctionCall, CallInput callInput, BlockParameter block)
        {
            callInput.Data = encodedFunctionCall;
            var result = await ethCall.SendRequestAsync(callInput, block).ConfigureAwait(false);
            return FunctionCallDecoder.DecodeFunctionOutput(functionOuput, result);
        }

        protected Task<string> SendTransactionAsync(string encodedFunctionCall)
        {
            return ethSendTransaction.SendRequestAsync(new TransactionInput(encodedFunctionCall, ContractAddress));
              
        }

        protected Task<string> SendTransactionAsync(string encodedFunctionCall, string from, HexBigInteger gas, HexBigInteger value) 
        {
            return ethSendTransaction.SendRequestAsync(new TransactionInput(encodedFunctionCall, ContractAddress, from, gas, value));
        }

        protected Task<string> SendTransactionAsync(string encodedFunctionCall,
            TransactionInput input)
        {
            input.Data = encodedFunctionCall;
            return ethSendTransaction.SendRequestAsync(input);
        }


    }
}