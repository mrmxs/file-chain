pragma solidity ^0.4.24;

contract UserStorage {

    struct User {
        bytes32 login;
        bytes32 password;     // set
        bytes32[2] firstName; // set
        bytes32[2] lastName;  // set
        bytes32[6] info;      // set
        bool isAdmin;         // set
        bool isValue;
    }

    address wallet;
    uint maxindex;
    mapping(bytes32 => User) users; // login => User

    modifier walletowner {
        if (msg.sender != wallet) revert("UNSOFFICIENT PRIVILEGE");
        _;
    }

    modifier existed (bytes32 _login) {
        if (!contains(_login)) revert("LOGIN DOESN'T EXIST");
        _;
    }

    modifier notexisted (bytes32 _login) {
        if (contains(_login)) revert("LOGIN ALREADY EXISTS");
        _;
    }

    modifier authorized (bytes32 _login, bytes32 _password) {
        if (_password != users[_login].password) revert("WRONG CREDENTIALS");
        _;
    }

    modifier asadmin (bytes32 _login) {
        if (!users[_login].isAdmin) revert("UNSOFFICIENT PRIVILEGE");
        _;
    }
   
    constructor(
        bytes32 _adminlogin, 
        bytes32 _adminpassword,
        bytes32[2] _firstName,
        bytes32[2] _lastName,
        bytes32[6] _info
        ) 
    public {
        wallet = msg.sender;
        
        users[_adminlogin] = User(
            _adminlogin, 
            _adminpassword,
            _firstName,
            _lastName,
            _info,
            true,
            true
        );
    }

    function contains(bytes32 _login)
    public
    view
    returns (bool) {
        return users[_login].isValue;
    }
        
    function add(
        bytes32 _login,
        bytes32 _password,
        bytes32[2] _firstName,
        bytes32[2] _lastName,
        bytes32[6] _info
    )
    public
    walletowner
    notexisted (_login)
    returns (bool) {
        users[_login] = User(
            _login, 
            _password,
            _firstName,
            _lastName,
            _info,
            false,
            true
        );

        return true;
    }

    function get(bytes32 _login)
    public
    view
    walletowner
    existed (_login)
    returns (
        bytes32 login,
        bytes32[2] firstName,
        bytes32[2] lastName,
        bytes32[6] info,
        bool isAdmin
    ) {
        login = _login;
        firstName = users[_login].firstName;
        lastName = users[_login].lastName;
        info = users[_login].info;
        isAdmin = users[_login].isAdmin;
    }

    function setName(
        bytes32 _login,
        bytes32 _password,
        bytes32[2] _firstName,
        bytes32[2] _lastName)
    public
    walletowner
    existed (_login)
    authorized (_login, _password)
    returns (bool) {
        users[_login].firstName = _firstName;
        users[_login].lastName = _lastName;

        return true;
    }

    function setInfo(
        bytes32 _login,
        bytes32 _password,
        bytes32[6] _info)
    public
    walletowner
    existed (_login)
    authorized (_login, _password)
    returns (bool) {
        users[_login].info = _info;

        return true;
    }

    function setPassword(
        bytes32 _login,
        bytes32 _password,
        bytes32 _newPassword)
    public
    walletowner
    existed (_login)
    authorized (_login, _password)
    returns (bool) {
        users[_login].password =_newPassword;

        return true;
    }

    function setPasswordAsAdmin(
        bytes32 _adminlogin,
        bytes32 _adminpassword,
        bytes32 _login,
        bytes32 _newPassword)
    public
    walletowner
    existed (_login)
    asadmin (_adminlogin)
    authorized (_adminlogin, _adminpassword)
    returns (bool) {
        users[_login].password =_newPassword;

        return true;
    }

    function setAdmin(
        bytes32 _adminlogin,
        bytes32 _adminpassword,
        bytes32 _login,
        bool _isAdmin)
    public
    walletowner
    existed (_login)
    asadmin (_adminlogin)
    authorized (_adminlogin, _adminpassword) 
    returns (bool) {
        users[_login].isAdmin = _isAdmin;

        return true;
    }
}