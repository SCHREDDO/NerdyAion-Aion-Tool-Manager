# Commands

| Name | Description |
|------|-------------|
| [info](#command-info) | information about NerdyAion |
| [help](#command-help) | shows commands and information |
| [clear](#command-clear) | clears the console |
| [settings](#command-settings) | handling settings |
| [dmg](#command-dmg) | provides damage information |
| [chatlog](#command-chatlog) | activate or deactivate the chatlog |
| [bye](#command-bye) | close NerdyAion |

## Command "info"
Displayed information about the used application.

Syntax:
```console
info [-a | -r | -v | -c]
```
Arguments:
- `-a`: shows only the author of the application
- `-r`: shows only the repository of the application
- `-v`: shows only the version of the application
- `-c`: checks if there is a new version available

Example:
```console
info
info -v
```

## Command "help"
Provides information about commands. If used without parameters a lists of all commands are displayed without a briefly description.

Syntax:
```console
help [command]
```
Arguments:
- `command`: the command you wish to receive more information about

Example:
```console
help
help dmg
```

## Command "clear"
Deletes the display information of the current console window.

Syntax:
```console
clear
```
Example:
```console
clear
```

## Command "settings"
For handling application settings.

Syntax:
```console
settings <show | edit | save | undo | reset> [-s] [setting] [value]
```
```console
settings show
settings edit [-s] <setting> <value>
settings save
settings undo
settings reset
```
Arguments:
- `show`: shows all settings and values
- `edit`: edit the `<setting>` with the new value `<value>`
- `save`: saved changes from settings
- `undo`: reset the last changes from settings, only if not saved
- `reset`: reset the last changes from settings, only if not saved
- `-s`: for saving instantly
- `setting`: which setting is to be changed
- `value`: what value the setting should be set to

Example:
```console
settings show
settings edit language EN
settings edit -s language DE
settings save
settings undo
settings reset
```

## Command "dmg"
For handling damage information.

Syntax:
```console
dmg <add | list | clear | remove | show | copy> [pointer name]
```
```console
dmg add <pointer name>
dmg list
dmg clear <pointer name>
dmg remove <pointer name>
dmg show <pointer name>
dmg copy <pointer name>
```
Arguments:
- `add`: add a pointer (start point) for the analyzing with the given name `<pointer name>`
- `list`: shows all pointer.
- `clear`: reset a given pointer
- `remove`: remove a given pointer
- `show`: Shows damage information of a given pointer
- `copy`: Copy damage information of a given pointer
- `pointer name`: the pointer name

Example:
```console
dmg add boss
dmg list
dmg clear boss
dmg remove boss
dmg show boss
dmg copy boss
```

## Command "chatlog"
Activate or deactivate the Aion chatlog. Aion is not allowed to run for this process.

Syntax:
```console
chatlog <option>
```
Arguments:
- `option`: on or off

Example:
```console
chatlog on
```

## Command "bye"
Closes the application.

Syntax:
```console
bye
```
Example:
```console
bye
```
