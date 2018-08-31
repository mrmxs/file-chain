pragma solidity ^0.4.24;

import {IpfsFileStorageLibrary} from "./IpfsFileStorageLibrary.sol";

contract UsersAndFiles {

    struct User {
        bytes32 login;
        bytes32 password;     // set
        bytes32[2] firstName; // set
        bytes32[2] lastName;  // set
        bytes32[6] info;      // set
        bool isAdmin;         // set
        bool isValue;
    }

    // ============== User Storage Region ======================== //

    address wallet;
    uint maxindex;
    mapping (bytes32 => User) users; // login => User

    modifier walletowner {
        if (msg.sender != wallet) revert("INSUFFICIENT PRIVILEGES");
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
        if (!users[_login].isAdmin) revert("INSUFFICIENT PRIVILEGES");
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


    // ============== User Files Region ======================== //

    modifier fileowner (bytes32 _login, uint _fileindex) {
        uint i = findFileIndex(_login, _fileindex);

        if (i == 0 && (userFiles[_login][i] != _fileindex))
            revert("INSUFFICIENT PRIVILEGES");
        _;
    }

    IpfsFileStorageLibrary.Data data;
    mapping (bytes32 => uint[]) userFiles; // login -> fileindex

    // add file
    function addFile (
        bytes32 _login,
        bytes32 _password,
        bytes32 _mimeType,
        bytes32[6] _ipfsHash,
        bytes32 _size,
        bytes32[3] _name,
        bytes32[6] _description,
        uint32 timestamp
    )
    public
    walletowner
    existed (_login)
    authorized (_login, _password)
    returns (uint fileindex, uint[] arr) {
        // todo add payable

        fileindex = IpfsFileStorageLibrary.add(
            data,
            _mimeType,
            _ipfsHash,
            _size,
            _name,
            _description,
            timestamp);

        userFiles[_login].push(fileindex);

        arr = userFiles[_login];
    }

    // delete file
    function deleteFile (
        bytes32 _login,
        bytes32 _password,
        uint _fileindex
    )
    public
    walletowner
    existed (_login)
    authorized (_login, _password)
    fileowner (_login, _fileindex)
    returns (uint[]) {
        uint len = userFiles[_login].length;
        uint i = findFileIndex(_login, _fileindex);

        if (i > 0 || userFiles[_login][i] == _fileindex) {
            userFiles[_login][i] = userFiles[_login][len - 1];
            userFiles[_login].length--;
        }

        return userFiles[_login];
    }

    // get file ids
    function getFileIds(
        bytes32 _login,
        bytes32 _password)
    public
    view
    walletowner
    existed (_login)
    authorized (_login, _password)
    returns (uint[]) {
        return userFiles[_login];
    }

    // get file
    function getFilePart1(
        bytes32 _login,
        bytes32 _password,
        uint _fileindex)
    public
    view
    walletowner
    existed (_login)
    authorized (_login, _password)
    fileowner (_login, _fileindex)
    returns (
        bytes32, bytes32[6], bytes32,
        bytes32[3]
    )
    {
        return IpfsFileStorageLibrary.get1(data, _fileindex);
    }

    // get file
    function getFilePart2(
        bytes32 _login,
        bytes32 _password,
        uint _fileindex)
    public
    view
    walletowner
    existed (_login)
    authorized (_login, _password)
    fileowner (_login, _fileindex)
    returns (
        bytes32[6], uint32, uint32
    )
    {
        return IpfsFileStorageLibrary.get2(data, _fileindex);
    }

    // edit file name
    function setFileName(
        bytes32 _login,
        bytes32 _password,
        uint _fileindex,
        bytes32[3] _value,
        uint32 _timestamp)
    public
    walletowner
    existed (_login)
    authorized (_login, _password)
    fileowner (_login, _fileindex)
    returns (bytes32[3])
    {
        IpfsFileStorageLibrary.setName(
            data, _fileindex, _value, _timestamp);

        return data.files[_fileindex].name;
    }

    // edit file description
    function setFileDescription(
        bytes32 _login,
        bytes32 _password,
        uint _fileindex,
        bytes32[6] _value,
        uint32 _timestamp)
    public
    walletowner
    existed (_login)
    authorized (_login, _password)
    fileowner (_login, _fileindex)
    returns (bytes32[6])
    {
        IpfsFileStorageLibrary.setDescription(
            data, _fileindex, _value, _timestamp);

        return data.files[_fileindex].description;
    }


    function findFileIndex(bytes32 _login, uint _fileindex)
    private
    view
    returns (uint) {
        uint len = userFiles[_login].length;

        for (uint i = 0; i < len; i++) {
            if (userFiles[_login][i] == _fileindex) {
                return i;
            }
        }
        i = 0;
    }
}