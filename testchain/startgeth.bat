RD /S /Q %~dp0\devChain\chainData
RD /S /Q %~dp0\devChain\dapp
RD /S /Q %~dp0\devChain\nodes
del %~dp0\devchain\nodekey

geth  --datadir=devChain init genesis_dev.json
geth  --rpc --networkid=39318 --maxpeers=0 --datadir=devChain  --rpccorsdomain "*" --rpcapi "eth,web3,personal,net,miner,admin" --ipcapi "eth,web3,personal,net,miner,admin" --verbosity 0 console  