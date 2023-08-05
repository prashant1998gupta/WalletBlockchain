using UnityEngine;
using UnityEngine.UI;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;
using System.Threading.Tasks;
using Nethereum.Contracts.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.Standards.ENS.Registrar.ContractDefinition;
using System.Numerics;
using Nethereum.Hex.HexConvertors.Extensions;
using System.Collections.Generic;
using Nethereum.Util;
using System;
using System.Text.RegularExpressions;
using Nethereum.Contracts.Standards.ERC721;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using Nethereum.Signer;
using Nethereum.Contracts.TransactionHandlers;


public class TokenTransfer : MonoBehaviour
{
    public const string contractAddressMLD = "0x64BD211a17bF2902C38AC2E55Bd3db58E00aCBf4";
    public const string contractAddressLord = "0x0F57E4ae054083178293bCC0c20c3387eBcaceB2";
   // public const string contractAddressMarkatePlace = "0x12d514e227c4aC7a0B2B2ee5D365eF8054B490F6"; // Replace with the contract address of the NFT
    public const string contractAddressMarkatePlace = "0x9D1DBd9fB855a091Afe7B14e1eD9B462E8E5F7C0"; // Replace with the contract address of the NFT

    int chainId = 80001; // testnet (Mumbai), the Chain ID is 80001
                         // int chainId = 137; //Ethereum mainnet is 137

    // const string rpcURL = "https://rpc.ankr.com/polygon_mumbai"; // On the Polygon testnet (Mumbai), the Chain ID is 80001.
    //const string rpcURL = "https://rpc.ankr.com/eth_sepolia"; // On the Polygon testnet (Mumbai), the Chain ID is 80001.
    const string rpcURL = "https://rpc-mumbai.maticvigil.com"; //  The Chain ID for Polygon (formerly known as Matic Network) on the Ethereum mainnet is 137.


