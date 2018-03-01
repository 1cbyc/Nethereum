using Nethereum.ABI.Model;

namespace Nethereum.Generator.Console
{
    public class FunctionOutputDTOModel
    {
        private CommonGenerators commonGenerators;

        public FunctionOutputDTOModel()
        {
            commonGenerators = new CommonGenerators();
        }
        
        public string GetFunctionOutputTypeName(FunctionABI functionABI)
        {
            return GetFunctionOutputTypeName(functionABI.Name);
        }

        public string GetFunctionOutputTypeName(string functionName)
        {
            return $"{commonGenerators.GenerateClassName(functionName)}OutputDTO";
        }

        public bool CanGenerateOutputDTO(FunctionABI functionABI)
        {
            return functionABI.OutputParameters != null && functionABI.OutputParameters.Length > 0 &&
                   functionABI.Constant;
        }
    }
}