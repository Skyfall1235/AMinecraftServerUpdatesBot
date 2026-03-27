# A Simple Minecraft Updates Bot
The name is a bit long, but the premise is simple.
A robust, containerized Discord bot built with **.NET 8** and **C#** to monitor Minecraft server status and host availability. Designed for home-lab deployment on Docker or in my case, on **CasaOS** in a homelab environment.

> [!TIP]
> This bot uses **MineStat** for accurate server querying and supports asynchronous slash commands for a modern Discord experience.

---

## Key Features

* **Live Status:** Use `/status` to fetch live player counts, server version, with easy expansion for MOTD.
* **Host Diagnostics:** Use `/pinghost` to verify if the physical machine hosting the server is reachable.
* **Slash Command Configuration:** Use `/setup` to dynamically update the target IP and Port without restarting the bot.
* **Cloud-Ready Logging:** Structured console logging for easy debugging via Docker or CasaOS.

---

## Tech Stack

* **Runtime:** .NET 8.0 (Long Term Support)
* **Language:** C#
* **Libraries:** `Discord.Net` (API Wrapper)
    * `MineStat` (Minecraft Querying)
    * `Microsoft.Extensions.Configuration` (Environment & Secret Management)
* **Containerization:** Docker (Multi-stage builds)
* **Orchestration:** CasaOS / Docker Compose

---

## Getting Started

### Local Development (Visual Studio 2022)

This project uses a `.env` file to manage sensitive tokens.

1.  **Create a `.env` file** in the project root:
    ```env
    DiscordToken=YOUR_BOT_TOKEN_HERE
    ```
2.  **Verify .gitignore:** Ensure `.env` is listed so your token is never pushed to GitHub.
3.  **Run:** Press `F5` to start debugging.

> [!WARNING]
> If the bot fails to start, ensure the `.env` file's properties are set to **"Copy to Output Directory: Copy if newer"** in Visual Studio.

### Deployment on CasaOS / Docker

This project uses a **Multi-Stage Dockerfile** to ensure the production image is as small as possible and contains only the necessary runtimes.

1.  **Build the Image:**
    ```bash
    docker build -t mc-updates-bot:latest .
    ```
2.  **Export/Import to CasaOS:**
    ```bash
    docker save -o mc-bot.tar mc-updates-bot:latest
    # Move to CasaOS and run:
    docker load -i mc-bot.tar
    ```
3.  **Configure in CasaOS UI:**
    Install as a **Custom App** and add the following Environment Variable:
    * **Key:** `DiscordToken`
    * **Value:** `your_discord_bot_token`

---

## Project Structure

```text
├── Modules/               # Discord Slash Command logic
├── Services/              # Background monitoring services
├── Program.cs             # Dependency Injection & Bot initialization
├── Dockerfile             # Multi-stage production build recipe
└── .dockerignore          # Keeps bin/obj folders out of the image
