# AI development guide

## What this application does

TomiSoft YouTube Downloader is an ASP.NET Core web application that accepts a
media URL, queries `yt-dlp`/`youtube-dl` for metadata, queues a video or MP3
download, reports progress over SignalR, and serves the completed file.

The current web application targets `net10.0`. The reusable downloader and
common libraries target `netstandard2.0`; business logic targets
`netstandard2.1`.

## Repository structure and architecture

- `TomiSoft.YouTubeDownloader.WebUI/` is the composition root and web host.
  `Program.cs` configures the host, NLog, and configuration sources.
  `Startup.cs` registers services and configures MVC, SignalR, metrics, and
  hosted services.
- `TomiSoft.YoutubeDownloader.BusinessLogic/` contains queue consumption,
  download lifecycle management, maintenance, retention, and in-memory
  filename state.
- `Tomisoft.YoutubeDownloader/` contains the media-downloader abstraction,
  yt-dlp command construction, process integration, metadata models, download
  progress parsing, and chapter detection.
- `TomiSoft.Common/` contains shared queues, process abstractions, filesystem
  abstractions, system-clock abstractions, and ConfigMap file-provider support.
- `Views/Home/Index.cshtml` and
  `TomiSoft.YouTubeDownloader.WebUI/wwwroot/scripts/main.js` are the active
  frontend. `TomiSoft.YouTubeDownloader.WebUI.Client/` contains no active
  application source and should not be assumed to be the frontend.

The main runtime flow is:

```text
HTTP metadata request
  -> DownloaderController
  -> IMediaDownloader/YoutubeDl
  -> external yt-dlp process
  -> JSON media response

SignalR EnqueueDownload
  -> QueueService<DownloadRequestBM>
  -> BackgroundDownloaderService
  -> DownloadManagementService
  -> IDownload/YoutubeDl process
  -> SignalR status events
  -> HTTP file download
```

The application is stateful and stores queue, active/completed download
records, and filename mappings in process memory. Redis, when configured, is a
SignalR backplane only; it does not persist or distribute download state.

## Confirmed public contracts

Do not change these names, paths, payload shapes, or serialization behavior
without treating the change as a compatibility change:

- MVC routes:
  - `downloader/getMediaInformation?MediaUri=<uri>`
  - `downloader/downloadFile?DownloadId=<guid>`
  - `Home/Version` is the conventional route for
    `HomeController.VersionAsync`; it currently has no dedicated route or
    integration test.
- SignalR hub path: `/downloadHub`
- Hub methods:
  - `EnqueueDownload(string mediaUri, string mediaFormat)`
  - `RequestProgress(string downloadId)`
- Client events:
  - `UseDownloadId`
  - `UpdateDownloadStatus`
  - `MaintenanceStarted`
  - `MaintenanceCompleted`
- Download status payload: `UpdateDownloadStatus` with `DownloadStatus` and
  `Percent` properties. JavaScript consumes the default JSON names
  `downloadStatus` and `percent`.
- Media-information JSON fields consumed by the frontend include
  `mediaUri`, `title`, `duration`, `videoId`, `thumbnail`, `description`, and
  `chapters`. The current JavaScript UI uses `mediaUri`, `title`,
  `description`, and `thumbnail`; the other fields are still emitted by the
  response contract.
- Error responses contain `message` and `errorCode`.
- Configuration section and key names are defined by
  `TomiSoft.YouTubeDownloader.WebUI/appsettings*.json` and
  `Documentation/appsettings.schema.json`.

The SignalR client currently sends `video` or `mp3audio`. The hub treats only
the exact value `mp3audio` as MP3 and silently treats all other values as
video. Do not change these values or the fallback behavior without explicitly
changing and testing the client contract.

The generated download filename is a GUID plus the actual media extension.
The user-facing title is tracked separately in `MemoryFilenameDB`. The
maintenance timestamp file is `LastUpdate.DateTime.txt`.

After a completed file is successfully resolved by
`DownloadManagementService.TryGetDownloadedFile`, its completed-download
record is removed. File retrieval is therefore one-shot within the process
unless this behavior is intentionally changed.

