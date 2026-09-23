# Copilot instructions

Read `AGENTS.md` before making changes. It is the canonical repository guide
for architecture, contracts, security constraints, workflow, and validation.

For ordinary code changes:

- Preserve the HTTP routes, `/downloadHub`, hub methods, SignalR event names,
  JSON property names, configuration keys, GUID-based file naming, and
  `LastUpdate.DateTime.txt`.
- Reuse the repository's process, filesystem, queue, downloader, and notifier
  abstractions. Unit tests must not invoke a real yt-dlp process.
- Treat the unauthenticated URL, SignalR enqueue, file-download, and optional
  metrics surfaces as security-sensitive. Do not broaden accepted URLs,
  protocols, formats, destinations, or external process arguments without
  explicit validation and threat-model review.
- Remember that download state is process-local and ordinary collections are
  not a guarantee of safe concurrent access.
- Update the configuration schema and documentation together when configuration
  changes.
- Inspect affected callers, tests, public contracts, and deployment files
  before editing; review the diff afterward.

Use the validation commands and specialized Docker/CI guidance in `AGENTS.md`.
