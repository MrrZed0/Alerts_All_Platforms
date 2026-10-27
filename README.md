# Streamer.bot Multi-Platform Alert Studio Pro
![PreView](https://github.com/MrrZed0/Alerts_All_Platforms/blob/main/Preview.png?raw=true)

# Streamer.bot Multi-Platform Alert Studio Pro

A modular, zero-dependency alert overlay and visual customizer studio for streamers broadcasting across **Twitch**, **Kick**, **YouTube**, **Streamlabs**, and **StreamElements**.

This system pairs a local HTML5/Tailwind/Web Audio customizer studio with native Streamer.bot C# broadcast scripts, giving you 100+ animation entrance/exit effects, 100+ Web Audio synth chimes, customizable layouts, platform-adaptive shoutouts, and custom artwork background replacement.

---

## Table of Contents

* [Features](https://www.google.com/search?q=%23features)
* [Supported Platforms & Events](https://www.google.com/search?q=%23supported-platforms--events)
* [Architecture Overview](https://www.google.com/search?q=%23architecture-overview)
* [Quick Start Installation](https://www.google.com/search?q=%23quick-start-installation)
* [1. Streamer.bot WebSocket Setup](https://www.google.com/search?q=%231-streamerbot-websocket-setup)
* [2. C# Actions Setup](https://www.google.com/search?q=%232-c-actions-setup)
* [3. OBS Studio Browser Source Setup](https://www.google.com/search?q=%233-obs-studio-browser-source-setup)


* [Custom Artwork Background Replacement](https://www.google.com/search?q=%23custom-artwork-background-replacement)
* [Platform-Adaptive Shoutout Engine](https://www.google.com/search?q=%23platform-adaptive-shoutout-engine)
* [WebSocket JSON Payload Specification](https://www.google.com/search?q=%23websocket-json-payload-specification)
* [Repository Structure](https://www.google.com/search?q=%23repository-structure)
* [License](https://www.google.com/search?q=%23license)

---

## Features

* **Multi-Platform Support**: Unified design language across Twitch, Kick, YouTube, Streamlabs, and StreamElements.
* **100+ Entrance (IN) & Exit (OUT) Keyframes**: Smooth cubic-bezier slides, 3D axis flips, glitch disintegration, CRT TV turn-offs, vortex portals, and elastic springs, plus a `🎲 Random Every Alert` selector.
* **100+ Procedural Web Audio Synth Chimes**: Generates 8-bit coins, cyberpunk arpeggios, laser sweeps, and 808 sub-bass impacts in real time via the browser's native Web Audio API—zero MP3 downloads or audio hosting required. Custom MP3 override files are also supported[cite: 1].
* **Custom Artwork Background Replacement**: Swap plain solid backgrounds for an `image.png` asset that fills the alert card with an adjustable darkening scrim, keeping text and user avatars placed on top.
* **12 Dynamic Card Layouts**: Classic left/right orientations, centered esports pills, tactical sci-fi HUDs, cinema lower-thirds, and compact tickers[cite: 1].
* **12 Avatar Frame Shapes**: Circular, rounded squircle, hexagon, octagon, tactical shield, diamond, slant-cut, and cyber-notch borders[cite: 1].
* **Platform-Adaptive Shoutout Engine**: Dedicated shoutout alert handling with automated platform brand colors, chat broadcast messages, and game metadata.
* **Pure Overlay Mode**: Toggle between the full visual Customizer Studio and an OBS-ready transparent overlay using the `?overlay=true` URL parameter[cite: 1].

---

## Supported Platforms & Events

| Platform | Supported Alert Triggers | Default Brand Accent |
| --- | --- | --- |
| **Twitch** | Follows, Subscriptions, Resubs, Gift Subs, Raids, Bits / Cheers[cite: 1] | `#9146FF`[cite: 1] |
| **Kick** | Followers, Subscribers, Resubscriptions, Gifted Subs, Raids, Kicks[cite: 1] | `#53FC18`[cite: 1] |
| **YouTube** | Subscribers, Channel Memberships (Sponsors), Super Chats, Super Stickers | `#FF0000` |
| **Streamlabs** | Tips / Direct Donations, Merch Purchases, Cloudbot Alerts | `#31C3A2` |
| **StreamElements** | Direct Tips, Store Loyalty Redemptions, Merch Orders | `#0084FF` |
| **Cross-Platform** | Streamer Shoutouts (`!so <user>`) | Adaptive |

---

## Architecture Overview

```
 ┌────────────────────────────────────────────────────────┐
 │                      Streamer.bot                      │
 │                                                        │
 │  [Twitch / Kick / YouTube / Streamlabs / SE Triggers]  │
 │                           │                            │
 │                           ▼                            │
 │            [Execute C# Inline Action]                  │
 │                           │                            │
 │                           ▼                            │
 │         CPH.WebsocketBroadcastJson(payload)            │
 └───────────────────────────┬────────────────────────────┘
                             │
                             ▼  ws://127.0.0.1:8080/
 ┌────────────────────────────────────────────────────────┐
 │                    OBS Studio Scene                    │
 │                                                        │
 │   Browser Source: alerts.html?overlay=true             │
 │   ├── Web Audio API (100+ Procedural Synths)           │
 │   ├── Dynamic CSS Keyframe Engine (100+ In/Out)        │
 │   └── Background Artwork Engine (image.png Scrim)      │
 └────────────────────────────────────────────────────────┘

```

---

## Quick Start Installation

### 1. Streamer.bot WebSocket Setup

1. In **Streamer.bot**, navigate to the **Servers/Clients** tab[cite: 1].
2. Select **WebSocket Server**[cite: 1].
3. Ensure the server status is set to **Started** on port `8080` (endpoint: `ws://127.0.0.1:8080/`)[cite: 1].
4. Check **Auto Start** so the server boots with Streamer.bot.

### 2. C# Actions Setup

1. Head to the **Actions** tab in Streamer.bot and create a new Action (e.g., `Alert - Master Broadcast`).
2. Add a sub-action: **Core** ➔ **C# Code** ➔ **Execute C# Code**.
3. Paste the contents of [`StreamerBot_MultiPlatform_Broadcast.cs`](https://www.google.com/search?q=%23streamerbot_multiplatform_broadcastcs) into the editor and click **Save and Compile**.
4. Set up your triggers (Twitch Follow, Kick Sub, YouTube Super Chat, etc.) to call this action[cite: 1, 2].
5. Create a separate Action for shoutouts (e.g., `Command - Shoutout`) bound to your chat trigger `!so` using [`StreamerBot_Shoutout.cs`](https://www.google.com/search?q=%23streamerbot_shoutoutcs).

### 3. OBS Studio Browser Source Setup

1. Download or clone this repository to your local streaming PC.
2. Open `alert_customizer_studio.html` in your web browser to configure your preferred layout, timings, fonts, and colors[cite: 1].
3. Click **Download alerts.html** to export your configuration file[cite: 1].
4. In **OBS Studio**, add a new **Browser Source** to your active scene:
* Check **Local File**[cite: 1].
* **File**: Browse and select your exported `alerts.html` (or `alert_customizer_studio.html?overlay=true`)[cite: 1].
* **Width**: `1920`[cite: 1]
* **Height**: `1080`[cite: 1]
* Check **Shutdown source when not visible** and **Refresh browser when scene becomes active**.



---

## Custom Artwork Background Replacement

Instead of rendering static box graphics as floating thumbnails, this system allows `image.png` to replace the alert container background entirely:

1. Place your transparent or designed graphic named `image.png` into the same folder as `alerts.html`[cite: 1].
2. In the Studio under **Layout & Artwork**, set **Custom Artwork Mode** to:
* **Cover**: Stretches and crops the artwork to fill the bounding box.
* **Contain**: Centers the artwork inside the container while preserving aspect ratio.
* **Blurred**: Creates an ambient background blur behind the content.


3. Adjust the **Background Scrim / Darken** slider (e.g., `65%`) to dim the graphic, ensuring username, avatar, and event text remain legible.

---

## Platform-Adaptive Shoutout Engine

The shoutout engine dynamically recolors the alert and sets the corresponding platform badge depending on where the user broadcasts:

* **Command Syntax**: Type `!so <username>` in your chat.
* **Dynamic Identification**: The C# script evaluates the platform type and sets the accent color to Twitch Purple (`#9146FF`), Kick Green (`#53FC18`), or YouTube Red (`#FF0000`).
* **Data Resolution**: The script queries Streamer.bot's user info API to retrieve the user's avatar image and latest game broadcast[cite: 2].

---

## WebSocket JSON Payload Specification

If you are writing custom actions, bots, or external integrations, broadcast JSON packets matching this schema to `ws://127.0.0.1:8080/`[cite: 1, 2]:

```json
{
  "alert": true,
  "platform": "twitch",
  "type": "follow",
  "title": "NEW FOLLOWER",
  "user": "MrrZed0",
  "avatar": "https://example.com/avatar.png",
  "detail": "Welcome to the stream!",
  "color": "#9146FF",
  "inEffect": "elasticBounce",
  "outEffect": "tvTurnOff",
  "sound": ""
}

```

### Parameter Reference

| Key | Type | Description |
| --- | --- | --- |
| `alert` | `boolean` | Set to `true` to flag this payload as an on-screen alert[cite: 2]. |
| `platform` | `string` | Target service: `"twitch"`, `"kick"`, `"youtube"`, `"streamlabs"`, `"streamelements"`. |
| `type` | `string` | Alert category: `"follow"`, `"sub"`, `"resub"`, `"giftsub"`, `"raid"`, `"cheer"`, `"superchat"`, `"donation"`, `"shoutout"`[cite: 1, 2]. |
| `title` | `string` | Header text (e.g., `"INCOMING RAID"`, `"SUPER CHAT"`)[cite: 1]. |
| `user` | `string` | Primary display name[cite: 2]. |
| `avatar` | `string` | Direct URL to user avatar image; falls back to auto-generated monogram if empty[cite: 1, 2]. |
| `detail` | `string` | Secondary descriptive text or message body[cite: 2]. |
| `color` | `string` | *(Optional)* Hex override for alert accent borders and glows (e.g. `"#FF0055"`)[cite: 1, 2]. |
| `inEffect` | `string` | *(Optional)* Animation keyframe override for alert entrance[cite: 1, 2]. |
| `outEffect` | `string` | *(Optional)* Animation keyframe override for alert exit[cite: 1, 2]. |

---

## Repository Structure

```text
├── alert_customizer_studio.html          # Web-based configuration studio and live preview stage
├── alerts.html                           # OBS-ready standalone overlay export
├── StreamerBot_MultiPlatform_Broadcast.cs # Master C# trigger broadcast script
├── StreamerBot_Shoutout.cs               # Platform-adaptive C# shoutout script
├── image.png                             # Optional custom background replacement artwork
└── README.md                             # Documentation

```

---

## License

Distributed under the MIT License. See `LICENSE` for more information.
