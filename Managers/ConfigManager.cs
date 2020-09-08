using System.Collections.Generic;
using IniParser;
using IniParser.Model;
using System.Text.Json;



namespace GuildBankNow
{
    class ConfigManager
    {
        private static FileIniDataParser _parser = new FileIniDataParser();
        private static IniData _data;
        public string luaFileName;
        public string sortByValue;
        public List<string> characterNames;
        public List<long> excludedItemIDs;
        public string discordBotToken;
        public string discordGuildID;
        public string discordChannelID;
        public string discordMessageID;
        public bool reverseSort;
        public bool showAllItems;
        public bool showCharacterSpecificItems;
        public ConfigManager(){
            this.getData();
        }
        public void saveData(){
            _parser.WriteFile("GuildBankNow.ini",_data);
        }
        public void setDiscordMessageNumber(string messageNumber){
            _data["OPTIONS"]["discordmessagenumber"] = messageNumber;
        }
        private void getData(){
            _data = _parser.ReadFile("GuildBankNow.ini");
            //Options
            this.setLuaFileName();
            this.setDiscordToken();
            this.setDiscordGuildNumber();
            this.setDiscordChannelNumber();
            this.setDiscordMessageNumber();
            this.setCharacterNames();
            this.setExcludedItems();

            //Display
            this.setShowAllItems();
            this.setShowCharacterSpecificItems();
            this.setSortBy();
            this.setReverseSort();
        }
        private void setLuaFileName(){
            luaFileName = _data["OPTIONS"]["filename"];
            if(luaFileName == ""){
                luaFileName = null;
            }
        }
        private void setDiscordToken(){
            discordBotToken = _data["OPTIONS"]["discordtoken"];
            if(discordBotToken == ""){
                discordBotToken = null;
            }
        }
        private void setDiscordGuildNumber(){
            discordGuildID = _data["OPTIONS"]["discordguildnumber"];
            if(discordGuildID == ""){
                discordGuildID = null;
            }
        }
        private void setDiscordChannelNumber(){
            discordChannelID = _data["OPTIONS"]["discordchannelnumber"];
            if(discordChannelID == ""){
                discordChannelID = null;
            }
        }
        private void setDiscordMessageNumber(){            
            discordMessageID = _data["OPTIONS"]["discordmessagenumber"];
            if(discordMessageID == ""){
                discordMessageID = null;
            }
        }
        private void setCharacterNames(){            
            string tempData = _data["OPTIONS"]["characternames"];
            characterNames = JsonSerializer.Deserialize<List<string>>(tempData);
        }
        private void setExcludedItems(){            
            string tempData = _data["OPTIONS"]["excludeditems"];
            excludedItemIDs = JsonSerializer.Deserialize<List<long>>(tempData);
        }
        private void setShowAllItems(){            
            showAllItems = _data["DISPLAY"]["allitems"] == "True";
        }
        private void setShowCharacterSpecificItems(){            
            showCharacterSpecificItems = _data["DISPLAY"]["characterspecificitems"] == "True";
        }
        private void setReverseSort(){            
            reverseSort = _data["DISPLAY"]["reversesort"] == "True";
        }
        private void setSortBy(){            
            sortByValue = _data["DISPLAY"]["sortby"];
        }
    }
}
