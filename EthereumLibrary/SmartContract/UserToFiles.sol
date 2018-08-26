pragma solidity ^0.4.24;

contract UserToFilesStorage {
    address userStorage;
    address filstorage;


   uint[] admins;

   mapping (uint => uint[]) fileOwners; //user to files
   mapping (uint => uint[]) fileEditors; //user to files
   mapping (uint => uint[]) fileViewers; //user to files












}