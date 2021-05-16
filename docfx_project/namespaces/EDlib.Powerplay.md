---
uid: EDlib.Powerplay
summary: *content
---
The EDlib.Powerplay namespace contains classes and resources that get Powerplay data from an API provided by Taranis Software.

## CycleService
This service does not request data from an online API but calculates the current Powerplay cycle and when it changes (ticks).

## PowerDetailsService
Provides data about every Power including statistics, ethos and benefits. 
Also gets every Powerplay group's communications data (Reddit and Discord / Slack) from an API provided by Taranis Software. This data changes rarely so please cache for several days minimum.

## StandingsService
Gets the current Powerplay Galactic Standings from an API provided by Taranis Software. 
The standings update weekly on a Thursday between 07:00 - 09:00 UTC. Please cache the data till 07:00 UTC Thursday and then check every 15 minutes until an update is posted.
