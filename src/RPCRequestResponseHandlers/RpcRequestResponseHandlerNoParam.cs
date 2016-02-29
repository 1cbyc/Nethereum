﻿using edjCase.JsonRpc.Client;
using edjCase.JsonRpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPCRequestResponseHandlers
{
    public class RpcRequestResponseHandlerNoParam<TResponse>: IRpcRequestHandler
    {
        public string MethodName { get; }
        public RpcClient Client { get; }

        public RpcRequestResponseHandlerNoParam(RpcClient client, string methodName)
        {
            this.MethodName = methodName;
            this.Client = client;
        }

        public virtual async Task<TResponse> SendRequestAsync(object id)
        {
            var response = await Client.SendRequestAsync(BuildRequest(id));
            if (response.HasError) throw new RPCResponseException(response);
            return response.GetResult<TResponse>();
        }
        public RpcRequest BuildRequest(object id)
        {
            if (id == null) id = Configuration.DefaultRequestId;
            id = id?.ToString();

            return new RpcRequest((string)id  , MethodName, (object)null);
        }
    }

}
