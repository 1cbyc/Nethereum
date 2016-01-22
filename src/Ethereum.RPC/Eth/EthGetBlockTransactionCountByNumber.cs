

using System;
using System.Threading.Tasks;
using edjCase.JsonRpc.Client;
using edjCase.JsonRpc.Core;
using Ethereum.RPC.Eth;
using Ethereum.RPC.Generic;
using RPCRequestResponseHandlers;

namespace Ethereum.RPC
{

    ///<Summary>
       /// eth_getBlockTransactionCountByNumber
/// 
/// Returns the number of transactions in a block from a block matching the given block number.
/// 
/// Parameters
/// 
/// QUANTITY|TAG - integer of a block number, or the string "earliest", "latest" or "pending", as in the default block parameter.
/// params: [
///    '0xe8', // 232
/// ]
/// Returns
/// 
/// QUANTITY - integer of the number of transactions in this block.
/// 
/// Example
/// 
///  Request
/// curl -X POST --data '{"jsonrpc":"2.0","method":"eth_getBlockTransactionCountByNumber","params":["0xe8"],"id":1}'
/// 
///  Result
/// {
///   "id":1,
///   "jsonrpc": "2.0",
///   "result": "0xa" // 10
/// }    
    ///</Summary>
    public class EthGetBlockTransactionCountByNumber : RpcRequestResponseHandler<HexBigInteger>
        {
            public EthGetBlockTransactionCountByNumber() : base(ApiMethods.eth_getBlockTransactionCountByNumber.ToString()) { }

            public async Task<HexBigInteger> SendRequestAsync(RpcClient client, BlockParameter block, string id = Constants.DEFAULT_REQUEST_ID)
            {
                return await base.SendRequestAsync(client, id, block);
            }

            public async Task<HexBigInteger> SendRequestAsync(RpcClient client, string id = Constants.DEFAULT_REQUEST_ID)
            {
                return await SendRequestAsync(client, BlockParameter.CreateLatest(), id);
            }

        public RpcRequest BuildRequest(BlockParameter block, string id = Constants.DEFAULT_REQUEST_ID)
            {
                return base.BuildRequest(id, block);
            }
        }

    }