    // Replace YOUR_TOKEN_ABI with the ABI of your token contract
    private const string MLD_ABI = "[{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_communityTreasury\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"_partnersTreasury\",\"type\":\"address\"}],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"time\",\"type\":\"uint256\"}],\"name\":\"ExtraTokenUnlocks\",\"type\":\"error\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"Approval\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"delegator\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"fromDelegate\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"toDelegate\",\"type\":\"address\"}],\"name\":\"DelegateChanged\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"delegate\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"previousBalance\",\"type\":\"uint256\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"newBalance\",\"type\":\"uint256\"}],\"name\":\"DelegateVotesChanged\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"previousOwner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"OwnershipTransferred\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"Transfer\",\"type\":\"event\"},{\"inputs\":[],\"name\":\"DOMAIN_SEPARATOR\",\"outputs\":[{\"internalType\":\"bytes32\",\"name\":\"\",\"type\":\"bytes32\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"}],\"name\":\"allowance\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"approve\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"_amount\",\"type\":\"uint256\"}],\"name\":\"burn\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"internalType\":\"uint32\",\"name\":\"pos\",\"type\":\"uint32\"}],\"name\":\"checkpoints\",\"outputs\":[{\"components\":[{\"internalType\":\"uint32\",\"name\":\"fromBlock\",\"type\":\"uint32\"},{\"internalType\":\"uint224\",\"name\":\"votes\",\"type\":\"uint224\"}],\"internalType\":\"struct ERC20Votes.Checkpoint\",\"name\":\"\",\"type\":\"tuple\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"communityAllocation\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"communityTreasury\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"contributorsAllocation\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"decimals\",\"outputs\":[{\"internalType\":\"uint8\",\"name\":\"\",\"type\":\"uint8\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"subtractedValue\",\"type\":\"uint256\"}],\"name\":\"decreaseAllowance\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"delegatee\",\"type\":\"address\"}],\"name\":\"delegate\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"delegatee\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"nonce\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"expiry\",\"type\":\"uint256\"},{\"internalType\":\"uint8\",\"name\":\"v\",\"type\":\"uint8\"},{\"internalType\":\"bytes32\",\"name\":\"r\",\"type\":\"bytes32\"},{\"internalType\":\"bytes32\",\"name\":\"s\",\"type\":\"bytes32\"}],\"name\":\"delegateBySig\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"delegates\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"blockNumber\",\"type\":\"uint256\"}],\"name\":\"getPastTotalSupply\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"blockNumber\",\"type\":\"uint256\"}],\"name\":\"getPastVotes\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"getVotes\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"addedValue\",\"type\":\"uint256\"}],\"name\":\"increaseAllowance\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"lastMinted\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"_amount\",\"type\":\"uint256\"}],\"name\":\"mintExtraToken\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"mintYearlyToken\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"mintingYear\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"mintingYearData\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"mintingUnlockTime\",\"type\":\"uint256\"},{\"internalType\":\"bool\",\"name\":\"isMinted\",\"type\":\"bool\"},{\"internalType\":\"uint256\",\"name\":\"maxMintingSupply\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"name\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"}],\"name\":\"nonces\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"numCheckpoints\",\"outputs\":[{\"internalType\":\"uint32\",\"name\":\"\",\"type\":\"uint32\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"owner\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"partnersAllocation\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"partnersTreasury\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"deadline\",\"type\":\"uint256\"},{\"internalType\":\"uint8\",\"name\":\"v\",\"type\":\"uint8\"},{\"internalType\":\"bytes32\",\"name\":\"r\",\"type\":\"bytes32\"},{\"internalType\":\"bytes32\",\"name\":\"s\",\"type\":\"bytes32\"}],\"name\":\"permit\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"renounceOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"symbol\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"totalSupply\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"transfer\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"transferFrom\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"transferOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_communityTreasury\",\"type\":\"address\"}],\"name\":\"updateCommunityTreasury\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_partnersTreasury\",\"type\":\"address\"}],\"name\":\"updatePartnersTreasury\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";
    private const string LORD_ABI = "[{\"inputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"Approval\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"previousOwner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"OwnershipTransferred\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"Transfer\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"}],\"name\":\"allowance\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"approve\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"burn\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"burnFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"decimals\",\"outputs\":[{\"internalType\":\"uint8\",\"name\":\"\",\"type\":\"uint8\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"subtractedValue\",\"type\":\"uint256\"}],\"name\":\"decreaseAllowance\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"addedValue\",\"type\":\"uint256\"}],\"name\":\"increaseAllowance\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"mint\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"name\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"owner\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"renounceOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"symbol\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"totalSupply\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"transfer\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"transferFrom\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"transferOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";
    private const string MarkatePlace_contractABI = "[{\"inputs\":[{\"internalType\":\"address payable\",\"name\":\"_feeReceiver\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"_platformFeesPercentage\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"_royaltyPercentage\",\"type\":\"uint256\"}],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"contract IERC721\",\"name\":\"nftContractAddress\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"tokenID\",\"type\":\"uint256\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"bidder\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"bidAmount\",\"type\":\"uint256\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"biddingTime\",\"type\":\"uint256\"}],\"name\":\"BidPlaced\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"contract IERC721\",\"name\":\"nftContractAddress\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"tokenID\",\"type\":\"uint256\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"bidder\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"withdrawalTime\",\"type\":\"uint256\"}],\"name\":\"BidWithdrawn\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"contract IERC721\",\"name\":\"nftContractAddress\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"tokenID\",\"type\":\"uint256\"},{\"indexed\":false,\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"address\",\"name\":\"previousOwner\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"price\",\"type\":\"uint256\"},{\"indexed\":false,\"internalType\":\"contract IERC20\",\"name\":\"currency\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"timestamp\",\"type\":\"uint256\"}],\"name\":\"NFTtraded\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"contract IERC721\",\"name\":\"nftAddress\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"},{\"indexed\":false,\"internalType\":\"contract IERC20\",\"name\":\"listingCurrency\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"price\",\"type\":\"uint256\"},{\"indexed\":false,\"internalType\":\"bool\",\"name\":\"isAtAuction\",\"type\":\"bool\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"listingTime\",\"type\":\"uint256\"}],\"name\":\"NftListed\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"previousOwner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"OwnershipTransferred\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"contract IERC721\",\"name\":\"_nftContract\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"_tokenID\",\"type\":\"uint256\"}],\"name\":\"acceptHighestBid\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"contract IERC721\",\"name\":\"_nftContract\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"_tokenID\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"_biddingAmount\",\"type\":\"uint256\"}],\"name\":\"bid\",\"outputs\":[],\"stateMutability\":\"payable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"contract IERC721\",\"name\":\"_nftContract\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"_tokenID\",\"type\":\"uint256\"}],\"name\":\"buyAtFixedPrice\",\"outputs\":[],\"stateMutability\":\"payable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"contract IERC20\",\"name\":\"_currency\",\"type\":\"address\"}],\"name\":\"getCurrencyVolumeTraded\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"volumeTraded\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"contract IERC721\",\"name\":\"_nftContract\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"_tokenID\",\"type\":\"uint256\"}],\"name\":\"getNFTInfo\",\"outputs\":[{\"components\":[{\"internalType\":\"address payable\",\"name\":\"owner\",\"type\":\"address\"},{\"internalType\":\"contract IERC20\",\"name\":\"tradeCurrency\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"price\",\"type\":\"uint256\"},{\"internalType\":\"enum MetaspaceNFTMarketplace.ListingStage\",\"name\":\"listingStage\",\"type\":\"uint8\"},{\"internalType\":\"address\",\"name\":\"highestBidder\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"highestBid\",\"type\":\"uint256\"}],\"internalType\":\"struct MetaspaceNFTMarketplace.NFTInfo\",\"name\":\"\",\"type\":\"tuple\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"contract IERC20\",\"name\":\"_currency\",\"type\":\"address\"}],\"name\":\"getTotalFeesCollected\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"platformFeesEarned\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"royaltyEarned\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"contract IERC20\",\"name\":\"\",\"type\":\"address\"}],\"name\":\"isSupportedCurrency\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"contract IERC721\",\"name\":\"\",\"type\":\"address\"}],\"name\":\"isSupportedNFT\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"contract IERC721\",\"name\":\"_nftContractAddress\",\"type\":\"address\"},{\"internalType\":\"uint256[]\",\"name\":\"_tokenIds\",\"type\":\"uint256[]\"},{\"internalType\":\"contract IERC20\",\"name\":\"_currency\",\"type\":\"address\"},{\"internalType\":\"uint256[]\",\"name\":\"_price\",\"type\":\"uint256[]\"},{\"internalType\":\"bool[]\",\"name\":\"_listAsAuction\",\"type\":\"bool[]\"}],\"name\":\"listInBatch\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"contract IERC721\",\"name\":\"_nftContractAddress\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"_tokenID\",\"type\":\"uint256\"},{\"internalType\":\"contract IERC20\",\"name\":\"_currency\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"_price\",\"type\":\"uint256\"},{\"internalType\":\"bool\",\"name\":\"_listAsAuction\",\"type\":\"bool\"}],\"name\":\"listNFT\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"contract IERC721\",\"name\":\"\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"nftInfo\",\"outputs\":[{\"internalType\":\"address payable\",\"name\":\"owner\",\"type\":\"address\"},{\"internalType\":\"contract IERC20\",\"name\":\"tradeCurrency\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"price\",\"type\":\"uint256\"},{\"internalType\":\"enum MetaspaceNFTMarketplace.ListingStage\",\"name\":\"listingStage\",\"type\":\"uint8\"},{\"internalType\":\"address\",\"name\":\"highestBidder\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"highestBid\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"owner\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"platformData\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"platformFeesPercentage\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"royaltyPercentage\",\"type\":\"uint256\"},{\"internalType\":\"address payable\",\"name\":\"feeReceiver\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"totalNFTSold\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"renounceOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"transferOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"_newCommission\",\"type\":\"uint256\"}],\"name\":\"updatePlatformFeesPercentage\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"_newRoyaltyPercentage\",\"type\":\"uint256\"}],\"name\":\"updateRoyaltyFeePercentage\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"contract IERC20\",\"name\":\"_currency\",\"type\":\"address\"},{\"internalType\":\"bool\",\"name\":\"updateStatus\",\"type\":\"bool\"}],\"name\":\"updateSupportedCurrency\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"contract IERC721\",\"name\":\"_nftContractAddress\",\"type\":\"address\"},{\"internalType\":\"bool\",\"name\":\"updateStatus\",\"type\":\"bool\"}],\"name\":\"updateSupportedNFT\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"contract IERC721\",\"name\":\"_nftContract\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"_tokenID\",\"type\":\"uint256\"}],\"name\":\"withdrawHighestBid\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"stateMutability\":\"payable\",\"type\":\"receive\"}]";

    // privateKey
    private string privateKey;

    //contracts
    private Contract contractMLD;
    private Contract contractLord;
    private Contract contractMarkatePlace;

    private Nethereum.Contracts.Event eventReceivedMLD;
    private Nethereum.Contracts.Event eventReceivedLORD;

    private Web3 web3;
    private Account account;
    Web3AuthSample web3AuthScript;

    private void Awake()
    {
        /* web3AuthScript = FindObjectOfType<Web3AuthSample>();
         web3AuthScript.onWeb3authLoginInit += Init;*/
        Init();
    }

    private void Start()
    {
        //Init();
    }

    private async void Init()
    {
        privateKey = PlayerPrefs.GetString("privKey");
        Debug.Log($"privKey {privateKey}");
        var newAccount = new Account(privateKey, chainId);
        account = newAccount;
        Prashant.Utils.userWalletAccount = account.Address;
        web3 = new Web3(account, rpcURL);
        PlayerPrefs.SetString("userWalletAccount", Prashant.Utils.userWalletAccount);



        // Load the token contract
        contractMLD = web3.Eth.GetContract(MLD_ABI, contractAddressMLD);
        contractLord = web3.Eth.GetContract(LORD_ABI, contractAddressLord);
        contractMarkatePlace = web3.Eth.GetContract(MarkatePlace_contractABI, contractAddressMarkatePlace);

        // Subscribe to the event
        eventReceivedMLD = contractMLD.GetEvent("Transfer");
        eventReceivedLORD = contractLord.GetEvent("Transfer");

        Debug.Log($"privKey {privateKey} and account address {account.Address}");

        // await GetTransactionHistory(account.Address);

        //await GetListedListedNFTs();

        // await BuyNFT(0);

    }

    public static bool IsEthereumAddressValid(string address)
    {
        if (string.IsNullOrEmpty(address))
            return false;

        // Check if the address starts with '0x'
        if (!address.StartsWith("0x"))
            return false;

        // Remove the '0x' prefix for further validation
        address = address.Substring(2);

        // Check if the remaining characters are all hexadecimal
        Regex hexRegex = new Regex("^[0-9a-fA-F]+$");
        if (!hexRegex.IsMatch(address))
            return false;

        // Check if the address has exactly 40 characters after removing the '0x' prefix
        if (address.Length != 40)
            return false;

        return true;
    }

    #region sendToken

    public async Task<TransferResult> SendTokensMLD(string recipientAddress, string amount)
    {
        TransferResult result = new TransferResult();

        if (recipientAddress.Equals(string.Empty) || amount.Equals(string.Empty))
        {
            Debug.Log("Please enter recipientAddress and amount and in correct formate");

            result.StatusCode = -1;
            result.Message = "Please enter recipientAddress and amount in correct format";
            return result;
        }
        if(!IsEthereumAddressValid(recipientAddress))
        {
            Debug.Log("Please enter correct recipientAddress");
            result.StatusCode = -3;
            result.Message = "Please enter correct recipientAddress";
            return result;
        }

        try
        {
            // Convert the amount to 'wei' value (assuming 'amount' is in ether)
            decimal etherAmount = decimal.Parse(amount);
            BigInteger amountInWei = UnitConversion.Convert.ToWei(etherAmount);

            // Generate a transaction input to send tokens to the recipient
            var transferFunction = contractMLD.GetFunction("transfer");
            //TransactionInput transferInput = transferFunction.CreateTransactionInput(account.Address, recipientAddress, amountInWei);
            TransactionInput transferInput = transferFunction.CreateTransactionInput(account.Address, recipientAddress, amountInWei);

            // Estimate gas required for the transaction
            var gasEstimation = await transferFunction.EstimateGasAsync(account.Address, null, null, recipientAddress, amountInWei);

            // Add some buffer to the gas estimation
            var adjustedGasLimit = gasEstimation.Value + 100000;

            // Set the gas limit for the transaction
            transferInput.Gas = new HexBigInteger(adjustedGasLimit);

            var transferReceipt = await web3.Eth.TransactionManager.SendTransactionAndWaitForReceiptAsync(transferInput);

            // Check the transaction status
            if (transferReceipt.Status.Value == 1)
            {
                Debug.Log("Tokens sent successfully!");
                result.StatusCode = (int)transferReceipt.Status.Value;
                result.Message = "Tokens sent successfully!";
                //return (int)transferReceipt.Status.Value;
            }
            else
            {
                result.StatusCode = 0;
                result.Message = "Failed to send tokens.";
                //return 0;
            }

            return result;
        }
        catch (FormatException ex)
        {
            Debug.LogError($"FormatException: {ex.Message}");
            result.StatusCode = -2;
            result.Message = ex.Message;
            return result;
            
        }
        catch (Nethereum.JsonRpc.Client.RpcResponseException ex)
        {
            Debug.LogError($"Error sending transaction: {ex.Message}");
            result.StatusCode = 0;
            result.Message = ex.Message;
            return result;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error sending transaction: {ex.Message}");

            //if(ex.Message == "An error occurred encoding abi value. Order: '1', Type: 'address', Value: 'sdaasdasd'.  Ensure the value is valid for the abi type.")
            if (ex.Message.Contains("An error occurred encoding abi value"))
            {

                result.StatusCode = -2;
                result.Message = ex.Message;
                return result;

            }

            result.StatusCode = 0;
            result.Message = ex.Message;
            return result;
        }

    }

    public async Task<TransferResult> SendTokensLORD(string recipientAddress, string amount)
    {

        TransferResult result = new TransferResult();

        if (recipientAddress.Equals(string.Empty) || amount.Equals(string.Empty))
        {
            Debug.Log("Please enter recipientAddress and amount and in correct formate");

            result.StatusCode = -1;
            result.Message = "Please enter recipientAddress and amount in correct format";
            return result;
        }
        if (!IsEthereumAddressValid(recipientAddress))
        {
            Debug.Log("Please enter correct recipientAddress");
            result.StatusCode = -3;
            result.Message = "Please enter correct recipientAddress";
            return result;
        }

        try
        {
            // Convert the amount to 'wei' value (assuming 'amount' is in ether)
            decimal etherAmount = decimal.Parse(amount);
            BigInteger amountInWei = UnitConversion.Convert.ToWei(etherAmount);

            // Generate a transaction input to send tokens to the recipient
            var transferFunction = contractLord.GetFunction("transfer");
            //TransactionInput transferInput = transferFunction.CreateTransactionInput(account.Address, recipientAddress, amountInWei);
            TransactionInput transferInput = transferFunction.CreateTransactionInput(account.Address, recipientAddress, amountInWei);

            // Estimate gas required for the transaction
            var gasEstimation = await transferFunction.EstimateGasAsync(account.Address, null, null, recipientAddress, amountInWei);

            // Add some buffer to the gas estimation
            var adjustedGasLimit = gasEstimation.Value + 100000;

            // Set the gas limit for the transaction
            transferInput.Gas = new HexBigInteger(adjustedGasLimit);

            var transferReceipt = await web3.Eth.TransactionManager.SendTransactionAndWaitForReceiptAsync(transferInput);

            // Check the transaction status
            if (transferReceipt.Status.Value == 1)
            {
                Debug.Log("Tokens sent successfully!");
                result.StatusCode = (int)transferReceipt.Status.Value;
                result.Message = "Tokens sent successfully!";
                //return (int)transferReceipt.Status.Value;
            }
            else
            {
                result.StatusCode = 0;
                result.Message = "Failed to send tokens.";
                //return 0;
            }

            return result;
        }
        catch (FormatException ex)
        {
            Debug.LogError($"FormatException: {ex.Message}");
            result.StatusCode = -2;
            result.Message = ex.Message;
            return result;

        }
        catch (Nethereum.JsonRpc.Client.RpcResponseException ex)
        {
            Debug.LogError($"Error sending transaction: {ex.Message}");
            result.StatusCode = 0;
            result.Message = ex.Message;
            return result;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error sending transaction: {ex.Message}");

            //if(ex.Message == "An error occurred encoding abi value. Order: '1', Type: 'address', Value: 'sdaasdasd'.  Ensure the value is valid for the abi type.")
            if (ex.Message.Contains("An error occurred encoding abi value"))
            {

                result.StatusCode = -2;
                result.Message = ex.Message;
                return result;

            }

            result.StatusCode = 0;
            result.Message = ex.Message;
            return result;
        }

    }

    public async Task<TransferResult> SendTokensMATIC(string recipientAddress, string amount)
    {
        TransferResult result = new TransferResult();

        if (recipientAddress.Equals(string.Empty) || amount.Equals(string.Empty))
        {
            Debug.Log("Please enter recipientAddress and amount and in correct formate");

            result.StatusCode = -1;
            result.Message = "Please enter recipientAddress and amount and in correct formate";
            return result;
        }
        if (!IsEthereumAddressValid(recipientAddress))
        {
            Debug.Log("Please enter correct recipientAddress");

            result.StatusCode = -3;
            result.Message = "Please enter correct recipientAddress";
            return result;
        }
        try
        {

            decimal etherAmount = decimal.Parse(amount);
            BigInteger amountInWei = UnitConversion.Convert.ToWei(etherAmount);
            var transaction = await web3.TransactionManager.SendTransactionAsync(account.Address, recipientAddress, new Nethereum.Hex.HexTypes.HexBigInteger(amountInWei));

            var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transaction);
            Debug.Log($"transaction {transaction} and {amountInWei}");

            if (receipt != null)
            {
                // Check the transaction status
                if (receipt.Status.Value == 1)
                {
                    Debug.Log("receipt Tokens sent successfully!");
                    result.StatusCode = (int)receipt.Status.Value;
                    result.Message = "Tokens sent successfully!";
                }
                else
                {
                    result.StatusCode = 0;
                    result.Message = "Failed to send tokens.";
                }

                return result;
            }
            else
            {
                Debug.Log("Tokens sent successfully!");

                result.StatusCode = 1;
                result.Message = "Tokens sent successfully!";
                return result;
            }


        }
        catch (FormatException ex)
        {
            Debug.LogError($"FormatException: {ex.Message}");
            result.StatusCode = -2;
            result.Message = ex.Message;
            return result;
        }
        catch (Nethereum.JsonRpc.Client.RpcResponseException ex)
        {
            Debug.LogError($"Error sending transaction: {ex.Message}");
            result.StatusCode = 0;
            result.Message = ex.Message;
            return result;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error sending transaction: {ex.Message}");

            //if(ex.Message == "An error occurred encoding abi value. Order: '1', Type: 'address', Value: 'sdaasdasd'.  Ensure the value is valid for the abi type.")
            if (ex.Message.Contains("An error occurred encoding abi value"))
            {

                result.StatusCode = -2;
                result.Message = ex.Message;
                return result;

            }

            result.StatusCode = 0;
            result.Message = ex.Message;
            return result;
        }

        //Debug.Log($"transaction {transaction}");
    }


    #endregion

    #region FetchToken
    public async Task<decimal> GetBalanceAsyncMLD()
    {
        //string addressToCheck = addressInput.text; // Get the Ethereum address from the input field

        // Initialize the Web3 instance with the Infura endpoint
        //var web3 = new Web3($"https://mainnet.infura.io/v3/{infuraProjectId}");

        // Create a new contract instance with the ERC20 token ABI and contract address
        //var contract = new Contract(tokenABI, tokenContractAddress);

        try
        {
            // Fetch the balance of the address for the ERC20 token
            var balanceFunction = contractMLD.GetFunction("balanceOf");
            var balanceInWei = await balanceFunction.CallAsync<BigInteger>(account.Address);

            // Convert the balance from wei to the token's decimal representation
            // Assuming the token has 18 decimals (adjust accordingly for different tokens)
            decimal balanceInToken = Web3.Convert.FromWei(balanceInWei, 18);

            Debug.Log($"Balance of {account.Address}: {balanceInToken} tokens");
            return balanceInToken;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error fetching token balance: {ex.Message}");
            return -1;
        }
    }

    public async Task<decimal> GetBalanceAsyncLORD()
    {
        //string addressToCheck = addressInput.text; // Get the Ethereum address from the input field

        // Initialize the Web3 instance with the Infura endpoint
        //var web3 = new Web3($"https://mainnet.infura.io/v3/{infuraProjectId}");

        // Create a new contract instance with the ERC20 token ABI and contract address
        //var contract = new Contract(tokenABI, tokenContractAddress);

        try
        {
            // Fetch the balance of the address for the ERC20 token
            var balanceFunction = contractLord.GetFunction("balanceOf");
            var balanceInWei = await balanceFunction.CallAsync<BigInteger>(account.Address);

            // Convert the balance from wei to the token's decimal representation
            // Assuming the token has 18 decimals (adjust accordingly for different tokens)
            decimal balanceInToken = Web3.Convert.FromWei(balanceInWei, 18);

            Debug.Log($"Balance of {account.Address}: {balanceInToken} tokens");
            return balanceInToken;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error fetching token balance: {ex.Message}");
            return 0;
        }
    }

    public async Task<decimal> GetBalanceAsyncMatic()
    {
        // Initialize the Web3 instance with the Infura endpoint
        //var web3 = new Web3($"https://mainnet.infura.io/v3/{infuraProjectId}");

        // Fetch the balance of the address
        var balance = await web3.Eth.GetBalance.SendRequestAsync(account.Address);

        // Convert the balance from wei to ether
        decimal balanceInEther = Web3.Convert.FromWei(balance.Value, UnitConversion.EthUnit.Ether);
        Debug.Log($"balanceInEther {balanceInEther}");
        return balanceInEther;
    }

    #endregion

    #region transectionHisotry

    /* public async Task GetTransactionHistory()
     {
         if (web3 == null || account == null)
         {
             Debug.LogError("Web3 or account is not initialized.");
             return;
         }

         var blockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
         var blocksToSearch = 100;
         var transactions = new List<Transaction>();

         Debug.Log($"blocknumber {blockNumber}");

         for (BigInteger i = blockNumber.Value; i > blockNumber.Value - blocksToSearch; i--)
         {
             var block = await web3.Eth.Blocks.GetBlockWithTransactionsHashesByNumber.SendRequestAsync(new BlockParameter((ulong)i));

             Debug.Log($" and i value is {i} block {block.Number} and hash {block.BlockHash}");

             if (block == null || block.TransactionHashes == null)
             {
                 Debug.Log($"block {block.Number}");
                 continue; // Skip if the block or transaction hashes are null
             }

             foreach (var transactionHash in block.TransactionHashes)
             {
                 var transaction = await web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(transactionHash);

                 if (transaction == null)
                 {
                     Debug.Log($"transaction to {transaction.To} and from {transaction.From} ");
                     continue; // Skip if the transaction is null
                 }

                 if (transaction.From == null || transaction.To == null)
                 {
                     Debug.Log($"transaction to or from contain empty  ");
                     continue; // Skip if the transaction is null
                 }
                 else
                 {
                     Debug.Log($"transaction to {transaction.To} and from {transaction.From} ");
                 }

                 if (transaction.From.ToLower() == account.Address.ToLower() || transaction.To.ToLower() == account.Address.ToLower())
                 {
                     Debug.Log($"transaction to {transaction.To} and from {transaction.From} ");

                     transactions.Add(transaction);
                     // Debug.Log($"Transaction: {transaction.To} {transaction.Gas} {transaction.From} {transaction.BlockHash} {transaction.BlockNumber} {transaction.Nonce}");
                 }
             }
         }


         // Process the list of transactions
         foreach (var tx in transactions)
         {
             Debug.Log($"TxHash: {tx.TransactionHash}");
             Debug.Log($"From: {tx.From}");
             Debug.Log($"To: {tx.To}");
             Debug.Log($"Value: {Web3.Convert.FromWei(tx.Value)} Ether");
             Debug.Log($"Block Number: {tx.BlockNumber.Value}");
             Debug.Log($"Timestamp: {await GetTransactionTimestamp((ulong)tx.BlockNumber.Value)}");
             Debug.Log("--------------------------");
         }
     }*/


    public async Task<string> GetTransactionHistory()
    {
        string apiKey = "4UPKM5QK3J9Z8GMB5G4VFAPIX4RGW631J3";
        string address = account.Address;
        string apiUrl = $"https://api-testnet.polygonscan.com/api?module=account&action=txlist&address={address}&apikey={apiKey}";

        Debug.Log($"apiUrl: {apiUrl}");

        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.GetStringAsync(apiUrl);
            var data = JObject.Parse(response);
            Debug.Log($"data: {data}");

            if (data["status"].ToString() == "1")
            {
                var transactions = data["result"];
                foreach (var tx in transactions)
                {
                    Debug.Log($"TxHash: {tx["hash"]}");
                    Debug.Log($"From: {tx["from"]}");
                    Debug.Log($"To: {tx["to"]}");
                    Debug.Log($"Value: {Web3.Convert.FromWei(BigInteger.Parse(tx["value"].ToString()))} Matic");
                    Debug.Log($"Block Number: {tx["blockNumber"]}");
                   // Debug.Log($"Timestamp: {await GetTransactionTimestamp((ulong)tx["blockNumber"])}");
                    Debug.Log("--------------------------");

                }
                return $"{response}";
            }
            else
            {
                Debug.Log($"Failed to fetch transaction history: {data["message"]}");
                return $"{response}";
            }
        }
    }

    /*   public async Task GetTransactionHistory()
       {
           var transferEvent = web3.Eth.GetEvent<TransferEventDTO>(contractAddressMLD);
           var filterInput = transferEvent.CreateFilterInput(new BlockParameter(0), new BlockParameter(100));
         //  var events = await transferEvent.GetAllChanges(filterInput);
           var events = await transferEvent.GetAllChangesAsync(filterInput);

           var tokenTransactions = new List<TransferEventDTO>();

           foreach (var ev in events)
           {
               if (ev.Event.From.Equals(account.Address, StringComparison.OrdinalIgnoreCase) ||
                   ev.Event.To.Equals(account.Address, StringComparison.OrdinalIgnoreCase))
               {
                   tokenTransactions.Add(ev.Event);
               }
           }

           // Process the token transactions
           foreach (var tx in tokenTransactions)
           {
               //Console.WriteLine($"Transaction Hash: {tx.Log.TransactionHash}");
               Console.WriteLine($"From: {tx.From}");
               Console.WriteLine($"To: {tx.To}");
               Console.WriteLine($"Amount: {Web3.Convert.FromWei(tx.Value, 18)} Tokens"); // Assuming 18 decimals
               Console.WriteLine("--------------------------");
           }
       }*/


    public async Task<string> GetTransactionTimestamp(ulong blockNumber)
    {
        var block = await web3.Eth.Blocks.GetBlockWithTransactionsHashesByNumber.SendRequestAsync(new BlockParameter(blockNumber));
        return block.Timestamp.Value.ToString();
    }

    #endregion

    #region nft

    public async Task BuyNFT(int tokenId)
    {
        // Assuming the NFT contract has a "buyAtFixedPrice" function that takes the tokenId as a parameter
        var buyNFTFunction = contractMarkatePlace.GetFunction("buyAtFixedPrice");

        // You may need to set additional transaction parameters such as gas and gas price
        var transactionReceipt = await buyNFTFunction.SendTransactionAndWaitForReceiptAsync(account.Address, new Nethereum.Hex.HexTypes.HexBigInteger(1000000), new Nethereum.Hex.HexTypes.HexBigInteger(0), null, tokenId);

        if (transactionReceipt.Status.Value == 1)
        {
            Debug.Log("NFT bought successfully!");
            // Perform any additional actions after buying the NFT
        }
        else
        {
            Debug.Log($"Failed to buy NFT. {transactionReceipt.BlockNumber}");
            Debug.Log($"Failed to buy NFT. {transactionReceipt.Status}");
        }
    }

   

    public async Task SellNFT(int tokenId, string priceInEther)
    {
        // Assuming the NFT contract has a "sellNFT" function that takes the tokenId and price as parameters
        var sellNFTFunction = contractMarkatePlace.GetFunction("sellNFT");

        // Convert the price from ether to wei (1 ether = 10^18 wei)
        var priceInWei = Web3.Convert.ToWei(priceInEther);

        // You may need to set additional transaction parameters such as gas and gas price
        var transactionReceipt = await sellNFTFunction.SendTransactionAndWaitForReceiptAsync(account.Address, new Nethereum.Hex.HexTypes.HexBigInteger(90000), new Nethereum.Hex.HexTypes.HexBigInteger(1000000), null, tokenId, priceInWei);

        if (transactionReceipt.Status.Value == 1)
        {
            Debug.Log("NFT listed for sale successfully!");
            // Perform any additional actions after listing the NFT for sale
        }
        else
        {
            Debug.Log("Failed to list NFT for sale.");
        }
    }

    public async Task MintNFT(int tokenId, string metadataUrl)
    {
        // Assuming the NFT contract has a "mint" function that takes the tokenId and metadataUrl as parameters
        var mintFunction = contractMarkatePlace.GetFunction("mint");

        // You may need to set additional transaction parameters such as gas and gas price
        var transactionReceipt = await mintFunction.SendTransactionAndWaitForReceiptAsync(account.Address, new Nethereum.Hex.HexTypes.HexBigInteger(90000), new Nethereum.Hex.HexTypes.HexBigInteger(1000000), null, tokenId, metadataUrl);

        if (transactionReceipt.Status.Value == 1)
        {
            Debug.Log("NFT minted successfully!");
            // Perform any additional actions after minting the NFT
        }
        else
        {
            Debug.Log("Failed to mint NFT.");
        }
    }

    public async Task<List<int>> GetListedListedNFTs()
    {
        // Assuming the marketplace contract has a function to get the list of listed NFTs
        var getListedNFTsFunction = contractMarkatePlace.GetFunction("owner");
        Debug.Log($"getListedNFTsFunction {getListedNFTsFunction.GetData()}");
        // Call the function to get the list of listed NFT IDs
        var listedNFTs = await getListedNFTsFunction.CallAsync<List<int>>();

        foreach (var item in listedNFTs)
        {
            Debug.Log($"listedNFTs {item}");
        }
       
        return listedNFTs;
    }

    public async Task<NFTMetadata> GetNFTMetadata(int nftId)
    {
        // Assuming you have a separate NFT contract with metadata and its ABI
        string nftContractAddress = "NFT_CONTRACT_ADDRESS"; // Replace with the contract address of the NFT
        string nftContractABI = "NFT_CONTRACT_ABI_JSON"; // Replace with the contract ABI in JSON format

        // Load the NFT contract
        var nftContract = new Contract(null, nftContractABI, nftContractAddress);

        // Assuming the NFT contract has a function to get the metadata of an NFT using its ID
        var getMetadataFunction = nftContract.GetFunction("getMetadata");

        // Call the function to get the metadata for the NFT with the given ID
        var nftMetadata = await getMetadataFunction.CallDeserializingToObjectAsync<NFTMetadata>(nftId);

        return nftMetadata;
    }

    public async Task GetNFTInformation(string contractAddress, string abi , int tokenId)
    {

        // Replace this with the actual contract address of the NFT contract
        contractAddress = "YOUR_NFT_CONTRACT_ADDRESS";

        // Replace this with the token ID you want to query information for
        tokenId = 123;

        // Load the contract's ABI (you can get this from the contract deployment)
        abi = "YOUR_NFT_CONTRACT_ABI_JSON";

        var nftContract = web3.Eth.GetContract(abi, contractAddress);

        // Call the contract's function to get the owner of the specified token ID
        var getOwnerFunction = nftContract.GetFunction("ownerOf");
        var owner = await getOwnerFunction.CallAsync<string>(tokenId);

        // Call the contract's function to get metadata (name, description, etc.) of the specified token ID
        var getMetadataFunction = nftContract.GetFunction("tokenURI");
        var metadataUri = await getMetadataFunction.CallAsync<string>(tokenId);

        // Now, you can use the 'metadataUri' to fetch the NFT's metadata (e.g., name, description, image URI) from a hosted JSON file or an IPFS gateway.

        Console.WriteLine($"Owner of NFT with token ID {tokenId}: {owner}");
        Console.WriteLine($"Metadata URI for NFT with token ID {tokenId}: {metadataUri}");
    }

    public async Task GetMintedNFTs(string contractAddress, string abi)
    {
        // Replace this with the actual contract address of the NFT contract
        contractAddress = "";

        // Load the contract's ABI (you can get this from the contract deployment)
         abi = "YOUR_NFT_CONTRACT_ABI_JSON";

        var nftContract = web3.Eth.GetContract(abi, contractAddress);

        // Assuming the NFT contract emits a "Transfer" event when new NFTs are minted
        // You can replace "Transfer" with the actual event name emitted by your contract
        var transferEvent = nftContract.GetEvent("Transfer");

        // Filter the events to only get the minted NFTs
        var filterInput = transferEvent.CreateFilterInput(new BlockParameter(0), new BlockParameter(1000));

        // Get the events matching the filter
        var events = await transferEvent.GetAllChangesDefaultAsync(filterInput);

        // Process the events to retrieve minted NFT details
        foreach (var ev in events)
        {
            // The 'ev.Event' object will contain details about the minted NFT, such as token ID, sender, and recipient.
           
            var tokenId = ev.Event;
            var sender = ev.Event;
            var recipient = ev.Event;

            Console.WriteLine($"Minted NFT Token ID: {tokenId}");
            Console.WriteLine($"Sender: {sender}");
            Console.WriteLine($"Recipient: {recipient}");
            Console.WriteLine("--------------------------");
        }
    }

    #endregion


    #region otherMethods for information

    public void getPrivateKey()
    {
        if (account == null)
        {
            Debug.Log("Please Login First");
           
            return;
        }
        Debug.Log(account.PrivateKey);
      
    }

    public void getAccount()
    {
        if (account == null)
        {
            Debug.Log("Please Login First");
            
            return;
        }
        Debug.Log(account.Address);
       
    }

    public async void getChainId()
    {
        if (account == null)
        {
            Debug.Log("Please Login First");
            
            return;
        }
        var chainId = await web3.Net.Version.SendRequestAsync();

        Debug.Log(chainId);
    }

    #endregion
}

// The TransferEventDTO class should match the 'Transfer' event definition in your token contract.
// It's used for decoding the event data into a more accessible format.
[Event("Transfer")]
public class TransferEventDTO : IEventDTO
{
    [Parameter("address", "from", 1, true)]
    public string From { get; set; }

    [Parameter("address", "to", 2, true)]
    public string To { get; set; }

    [Parameter("uint256", "value", 3, false)]
    public BigInteger Value { get; set; }
}


// Class to represent NFT metadata
[System.Serializable]
public class NFTMetadata
{
    public string Name;
    public string ImageURL;
    public string Description;
}

public class TransferResult
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
}