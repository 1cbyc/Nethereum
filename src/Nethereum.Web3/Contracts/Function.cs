using System.Threading.Tasks;
using Nethereum.JsonRpc.Client;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Transactions;

namespace Nethereum.Web3
{
   

    public class Function : FunctionBase
    {
       

        public Function(IClient rpcClient, Contract contract, FunctionABI function) : base(rpcClient, contract, function)
        {
           
        }

        public async Task<TReturn> CallAsync<TReturn>(params object[] functionInput)
        {
            var encodedInput = GetData(functionInput);
            return await base.CallAsync<TReturn>(encodedInput);
        }

        public async Task<TReturn> CallAsync<TReturn>(string from, HexBigInteger gas,
            HexBigInteger value,  params object[] functionInput) 
        {
            var encodedInput = GetData(functionInput);
            return await base.CallAsync<TReturn>(encodedInput, from, gas, value);
        }

        public async Task<TReturn> CallAsync<TReturn>(
            CallInput callInput, params object[] functionInput) 
        {
            var encodedInput = GetData(functionInput);
            return await base.CallAsync<TReturn>(encodedInput, callInput);
        }

        public async Task<TReturn> CallAsync<TReturn>(
            CallInput callInput, BlockParameter blockParameter, params object[] functionInput)
        {
            var encodedInput = GetData(functionInput);
            return await base.CallAsync<TReturn>(encodedInput, callInput, blockParameter);
        }

        public async Task<TReturn> CallDeserializingToObjectAsync<TReturn>(params object[] functionInput) where TReturn: new()
        {
            var encodedInput = GetData(functionInput);
            return await base.CallAsync<TReturn>(new TReturn(), encodedInput);
        }

        public async Task<TReturn> CallDeserializingToObjectAsync<TReturn>(string from, HexBigInteger gas,
            HexBigInteger value, TReturn functionOutput, params object[] functionInput) where TReturn : new()
        {
            var encodedInput = GetData(functionInput);
            return await base.CallAsync<TReturn>(new TReturn(), encodedInput, from, gas, value);
        }

        public async Task<TReturn> CallDeserializingToObjectAsync<TReturn>(
            CallInput callInput, params object[] functionInput) where TReturn : new()
        {
            var encodedInput = GetData(functionInput);
            return await base.CallAsync<TReturn>(new TReturn(), encodedInput, callInput);
        }

        public async Task<TReturn> CallDeserializingToObjectAsync<TReturn>(
            CallInput callInput, BlockParameter blockParameter, params object[] functionInput) where TReturn : new()
        {
            var encodedInput = GetData(functionInput);
            return await base.CallAsync<TReturn>(new TReturn(), encodedInput, callInput, blockParameter);
        }

        public async Task<string> SendTransactionAsync(string from, params object[] functionInput)
        {
            var encodedInput = GetData(functionInput);
            return await base.SendTransactionAsync(encodedInput, from, null, null);
        }

        public async Task<string> SendTransactionAsync(string from, HexBigInteger gas,
            HexBigInteger value, params object[] functionInput)
        {
            var encodedInput = GetData(functionInput);
            return await base.SendTransactionAsync(encodedInput, from, gas, value);
        }

        public async Task<string> SendTransactionAsync(
            TransactionInput input, params object[] functionInput)
        {
            var encodedInput = GetData(functionInput);
            return await base.SendTransactionAsync(encodedInput, input);
        }

        public string GetData(params object[] functionInput)
        {
            return FunctionCallEncoder.EncodeRequest(FunctionABI.Sha3Signature, FunctionABI.InputParameters, functionInput);
        }
    }
    
    public class Function<TFunctionInput> : FunctionBase
    {

        public Function(IClient rpcClient, Contract contract, FunctionABI functionABI) : base(rpcClient, contract, functionABI)
        {
        }

        public async Task<TReturn> CallAsync<TReturn>()
        {
            var encodedInput = FunctionABI.Sha3Signature;
            return await base.CallAsync<TReturn>(encodedInput);
        }

        public async Task<TReturn> CallAsync<TReturn>(TFunctionInput functionInput)
        {

            var encodedInput = GetData(functionInput);
            return await base.CallAsync<TReturn>(encodedInput);
        }

        public async Task<TReturn> CallAsync<TReturn>(TFunctionInput functionInput, string from, HexBigInteger gas,
            HexBigInteger value)
        {
            var encodedInput = GetData(functionInput);
            return await base.CallAsync<TReturn>(encodedInput, from, gas, value);
        }

        public async Task<TReturn> CallAsync<TReturn>(TFunctionInput functionInput,
            CallInput callInput) 
        {
            var encodedInput = GetData(functionInput);
            return await base.CallAsync<TReturn>(encodedInput, callInput);
        }

        public async Task<TReturn> CallAsync<TReturn>(TFunctionInput functionInput,
            CallInput callInput, BlockParameter blockParameter)         {
            var encodedInput = GetData(functionInput);
            return await base.CallAsync<TReturn>( encodedInput, callInput, blockParameter);
        }

        public async Task<TReturn> CallDeserializingToObjectAsync<TReturn>() where TReturn : new()
        {
            var encodedInput = FunctionABI.Sha3Signature;
            return await base.CallAsync<TReturn>(new TReturn(), encodedInput);
        }

        public async Task<TReturn> CallDeserializingToObjectAsync<TReturn>(TFunctionInput functionInput) where TReturn : new()
        {
            var encodedInput = GetData(functionInput);
            return await base.CallAsync<TReturn>(new TReturn(), encodedInput);
        }

        public async Task<TReturn> CallDeserializingToObjectAsync<TReturn>(TFunctionInput functionInput, string from, HexBigInteger gas,
            HexBigInteger value) where TReturn : new()
        {
            var encodedInput = GetData(functionInput);
            return await base.CallAsync<TReturn>(new TReturn(), encodedInput, from, gas, value);
        }

        public async Task<TReturn> CallDeserializingToObjectAsync<TReturn>(TFunctionInput functionInput,
            CallInput callInput) where TReturn : new()
        {
            var encodedInput = GetData(functionInput);
            return await base.CallAsync<TReturn>(new TReturn(), encodedInput, callInput);
        }

        public async Task<TReturn> CallDeserializingToObjectAsync<TReturn>(TFunctionInput functionInput,
            CallInput callInput, BlockParameter blockParameter) where TReturn : new()
        {
            var encodedInput = GetData(functionInput);
            return await base.CallAsync<TReturn>(new TReturn(), encodedInput, callInput, blockParameter);
        }


        public async Task<string> SendTransactionAsync(TFunctionInput functionInput) 
        {
            var encodedInput = GetData(functionInput);
            return await base.SendTransactionAsync(encodedInput);
        }

        public async Task<string> SendTransactionAsync(TFunctionInput functionInput, string from, HexBigInteger gas,
            HexBigInteger value) 
        {
            var encodedInput = GetData(functionInput);
            return await base.SendTransactionAsync(encodedInput, from, gas, value);
        }

        public async Task<string> SendTransactionAsync(TFunctionInput functionInput,
            TransactionInput input)
        {
            var encodedInput = GetData(functionInput);
            return await base.SendTransactionAsync(encodedInput, input);
        }

        public string GetData(TFunctionInput functionInput)
        {
            return FunctionCallEncoder.EncodeRequest(functionInput);
        }

    }
}