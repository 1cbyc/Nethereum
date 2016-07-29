using System.Threading;
using System.Threading.Tasks;
using Nethereum.Web3;
using Xunit;

namespace SimpleTests
{
    public class TransactionSigningTests
    {
        [Fact]
        public async Task<bool> ShouldSignAndSendRawTransaction()
        {
            var privateKey = "b5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
            var senderAddress = "12890d2cce102216644c59daE5baed380d84830c";
            var receiveAddress = "0x13f022d72158410433cbd66f5dd8bf6d2d129924";
            var web3 = new Web3();
            var txCount = await web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(senderAddress);
            var encoded = web3.OfflineTransactionSigning.SignTransaction(privateKey, receiveAddress, 10,
                txCount.Value);
            var txId = await web3.Eth.Transactions.SendRawTransaction.SendRequestAsync("0x" + encoded);
            await web3.Miner.Start.SendRequestAsync(4);
            var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txId);
            while (receipt == null)
            {
                Thread.Sleep(1000);
                receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txId);
            }

            Assert.Equal(txId, receipt.TransactionHash);
            return true;
        }
    }
}