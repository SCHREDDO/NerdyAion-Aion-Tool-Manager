# Commands

| Name | Description | Path |
|------|-------------|------|
|[goto](https://github.com/SCHREDDO/NerdyAion-Aion-Tool-Manager/blob/master/COMMANDS.md#command-goto)|go to path x||
|[back](https://github.com/SCHREDDO/NerdyAion-Aion-Tool-Manager/blob/master/COMMANDS.md#command-back)|go back to main path||
|[clear](https://github.com/SCHREDDO/NerdyAion-Aion-Tool-Manager/blob/master/COMMANDS.md#command-clear)|cleared the console||
|[info](https://github.com/SCHREDDO/NerdyAion-Aion-Tool-Manager/blob/master/COMMANDS.md#command-info)|informations about NerdyAion||
|[help](https://github.com/SCHREDDO/NerdyAion-Aion-Tool-Manager/blob/master/COMMANDS.md#command-help)|shows commands and paths||
|[bye](https://github.com/SCHREDDO/NerdyAion-Aion-Tool-Manager/blob/master/COMMANDS.md#command-bye)|close NerdyAion||
|[create](https://github.com/SCHREDDO/NerdyAion-Aion-Tool-Manager/blob/master/COMMANDS.md#command-create)|create a dmg chatlog pointer|dmg|
|[show](https://github.com/SCHREDDO/NerdyAion-Aion-Tool-Manager/blob/master/COMMANDS.md#command-show)|shows dmg informations from pointer x|dmg|
|[copy](https://github.com/SCHREDDO/NerdyAion-Aion-Tool-Manager/blob/master/COMMANDS.md#command-copy)|copy dmg informations from pointer x|dmg|
|[show](https://github.com/SCHREDDO/NerdyAion-Aion-Tool-Manager/blob/master/COMMANDS.md#command-show)|list of settings|settings|
|[edit](https://github.com/SCHREDDO/NerdyAion-Aion-Tool-Manager/blob/master/COMMANDS.md#command-edit)|edit a setting|settings|
|[save](https://github.com/SCHREDDO/NerdyAion-Aion-Tool-Manager/blob/master/COMMANDS.md#command-save)|saved changes from settings|settings|
|[undo](https://github.com/SCHREDDO/NerdyAion-Aion-Tool-Manager/blob/master/COMMANDS.md#command-undo)|reset the last changes|settings|

# Base Commands
## Command "goto"
Sets the activ path to the path `<path>`.
```console
goto <path>
```
Arguments:
- `path` the path
## Command "back"
Sets the activ path to the main path
```console
back
```
## Command "clear"
Cleared the console.
```console
clear
```
## Command "info"
Shows informations about NerdyAvion
```console
info
```
## Command "help"
Shows a list of commands and paths or informations about a the `<command>`.
```console
help [command]
```
Arguments:
- `command` the command
## Command "bye"
Closes NerdyAion.
```console
bye
```
# Damage (Path: dmg) Commands
## Command "create"
Create a pointer (start point) for the analyzing with the given name `<pointer name>`.
```console
create <pointer name>
```
Arguments:
- `pointer name` the pointer name
## Command "list"
Shows all pointer.
```console
list
```
## Command "show"
Shows dmg informations from pointer `<pointer name>`.
```console
show <pointer name>
```
Arguments:
- `pointer name` the pointer name
## Command "copy"
Cpy the dmg informations from pointer `<pointer name>`.
```console
copy <pointer name>
```
Arguments:
- `pointer name` the pointer name
# Settings (Path: settings) Commands
## Command "show"
Shows all settings an values.
```console
show
```
## Command "edit"
Edit the `<setting>` with the new value `<value>`.
```console
edit <setting> <value>
```
Arguments:
- `setting` which setting is to be changed
- `value` what value the setting should be set to 
## Command "save"
Saved changes from settings.
```console
save
```
## Command "undo"
Reset the last changes from settings.
```console
undo
```