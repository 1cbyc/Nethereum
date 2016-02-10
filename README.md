# Nethereum

[![Join the chat at https://gitter.im/juanfranblanco/Ethereum.RPC](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/juanfranblanco/Ethereum.RPC?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Nethereum is a similar implementation of the Javascript Etherum Web3 RPC Client Library in .Net.

It supports most of the JSON RPC methods, together with a simplified usage of Smart contracts including the deployment, function transactions and log event filtering.

##Getting Started

If you need to start a development chain you can use [this Ethereum test genesis](https://github.com/juanfranblanco/Ethereum.TestNet.Genesis). Other useful resources are [the online solidity compiler](https://chriseth.github.io/browser-solidity/), editor with syntax higlighting [Visual Studio Code Solidity](https://marketplace.visualstudio.com/items/JuanBlanco.solidity) and of course [Mix](https://github.com/ethereum/mix/releases)

###Web 3
Create an instance of Web3, this is the wrapper to interact with an Ethereum client like Geth. The parameterless constructor uses the defaults address "http://localhost:8545/", or you can supply your own.

```csharp
    var web3 = new Web3();
```

From Web3 you can access Eth, Net, Shh RPC methods, and from Eth categorised by functionality Transactions, Filters, Mining, Compilers, etc.  

```csharp
    web3.Eth.Transactions.GetTransactionReceipt;
    web3.Eth.Transactions.Call;
    web3.Eth.Transactions.EstimateGas;
    web3.Eth.Transactions.GetTransactionByBlockHashAndIndex;
    web3.Net.PeerCount;
    web3.Eth.GetBalance;
    web3.Eth.Mining.IsMining;
    web3.Eth.Accounts;
```

Each object is an RPC command which can be executed Async as:

```csharp
    await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
```
###RPC

The communication uses the JSON RPC interface, the full documentation can be found [in the Ethereum wiki](https://github.com/ethereum/wiki/wiki/JSON-RPC)

###RPC Data Types

The simplest datatypes to communicate with Ethereum are Numeric and Data.

Numeric: A HexBigInteger data type has been created to allow the simple conversion of the input and output of numbers from the RPC.
This types also handles the conversion to and from Big Endian, together with specific usages for Eth "0x0"

```csharp
    var number = new HexBigInteger(21000);
    var anotherNumber = new HexBigInteger("0x5208");
    var hex = number.HexValue; //0x5208
    BigInteger val = anotherNumber; //2100
```

Data: The transaction, blocks, uncles, addresses hashes are treated as strings to simplify usage.

Hex strings: Similar to HexBigInteger some strings need to be encoded and decoded Hex. HexString has been created to handle the conversion using UTF8 encoding, from and to Hex.

####DTOs
There are varoious RPC Data transfer objects for Block, Transaction, FilterInput, and BlockParameter, to name a few.

The BlockParameter can be either for the latest, earliest, pending or for a given blocknumber, this is defaulted to latest on the requests that uses it. If using Web3, changing the Default Block Parameter, will cascade to all the commands.

##Working with Smart Contracts
###Deployment

Given an example of a simple "multiply" smart contract, which takes as a constructor the intial value to be multiplied, and a function "multiply" with a parameter with the value to be multiplied.

```javascript
    contract test { 
               
        uint _multiplier;
               
        function test(uint multiplier){
                   
             _multiplier = multiplier;
        }
               
        function multiply(uint a) returns(uint d) { 
                    return a * _multiplier; 
        }
    }
```

To deploy the contract first we will declare the compiled contract and abi interface (the interface is like the wsdl of a web service). You can use the [Online Solidity Compiler](https://chriseth.github.io/browser-solidity/).

```csharp
    var contractByteCode =
                "0x606060405260405160208060ae833981016040528080519060200190919050505b806000600050819055505b5060768060386000396000f360606040526000357c010000000000000000000000000000000000000000000000000000000090048063c6888fa1146037576035565b005b604b60048080359060200190919050506061565b6040518082815260200191505060405180910390f35b6000600060005054820290506071565b91905056";

    var abi =
                @"[{""constant"":false,""inputs"":[{""name"":""a"",""type"":""uint256""}],""name"":""multiply"",""outputs"":[{""name"":""d"",""type"":""uint256""}],""type"":""function""},{""inputs"":[{""name"":""multiplier"",""type"":""uint256""}],""type"":""constructor""}]";
```

Now using Web3 and our address we can deploy the contract as follows:

```csharp
    var addressFrom = "0x12890d2cce102216644c59dae5baed380d84830c";
    var web3 = new Web3();
    var transactionHash = await web3.Eth.DeployContract.SendRequestAsync(abi, contractByteCode, addressFrom, 7);
```
In this scenario we are passing a constructor parameter 7, which will be the multiplier of our contract. 

Note: Because this is a simple contract we have not passed any gas, as the default will be enough. If you were going to deploy a more complex contract ensure that enough gas is used, as the transaction might be succesful but not code will be deployed.

After deploying the contract we will need to wait for the transaction to be mined to get the contract address. The contract address can be found on the transaction receipt. 

```csharp
   TransactionReceipt receipt = null;
   while (receipt == null)
   {
        await Task.Delay(500);
        receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
    }
```

Using the address we can create a Contract object which we can interact
```csharp
  var contract = web3.Eth.GetContract(abi, receipt.ContractAddress);
  
```
###Existing contract

To  interact with an existing contract we just need the ABI and the Contract address, it is mainly the last step of deploying a contract.

```csharp
  var abi =
                @"[{""constant"":false,""inputs"":[{""name"":""a"",""type"":""uint256""}],""name"":""multiply"",""outputs"":[{""name"":""d"",""type"":""uint256""}],""type"":""function""},{""inputs"":[{""name"":""multiplier"",""type"":""uint256""}],""type"":""constructor""}]";
  var contractAddress = "0x12890d2cce102216644c59dae5baed386868686c";
  var contract = web3.Eth.GetContract(abi, contractAddress);
```

###Calling a Function

A function call allows you to execute some contract code without creating a transaction, for example in the multiplication we can execute just the code. Because we are not creating a transaction, in normal occasions no gas will be used.  

Using the contract created on the last step, we can get the function "multiply" by using its name. This is the same name as defined in the contract or ABI interface.

```csharp
    var multiplyFunction = contract.GetFunction("multiply");
    var result = await multiplyFunction.CallAsync<int>(69);
```
On the call we have identified the return type we want, in this scenario we have specified a type of int. Type conversions are supported for simple types and when returning multiple output parameters a DTO object will be necesary to be created.

##DTOs

``` csharp
   [Function(Name = "test", Sha3Signature = "c6888fa1")]
        [FunctionOutput]
        public class FunctionMultipleInputOutput
        {
            [Parameter("string")]
            public string A { get; set; }

            [Parameter("uint[20]", "b", 2)]
            public List<BigInteger> B { get; set; }

            [Parameter("string", 3)]
            public string C { get; set; }
        }

        [Fact]
        public virtual void ShouldEncodeMultipleTypesIncludingDynamicStringAndIntArray()
        {
            var paramsEncoded =
                "00000000000000000000000000000000000000000000000000000000000002c0000000000000000000000000000000000000000000000000000000000003944700000000000000000000000000000000000000000000000000000000000394480000000000000000000000000000000000000000000000000000000000039449000000000000000000000000000000000000000000000000000000000003944a000000000000000000000000000000000000000000000000000000000003944b000000000000000000000000000000000000000000000000000000000003944c000000000000000000000000000000000000000000000000000000000003944d000000000000000000000000000000000000000000000000000000000003944e000000000000000000000000000000000000000000000000000000000003944f0000000000000000000000000000000000000000000000000000000000039450000000000000000000000000000000000000000000000000000000000003945100000000000000000000000000000000000000000000000000000000000394520000000000000000000000000000000000000000000000000000000000039453000000000000000000000000000000000000000000000000000000000003945400000000000000000000000000000000000000000000000000000000000394550000000000000000000000000000000000000000000000000000000000039456000000000000000000000000000000000000000000000000000000000003945700000000000000000000000000000000000000000000000000000000000394580000000000000000000000000000000000000000000000000000000000039459000000000000000000000000000000000000000000000000000000000003945a0000000000000000000000000000000000000000000000000000000000000300000000000000000000000000000000000000000000000000000000000000000568656c6c6f0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000005776f726c64000000000000000000000000000000000000000000000000000000";

            var function = new FunctionMultipleInputOutput();
            function.A = "hello";
            function.C = "world";

            var array = new BigInteger[20];
            for (uint i = 0; i < 20; i++)
            {
                array[i] = i + 234567;
            }

            function.B = array.ToList();

            var result = new FunctionCallEncoder().EncodeRequest(function);

            Assert.Equal("0x" + "c6888fa1" + paramsEncoded, result);
        }



        [Fact]
        public virtual void ShouldDecodeMultipleTypesIncludingDynamicStringAndIntArray()
        {

            var functionCallDecoder = new FunctionCallDecoder();

            var array = new uint[20];
            for (uint i = 0; i < 20; i++)
            {
                array[i] = i + 234567;
            }

            var result = functionCallDecoder.
                DecodeOutput<FunctionMultipleInputOutput>("0x" + "00000000000000000000000000000000000000000000000000000000000002c0000000000000000000000000000000000000000000000000000000000003944700000000000000000000000000000000000000000000000000000000000394480000000000000000000000000000000000000000000000000000000000039449000000000000000000000000000000000000000000000000000000000003944a000000000000000000000000000000000000000000000000000000000003944b000000000000000000000000000000000000000000000000000000000003944c000000000000000000000000000000000000000000000000000000000003944d000000000000000000000000000000000000000000000000000000000003944e000000000000000000000000000000000000000000000000000000000003944f0000000000000000000000000000000000000000000000000000000000039450000000000000000000000000000000000000000000000000000000000003945100000000000000000000000000000000000000000000000000000000000394520000000000000000000000000000000000000000000000000000000000039453000000000000000000000000000000000000000000000000000000000003945400000000000000000000000000000000000000000000000000000000000394550000000000000000000000000000000000000000000000000000000000039456000000000000000000000000000000000000000000000000000000000003945700000000000000000000000000000000000000000000000000000000000394580000000000000000000000000000000000000000000000000000000000039459000000000000000000000000000000000000000000000000000000000003945a0000000000000000000000000000000000000000000000000000000000000300000000000000000000000000000000000000000000000000000000000000000568656c6c6f0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000005776f726c64000000000000000000000000000000000000000000000000000000"
                );

            Assert.Equal("hello", result.A);
            Assert.Equal("world", result.C);
            Assert.Equal(array[6], result.B[6]);
        }
```

### Events, logs and filters

This is a demonstration how you can raise events from your solidity contract and filter and capture them from the logs. 

```csharp
public async Task<string> Test()
        {
            //The compiled solidity contract to be deployed
            /*
           contract test { 
    
                uint _multiplier;
    
                event Multiplied(uint indexed a, uint indexed result);
    
                event MultipliedLog(uint indexed a, uint indexed result, address sender, string hello );
    
                function test(uint multiplier){
                    _multiplier = multiplier;
                }
    
                function multiply(uint a) returns(uint d) {
                    d = a * _multiplier;
                    Multiplied(a, d);
                    MultipliedLog(a, d, msg.sender, "Hello world");
                    return d;
                }
    
                function multiply1(uint a) returns(uint d) {
                    return a * _multiplier;
                }
    
                function multiply2(uint a, uint b) returns(uint d){
                    return a * b;
                }
    
            }
           
           */

            var contractByteCode =
                "0x6060604052604051602080610216833981016040528080519060200190919050505b806000600050819055505b506101db8061003b6000396000f360606040526000357c01000000000000000000000000000000000000000000000000000000009004806361325dbc1461004f578063c23f4e3e1461007b578063c6888fa1146100b05761004d565b005b61006560048080359060200190919050506101b3565b6040518082815260200191505060405180910390f35b61009a60048080359060200190919080359060200190919050506101c9565b6040518082815260200191505060405180910390f35b6100c660048080359060200190919050506100dc565b6040518082815260200191505060405180910390f35b600060006000505482029050805080827f51ae5c4fa89d1aa731ff280d425357e6e5c838c6fc8ed6ca0139ea31716bbd5760405180905060405180910390a380827fffc23845ca34f573c322267502cda1440fac565d162e9c3a5b2a9caca600d91d33604051808273ffffffffffffffffffffffffffffffffffffffff168152602001806020018281038252600b8152602001807f48656c6c6f20776f726c640000000000000000000000000000000000000000008152602001506020019250505060405180910390a38090506101ae565b919050565b6000600060005054820290506101c4565b919050565b600081830290506101d5565b9291505056";

            var abi =
                @"[{""constant"":false,""inputs"":[{""name"":""a"",""type"":""uint256""}],""name"":""multiply1"",""outputs"":[{""name"":""d"",""type"":""uint256""}],""type"":""function""},{""constant"":false,""inputs"":[{""name"":""a"",""type"":""uint256""},{""name"":""b"",""type"":""uint256""}],""name"":""multiply2"",""outputs"":[{""name"":""d"",""type"":""uint256""}],""type"":""function""},{""constant"":false,""inputs"":[{""name"":""a"",""type"":""uint256""}],""name"":""multiply"",""outputs"":[{""name"":""d"",""type"":""uint256""}],""type"":""function""},{""inputs"":[{""name"":""multiplier"",""type"":""uint256""}],""type"":""constructor""},{""anonymous"":false,""inputs"":[{""indexed"":true,""name"":""a"",""type"":""uint256""},{""indexed"":true,""name"":""result"",""type"":""uint256""}],""name"":""Multiplied"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":true,""name"":""a"",""type"":""uint256""},{""indexed"":true,""name"":""result"",""type"":""uint256""},{""indexed"":false,""name"":""sender"",""type"":""address""},{""indexed"":false,""name"":""hello"",""type"":""string""}],""name"":""MultipliedLog"",""type"":""event""}]";

            var addressFrom = "0x12890d2cce102216644c59dae5baed380d84830c";
        
            var web3 = new Web3();
            var eth = web3.Eth;
            var transactions = eth.Transactions;

            //TODO set gas
            //deploy the contract, including abi and a paramter of 7. 
            var transactionHash = await eth.DeployContract.SendRequestAsync(abi, contractByteCode, addressFrom, 7);

            //the contract should be mining now

            //get the contract address 
            TransactionReceipt receipt = null;
            //wait for the contract to be mined to the address
            while (receipt == null)
            {
                await Task.Delay(5000);
                receipt = await transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
            }

            //TODO validate gas usage and getcode 

            var contract = eth.GetContract(abi, receipt.ContractAddress);

            var multipliedEvent = contract.GetEvent("Multiplied");
            var filterAllContract = await contract.CreateFilterAsync();
            var filterAll = await multipliedEvent.CreateFilterAsync();
            //filter on the first indexed parameter
            var filter69 = await multipliedEvent.CreateFilterAsync(69);
            //filter on the second indexed parameter
            var filter49 = await multipliedEvent.CreateFilterAsync<object, int>(null, 49);
            //filter OR on the first indexed parameter
            var filter69And18 = await multipliedEvent.CreateFilterAsync(new[] { 69, 18 });


            var multipliedEventLog =  contract.GetEvent("MultipliedLog");
            var filterAllLog = await multipliedEventLog.CreateFilterAsync();

            //get the function by name
            var multiplyFunction = contract.GetFunction("multiply");
           
            //TODO set gas
            var transaction69 = await multiplyFunction.SendTransactionAsync(addressFrom, 69);
            var transaction18 = await multiplyFunction.SendTransactionAsync(addressFrom, 18);
            var transaction7 = await multiplyFunction.SendTransactionAsync(addressFrom, 7);

            var multiplyFunction2 = contract.GetFunction("multiply2");
            var callResult = await multiplyFunction2.CallAsync<int>(7, 7);

            TransactionReceipt receiptTransaction = null;

            while (receiptTransaction == null)
            {
                await Task.Delay(5000);
                receiptTransaction = await transactions.GetTransactionReceipt.SendRequestAsync(transaction7);
            }

            var logs = await eth.Filters.GetFilterChangesForEthNewFilter.SendRequestAsync(filterAllContract);    
            var eventLogsAll = await multipliedEvent.GetFilterChanges<EventMultiplied>(filterAll);
            var eventLogs69 = await multipliedEvent.GetFilterChanges<EventMultiplied>(filter69);
            var eventLogsResult49 = await multipliedEvent.GetFilterChanges<EventMultiplied>(filter49);
            var eventLogsFor69and18 = await multipliedEvent.GetFilterChanges<EventMultiplied>(filter69And18);

            var multipliedLogEvents = await multipliedEventLog.GetFilterChanges<EventMultipliedLog>(filterAllLog);
            
            return "All logs :" + eventLogsAll.Count + " Logs for 69 " + eventLogs69.Count;

        } 

        public class EventMultiplied
        {
            [Parameter("uint", "a", 1, true)]
            public int A { get; set; }

            [Parameter("uint", "result", 2, true)]
            public int Result { get; set; }

            

        }

        public class EventMultipliedLog
        {
            [Parameter("uint", "a", 1, true)]
            public int A { get; set; }

            [Parameter("uint", "result", 2, true)]
            public int Result { get; set; }

            [Parameter("address", "sender", 3, false)]
            public string Sender { get; set; }

            
            [Parameter("string", "hello", 4, false)]
            public string Hello { get; set; }

        }
    }
}
```




### Running on Linux
* Install DNX following the [asp.net guide](http://docs.asp.net/en/latest/getting-started/installing-on-linux.html). (Use coreclr)
* Run dnu restore at the solution level, to restore all packages and dependencies.
    ```dnu restore ```
* Compile using only dnxcore 
    ```dnu build --framework dnxcore50 ```
* Run using dnx (ie dnx text, dnx run)

### Current TODO
This is the current TODO list in order of priority 
* ~~Generic RPC~~
* ~~ABI encoding, contract deployment, contract transaction and calls example, together with an end to end example on how Ethereum works~~.
* ~~Windows / Linux deployment and unit test~~
* ~~Filters, Events and Logging, together with and end to end example for reference on how Ethereum works.~~
* ~~Refactor~~ 
* ~~BigIntegers everywhere as opposed to long / int64~~
* ~~Hex Types (BigInteger, String) to simplify Rpc encoding~~
* ~~ABI Encoding decoding simplification using DTO pattern and attributes for encoding / decoding values~~
* ~~Extract projects for RPC / ABI / Web3~~
* ~~Create Web3 similar wrapper (wont be the same for contracts / functions) to simplify usage (ie Web3.Eth.Get..)~~
* ~~Complete Eth, Net partial Shh~~
* Add wei conversion support
* ~~Simplify Filtering / Events encoding/decoding on Web3~~
* Examples / Documentation
* Nuget (beta)
* Other shh methods
* Code generate Typed DTO Functions to simplify usage 
* Example of windows universal app using a contract (Windows, Mobile, RPI2)
* Example of unit testing contracts (.net driven)
* Example of using [dapple / dappsys](https://github.com/NexusDevelopment/dapple) unit testing (solidity driven).

