using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TomiSoft.YouTubeDownloader.WebUI.Client.Models;

namespace TomiSoft.YouTubeDownloader.WebUI.Client.Services {
    public class BackendService {
        private readonly HttpClient client;

        public BackendService(HttpClient client) {
            this.client = client;
        }

        public async Task<AppVersionInfo> GetAppVersionAsync() {
            try {
                return await client.GetFromJsonAsync<AppVersionInfo>("/Home/Version");
            }
            catch {
                return new AppVersionInfo() {
                    Version = new Version(1, 0, 0),
                    BuildTime = DateTimeOffset.UtcNow,
                };
            }
        }
    }
}
