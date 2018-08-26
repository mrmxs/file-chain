pragma solidity ^0.4.24;

import {IpfsFileStorage} from "./IpfsFileStorageLibrary.sol";
import {UserStorage} from "./UserStorageLibrary.sol";

contract UserToFilesStorage { 
    using IpfsFileStorage for IpfsFileStorage.Data; 
    using UserStorage for UserStorage.Data;
    
    UserStorage.Data users;
    IpfsFileStorage.Data files;

    uint[] admins;

    mapping (uint => uint[]) fileOwners;
    mapping (uint => uint[]) fileEditors; 
    mapping (uint => uint[]) fileViewers;

    /* modifier onlyOwner {
        require(
            msg.sender == owner,
            "Only owner can call this function."
        );
        _;
    }*/

    // Is owner

    // Is admin

    // Is editor

    // Is viewer

    // AddUser

    // Add file 
    // add to storage with IPFS hash, get return uint index
    // add to owners list


    // Edit file
    // by admin or owner or editor can

    // Delete file

    // Add editor
    // by owner

    // Add viewer
    // by owner

    // Delete editor
    // by owner or admin

    // Delete viewer
    // by owner or admin












}