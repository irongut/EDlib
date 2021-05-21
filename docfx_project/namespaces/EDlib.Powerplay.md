---
uid: EDlib.Powerplay
summary: *content
---
The `EDlib.Powerplay` namespace contains classes and resources that get Powerplay data from an API provided by Taranis Software.

## CycleService
A static class that represents the Powerplay cycle and when it changes (ticks). This service does not request data from an online API.

## PowerDetailsService
Provides data about every Power including statistics, ethos and benefits. 
Also gets every Powerplay group's communications data (Reddit and Discord / Slack) from an API provided by Taranis Software. This data changes rarely so please cache for 1 week minimum.

## StandingsService
Gets the current Powerplay Galactic Standings from an API provided by Taranis Software. 
The standings update weekly on a Thursday between 08:00 - 10:00 UTC. Please cache the data until 08:00 UTC Thursday and then check every 15 minutes until an update is posted.
