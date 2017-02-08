rem packing web3 and dependencies
del /S *.*.nupkg
cd Nethereum.Hex
dotnet pack
cd ..
cd Nethereum.ABI
dotnet pack
cd ..
cd Nethereum.JsonRpc.Client
dotnet pack
cd ..
cd Nethereum.RPC
dotnet pack
cd ..
cd Nethereum.Web3
dotnet pack
cd ..
cd Nethereum.StandardToken*
dotnet pack
cd ..
cd Nethereum.JsonRpc.IpcClient*
dotnet pack
cd ..
cd Nethereum.KeyStore*
dotnet pack
cd ..
cd Nethereum.ENS*
dotnet pack
cd ..
cd Nethereum.Quorum*
dotnet pack
cd ..
cd Nethereum.Geth*
dotnet pack
cd ..
cd Nethereum.Contracts*
dotnet pack
cd ..
cd Nethereum.RLP*
dotnet pack
cd ..
cd Nethereum.Signer*
dotnet pack
cd ..
cd Nethereum.Util*
dotnet pack
cd ..
setlocal
set DIR=%~dp0
set OUTPUTDIR=%~dp0\packages
for /R %DIR% %%a in (*.nupkg) do xcopy "%%a" "%OUTPUTDIR%"
xcopy *.nupkg packages /s /y