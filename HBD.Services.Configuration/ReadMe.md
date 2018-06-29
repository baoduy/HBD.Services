## HBD.Configuration
This is a service that allows loading the configuration from various sources.
The ConfigurationService will cache the configuration in the Cache service for the subsequence use.
Once the source config has changed, or the cache expired then the service will call the Adapter again to reload the latest version of config.

### 1. Create custom Adapter
Each ConfigAdapter will serve a particular Config Item as sample code below to create a config item and config adapter.

```csharp
//Config Item
class TestItem
{
    public string Name { get; set; }
    public string Email { get; set; }
}

//Config Adapter for config item above
//Just simply provide the Json path file to the base class (JsonConfigAdapter)
class TestJsonConfigAdapter : JsonConfigAdapter<TestItem>
{
    public TestJsonConfigAdapter() : base("TestData\\json1.json")
    {
    }
}
```

As the config item will be cached in the CacheService so if you want to manage the caching duration, then override the Expiration property and provide the duration.

```csharp
 /// <summary>
 /// Caching in 1 hours.
 /// </summary>
 public override TimeSpan? Expiration => new TimeSpan(1, 0, 0);
```



> However, If you want to create an Adapter for the other config source instead of Json file you should implement the IConfigAdapter<out TConfig> instead.

### 2. Config File Finder
There is a helper class had been provided allow to searching the config file in the Application Folder or the specific one at runtime. So that you place your config file in any folders in the application root, just ensure that the file name is identical to other files.

- Searching in a particular folder.

```csharp
//Find the configFile.json in the Configuration folder includes sub folders.
var finder = new FileFinder().Find("configFile.json").In("Configuration");

//Pass the finder to the Adapter
//FileNotFoundException will be threw if file is not found.
var adapter = new JsonConfigAdapter<TestItem>(finder);
```
- Searching config file in Application folder (AppDomain.Current.BaseDirectory)

```csharp
//Find the configFile.json in the Configuration folder includes sub folders.
var finder = new FileFinder().Find("configFile.json");

//Pass the finder to the Adapter
//FileNotFoundException will be threw if file is not found.
var adapter = new JsonConfigAdapter<TestItem>(finder);
```

### 3. Export an Adapter to Mef

Using Export attribute to export an adapter to Mef. The ConfigurationService will scan all exported adapters automatically.

- Exporting on .Net Framework 

```csharp
[Export(typeof(IConfigAdapter)),PartCreationPolicy(CreationPolicy.Shared)]
class TestJsonConfigAdapter : JsonConfigAdapter<TestItem>{...}
```

- Exporting on .Net Core

```csharp
[Export(typeof(IConfigAdapter)),Shared]
class TestJsonConfigAdapter : JsonConfigAdapter<TestItem>{...}
```


### 4. Manual Configuration
However, in case you are not using Mef and want to initialize the ConfigurationService manually then using below code snap.

```csharp
 var service = new ConfigurationServiceBuilder()
                .WithAdapters(new TestConfigAdapter())
                .WithCacheProvider(new MemoryCacheProvider())
                .WithServiceLocator(HBD.ServiceLocator.Current)
                .Build();

var t = service.Get<TestItem>();
t.Should().NotBeNull();
```

By default, the Configuration manager supports loading the XML and Json configuration without creating adapters instead just simply register your config type and file location into ConfigurationServiceBuilder.
```csharp
  var config = new ConfigurationServiceBuilder()
                 .RegisterFile<ConfigItem1>(new FileFinder().Find("Config.xml"))
                 .RegisterFile<ConfigItem2>("Config.json")
                 .Build();
```

In case you don't want the ConfigurationService to load the adapters from ServiceLocator you can ignore it from the ConfigurationServiceBuilder.
This feature also available for Caching mechanism.
```csharp
   var service = new ConfigurationServiceBuilder()
                 .RegisterFile<TestItem>("TestData\\json1.json")
                 .IgnoreCaching()
                 .IgnoreServiceLocator()
                 .Build();
```