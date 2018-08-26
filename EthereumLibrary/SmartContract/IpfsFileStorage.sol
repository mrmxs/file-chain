pragma solidity ^0.4.24;

contract IpfsFileStorage {

    struct IpfsFile {
        string mimeType;
        uint size;
        string ipfsHash;
        string name;        // set
        string description; // set
        bool isActive;      // set
        uint created;
        uint accessed;      // internal set
        uint modified;      // internal set
        bool isValue;
    }

    uint filesCount;
    mapping(uint => IpfsFile) files;

    function addIpfsFileToStorage(
        string _mimeType,
        uint _size,
        string _ipfsHash,
        string _name,
        string _description,
        bool _isActive,
        uint _created,
        uint _accessed,
        uint _modified
    ) public returns (uint index){
        // todo add payable
        // todo some checks
        files[++filesCount] = IpfsFile(
            _mimeType, _size, _ipfsHash, _name, _description, _isActive,
            _created, _accessed, _modified, true
        );

        return filesCount;
    }

    // todo modifier owner+editors+viewers
    function getIpfsFile(uint _index) public returns (
        string mimeType,
        uint size,
        string ipfsHash,
        string name,
        string description,
        bool isActive,
        uint created,
        uint accessed,
        uint modified
    ) {
        if (!files[_index].isValue) revert("NOT EXISTING INDEX");
        
        files[_index].accessed = now;
        //todo should it be payable?

        mimeType = files[_index].mimeType;
        size = files[_index].size;
        ipfsHash = files[_index].ipfsHash;
        name = files[_index].name;
        description = files[_index].description;
        isActive = files[_index].isActive;
        created = files[_index].created;
        accessed = files[_index].accessed;
        modified = files[_index].modified;
    }

    // todo modifier owner+editors
    function setName(uint _index, string _value) public {
        // todo add payable
        // todo some checks
        if (!files[_index].isValue) revert("NOT EXISTING INDEX");

        files[_index].name = _value;
        files[_index].modified = now;
    }

    // todo modifier owner+editors
    function setDescription(uint _index, string _value) public {
        // todo add payable
        // todo some checks
        if (!files[_index].isValue) revert("NOT EXISTING INDEX");

        files[_index].description = _value;
        files[_index].modified = now;
    }
}