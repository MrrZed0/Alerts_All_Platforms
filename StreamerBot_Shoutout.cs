
using System;
using Newtonsoft.Json;

// Streamer.bot SHOUTOUT script
// Trigger: a command such as !so (enable it for Twitch, YouTube and/or Kick chat). Usage: !so @username
// The overlay colors the alert with the platform the command came from (Twitch purple, YouTube red, Kick green).
// Optional action arguments: soChatMessage (text sent to chat), nativeShoutout (false = skip Twitch's built-in /shoutout),
//                            alertColor, inEffect, outEffect, alertSound
public class CPHInline
{
    string Arg(params string[] names)
    {
        foreach (string n in names)
            if (CPH.TryGetArg(n, out object v) && v != null && !string.IsNullOrWhiteSpace(v.ToString()))
                return v.ToString();
        return "";
    }

    public bool Execute()
    {
        string target = Arg("targetUser", "targetUserName", "input0").TrimStart('@');
        if (string.IsNullOrEmpty(target))
        {
            CPH.LogWarn("[Shoutout] No target user. Use: !so @username");
            return false;
        }

        // Platform the command was typed on
        string source = (Arg("eventSource") + " " + CPH.GetEventType()).ToLower();
        string platform = source.Contains("youtube") ? "youtube" : source.Contains("kick") ? "kick" : "twitch";

        // Target avatar + last game
        string avatar = Arg("targetUserProfileImageUrl", "targetProfileImageUrl");
        string game = Arg("targetUserGame", "targetGame", "game", "gameName");
        if (string.IsNullOrEmpty(game)) game = "something awesome";

        if (platform == "twitch" && string.IsNullOrEmpty(avatar))
        {
            try
            {
                var info = CPH.TwitchGetExtendedUserInfoByLogin(target);
                if (info != null && !string.IsNullOrEmpty(info.ProfileImageUrl)) avatar = info.ProfileImageUrl;
            }
            catch { }
        }

        // Twitch's built-in shoutout (shows the shoutout card in Twitch chat)
        if (platform == "twitch" && Arg("nativeShoutout").ToLower() != "false")
        {
            try { CPH.TwitchSendShoutoutByLogin(target); } catch { }
        }

        // Chat message
        string url = platform == "kick" ? "https://kick.com/" + target
                   : platform == "youtube" ? "https://youtube.com/@" + target
                   : "https://twitch.tv/" + target;
        string chat = Arg("soChatMessage");
        if (string.IsNullOrEmpty(chat))
            chat = "Go show some love to " + target + "! They were last playing " + game + " -> " + url;
        try
        {
            if (platform == "youtube") CPH.SendYouTubeMessage(chat);
            else if (platform == "kick") CPH.SendKickMessage(chat);
            else CPH.SendMessage(chat);
        }
        catch { }

        // Overlay alert (color stays empty on purpose so the overlay uses the platform color)
        var payload = new
        {
            alert = true,
            platform = platform,
            type = "shoutout",
            title = Arg("alertTitle"),
            user = target,
            avatar = avatar,
            detail = "",
            useTemplate = true,
            game = game,
            color = Arg("alertColor"),
            inEffect = Arg("inEffect"),
            outEffect = Arg("outEffect"),
            sound = Arg("alertSound")
        };

        CPH.WebsocketBroadcastJson(JsonConvert.SerializeObject(payload));
        return true;
    }
}
