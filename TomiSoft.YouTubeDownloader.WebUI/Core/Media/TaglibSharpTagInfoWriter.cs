using System;
using TomiSoft.YoutubeDownloader.Media;

namespace TomiSoft.YouTubeDownloader.WebUI.Core.Media {
    public class TaglibSharpTagInfoWriter : ITagInfoWriter {
        public bool Write(string path, IIdentifiedSong songMetadata) {
            TagLib.File f = TagLib.File.Create(path);
            f.Tag.Title = songMetadata.TrackTitle;
            f.Tag.AlbumArtists = new string[] { songMetadata.Artist };

            try {
                f.Save();
            }
            catch (Exception e) {
                return false;
            }

            return true;
        }
    }
}
