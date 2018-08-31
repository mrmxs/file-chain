pragma solidity ^0.4.24;

library IpfsFileStorageLibrary {

    struct IpfsFile {
        bytes32 mimeType;
        bytes32[6] ipfsHash;
        bytes32 size;
        bytes32[3] name;        // set
        bytes32[6] description; // set
        uint32 created;
        uint32 modified;        // internal set
        bool isValue;
    }

    struct Data{
        mapping(uint => IpfsFile) files;
        uint maxindex;
    }

    modifier existed (Data storage _self, uint _index) {
        if (!contains(_self, _index)) revert("NOT EXISTING INDEX");
        _;
    }

    function contains (Data storage _self, uint _index)
    public
    view
    returns (bool result) {
        return _self.files[_index].isValue;
    }

    function add (
        Data storage _self,
        bytes32 _mimeType,
        bytes32[6] _ipfsHash,
        bytes32 _size,
        bytes32[3] _name,
        bytes32[6] _description,
        uint32 timestamp
    )
    public
    returns (uint) {
        _self.maxindex++;
        _self.files[_self.maxindex] = IpfsFile(
            _mimeType,
            _ipfsHash,
            _size,
            _name,
            _description,
            timestamp,
            timestamp,
            true
        );

        return _self.maxindex;
    }

    function get (Data storage _self, uint _index)
    public
    view
    existed (_self, _index)
    returns (
        bytes32 mimeType,
        bytes32[6] ipfsHash,
        bytes32 size,
        bytes32[3] name,
        bytes32[6] description,
        uint32 created,
        uint32 modified
    ) {
        mimeType = _self.files[_index].mimeType;
        size = _self.files[_index].size;
        ipfsHash = _self.files[_index].ipfsHash;
        name = _self.files[_index].name;
        description = _self.files[_index].description;
        created = _self.files[_index].created;
        modified = _self.files[_index].modified;
    }

    function get1 (Data storage _self, uint _index)
    public
    view
    existed (_self, _index)
    returns (
        bytes32 mimeType,
        bytes32[6] ipfsHash,
        bytes32 size,
        bytes32[3] name
    ) {
        mimeType = _self.files[_index].mimeType;
        size = _self.files[_index].size;
        ipfsHash = _self.files[_index].ipfsHash;
        name = _self.files[_index].name;
    }

    function get2 (Data storage _self, uint _index)
    public
    view
    existed (_self, _index)
    returns (
        bytes32[6] description,
        uint32 created,
        uint32 modified
    ) {
        description = _self.files[_index].description;
        created = _self.files[_index].created;
        modified = _self.files[_index].modified;
    }

    function setName (Data storage _self, uint _index, bytes32[3] _value, uint32 timestamp)
    public
    existed (_self, _index) {
        // todo add payable
        _self.files[_index].name = _value;
        _self.files[_index].modified = timestamp;
    }

    function setDescription(Data storage _self, uint _index, bytes32[6] _value, uint32 timestamp)
    public
    existed (_self, _index) {
        // todo add payable
        _self.files[_index].description = _value;
        _self.files[_index].modified = timestamp;
    }
}