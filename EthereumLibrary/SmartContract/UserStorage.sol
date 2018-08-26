pragma solidity ^0.4.24;

library UserStorage {

    struct FileUser {
        address wallet;
        string firstName; // set
        string lastName;  // set
        string info;      // set
        bool isValue;
    }

    struct Data {
        uint usersCount;
        mapping(uint => FileUser) users;
    }

    modifier existed (Data storage _self, uint _index) {
        if (!contains(_self, _index)) revert("NOT EXISTING INDEX");
        _;
    }

    modifier notexisted (Data storage _self, address _wallet) {
        if (containsWallet(_self, _wallet)) revert("WALLET ALREADY EXISTS");
        _;
    }

    modifier owned (Data storage _self, uint _index) {
        if (msg.sender != _self.users[_index].wallet) revert("UNSOFFICIENT PRIVILEGE");
        _;
    }

    function addUser(
        Data storage _self,
        address _wallet,
        string _firstName,
        string _lastName,
        string _info
    )
    public
    notexisted (_self, _wallet)
    returns (uint index){
        // todo add payable
        // todo some checks
        _self.users[_self.usersCount + 1] = FileUser(
            _wallet, _firstName, _lastName, _info, true
        );

        return ++_self.usersCount;
    }

    // todo modifier owner+editors+viewers
    function getUser(Data storage _self, uint _index)
    public
    view
    existed (_self, _index)
    returns (
        address wallet,
        string firstName,
        string lastName,
        string info
    ) {
        wallet = _self.users[_index].wallet;
        firstName = _self.users[_index].firstName;
        lastName = _self.users[_index].lastName;
        info = _self.users[_index].info;
    }

    function setName(Data storage _self, uint _index, string _firstName, string _lastName)
    public
    existed (_self, _index)
    owned (_self, _index) {
        // todo add payable
        // todo some checks
        _self.users[_index].firstName = _firstName;
        _self.users[_index].lastName = _lastName;
    }

    function setInfo(Data storage _self, uint _index, string _firstName, string _lastName)
    public 
    existed (_self, _index)
    owned (_self, _index) {
        // todo add payable
        // todo some checks
        _self.users[_index].firstName = _firstName;
        _self.users[_index].lastName = _lastName;
    }

    function contains(Data storage _self, uint _index)
    public
    view
    returns (bool result) {
        return _self.users[_index].isValue;
    }

    function containsWallet(Data storage _self, address _wallet)
    public
    view
    returns (bool result) {
        result = false;

        for (uint i=0; i < _self.usersCount; i++) {
            if (contains(_self, i)){
                if (_self.users[i].wallet == _wallet) {
                    result = true;
                    break;
                }
            }
        }
    }
}