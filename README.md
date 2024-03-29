# ![EDlib](edlib-logo-50.png) EDlib
A .Net Standard library for creating companion tools for the game Elite Dangerous by Frontier Developments.

<div align="center">
  
[![GitHub](https://img.shields.io/badge/GitHub-irongut/EDlib-informational?style=flat&logo=github)](https://github.com/irongut/EDlib)
&nbsp;
![.Net Standard](https://img.shields.io/badge/.Net-Standard%202.0-informational?style=flat&logo=visual-studio)
&nbsp;
[![CI Build Status](https://github.com/irongut/EDlib/actions/workflows/cibuild.yml/badge.svg)](https://github.com/irongut/EDlib/actions/workflows/cibuild.yml)
&nbsp;
[![CodeQL Scan](https://github.com/irongut/EDlib/actions/workflows/codeql-scan.yml/badge.svg)](https://github.com/irongut/EDlib/actions/workflows/codeql-scan.yml)

[![forthebadge](https://forthebadge.com/images/badges/made-with-c-sharp.svg)](https://forthebadge.com)
&nbsp;
[![forthebadge](https://forthebadge.com/images/badges/powered-by-coffee.svg)](https://forthebadge.com)

</div>

## Introduction

EDlib provides classes that enable your .Net apps to easily consume information about the Elite Dangerous universe from several APIs. Information available includes:

* The time that the Background Simulation (BGS) updates, known as the BGS tick.
* News articles from GalNet, classified by topic and content tags.
* The Powerplay cycle and Galactic Standings.
* Detailed information about each Power and communications links for their player groups.
* Data from [EDSM](https://www.edsm.net/) including game server status and information on systems, markets, shipyards and more.
* Data from [INARA](https://inara.cz/) including Community Goals and Commander profiles.

EDlib conforms to .Net Standard 2.0 making it compatible with .Net Framework, .Net Core and .Net 5 and has been used for CLI, desktop, Xamarin Forms and Azure Functions projects.

## Installation

EDlib is currently available as a Nuget package from GitHub.

### Installation from GitHub

To install EDlib from GitHub you will need to add either a [local Nuget package feed](https://docs.microsoft.com/en-us/nuget/hosting-packages/local-feeds) or GitHub Packages as a package feed using the [Package Manager UI](https://docs.microsoft.com/en-us/nuget/consume-packages/install-use-packages-visual-studio#package-sources) or the [Nuget CLI](https://docs.microsoft.com/en-us/nuget/reference/cli-reference/cli-ref-sources).

### Source Link

EDlib is built with Source Link enabled. Source Link enables source code debugging of .NET assemblies from NuGet by embedding source control metadata inside assemblies and the package. If you have Source Link enabled in Visual Studio you can step into the EDlib source code for a better debugging experience. To enable Source Link in Visual Studio please see [this excellent guide by Aaron Stannard](https://aaronstannard.com/visual-studio-sourcelink-setup/).

## Please Contribute!

This is an open source project that welcomes contributions, suggestions and bug reports from those who use it. If you have any ideas on how to improve the library, please read the [How to Contribute](/articles/how-to-contribute.html) article and [create an issue](https://github.com/irongut/EDlib/issues) on GitHub.

&nbsp;

&nbsp;

EDlib icon made by [Freepik](https://www.flaticon.com/authors/freepik) from [www.flaticon.com](https://www.flaticon.com/).
