using System;
using System.Collections.Generic;
using TomiSoft.YoutubeDownloader.VideoDescriptionAnalyzer;

namespace TomiSoft.YoutubeDownloader.Tests.VideoDescriptionAnalyzer.Samples {
	class TimestampIsWrappedWithSquareParenthesesSample : IChapterExtractTestSample {
		public string Description => @"📸 Visualizer and mix made by us (with After Effects and Ableton) |
Mix showcasing the best tracks by SWARM, a dubstep producer. There are a lot of songs based on Orchestral Dubstep, Dark Dubstep, Mid Tempo and Electro.

The productions by SWARM are mainly focused on a dark atmosphere, and adding a lot of orchestral or even metal elements to his songs, the latter by featuring some vocalist such as Man Ov God for scremo metal-like vocals, and Dani King with a more calm and melodic approach.

He made a lot of success with some of his productions such as Take Me To Hell, one of his most impressive track ever, filled of awesome melodies and well-executed dark Mid-Tempo drops. Alpha & Omega and Devil's At Your Door are some other good examples of big tunes from him.
SWARM also reinforced some of his tracks with collab by more underground names such as Caster, pretty known as an orchestral deathstep producer, or even Hairitage for a more trap / hybrid trap approach.

Some of his remixes made a lot of success as well, such as his remix of the classic ""Pulse Of The Maggots"" by Slipknot, and also his remix of ""Nihil"" by the famous rapper Ghostemane.
Our mix will showcase the best of his stuff, and also help you to get into the dark side of electronic music 🖤


➖➖ MIX INFO ➖➖

🤴 Mixed by : Flowxy(Delta Bass's owner)

🎼 Genre : Dubstep / Orchestral Dubstep / Mid Tempo / Electro / Electronic / Metalstep / Psytrance / Darkstep

► Download / Stream : https://soundcloud.com/delta_bass_off...


➖➖ TRACKLIST ➖➖ 

• [0:00] SWARM - Unbreakable (with Julian Dae)
• [3:37] SWARM - Void
• [6:33] SWARM & CASTER - Blood
• [9:09] SWARM - All Hope Is Lost
• [11:55] SWARM - Drag Me Down(with Man Ov God)
• [15:15] SWARM - Fear
• [18:40] SWARM - This Is The End
• [22:40] SWARM - Take Me To Hell
• [26:50] SWARM & Dani King - Heartless
• [30:27] SWARM & ATHRS - Singing To The Sky
• [34:56] Rezz & Laura Brehm - Melancholy(SWARM Remix)
• [38:12] Ghostemane - Nihil(SWARM Remix)
• [42:09] Slipknot - Pulse Of The Maggots(SWARM Remix)
• [46:02] SWARM - Savior
• [49:15] SWARM - The Nothing
• [52:48] SWARM & TINYKVT - Devil At Your Door
• [55:46] SWARM - In My Dreams
• [1:00:38] SWARM - I'll Never See The World (with Brian Lenington)


➖➖ ARTIST ➖➖

😈 SWARM :
• SoundCloud : https://soundcloud.com/swarm-music
• Spotify : https://sptfy.com/1DOk
• YouTube : https://www.youtube.com/channel/UClaF...
• Twitter : https://twitter.com/houseofswarm
• Facebook : https://facebook.com/houseofswarm
• Instagram : https://instagram.com/houseofswarm
• Website : https://houseofswarm.com


➖➖ BACKGROUND ➖➖

ℹ Cover Art for the single : SWARM & Dani King - Heartless
+ Slightly edited by Showxy(light effects...)


➖➖ DELTA BASS ➖➖

💞 Follow Us :
https://linktr.ee/delta_bass

💬 Discord Server :
https://discord.gg/XYqzXC9

🔊 Spotify Playlists :
https://open.spotify.com/user/ki1ygis...


➖➖ SUBMISSION ➖➖

📡 Do you want to be released on our music label ? Submit here :
https://forms.gle/AnPajmUUckw3VEN3A


➖➖ COPYRIGHT ISSUE ➖➖

🙋 If you own the song(or background or other product) showcased in the current video, and you want the video to be removed, then mail us and we will remove it within 24h.
Mail : shoxmusic2020 @gmail.com


#Dubstep #DubstepMix #OrchestralDubstep #SWARM #DeltaBass";

		public double Duration => new TimeSpan(01, 05, 48).TotalSeconds;
		public bool ShouldSucceed => true;
		public IReadOnlyList<Chapter> ExpectedOutput => new List<Chapter>() {
			new Chapter(new TimeSpan(0, 00, 00), new TimeSpan(0, 03, 37), "SWARM - Unbreakable (with Julian Dae)"),
			new Chapter(new TimeSpan(0, 03, 37), new TimeSpan(0, 06, 33), "SWARM - Void"),
			new Chapter(new TimeSpan(0, 06, 33), new TimeSpan(0, 09, 09), "SWARM & CASTER - Blood"),
			new Chapter(new TimeSpan(0, 09, 09), new TimeSpan(0, 11, 55), "SWARM - All Hope Is Lost"),
			new Chapter(new TimeSpan(0, 11, 55), new TimeSpan(0, 15, 15), "SWARM - Drag Me Down(with Man Ov God)"),
			new Chapter(new TimeSpan(0, 15, 15), new TimeSpan(0, 18, 40), "SWARM - Fear"),
			new Chapter(new TimeSpan(0, 18, 40), new TimeSpan(0, 22, 40), "SWARM - This Is The End"),
			new Chapter(new TimeSpan(0, 22, 40), new TimeSpan(0, 26, 50), "SWARM - Take Me To Hell"),
			new Chapter(new TimeSpan(0, 26, 50), new TimeSpan(0, 30, 27), "SWARM & Dani King - Heartless"),
			new Chapter(new TimeSpan(0, 30, 27), new TimeSpan(0, 34, 56), "SWARM & ATHRS - Singing To The Sky"),
			new Chapter(new TimeSpan(0, 34, 56), new TimeSpan(0, 38, 12), "Rezz & Laura Brehm - Melancholy(SWARM Remix)"),
			new Chapter(new TimeSpan(0, 38, 12), new TimeSpan(0, 42, 09), "Ghostemane - Nihil(SWARM Remix)"),
			new Chapter(new TimeSpan(0, 42, 09), new TimeSpan(0, 46, 02), "Slipknot - Pulse Of The Maggots(SWARM Remix)"),
			new Chapter(new TimeSpan(0, 46, 02), new TimeSpan(0, 49, 15), "SWARM - Savior"),
			new Chapter(new TimeSpan(0, 49, 15), new TimeSpan(0, 52, 48), "SWARM - The Nothing"),
			new Chapter(new TimeSpan(0, 52, 48), new TimeSpan(0, 55, 46), "SWARM & TINYKVT - Devil At Your Door"),
			new Chapter(new TimeSpan(0, 55, 46), new TimeSpan(1, 00, 38), "SWARM - In My Dreams"),
			new Chapter(new TimeSpan(1, 00, 38), new TimeSpan(1, 05, 48), "SWARM - I'll Never See The World (with Brian Lenington)"),
		};
	}
}
