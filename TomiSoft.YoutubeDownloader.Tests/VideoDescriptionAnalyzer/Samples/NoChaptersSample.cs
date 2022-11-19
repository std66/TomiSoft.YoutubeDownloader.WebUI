using System;
using System.Collections.Generic;
using TomiSoft.YoutubeDownloader.VideoDescriptionAnalyzer;

namespace TomiSoft.YoutubeDownloader.Tests.VideoDescriptionAnalyzer.Samples {
	internal class NoChaptersSample : IChapterExtractTestSample {
		public string Description => @"◊ Title: 死奏憐音、玲瓏ノ終 (Symphony of Death by the Sound of Transmigrating Souls, The End of Clear Bells' Ringing)
◊ Arranger: 黒鳥 (Kokuchou)
◊ Lyrics: いずみん (Izumin)
◊ Vocals: nayuta
◊ Album: Scattered Destiny
◊ Circle: EastNewSound
◊ Release date: March 8th, 2009 (Reitaisai 6)
◊ Website: http://e-ns.net/discography/ens0002.html
◊ Original: 幽雅に咲かせ、墨染の桜 ～ Border of Life ／ Bloom Nobly, Cherry Blossoms of Sumizome ~ Border of Life
「東方妖々夢 ～ Perfect Cherry Blossom, Yuyuko Saigyouji's theme」
◊ Picture artist: T-RAy, Ayakohi
http://ayakohi.deviantart.com/art/Req...
http://www.pixiv.net/member_illust.ph...
◊ Translation: kafka-fuura
http://kafkafuura.wordpress.com/

Русский перевод: kir2yar, olenuu
";

		public double Duration => 1.0;
		public bool ShouldSucceed => false;
		public IReadOnlyList<Chapter> ExpectedOutput => Array.Empty<Chapter>();
	}
}