The filename lifecycle is process-local: metadata lookup adds a URI/title
record, enqueue associates the first unassigned matching URI with a download
ID, and timer cleanup removes unassigned records after one minute and
assigned records after sixty minutes. This is not a durable or transactional
mapping.

## Runtime and deployment facts

Configuration expected by the shipped schema and default files:

- `YoutubeConfiguration:ExecutablePath`
- `YoutubeConfiguration:DownloadPath`
- `Metrics:Enabled`, `Metrics:Port`, `Metrics:Uri`
- `DataRetention:Enabled`,
  `DataRetention:DeleteFilesAfterMinutesElapsed`
- `AutoUpdate:Enabled`, `AutoUpdate:UpdateIntervalInHours`

The application binds these sections but does not register startup validation.
Missing or invalid values may therefore fail later while starting or using the
external process, filesystem, metrics, retention, or maintenance features.

Optional configuration:

- `ConnectionStrings:Redis` enables the SignalR Redis backplane.
- `config/appsettings.json` overrides matching settings when the mounted
  `config` directory exists.
- `config/nlog.config` replaces the default NLog configuration.

The Docker image uses .NET 10, installs ffmpeg, Python 3, and Deno, downloads
yt-dlp as `/app/youtube-dl/youtube-dl`, listens on port 80, and exposes the
Prometheus listener on port 9000. During the image build, the Linux
`appsettings-linux.json` and `nlog-linux.config` files are moved over the
standard configuration filenames before publish. The configured download
directory must be writable. Do not assume that container-local in-memory state
survives restart.

The repository contains both GitHub Actions CI and AppVeyor scripts. Their
canonical/active status is not established in the repository; do not remove or
alter either pipeline without human confirmation.

`TomiSoft.YouTubeDownloader.WebUI/AppVersion.cs` is generated or rewritten by
CI from `ContinousIntegration/Build/Templates/AppVersion.cs`. Do not treat
the generated commit/version values as durable hand-maintained source.

The host enables JSON `reloadOnChange`, while
`Documentation/Configuration.md` says configuration changes require a restart.
The intended production behavior is unresolved; do not change reload behavior
or its documentation casually.

## Rules for agents

1. Inspect the relevant project, interfaces, callers, tests, configuration,
   and documentation before editing.
2. Preserve project dependency direction. The web project composes lower
   layers; lower layers must not acquire web-specific dependencies casually.
3. Reuse existing abstractions such as `IMediaDownloader`, `IProcessFactory`,
   `IProcess`, `IFileManager`, `IFile`, `IQueueService<T>`, and notifier
   interfaces. Use their test doubles rather than invoking real external
   processes in unit tests.
4. When adding or changing configuration, update the JSON schema and
   configuration documentation together.
5. When changing an HTTP or SignalR contract, update the JavaScript client and
   tests in the same change, and explicitly assess backward compatibility.
6. Treat yt-dlp command arguments, stdout parsing regexes, generated file
   naming, and ffmpeg behavior as external-process contracts.
7. Do not introduce shared mutable state, new singleton state, or concurrency
   assumptions without reviewing the existing in-memory lifecycle and
   single-instance deployment model.
8. Treat the unauthenticated metadata, SignalR enqueue, and file-download
   surfaces as security-sensitive. User-provided URIs are passed to an
   external downloader; consider SSRF, unrestricted destinations/protocols,
   bandwidth/storage exhaustion, timeouts, and rate limiting before changing
   these paths. Changes to URL validation, downloader invocation, download destinations,
   file retrieval, or resource consumption are security-sensitive changes.
   
   Before implementing such a change, explicitly inspect and document:
   - accepted URL schemes/protocols
   - destination handling
   - downloader argument construction
   - SSRF implications
   - filesystem/path implications
   - bandwidth and storage exhaustion
   - timeout/cancellation behavior
   - rate limiting or abuse controls
   
   Do not claim that a change is secure merely because input validation was added.
   Identify what was actually validated and what remains outside the scope of the change.
9. Do not broaden accepted protocols, destinations, formats, or process
   arguments accidentally. Preserve the current `video`/`mp3audio` behavior
   unless the contract is intentionally changed.
10. Do not silently swallow new errors or add broad catches. Preserve explicit
    logging and user-visible error behavior. Existing behavior such as the
    empty catch in `HomeController.VersionAsync` is compatibility behavior and
    requires explicit scope before changing.
