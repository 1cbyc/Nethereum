

using System.Threading.Tasks;
using edjCase.JsonRpc.Core;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Newtonsoft.Json.Linq;

namespace Nethereum.RPC
{

    ///<Summary>
       /// Retrieves the state that corresponds to the block number and returns a list of accounts (including storage and code).    
    ///</Summary>
    public class DebugDumpBlock : RpcRequestResponseHandler<JObject>
        {
            public DebugDumpBlock(IClient client) : base(client,ApiMethods.debug_dumpBlock.ToString()) { }

            public Task<JObject> SendRequestAsync(long blockNumber, object id = null)
            {
                return base.SendRequestAsync(id, blockNumber);
            }
            public RpcRequest BuildRequest(long blockNumber, object id = null)
            {
                return base.BuildRequest(id, blockNumber);
            }
        }

    }

