
using System;
using Newtonsoft.Json;

// Streamer.bot -> OBS alert broadcaster
// Attach this one C# sub-action to ANY trigger: Twitch, Kick, YouTube, Streamlabs, StreamElements.
public class CPHInline
{
    // Returns the first non-empty argument from a list of possible names
    string Arg(params string[] names)
    {
        foreach (string n in names)
            if (CPH.TryGetArg(n, out object v) && v != null && !string.IsNullOrWhiteSpace(v.ToString()))
                return v.ToString();
        return "";
    }

    public bool Execute()
    {
        string trigger = CPH.GetEventType().ToString().ToLower();

        // 1. Platform (from the trigger name)
        string platform = trigger.Contains("youtube") ? "youtube"
                        : trigger.Contains("kick") ? "kick"
                        : trigger.Contains("streamelements") ? "streamelements"
                        : trigger.Contains("streamlabs") ? "streamlabs"
                        : "twitch";

        // 2. User + avatar
        string user = Arg("user", "userName", "displayName", "donationFrom", "from", "donor", "name");
        if (string.IsNullOrEmpty(user)) user = "Anonymous";

        string avatar = Arg("targetUserProfileImageUrl", "userProfileImageUrl", "profileImageUrl", "avatar", "userAvatar");
        if (string.IsNullOrEmpty(avatar) && platform == "twitch" && user != "Anonymous")
        {
            try
            {
                var info = CPH.TwitchGetExtendedUserInfoByLogin(user);
                if (info != null && !string.IsNullOrEmpty(info.ProfileImageUrl)) avatar = info.ProfileImageUrl;
            }
            catch { }
        }

        // 3. Alert type (order matters: most specific first)
        string type;
        if (trigger.Contains("supersticker")) type = "supersticker";
        else if (trigger.Contains("superchat")) type = "superchat";
        else if (trigger.Contains("milestone")) type = "milestone";
        else if (trigger.Contains("giftmembership") || trigger.Contains("membershipgift")) type = "membergift";
        else if (trigger.Contains("sponsor") || trigger.Contains("member")) type = "member";
        else if (trigger.Contains("merch")) type = "merch";
        else if (trigger.Contains("charity")) type = "charity";
        else if (trigger.Contains("donation") || trigger.Contains("tip")) type = "tip";
        else if (platform == "youtube" && trigger.Contains("subscriber")) type = "follow";
        else if (trigger.Contains("raid")) type = "raid";
        else if (trigger.Contains("host")) type = "host";
        else if (trigger.Contains("gift")) type = "giftsub";
        else if (trigger.Contains("resub")) type = "resub";
        else if (trigger.Contains("sub")) type = "sub";
        else if (trigger.Contains("cheer") || trigger.Contains("kicks") || trigger.Contains("bits")) type = "cheer";
        else type = "follow";

        // 4. Details (argument names differ per integration - check the trigger's variables in Streamer.bot
        //    and add names to these lists if one of your fields comes through empty)
        string amount    = Arg("formattedAmount", "amount", "bits", "kicks", "donationAmount");
        string currency  = Arg("currency", "currencyCode");
        string message   = Arg("message", "donationMessage", "rawInput");
        string months    = Arg("cumulative", "cumulativeMonths", "months", "memberMonths");
        string count     = Arg("total", "giftCount", "gifts", "count", "membershipCount");
        string viewers   = Arg("viewers", "viewerCount");
        string recipient = Arg("recipientUser", "recipientUserName", "recipient");
        if (string.IsNullOrEmpty(viewers)) viewers = "1";
        if (string.IsNullOrEmpty(recipient)) recipient = "the community";

        string title = Arg("alertTitle");
        if (string.IsNullOrEmpty(title) && platform == "youtube" && type == "follow") title = "NEW SUBSCRIBER";

        // 5. Optional per-action overrides (set these as arguments on the action)
        var payload = new
        {
            alert = true,
            platform = platform,
            type = type,
            title = title,
            user = user,
            avatar = avatar,
            detail = "",
            useTemplate = true,
            recipient = recipient,
            viewers = viewers,
            months = months,
            amount = amount,
            currency = currency,
            message = message,
            count = count,
            color = Arg("alertColor"),
            inEffect = Arg("inEffect"),
            outEffect = Arg("outEffect"),
            sound = Arg("alertSound")
        };

        CPH.WebsocketBroadcastJson(JsonConvert.SerializeObject(payload));
        return true;
    }
}
