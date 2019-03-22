# HyparDrive

## Introduction
HyparDrive is the drive software for the Hypar installation made by Team IGNITE for GLOW Eindhoven 2019. This software is created by the IGNITE programming team, with Lead Programmer Joris Lodewijks. A [development vlog](https://www.youtube.com/channel/UC_aMt1voKEiOnQGPngewUEA) showing concepting and building of the system is uploaded.

## Copyright and use
This software doesn't have a license and it is thus not allowed to copy or use the source code for private and/or commercial use. However, private use can be requested. Send a mail to info@jorislodewijks.nl and ask nicely to use (parts of) the source code.

## Structure
### 1st Iteration Structure
The first iteration structure depends on an external software, called ELM (ENNTECH LED MAPPER). This software uses a bitmap texture with overlayed led strip mapping, to send the pixel data via Art-net. This meant cross application communication and this is not what I wanted for our flagship software project.
![Image of HyperDrive Structure](https://github.com/ScrambledFox/HyparDrive/blob/master/Images/HyparDrive.PNG)

### Dependencies
The structure of the software is dependent on the ELM software from ENNTECH. For integrating interaction and music outputting, speakers and interaction modules will need to be connected.

### Installation Controller
The Installation Controller is the main part of the software. This static class connects every part of the system and makes all the decisions. Other classes give or receive requests from this class.

## Contact
Team IGNITE: TU/e Student Team at Innovation Space (Matrix)
> [Website](http://www.teamignite.nl/) |
> [LinkedIn](https://www.linkedin.com/company/team-ignite/)

Lead Programmer: Joris Lodewijks (Industrial Design)
> [Portfolio](http://lodewijks.design/) |
> [Development Vlog](https://www.youtube.com/channel/UC_aMt1voKEiOnQGPngewUEA) |
> [LinkedIn](https://www.linkedin.com/in/jorislodewijks/) |
> [Instagram](https://www.instagram.com/jorislodewijks/) |
> contact@jorislodewijks.nl

© Team Ignite - Joris Lodewijks - 2019
