using System;

namespace GuildBankNow
{
    class Item
    {
        private long _count;
        private string _containerNumber;
        private string _slotNumber;
        private string _itemName;
        private string _containerType;
        private string _id;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public long Count
        {
            get { return _count; }
            set { _count = value; }
        }
        public string ContainerNumber
        {
            get { return _containerNumber; }
            set { _containerNumber = value; }
        }
        public string SlotNumber
        {
            get { return _slotNumber; }
            set { _slotNumber = value; }
        }
        public string ItemName
        {
            get { return _itemName; }
            set { _itemName = value; }
        }
        public string ContainerType
        {
            get { return _containerType; }
            set { _containerType = value; }
        }

        public override string ToString()
        {
            return _count+" "+_itemName+" stored in "+_containerType+" number "+_containerNumber+" at location "+_slotNumber;
        }
    }
}