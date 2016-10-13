using System.Threading.Tasks;
using EdjCase.JsonRpc.Core;

namespace Nethereum.JsonRpc.Client
{
    public interface IClient
    {
        RequestInterceptor OverridingRequestInterceptor { get; set; }
        Task<RpcResponse> SendRequestAsync(RpcRequest request, string route = null);
        Task<RpcResponse> SendRequestAsync(string method, string route = null, params object[] paramList);
    }
}