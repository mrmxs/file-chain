pragma solidity ^0.4.24;

contract WalletStorage {

    address wallet;

    constructor(address _wallet) public {
        wallet = _wallet;
    }

    struct WalletUser {
        string login; //todo bytes with fixed length
        bytes32 passwordHash;
        string firstName; // set
        string lastName;  // set
        string info;      // set
        bool isValue;
    }

    uint usersCount;
    mapping(string => WalletUser) users; // login => User

    function addUser(
        string _login,
        string _password,
        string _firstName,
        string _lastName,
        string _info
    ) public returns (uint index){
        // todo add payable
        // todo some checks
        if (msg.sender != wallet) revert("UNSOFFICIENT PRIVILEGE");
        if (users[_login].isValue) revert("LOGIN ALREADY EXISTS");

        users[_login] = WalletUser(
            _login, keccak256(abi.encodePacked(_password)),
            _firstName, _lastName, _info,
            true
        );

        return ++usersCount;
    }

    function getUser(string _login) public view returns (
        string login,
        string firstName,
        string lastName,
        string info
    ) {
        if (!users[_login].isValue) revert("LOGIN DOESN'T EXIST");

        login = _login;
        firstName = users[_login].firstName;
        lastName = users[_login].lastName;
        info = users[_login].info;
    }

    // todo only user its own can change info
    function setName(string _login, string _password, string _firstName, string _lastName) public {
        // todo add payable
        // todo some checks
        if (msg.sender != wallet) revert("UNSOFFICIENT PRIVILEGE");
        if (!users[_login].isValue) revert("NOT EXISTING INDEX");
        if (keccak256(abi.encodePacked(_password)) != users[_login].passwordHash)
            revert("WRONG CREDENTIALS");

        users[_login].firstName = _firstName;
        users[_login].lastName = _lastName;
    }

    // todo only user its own can change info
    function setInfo(string _login, string _password, string _info) public {
        // todo add payable
        // todo some checks
        if (msg.sender != wallet) revert("UNSOFFICIENT PRIVILEGE");
        if (!users[_login].isValue) revert("NOT EXISTING INDEX");
        if (keccak256(abi.encodePacked(_password)) != users[_login].passwordHash)
            revert("WRONG CREDENTIALS");

        users[_login].info = _info;
    }

    // todo only user its own can change password
    // todo and admin
    function setPassword(string _login, string _password, string _newPassword) public {
        // todo add payable
        // todo some checks
        if (msg.sender != wallet) revert("UNSOFFICIENT PRIVILEGE");
        if (!users[_login].isValue) revert("NOT EXISTING INDEX");
        if (keccak256(abi.encodePacked(_password)) != users[_login].passwordHash)
            revert("WRONG CREDENTIALS");

        users[_login].passwordHash = keccak256(abi.encodePacked(_newPassword));
    }
}