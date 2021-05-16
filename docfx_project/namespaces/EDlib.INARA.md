---
uid: EDlib.INARA
summary: *content
---
The EDlib.INARA namespace contains classes and resources for interacting with the [INARA API](https://inara.cz/inara-api-devguide/).

**NOTE: Before using the INARA API you must contact [Artie](https://inara.cz/cmdr/1/), owner of INARA to have your app whitelisted.**

All requests to the INARA API require an `InaraIdentity` object which encapsulates authentication data for the INARA API with the properties:
* AppName - The name of the application, must match the name whitelisted by INARA.
* AppVersion - The version of the application.
* ApiKey - A user's personal API key or a generic application API key (for read-only events).
* IsDeveloped - A flag indicating this version is in development.

## CommanderProfileService
Gets basic information about a Commander from the INARA API like ranks, squadron and allegiance. 
See INARA documentation for [getCommanderProfile](https://inara.cz/inara-api-docs/#event-2).

Note: The information returned will be determined by the Commander's privacy settings on INARA.

## CommunityGoalsService

Gets details of ongoing and recently completed Community Goals from the INARA API. 
See INARA documentation for [getCommunityGoalsRecent](https://inara.cz/inara-api-docs/#event-37)).
