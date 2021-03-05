using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using TomiSoft.YouTubeDownloader.WebUI.Client.Shared;

namespace TomiSoft.YouTubeDownloader.WebUI.Client.Pages {
    public partial class Index {
        [Parameter]
        public string Url { get; set; }
        
        [Parameter]
        public int? ProgressValue { get; set; }

        [Parameter]
        public EventCallback<int?> ProgressValueChanged { get; set; }

        public SwitchablePages PageContainer { get; set; }

        public async Task BeginDownload() {
            PageContainer.ActivatePage("Loader");
            await Task.Delay(1000);
            PageContainer.ActivatePage("DownloadStatus");

            for (int i = 0; i <= 100; i++) {
                await Task.Delay(50);
                ProgressValue = i;
                await ProgressValueChanged.InvokeAsync(ProgressValue);
                
            }

            PageContainer.ActivatePage("Index");
        }
    }
}
