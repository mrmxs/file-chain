pragma solidity ^0.4.24;

contract UserStorage {

    struct FileUser {
        address wallet;
        string firstName; // set
        string lastName;  // set
        string info;      // set
        bool isValue;
    }

    uint usersCount;
    mapping(uint => FileUser) users;

    function addUser(
        address _wallet,
        string _firstName,
        string _lastName,
        string _info
    ) public returns (uint index){
        // todo add payable
        // todo some checks
        users[usersCount + 1] = FileUser(
            _wallet, _firstName, _lastName, _info, true
        );

        return ++usersCount;
    }

    // todo modifier owner+editors+viewers
    function getUser(uint _index) public view returns (
        address wallet,
        string firstName,
        string lastName,
        string info
    ) {
        if (!users[_index].isValue) revert("NOT EXISTING INDEX");

        wallet = users[_index].wallet;
        firstName = users[_index].firstName;
        lastName = users[_index].lastName;
        info = users[_index].info;
    }

    function setName(uint _index, string _firstName, string _lastName) public {
        // todo add payable
        // todo some checks
        if (!users[_index].isValue) revert("NOT EXISTING INDEX");
        if (msg.sender != users[_index].wallet) revert("UNSOFFICIENT PRIVILEGE");

        users[_index].firstName = _firstName;
        users[_index].lastName = _lastName;
    }

    function setInfo(uint _index, string _firstName, string _lastName) public {
        // todo add payable
        // todo some checks
        if (!users[_index].isValue) revert("NOT EXISTING INDEX");
        if (msg.sender != users[_index].wallet) revert("UNSOFFICIENT PRIVILEGE");

        users[_index].firstName = _firstName;
        users[_index].lastName = _lastName;
    }
}