﻿using System.ComponentModel.DataAnnotations;

namespace TomiSoft.YouTubeDownloader.WebUI.Core {
	public enum ErrorCodes {
		[Display(Name = "Request succeeded")]
		Success = 0,

		[Display(Name = "Unknown error occurred")]
		GenericError = 1,

		[Display(Name = "Media URI is malformed")]
		MalformedMediaUri = 2,

		[Display(Name = "Media URI is empty")]
		MediaUriIsEmpty = 3,

		[Display(Name = "Download not found with the given ID")]
		DownloadNotFound = 4,

		[Display(Name = "Download ID is malformed")]
		InvalidDownloadId = 5,

		[Display(Name = "Download is not completed")]
		DownloadNotCompleted = 6,

		[Display(Name = "Live streams are not supported")]
		LiveStreamsAreNotSupported = 7,

		[Display(Name = "Downloading private media is not supported")]
		PrivateMediaIsNotSupported = 8,

		[Display(Name = "Downloading content that requires (paid) subscription is not supported")]
		RestrictedContentBySubscriptionPolicy = 9
	}
}