using System;
using System.Text;
using NBitcoin.Crypto;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Util;

namespace Nethereum.Signer
{
    public class MessageSigner
    {
        public virtual string EcRecover(byte[] hashMessage, string signature)
        {
            var ecdaSignature = ExtractEcdsaSignature(signature);
            return EthECKey.RecoverFromSignature(ecdaSignature, hashMessage).GetPublicAddress();
        }

        public byte[] Hash(byte[] plainMessage)
        {
            var hash = new Sha3Keccack().CalculateHash(plainMessage);
            return hash;
        }

        public string HashAndEcRecover(string plainMessage, string signature)
        {
            return EcRecover(Hash(Encoding.UTF8.GetBytes(plainMessage)), signature);
        }

        public string HashAndSign(string plainMessage, string privateKey)
        {
            return HashAndSign(Encoding.UTF8.GetBytes(plainMessage), new ECKey(privateKey.HexToByteArray(), true));
        }

        public string HashAndSign(byte[] plainMessage, string privateKey)
        {
            return HashAndSign(plainMessage, new ECKey(privateKey.HexToByteArray(), true));
        }

        public virtual string HashAndSign(byte[] plainMessage, ECKey key)
        {
            var hash = Hash(plainMessage);
            var signature = key.SignAndCalculateV(hash);
            return CreateStringSignature(signature);
        }

        public string Sign(byte[] message, string privateKey)
        {
            return Sign(message, new ECKey(privateKey.HexToByteArray(), true));
        }

        public virtual string Sign(byte[] message, ECKey key)
        {
            var signature = key.SignAndCalculateV(message);
            return CreateStringSignature(signature);
        }

        public string Sign(string plainMessage, string privateKey)
        {
            return Sign(Encoding.UTF8.GetBytes(plainMessage), privateKey);
        }

        private static string CreateStringSignature(ECDSASignature signature)
        {
            return signature.R.ToByteArrayUnsigned().ToHex(true) +
                   signature.S.ToByteArrayUnsigned().ToHex() +
                   signature.V.ToString("X2");
        }

        private static ECDSASignature ExtractEcdsaSignature(string signature)
        {
            var signatureArray = signature.HexToByteArray();

            var v = signatureArray[64];

            if ((v == 0) || (v == 1))
                v = (byte) (v + 27);

            var r = new byte[32];
            Array.Copy(signatureArray, r, 32);
            var s = new byte[32];
            Array.Copy(signatureArray, 32, s, 0, 32);

            var ecdaSignature = EthECDSASignatureFactory.FromComponents(r, s, v);
            return ecdaSignature;
        }
    }
}