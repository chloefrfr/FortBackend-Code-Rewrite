# FortBackend

> ## FortBackend is a Universal Fortnite Private Server written in C#.

## Key Features

- **HTTP/HTTPS Support**
- **Season Support**: Compatible with Seasons 3 to 15, including a season shop for Season 1.
- **Quests**: 1:1 Quests, with varying supprt across seasons.
- **Arena UI/Playlists**: Supported on seasons 8 - 23 (Note: Arena is very unfinished)
- **Ban Assist**

## Experimental Features

- You may use experimental/unfinished features in [FortLibrary/ConfigHelpers/FortConfig.cs](https://github.com/zinx28/FortBackend/blob/main/FortLibrary/ConfigHelpers/FortConfig.cs)

## Setup Instructions

### Prerequisites

- **Visual Studio**: Required for building the project.

### Installation

Details are available [here](https://github.com/zinx28/FortServer/blob/main/Setup.md).

## Discord Bot

- Make sure you set up the bot in the configs, and the /test command works
- Discord Bot Disabled On "DEVELOPMENT"
- ActivityType max is 5 (check in config cs file!! for more details)
- More info to setup and commands [here](https://github.com/zinx28/FortBackend/blob/main/DiscordBotSetup.md)

## Admin Panel (WIP)

I've created an admin page to make it easier to manage JSON configs.

### About

- The admin account is essential and cannot be deleted or edited.
- Admins can grant other "FortBackend" users admin or moderator roles.
- Admins have the ability to edit user roles, but moderators cannot edit users or add new ones (work in progress).
- INI Manager on the dashboard is a mess and i will redo it in the future

### Setup and Login

- Start the FortBackend application.
- Access the login page [here](http://127.0.0.1:2222/login) (right-click to copy the link).

3. Use the default credentials:
   - **Email**: Admin@gmail.com
   - **Password**: AdminPassword123

- Upon first login, you will be prompted to change your email and password.
