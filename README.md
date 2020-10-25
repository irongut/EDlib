# ![EDlib](edlib-logo-50.png) EDlib
A .Net Standard library for creating companion tools for the game Elite Dangerous by Frontier Developments.

<div align="center">
  
[![forthebadge](https://forthebadge.com/images/badges/made-with-c-sharp.svg)](https://forthebadge.com)
&nbsp;
[![forthebadge](https://forthebadge.com/images/badges/powered-by-coffee.svg)](https://forthebadge.com)

![.Net Standard](https://img.shields.io/badge/.Net-Standard%202.0-informational?style=flat&logo=visual-studio)
&nbsp;
[![GitHub](https://img.shields.io/badge/GitHub-irongut/EDlib-informational?style=flat&logo=github)](https://github.com/irongut/EDlib)
&nbsp;
[![Build Status](https://dev.azure.com/taranissoftware/EDlib/_apis/build/status/irongut.EDlib?branchName=master)](https://dev.azure.com/taranissoftware/EDlib/_build/latest?definitionId=3&branchName=master)
&nbsp;
[![Dependabot Status](https://api.dependabot.com/badges/status?host=github&repo=irongut/EDlib&identifier=258617844)](https://dependabot.com)

</div>

## Introduction

EDlib provides classes that enable your .Net apps to easily consume information about the Elite Dangerous universe from several APIs. Information available includes:

* The time that the Background Simulation (BGS) updates, known as the BGS tick.
* News articles from GalNet, classified by topic and content.
* The current Powerplay Galactic Standings.
* Detailed information about each Power in Powerplay and communications links for their player groups.
* The current Powerplay cycle and the time until it ends.

## Installation

In the future EDlib will be available as a Nuget but for now you have two choices:

* Clone the EDlib repository, build the solution and include the dlls in your project.
* Use Git submodules to include EDlib in your solution and reference it from your code.

### Installation using Git Submodules

Git submodules is currently the preferred method of installation. To add EDlib to your repository as a submodule:

```
git submodule add https://github.com/irongut/EDlib EDlib
```

At this point you'll have an `EDlib` folder inside your project, but depending on your Git version it might be empty. Older versions of Git will require you to explicitly download the EDlib code:

```
git submodule update --init --recursive
```

If everything looks good, you can commit this change and you'll have an `EDlib` folder in your repository with all the content from the EDlib repository. On GitHub, the `EDlib` folder icon will have a little indicator showing that it is a submodule and clicking on it will take you over to the EDlib repository. Avoid making any changes to the files in the `EDlib` folder or running Git commands from that folder, if you want to contribute improvements please see the [section below](#please-contribute). For more information see the [Git submodules documentation](https://github.blog/2016-02-01-working-with-submodules/) on Github.

## Please Contribute!

This is an open source project that welcomes contributions, suggestions and bug reports from those who use it. If you have any ideas on how to improve the library, please read the [How to Contribute](/articles/how-to-contribute.html) article and [create an issue](https://github.com/irongut/EDlib/issues) on GitHub.

&nbsp;

&nbsp;

EDlib icon made by [Freepik](https://www.flaticon.com/authors/freepik) from [www.flaticon.com](https://www.flaticon.com/).
