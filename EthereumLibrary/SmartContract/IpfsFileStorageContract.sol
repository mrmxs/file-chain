pragma solidity ^0.4.24;

contract IpfsFileStorage2 {

    struct IpfsFile {
        bytes32 mimeType;
        bytes32[2] ipfsHash;
        bytes32 size;
        bytes32[3] name;        // set
        bytes32[6] description; // set
        uint32 created;
        uint32 modified;        // internal set
        bool isValue;
    }

    mapping(uint => IpfsFile) files;
    uint maxindex;

    modifier existed (uint _index) {
        if (!contains(_index)) revert("NOT EXISTING INDEX");
        _;
    }

    function contains(uint _index)
    public
    view
    returns (bool result) {
        return files[_index].isValue;
    }

    function add (
        bytes32 _mimeType,
        bytes32[2] _ipfsHash,
        bytes32 _size,
        bytes32[3] _name,
        bytes32[6] _description,
        uint32 timestamp
    )
    public
    returns (uint) {
        // todo add payable
        // todo some checks
        maxindex++;
        files[maxindex] = IpfsFile(
            _mimeType,
            _ipfsHash,
            _size,
            _name,
            _description,
            timestamp,
            timestamp,
            true
        );

        return maxindex;
    }

    function get(uint _index)
    public
    view
    existed (_index)
    returns (
        bytes32 mimeType,
        bytes32[2] ipfsHash,
        bytes32 size,
        bytes32[3] name,
        bytes32[6] description,
        uint32 created,
        uint32 modified
    ) {
        mimeType = files[_index].mimeType;
        size = files[_index].size;
        ipfsHash = files[_index].ipfsHash;
        name = files[_index].name;
        description = files[_index].description;
        created = files[_index].created;
        modified = files[_index].modified;
    }

    function setName(uint _index, bytes32[3] _value, uint32 timestamp)
    public
    existed (_index) {
        // todo add payable
        files[_index].name = _value;
        files[_index].modified = timestamp;
    }

    function setDescription(uint _index, bytes32[6] _value, uint32 timestamp)
    public
    existed (_index) {
        // todo add payable
        files[_index].description = _value;
        files[_index].modified = timestamp;
    }
}