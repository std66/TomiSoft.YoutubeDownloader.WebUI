using System;

namespace TomiSoft.YoutubeDownloader.BusinessLogic.Data {
    public interface IPersistedServiceStatusDataManager {
        DateTime LastUpdate { get; set; }
    }
}