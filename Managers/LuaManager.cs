using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NLua;
namespace GuildBankNow
{
    class LuaManager
    {
        private static Lua _lua = new Lua();
        private static String _CharTablePath = "GuildBankNowDB.char";
        public List<string> availableCharacterNames = new List<string>();
        private string _regexNameParserText;
        public LuaManager(string filepath, string regexNameParserText){
            _regexNameParserText = regexNameParserText; 
            _lua.DoFile(filepath);
            LuaTable CharTable = _lua.GetTable(_CharTablePath);
            IDictionaryEnumerator CharTableEnumerator = CharTable.GetEnumerator();
            this.setCharTable(CharTableEnumerator);           
        }
        private void setCharTable(IDictionaryEnumerator tableEnumerator){
            while(tableEnumerator.MoveNext()){
                availableCharacterNames.Add(tableEnumerator.Entry.Key.ToString());
            }
        }
        private void getItemsInBag(IDictionaryEnumerator tableEnumerator,string previousPath,Character character){
            Regex rx = new Regex(_regexNameParserText);
            while(tableEnumerator.MoveNext()){
                Item item = new Item();
                string pathPrefix = previousPath+"."+tableEnumerator.Entry.Key.ToString()+".";
                try{
                    Match match = rx.Match(_lua.GetString(pathPrefix+"itemLink"));
                    string itemNameValue = match.Value.ToString();
                    item.ItemName = itemNameValue;
                    item.Id = _lua.GetString(pathPrefix+"ID");
                    item.SlotNumber = _lua.GetString(pathPrefix+"slotNumber");
                    item.ContainerNumber = _lua.GetString(pathPrefix+"containerNumber");
                    item.ContainerType = _lua.GetString(pathPrefix+"containerType");
                    item.Count = Convert.ToInt64(_lua.GetString(pathPrefix+"count"));
                    character.addItem(item);
                }
                catch(NullReferenceException){
                    continue;
                }
            }
        }
        private void getBags(IDictionaryEnumerator tableEnumerator,string previousPath,Character character){
            while(tableEnumerator.MoveNext()){
                Item item = new Item();
                string path = previousPath+"."+tableEnumerator.Entry.Key.ToString();
                LuaTable bagsTable  = _lua.GetTable(path);
                IDictionaryEnumerator bagsTableEnumerator = bagsTable.GetEnumerator();
                this.getItemsInBag(bagsTableEnumerator,path,character);
            }
        }

        public void getCharacterItems(Character character){
            string charBagsPath = _CharTablePath+"."+character.Name+".Bag.Items";
            string charBagScanPath = _CharTablePath+"."+character.Name+".Bag.lastScanned";
            string charBankBagsPath = _CharTablePath+"."+character.Name+".Bank.Items";
            string charBankScanPath = _CharTablePath+"."+character.Name+".Bank.lastScanned";

            character.LastBagScan = _lua.GetString(charBagScanPath);
            character.LastBankScan = _lua.GetString(charBankScanPath);
            
            LuaTable charBagsTable  = _lua.GetTable(charBagsPath);
            IDictionaryEnumerator charBagsTableEnumerator = charBagsTable.GetEnumerator();
            this.getBags(charBagsTableEnumerator,charBagsPath,character);

            LuaTable charBankBagsTable = _lua.GetTable(charBankBagsPath);
            IDictionaryEnumerator charBankBagsTableEnumerator = charBankBagsTable.GetEnumerator();
            this.getBags(charBankBagsTableEnumerator,charBankBagsPath,character);
        }
    }
}
