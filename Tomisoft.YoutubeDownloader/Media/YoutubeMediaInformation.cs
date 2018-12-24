using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TomiSoft.YoutubeDownloader.Media {
    public class YoutubeMediaInformation : MediaInformation, ILikeableMedia {
        
        [JsonProperty("tags")]
        public string[] Tags { get; set; }
        
        [JsonProperty("like_count")]
        public int Likes { get; set; }

        [JsonProperty("dislike_count")]
        public int Dislikes { get; set; }
        
    }
}
