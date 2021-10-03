using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Dynamic;

namespace TomiSoft.YoutubeDownloader.Media {
    internal class MediaInformationFactory {
        internal static IMediaInformation Create(string StdOut) {
            MediaInformation GenericMediaInformation = JsonConvert.DeserializeObject<MediaInformation>(StdOut);

            switch (GenericMediaInformation.Extractor) {
                case "youtube":
                    return CreateYoutubeMediaInformation(StdOut);

                default:
                    return GenericMediaInformation;
            }
        }

        private static IMediaInformation CreateYoutubeMediaInformation(string StdOut) {
            IDictionary<string, object> data = JsonConvert.DeserializeObject<ExpandoObject>(StdOut, new ExpandoObjectConverter());

            bool containsIdentifiedSong =
                data.ContainsKey("track") && !string.IsNullOrEmpty(data["track"] as string) &&
                data.ContainsKey("artist") && !string.IsNullOrEmpty(data["artist"] as string);

            if (containsIdentifiedSong) {
                return JsonConvert.DeserializeObject<YoutubeMediaInformationWithIdentifiedSong>(StdOut);
            }
            else {
                return JsonConvert.DeserializeObject<YoutubeMediaInformation>(StdOut);
            }
        }
    }
}
