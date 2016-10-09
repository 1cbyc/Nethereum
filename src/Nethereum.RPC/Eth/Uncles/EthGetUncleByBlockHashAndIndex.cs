using System.Threading.Tasks;
using EdjCase.JsonRpc.Core;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.RPC.Eth.Uncles
{
    /// <Summary>
    ///     eth_getUncleCountByBlockHash
    ///     Returns the number of uncles in a block from a block matching the given block hash.
    ///     Parameters
    ///     DATA, 32 Bytes - hash of a block
    ///     params: [
    ///     '0xb903239f8543d04b5dc1ba6579132b143087c68db1b2168786408fcbce568238'
    ///     ]
    ///     Returns
    ///     QUANTITY - integer of the number of uncles in this block.
    ///     Example
    ///     Request
    ///     curl -X POST --data
    ///     '{"jsonrpc":"2.0","method":"eth_getUncleCountByBlockHash","params":["0xb903239f8543d04b5dc1ba6579132b143087c68db1b2168786408fcbce568238"],"id"Block:1}'
    ///     Result
    ///     {
    ///     "id":1,
    ///     "jsonrpc": "2.0",
    ///     "result": "0x1" // 1
    ///     }
    /// </Summary>
    public class EthGetUncleByBlockHashAndIndex : RpcRequestResponseHandler<BlockWithTransactionHashes>
    {
        public EthGetUncleByBlockHashAndIndex(IClient client)
            : base(client, ApiMethods.eth_getUncleByBlockHashAndIndex.ToString())
        {
        }

        public Task<BlockWithTransactionHashes> SendRequestAsync(string blockHash, HexBigInteger uncleIndex, object id = null)
        {
            return base.SendRequestAsync(id, blockHash, uncleIndex);
        }

        public RpcRequest BuildRequest(string blockHash, HexBigInteger uncleIndex, object id = null)
        {
            return base.BuildRequest(id, blockHash, uncleIndex);
        }
    }
}
