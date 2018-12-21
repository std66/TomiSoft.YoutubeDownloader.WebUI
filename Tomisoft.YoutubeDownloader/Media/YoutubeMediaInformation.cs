﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tomisoft.YoutubeDownloader.Media {
    public class YoutubeMediaInformation : MediaInformation {
        
        [JsonProperty("tags")]
        public string[] Tags { get; set; }
        
        [JsonProperty("like_count")]
        public int Likes { get; set; }

        [JsonProperty("dislike_count")]
        public int Dislikes { get; set; }
        
    }
}