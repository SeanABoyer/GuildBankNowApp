using System;
using System.Collections.Generic;

namespace GuildBankNow
{
    class Character
    {
        public Character(string name){
            _name = name;
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _LastBagScan;
        public string LastBagScan
        {
            get { return _LastBagScan; }
            set { _LastBagScan = value; }
        }
        private string _LastBankScan;
        public string LastBankScan
        {
            get { return _LastBankScan; }
            set { _LastBankScan = value; }
        }
        
        private List<Item> _items = new List<Item>();

        public void addItem(Item item){
            _items.Add(item);
        }
        public void sortItemsByCount(bool reverse){
            if(reverse){
                _items.Sort((x,y) => y.Count.CompareTo(x.Count));
            }
            else{
                _items.Sort((x,y) => x.Count.CompareTo(y.Count));
            }
        }
        public List<Item> Items
        {
            get { return _items; }
        }
    }
}
