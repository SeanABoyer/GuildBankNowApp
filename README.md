# GuildBankNowApp

## Compile from source code
If you would like to compile your own exe from this code. you will need dotnet 3.1.401 or greater. The following command can be used to compile to a single `.exe` file.

    
    PS C:\Users\UserName\Documents\GuildBankNowApp> dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true
    
## Getting Started
This application reads from a `.ini` file located in the same directory as the `.exe` called `GuildBankNow.ini`. An example file is provided, but some properties within the `.ini` file will need to be modified for your specific use, as well as the `.example` removed from the filename.

The first property that will need to be modified is the `filename` property. if your wow classic installation is in the default location, on your `C: drive`, then you will just need to replace `{WOW_ACCOUNT_NAME_GOES_HERE}` with the name of your account. Otherwise you will need to get the full path to your GuildBankNow.Lua SavedVariables file. This file may not generate until the addon is installed and you login/logout of a character on your account.

Example:

    filename = C:\Program Files (x86)\World of Warcraft\_classic_\WTF\Account\SeanABoyer\SavedVariables\GuildBankNow.lua

The second (`discordtoken`), third (`discordguildnumber`), and fourth(`discordchannelnumber`) properties will require a Discord Bot be setup for your specific use. The python docs for discords library has pretty solid steps on how to set those up [Discord Py Link](https://discordpy.readthedocs.io/en/latest/discord.html). Do note that the only the `"Send Messages"` Text Permission is needed. Once your bot is up and running, you can get the properties from the following locations:
-  Discord Token from [Discord Developer site](https://discord.com/developers/applications) and then going into your bot, under Settings > Bot > Copy Token.
- The next two require  enabling developer mode in the Discord Application, you can do this by going to Settings > Apperance > Advance > Developer Mode.
    - Discord Guild Number by going into your discord application and right clicking on your server icon > Copy ID.
    - Discord Channel number by going into your discord application, selecting your discord server and then right clicking on the channel > Copy ID.

Example:

    discordtoken = ABC123_LETSPRETENDTHISISMYBOTSTOKEN_321CBA
    discordguildnumber = 123456789101112
    discordchannelnumber = 1314151617181920

The fifth property of `discordmessagenumber` can be left blank, as the application will automatically fill that in, for reuse later on.

The sixth property of `characternames` will need to be a list of the characters you want to display their banks and bags from.

Example:

    characternames = ["Bankorone - Mankrik","Bankortwo - Mankrik"]

The seventh property of `excludeditems` is a `,` seperated list of numbers that you do not want to be listed in the discord server. By default it has only hearthstone (6948)

Example:

    excludeditems = [6948,1234,4567]

All of the properties under Display can be tinkered with to change the way text is displayed within the discord server.
- `allitems` [True/False] determines whether or not show all of them items joined togeother.
- `characterspecificitems` [True/False] determines whether or not to show character specific items.
- `reversesort` [True/False] determines whether or not to reverse the sort order (defaults to sorting by count with the largest at the top).