11. Avoid unrelated cleanup, dependency upgrades, formatting churn, or
    modernization of legacy CI unless requested.

## Workflow and validation

Before editing:

1. Check `git status --short`.
2. Read the applicable project file and nearby interfaces/callers.
3. Search for every public name, configuration key, route, event, and file
   format affected by the change.
4. Identify whether the change affects the web contract, external process
   boundary, filesystem state, concurrency, or deployment.

After editing:

1. Review the diff and confirm no unrelated source files changed.
2. On a clean checkout or after dependency changes, restore first:

   ```text
   dotnet restore
   ```

3. Build the solution:

   ```text
   dotnet build
   ```

4. Run the solution tests:

   ```text
   dotnet test --no-build --no-restore --logger "console;verbosity=minimal"
   ```

5. `TomiSoft.Common.Tests` is not included in the solution. Run it explicitly
   when common-library behavior changes, or when validating all repository
   tests:

   ```text
   dotnet test TomiSoft.Common.Tests/TomiSoft.Common.Tests.csproj
   ```

6. For Dockerfile or runtime-image changes, run the Docker build and inspect
   the published configuration and required external tools. For CI or
   deployment changes, validate the relevant workflow/script path in addition
   to the .NET build and tests.

## Known limitations and technical debt

These are current-system facts, not rules to preserve:

- Download queues, active/completed downloads, and filename mappings are
  process-local and are lost on restart.
- The stateful collections are ordinary lists used across asynchronous,
  timer, and process-event paths; concurrency behavior is not comprehensively
  tested.
- Redis does not make downloads horizontally scalable.
- The web test project has only a placeholder test; public HTTP and SignalR
  contracts lack comprehensive automated coverage.
- Cleanup and maintenance are polling hosted services.
- SignalR notification sends are fire-and-forget.
- Background-service exception behavior is configured to ignore exceptions.
- Cleanup and maintenance delay tasks suppress cancellation/fault completion
  through `ContinueWith(x => { })`.
- The Docker build downloads the latest yt-dlp artifact without pinning or
  checksum verification.
- The application has no visible authentication or authorization; metadata,
  SignalR enqueue, file retrieval, and enabled metrics are not protected by an
  authorization policy. `AllowedHosts` is `*`.
- `Program.cs` enables configuration reload-on-change, but the configuration
  documentation requires restart; the intended behavior is unresolved.
- Documentation contains version drift: older README text mentions .NET 6 and
  youtube-dl while the current project and Dockerfile use .NET 10 and yt-dlp.
- The current audit/build observed a NuGet warning for a transitive
  `Microsoft.Build` package; revalidate this warning during dependency
  changes rather than treating it as permanent.

If a change intentionally addresses one of these limitations, document the
new behavior and add tests for the affected lifecycle or contract.

## When to ask for human clarification

Do not make an architectural or compatibility decision silently when:

- the requested change conflicts with an existing public contract;
- the repository contains contradictory documentation or configuration behavior;
- the change requires choosing between multiple valid architectural approaches;
- a security boundary would change;
- persistent/distributed state would be introduced;
- deployment topology or horizontal scaling assumptions would change;
- a database or new infrastructure dependency would be introduced;
- an existing limitation in "Known limitations and technical debt" would be
  intentionally changed;
- a breaking HTTP, SignalR, configuration, filesystem, or CLI contract is
  required;
- the intended behavior cannot be established from the repository and task
  description.

In these cases:
1. explain the conflict or uncertainty;
2. identify the affected files/contracts;
3. propose the available options;
4. ask for clarification before implementing the irreversible or
   compatibility-sensitive change.
   
## Known limitations and technical debt

This section reflects the repository state as of 2026-09-24.
These are current-system observations, not rules to preserve.
They should be revalidated when the affected subsystem is changed.

## Completion report

When finishing a coding task, report:

1. Summary of changes.
2. Files changed.
3. Contracts/invariants affected.
4. Tests added or changed.
5. Validation commands executed and their results.
6. Validation that could not be executed and why.
7. Known remaining risks or uncertainties.
8. Any behavior intentionally left unchanged.