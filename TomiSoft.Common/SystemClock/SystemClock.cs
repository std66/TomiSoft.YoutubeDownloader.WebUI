using System;

namespace TomiSoft.Common.SystemClock {
	public class SystemClock : ISystemClock {
		public DateTime UtcNow => DateTime.UtcNow;
	}
}