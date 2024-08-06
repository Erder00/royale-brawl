# RoyaleBrawl
Private brawl stars server with PvP Battles (v27.269)

# this server is made by xeondev and not by me! i'm just archiving it here bc other archives on github are broken or don't include the client. also maybe i'll do a few improvements if i feel like it. :)

## [Full setup guide + showcase](https://www.youtube.com/watch?v=zDfj-73MKLc) (video is blocked in Russia, use a vpn)
### ROYALE BRAWL RUNS BETTER ON LINUX, THE VIDEO SHOWS THE WINDOWS SETUP, BECAUSE I THINK MOST PEOPLE USE WINDOWS!

![Screenshot](https://github.com/Erder00/royale-brawl/blob/main/screenshots/ingame.jpg)

## [DOWNLOAD CLIENT](https://mega.nz/file/zuAVxKII#JmnmSeK7V76x1xhpaOcBJa29ls5eLFsem7Gdo029hj4)

### Setup
##### Compiling
- Method 1: Open .sln file in Visual Studio and run project `Supercell.Laser.Server`
- Method 2: Compile `Supercell.Laser.Server.csproj` using `dotnet` & run it using `dotnet Supercell.Laser.Server.dll`
##### Database
You need to setup mysql server and import `database.sql` file from this repository.
##### Configuration
- Set your ip in `config.json` -> `udp_host`
- Set your mysql database name, password, and username in `config.json`
##### Client
- Download apk here: [link](https://mega.nz/file/zuAVxKII#JmnmSeK7V76x1xhpaOcBJa29ls5eLFsem7Gdo029hj4)
- Open this file: `lib/armeabi-v7a/libreversed.script.so` and put your ip address in `HostPatcher`
- Compile apk and play enjoy playing this server.

### Requirements
- dotnet 6.0
- mysql (on windows I prefer *XAMPP* with *phpmyadmin*)

### Features:
- PvP Battles (3 Game modes working: Gem Grab, Showdown, Bounty)
- Shop with daily deals
- Online game rooms
- Friends & Alliances
- Brawl Pass & Trophy Road
- Leaderboards
- Events refresh
- slash commands in clubs
- slash commands in terminal

### all availible club commands:
- /help
- /status

### all availible console commands:
- /unlockall [TAG]
- /premium [TAG] (does nothing if u don't specify anything)
- /unban [TAG]
- /ban [TAG]
- /changevalue [TAG] [NewName]
- /settrophies [TAG] [ammount]
- /addgems [TAG] [ammount]
- /addcoins [TAG] [ammount]
- /addtokens [TAG] [ammount]
- /addstartokens [TAG] [ammount]
- /removecoins [TAG] [ammount] (idk why it's here, u could just use a negative value with /addcoins)
- /removegems [TAG] [ammount] (same here, no idea why it's there)
- /removestartokens [TAG] [ammount] (yep, here too)
- /givedev [TAG] (also doesn't do anything if u don't specify it, maybe i'll add club cmds that only devs can use or sum)
- /changevalue [TAG] [FieldName]
- /changevalue [TAG] [FieldName] [Value] (twice? wtf)
- /givebrawler [tag] [brawler]
