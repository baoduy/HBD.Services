# Introduction 
The extension services for Mef.

### 1. HBD.ConfigurationService.
The service allows loading the configuration from various sources. The configuration will cache automatically to speed up the performance and avoid to reload the config from source frequently.
However, when caching is expired, or the is a new version of config ready. The configuration will be re-load automatically.