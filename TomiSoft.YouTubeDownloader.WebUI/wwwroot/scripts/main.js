var vm = null;

class ViewModel {
    constructor(signalRConnection, pageManager) {
        this.signalR = signalRConnection;
        this.pageManager = pageManager;
        this.downloadId = null;

        this.signalR.on("UseDownloadId", (x) => this.downloadId = x);
        this.signalR.on("UpdateDownloadStatus", (x) => this.UpdateDownloadStatus(x));
    }

    async EnqueueDownload(mediaUri, mediaFormat) {
        await this.signalR
            .invoke("EnqueueDownload", mediaUri, mediaFormat)
            .catch(
                function (err) {
                    return console.error(err);
                }
            );
    }

    UpdateDownloadStatus(result) {
        $("#downloadStatusText").text(result.downloadStatus);

        if (result.downloadStatus == "Completed") {
            this.SetProgress(100);
            this.pageManager.ShowStartPage();
            $("#downloadStatusText").text("We are doing nothing");

            window.location = 'downloader/downloadFile?downloadId=' + this.downloadId;
        }
        else if (result.downloadStatus == "Downloading") {
            this.SetProgress(result.percent);
        }
        else if (result.downloadStatus == "PostProcessing") {
            this.SetProgress(100);
        }
        else if (result.downloadStatus == "Failed") {
            this.ShowMessage("Sorry, we couldn't download it :(");
        }
    }

    async GetMediaInformation(mediaUri) {
        this.pageManager.ShowLoaderPage();

        let result = null;

        try {
            result = await $.getJSON(
                'downloader/getMediaInformation',
                {
                    MediaUri: mediaUri
                }
            );
        }
        catch (jqXHR) {
            this.pageManager.ShowStartPage();

            switch (jqXHR.status) {
                case 400:
                    this.ShowMessage(jqXHR.responseJSON.message);
                    break;

                case 500:
                    this.ShowMessage("Sorry, we have absolutely no idea what happened :(");
                    break;
            }

            return false;
        }

        this.SetProgress(0);

        this.pageManager.ShowStatusPage(
            result.title,
            result.description,
            result.webpage_url,
            result.thumbnail
        );

        return true;
    }

    //utility
    SetProgress(percentage) {
        var progressbar = $("#progressbar");
        progressbar.css("width", percentage + "%");
        progressbar.attr("aria-valuenow", percentage);
    }

    //utility
    ShowMessage(message) {
        $("#modal-message").text(message);
        $('#myModal').modal();
    }
}

class PageManager {
    constructor() {
        this.pages = [];

        this.pages["startPage"] = $("#mediaUriInputBox");
        this.pages["statusPage"] = $("#downloadStatus");
        this.pages["loaderPage"] = $("#loader");
    }

    ShowLoaderPage() {
        this.showPage("loaderPage");
    }

    ShowStatusPage(videoTitle, videoDescription, videoUrl, videoImage) {
        $("#video-title").text(videoTitle);
        $("#video-description").text(videoDescription);
        $("#video-url").text(videoUrl);
        $("#video-thumbnail").attr('src', videoImage);

        this.showPage("statusPage");
    }

    ShowStartPage() {
        this.showPage("startPage");
    }

    showPage(pageName) {
        for (var pageKey in this.pages) {
            if (pageKey == pageName)
                this.pages[pageKey].show();
            else
                this.pages[pageKey].hide();
        }
    }
}

$(document).ready(function () {
    var connection = new signalR.HubConnectionBuilder().withUrl("/downloadHub").build();
    connection.start();

    vm = new ViewModel(connection, new PageManager());
});

async function initDownload() {
    var mediaUri = $("#mediaUri").val();
    var targetFormat = $("[name=targetFormat]:checked").val();
    
    if (await vm.GetMediaInformation(mediaUri)) {
        await vm.EnqueueDownload(mediaUri, targetFormat);
    }
}