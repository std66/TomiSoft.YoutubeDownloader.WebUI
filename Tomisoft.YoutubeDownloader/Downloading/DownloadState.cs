namespace TomiSoft.YoutubeDownloader.Downloading {
	public enum DownloadState {
		WaitingForStart, Starting, Downloading, PostProcessing, AuthenticationRequired, Completed, Failed
	}
}