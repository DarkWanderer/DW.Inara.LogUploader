![ELA logo](EliteLogAgent/Resources/elite-dangerous-icon.ico)

# Elite-Log-Agent

A windows utility written in C#, presenting Json events from Elite: Dangerous log as data source. 
Allows updating external sites (and possibly do other actions) in near-real-time as the log gets updated

**Does not require Elite login/password - only the target sites API keys**

Currently in beta stage

## Quickstart

* Download [latest version][clickonce]
* Input commander name, Inara and/or EDSM API keys, click 'verify' to check info is correct
* Save settings (apply and/or OK)
* (optional) Upload logs from last few sessions
* launch the game.

## Current Features

* Inara
* EDSM
* EDDN

## Planned features

* "Cloud" error reporting
* Support for [Canonn API v2](https://github.com/canonn-science/CAPIv2-Strapi)
* Google sheets for Powerplay/BGS work

## Get latest version

* ClickOnce application installer [here][clickonce]

## Plugin development

The code base has not yet reached maturity, so any (even ostensibly public) APIs might change. Hence, at this time, I see the best way of plugin support as incorporating them into this codebase. This will most likely change once a stable version is reached.
To contribute to development, please fork the repository

## Contributions

You're welcome to contribute by:

1. Using the application!
2. Raising [issues](https://github.com/DarkWanderer/Elite-Log-Agent/issues) on GitHub
3. Proposing pull request with changes and/or new functionality, including plugins

## SDLC

Builds are done in AppVeyor. `master` branch is the primary integration branch ('potentially releasable').
GitHub pre-releases are published from `master`
Releasing to ClickOnce installer is done via merging to `prod` branch

| Branch        | Build status  |
| ------------- | ------------: |
| master        | [![appveyor build status][buildstatus-master]][project] |
| prod          | [![appveyor build status][buildstatus-prod]][project]   |

## Links

* [Elite: Dangerous in official store](https://www.frontierstore.net/games/elite-dangerous-cat.html)
* [INARA](https://inara.cz)
* [EDSM](https://edsm.net)

[buildstatus-master]: https://ci.appveyor.com/api/projects/status/6n52i9wkthtwtb34/branch/master
[buildstatus-prod]: https://ci.appveyor.com/api/projects/status/6n52i9wkthtwtb34/branch/prod
[project]: https://ci.appveyor.com/project/DarkWanderer/Elite-Log-Agent
[clickonce]: https://elitelogagent.blob.core.windows.net/clickonce/EliteLogAgent.application
[releases]: https://github.com/DarkWanderer/Elite-Log-Agent/releases
