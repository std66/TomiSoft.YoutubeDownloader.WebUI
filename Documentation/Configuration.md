# Configuration

This documentation describes the features supported by TomiSoft.YouTubeDownloader.WebUI and provides guidelines on configuring each parameter to fit your needs.

## Configuration files

The application has the following configuration files:

Path                    | Requirement | Role
------------------------|-------------|------
appsettings.json        | Required    | The main configuration file with all the parameters with default values.
nlog.config             | Required    | Determines how the application should output its logs. See https://nlog-project.org/config/
config/appsettings.json | Optional    | Same as appsettings.json. Can be used with Docker Volume Mount and Kubernetes ConfigMap. If this file exists, it will override the specified settings defined in appsettings.json.
config/nlog.config      | Optional    | Same as nlog.config. Can be used with Docker Volume Mount and Kubernetes ConfigMap. If this file exists, the application will use this file instead of nlog.config as logging configuration.

### Configuring with Docker Volume Mount and Kubernetes ConfigMap

It is possible to inject configuration from external sources provided as volume mount in the `config/` folder.

#### appsettings.json

For `appsettings.json`, you need to provide only those values in `config/appsettings.json` that you want to override. You still need to follow the layout of the original appsettings.json. This example overrides the download files' location to an external network block storage and the metric port to 5000.

``` json
{
    "YoutubeConfiguration": {
        "DownloadPath": "/mnt/storage/youtube-downloads"
    },
    "Metrics": {
        "Port": 5000
    }
}
```

**Verification:** The following log messages are logged by TomiSoft.YouTubeDownloader.WebUI:
```
new ConfigMapFileProviderChangeToken for appsettings.json
Checking for changes in /app/config/appsettings.json
```

#### nlog.config

For `nlog.config`, the `config/nlog.config` must be a valid NLog configuration file.

**Verification:** The following log message is logged by TomiSoft.YouTubeDownloader.WebUI:
```
NLog is configured using '"config/nlog.config"'
```

#### Kubernetes ConfigMap

An example Kubernetes ConfigMap may look like this:

``` yaml
apiVersion: v1
kind: ConfigMap
metadata:
  name: ytdl-configmap
data:
  appsettings.json: |
    {
        "YoutubeConfiguration": {
            "DownloadPath": "/mnt/storage/youtube-downloads"
        },
        "Metrics": {
            "Port": 5000
        }
    }
  nlog.config: |
    <?xml version="1.0" encoding="utf-8" ?>
    <nlog ...>

    </nlog>
```

## Configuration parameters

For actual documentation, refer the `Documentation/appsettings.schema.json` JSON Schema.

## Configuration hot reload support

The TomiSoft.YouTubeDownloader.WebUI application does not support applying configuration changes at runtime. Restarting the application is necessary for changes to take effect.