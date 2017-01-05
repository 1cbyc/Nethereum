using System.Collections.Generic;
using System.Text;
using NBitcoin.Crypto;
using Nethereum.Hex.HexConvertors.Extensions;

namespace Nethereum.Core
{
    public class GethSigner:SimpleSigner
    {
   
        public override string HashAndSign(byte[] plainMessage, ECKey key)
        {
            return base.Sign(HashAndHashPrefixedMessage(plainMessage), key);
        }

        public byte[] HashAndHashPrefixedMessage(byte[] message)
        {
            return HashPrefixedMessage(Hash(message));
        }

        public byte[] HashPrefixedMessage(byte[] message)
        {
            var byteList = new List<byte>();
            var bytePrefix = "0x19".HexToByteArray();
            var textBytePrefix = Encoding.UTF8.GetBytes("Ethereum Signed Message:\n" + message.Length);
           
            byteList.AddRange(bytePrefix);
            byteList.AddRange(textBytePrefix);
            byteList.AddRange(message);
            return Hash(byteList.ToArray());
        }

        public override string Sign(byte[] message, ECKey key)
        {
            return base.Sign(HashPrefixedMessage(message), key);
        }

        public override string EcRecover(byte[] message, string signature)
        {
            return base.EcRecover(HashPrefixedMessage(message), signature);
        }
    }
}