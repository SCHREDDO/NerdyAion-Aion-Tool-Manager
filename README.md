# NerdyAion-Aion-Tool-Manager
![build version](https://img.shields.io/badge/version-v2.1.0-brightgreen.svg)
![release version](https://img.shields.io/badge/release-v2.1.0-blue.svg)
![framework or language](https://img.shields.io/badge/.net-4.6.1-blue.svg)
![license](https://img.shields.io/badge/license-AGPL--3.0-lightgrey.svg)

### Current Version 2.1.0
### Development Version 2.1.0

## About
NerdyAion is a DMG meter for the game Aion. The primary goal of NerdyAion is to provide the user with useful information, especially about his and others players DMG data. NerdyAion is programed in C# (.net 4.6.1).

##### Info: 
- English isn’t my first language, so please excuse any mistakes.
- Contact me for proposals or questions.
- The icon was generated with [Android Asset Studio](https://romannurik.github.io/AndroidAssetStudio/).

## Overview
- [About](#about)
- [Getting Started](#getting-started)
- [Supported Languages](#supported-languages)
- [Commands](#commands)
- [Settings](#settings)
- [Roadmap](#roadmap)
- [Dependencies](#dependencies)
- [Changelog](#changelog-complete-changelog-changelogmd)
- [Support Possibilities](#support-possibilities)
- [License](#license)

## Getting Started
0. install [.net runtime](https://dotnet.microsoft.com/download)
1. download [build.zip](./releases)
2. execute NerdyAion.exe
3. set the path to the Aion folder (setting: aion) example: `settings edit -s aion "C:\Program Files\Gameforge\AION Free-To-Play"`
4. set you Aion clinet languages (setting: languarge) example: `settings edit -s language DE`
5. activate Aion Chat.log with `chatlog on`
6. [optional] enable automatic check if Chat.log is enabled (setting: check_chatlog_active) example: `settings edit -s check_chatlog_active 1`
7. start Aion
8. use the dmg commands to get information

## Supported Languages
- german (DE)
- english (EN)

## Commands

| Name | Description |
|------|-------------|
| [info](./COMMANDS.md#command-info) | information about NerdyAion |
| [help](./COMMANDS.md#command-help) | shows commands and information |
| [clear](./COMMANDS.md#command-clear) | clears the console |
| [settings](./COMMANDS.md#command-settings) | handling settings |
| [dmg](./COMMANDS.md#command-dmg) | provides damage information |
| [chatlog](./COMMANDS.md#command-chatlog) | activate or deactivate the chatlog |
| [bye](./COMMANDS.md#command-bye) | close NerdyAion |

## Settings
| Name | Description |
|------|-------------|
| language | language of the Aion client (DE, EN) |
| aion | the path to Aion (e.g. 'C:\Program Files\Gameforge\AION Free-To-Play') |
| check_chatlog_active | check by start from NerdyAion if chatlog is active if not chatlog will be activated (0, 1) |
| player | name by which the player is displayed |
| dmg_template | "dmg show/copy" information structure ([Damage template](#damage-template)) |
| show_max | "dmg show/copy" only shows the x players (e.g. 10) for all use 0 |
| sort_by | "dmg show/copy" sort the displayed players (name, dmg, dps, dot_dmg, crit_dmg, action_time, skill_count) |
| check_version | Checks if there is a new version available from NerdyAion by start (0, 1) |
| new_version_browser | If a new version available the '/releases' will be open in browser (0, 1) |

### Damage Template
Damage template are for the customization from the command dmg show and dmg copy. 
- `<player>`: name of the player
- `<dmg>`: player damage
- `<dps>`: player dps
- `<dot_dmg>`: damage from dots from player
- `<crit_dmg>`: crit damage from player
- `<used_skills_count>`: number of skills/attacks performance from player
- `<time>`: time in sec where the player was active

Example:
```console
<player>: <dmg>(<dps>)
The <player> has a dps of <dps>.
<player> did <crit_dmg> crit damage in <time> sec.
```

## Roadmap
### v2.x.x
- rework command system
- expand base settings
- add damage show settings
- expand damage command with more options
- add interval damage calculation
### v3.x.x
- add a heal command thats provides heal information
- heal calculation on the base of the immediate heal and by heal ticks (heal over time)
### v4.x.x
- expand the damage calculation by support damage (boosted damage)
- expand the heal calculation by support heal (boosted heal)
- expand damage command with more options
### v5.x.x
- add a graphical user interface
- add 'realtime' damage and heal calculation

## Dependencies
### Runtime Dependencies
- .net runtime 
### Development Dependencies
- .net 4.6.1

## Changelog ([complete changelog: CHANGELOG.md](./CHANGELOG.md))
### = 2.1.0 May 7th 2020 =
#### Added
- add possibility to analyzed Chat.log in background
- new parameter (`-i`) to analyzed Chat.log in background, command: dmg
- add check if all NerdyAion files exists by start from NerdyAion.exe
- add check if the set Aion folder exists (respectively system.cfg) by start from NerdyAion.exe
- add check if Chat.log exists by start from NerdyAion.exe
- add check if Aion.exe is running by start from NerdyAion.exe
- add check if nerdy.ini exists if not a default will be generated by start from NerdyAion.exe
- add check if base.conf exists if not a default will be generated by start from NerdyAion.exe
#### Fixed
- NerdyAion will no longer crash if Chat.log does not exists

## Support Possibilities
- give proposals
- report bugs

## License
NerdyAion is released under the [AGPL-3.0](https://www.gnu.org/licenses/agpl-3.0.de.html) License.
