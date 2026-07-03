# Discord Server Status Bot

A lightweight Discord bot built with **Discord.Net** & **.Net8.0** that automatically updates its Discord activity based on your game server's status.

Instead of hardcoding values, the bot now uses a simple `config.json` file for configuration.

## Features

- 🎮 Live player count display
- 👥 Joining player counter
- 🎉 Event detection
- 💬 Rotating online messages when the server population is low
- 🔴 Offline detection
- ⚙️ Easy configuration via `config.json`
- 🚀 Automatic status updates every minute
- 🪶 Lightweight and easy to host

---

## Example Statuses

Busy server

```
Watching (58/150) Players
```

Players joining

```
Watching (58/150) Players ⇌ Joining(4)
```

Active event

```
Watching The Event’s On — Hop On The Bandwagon!
```

Low population

```
Watching Server is online, enjoy your game!
```

Offline

```
Watching Offline
```

---

# Requirements

- .NET 8.0 or newer
- Discord Bot Token
- A server status API
- An event API (optional but recommended)

---

# Installation

Clone the repository:

```bash
git clone https://github.com/DarkGameSTD/DiscordNet.git
```

Navigate into the project:

```bash
cd DiscordNet
```

Restore dependencies:

```bash
dotnet restore
```

Build:

```bash
dotnet build
```

Run:

```bash
dotnet run
```

---

# Configuration

On the first launch, the bot automatically creates a `config.json` file if one does not already exist.

Example:

```json
{
  "token": "apitoken",
  "eventapi": "http://your-server:2570/",
  "primaryapi": "http://your-server:2569/"
}
```

Update the values before running the bot.

| Property | Description |
|----------|-------------|
| `token` | Your Discord bot token |
| `primaryapi` | API returning the current server status |
| `eventapi` | API used to determine whether an event is currently active |

If the bot detects that the token is still `"apitoken"`, it will stop and prompt you to edit the configuration.

---

# API Format

The **Primary API** should return JSON similar to:

```json
{
    "CurrentPlayers": 42,
    "MaxPlayers": 150,
    "JoiningPlayers": 3
}
```

### Fields

| Field | Description |
|-------|-------------|
| `CurrentPlayers` | Number of connected players |
| `MaxPlayers` | Maximum server capacity |
| `JoiningPlayers` | Number of players currently joining |

---

# Event API

The Event API is very simple.

If the endpoint returns **HTTP 200 (OK)**, the bot assumes an event is active and displays:

```
Watching The Event’s On — Hop On The Bandwagon!
```

If the request fails or returns an error, normal player-count status is shown instead.

---

# How It Works

Every minute the bot:

1. Checks whether an event is active.
2. Requests the server status.
3. Chooses the appropriate Discord activity:
   - Event message
   - Player count
   - Rotating online message (if fewer than 10 players are online)
   - Offline
4. Updates the bot's Discord activity.

---

# Dependencies

- Discord.Net
- Newtonsoft.Json

---

# Running as a Service

The application is designed to run continuously and works well with:

- Linux systemd
- Windows Task Scheduler
- Windows Service wrappers
- Docker (future support)

---

# Security

Never commit your real `config.json` containing your Discord bot token.

Instead:

- Add `config.json` to `.gitignore`
- Commit a sample configuration file instead (for example, `config.example.json`)

Example `.gitignore`:

```gitignore
config.json
```

---

# Roadmap

- [ ] Configurable update interval
- [ ] Multiple server monitoring
- [ ] Rich Presence support
- [ ] Docker image
- [ ] Better logging
- [ ] Slash command support
- [ ] Custom low-player messages via configuration
- [ ] Automatic reconnect handling

---

# License

This project is licensed under the MIT License.

---

# Contributing

Contributions, bug reports, and feature requests are welcome.

If you find this project useful, consider giving it a ⭐ on GitHub!
