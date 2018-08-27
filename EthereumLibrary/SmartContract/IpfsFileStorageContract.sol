pragma solidity ^0.4.24;

contract IpfsFileStorage {

    struct IpfsFile {
        string mimeType;
        uint size;
        string ipfsHash;
        string name;        // set
        string description; // set
        address owner;
        uint created;
        uint accessed;      // internal set
        uint modified;      // internal set
        bool isValue;
    }

    uint filesCount;
    mapping(uint => IpfsFile) files;

    modifier existed (uint _index) {
        if (!contains(_index)) revert("NOT EXISTING INDEX");
        _;
    }

    modifier onlyOwner (uint _index) {
        if (msg.sender != files[_index].owner) revert("UNSOFFICIENT PREVILEGE");
        _;
    }

    function addIpfsFileToStorage(
        string _mimeType,
        uint _size,
        string _ipfsHash,
        string _name,
        string _description
    )
    public
    returns (uint index){
        // todo add payable
        // todo some checks
        uint created = now;
        files[++filesCount] = IpfsFile(
            _mimeType, _size, _ipfsHash, _name, _description,
            msg.sender, created, created, created, true
        );

        return filesCount;
    }

    // todo modifier owner+editors+viewers
    function getIpfsFile(uint _index)
    public
    existed (_index)
    returns (
        string mimeType,
        uint size,
        string ipfsHash,
        string name,
        string description,
        address owner,
        uint created,
        uint accessed,
        uint modified
    ) {
        files[_index].accessed = now;
        //todo should it be payable?

        mimeType = files[_index].mimeType;
        size = files[_index].size;
        ipfsHash = files[_index].ipfsHash;
        name = files[_index].name;
        description = files[_index].description;
        owner = files[_index].owner;
        created = files[_index].created;
        accessed = files[_index].accessed;
        modified = files[_index].modified;
    }

    // todo modifier owner+editors
    function setName(uint _index, string _value)
    public 
    existed (_index) 
    onlyOwner (_index) {
        // todo add payable
        // todo some checks
        files[_index].name = _value;
        files[_index].modified = now;
    }

    // todo modifier owner+editors
    function setDescription(uint _index, string _value)
    public
    existed (_index) 
    onlyOwner (_index) {
        // todo add payable
        // todo some checks
        files[_index].description = _value;
        files[_index].modified = now;
    }

    function contains(uint _index)
    public
    view
    returns (bool result) {
        return files[_index].isValue;
    }
}