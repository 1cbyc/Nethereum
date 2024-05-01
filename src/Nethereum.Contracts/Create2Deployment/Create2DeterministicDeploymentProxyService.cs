﻿using System;
using System.Threading.Tasks;
using Nethereum.Contracts.Services;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Hex.HexTypes;
using System.Numerics;
using Nethereum.Model;
using Nethereum.Util;
using System.Diagnostics;

namespace Nethereum.Contracts.Create2Deployment
{
    /// <summary>
    /// Deterministic Deployment Proxy Service supporting https://github.com/Arachnid/deterministic-deployment-proxy.git and extended to support EIP155
    /// 
    /// Use in combination with the Create2DeterministicDeployment object to create EIP155 create2 deployments or the default legacy deployments
    /// 
    /// EIP155 support is added by using the ChainId to calculate the V value and Legacy transaction signing, note this will not provide the same address for all chains as per the legacy deployment
    /// </summary>
    public class Create2DeterministicDeploymentProxyService
    {
        private readonly IEthApiContractService _ethApiContractService;

        public Create2DeterministicDeploymentProxyService(IEthApiContractService ethApiContractService)
        {
            _ethApiContractService = ethApiContractService;
        }

#if !DOTNET35
        public async Task<bool> HasProxyBeenDeployedAsync(string address)
        {
            var code = await _ethApiContractService.GetCode.SendRequestAsync(address);
            return !string.IsNullOrEmpty(code?.RemoveHexPrefix()) && code.Length >= Create2DeterministicDeploymentProxyDeployment.RuntimeByteCode.Length;
               
        }

        /// <summary>
        /// Create a deterministic deployment EIP155 using the predefined Create2DeterministicDeploymentProxyDeployment ByteCode and the current Account and chainId
        /// configured in Web3 as the signer
        /// </summary>
        public async Task<Create2DeterministicDeploymentProxyDeployment> GenerateEIP155DeterministicDeploymentAsync
            (
                long gasPrice = Create2DeterministicDeploymentProxyDeployment.DefaultGasPrice,
                long gasLimit = Create2DeterministicDeploymentProxyDeployment.DefaultGasLimit,
                long nonce =0
            )
        {
            var chainId = _ethApiContractService.TransactionManager.ChainId;
            if (chainId == null)
            {
                chainId = await _ethApiContractService.ChainId.SendRequestAsync();
            }
            var transactionInput = new TransactionInput()
            {
                From = _ethApiContractService.TransactionManager.Account.Address,
                GasPrice = new HexBigInteger(gasPrice),
                Gas = new HexBigInteger(gasLimit),
                Data = Create2DeterministicDeploymentProxyDeployment.ByteCode,
                Value = new HexBigInteger(0),
                Nonce = new HexBigInteger(nonce)
            };

            var rawTransaction = await _ethApiContractService.TransactionManager.SignTransactionAsync(transactionInput);
            var contractAddress = ContractUtils.CalculateContractAddress(transactionInput.From, (int)nonce);
            return new Create2DeterministicDeploymentProxyDeployment()
            {
                GasLimit = gasLimit,
                GasPrice = gasPrice,
                RawTransaction = rawTransaction,
                SignerAddress = transactionInput.From,
                Address = contractAddress,
                ChainId = chainId
            };
            
        }



        /// <summary>
        /// Create a deterministic deployment EIP155 for the current chain using the predefined Create2DeterministicDeploymentProxyDeployment ByteCode and recovery signature
        /// </summary>
        /// <remarks>
        /// The proxy address and signer address will be different depending on the chain, use GenerateEIP155DeterministicDeploymentAsync with your own private key to get the same address for all chains
        /// </remarks>
        public async Task<Create2DeterministicDeploymentProxyDeployment> GenerateEIP155DeterministicDeploymentUsingPreconfiguredSignatureAsync()
        {
            var chainId = _ethApiContractService.TransactionManager.ChainId;
            if(chainId == null)
            {
                 chainId = await _ethApiContractService.ChainId.SendRequestAsync();
            }
            return GenerateEIP155DeterministicDeploymentUsingPreconfiguredSignatureAsync(chainId.Value);
        }

        /// <summary>
        /// Create a deterministic deployment for a chain using the predefined Create2DeterministicDeploymentProxyDeployment ByteCode and recovery signature
        /// </summary>
        /// <remarks>
        /// The proxy address and signer address will be different depending on the chain, use GenerateEIP155DeterministicDeploymentAsync with your own private key to get the same address for all chains
        /// </remarks>
        public Create2DeterministicDeploymentProxyDeployment GenerateEIP155DeterministicDeploymentUsingPreconfiguredSignatureAsync(BigInteger chainId)
        {
            var nonce = 0;
            var legacyTransactionChainId = new LegacyTransactionChainId(
                "",
                0,
                nonce,
                Create2DeterministicDeploymentProxyDeployment.DefaultGasPrice,
                Create2DeterministicDeploymentProxyDeployment.DefaultGasLimit,
                Create2DeterministicDeploymentProxyDeployment.ByteCode,
                chainId,
                Create2DeterministicDeploymentProxyDeployment.DefaultR,
                Create2DeterministicDeploymentProxyDeployment.DefaultS,
                Create2DeterministicDeploymentProxyDeployment.CalculateVForChainIdAsBytes(chainId));
             var rawTransaction = legacyTransactionChainId.GetRLPEncoded();
            
             var signerAddress = _ethApiContractService.TransactionManager.TransactionVerificationAndRecovery.GetSenderAddress(legacyTransactionChainId);
             var address = ContractUtils.CalculateContractAddress(signerAddress,nonce);

            return new Create2DeterministicDeploymentProxyDeployment()
            {
                GasLimit = Create2DeterministicDeploymentProxyDeployment.DefaultGasLimit,
                GasPrice = Create2DeterministicDeploymentProxyDeployment.DefaultGasPrice,
                RawTransaction = rawTransaction.ToHex(),
                SignerAddress = signerAddress,
                Address = address,
                ChainId = chainId
            };

        }

