using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Discord;
using Discord.WebSocket;

namespace GuildBankNow
{
    class Program
    {
        private static ConfigManager _config = new ConfigManager();
        private static LuaManager _lua;
        private static List<Character> _characters = new List<Character>();
        private static string _regexNameParserText = @"(\[.*])";
        private static DiscordSocketClient _client =  new DiscordSocketClient();
        private static string _message;
        static void Main(string[] args)
        {
            _lua = new LuaManager(_config.luaFileName,_regexNameParserText);
            List<string> availableCharacterNames = _lua.availableCharacterNames;
            foreach(string name in availableCharacterNames){
                //TODO:: If Name is one of the wanted names
                if(_config.characterNames.Contains(name)){
                    Character character = new Character(name);
                    _lua.getCharacterItems(character);
                    character.sortItemsByCount(true);
                    _characters.Add(character);

                }
            }
            _message += buildScanDateData();
            if(_config.showCharacterSpecificItems){
                _message += buildCharacterSpecificMessage();
            }
            if(_config.showAllItems || (!_config.showCharacterSpecificItems)){
                _message += buildMessage();
            }
            
            new Program().MainAsync().GetAwaiter().GetResult();
        }
        public Program() {
            _client.Ready += ReadyAsync;
            
        }
        public async Task MainAsync()
        {
            
            await _client.LoginAsync(TokenType.Bot, _config.discordBotToken);
            await _client.StartAsync();

            // Block the program until it is closed.
            await Task.Delay(Timeout.Infinite);
        }
        private Task ReadyAsync()
        {
            //Console.WriteLine($"{_client.CurrentUser} is connected!");
            bool msgFound = false;
            SocketGuild guild = _client.GetGuild(Convert.ToUInt64(_config.discordGuildID));
            IMessageChannel channel = guild.GetChannel(Convert.ToUInt64(_config.discordChannelID)) as IMessageChannel;
            if(_config.discordMessageID != null){
                Task<IMessage> msg = channel.GetMessageAsync(Convert.ToUInt64(_config.discordMessageID));
                msg.Wait();
                IUserMessage mesg = msg.Result as IUserMessage;
                if(mesg != null){
                    msgFound = true;
                    mesg.ModifyAsync(x => x.Content = _message);
                }
            }
            if(_config.discordMessageID == null || !msgFound){
                Task<IUserMessage> msg = channel.SendMessageAsync(_message);
                msg.Wait();
                IUserMessage mesg = msg.Result;
                _config.setDiscordMessageNumber(Convert.ToString(mesg.Id));
                _config.saveData();
            }
            
            Environment.Exit(0);
            return Task.CompletedTask;
        }
        private static string buildScanDateData(){
            string message = "The data below is based on the Bank and Bags from the following characters:\n";
            message +="```";
            foreach(Character character in _characters){
                message += character.Name+"\n";
                message += "- Last Bag Scan: \t"+character.LastBagScan+"\n";
                message += "- Last Bank Scan:\t"+character.LastBankScan+"\n";
            }
            message +="```";
            return message;
        }
        private static string buildCharacterSpecificMessage(){
            string message = "";
            foreach(Character character in _characters){
                List<Item> tmpItems = new List<Item>();
                message += character.Name+" Items:";
                message +="```";
                foreach(Item item in character.Items){
                    
                    Item tempItem = tmpItems.Find(p => p.Id == item.Id);
                    if(tempItem != null){
                        tempItem.Count += item.Count;
                    }
                    else{
                        tmpItems.Add(item);
                    }
                }
                foreach(Item item in tmpItems){
                    if(_config.excludedItemIDs.Contains(Convert.ToInt64(item.Id))){
                        continue;
                    }
                    message += item.ItemName+"x"+item.Count+"\n";
                }
                message +="```";
            }
            return message;
        }
        private static string buildMessage(){
            string message = "All Items:";
            List<Item> tmpItems = new List<Item>();
            message += "```";
            foreach(Character character in _characters){
                foreach(Item item in character.Items){
                    
                    Item tempItem = tmpItems.Find(p => p.Id == item.Id);
                    if(tempItem != null){
                        tempItem.Count += item.Count;
                    }
                    else{
                        tmpItems.Add(item);
                    }
                }
            }
            foreach(Item item in tmpItems){
                if(_config.excludedItemIDs.Contains(Convert.ToInt64(item.Id))){
                    continue;
                }
                message += item.ItemName+"x"+item.Count+"\n";
            }
            message += "```";
            return message;
        }
    }
}