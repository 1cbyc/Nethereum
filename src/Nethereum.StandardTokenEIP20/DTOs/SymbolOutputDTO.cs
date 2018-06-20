using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Nethereum.StandardTokenEIP20.DTOs
{
    [FunctionOutput]
    public class SymbolOutputDTO
    {
        [Parameter("bytes32", "", 1)]
        public string B {get; set;}
    }
}
