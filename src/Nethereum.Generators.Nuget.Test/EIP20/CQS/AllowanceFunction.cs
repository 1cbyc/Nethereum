using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.Contracts.CQS;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Generators.Nuget.Test.EIP20.DTO;
namespace Nethereum.Generators.Nuget.Test.EIP20.CQS
{
    [Function("allowance", "uint256")]
    public class AllowanceFunction:ContractMessage
    {
        [Parameter("address", "_owner", 1)]
        public string Owner {get; set;}
        [Parameter("address", "_spender", 2)]
        public string Spender {get; set;}
    }
}
