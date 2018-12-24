using Microsoft.Extensions.Logging;
using System;

namespace TomiSoft.YoutubeDownloader.WebUI.Tests.Mocks {
    class NullLoggerMock<T> : ILogger<T> {
        private class Disposable : IDisposable {
            public void Dispose() {
                
            }
        }

        public IDisposable BeginScope<TState>(TState state) {
            return new Disposable();
        }

        public bool IsEnabled(LogLevel logLevel) {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) {
            
        }
    }
}
