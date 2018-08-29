pragma solidity ^0.4.24;

library WalletStorage {

    struct WalletUser {
        string login; //todo bytes with fixed length
        bytes32 passwordHash;
        string firstName; // set
        string lastName;  // set
        string info;      // set
        bool isValue;
    }

    struct Data {
        address wallet;
        uint usersCount;
        mapping(string => WalletUser) users; // login => User
    }

    function setWallet(Data storage self, address _wallet)
    public {
        // todo add payable
        // todo some checks
        self.wallet = _wallet;
    }

    function addUser(
        Data storage self,
        string _login,
        string _password,
        string _firstName,
        string _lastName,
        string _info
    )
    public
    returns (uint index) {
        // todo add payable
        // todo some checks
        if (msg.sender != self.wallet) revert("INSUFFICIENT PRIVILEGES");
        if (self.users[_login].isValue) revert("LOGIN ALREADY EXISTS");

        self.users[_login] = WalletUser(
            _login, keccak256(abi.encodePacked(_password)),
            _firstName, _lastName, _info,
            true
        );

        return ++self.usersCount;
    }

    function getUser(Data storage self, string _login)
    public
    view
    returns (
        string login,
        string firstName,
        string lastName,
        string info
    ) {
        if (!self.users[_login].isValue) revert("LOGIN DOESN'T EXIST");

        login = _login;
        firstName = self.users[_login].firstName;
        lastName = self.users[_login].lastName;
        info = self.users[_login].info;
    }

    // todo only user its own can change info
    function setName(Data storage self,
        string _login, string _password, string _firstName, string _lastName)
    public {
        // todo add payable
        // todo some checks
        if (msg.sender != self.wallet) revert("INSUFFICIENT PRIVILEGES");
        if (!self.users[_login].isValue) revert("NOT EXISTING INDEX");
        if (keccak256(abi.encodePacked(_password)) != self.users[_login].passwordHash)
            revert("WRONG CREDENTIALS");

        self.users[_login].firstName = _firstName;
        self.users[_login].lastName = _lastName;
    }

    // todo only user its own can change info
    function setInfo(Data storage self,
        string _login, string _password, string _info)
    public {
        // todo add payable
        // todo some checks
        if (msg.sender != self.wallet) revert("INSUFFICIENT PRIVILEGES");
        if (!self.users[_login].isValue) revert("NOT EXISTING INDEX");
        if (keccak256(abi.encodePacked(_password)) != self.users[_login].passwordHash)
            revert("WRONG CREDENTIALS");

        self.users[_login].info = _info;
    }

    // todo only user its own can change password
    // todo and admin
    function setPassword(Data storage self,
        string _login, string _password, string _newPassword)
    public {
        // todo add payable
        // todo some checks
        if (msg.sender != self.wallet) revert("INSUFFICIENT PRIVILEGES");
        if (!self.users[_login].isValue) revert("NOT EXISTING INDEX");
        if (keccak256(abi.encodePacked(_password)) != self.users[_login].passwordHash)
            revert("WRONG CREDENTIALS");

        self.users[_login].passwordHash = keccak256(abi.encodePacked(_newPassword));
    }
}