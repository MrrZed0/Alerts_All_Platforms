# 🚀 Ultimate Streamer.bot Multi-Platform Alert Studio

![Streamer.bot](https://img.shields.io/badge/Streamer.bot-v0.2.2+-purple?style=for-the-badge&logo=twitch)
![OBS Studio](https://img.shields.io/badge/OBS_Studio-Browser_Source-black?style=for-the-badge&logo=obsstudio)
![Platforms](https://img.shields.io/badge/Supported-Twitch_|_Kick_|_YouTube_|_SL_|_SE-success?style=for-the-badge)

![https://github.com/MrrZed0/Alerts_All_Platforms/blob/main/Preview.png](https://github.com/MrrZed0/Alerts_All_Platforms/blob/main/Preview.png?raw=true)

A fully customizable, zero-dependency, single-file HTML OBS overlay for Streamer.bot. Visually design your stream alerts with **100+ Entrance/Exit animations**, **100+ Idle/Progress FX**, and **100+ Web Audio Synth styles** directly in your browser, then export a standalone HTML file ready for OBS.

## ✨ Features

* **🌍 Multi-Platform Ready:** Supports Twitch, Kick, YouTube (Subs, Members, SuperChats), Streamlabs (Donations), and StreamElements (Tips).
* **🎨 100,000+ Combinations:** Procedurally generated CSS engine providing 100+ In FX, 100+ Out FX, 100+ Idle FX, and 100+ Progress Bar FX.
* **🖼️ Custom Background Images:** Drop an `image.png` next to your HTML file to instantly map your text and avatars onto a custom streaming HUD or banner frame.
* **📣 Intelligent Shoutouts:** Dedicated Shoutout integration that automatically detects if the raided/shouted-out user is from Twitch, Kick, or YouTube, and changes the alert color to match their platform!
* **🎹 Built-In Audio Synth:** No MP3s required! Generates retro, cyberpunk, and classic alert chimes dynamically via the Web Audio API. (Custom MP3s are also supported).

---

## 🛠️ Installation & Setup

### Step 1: Customize Your Overlay
1. Open `alert_customizer.html` in any modern web browser.
2. Tweak your colors, layout, fonts, and animations.
3. Click the **Download alerts.html** button to export your final, lightweight OBS overlay.
4. Move the downloaded `alerts.html` (and your optional `image.png` / custom `.mp3` files) into a dedicated folder on your PC.

### Step 2: Add to OBS Studio
1. Add a new **Browser Source** in OBS.
2. Check **Local File** and point it to your exported `alerts.html`.
3. Set **Width** to `1920` and **Height** to `1080`.
4. Check *Refresh browser when scene becomes active*.

### Step 3: Streamer.bot WebSocket Configuration
1. In Streamer.bot, go to `Servers/Clients` -> `WebSocket Server`.
2. Ensure the server is **started** on `ws://127.0.0.1:8080/`.

---

## 💻 Streamer.bot C# Actions

To trigger these alerts, you need to create two C# Actions in Streamer.bot. 

### Action 1: General Multi-Platform Alerts
Create a new Action (e.g., "OBS Alerts Trigger"). Add a **Core -> C# -> Execute C# Code** sub-action and paste the following:

```csharp
using System;
using Newtonsoft.Json;

public class CPHInline
{
    public bool Execute()
    {
        // 1. Grab Username
        string user = CPH.TryGetArg("user", out string u) ? u :
                      CPH.TryGetArg("userName", out string un) ? un :
                      CPH.TryGetArg("name", out string n) ? n : "Anonymous";

        // 2. Determine Platform
        string platform = CPH.TryGetArg("userType", out string ut) ? ut.ToLower() : "twitch";
        string source = CPH.TryGetArg("eventSource", out string src) ? src.ToLower() : "";
        string triggerName = CPH.GetEventType().ToString().ToLower();

        if (triggerName.Contains("youtube") || source == "youtube") platform = "youtube";
        else if (triggerName.Contains("streamlabs") || source == "streamlabs") platform = "streamlabs";
        else if (triggerName.Contains("streamelements") || source == "streamelements") platform = "streamelements";
        else if (triggerName.Contains("kick")) platform = "kick";

        // 3. Grab Avatar URL
        string avatar = "";
        string[] avatarArgs = { "targetUserProfileImageUrl", "userProfileImageUrl", "profileImageUrl", "avatar", "userAvatar" };
        foreach (string argName in avatarArgs)
        {
            if (CPH.TryGetArg(argName, out string foundAvatar) && !string.IsNullOrEmpty(foundAvatar))
            {
                avatar = foundAvatar; break;
            }
        }
        
        if (string.IsNullOrEmpty(avatar) && platform.Contains("twitch") && user != "Anonymous")
        {
            try {
                var userInfo = CPH.TwitchGetExtendedUserInfoByLogin(user);
                if (userInfo != null && !string.IsNullOrEmpty(userInfo.ProfileImageUrl))
                    avatar = userInfo.ProfileImageUrl;
            } catch { }
        }

        // 4. Determine Alert Type & Details
        string alertType = "follow";
        string recipient = CPH.TryGetArg("recipientUser", out string r) ? r : "the community";
        string viewers = CPH.TryGetArg("viewers", out object v) ? v.ToString() : "1";
        string months = CPH.TryGetArg("cumulative", out object m) ? m.ToString() : "";
        string amount = "";

        if (triggerName.Contains("raid")) alertType = "raid";
        else if (triggerName.Contains("gift")) alertType = "giftsub";
        else if (triggerName.Contains("resub") || triggerName.Contains("member")) alertType = "resub";
        else if (triggerName.Contains("sub")) alertType = "sub";
        else if (triggerName.Contains("cheer") || triggerName.Contains("kicks"))
        {
            alertType = "cheer";
            amount = CPH.TryGetArg("bits", out object b) ? b.ToString() : CPH.TryGetArg("amount", out object k) ? k.ToString() : "";
        }
        else if (triggerName.Contains("superchat") || triggerName.Contains("donation") || triggerName.Contains("tip"))
        {
            alertType = triggerName.Contains("superchat") ? "superchat" : "tip";
            amount = CPH.TryGetArg("amount", out object a) ? a.ToString() : CPH.TryGetArg("formattedAmount", out object fa) ? fa.ToString() : "";
        }

        // 5. Broadcast to OBS 
        var payload = new
        {
            alert = true, platform = platform, type = alertType, user = user, avatar = avatar, useTemplate = true,
            recipient = recipient, viewers = viewers, months = months, amount = amount,
            
            // Optional Action Overrides
            color = CPH.TryGetArg("alertColor", out string col) ? col : "",
            inEffect = CPH.TryGetArg("inEffect", out string inEff) ? inEff : "",
            outEffect = CPH.TryGetArg("outEffect", out string outEff) ? outEff : ""
        };

        CPH.WebsocketBroadcastJson(JsonConvert.SerializeObject(payload));
        return true;
    }
}
```

*Assign this action to your Twitch Follow, Twitch Sub, Kick Sub, YouTube SuperChat, etc., triggers!*

---

### Action 2: Dynamic Shoutouts (`!so`)
Create an Action called "Shoutout Trigger" and attach it to your `!so` chat command. Paste this C# Code:

```csharp
using System;
using Newtonsoft.Json;

public class CPHInline
{
    public bool Execute()
    {
        string targetUser = CPH.TryGetArg("rawInput", out string ri) ? ri.Replace("@", "").Trim() : "";
        if (string.IsNullOrEmpty(targetUser)) return false;

        string platform = "twitch"; // Default
        string avatar = "";
        string game = "Just Chatting";
        
        try
        {
            var userInfo = CPH.TwitchGetExtendedUserInfoByLogin(targetUser);
            if (userInfo != null)
            {
                targetUser = userInfo.DisplayName;
                avatar = userInfo.ProfileImageUrl;
                if (!string.IsNullOrEmpty(userInfo.Game)) game = userInfo.Game;
            }
        }
        catch { }

        // Broadcast Shoutout
        var payload = new
        {
            alert = true, shoutout = true, platform = platform, type = "shoutout",
            title = "STREAMER SHOUTOUT", user = targetUser, avatar = avatar, game = game, useTemplate = true,
            
            color = CPH.TryGetArg("alertColor", out string col) ? col : "",
            inEffect = CPH.TryGetArg("inEffect", out string inEff) ? inEff : "",
            outEffect = CPH.TryGetArg("outEffect", out string outEff) ? outEff : ""
        };

        CPH.WebsocketBroadcastJson(JsonConvert.SerializeObject(payload));
        return true;
    }
}
```

## 🎛️ Action Overrides
You can completely override the alert studio settings on a *per-action basis* by using the `Set Argument` sub-action in Streamer.bot right before the C# Code executes! 

| Argument Name | Data Type | Example Value | Description |
|---|---|---|---|
| `alertColor` | String | `#FF0055` | Overrides the accent color for this specific alert |
| `inEffect` | String | `genIn_25` | Overrides the entrance animation (get the ID from the studio) |
| `outEffect` | String | `floatUpFade` | Overrides the exit animation |

---
*Created by the Community for Streamer.bot*
