pragma solidity ^0.4.0;

contract Test {
    int _multiplier;

    constructor (int multiplier) public{
        _multiplier = multiplier;
    }

    function multiply(int val) public view returns (int d) {
        return val * _multiplier;
    }
}
