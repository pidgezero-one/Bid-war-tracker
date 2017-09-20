# Bid-war-tracker
Looks for a string in a streamlabs donation or cheer event and accumulates all donations that match.

Created for https://twitch.tv/Chrescendo

Opens a dashboard where you are prompted to input a streamlabs and twitch authtoken (do not broadcast the window that generates it). Then you can launch a connector to Streamlabs that checks for donations every 10 seconds, and a connector to Twitch that receives cheer events in realtime.

Cheer events that happen while NOT connected cannot be recorded, Twitch does not have a REST API for this yet.

All donations are converted to USD, and all money is converted to points. 1 bit = 1 point, 1 cent = 1 point.

You can have as many bid war options as you like.

If you aren't a partner, the cheer window will open and just have an error. This won't affect the execution of the program as far as streamlabs donations are concerned.

Each option you input creates a text file that states "string: points" (i.e. "#TeamLeft: 1000"). You can use text file OBS sources to put these in your stream layout, and they update automatically.

Current build is here: https://www.mediafire.com/file/kwtwo86v31z8cn6/release9.zip

Right now messages included with a new subscription are not detected, that will be updated in next release (i wrote this before that was a thing).