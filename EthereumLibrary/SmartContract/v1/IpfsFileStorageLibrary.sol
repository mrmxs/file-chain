pragma solidity ^0.4.24;

library IpfsFileStorage {

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

    struct Data {
        uint filesCount;
        mapping(uint => IpfsFile) files;
    }

    modifier existed (Data storage _self, uint _index) {
        if (!contains(_self, _index)) revert("NOT EXISTING INDEX");
        _;
    }

    function addIpfsFileToStorage(
        Data storage _self,
        string _mimeType,
        uint _size,
        string _ipfsHash,
        string _name,
        string _description,
        bool _isActive,
        uint _created,
        uint _accessed,
        uint _modified
    )
    public
    returns (uint index){
        // todo add payable
        // todo some checks
        _self.files[++_self.filesCount] = IpfsFile(
            _mimeType, _size, _ipfsHash, _name, _description, _isActive,
            _created, _accessed, _modified, true
        );

        return _self.filesCount;
    }

    // todo modifier owner+editors+viewers
    function getIpfsFile(Data storage _self, uint _index)
    public
    existed (_self, _index)
    returns (
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
        _self.files[_index].accessed = now;
        //todo should it be payable?

        mimeType = _self.files[_index].mimeType;
        size = _self.files[_index].size;
        ipfsHash = _self.files[_index].ipfsHash;
        name = _self.files[_index].name;
        description = _self.files[_index].description;
        isActive = _self.files[_index].isActive;
        created = _self.files[_index].created;
        accessed = _self.files[_index].accessed;
        modified = _self.files[_index].modified;
    }

    // todo modifier owner+editors
    function setName(Data storage _self, uint _index, string _value)
    public 
    existed (_self, _index) {
        // todo add payable
        // todo some checks
        _self.files[_index].name = _value;
        _self.files[_index].modified = now;
    }

    // todo modifier owner+editors
    function setDescription(Data storage _self, uint _index, string _value)
    public
    existed (_self, _index) {
        // todo add payable
        // todo some checks
        _self.files[_index].description = _value;
        _self.files[_index].modified = now;
    }

    function contains(Data storage _self, uint _index)
    public
    view
    returns (bool result) {
        return _self.files[_index].isValue;
    }
}