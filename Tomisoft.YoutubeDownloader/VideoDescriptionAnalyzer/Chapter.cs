using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TomiSoft.YoutubeDownloader.VideoDescriptionAnalyzer {
	[DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
	public class Chapter : IEquatable<Chapter> {
		public Chapter(TimeSpan startTime, TimeSpan endTime, string title) {
			StartTime = startTime;
			EndTime = endTime;
			Title = title;
		}

		public TimeSpan StartTime { get; }
		public TimeSpan EndTime { get; }
		public string Title { get; }

		public override bool Equals(object obj) {
			return Equals(obj as Chapter);
		}

		public bool Equals(Chapter other) {
			return other != null &&
				   StartTime.Equals(other.StartTime) &&
				   EndTime.Equals(other.EndTime) &&
				   Title == other.Title;
		}

		public override int GetHashCode() {
			int hashCode = 971866234;
			hashCode = hashCode * -1521134295 + StartTime.GetHashCode();
			hashCode = hashCode * -1521134295 + EndTime.GetHashCode();
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Title);
			return hashCode;
		}

		public override string ToString() {
			return $"{StartTime} - {EndTime}: {Title}";
		}

		private string GetDebuggerDisplay() {
			return ToString();
		}

		public static bool operator ==(Chapter left, Chapter right) {
			return EqualityComparer<Chapter>.Default.Equals(left, right);
		}

		public static bool operator !=(Chapter left, Chapter right) {
			return !(left == right);
		}
	}
}