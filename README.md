# RoyaleBrawl
Private brawl stars server with PvP Battles (v27.269)

![Screenshot](https://github.com/chainsawy/RoyaleBrawl/blob/main/screenshots/ingame.jpg)

## [DOWNLOAD CLIENT](https://drive.google.com/file/d/1DAZRiVBqTwVrE9IivYz0DOroFcg3YFoq/view?usp=sharing)

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
- Download apk here: [link](https://drive.google.com/file/d/1DAZRiVBqTwVrE9IivYz0DOroFcg3YFoq/view?usp=sharing)
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
