using System;
using System.Collections.Generic;
using TomiSoft.YoutubeDownloader.VideoDescriptionAnalyzer;

namespace TomiSoft.YoutubeDownloader.Tests.VideoDescriptionAnalyzer.Samples {
	internal class TimestampsInFrontAndOnlyStartTimesAreGivenSample : IChapterExtractTestSample {
		public string Description => @"Hello Bats, today I have prepared for you a special fetish-style set to celebrate because I will soon have 50,000 followers. 🔥🔥
I will always be eternally grateful to each of you for all the support provided, I promise that I will continue working to provide you with better quality content.
find my mix in Soundcloud: https://soundcloud.com/yami-spechie

On this special day I have prepared an exhaustive selection of pure fire songs  that will make you dance throughout the night with my incredible dancers and performers.


Please! support this channel with your donation:
https://www.paypal.com/paypalme/yamis...
https://www.instagram.com/yamispechiee/
_
Playlist & Labels:
EBM - TECHNO
00:00:00 - Lera Foer & Gegen Mann - Precious Game [Maxima Culpa]
00:04:57 - Fractions - Body Limit [FLEISCH]
00:09:56 - Arnaud Rebotini -  What you want me to do[Mannequin Records]
00:15:27 - Puritan - Whitesnake [Murder]
00:21:47 - HOF - The Return [Fe Chrome]
00:25:47 - Rougue - Nuclear [Detriti Records]
00:28:50 - Lbeeze - Boogyman [Idlestates Recordings]

DARKWAVE - POSTPUNK - COLDWAVE
00:32:25 - Mulfti - Fields of Passion [nothingisrealrecords]
00:38:28 - Zack Zack Zack - Butun [Rennweg Records]
00:41:18 - 11 - T.G.T.B. - Derelict [Detriti Records]
00:45:25 - Balvanera - Medium [DKA Records]
00:48:55 - Minuit Machine - Don't Run From The Fire [Synth Religion]

EBM - TECHNO
00:53:38 - Mind/Matter - Feture [ ]
00:57:30 - Lera Foer - Open Eyes [Oberwave Records]
01:03:02 -  Rougue - Potential Danger [Detriti Records]
01:07:20 - Ton Globiter - Hysteria Malmeria [Slow Motion Records]
01:11:15 - Garlen - Black out dats [ ]

NEW BEAT
01:14:14 - Time Modern - Mantel Der nacho (Alien Elements) [BOY Records ]
01:17:49 - Bit-Max - Dance (Just Dance for me) [Discomagic Records]

ITALO DISCO - DARK DISCO - NU DISCO
01:20:40 - Ton Globiter - Kaunas in the new London [Slow Motion Records]
01:23:30 - Plastic - slave to the beat (Mufti edit)[ ]
 01:27:37 - Highlite - HindieDance (Kendal Remix) [JEAHMON! Records]
 01:31:50 - Sex kino - We have ways of making you dance [HEARec]
 01:38:57- Gigi slugostino - Coffin DANCE (Italo disco remix)[Slugs On Drugs]
*****************
Model & Performers: 
Thilda: https://instagram.com/thildasbeinhaus 
Steve: https://instagram.com/yosoystev 
Missfortune: https://instagram.com/missfortunelondon 
Silke Furious: https://instagram.com/silke.furious 
cherrylips: https://instagram.com/cherrylips.cherry 
devil.s.daughter: https://instagram.com/devil.s.daughter 
ashleyagony: https://instagram.com/ashleyagony_
baphowitch: https://instagram.com/baphowitch 
zedkielzuastegui: https://instagram.com/zedkielzuastegui 
dark_rubberella: https://instagram.com/dark_rubberella 
Vortex: https://instagram.com/vortex_abrax 
Dvonboudoir: https://instagram.com/dvonboudoir 
Carolbonarde: https://instagram.com/carolbonarde
Camillespectrae: https://instagram.com/camillespectrae 
Sham.shadows: https://instagram.com/sham.shaddows
Jorjamoura: https://www.instagram.com/jorjamoura/
zia_angy: https://www.instagram.com/zia_angy_/
iseverythingclaire: https://www.instagram.com/iseverythin...
anarosapano: https://www.instagram.com/anarosapano/
icedancedame: https://www.instagram.com/icedancedame/
deu.tineurones: https://www.instagram.com/deu.tineuro...
catgirl_mona: https://www.instagram.com/catgirl_mona/
alejandra_lavrde: https://www.instagram.com/alejandra_l...
ioan.eterea: https://www.instagram.com/ioan.eterea/
teodor.bathory: https://www.instagram.com/teodor.bath...
motor.kitty: https://www.instagram.com/motor.kitty/

https://instagram.com/cinemueve
https://www.instagram.com/shotbytrip/
__
#DarkItalo #Sexwave #Yamispechie
";

