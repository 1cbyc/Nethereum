namespace Nethereum.ABI.FunctionEncoding
{
    public class FunctionABI
    {

        private SignatureEncoder signatureEncoder;

        public FunctionABI(string name, bool constant)
        {
            Name = name;
            Constant = constant;
            signatureEncoder = new SignatureEncoder();   
        }

        public bool Constant { get; private set; }

        public string Name { get; private set; }
 
        public Parameter[] InputParameters { get; set; }
        public Parameter[] OutputParameters { get; set; }

        private string sha3Signature;
        public string Sha3Signature
        {
            get
            {
                if (sha3Signature != null) return sha3Signature;
                sha3Signature = signatureEncoder.GenerateSha3Signature(Name, InputParameters, 4);
                return sha3Signature;
            }

        }
    }
}