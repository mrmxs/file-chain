# Commands

## Build and run on server

### ganache-cli

Generate private blockchain network with Ganache

`ganache-cli -p 7545 -l 6721975 -g 20000000000 -v -e 100000000`

### DeployApp - deploy smart contracts

````
params
-wa=0xcc03511b073b145a013ad788f7f95a5100dfd94f  <---- wallet
-wp=0x0ffe15e7a91070fdecd9b0f193987660d207b3bbe66903d26395e50abbf71145 <---- password
-al=admin      <---- admin login
-ap=qwerty123  <---- admin password
[-afn=John     <---- admin first name ]
[-aln=Doe      <---- admin last name  ]
[-rcp          <---- RCP client host  ]
[-lib          <---- library address  ]
[-gas          <---- Gas              ]
````

Run DeployApp

`dotnet ~/DocChain/DeployApp/bin/Debug/netcoreapp2.1/DeployApp.dll -wa=0xcc03511b073b145a013ad788f7f95a5100dfd94f -wp=0x0ffe15e7a91070fdecd9b0f193987660d207b3bbe66903d26395e50abbf71145 -al=admin -ap=qwerty123`


### API - C# api to call smart contract functions

````
export ASPNETCORE_ENVIRONMENT=Production
export ASPNETCORE_URLS=http://localhost:5000

dotnet build ~/DocChain/API/API.csproj
dotnet ~/DocChain/API/bin/Debug/netcoreapp2.1/API.dll
````

## Other

Save wallet address to environment variable

`DOCCHAIN_CONTRACT_ADDRESS=0x5ca0ec73816cdee7eacc2a6c69410d6e47bd6855`

Save contract address to environment variable

`DOCCHAIN_CONTRACT_ADDRESS=0x5ca0ec73816cdee7eacc2a6c69410d6e47bd6855`