        public async Task<string> DeployProxyAndGetContractAddressAsync(Create2DeterministicDeploymentProxyDeployment deployment)
        {
            if (await HasProxyBeenDeployedAsync(deployment.Address)) return deployment.Address;

            await ValidateAndSendEnoughBalanceForProxyDeploymentAndWaitForReceiptAsync(deployment);
            var currentLegacySetting = _ethApiContractService.TransactionManager.UseLegacyAsDefault;
         
            _ethApiContractService.TransactionManager.UseLegacyAsDefault = true;
            
            var txn = await _ethApiContractService.Transactions.SendRawTransaction.SendRequestAsync(deployment.RawTransaction);
            
            _ethApiContractService.TransactionManager.UseLegacyAsDefault = currentLegacySetting;

           var receipt = await _ethApiContractService.TransactionManager.TransactionReceiptService.PollForReceiptAsync(txn);
            var deployed = await HasProxyBeenDeployedAsync(deployment.Address);
            if(!deployed) throw new Exception("Proxy not deployed");
            return deployment.Address; 
        }

        public async Task ValidateAndSendEnoughBalanceForProxyDeploymentAndWaitForReceiptAsync(Create2DeterministicDeploymentProxyDeployment deployment)
        {
            var gasRequired = deployment.GasLimit * deployment.GasPrice;
            var currentBalance = await _ethApiContractService.GetBalance.SendRequestAsync(deployment.SignerAddress);
            var gasNeeded = (BigInteger)gasRequired - currentBalance;
            if (gasNeeded > 0)
            {
                var gasPaymentTransactionReceipt = await _ethApiContractService.TransactionManager.SendTransactionAndWaitForReceiptAsync(
                    new TransactionInput()
                    {
                        From = _ethApiContractService.TransactionManager.Account.Address,
                        To = deployment.SignerAddress,
                        Value = new HexBigInteger(gasNeeded),
                    });

            }
        }

        public string CalculateCreate2Address(string deployerProxyAddress, string salt, string contractByteCode)
        {
            return ContractUtils.CalculateCreate2Address(deployerProxyAddress, salt, contractByteCode);
        }

        public async Task<bool> CheckContractAlreadyDeployedAsync(string deployerProxyAddress, string salt, string contractByteCode)
        {
            var create2Address = ContractUtils.CalculateCreate2Address(deployerProxyAddress, salt, contractByteCode);
            var code = await _ethApiContractService.GetCode.SendRequestAsync(create2Address);
            return !string.IsNullOrEmpty(code?.RemoveHexPrefix());
        }

        public async Task<bool> CheckContractAlreadyDeployedAsync(string address)
        {
            var code = await _ethApiContractService.GetCode.SendRequestAsync(address);
            return !string.IsNullOrEmpty(code?.RemoveHexPrefix());
        }

        public async Task<string> DeployContractRequestAsync(string deployerProxyAddress, string salt, string contractByteCode)
        {
            if (await CheckContractAlreadyDeployedAsync(deployerProxyAddress, salt, contractByteCode))
                throw new Exception("Contract already deployed");
            var transactionInput = new TransactionInput()
            {
                From = _ethApiContractService.TransactionManager.Account.Address,
                Data = salt.EnsureHexPrefix() + contractByteCode.RemoveHexPrefix(),
                To = deployerProxyAddress
            };

            var gas = await _ethApiContractService.Transactions.EstimateGas.SendRequestAsync(transactionInput);
            transactionInput.Gas = gas;
            return await _ethApiContractService.TransactionManager.SendTransactionAsync(transactionInput);
        }

        public async Task<TransactionReceipt> DeployContractRequestAndWaitForReceiptAsync(string deployerProxyAddress, string salt, string contractByteCode)
        {
            var txnHash = await DeployContractRequestAsync(deployerProxyAddress, salt, contractByteCode);
            var receipt = await _ethApiContractService.TransactionManager.TransactionReceiptService.PollForReceiptAsync(txnHash);
            if (await CheckContractAlreadyDeployedAsync(deployerProxyAddress, salt, contractByteCode))
            {
                return receipt;
            }
            throw new Exception("Contract not deployed");
        }
#endif
    }
}
