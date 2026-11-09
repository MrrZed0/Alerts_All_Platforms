ALERT STUDIO — Streamer.bot alerts for OBS
Twitch · Kick · YouTube · TikTok · Streamlabs · StreamElements

Two files:
  customizer.html  the studio you open in a browser to design your alerts
  alerts.html      the file you point OBS at (export it from the studio)

Nothing about how the overlay talks to Streamer.bot has changed. If you already
have it working, your existing action and C# script keep working as they are.


HOW IT WORKS
------------
1. Something happens on stream (a follow, a gift, a raid).
2. Your Streamer.bot action runs the C# script.
3. The script works out the platform and the event type, then broadcasts a
   single JSON payload over the WebSocket.
4. alerts.html picks it up and shows the alert in that platform's colour:
   purple for Twitch, green for Kick, red for YouTube, cyan for TikTok.

Twitch, YouTube and Kick talk to Streamer.bot directly. TikTok needs a bridge —
see the TikTok section.


SETUP
-----
1. In Streamer.bot: Servers/Clients -> WebSocket Server. Start it on
   127.0.0.1:8080 and tick auto-start. That matches the URL in the studio.
2. Create an action called "Alerts". Add Core -> C# -> Execute C# Code and
   paste in the script from the studio's "C# script" button. If it will not
   compile, add Newtonsoft.Json.dll on the References tab, then Compile.
3. Attach that action to whichever triggers you want: Twitch Follow, Kick
   Subscription, YouTube Super Chat, and so on. One action handles all of
   them — the script reads the trigger name to work out the platform and the
   alert type. (TikTok is different; see below.)
4. In the studio, design the alert, click "Download alerts.html", and point an
   OBS Browser Source at that file (Local file, 1920 x 1080).


TIKTOK
------
Streamer.bot has no TikTok support. Its platform list is Twitch, YouTube and
Kick only, and there is no TikTok integration either. TikTok has no public API
for live events, which is why no amount of searching Streamer.bot's settings
will turn one up.

So TikTok has to come in through a bridge: a separate program that watches your
TikTok live and tells Streamer.bot about it. TikFinity is the usual one:

    https://tikfinity.zerody.one/

The bridge runs one of your Streamer.bot actions. The action runs the same C#
script as everything else, and the alert reaches the overlay the same way.


TIKFINITY SETUP
---------------
1. Streamer.bot: Servers/Clients -> WebSocket Server, running on
   127.0.0.1:8080. (Same server the overlay uses.)
2. TikFinity: Setup -> Streamer.bot Connection -> Test Connection.
3. TikFinity: Actions & Events -> Create Action -> "Streamer.bot Action" ->
   choose the Streamer.bot action to run -> Save -> assign it to a TikTok event.

Gifts and likes need nothing else. TikFinity sends giftName, coins,
repeatCount and likeCount, and no other platform uses those names, so the
script recognises TikTok on its own and picks the right alert.

Shares, follows and subs have no fields that give them away, so for those make
one small Streamer.bot action per event. Each one holds three sub-actions:

    Core -> Arguments -> Set Argument     platform   = tiktok
    Core -> Arguments -> Set Argument     alertType  = tiktokshare
    Core -> C# -> Execute C# Code         (the alert script)

Then point that TikFinity event at that action. Change only alertType per
action:

    tiktokgift    gift received
    tiktoklike    likes
    tiktokshare   shared the stream
    follow        new follower
    sub           new subscriber

Setting platform and alertType explicitly always wins, on every platform. If
you would rather be explicit about gifts and likes too, set them the same way
and the auto-detection stays out of the way.

If an alert shows up purple as a Twitch follow, platform and alertType did not
reach the script — check the spelling of the argument names, and check the
action history in Streamer.bot to see what actually arrived.

Any other TikTok bridge works the same way: if it can run a Streamer.bot
action, give that action the two arguments.


WHAT THE SCRIPT READS FROM TIKFINITY
------------------------------------
These are picked up automatically if the bridge sends them, so you only need to
map them yourself if your setup names them differently:

    username, nickname          the name on the alert
    profilePicturUrl            the avatar (TikFinity's own spelling; the
                                correct spelling is accepted too)
    giftName                    shown as the gift
    coins, diamondCount         the coin amount
    repeatCount                 how many were sent
    likeCount, totalLikeCount   the like count
    subMonth                    months subscribed


SHOUTOUTS
---------
1. Add a second action, "Shoutout", with the script from the "Shoutout C#"
   button in the studio.
2. Add a command !so, enable it for each platform you stream on, set
   permissions to Moderator and Broadcaster, and attach that action.
3. Someone types !so @shroud and the overlay shows the shoutout in the colour
   of whichever platform the command was typed on.

Arguments you must set: none. Streamer.bot creates targetUser and input0
automatically when the command runs.

Optional arguments (Set Argument before the C# sub-action):
  soChatMessage   custom chat text
  nativeShoutout  false to skip Twitch's built-in /shoutout
  alertTitle, alertColor, inEffect, outEffect, alertSound
                  per-action overrides. Leave alertColor empty to keep the
                  platform colour.

Getting the target's game and avatar:
  On Twitch the avatar is looked up automatically; the game is not. Add a
  Twitch -> Get User Info for Target sub-action before the C# one. The script
  reads targetUserGame, targetGame, game or gameName. Check the action history
  for the real name if the alert says "something awesome".
  On YouTube, TikTok and Kick the avatar is not looked up, so you get the
  initials fallback unless you pass targetUserProfileImageUrl.


FALLING ITEMS
-------------
Bits and cheers drop your own GIFs: 1.gif, 100.gif, 1000.gif, 5000.gif and
10000.gif. They are built into the exported file, so you do not need to keep
the folder around. Every tier up to the amount falls, so 1,000 bits drops
1 + 100 + 1000. Other alert types can drop an emoji instead — turn them on
under the Falling tab.


TESTING
-------
Use the test bank under the preview in the studio. Filter it by platform with
the chips on the left. You can also open alerts.html with ?test=true on the
end to fire a few sample alerts, or right-click the action in Streamer.bot and
use Test Trigger.


NOTES
-----
The studio no longer loads anything from the internet for its own interface,
so it works offline. The alert fonts still come from Google Fonts when you are
online; if you pick a font installed on your PC it works in OBS either way.


MOVING THINGS AROUND
--------------------
Layout tab -> "Nudge the contents". Separate left/right and up/down sliders for
the avatar, the text block and the artwork, plus the gap between the avatar and
the text. This is mainly for lining content up inside your own image.png: turn
on "image.png as the alert box", then nudge until the text sits where the art
has room for it. Click a slider and use the arrow keys for single pixels.
"Recentre" puts everything back.

Nudging does not resize the frame, so the art will not jump around while you
adjust it.


MORE OPTIONS
------------
Card layouts        32, with a search box
Avatar frames       25, with a search box
Avatar rings        18
Platform badges     13 styles including None, 8 positions
Entrance effects    116
Exit effects        110
Idle motions        120
Progress bars       120
Sounds              105

To remove the platform badge, set Platform Badge Style to "None (hide the
badge)". The "Show platform mini badge" tickbox does the same thing.

Falling items: each alert type has a "None (nothing falls)" entry at the top of
its list. Picking it silences that type and unticks it.