		public double Duration => new TimeSpan(1, 44, 45).TotalSeconds;
		public bool ShouldSucceed => true;
		public IReadOnlyList<Chapter> ExpectedOutput => new List<Chapter>() {
			new Chapter(new TimeSpan(0,00,00), new TimeSpan(0,04,57), "Lera Foer & Gegen Mann - Precious Game [Maxima Culpa]"),
			new Chapter(new TimeSpan(0,04,57), new TimeSpan(0,09,56), "Fractions - Body Limit [FLEISCH]"),
			new Chapter(new TimeSpan(0,09,56), new TimeSpan(0,15,27), "Arnaud Rebotini -  What you want me to do[Mannequin Records]"),
			new Chapter(new TimeSpan(0,15,27), new TimeSpan(0,21,47), "Puritan - Whitesnake [Murder]"),
			new Chapter(new TimeSpan(0,21,47), new TimeSpan(0,25,47), "HOF - The Return [Fe Chrome]"),
			new Chapter(new TimeSpan(0,25,47), new TimeSpan(0,28,50), "Rougue - Nuclear [Detriti Records]"),
			new Chapter(new TimeSpan(0,28,50), new TimeSpan(0,32,25), "Lbeeze - Boogyman [Idlestates Recordings]"),
			new Chapter(new TimeSpan(0,32,25), new TimeSpan(0,38,28), "Mulfti - Fields of Passion [nothingisrealrecords]"),
			new Chapter(new TimeSpan(0,38,28), new TimeSpan(0,41,18), "Zack Zack Zack - Butun [Rennweg Records]"),
			new Chapter(new TimeSpan(0,41,18), new TimeSpan(0,45,25), "11 - T.G.T.B. - Derelict [Detriti Records]"),
			new Chapter(new TimeSpan(0,45,25), new TimeSpan(0,48,55), "Balvanera - Medium [DKA Records]"),
			new Chapter(new TimeSpan(0,48,55), new TimeSpan(0,53,38), "Minuit Machine - Don't Run From The Fire [Synth Religion]"),
			new Chapter(new TimeSpan(0,53,38), new TimeSpan(0,57,30), "Mind/Matter - Feture [ ]"),
			new Chapter(new TimeSpan(0,57,30), new TimeSpan(1,03,02), "Lera Foer - Open Eyes [Oberwave Records]"),
			new Chapter(new TimeSpan(1,03,02), new TimeSpan(1,07,20), "Rougue - Potential Danger [Detriti Records]"),
			new Chapter(new TimeSpan(1,07,20), new TimeSpan(1,11,15), "Ton Globiter - Hysteria Malmeria [Slow Motion Records]"),
			new Chapter(new TimeSpan(1,11,15), new TimeSpan(1,14,14), "Garlen - Black out dats [ ]"),
			new Chapter(new TimeSpan(1,14,14), new TimeSpan(1,17,49), "Time Modern - Mantel Der nacho (Alien Elements) [BOY Records ]"),
			new Chapter(new TimeSpan(1,17,49), new TimeSpan(1,20,40), "Bit-Max - Dance (Just Dance for me) [Discomagic Records]"),
			new Chapter(new TimeSpan(1,20,40), new TimeSpan(1,23,30), "Ton Globiter - Kaunas in the new London [Slow Motion Records]"),
			new Chapter(new TimeSpan(1,23,30), new TimeSpan(1,27,37), "Plastic - slave to the beat (Mufti edit)[ ]"),
			new Chapter(new TimeSpan(1,27,37), new TimeSpan(1,31,50), "Highlite - HindieDance (Kendal Remix) [JEAHMON! Records]"),
			new Chapter(new TimeSpan(1,31,50), new TimeSpan(1,38,57), "Sex kino - We have ways of making you dance [HEARec]"),
			new Chapter(new TimeSpan(1,38,57), new TimeSpan(1,44,45), "Gigi slugostino - Coffin DANCE (Italo disco remix)[Slugs On Drugs]")
		};
	}
}
