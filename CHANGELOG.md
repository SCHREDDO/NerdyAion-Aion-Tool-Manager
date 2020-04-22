# Changelog
## = 2.0.0 March 26th 2019 =
### Added
- new setting: dmg_template
- new setting: show_max
- new setting: sort_by
- new setting: check_version
- new setting: new_version_browser
- add a possibility (setting: sort_by) to order by `name`
- add a possibility (setting: sort_by) to order by `dmg`
- add a possibility (setting: sort_by) to order by `dps`
- add a possibility (setting: sort_by) to order by `dot_dmg`
- add a possibility (setting: sort_by) to order by `crit_dmg`
- add a possibility (setting: sort_by) to order by `action_time`
- add a possibility (setting: sort_by) to order by `skill_count`
- add a possibility (setting: dmg_template) to modify the output pattern for the `dmg show/copy` command
- add pattern placeholder `<player>`
- add pattern placeholder `<dmg>`
- add pattern placeholder `<dps>`
- add pattern placeholder `<dot_dmg>`
- add pattern placeholder `<crit_dmg>`
- add pattern placeholder `<used_skills_count>`
- add pattern placeholder `<time>`
- new command: dmg
- new command: settings
- add possibility to check for a new version (command: info)
- add possibility to open download page for new version (command: info)
- new parameter (`-a`) to define the wanted info from the command: info
- new parameter (`-r`) to define the wanted info from the command: info
- new parameter (`-v`) to define the wanted info from the command: info
- new parameter (`-c`) to define the wanted info from the command: info
### Changed
- rework the backbone of the command system
- rename setting: languarge to language
- updated all help command outputs
### Removed
- remove command: goto
- remove command: back
- remove damage (path: dmg) command: create
- remove damage (path: dmg) command: list
- remove damage (path: dmg) command: show
- remove damage (path: dmg) command: copy
- remove settings (path: settings) command: show
- remove settings (path: settings) command: edit
- remove settings (path: settings) command: save
- remove settings (path: settings) command: undo
## = 1.0.0 March 26th 2019 =
### Added
- add language support for english
- new base setting: languarge
- new damage (path: dmg) command: reset
- new damage (path: dmg) command: delete
## = 0.5.0-alpha March 25th 2019 =
### Added
- new base setting: check_chatlog_active
## = 0.4.0-alpha March 25th 2019 =
### Added
- new base command: chatlog
- new base setting: aion
### Changed
- changed base command: info (optical change)
### Removed
- removed base setting: log
## = 0.3.0-alpha March 24th 2019 =
### Added
- new damage informationen: dps (damage per second) 
- new base setting: player
### Fixed
- Fix damage calculation: the same attack added multiple times
- Fix damage calculation: damage tick associated for multiple attacks
## = 0.2.0-alpha March 23rd 2019 =
### Changed
- expand the damage calculation by dot (damage over time) ticks
- expand the damage calculation by critique hits
## = 0.1.0-alpha March 18th 2019 =
### Added
- damage calculation on the base of the immediate damage
- new base command: goto
- new base command: back
- new base command: clear
- new base command: info
- new base command: help
- new base command: bye
- new damage (path: dmg) command: create
- new damage (path: dmg) command: list
- new damage (path: dmg) command: show
- new damage (path: dmg) command: copy
- new settings (path: settings) command: show
- new settings (path: settings) command: edit
- new settings (path: settings) command: save
- new settings (path: settings) command: undo
- new base setting: log