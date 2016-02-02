using System.Threading.Tasks;
using edjCase.JsonRpc.Client;
using edjCase.JsonRpc.Core;
using RPCRequestResponseHandlers;

namespace Nethereum.RPC.Eth
{

    ///<Summary>
       /// eth_sign
/// 
/// Signs data with a given address.
/// 
/// Note the address to sign must be unlocked.
/// 
/// Parameters
/// 
/// DATA, 20 Bytes - address
/// DATA, Data to sign
/// Returns
/// 
/// DATA: Signed data
/// 
/// Example
/// 
///  Request
/// curl -X POST --data '{"jsonrpc":"2.0","method":"eth_sign","params":["0xd1ade25ccd3d550a7eb532ac759cac7be09c2719", "Schoolbus"],"id":1}'
/// 
///  Result
/// {
///   "id":1,
///   "jsonrpc": "2.0",
///   "result": "0x2ac19db245478a06032e69cdbd2b54e648b78431d0a47bd1fbab18f79f820ba407466e37adbe9e84541cab97ab7d290f4a64a5825c876d22109f3bf813254e8601"
/// }    
    ///</Summary>
    public class EthSign : RpcRequestResponseHandler<string>
        {
            public EthSign(RpcClient client) : base(client, ApiMethods.eth_sign.ToString()) { }

            public async Task<string> SendRequestAsync( string address, string data, string id = Constants.DEFAULT_REQUEST_ID)
            {
                return await base.SendRequestAsync( id, address, data);
            }
            public RpcRequest BuildRequest(string address, string data, string id = Constants.DEFAULT_REQUEST_ID)
            {
                return base.BuildRequest(id, address, data);
            }
        }

    }

