using System;

namespace TomiSoft.Common.SystemClock {
	/// <summary>
	/// Represents the system clock.
	/// </summary>
	public interface ISystemClock {
		DateTime UtcNow { get; }
	}
}