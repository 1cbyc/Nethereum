using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Contracts.CQS;

namespace Nethereum.StandardTokenEIP20.CQS
{
    [Function("allowance", "uint256")]
    public class AllowanceFunction:FunctionMessage
    {
        [Parameter("address", "_owner", 1)]
        public string Owner {get; set;}
        [Parameter("address", "_spender", 2)]
        public string Spender {get; set;}
    }
}
