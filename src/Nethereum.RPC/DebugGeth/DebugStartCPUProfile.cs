

using System.Threading.Tasks;
using edjCase.JsonRpc.Core;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;

namespace Nethereum.RPC
{

    ///<Summary>
       /// Turns on CPU profiling indefinitely, writing to the given file.    
    ///</Summary>
    public class DebugStartCPUProfile : RpcRequestResponseHandler<object>
        {
            public DebugStartCPUProfile(IClient client) : base(client,ApiMethods.debug_startCPUProfile.ToString()) { }

            public Task<object> SendRequestAsync(string filePath, object id = null)
            {
                return base.SendRequestAsync(id, filePath);
            }
            public RpcRequest BuildRequest(string filePath, object id = null)
            {
                return base.BuildRequest(id, filePath);
            }
        }

    }

