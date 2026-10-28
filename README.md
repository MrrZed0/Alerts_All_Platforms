# Streamer.bot Multi-Platform Alert Studio Pro
![https://github.com/MrrZed0/Alerts_All_Platforms/blob/main/Preview.png?raw=true](https://github.com/MrrZed0/Alerts_All_Platforms/blob/main/Preview.png?raw=true)

# Streamer.bot Multi-Platform Alert Studio Pro

A modular, zero-dependency alert overlay and visual customizer studio for streamers broadcasting across **Twitch**, **Kick**, **YouTube**, **Streamlabs**, and **StreamElements**.

This system pairs a local HTML5/Tailwind/Web Audio customizer studio with native Streamer.bot C# broadcast scripts, giving you **100+ animation entrance/exit effects**, **100+ Web Audio synth chimes**, customizable layouts, platform-adaptive shoutouts, and custom artwork background replacement.

---

## 🚀 How To Use

1. **Download the files** (zipped or individual source files).
2. **Download and install [Streamer.bot](https://streamer.bot/)**.
3. Open `Import_Code.txt` from the Streamer.bot import code folder and copy the contents.
4. In Streamer.bot, click on **Import**, paste the code, and click **OK**.
5. Open `customizer.html` inside your web browser, choose your settings, layout, colors, and effects live.
6. Click **Export / Download alerts.html** and place it in your local streaming asset folder, replacing any existing `overlay.html` / `alerts.html` file.
7. Open **OBS Studio** or **Streamlabs**, add a **Browser Source**, select your local `alerts.html` file, set the dimensions to **1920x1080**, and make sure to check **Control audio via OBS**.

---

## 🎨 Features & Architecture

* **Multi-Platform Support**: Native hooks and styling for Twitch, Kick, YouTube, Streamlabs, and StreamElements.
* **100+ Entrance & Exit Animations**: Fully customizable speed, triggers, and sequencing.
* **Modular Layouts**: Supports classic horizontal banners, split panels, slim tickers, neon outlines, and custom image frames.
* **Dynamic Audio Engine**: Choose from built-in Web Audio synth presets or hook up custom sound effects (`.mp3` / `.wav`).
* **WebSocket Integrated**: Real-time communication directly with Streamer.bot servers.

---

## 🛠️ File Structure

* `customizer.html`: The visual builder interface to customize and test your alerts live.
* `overlay.html`: The production-ready standalone output file for your OBS Browser Source.
* `Import_Code.txt`: Core Streamer.bot C# action script for broadcasting triggers.
