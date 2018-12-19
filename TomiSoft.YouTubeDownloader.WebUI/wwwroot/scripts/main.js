var pages = [];
var downloadId;
var downloadProgressInterval;

$(document).ready(function () {
    pages["loaderPage"] = $("#loader");
    pages["startPage"] = $("#mediaUriInputBox");
    pages["statusPage"] = $("#downloadStatus");
});

function initDownload() {
    var mediaUri = $("#mediaUri").val();
    var targetFormat = $("[name=targetFormat]:checked").val();

    if (targetFormat === 'video') {
        showMessage("Downloading video is not supported yet.");
        return;
    }

    getMediaInformation(mediaUri).done(function () {
        enqueueDownload(mediaUri).done(function () {
            downloadProgressInterval = setInterval(checkDownloadProgress, 1000);
        });
    });
}

function enqueueDownload(mediaUri) {
    return $.getJSON(
        'downloader/enqueueDownload',
        {
            MediaUri: mediaUri
        },
        function (result) {
            downloadId = result.downloadId
        }
    );
}

function getMediaInformation(mediaUri) {
    showPage("loaderPage");

    return $.getJSON(
        'downloader/getMediaInformation',
        {
            MediaUri: mediaUri
        },
        function (result) {
            setProgress(0);

            showDownloadStatusPage(
                result.title,
                result.description,
                result.webpage_url,
                result.thumbnail
            );
        }
    ).fail(function (jqXHR) {
        showPage("startPage");

        switch (jqXHR.status) {
            case 400:
                showMessage(jqXHR.responseJSON.message);
                break;

            case 500:
                showMessage("Sorry, we have absolutely no idea what happened :(");
                break;
        }
    });
}

function showPage(pageName) {
    for (var pageKey in pages) {
        if (pageKey == pageName)
            pages[pageKey].show();
        else
            pages[pageKey].hide();
    }
}

function showDownloadStatusPage(videoTitle, videoDescription, videoUrl, videoImage) {
    $("#video-title").text(videoTitle);
    $("#video-description").text(videoDescription);
    $("#video-url").text(videoUrl);
    $("#video-thumbnail").attr('src', videoImage);

    showPage("statusPage");
} 

function showMessage(message) {
    $("#modal-message").text(message);
    $('#myModal').modal();
}

function setProgress(percentage) {
    var progressbar = $("#progressbar");
    progressbar.css("width", percentage + "%");
    progressbar.attr("aria-valuenow", percentage);
}

function checkDownloadProgress() {
    $.getJSON(
        'downloader/getProgress',
        {
            "downloadId": downloadId
        },
        function (result) {
            $("#downloadStatusText").text(result.downloadStatus);

            if (result.downloadStatus == "Completed") {
                clearInterval(downloadProgressInterval);
                setProgress(100);
                showPage("startPage");
                $("#downloadStatusText").text("We are doing nothing");

                window.location = 'downloader/downloadFile?downloadId=' + downloadId;
            }
            else if (result.downloadStatus == "Downloading") {
                setProgress(result.percent);
            }
            else if (result.downloadStatus == "PostProcessing") {
                setProgress(100);
            }
            else if (result.downloadStatus == "Failed") {
                showMessage("Sorry, we couldn't download it :(");
            }
        }
    ).fail(function (jqXHR) {
        showPage("startPage");

        if (jqXHR.status == 500) {
            showMessage("Sorry, we have absolutely no idea what happened :(");
        }
        else {
            showMessage(jqXHR.responseJSON.message);
        }
    });
}